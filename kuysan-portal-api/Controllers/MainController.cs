using Microsoft.AspNetCore.Mvc;
using SAPB1SLayerWebAPI.Services;

namespace SAPB1SLayerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly MainService mainService;
        public MainController() => mainService = new();
   
        // GET SERIES - userId, companyDB, document
        [HttpGet("GetSeries/{userId}/{companyDB}/{document}")]
        public async Task<IActionResult> GetSeries(int userId, string companyDB, string document) =>
            Ok(await mainService.GetSeriesAsync(userId, companyDB, document));

        // GET PAYMENT TERMS - userId, companyDB
        [HttpGet("GetPaymentTerms/{userId}/{companyDB}")]
        public async Task<IActionResult> GetPaymentTerms(int userId, string companyDB) =>
           Ok(await mainService.GetPaymentTermsAsync(userId, companyDB));

        // GET SALES PERSONS - userId, companyDB
        [HttpGet("GetSalesPersons/{userId}/{companyDB}")]
        public async Task<IActionResult> GetSalesPersons(int userId, string companyDB) =>
          Ok(await mainService.GetSalesPersonsAsync(userId, companyDB));

        // GET SHIPPING TYPES - userId, companyDB
        [HttpGet("GetShippingTypes/{userId}/{companyDB}")]
        public async Task<IActionResult> GetShippingTypes(int userId, string companyDB) =>
          Ok(await mainService.GetShippingTypesAsync(userId, companyDB));

        // GET VAT GROUPS - userId, companyDB
        [HttpGet("GetVatGroups/{userId}/{companyDB}/{category}")]
        public async Task<IActionResult> GetVatGroups(int userId, string companyDB, char category) =>
          Ok(await mainService.GetVatGroupsAsync(userId, companyDB, category));

        // GET ITEM GROUPS - userId, companyDB
        [HttpGet("GetItemGroups/{userId}/{companyDB}")]
        public async Task<IActionResult> GetItemGroups(int userId, string companyDB) =>
          Ok(await mainService.GetItemGroupsAsync(userId, companyDB));

        // GET ITEM - userId, companyDB, itemCode
        [HttpGet("GetItem/{userId}/{companyDB}/{itemCode}")]
        public async Task<IActionResult> GetItem(int userId, string companyDB, string itemCode) => Ok(await mainService.GetItemAsync(userId, companyDB, itemCode));

        // GET BUSINESS PARTNER - userId, companyDB, cardCode
        [HttpGet("GetBusinessPartner/{userId}/{companyDB}/{cardCode}")]
        public async Task<IActionResult> GetBusinessPartner(int userId, string companyDB, string cardCode) =>
             Ok(await mainService.GetBusinessPartnerAsync(userId, companyDB, cardCode));

        // GET UOMS - userId, companyDB, ugpEntry
        [HttpGet("GetUoms/{userId}/{companyDB}/{ugpEntry}")]
        public async Task<IActionResult> GetUoms(int userId, string companyDB, int ugpEntry) =>
            Ok(await mainService.GetUomsAsync(userId, companyDB, ugpEntry));

        // GET UOM - userId, companyDB, uomEntry
        [HttpGet("GetUom/{userId}/{companyDB}/{uomEntry}")]
        public async Task<IActionResult> GetUom(int userId, string companyDB, int uomEntry) =>
            Ok(await mainService.GetUomAsync(userId, companyDB, uomEntry));

        // GET EXCHANGE RATES - userId, companyDB, postingDate
        [HttpGet("GetExchangeRates/{userId}/{companyDB}/{postingDate}")]
        public async Task<IActionResult> GetUoms(int userId, string companyDB, string postingDate) =>
           Ok(await mainService.GetExchangeRatesAsync(userId, companyDB, postingDate));

        // GET ADMIN INFO - userId, companyDB
        [HttpGet("GetAdminInfo/{userId}/{companyDB}")]
        public async Task<IActionResult> GetAdminInfo(int userId, string companyDB) =>
           Ok(await mainService.GetAdminInfoAsync(userId, companyDB));

        // GET COUNTRY - userId, companyDB, code
        [HttpGet("GetCountry/{userId}/{companyDB}/{code}")]
        public async Task<IActionResult> GetCountry(int userId, string companyDB, string code) =>
           Ok(await mainService.GetCountryAsync(userId, companyDB, code));

        // GET GL ACCOUNT ADVANCED RULES - userId, companyDB, itemsGroupCode, financialYear
        [HttpGet("GetGLAccountAdvancedRules/{userId}/{companyDB}/{itemsGroupCode}/{financialYear}")]
        public async Task<IActionResult> GetGLAccountAdvancedRules(int userId, string companyDB, int itemsGroupCode, int financialYear) =>
           Ok(await mainService.GetGLAccountAdvancedRulesAsync(userId, companyDB, itemsGroupCode, financialYear));

        // GET EMPLOYEES
        [HttpGet("GetEmployees/{userId}/{companyDB}")]
        public async Task<IActionResult> GetEmployees(int userId, string companyDB) => Ok(await mainService.GetEmployeesAsync(userId, companyDB));

        // GET BRANCHES
        [HttpGet("GetBranches/{userId}/{companyDB}")]
        public async Task<IActionResult> GetBranches(int userId, string companyDB) => Ok(await mainService.GetBranchesAsync(userId, companyDB));

        // GET DEPARTMENTS
        [HttpGet("GetDepartments/{userId}/{companyDB}")]
        public async Task<IActionResult> GetDepartments(int userId, string companyDB) => Ok(await mainService.GetDepartmentsAsync(userId, companyDB));

        // GET ATTACHMENT
        [HttpGet("GetAttachment/{userId}/{companyDB}/{attachmentEntry}")]
        public async Task<IActionResult> GetAttachment(int userId, string companyDB, int attachmentEntry) => Ok(await mainService.GetAttachmentAsync(userId, companyDB, attachmentEntry));

        // GET ATTACHMENT FILE
        [HttpGet("GetAttachmentFile/{userId}/{companyDB}/{attachmentEntry}/{fileName}")]
        public async Task<IActionResult> GetAttachmentFile(int userId, string companyDB, int attachmentEntry, string fileName) => Ok(await mainService.GetAttachmentFileAsync(userId, companyDB, attachmentEntry, fileName));

        // GET DOCUMENT LINES CHART OF ACCOUNTS
        [HttpPost("GetDocumentLinesChartOfAccounts/{userId}/{companyDB}")]
        public async Task<IActionResult> GetDocumentLinesChartOfAccounts(int userId, string companyDB, List<string> accountCodes) => Ok(await mainService.GetDocumentLinesChartOfAccountsAsync(userId, companyDB, accountCodes));

        // GET WAREHOUSES
        [HttpPost("GetWarehouses/{userId}/{companyDB}")]
        public async Task<IActionResult> GetWarehouses(int userId, string companyDB, List<string> warehouseCodes) => Ok(await mainService.GetWarehousesAsync(userId, companyDB, warehouseCodes));

        // GET PAGE DATA - userId, companyDB, objType, actionType, docEntry, series
        [HttpGet("GetPageData/{userId}/{companyDB}/{objType}/{actionType}/{docEntry}/{series}")]
        public async Task<IActionResult> GetPageData(int userId, string companyDB, int objType, char actionType, int docEntry, int series) => Ok(await mainService.GetPageDataAsync(userId, companyDB, objType, actionType, docEntry, series));

        // GET PAGE DATA MULTI - userId, companyDB, objType, actionType, docEntry, series
        [HttpGet("GetPageDataMulti/{userId}/{companyDB}/{objType}/{actionType}/{docEntry}/{series}")]
        public async Task<IActionResult> GetPageDataMulti(int userId, string companyDB, int objType, char actionType, int docEntry, int series) => Ok(await mainService.GetPageDataMultiAsync(userId, companyDB, objType, actionType, docEntry, series));

        // GET RELATIONSHIPMAP
        [HttpGet("GetRelationshipMap/{userId}/{companyDB}/{docEntry}/{objType}")]
        public async Task<IActionResult> GetRelationshipMap(int userId, string companyDB, int docEntry, int objType) => Ok(await mainService.GetRelationshipMapAsync(userId, companyDB, docEntry, objType));

        // PURCHASING
        // GET INIT PURCHASE REQUEST
        [HttpGet("GetInitPurchaseRequest/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitPurchaseRequest(int userId, string companyDB) => Ok(await mainService.GetInitPurchaseRequestAsync(userId, companyDB));

        // GET INIT PURCHASE ORDER
        [HttpGet("GetInitPurchaseOrder/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitPurchaseOrder(int userId, string companyDB) => Ok(await mainService.GetInitPurchaseOrderAsync(userId, companyDB));

        // GET INIT GOODS RECEIPT PO
        [HttpGet("GetInitGoodsReceiptPO/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitGoodsReceiptPO(int userId, string companyDB) => Ok(await mainService.GetInitGoodsReceiptPOAsync(userId, companyDB));

        // GET INIT AP INVOICE
        [HttpGet("GetInitAPInvoice/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitAPInvoice(int userId, string companyDB) => Ok(await mainService.GetInitAPInvoiceAsync(userId, companyDB));

        // SALES
        // GET INIT SALES QUOTATION
        [HttpGet("GetInitSalesQuotation/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitSalesQuotation(int userId, string companyDB) => Ok(await mainService.GetInitSalesQuotationAsync(userId, companyDB));

        // GET INIT SALES ORDER
        [HttpGet("GetInitSalesOrder/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitSalesOrder(int userId, string companyDB) => Ok(await mainService.GetInitSalesOrderAsync(userId, companyDB));

        // GET INIT DELIVERY
        [HttpGet("GetInitDelivery/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitDelivery(int userId, string companyDB) => Ok(await mainService.GetInitDeliveryAsync(userId, companyDB));

        // GET INIT AR INVOICE
        [HttpGet("GetInitARInvoice/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitARInvoice(int userId, string companyDB) => Ok(await mainService.GetInitARInvoiceAsync(userId, companyDB));

        // INVENTORY
        // GET INIT GOODS RECEIPT
        [HttpGet("GetInitGoodsReceipt/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitGoodsReceipt(int userId, string companyDB) => Ok(await mainService.GetInitGoodsReceiptAsync(userId, companyDB));

        // GET INIT GOODS ISSUE
        [HttpGet("GetInitGoodsIssue/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitGoodsIssue(int userId, string companyDB) => Ok(await mainService.GetInitGoodsIssueAsync(userId, companyDB));

        // GET INIT INVENTORY TRANSFER REQUEST
        [HttpGet("GetInitInventoryTransferRequest/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitInventoryTransferRequest(int userId, string companyDB) => Ok(await mainService.GetInitInventoryTransferRequestAsync(userId, companyDB));

        // GET INIT INVENTORY TRANSFER
        [HttpGet("GetInitInventoryTransfer/{userId}/{companyDB}")]
        public async Task<IActionResult> GetInitInventoryTransfer(int userId, string companyDB) => Ok(await mainService.GetInitInventoryTransferAsync(userId, companyDB));

    }
}
