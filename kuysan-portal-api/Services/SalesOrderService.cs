﻿using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Models;
using SLayerConnectionLib;
using B1SLayer;
using SAPB1SLayerWebAPI.Utils;

namespace SAPB1SLayerWebAPI.Services
{
    public class SalesOrderService
    {
        // GET SALES ORDERS
        public async Task<Response> GetSalesOrdersAsync(int userId, string companyDB, char status, char cancelled, string dateFrom, string dateTo, Paginate paginate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = paginate.OrderBy[0].ToString().ToUpper() + paginate.OrderBy[1..];
                string queryFilter = $"DocumentStatus eq '{status}' and Cancelled eq '{cancelled}' and DocDate ge '{dateFrom}' and DocDate le '{dateTo}'" + paginate.Filter;


                var count = await connection.Request(EntitiesKeys.Orders)
                    .Filter(queryFilter)
                    .GetCountAsync();

                var result = await connection.Request(EntitiesKeys.Orders)
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

        // CREATE SALES ORDER
        public async Task<Response> CreateSalesOrderAsync(int userId, string companyDB, char forApproval, dynamic salesOrder) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                await connection.Request(EntitiesKeys.Orders).PostAsync(salesOrder);

                var result = await connection.Request(EntitiesKeys.Orders).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                var newSO = result.First();

                CostCenterService.AddCostCenterFromSO(userId, companyDB, newSO.DocNum.ToString());

                Logger.CreateLog(false, "CREATE SALES ORDER", "SUCCESS", JsonConvert.SerializeObject(salesOrder));

                return new Response
                {
                    Status = "success",
                    Message = $"SO #{newSO.DocNum} CREATED successfully!",
                    Payload = newSO
                };

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("no matching record"))
                {
                    if (forApproval == 'Y')
                    {
                        Logger.CreateLog(false, "CREATE SALES ORDER APPROVAL", "SUCCESS", JsonConvert.SerializeObject(salesOrder));
                        return new Response
                        {
                            Status = "success",
                            Message = $"SO For Approval CREATED successfully!",
                        };
                    }
                    else
                    {
                        Logger.CreateLog(true, "CREATE SALES ORDER", ex.Message, JsonConvert.SerializeObject(salesOrder));
                        return new Response
                        {
                            Status = "failed",
                            Message = ex.Message
                        };
                    }
                }
                Logger.CreateLog(true, "CREATE SALES ORDER", ex.Message, JsonConvert.SerializeObject(salesOrder));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // UPDATE SALES ORDER
        public async Task<Response> UpdateSalesOrderAsync(int userId, string companyDB, dynamic salesOrder) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                await connection.Request(EntitiesKeys.Orders, salesOrder.DocEntry).PatchAsync(salesOrder);
                var result = await connection.Request(EntitiesKeys.Orders, salesOrder.DocEntry).GetAsync();
                Logger.CreateLog(false, "UPDATE SALES ORDER", "SUCCESS", JsonConvert.SerializeObject(salesOrder));
                return new Response
                {
                    Status = "success",
                    Message = $"SO #{result.DocNum} UPDATED successfully!",
                    Payload = result
                };
            }
            catch (Exception ex)
            {

                Logger.CreateLog(true, "UPDATE SALES ORDER", ex.Message, JsonConvert.SerializeObject(salesOrder));
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        });

        // CANCEL SALES ORDER
        public async Task<Response> CancelSalesOrderAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.Orders}({docEntry})/Cancel";

                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.Orders, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"SO #{result.DocNum} CANCELED successfully!",
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

        // CLOSE SALES ORDER
        public async Task<Response> CloseSalesOrderAsync(int userId, string companyDB, int docEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = $"{EntitiesKeys.Orders}({docEntry})/Close";
                await connection.Request(reqParam).PostAsync();
                var result = await connection.Request(EntitiesKeys.Orders, docEntry).GetAsync();

                return new Response
                {
                    Status = "success",
                    Message = $"SO #{result.DocNum} CLOSED successfully!",
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

        // GET SALES QUOTATIONS
        public async Task<Response> GetSalesQuotationsAsync(int userId, string companyDB, string cardCode, string docType, string priceMode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var salesQuotations = await connection.Request(EntitiesKeys.Quotations)
                    .Filter($"CardCode eq '{cardCode}' and DocType eq '{docType}' and PriceMode eq '{priceMode}' and DocumentStatus eq 'O'")
                    .GetAllAsync<dynamic>();


                return new Response
                {
                    Status = "success",
                    Payload = salesQuotations
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
