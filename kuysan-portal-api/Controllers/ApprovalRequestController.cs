using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Services;

namespace SAPB1SLayerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalRequestController : ControllerBase
    {
        #region VARIABLES
        private readonly ApprovalRequestService approvalRequestService;
        #endregion
        public ApprovalRequestController() => approvalRequestService = new();

        [HttpPost("GetApprovalRequests/{userId}/{companyDB}")]
        public async Task<IActionResult> GetApprovalRequests(int userId, string companyDB, ApprovalRequestBody body) => Ok(await approvalRequestService.GetApprovalRequestsAsync(userId, companyDB, body));

        //[HttpGet("GetApprovedApprovalRequests/{userId}/{companyDB}")]
        //public async Task<IActionResult> GetApprovedApprovalRequests(int userId, string companyDB, ApprovalRequestBody body) => Ok(await approvalRequestService.GetApprovedApprovalRequests(userId, companyDB, body));

        [HttpPost("ApproveApprovalRequest/{userId}/{companyDB}")]
        public async Task<IActionResult> ApproveApprovalRequest(int userId, string companyDB, dynamic body) => Ok(await approvalRequestService.ApproveApprovalRequestAsync(userId, companyDB, body));

        [HttpPost("RejectApprovalRequest/{userId}/{companyDB}")]
        public async Task<IActionResult> RejectApprovalRequest(int userId, string companyDB, dynamic body) => Ok(await approvalRequestService.RejectApprovalRequestAsync(userId, companyDB, body));

        [HttpPost("AddToDocumentApprovalRequest/{userId}/{companyDB}")]
        public async Task<IActionResult> AddToDocumentApprovalRequest(int userId, string companyDB, dynamic body) => Ok(await approvalRequestService.AddToDocumentApprovalRequestAsync(userId, companyDB, body));

        [HttpPost("UpdateDraft/{userId}/{companyDB}")]
        public async Task<IActionResult> UpdateDraft(int userId, string companyDB, dynamic draft) => Ok(await approvalRequestService.UpdateDraftAsync(userId, companyDB, draft));

        [HttpPost("GetApprovalRequestsBilling/{userId}/{companyDB}")]
        public async Task<IActionResult> GetApprovalRequestsBilling(int userId, string companyDB, ApprovalRequestBody body) => Ok(await approvalRequestService.GetApprovalRequestsBillingAsync(userId, companyDB, body));


    }
}
