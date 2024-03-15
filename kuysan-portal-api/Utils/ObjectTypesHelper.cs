using SLayerConnectionLib;
using System.Data;
using System.Linq;

namespace SAPB1SLayerWebAPI.Utils
{
    public class ObjectTypesHelper
    {
        public static readonly List<int> OBJECT_TYPES = [13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 1470000113, 1250000001, 67, 59, 60, 156];
        public static string GetObjectType (int objType)
        {
            string reqParam = string.Empty;
            switch (objType)
            {
                case 13:
                    reqParam = EntitiesKeys.Invoices;
                    break;
                case 14:
                    reqParam = EntitiesKeys.CreditNotes;
                    break;
                case 15:
                    reqParam = EntitiesKeys.DeliveryNotes;
                    break;
                case 16:
                    reqParam = EntitiesKeys.Returns;
                    break;
                case 17:
                    reqParam = EntitiesKeys.Orders;
                    break;
                case 18:
                    reqParam = EntitiesKeys.PurchaseInvoices;
                    break;
                case 19:
                    reqParam = EntitiesKeys.PurchaseCreditNotes;
                    break;
                case 20:
                    reqParam = EntitiesKeys.PurchaseDeliveryNotes;
                    break;
                case 21:
                    reqParam = EntitiesKeys.PurchaseReturns;
                    break;
                case 22:
                    reqParam = EntitiesKeys.PurchaseOrders;
                    break;
                case 23:
                    reqParam = EntitiesKeys.Quotations;
                    break;
                case 1470000113:
                    reqParam = EntitiesKeys.PurchaseRequests;
                    break;
                case 1250000001:
                    reqParam = EntitiesKeys.InventoryTransferRequests;
                    break;
                case 67:
                    reqParam = EntitiesKeys.StockTransfers;
                    break;
                case 59:
                    reqParam = EntitiesKeys.InventoryGenEntries;
                    break;
                case 60:
                    reqParam = EntitiesKeys.InventoryGenExits;
                    break;
                case 156:
                    reqParam = EntitiesKeys.PickLists;
                    break;
                default:
                    break;
            }

            return reqParam;
        }
    }
}
