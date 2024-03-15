using B1SLayer;
using Models;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Utils;
using SLayerConnectionLib;

namespace SAPB1SLayerWebAPI.Services
{
    public class ApprovalRequestService
    {
        public async Task<Response> GetApprovalRequestsAsync(int userId, string companyDB, ApprovalRequestBody body) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = body.Paginate.OrderBy[0].ToString().ToUpper() + body.Paginate.OrderBy[1..];

                List<SLApprovalRequest> approvalRequests = [];

                if (body.Approvals.Count > 0)
                {
                    for (int i = 0; i < body.Approvals.Count; i++)
                    {

                        var appReqs = await connection.Request(EntitiesKeys.ApprovalRequests).Filter($"ApprovalTemplatesID eq {body.Approvals[i]} and IsDraft eq 'Y' and Status eq '{body.Status}'").GetAllAsync<SLApprovalRequest>();
                        foreach (var appReq in appReqs)
                        {
                            // originator
                            if (appReq.OriginatorID == body.UserId) approvalRequests.Add(appReq);
                            else
                            {
                                //approver
                                var currStage = await connection.Request(EntitiesKeys.ApprovalStages, appReq.CurrentStage).GetAsync<ApprovalStage>();
                                if (currStage.ApprovalStageApprovers.Any(asa => asa.UserID == body.UserId)) approvalRequests.Add(appReq);
                            }
                        }
                    }
                }

                List<int> draftEntries = approvalRequests.Select(ar => ar.DraftEntry).ToList();

                string queryFilter = "";
                for (int i = 0; i < draftEntries.Count; i++)
                {
                    queryFilter += $"DocEntry eq {draftEntries[i]}";
                    if (i < draftEntries.Count - 1) queryFilter += " or ";
                }
                if (queryFilter != string.Empty) queryFilter = $"({queryFilter})" + body.Paginate.Filter;

                long count = 0;
                List<dynamic> result = [];
                if (draftEntries.Count != 0)
                {
                    count = await connection.Request(EntitiesKeys.Drafts)
                    .Filter(queryFilter)
                    .GetCountAsync();

                    result = await connection.Request(EntitiesKeys.Drafts)
                    .Filter(queryFilter)
                    .Skip(body.Paginate.Page * body.Paginate.Size)
                    .Top(body.Paginate.Size)
                    .OrderBy($"{orderBy} {body.Paginate.Direction}")
                    .GetAsync<List<dynamic>>();
                }


                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        ApprovalRequests = approvalRequests,
                        Drafts = new
                        {
                            Count = count,
                            Data = result
                        }
                    }
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
        
        public async Task<Response> ApproveApprovalRequestAsync(int userId, string companyDB, dynamic body) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = EntitiesKeys.ApprovalRequests + $"({body.code})";

                await connection.Request(reqParam).PatchAsync(new
                {
                    ApprovalRequestDecisions = new List<dynamic>
                    {
                        new { Status = "ardApproved",  Remarks = body.remarks }
                    }
                });

                return new Response
                {
                    Status = "success",
                    Message = $"Approval Request successfully approved."
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

        public async Task<Response> RejectApprovalRequestAsync(int userId, string companyDB, dynamic body) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = EntitiesKeys.ApprovalRequests + $"({body.code})";

                await connection.Request(reqParam).PatchAsync(new
                {
                    ApprovalRequestDecisions = new List<dynamic>
                    {
                        new { Status = "ardNotApproved", Remarks = body.remarks  }
                    }
                });

                return new Response
                {
                    Status = "success",
                    Message = $"Approval Request is set to Not Approved."
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

        public async Task<Response> AddToDocumentApprovalRequestAsync(int userId, string companyDB, dynamic body) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                await connection.Request($"{ActionsKeys.DraftsService}_SaveDraftToDocument").PostAsync(new
                {
                    Document = new { DocEntry = body.docEntry }
                });

                string reqParam = ObjectTypesHelper.GetObjectType(int.Parse(body.objCode.ToString()));

                var result = await connection.Request(reqParam).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                var newDoc = result.First();

                if (body.objCode == 17)
                    CostCenterService.AddCostCenterFromSO(userId, companyDB, newDoc.DocNum.ToString());

                return new Response
                {
                    Status = "success",
                    Message = $"#{newDoc.DocNum} successfully added to Posted Document.",
                    Payload = newDoc
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

        public async Task<Response> UpdateDraftAsync(int userId, string companyDB, dynamic draft) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                await connection.Request(EntitiesKeys.Drafts, draft.DocEntry).PutAsync(draft);
                var result = await connection.Request(EntitiesKeys.Drafts, draft.DocEntry).GetAsync<dynamic>();

                return new Response
                {
                    Status = "success",
                    Message= $"Document Draft #{draft.DocEntry} updated successfully.",
                    Payload = result,
                };
            }
            catch (Exception ex)
            {
                return new Response {
                    Status = "success",
                    Message = ex.Message
                };
            }
        });

        public async Task<Response> GetApprovalRequestsBillingAsync(int userId, string companyDB, ApprovalRequestBody body) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string orderBy = body.Paginate.OrderBy[0].ToString().ToUpper() + body.Paginate.OrderBy[1..];

                List<SLApprovalRequest> approvalRequests = [];

                if (body.Approvals.Count > 0)
                {
                    for (int i = 0; i < body.Approvals.Count; i++)
                    {

                        var appReqs = await connection.Request(EntitiesKeys.ApprovalRequests).Filter($"ApprovalTemplatesID eq {body.Approvals[i]} and IsDraft eq 'Y' and Status eq '{body.Status}'").GetAllAsync<SLApprovalRequest>();
                        foreach (var appReq in appReqs)
                        {
                            if (body.Status == "arsApproved")
                            {
                                approvalRequests.Add(appReq);
                            }
                            else
                            {
                                // originator
                                if (appReq.OriginatorID == body.UserId) approvalRequests.Add(appReq);
                                else
                                {
                                    //approver
                                    var currStage = await connection.Request(EntitiesKeys.ApprovalStages, appReq.CurrentStage).GetAsync<ApprovalStage>();
                                    if (currStage.ApprovalStageApprovers.Any(asa => asa.UserID == body.UserId)) approvalRequests.Add(appReq);
                                }
                            }
                        }
                    }
                }

                List<int> draftEntries = approvalRequests.Select(ar => ar.DraftEntry).ToList();

                string queryFilter = "";
                for (int i = 0; i < draftEntries.Count; i++)
                {
                    queryFilter += $"DocEntry eq {draftEntries[i]}";
                    if (i < draftEntries.Count - 1) queryFilter += " or ";
                }
                if (queryFilter != string.Empty) queryFilter = $"({queryFilter})" + body.Paginate.Filter;

                long count = 0;
                List<dynamic> result = [];
                if (draftEntries.Count != 0)
                {
                    count = await connection.Request(EntitiesKeys.Drafts)
                    .Filter(queryFilter)
                    .GetCountAsync();

                    result = await connection.Request(EntitiesKeys.Drafts)
                    .Filter(queryFilter)
                    .Skip(body.Paginate.Page * body.Paginate.Size)
                    .Top(body.Paginate.Size)
                    .OrderBy($"{orderBy} {body.Paginate.Direction}")
                    .GetAsync<List<dynamic>>();
                }


                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        ApprovalRequests = approvalRequests,
                        Drafts = new
                        {
                            Count = count,
                            Data = result
                        }
                    }
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
