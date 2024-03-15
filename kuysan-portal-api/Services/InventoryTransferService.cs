using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Models;
using SLayerConnectionLib;
using B1SLayer;
using SAPB1SLayerWebAPI.Utils;
using System.Threading.Tasks;

namespace SAPB1SLayerWebAPI.Services
{
    public class InventoryTransferService
    {
        // GET INVENTORY TRANSFERS
        public async Task<Response> GetInventoryTransfersAsync(int userId, string companyDB, char status, string dateFrom, string dateTo, Paginate paginate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = paginate.OrderBy[0].ToString().ToUpper() + paginate.OrderBy[1..];
                string queryFilter = $"DocumentStatus eq '{status}' and DocDate ge '{dateFrom}' and DocDate le '{dateTo}'" + paginate.Filter;


                var count = await connection.Request(EntitiesKeys.StockTransfers)
                    .Filter(queryFilter)
                    .GetCountAsync();

                var result = await connection.Request(EntitiesKeys.StockTransfers)
                    .Filter(queryFilter)
                    .Skip(paginate.Page * paginate.Size)
                    .Top(paginate.Size)
                    .OrderBy($"{orderBy} {paginate.Direction}")
                    .GetAsync<List<DocumentList>>();

                for (int i = 0; i < result.Count; i++)
                {
                    var warehouse = await connection.Request(EntitiesKeys.Warehouses, result[i].FromWarehouse).GetAsync<Warehouse>();
                    result[i].FromWarehouseName = warehouse.WarehouseName;
                }

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

        // CREATE INVENTORY TRANSFER
        public async Task<Response> CreateInventoryTransfersAsync(int userId, string companyDB, char forApproval, dynamic inventoryTransfer) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                await connection.Request(EntitiesKeys.StockTransfers).PostAsync(inventoryTransfer);

                var result = await connection.Request(EntitiesKeys.StockTransfers).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                var newIT = result.First();

                Logger.CreateLog(false, "CREATE INVENTORY TRANSFER", "SUCCESS", JsonConvert.SerializeObject(inventoryTransfer));
                return new Response
                {
                    Status = "success",
                    Message = $"IT #{newIT.DocNum} CREATED successfully!",
                    Payload = newIT
                };
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("no matching record"))
                {
                    if (forApproval == 'Y')
                    {
                        Logger.CreateLog(false, "CREATE INVENTORY TRANSFER APPROVAL", "SUCCESS", JsonConvert.SerializeObject(inventoryTransfer));
                        return new Response
                        {
                            Status = "success",
                            Message = $"IT For Approval CREATED successfully!",
                        };
                    }
                    else
                    {
                        Logger.CreateLog(true, "CREATE INVENTORY TRANSFER", ex.Message, JsonConvert.SerializeObject(inventoryTransfer));
                        return new Response
                        {
                            Status = "failed",
                            Message = ex.Message
                        };
                    }
                }
                Logger.CreateLog(true, "CREATE INVENTORY TRANSFER", ex.Message, JsonConvert.SerializeObject(inventoryTransfer));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }

            //try
            //{
            //    var connection = Main.GetConnection(userId, companyDB);
            //    await connection.Request(EntitiesKeys.StockTransfers).PostAsync(inventoryTransfer);

            //    var result = await connection.Request(EntitiesKeys.StockTransfers).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
            //    var newDR = result.First();

            //    Logger.CreateLog(false, "CREATE INVENTORY TRANSFER", "SUCCESS", JsonConvert.SerializeObject(inventoryTransfer));
            //    return new Response
            //    {
            //        Status = "success",
            //        Message = $"IT #{newDR.DocNum} CREATED successfully!",
            //        Payload = newDR
            //    };
            //}
            //catch (Exception ex)
            //{
            //    Logger.CreateLog(true, "CREATE INVENTORY TRANSFER", ex.Message, JsonConvert.SerializeObject(inventoryTransfer));
            //    return new Response
            //    {
            //        Status = "failed",
            //        Message = ex.Message
            //    };
            //}
        });

        // UPDATE INVENTORY TRANSFER
        public async Task<Response> UpdateInventoryTransferAsync(int userId, string companyDB, dynamic inventoryTransfer) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                await connection.Request(EntitiesKeys.StockTransfers, inventoryTransfer.DocEntry).PatchAsync(inventoryTransfer);
                var result = await connection.Request(EntitiesKeys.StockTransfers, inventoryTransfer.DocEntry).GetAsync();
                Logger.CreateLog(false, "UPDATE INVENTORY TRANSFER", "SUCCESS", JsonConvert.SerializeObject(inventoryTransfer));
                return new Response
                {
                    Status = "success",
                    Message = $"IT #{result.DocNum} UPDATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                Logger.CreateLog(true, "UPDATE INVENTORY TRANSFER", ex.Message, JsonConvert.SerializeObject(inventoryTransfer));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // CANCEL INVENTORY TRANSFER REQUEST
        public async Task<Response> CancelInventoryTransferAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.StockTransfers}({docEntry})/Cancel";

                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.StockTransfers, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"IT #{result.DocNum} CANCELED successfully!",
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

        // GET INVENTORY TRANSFER REQUESTS
        public async Task<Response> GetInventoryTransferRequestsAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var inventoryTransferRequests = await connection.Request(EntitiesKeys.InventoryTransferRequests)
                    .Filter("DocumentStatus eq 'O'")
                    .GetAllAsync<dynamic>();

                for (int i = 0; i < inventoryTransferRequests.Count; i++)
                {
                    var warehouse = await connection.Request(EntitiesKeys.Warehouses, inventoryTransferRequests[i].FromWarehouse.ToString()).GetAsync<Warehouse>();
                    inventoryTransferRequests[i].FromWarehouseName = warehouse.WarehouseName;
                }


                return new Response
                {
                    Status = "success",
                    Payload = inventoryTransferRequests
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
