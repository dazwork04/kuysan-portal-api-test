using Newtonsoft.Json;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Models;
using SLayerConnectionLib;
using B1SLayer;
using SAPB1SLayerWebAPI.Utils;

namespace SAPB1SLayerWebAPI.Services
{
    public class UserFieldsMDService
    {
        // GET USER FIELDS
        // TableName, FieldId
        public async Task<Response> GetUserFieldsMDAsync(int userId, string companyDB, List<UserFieldParam> parameter) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string queryFilter = "";
                for (int i = 0; i < parameter.Count; i++)
                {
                    queryFilter += $"(TableName eq '{parameter[0].TableName}' and FieldID eq {parameter[0].FieldID})";
                    if (i < parameter.Count - 1) queryFilter += " or ";
                }
                //List<UserFieldsMD> results = [];
                //foreach (var item in results)
                //{
                //    var result = await connection.Request($"{EntitiesKeys.UserFieldsMD}(TableName={},FieldID={})")
                //    .GetAsync<UserFieldsMD>();

                //    results.Add(result);
                //}

                //var result = await connection.Request($"{EntitiesKeys.UserFieldsMD)")
                //    .GetAsync<UserFieldsMD>();

                var result = await connection.Request(EntitiesKeys.UserFieldsMD)
                    .Filter(queryFilter)
                    .GetAsync<List<UserFieldsMD>>();

                return new Response
                {
                    Status = "success",
                    Payload = result
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

    }
}
