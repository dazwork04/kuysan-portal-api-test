using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Services;

namespace SAPB1SLayerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTransferController : ControllerBase
    {
        private readonly InventoryTransferService itService;
        public InventoryTransferController() => itService = new();

        // GET INVENTORY TRANSFERS
        [HttpPost("GetInventoryTransfers/{userId}/{companyDB}/{status}/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetInventoryTransfers(int userId, string companyDB, char status, string dateFrom, string dateTo, Paginate paginate) =>
            Ok(await itService.GetInventoryTransfersAsync(userId, companyDB, status, dateFrom, dateTo, paginate));

        // CREATE INVENTORY TRANSFER
        [HttpPost("CreateInventoryTransfer/{userId}/{companyDB}/{forApproval}")]
        public async Task<IActionResult> CreateInventoryTransfer(int userId, string companyDB, char forApproval, dynamic inventoryRequestTransfer) => 
            Ok(await itService.CreateInventoryTransfersAsync(userId, companyDB, forApproval,inventoryRequestTransfer));

        // UPDATE INVENTORY TRANSFER
        [HttpPost("UpdateInventoryTransfer/{userId}/{companyDB}")]
        public async Task<IActionResult> UpdateInventoryTransfer(int userId, string companyDB, dynamic inventoryRequestTransfer) => 
            Ok(await itService.UpdateInventoryTransferAsync(userId, companyDB, inventoryRequestTransfer));

        // CANCEL INVENTORY TRANSFER
        [HttpPost("CancelInventoryTransfer/{userId}/{companyDB}/{docEntry}")]
        public async Task<IActionResult> CancelInventoryTransfer(int userId, string companyDB, int docEntry) => 
            Ok(await itService.CancelInventoryTransferAsync(userId, companyDB, docEntry));

        // GET INVENTORY TRANSFER REQUESTS
        [HttpGet("GetInventoryTransferRequests/{userId}/{companyDB}")]
        public async Task<IActionResult> GetPurchaseOrders(int userId, string companyDB) =>
            Ok(await itService.GetInventoryTransferRequestsAsync(userId, companyDB));
    }
}
