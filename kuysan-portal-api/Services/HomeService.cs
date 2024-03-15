using B1SLayer;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SLayerConnectionLib;

namespace SAPB1SLayerWebAPI.Services
{
    public class HomeService
    {
        // GET DOCUMENTS COUNT
        public async Task<Response> GetDocumentsCount(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string queryFilter = "DocumentStatus eq 'O'";

                // SALES
                var salesQuotationCount = await connection.Request(EntitiesKeys.Quotations).Filter(queryFilter).GetCountAsync();
                var salesOrderCount = await connection.Request(EntitiesKeys.Orders).Filter(queryFilter).GetCountAsync();
                var deliveryCount = await connection.Request(EntitiesKeys.DeliveryNotes).Filter(queryFilter).GetCountAsync();
                var arInvoiceCount = await connection.Request(EntitiesKeys.Invoices).Filter(queryFilter).GetCountAsync();

                // PURCHASING
                var purchaseRequestCount = await connection.Request(EntitiesKeys.PurchaseRequests).Filter(queryFilter).GetCountAsync();
                var purchaseOrderCount = await connection.Request(EntitiesKeys.PurchaseOrders).Filter(queryFilter).GetCountAsync();
                var goodsReceiptPOCount = await connection.Request(EntitiesKeys.PurchaseDeliveryNotes).Filter(queryFilter).GetCountAsync();
                var apInvoiceCount = await connection.Request(EntitiesKeys.PurchaseInvoices).Filter(queryFilter).GetCountAsync();

                // INVENTORY
                var pickList = await connection.Request(EntitiesKeys.PickLists).GetCountAsync();
                var goodsReceipt = await connection.Request(EntitiesKeys.InventoryGenEntries).Filter(queryFilter).GetCountAsync();
                var goodsIssue = await connection.Request(EntitiesKeys.InventoryGenExits).Filter(queryFilter).GetCountAsync();
                var inventoryTransferRequest = await connection.Request(EntitiesKeys.InventoryTransferRequests).Filter(queryFilter).GetCountAsync();
                var inventoryTransfer = await connection.Request(EntitiesKeys.StockTransfers).Filter(queryFilter).GetCountAsync();

                return new Response
                {
                    Status = "success",
                    Payload = new DocumentsCount
                    {
                        SalesQuotation = salesQuotationCount,
                        SalesOrder = salesOrderCount,
                        Delivery = deliveryCount,
                        ARInvoice = arInvoiceCount,
                        //
                        PurchaseRequest = purchaseRequestCount,
                        PurchaseOrder = purchaseOrderCount,
                        GoodsReceiptPO = goodsReceiptPOCount,
                        APInvoice = apInvoiceCount,
                        //
                        PickList = pickList,
                        GoodsReceipt = goodsReceipt,
                        GoodsIssue = goodsIssue,
                        InventoryTransferRequest = inventoryTransferRequest,
                        InventoryTransfer = inventoryTransfer,
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
