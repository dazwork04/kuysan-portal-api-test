using B1SLayer;
using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Utils;
using SLayerConnectionLib;

namespace SARB1SLayerWebARI.Services
{
    public class ARInvoiceService
    {
        // GET AR INVOICES
        public async Task<Response> GetARInvoicesAsync(int userId, string companyDB, char status, char cancelled, string dateFrom, string dateTo, Paginate paginate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = paginate.OrderBy[0].ToString().ToUpper() + paginate.OrderBy[1..];
                string queryFilter = $"DocumentStatus eq '{status}' and Cancelled eq '{cancelled}' and DocDate ge '{dateFrom}' and DocDate le '{dateTo}'" + paginate.Filter;


                var count = await connection.Request(EntitiesKeys.Invoices)
                    .Filter(queryFilter)
                    .GetCountAsync();

                var result = await connection.Request(EntitiesKeys.Invoices)
                    .Filter(queryFilter)
                    .Skip(paginate.Page * paginate.Size)
                    .Top(paginate.Size)
                    .OrderBy($"{orderBy} {paginate.Direction}")
                    .GetAsync<List<DocumentList>>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Count = count,
                        Data = result
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                    Payload = new List<dynamic>()
                };
            }
        });

        // CREATE AR INVOICE
        public async Task<Response> CreateARInvoiceAsync(int userId, string companyDB, char forApproval, Document apInvoice) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                await connection.Request(EntitiesKeys.Invoices).PostAsync(apInvoice);

                var result = await connection.Request(EntitiesKeys.Invoices).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                var newAR = result.First();

                Logger.CreateLog(false, "CREATE AR INVOICE", "SUCCESS", JsonConvert.SerializeObject(apInvoice));
                return new Response
                {
                    Status = "success",
                    Message = $"AR #{newAR.DocNum} CREATED successfully!",
                    Payload = newAR
                };

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("no matching record"))
                {
                    if (forApproval == 'Y')
                    {
                        Logger.CreateLog(false, "CREATE AR INVOICE APPROVAL", "SUCCESS", JsonConvert.SerializeObject(apInvoice));
                        return new Response
                        {
                            Status = "success",
                            Message = $"AR For Approval CREATED successfully!",
                        };
                    }
                    else
                    {
                        Logger.CreateLog(true, "CREATE AR INVOICE", ex.Message, JsonConvert.SerializeObject(apInvoice));
                        return new Response
                        {
                            Status = "failed",
                            Message = ex.Message
                        };
                    }
                }
                Logger.CreateLog(true, "CREATE AR INVOICE", ex.Message, JsonConvert.SerializeObject(apInvoice));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
            //try
            //{
            //    var connection = Main.GetConnection(userId, companyDB);
            //    var result = await connection.Request(EntitiesKeys.Invoices).PostAsync<dynamic>(apInvoice);

            //    Logger.CreateLog(false, "CREATE AR INVOICE", "SUCCESS", JsonConvert.SerializeObject(apInvoice));
            //    return new Response
            //    {
            //        Status = "success",
            //        Message = $"ARI #{result.DocNum} CREATED successfully!",
            //        Payload = result
            //    };
            //}
            //catch (Exception ex)
            //{
            //    Logger.CreateLog(true, "CREATE AR INVOICE", ex.Message, JsonConvert.SerializeObject(apInvoice));
            //    return new Response
            //    {
            //        Status = "failed",
            //        Message = ex.Message
            //    };
            //}
        });

        // UPDATE AR INVOICE
        public async Task<Response> UpdateARInvoiceAsync(int userId, string companyDB, dynamic apInvoice) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                await connection.Request(EntitiesKeys.Invoices, apInvoice.DocEntry).PutAsync(apInvoice);

                var result = await connection.Request(EntitiesKeys.Invoices, apInvoice.DocEntry).GetAsync();
                Logger.CreateLog(false, "UPDATE AR INVOICE", "SUCCESS", JsonConvert.SerializeObject(apInvoice));
                return new Response
                {
                    Status = "success",
                    Message = $"ARI #{result.DocNum} UPDATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                Logger.CreateLog(true, "UPDATE AR INVOICE", ex.Message, JsonConvert.SerializeObject(apInvoice));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // CANCEL AR INVOICE
        public async Task<Response> CancelARInvoiceAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.Invoices}({docEntry})/Cancel";

                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.Invoices, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"ARI #{result.DocNum} CANCELED successfully!",
                    Payload = result,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // CLOSE AR INVOICE
        public async Task<Response> CloseARInvoiceAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.Invoices}({docEntry})/Close";
                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.Invoices, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"ARI #{result.DocNum} CLOSED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });


        // GET SALES ORDERS
        public async Task<Response> GetSalesOrdersAsync(int userId, string companyDB, string cardCode, string docType, string priceMode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var purchaseOrders = await connection.Request(EntitiesKeys.Orders)
                    .Filter($"CardCode eq '{cardCode}' and DocType eq '{docType}' and PriceMode eq '{priceMode}' and DocumentStatus eq 'O'")
                    .GetAllAsync<dynamic>();


                return new Response
                {
                    Status = "success",
                    Payload = purchaseOrders
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // GET DELIVERIES
        public async Task<Response> GetDeliveriesAsync(int userId, string companyDB, string cardCode, string docType, string priceMode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var goodsReceiptPOs = await connection.Request(EntitiesKeys.DeliveryNotes)
                    .Filter($"CardCode eq '{cardCode}' and DocType eq '{docType}' and PriceMode eq '{priceMode}' and DocumentStatus eq 'O'")
                    .GetAllAsync<dynamic>();


                return new Response
                {
                    Status = "success",
                    Payload = goodsReceiptPOs
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });
    }
}
