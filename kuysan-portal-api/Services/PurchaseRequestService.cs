using B1SLayer;
using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Utils;
using SLayerConnectionLib;
using System.Reflection.Metadata;

namespace SAPB1SLayerWebAPI.Services
{
    public class PurchaseRequestService
    {
        // GET PURCHASE REQUESTS
        public async Task<Response> GetPurchaseRequestsAsync(int userId, string companyDB, char status, char cancelled, string dateFrom, string dateTo, Paginate paginate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = paginate.OrderBy[0].ToString().ToUpper() + paginate.OrderBy[1..];
                string queryFilter = $"DocumentStatus eq '{status}' and Cancelled eq '{cancelled}' and DocDate ge '{dateFrom}' and DocDate le '{dateTo}'" + paginate.Filter;


                var count = await connection.Request(EntitiesKeys.PurchaseRequests)
                    .Filter(queryFilter)
                    .GetCountAsync();

                var result = await connection.Request(EntitiesKeys.PurchaseRequests)
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

        // CREATE PURCHASE REQUEST
        public async Task<Response> CreatePurchaseRequestAsync(int userId, string companyDB, char forApproval, dynamic purchaseRequest) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.PurchaseRequests).PostAsync<dynamic>(purchaseRequest);

                Logger.CreateLog(false, "CREATE PURCHASE REQUEST", "SUCCESS", JsonConvert.SerializeObject(purchaseRequest));

                return new Response
                {
                    Status = "success",
                    Message = $"PR #{result.DocNum} CREATED successfully!",
                    Payload = result
                };

            }
            catch (Exception ex)
            {

                if (ex.Message.ToLower().Contains("no matching record"))
                {
                    if (forApproval == 'Y')
                    {
                        Logger.CreateLog(false, "CREATE PURCHASE REQUEST APPROVAL", "SUCCESS", JsonConvert.SerializeObject(purchaseRequest));
                        return new Response
                        {
                            Status = "success",
                            Message = $"PR For Approval CREATED successfully!",
                        };
                    }
                    else
                    {
                        Logger.CreateLog(true, "CREATE PURCHASE REQUEST", ex.Message, JsonConvert.SerializeObject(purchaseRequest));
                        return new Response
                        {
                            Status = "failed",
                            Message = ex.Message
                        };
                    }
                }
                Logger.CreateLog(true, "CREATE PURCHAE REQUEST", ex.Message, JsonConvert.SerializeObject(purchaseRequest));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // UPDATE PURCHASE REQUEST
        public async Task<Response> UpdatePurchaseRequestAsync(int userId, string companyDB, dynamic purchaseRequest) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                await connection.Request(EntitiesKeys.PurchaseRequests, purchaseRequest.DocEntry).PutAsync(purchaseRequest);


                var result = await connection.Request(EntitiesKeys.PurchaseRequests, purchaseRequest.DocEntry).GetAsync();
                Logger.CreateLog(false, "UPDATE PURCHASE REQUEST", "SUCCESS", JsonConvert.SerializeObject(purchaseRequest));
                return new Response
                {
                    Status = "success",
                    Message = $"PR #{result.DocNum} UPDATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                Logger.CreateLog(true, "UPDATE PURCHASE REQUEST", ex.Message, JsonConvert.SerializeObject(purchaseRequest));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // CANCEL PURCHASE REQUEST
        public async Task<Response> CancelPurchaseRequestAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.PurchaseRequests}({docEntry})/Cancel";

                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.PurchaseRequests, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"PR #{result.DocNum} CANCELED successfully!",
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

        // CLOSE PURCHASE REQUEST
        public async Task<Response> ClosePurchaseRequestAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.PurchaseRequests}({docEntry})/Close";
                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.PurchaseRequests, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"PR #{result.DocNum} CLOSED successfully!",
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

    }
}
