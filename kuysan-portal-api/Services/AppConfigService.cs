﻿using B1SLayer;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Utils;
using SLayerConnectionLib;

namespace SAPB1SLayerWebAPI.Services
{
    public class AppConfigService
    {
        // GET APP CONFIG
        public async Task<Response> GetAppConfigAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // ADMIN INFO
                AdminInfo adminInfo = await connection.Request($"{ActionsKeys.CompanyService}_GetAdminInfo").PostAsync<AdminInfo>();

                // PATH ADMIN
                PathAdmin pathAdmin = await connection.Request($"{ActionsKeys.CompanyService}_GetPathAdmin").PostAsync<PathAdmin>();

                // APPROVAL TEMPLATES
                var approvalTemplates = await connection.Request(EntitiesKeys.ApprovalTemplates).Filter("IsActive eq 'Y'").GetAllAsync<ApprovalTemplate>();

                // APPROVAL STAGES
                 List<int> appStageCodes = [];
                foreach (var appTemp in approvalTemplates)
                {
                    appStageCodes.AddRange(appTemp.ApprovalTemplateStages.Select(ats => ats.ApprovalStageCode).ToList());
                }


                string filter = string.Join(" or ", appStageCodes.Select(asc => $"Code eq {asc}").ToList());
                var approvalStages = await connection.Request(EntitiesKeys.ApprovalStages).Filter(filter).GetAllAsync<ApprovalStage>();
               
                // DOCUMENT CHANGED MENU NAMES
                List<DocumentChangedMenuName> documentChangedMenuNames = [];
                foreach (int objType in ObjectTypesHelper.OBJECT_TYPES)
                {
                    var result = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentChangedMenuName")
                        .OrderBy("Series")
                        .PostAsync<DocumentChangedMenuName>(new { DocumentTypeParams = new { Document = objType } });
                    documentChangedMenuNames.Add(result);
                }

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        AdminInfo = adminInfo,
                        PathAdmin = pathAdmin,
                        DocumentChangedMenuNames = documentChangedMenuNames,
                        ApprovalTemplates = approvalTemplates,
                        ApprovalStages = approvalStages,
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

    }
}
