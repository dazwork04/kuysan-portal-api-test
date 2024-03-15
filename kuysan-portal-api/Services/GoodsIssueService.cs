using B1SLayer;
using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Utils;
using SLayerConnectionLib;

namespace SAPB1SLayerWebAPI.Services
{
    public class GoodsIssueService
    {
        // GET GOODS ISSUES
        public async Task<Response> GetGoodsIssuesAsync(int userId, string companyDB, string dateFrom, string dateTo, Paginate paginate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = paginate.OrderBy[0].ToString().ToUpper() + paginate.OrderBy[1..];
                string queryFilter = $"DocDate ge '{dateFrom}' and DocDate le '{dateTo}'" + paginate.Filter;

                var count = await connection.Request(EntitiesKeys.InventoryGenExits)
                    .Filter(queryFilter)
                    .GetCountAsync();

                var result = await connection.Request(EntitiesKeys.InventoryGenExits)
                    .Filter(queryFilter)
                    .Skip(paginate.Page * paginate.Size)
                    .Top(paginate.Size)
                    .OrderBy($"{orderBy} {paginate.Direction}")
                    .GetAsync<List<dynamic>>();

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

        // CREATE GOODS ISSUE
        public async Task<Response> CreateGoodsIssueAsync(int userId, string companyDB, char forApproval, dynamic goodsIssue) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                await connection.Request(EntitiesKeys.InventoryGenExits).PostAsync(goodsIssue);

                var result = await connection.Request(EntitiesKeys.InventoryGenExits).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                var newGI = result.First();

                Logger.CreateLog(false, "CREATE GOODS ISSUE", "SUCCESS", JsonConvert.SerializeObject(goodsIssue));

                return new Response
                {
                    Status = "success",
                    Message = $"GI #{newGI.DocNum} CREATED successfully!",
                    Payload = newGI
                };

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("no matching record"))
                {
                    if (forApproval == 'Y')
                    {
                        Logger.CreateLog(false, "CREATE GOODS ISSUE APPROVAL", "SUCCESS", JsonConvert.SerializeObject(goodsIssue));
                        return new Response
                        {
                            Status = "success",
                            Message = $"GI For Approval CREATED successfully!",
                        };
                    }
                    else
                    {
                        Logger.CreateLog(true, "CREATE GOODS ISSUE", ex.Message, JsonConvert.SerializeObject(goodsIssue));
                        return new Response
                        {
                            Status = "failed",
                            Message = ex.Message
                        };
                    }
                }
                Logger.CreateLog(true, "CREATE GOODS ISSUE", ex.Message, JsonConvert.SerializeObject(goodsIssue));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };

            }

            //try
            //{
            //    var connection = Main.GetConnection(userId, companyDB);
            //    await connection.Request(EntitiesKeys.InventoryGenExits).PostAsync(goodsIssue);

            //    var result = await connection.Request(EntitiesKeys.InventoryGenExits).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
            //    var newDR = result.First();

            //    Logger.CreateLog(false, "CREATE GOODS ISSUE", "SUCCESS", JsonConvert.SerializeObject(goodsIssue));
            //    return new Response
            //    {
            //        Status = "success",
            //        Message = $"GI #{newDR.DocNum} CREATED successfully!",
            //        Payload = newDR
            //    };
            //}
            //catch (Exception ex)
            //{
            //    Logger.CreateLog(true, "CREATE GOODS ISSUE", ex.Message, JsonConvert.SerializeObject(goodsIssue));
            //    return new Response
            //    {
            //        Status = "failed",
            //        Message = ex.Message
            //    };
            //}
        });

        // UPDATE GOODS ISSUE
        public async Task<Response> UpdateGoodsIssueAsync(int userId, string companyDB, dynamic goodsIssue) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                await connection.Request(EntitiesKeys.InventoryGenExits, goodsIssue.DocEntry).PatchAsync(goodsIssue);
                var result = await connection.Request(EntitiesKeys.InventoryGenExits, goodsIssue.DocEntry).GetAsync();
                Logger.CreateLog(false, "UPDATE GOODS ISSUE", "SUCCESS", JsonConvert.SerializeObject(goodsIssue));
                return new Response
                {
                    Status = "success",
                    Message = $"GI #{result.DocNum} UPDATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                Logger.CreateLog(true, "UPDATE GOODS ISSUE", ex.Message, JsonConvert.SerializeObject(goodsIssue));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });
    }
}
