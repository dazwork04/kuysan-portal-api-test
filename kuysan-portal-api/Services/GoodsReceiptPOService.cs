using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Models;
using SLayerConnectionLib;
using B1SLayer;
using SAPB1SLayerWebAPI.Utils;

namespace SAPB1SLayerWebAPI.Services
{
    public class GoodsReceiptPOService
    {
        // GET GOODS RECEIPT POS
        //GetPurchaseOrders/{userId}/{companyDB}/{status}/{cancelled}/{dateFrom}/{dateTo}
        public async Task<Response> GetGoodsReceiptPOsAsync(int userId, string companyDB, char status, char cancelled, string dateFrom, string dateTo, Paginate paginate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = paginate.OrderBy[0].ToString().ToUpper() + paginate.OrderBy[1..];
                string queryFilter = $"DocumentStatus eq '{status}' and Cancelled eq '{cancelled}' and DocDate ge '{dateFrom}' and DocDate le '{dateTo}'" + paginate.Filter;


                var count = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes)
                    .Filter(queryFilter)
                    .GetCountAsync();

                var result = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes)
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

        // CREATE GOODS RECEIPT PO
        public async Task<Response> CreateGoodsReceiptPOAsync(int userId, string companyDB, Document goodsReceiptPO) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var result = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes).PostAsync<dynamic>(goodsReceiptPO);

                Logger.CreateLog(false, "CREATE GOODS RECEIPT PO", "SUCCESS", JsonConvert.SerializeObject(goodsReceiptPO));
                return new Response
                {
                    Status = "success",
                    Message = $"GRPO #{result.DocNum} CREATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {
                Logger.CreateLog(true, "CREATE GOODS RECEIPT PO", ex.Message, JsonConvert.SerializeObject(goodsReceiptPO));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // UPDATE GOODS RECEIPT PO
        public async Task<Response> UpdateGoodsReceiptPOAsync(int userId, string companyDB, dynamic goodsReceiptPO) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                await connection.Request(EntitiesKeys.PurchaseDeliveryNotes, goodsReceiptPO.DocEntry).PutAsync(goodsReceiptPO);


                var result = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes, goodsReceiptPO.DocEntry).GetAsync();
                Logger.CreateLog(false, "UPDATE GOODS RECEIPT PO", "SUCCESS", JsonConvert.SerializeObject(goodsReceiptPO));
                return new Response
                {
                    Status = "success",
                    Message = $"GRPO #{result.DocNum} UPDATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                Logger.CreateLog(true, "UPDATE GOODS RECEIPT PO", ex.Message, JsonConvert.SerializeObject(goodsReceiptPO));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // CANCEL PURCHASE ORDER
        public async Task<Response> CancelGoodsReceiptPOAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.PurchaseDeliveryNotes}({docEntry})/Cancel";

                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"GRPO #{result.DocNum} CANCELED successfully!",
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

        // CLOSE PURCHASE ORDER
        public async Task<Response> CloseGoodsReceiptPOAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.PurchaseDeliveryNotes}({docEntry})/Close";
                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"GRPO #{result.DocNum} CLOSED successfully!",
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

        // GET PURCHASE ORDERS
        public async Task<Response> GetPurchaseOrdersAsync(int userId, string companyDB, string cardCode, string docType, string priceMode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var purchaseOrders = await connection.Request(EntitiesKeys.PurchaseOrders)
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
    }
}
