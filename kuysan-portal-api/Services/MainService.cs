using B1SLayer;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Utils;
using SLayerConnectionLib;

namespace SAPB1SLayerWebAPI.Services
{
    public class MainService
    {
        // GET SERIES
        public async Task<Response> GetSeriesAsync(int userId, string companyDB, string document) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                
                var parameters = new { DocumentTypeParams = new { Document = document } };
                string reqParam = $"{ActionsKeys.SeriesService}_GetDocumentSeries";

                var result = await connection.Request(reqParam)
                    .OrderBy("Series")
                    .PostAsync<List<SLSeries>>(parameters);


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
                };
            }
        });

        // GET PAYMENT TERMS
        public async Task<Response> GetPaymentTermsAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.PaymentTermsTypes)
                    //.Select("GroupNumber, PaymentTermsGroupName, NumberOfAdditionalDays")
                    .OrderBy("GroupNumber")
                    .GetAsync<List<SLPaymentTermType>>();

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
                    Message = ex.Message
                };
            }
        });

        // GET SALES PERSOMS
        public async Task<Response> GetSalesPersonsAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.SalesPersons)
                    //.Select("SalesEmployeeCode, SalesEmployeeName")
                    .Filter("Active eq 'Y'")
                    .OrderBy("SalesEmployeeCode")
                    .GetAsync<List<SalesPerson>>();

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
                    Message = ex.Message
                };
            }
        });

        // GET SHIPPING TYPE
        public async Task<Response> GetShippingTypesAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.ShippingTypes)
                    //.Select("Code, Name")
                    .OrderBy("Name")
                    .GetAsync<List<ShippingType>>();

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
                    Message = ex.Message
                };
            }
        });

        //GET VAT GROUPS
        public async Task<Response> GetVatGroupsAsync(int userId, string companyDB, char category) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.VatGroups)
                    //.Select("Code, Name, VatGroups_Lines")
                    .Filter($"Inactive eq 'N' and Category eq '{category}'")
                    .GetAsync<List<VatGroup>>();

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
                    Message = ex.Message
                };
            }
        });

        //GET ITEM GROUPS
        public async Task<Response> GetItemGroupsAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.ItemGroups)
                    //.Select("Number, GroupName")
                    .GetAsync<List<ItemGroup>>();

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
                    Message = ex.Message
                };
            }
        });

        //GET ITEMS
        public async Task<Response> GetItemsAsync(int userId, string companyDB, short itemsGroupCode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var result = await connection.Request(EntitiesKeys.Items)
                    .Filter($"ItemsGroupCode eq {itemsGroupCode}")
                    .OrderBy("ItemName")
                    .GetAsync<List<Item>>();

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
                    Message = ex.Message
                };
            }
        });

        // GET ITEM
        public async Task<Response> GetItemAsync(int userId, string companyDB, string itemCode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var result = await connection.Request(EntitiesKeys.Items, itemCode).GetAsync<Item>();
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
                    Message = ex.Message
                };
            }
        });

         //GET BUSINESS PARTNERS
        public async Task<Response> GetBusinessPartnersAsync(int userId, string companyDB, char cardType) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var result = await connection.Request(EntitiesKeys.BusinessPartners)
                    .Filter($"CardType eq '{cardType}' and Frozen eq 'N'")
                    .OrderBy("CardName")
                    .GetAsync<List<BusinessPartner>>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        result.Count,
                        Data = result
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

        // GET BUSINESS PARTNER
        public async Task<Response> GetBusinessPartnerAsync(int userId, string companyDB, string cardCode) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var result = await connection.Request(EntitiesKeys.BusinessPartners, cardCode).GetAsync<BusinessPartner>();

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
                    Message = ex.Message
                };
            }
        });

        // GET UOMS
        public async Task<Response> GetUomsAsync(int userId, string companyDB, int ugpEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var uomGroups = await connection.Request(EntitiesKeys.UnitOfMeasurementGroups, ugpEntry)
                    .GetAsync<UomGroup>();

                var collection = uomGroups.UoMGroupDefinitionCollection;

                List<Uom> uomLists = new();

                foreach (var item in collection)
                {
                    int uomEntry = item.AlternateUoM;
                    var uom = await connection.Request(EntitiesKeys.UnitOfMeasurements, uomEntry).GetAsync<Uom>();
                    uomLists.Add(uom);
                }

                return new Response
                {
                    Status = "success",
                    Payload = uomLists
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

        // GET UOM
        public async Task<Response> GetUomAsync(int userId, string companyDB, int uomEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var uom = await connection.Request(EntitiesKeys.UnitOfMeasurements, uomEntry)
                    .GetAsync<Uom>();

                return new Response
                {
                    Status = "success",
                    Payload = uom
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

        // GET EXCHANGE RATES
        public async Task<Response> GetExchangeRatesAsync(int userId, string companyDB, string postingDate) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var body = new { ParamList = $"postingDate='{postingDate}'" };
                string request = $"{EntitiesKeys.SQLQueries}('getCurrencyRates')/List";
                var result = await connection.Request(request).PostAsync<List<ExchangeRate>>(body);

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
                    Message = ex.Message
                };
            }
        });

        // GET ADMIN INFO
        public async Task<Response> GetAdminInfoAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string request = $"{ActionsKeys.CompanyService}_GetAdminInfo";
                AdminInfo result = await connection.Request(request)
                    .PostAsync<AdminInfo>();

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
                    Message = ex.Message
                };
            }
        });

        // GET COUNTRY
        public async Task<Response> GetCountryAsync(int userId, string companyDB, string code) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                var result = await connection.Request(EntitiesKeys.Countries, code)
                    .GetAsync<Country>();

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
                    Message = ex.Message
                };
            }
        });

        // GET GL ACCOUNT ADVANCED RULES
        public async Task<Response> GetGLAccountAdvancedRulesAsync(int userId, string companyDB, int itemsGroupCode, int financialYear) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.GLAccountAdvancedRules)
                    .Filter($"ItemGroup eq {itemsGroupCode} and IsActive eq 'Y' and FinancialYear eq {financialYear}")
                    .GetAsync<List<GLAccountAdvancedRule>>();

                if (result.Count == 0)
                {
                    result = await connection.Request(EntitiesKeys.GLAccountAdvancedRules)
                        .Filter($"ItemGroup eq -1 and IsActive eq 'Y' and FinancialYear eq {financialYear}")
                        .GetAsync<List<GLAccountAdvancedRule>>();
                }

                return new Response
                {
                    Status = "success",
                    Payload = result.Count > 0 ? result[0] : null
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

        // GET EMPLOYEES
        public async Task<Response> GetEmployeesAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.EmployeesInfo)
                    .Filter("Active eq 'Y'")
                    .GetAllAsync<EmployeeInfo>();

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
                    Message = ex.Message
                };
            }
        });

        // GET BRANCHES
        public async Task<Response> GetBranchesAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.Branches)
                    .GetAllAsync<Branch>();

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
                    Message = ex.Message
                };
            }
        });

        // GET DEPARTMENTS
        public async Task<Response> GetDepartmentsAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.Departments)
                    .GetAllAsync<Department>();

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
                    Message = ex.Message
                };
            }
        });

        // GET ATTACHMENT
        public async Task<Response> GetAttachmentAsync(int userId, string companyDB, int attachmentEntry) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                var result = await connection.Request(EntitiesKeys.Attachments2, attachmentEntry).GetAsync<Attachment>();

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
                    Message = ex.Message
                };
            }
        });

        // DOWNLOAD ATTACHMENT
        public async Task<Response> GetAttachmentFileAsync(int userId, string companyDB, int attachmentEntry, string fileName) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                //var result = await connection.Request(EntitiesKeys.Attachments2, attachmentEntry).GetAsync<Attachment>();

                byte[] byteArray = await connection.GetAttachmentAsBytesAsync(attachmentEntry, fileName);
                //MemoryStream stream = new(byteArray);
                //StreamReader reader = new(stream);
                //string result = reader.ReadToEnd();

                return new Response
                {
                    Status = "success",
                    Payload = byteArray,
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



        // GET DOCUMENT LINES CHART OF ACCOUNTS
        public async Task<Response> GetDocumentLinesChartOfAccountsAsync(int userId, string companyDB, List<string> accountCodes) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string filter = "";
                for(int i = 0; i < accountCodes.Count; i++)
                {
                    filter += $"Code eq '{accountCodes[i]}'";
                    if (i < accountCodes.Count - 1) filter += " or ";
                }

                var result = await connection.Request(EntitiesKeys.ChartOfAccounts)
                    .Filter(filter += " and ActiveAccount eq 'Y' and FrozenFor eq 'N'")
                    .GetAllAsync<ChartOfAccount>();

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
                    Message = ex.Message
                };
            }
        });

        // GET WAREHOUSES
        public async Task<Response> GetWarehousesAsync(int userId, string companyDB, List<string> warehouseCodes) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string filter = "";
                for (int i = 0; i < warehouseCodes.Count; i++)
                {
                    filter += $"WarehouseCode eq '{warehouseCodes[i]}'";
                    if (i < warehouseCodes.Count - 1) filter += " or ";
                }

                var result = await connection.Request(EntitiesKeys.Warehouses)
                    .Filter(filter)
                    .GetAllAsync<Warehouse>();

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
                    Message = ex.Message
                };
            }
        });

        // GET PAGE DATA -- FPNLR
        public async Task<Response> GetPageDataAsync(int userId, string companyDB, int objType, char actionType, int docEntry, int series) => await Task.Run(async () =>
        {
            try
            {
                string reqParam = ObjectTypesHelper.GetObjectType(objType);
                string filter = $"Series eq {series}";

                var connection = Main.GetConnection(userId, companyDB);

                dynamic? payload = null;
                var result1 = await connection.Request(reqParam).Filter(filter).OrderBy("DocEntry asc").Top(1).GetAsync<List<dynamic>>(); //list
                var result2 = await connection.Request(reqParam).Filter(filter).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>(); // list
                var firstData = result1.First();
                var lastData = result2.First();

                switch (actionType)
                {
                    case 'F': // First
                        payload = firstData;
                        break;
                    case 'P': // Prev
                        if (docEntry == (int)firstData.DocEntry || docEntry == 0) payload = lastData;
                        else payload = await connection.Request(reqParam, docEntry - 1).GetAsync();
                        break;
                    case 'N': // Next
                        if (docEntry == (int)lastData.DocEntry || docEntry == 0) payload = firstData;
                        else payload = await connection.Request(reqParam, docEntry + 1).GetAsync();
                        break;
                    case 'L': // Last
                        payload = lastData;
                        break;
                    case 'R': // Refresh
                        payload = await connection.Request(reqParam, docEntry).GetAsync();
                        break;
                    default:
                        return new Response
                        {
                            Status = "failed",
                            Message = "Invalid Action Type."
                        };
                }


                return new Response
                {
                    Status = "success",
                    Payload = payload
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

        // GET PAGE DATA MULTI -- FPNLR
        public async Task<Response> GetPageDataMultiAsync(int userId, string companyDB, int objType, char actionType, int docEntry, int series) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);
                string reqParam = ObjectTypesHelper.GetObjectType(objType);

                string request = $"$crossjoin({reqParam}, {reqParam}/DocumentLines)"; // InventoryGenEntries, InventoryGenExits
                string expand = $"{reqParam}($select = DocEntry)"; //,{reqParam}/DocumentLines($select = BaseType, DocEntry, LineNum)
                string filter = $"{reqParam}/Series eq {series} and {reqParam}/DocEntry eq {reqParam}/DocumentLines/DocEntry and {reqParam}/DocumentLines/BaseType ne 202 and {reqParam}/DocumentLines/LineNum eq 0";

                dynamic? result1 = null;
                dynamic? result2 = null;

                if (objType == 59) // GR
                {
                    result1 = await connection.Request(request).Expand(expand).Filter(filter).OrderBy("DocEntry asc").Top(1).GetAsync<List<dynamic>>();
                    result2 = await connection.Request(request).Expand(expand).Filter(filter).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                } 
                else // GI
                {
                    result1 = await connection.Request(request).Expand(expand).Filter(filter).OrderBy("DocEntry asc").Top(1).GetAsync<List<dynamic>>();
                    result2 = await connection.Request(request).Expand(expand).Filter(filter).OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                }

                int docEntry1 = result1[0][reqParam]["DocEntry"];
                var firstData = await connection.Request(reqParam, docEntry1).GetAsync();
                int docEntry2 = result2[0][reqParam]["DocEntry"];
                var lastData = await connection.Request(reqParam, docEntry2).GetAsync();

                dynamic? payload = null;
                switch (actionType)
                {
                    case 'F': // First
                        payload = firstData;
                        break;
                    case 'P': // Prev
                        if (docEntry == (int)firstData.DocEntry || docEntry == 0) payload = lastData;
                        else
                        {
                            var result = await connection.Request(request).Expand(expand).Filter($"{filter} and {reqParam}/DocEntry lt {docEntry}").OrderBy("DocEntry desc").Top(1).GetAsync<List<dynamic>>();
                            int resDocEntry = result[0][reqParam]["DocEntry"];
                            var data = await connection.Request(reqParam, resDocEntry).GetAsync();
                            payload = data;
                        }
                        break;
                    case 'N': // Next
                        if (docEntry == (int)lastData.DocEntry || docEntry == 0) payload = firstData;
                        else
                        {
                            var result = await connection.Request(request).Expand(expand).Filter($"{filter} and {reqParam}/DocEntry gt {docEntry}").OrderBy("DocEntry asc").Top(1).GetAsync<List<dynamic>>();
                            int resDocEntry = result[0][reqParam]["DocEntry"];
                            var data = await connection.Request(reqParam, resDocEntry).GetAsync();
                            payload = data;
                        }
                        break;
                    case 'L': // Last
                        payload = lastData;
                        break;
                    case 'R': // Refresh
                        payload = await connection.Request(reqParam, docEntry).GetAsync();
                        break;
                    default:
                        return new Response
                        {
                            Status = "failed",
                            Message = "Invalid Action Type."
                        };
                }


                return new Response
                {
                    Status = "success",
                    Payload = payload
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

        // GET RELATIONSHIP MAP
        public async Task<Response> GetRelationshipMapAsync(int userId, string companyDB, int docEntry, int objType) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                string reqParam = ObjectTypesHelper.GetObjectType(objType);

                List<BusinessPartnerMap> businessPartners = new();

                var result = await connection.Request(reqParam, docEntry).GetAsync<Document>();
                var mainDoc = await connection.Request(reqParam, docEntry).GetAsync<DocumentMap>();
                businessPartners.Add(new () { CardCode = result.CardCode, CardName = result.CardName });

                var docLines = result.DocumentLines.Select(x => new { x.BaseEntry, x.BaseType, x.TargetEntry, x.TargetType }).Distinct().ToList();

                List<DocumentMap> bases = new();
                foreach (var docLine in docLines)
                {
                    if (docLine.BaseType != -1 && docLine.BaseType != 0)
                    {
                        string lineResource = ObjectTypesHelper.GetObjectType(docLine.BaseType);
                        var baseDoc = await connection.Request(lineResource, docLine.BaseEntry).GetAsync<DocumentMap>();
                        businessPartners.Add(new()
                        {
                            CardCode = baseDoc.CardCode,
                            CardName = baseDoc.CardName
                        });
                        bases.Add(baseDoc);
                    }
                }

                List<DocumentMap> targets = new();
                foreach (var docLine in docLines)
                {
                    if (docLine.TargetType != -1 && docLine.TargetType != 0)
                    {
                        string lineResource = ObjectTypesHelper.GetObjectType(docLine.TargetType);
                        var targetDoc = await connection.Request(lineResource, docLine.TargetEntry).GetAsync<DocumentMap>();
                        businessPartners.Add(new()
                        {
                            CardCode = targetDoc.CardCode,
                            CardName = targetDoc.CardName
                        });
                        targets.Add(targetDoc);
                    }
                }

                RelationshipMap relationshipMap = new()
                {
                    BusinessPartners = businessPartners.DistinctBy(x => x.CardCode).ToList(),
                    Main = mainDoc,
                    Bases = bases,
                    Targets = targets
                };


                return new Response
                {
                    Status = "success",
                    Payload = relationshipMap
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

        // INIT
        // PURCHASING
        // GET INIT PURCHASE REQUEST
        public async Task<Response> GetInitPurchaseRequestAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 1470000113 } });
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'I'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();
                // BRANCHES
                var branches = await connection.Request(EntitiesKeys.Branches).GetAllAsync<Branch>();
                // DEPARTMENTS
                var departments = await connection.Request(EntitiesKeys.Departments).GetAllAsync<Department>();
                // EMPLOYEES
                var employees = await connection.Request(EntitiesKeys.EmployeesInfo).Filter("Active eq 'Y'").GetAllAsync<EmployeeInfo>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                        Branches = branches,
                        Departments = departments,
                        Employees = employees
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT PURCHASE ORDER
        public async Task<Response> GetInitPurchaseOrderAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 22 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                // SHIPPING TYPES
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'I'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT GOODS RECEIPT PO
        public async Task<Response> GetInitGoodsReceiptPOAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 20 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                // SHIPPING TYPES
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'I'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT AP INVOICE
        public async Task<Response> GetInitAPInvoiceAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 18 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                // SHIPPING TYPES
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'I'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // SALES
        // GET INIT SALES QUOTATION
        public async Task<Response> GetInitSalesQuotationAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 23 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                //
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'O'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT SALES QUOTATION
        public async Task<Response> GetInitSalesOrderAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

               

                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 17 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                //
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'O'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT DELIVERY
        public async Task<Response> GetInitDeliveryAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 15 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                //
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'O'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT AR INVOICE
        public async Task<Response> GetInitARInvoiceAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 13 } });
                // PAYMENT TERMS
                var paymentTerms = await connection.Request(EntitiesKeys.PaymentTermsTypes).OrderBy("GroupNumber").GetAllAsync<SLPaymentTermType>();
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                // SHIPPING TYPES
                var shippingTypes = await connection.Request(EntitiesKeys.ShippingTypes).OrderBy("Name").GetAllAsync<ShippingType>();
                // VAT GROUPS
                var vatGroups = await connection.Request(EntitiesKeys.VatGroups).Filter($"Inactive eq 'N' and Category eq 'O'").GetAllAsync<VatGroup>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        ShippingTypes = shippingTypes,
                        VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // INVENTORY
        // GET INIT GOODS RECEIPT
        public async Task<Response> GetInitGoodsReceiptAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 59 } });
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT GOODS ISSUE
        public async Task<Response> GetInitGoodsIssueAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 60 } });
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT INVENTORY TRANSFER REQUEST
        public async Task<Response> GetInitInventoryTransferRequestAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 1250000001 } });
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<SalesPerson>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        //PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        //ShippingTypes = shippingTypes,
                        //VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });

        // GET INIT INVENTORY TRANSFER
        public async Task<Response> GetInitInventoryTransferAsync(int userId, string companyDB) => await Task.Run(async () =>
        {
            try
            {
                var connection = Main.GetConnection(userId, companyDB);

                // SERIES
                var series = await connection.Request($"{ActionsKeys.SeriesService}_GetDocumentSeries").OrderBy("Series").PostAsync<List<SLSeries>>(new { DocumentTypeParams = new { Document = 67 } });
                // SALES PERSONS
                var salesPersons = await connection.Request(EntitiesKeys.SalesPersons).Filter("Active eq 'Y'").OrderBy("SalesEmployeeCode").GetAllAsync<SalesPerson>();
                // ITEM GROUPS
                var itemGroups = await connection.Request(EntitiesKeys.ItemGroups).GetAllAsync<ItemGroup>();

                return new Response
                {
                    Status = "success",
                    Payload = new
                    {
                        Series = series,
                        //PaymentTerms = paymentTerms,
                        SalesPersons = salesPersons,
                        //ShippingTypes = shippingTypes,
                        //VatGroups = vatGroups,
                        ItemGroups = itemGroups,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "failed",
                    Message = ex.Message,
                };
            }
        });
    }
}
