using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;


namespace GTERP.Controllers
{
    public static class StringExt
    {
        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

    }
    [OverridableAuthorize]
    public class COM_MasterLCController : Controller
    {
        private GTRDBContext db;
        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public COM_MasterLCController(GTRDBContext context, IHostingEnvironment hostingEnvironment)
        {
            db = context;
            _hostingEnvironment = hostingEnvironment;

        }

        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //UserLog //UserLog;

        // GET: COM_MasterLC
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Index()
        {
            try
            {
                //var UserId = HttpContext.Session.GetString("userid");
                //var conString = db.Database.GetDbConnection().ConnectionString;

                //ReadExcelFile(UserId , conString);
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), "Index List Of Data View", "0", "View");

                //IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs.Include(c => c.BuyerInformations).Include(c => c.CommercialCompanies).Include(c => c.Currency).Include(c => c.Destinations).Include(c => c.LienBank).Include(c => c.OpeningBank).Include(c => c.PortOfLoading).Include(c => c.UnitMaster);
                //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());
                //HttpContext.Session.SetString("userid","12abcd");




                //if (Roles.IsUserInRole("Admin"))
                //{
                //    IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs;
                //    return View(cOM_MasterLC.ToList());
                //}
                //else
                //{
                //    IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs.Where(x => x.userid == UserId.ToString());
                //    return View(cOM_MasterLC.ToList());

                //}
                if (User.IsInRole("Admin"))
                {
                    IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs.Include(m => m.COM_MasterLC_Details).Include(m => m.BuyerInformations).Include(m => m.CommercialCompanies).Include(m => m.COM_GroupLC_Subs).Include(m => m.COM_MasterLCExport).Include(m => m.ExportInvoiceMasters);
                    return View(cOM_MasterLC.ToList());
                }
                else
                {
                    IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs.Include(m => m.COM_MasterLC_Details).Include(m => m.BuyerInformations).Include(m => m.CommercialCompanies).Include(m => m.COM_GroupLC_Subs).Include(m => m.COM_MasterLCExport).Include(m => m.ExportInvoiceMasters);

                    //IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs.Where(x => x.userid == UserId.ToString());
                    //return View(cOM_MasterLC.ToList());
                    return View(cOM_MasterLC.ToList());

                }
                //.Include(c => c.BuyerInformations).Include(c => c.CommercialCompanies).Include(c => c.Currency).Include(c => c.Destinations).Include(c => c.LienBank).Include(c => c.OpeningBank).Include(c => c.PortOfLoading).Include(c => c.UnitMaster);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult SalesContactMasterLC()
        {
            //IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs.Include(c => c.BuyerInformations).Include(c => c.CommercialCompanies).Include(c => c.Currency).Include(c => c.Destinations).Include(c => c.LienBank).Include(c => c.OpeningBank).Include(c => c.PortOfLoading).Include(c => c.UnitMaster);

            IQueryable<COM_MasterLC> cOM_MasterLC = db.COM_MasterLCs;
            //.Include(c => c.BuyerInformations).Include(c => c.CommercialCompanies).Include(c => c.Currency).Include(c => c.Destinations).Include(c => c.LienBank).Include(c => c.OpeningBank).Include(c => c.PortOfLoading).Include(c => c.UnitMaster);

            //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), "SalesContactMasterLC List Of Data View", "0", "View");

            return View(cOM_MasterLC.ToList());
        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult ShipmentScheduleIndex(int? BuyerGroupId, string FromDate, string ToDate)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);

            }
            //ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName");

            if (BuyerGroupId == null)
            {
                BuyerGroupId = 0;
            }
            // return View(db.ExportOrders.Where(p => (p.ShipDate >= dtFrom && p.ShipDate <= dtTo) && (p.StyleID.ToString() == supplierid.ToString()) && p.isDelete == false).ToList()); //p.ComId == HttpContext.Session.GetString("comid") && 

            List<COM_MasterLC_Details> COM_MasterLC_Details = db.COM_MasterLC_Detailss.Where(p => (p.COM_MasterLC.BuyerGroupID.ToString() == BuyerGroupId.ToString())).OrderBy(x => x.ShipmentDate).ToList();

            return View(COM_MasterLC_Details); //p.ComId == HttpContext.Session.GetString("comid") && 

        }



        public ActionResult LCMarginWithAvailableBalance()
        {
            //himu
            //List<LCMarginWithAvailableLCBalance> ProductSerialresult = (db.Database.SqlQuery<LCMarginWithAvailableLCBalance>("[SP_COM_LCMarginWithAvailableLCBalance]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            //SqlParameter[] sqlParameter = new SqlParameter[] { comid,userid };

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@p0", HttpContext.Session.GetString("comid"));
            sqlParameter[1] = new SqlParameter("@p1", HttpContext.Session.GetString("useid"));
            List<LCMarginWithAvailableLCBalance> ProductSerialresult = Helper.ExecProcMapTList<LCMarginWithAvailableLCBalance>("SP_COM_LCMarginWithAvailableLCBalance", sqlParameter);
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.LCMarginWithAvailableLCBalance = ProductSerialresult;


            return View(); //p.ComId == HttpContext.Session.GetString("comid") && 
        }



        public class LCMarginWithAvailableLCBalance
        {
            public string GroupLCRefName { get; set; }
            public int TotalMasterLC { get; set; }
            public decimal TotalGroupLCValue { get; set; }
            public int BBLCCount { get; set; }
            public decimal TotalBBLCValue { get; set; }
            public decimal Balance { get; set; }
            public decimal Margin { get; set; }

        }

        public JsonResult BuyerInfo(int? id)
        {
            // check it
            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;


            BuyerInformation data = db.BuyerInformation.Where(m => m.BuyerId == id).FirstOrDefault();

            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConcernOpeningBankAccountList(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;


            //BankAccountNo data = db.BankAccountNos.Where(m => m.CommercialCompanyId == id).FirstOrDefault();


            //return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);


            List<BankAccountNo> BankAccountNos = db.BankAccountNos.Where(m => m.CommercialCompanyId == id).ToList();

            List<SelectListItem> bangkacclist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (BankAccountNos != null)
            {
                foreach (BankAccountNo x in BankAccountNos)
                {
                    bangkacclist.Add(new SelectListItem { Text = x.BankAccountNumber, Value = x.BankAccountId.ToString() });
                }
            }
            //return Json(new SelectList(bangkacclist, "Value", "Text", JsonRequestBehavior.AllowGet));
            return Json(new SelectList(bangkacclist, "Value", "Text"));

        }

        public JsonResult SupplierBankList(int? id)
        {




            List<BankAccountNoLienBank> BankAccountNoLienBanks = db.BankAccountNoLienBanks.Where(m => m.SupplierId == id).ToList();

            List<SelectListItem> supplierbankaccount = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (BankAccountNoLienBanks != null)
            {
                foreach (BankAccountNoLienBank x in BankAccountNoLienBanks)
                {
                    supplierbankaccount.Add(new SelectListItem { Text = x.BankAccountNumber, Value = x.LienBankAccountId.ToString() });
                }
            }
            //return Json(new SelectList(supplierbankaccount, "Value", "Text", JsonRequestBehavior.AllowGet));
            return Json(new SelectList(supplierbankaccount, "Value", "Text"));

        }


        public JsonResult ConcernOpeningBankList(int? id)
        {

            BankAccountNo BankAccountNo = db.BankAccountNos.Where(x => x.BankAccountId == id).FirstOrDefault();


            List<OpeningBank> OpeningBanks = db.OpeningBanks.Where(m => m.OpeningBankId == BankAccountNo.OpeningBankId).ToList();

            List<SelectListItem> openingbanklist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (OpeningBanks != null)
            {
                foreach (OpeningBank x in OpeningBanks)
                {
                    openingbanklist.Add(new SelectListItem { Text = x.OpeningBankName, Value = x.OpeningBankId.ToString() });
                }
            }
            //return Json(new SelectList(openingbanklist, "Value", "Text", JsonRequestBehavior.AllowGet));
            return Json(new SelectList(openingbanklist, "Value", "Text"));

        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult Download(string file)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "Download\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }
        // GET: COM_MasterLC/Create
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Create(int? BuyerID, int? Flag)
        {
            try
            {
                ViewBag.Title = "Create";
                if (Flag == 1)
                {

                    #region excelupload
                    //var userid = HttpContext.Session.GetString("userid");
                    var userid = HttpContext.Session.GetString("userid");

                    if (userid.ToString() == "" || userid == null)
                    {
                        return BadRequest();
                    }


                    List<Temp_COM_MasterLC_Detail> Import = db.Temp_COM_MasterLC_Details.Where(m => m.userid == userid.ToString()).ToList();
                    Temp_COM_MasterLC_Master ImportMaster = db.Temp_COM_MasterLC_Masters.Where(m => m.userid == userid.ToString()).FirstOrDefault();

                    int openingbankid = 0;
                    OpeningBank openingbank = db.OpeningBanks.Where(m => m.OpeningBankName.ToUpper().Contains(ImportMaster.OpeningBank)).FirstOrDefault();
                    if (openingbank != null)
                    {
                        openingbankid = openingbank.OpeningBankId;
                    }
                    int bankaccountid = 0;
                    BankAccountNo bankaccountno = db.BankAccountNos.Where(m => m.BankAccountNumber.ToUpper().Contains(ImportMaster.BankAccountNo)).FirstOrDefault();
                    if (bankaccountno != null)
                    {
                        bankaccountid = bankaccountno.BankAccountId;
                    }

                    int lienbankid = 0;
                    LienBank lienbank = db.LienBanks.Where(m => m.LienBankName.ToUpper().Contains(ImportMaster.ConsigneeBank)).FirstOrDefault();
                    if (lienbank != null)
                    {
                        lienbankid = lienbank.LienBankId;
                    }

                    int CommercialCompanyId = 0;
                    SisterConcernCompany SisterConcernCompany = db.SisterConcernCompany.Where(m => m.CompanyName.ToUpper().Contains(ImportMaster.Company)).FirstOrDefault();
                    if (SisterConcernCompany != null)
                    {
                        CommercialCompanyId = SisterConcernCompany.SisterConcernCompanyId;
                    }

                    //int destinationid = 0;
                    //Destination destination = db.Destinations.Where(m => m.DestinationName.ToUpper().Contains(ImportMaster.PortOfDestination)).FirstOrDefault();
                    //if (destination != null)
                    //{
                    //    destinationid = destination.DestinationID;
                    //}

                    int supplierid = 0;
                    SupplierInformation supplier = db.SupplierInformations.Where(m => m.SupplierName.ToUpper().Contains(ImportMaster.ShippingAgent)).FirstOrDefault();
                    if (supplier != null)
                    {
                        supplierid = supplier.ContactID;
                    }

                    int buyerid = 0;
                    BuyerInformation buyer = db.BuyerInformation.Where(m => m.BuyerName.ToUpper().Contains(ImportMaster.Buyer)).FirstOrDefault();
                    if (buyer != null)
                    {
                        buyerid = buyer.BuyerId;
                    }
                    int buyergroupid = 0;
                    BuyerGroup buyergroup = db.BuyerGroups.Where(m => m.BuyerGroupName.ToUpper().Contains(ImportMaster.Buyer.ToUpper())).FirstOrDefault();
                    if (buyergroup != null)
                    {
                        buyergroupid = buyergroup.BuyerGroupId;
                    }

                    int currencyid = 0;
                    Currency currency = db.Currency.Where(m => m.CurCode.ToUpper().Contains(ImportMaster.Curency)).FirstOrDefault();
                    if (currency != null)
                    {
                        currencyid = currency.CurrencyId;
                    }

                    int portofloadingid = 0;
                    PortOfLoading portofloading = db.PortOfLoadings.Where(m => m.PortOfLoadingName.ToUpper().Contains(ImportMaster.PortOfLoading)).FirstOrDefault();
                    if (portofloading != null)
                    {
                        portofloadingid = portofloading.PortOfLoadingId;
                    }

                    int portofdischargeid = 0;
                    PortOfDischarge portofdischarge = db.PortOfDischarges.Where(m => m.PortOfDischargeName.ToUpper().Contains(ImportMaster.PortOfDischarge)).FirstOrDefault();
                    if (portofdischarge != null)
                    {
                        portofdischargeid = portofdischarge.PortOfDischargeId;
                    }


                    int portofdestinationid = 0;
                    Destination portofdestination = db.Destinations.Where(m => m.DestinationName.ToUpper().Contains(ImportMaster.PortOfDestination)).FirstOrDefault();
                    if (portofdestination != null)
                    {
                        portofdestinationid = portofdestination.DestinationID;
                    }


                    int tolerance = 0;
                    if (ImportMaster.Tolerance != null)
                    {
                        tolerance = int.Parse(ImportMaster.Tolerance);
                    }

                    int tenor = 0;
                    if (ImportMaster.Tolerance != null)
                    {
                        tenor = int.Parse(ImportMaster.Tolerance);
                    }



                    //var openingbangkid = db.OpeningBanks.Where(m => m.OpeningBankName.ToUpper().Contains(ImportMaster.OpeningBank));
                    //var BuyerID = db.OpeningBanks.Where(m => m.OpeningBankName.ToUpper().Contains(ImportMaster.OpeningBank));

                    COM_MasterLC Importmaster = new COM_MasterLC
                    {
                        MasterLCID = 0,
                        LCRefNo = ImportMaster.Contract.ToString(),
                        BuyerLCRef = ImportMaster.Contract.ToString(),
                        CommercialCompanyId = CommercialCompanyId,

                        LCOpenDate = DateTime.Parse(ImportMaster.LCIssueDate.ToString()),
                        //DateTime.Now.Date, //ImportMaster.Contract.ToString(),
                        LCExpirydate = DateTime.Now.Date,  //DateTime.Parse(ImportMaster.LastShipDate.ToString()),//
                        BuyerID = buyerid,
                        BuyerGroupID = buyergroupid,
                        OpeningBankId = openingbankid,
                        BankAccountId = bankaccountid,
                        LienBankId = lienbankid,
                        TotalLCQty = int.Parse(ImportMaster.LCQty ?? "0"), /////now coming null
                        UnitMasterId = "Pcs",
                        LCValue = decimal.Parse(ImportMaster.LCValue),
                        CurrencyId = currencyid,
                        Tenor = tenor,
                        DestinationId = portofdestinationid,
                        DestinationContract = ImportMaster.PortOfDestination,
                        PortOfLoadingId = portofloadingid,
                        PortOfDischargeId = portofdischargeid,


                        Addedby = "fhd",
                        DateAdded = DateTime.Now,
                        Updatedby = "fhd",
                        DateUpdated = DateTime.Now,
                        userid = HttpContext.Session.GetString("userid"),
                        comid = (HttpContext.Session.GetString("comid")),
                        isDelete = false,

                        LCTypeId = 1,
                        LCStatusId = 1,

                        LCNatureId = 1,
                        TradeTermId = 1,
                        DayListId = 1,

                        Tolerance = tenor,
                        SupplierId = supplierid,

                        Insurance = ImportMaster.Insurance,
                        AccountNo = ImportMaster.AccountNo,
                        FirstShipmentDate = ImportMaster.LCIssueDate,
                        LastShipmentDate = ImportMaster.LastShipDate,

                        RemarksOne = ImportMaster.Remarks1,
                        RemarksTwo = ImportMaster.Remarks2,
                        RemarksThree = ImportMaster.Remarks3,
                        RemarksFour = ImportMaster.Remarks4 ?? "",
                        RemarksFive = ImportMaster.Remarks5 ?? ""



                    };
                    Importmaster.COM_MasterLC_Details = new List<COM_MasterLC_Details>();
                    foreach (Temp_COM_MasterLC_Detail item in Import)
                    {
                        COM_MasterLC_Details COM_CNFBillImportDetail = new COM_MasterLC_Details
                        {
                            MasterLCID = 0,
                            ExportPONo = item.PONo,
                            StyleName = item.Style,
                            ItemName = item.ItemName,
                            HSCode = item.HSCode,


                            CatNo = item.CatNo,
                            ContractNo = item.ContractNo,
                            OrderType = item.OrderType,
                            DeliveryNo = item.DeliveryNo,

                            DestinationPO = item.DestinationPO,
                            Kimball = item.Kimball,
                            ColorCode = item.ColorCode,

                            Fabrication = item.Fabrication,
                            OrderQty = item.OrderQty,
                            UnitMasterId = item.OrderUOM, //"Pcs"
                                                          // COM_CNFBillImportDetail.UnitMaster.UnitName = "Pcs";//item.OrderUOM;


                            Factor = item.Factor,
                            QtyInPcs = item.QuantityPcs,
                            //COM_CNFBillImportDetail.UnitMasterId = item.UOM;

                            UnitPrice = decimal.Parse(item.UnitPrice.ToString()),
                            TotalValue = decimal.Parse(item.TotalValue.ToString()), ///item.UnitPrice * item.OrderQty,
                            ShipmentDate = (DateTime.Parse(item.ShipDate.ToString()).Date),// if (item.ShipDate == null) { return DateTime.Now.Date; } else { return item.ShipDate; };
                            ContractDate = (DateTime.Parse(item.ContractDate.ToString()).Date),// if (item.ShipDate == null) { return DateTime.Now.Date; } else { return item.ShipDate; };
                            SL = item.SL,
                            Destination = item.Destination
                        };

                        //COM_CNFBillImportDetail.COM_CNFExpanseTypes = new COM_CNFExpanseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };
                        Importmaster.COM_MasterLC_Details.Add(COM_CNFBillImportDetail);
                    }

                    #endregion


                    ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", Importmaster.BuyerID);
                    ViewBag.BuyerGroupID = new SelectList(db.BuyerGroups, "BuyerGroupID", "BuyerGroupName", Importmaster.BuyerGroupID);

                    ViewBag.CommercialCompanyId = new SelectList(db.SisterConcernCompany, "CommercialCompanyId", "CompanyName", Importmaster.CommercialCompanyId);
                    ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", Importmaster.CurrencyId);
                    ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", Importmaster.DestinationId);
                    ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", Importmaster.LienBankId);
                    ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", Importmaster.OpeningBankId);
                    ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", Importmaster.BankAccountId);

                    ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", Importmaster.PortOfLoadingId);
                    ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", Importmaster.PortOfDischargeId);

                    ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", Importmaster.UnitMasterId);

                    ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", Importmaster.TradeTermId);
                    ViewBag.LCNatureId = new SelectList(db.LCNatures, "LCNatureId", "LCNatureName", Importmaster.LCNatureId);
                    ViewBag.LCTypeId = new SelectList(db.LCTypes, "LCTypeId", "LCTypeName", Importmaster.LCTypeId);

                    ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", Importmaster.ShipModeId);
                    ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", Importmaster.PaymentTermsId);
                    ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", Importmaster.DayListId);
                    ViewBag.LCStatusId = new SelectList(db.LCStatus, "LCStatusId", "LCStatusName", Importmaster.LCStatusId);


                    if (BuyerID == null)
                    {
                        BuyerID = 0;
                    }

                    ViewBag.exportorder = (from exportorder in db.ExportOrders.Where(m => m.comid == HttpContext.Session.GetString("comid"))
                                           join s in db.StyleInformation
                                           on exportorder.StyleID equals s.StyleId
                                           where s.BuyerId == BuyerID
                                           && !db.COM_MasterLCExports.Any(f => f.ExportOrderID == exportorder.ExportOrderID && exportorder.comid == HttpContext.Session.GetString("comid"))
                                           select exportorder).Distinct().ToList();

                    return View("Create", Importmaster);

                }
                else
                {


                    ViewBag.BuyerID = BuyerID;
                    if (BuyerID == null)
                    {
                        BuyerID = 0;
                    }

                    ViewBag.exportorder = (from exportorder in db.ExportOrders.Where(m => m.comid == HttpContext.Session.GetString("comid"))
                                           join s in db.StyleInformation
                                           on exportorder.StyleID equals s.StyleId
                                           where s.BuyerId == BuyerID
                                           && !db.COM_MasterLCExports.Any(f => f.ExportOrderID == exportorder.ExportOrderID && exportorder.comid == HttpContext.Session.GetString("comid"))
                                           select exportorder).Distinct().ToList();

                    //ViewBag.exportorder = (from exportorder in db.ExportOrders.Where(m => m.comid == HttpContext.Session.GetString("comid"))
                    //                       join s in db.StyleInformation
                    //                       on exportorder.StyleID equals s.StyleId
                    //                       where s.BuyerId == BuyerID
                    //                       && !db.COM_MasterLCExports.Any(f => f.ExportOrderID == exportorder.ExportOrderID && exportorder.comid == HttpContext.Session.GetString("comid"))
                    //                       select exportorder).ToList();

                    //var Import = (from exportorder in db.ExportOrders.Where(m =>  m.comid == HttpContext.Session.GetString("comid"))
                    //              where !db.COM_MasterLCExports.Any(f => f.ExportOrderID == exportorder.ExportOrderID && exportorder.comid == HttpContext.Session.GetString("comid"))
                    //              select exportorder).ToList();

                    var data = db.BuyerGroups.ToList();
                    ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");

                    ViewBag.BuyerGroupID = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName");

                    ViewBag.CommercialCompanyId = new SelectList(db.SisterConcernCompany, "CommercialCompanyId", "CompanyName");
                    ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
                    ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch");
                    ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName");
                    ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");
                    ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber");

                    ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName");
                    ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName");

                    ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId");

                    ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName");
                    ViewBag.LCNatureId = new SelectList(db.LCNatures, "LCNatureId", "LCNatureName");
                    ViewBag.LCTypeId = new SelectList(db.LCTypes, "LCTypeId", "LCTypeName");

                    ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName");
                    ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName");
                    ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName");
                    ViewBag.LCStatusId = new SelectList(db.LCStatus, "LCStatusId", "LCStatusName");
                }

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //public class ValidateAjaxAttribute : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        if (!filterContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            return;
        //        }

        //        ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;
        //        if (!modelState.IsValid)
        //        {
        //            var errorModel =
        //                    from x in modelState.Keys
        //                    where modelState[x].Errors.Count > 0
        //                    select new
        //                    {
        //                        key = x,
        //                        errors = modelState[x].Errors.
        //                                                      Select(y => y.ErrorMessage).
        //                                                      ToArray()
        //                    };
        //            filterContext.Result = new JsonResult()
        //            {
        //                Data = errorModel
        //            };
        //            filterContext.HttpContext.Response.StatusCode =
        //                                                  (int)HttpStatusCode.BadRequest;
        //        }
        //    }
        //}
        // POST: COM_MasterLC/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        [HttpPost]
        //[ValidateAjax]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(COM_MasterLC cOM_MasterLC) //[Bind(/*Include =*/ "MasterLCID, LCRefNo,BuyerLCRef,CommercialCompanyId,LCOpenDate,LCExpirydate,BuyerID,OpeningBankId,LienBankId,TotalLCQty,UnitMasterId,LCValue,CurrencyId,Tenor,DestinationId,Addedby,DateAdded,Updatedby,DateUpdated,userid,comid,isDelete,PortOfLoadingId,LCTypeId,LCStatusId,LCNatureId,TradeTermId,ShipModeId,PaymentTermsId,DayListId")] 
        {
            try
            {
                if (HttpContext.Session.GetString("comid") == "0")
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }

                //Mastersmain.VoucherInputDate = DateTime.Now.Date;



                //if (ModelState.IsValid)
                //{

                //    if (cOM_MasterLC.MasterLCID > 0)
                //    {
                //        cOM_MasterLC.DateUpdated = DateTime.Now;
                //        cOM_MasterLC.DateAdded = DateTime.Now;

                //        //db.Entry(cOM_MasterLC).State = EntityState.Modified;
                //        //db.SaveChanges();

                //        IQueryable<COM_MasterLCExport> currentexportlc = db.COM_MasterLCExports.Where(p => p.MasterLCID == cOM_MasterLC.MasterLCID);

                //        foreach (COM_MasterLCExport ss in currentexportlc)
                //        {
                //            db.COM_MasterLCExports.Remove(ss);
                //        }

                //        foreach (COM_MasterLCExport ss in cOM_MasterLC.COM_MasterLCExport)
                //        {
                //            ss.DateAdded = DateTime.Now;
                //            ss.DateUpdated = DateTime.Now;

                //            //db.VoucherSubs.Add(ss);
                //            db.COM_MasterLCExports.Add(ss);



                //        }


                //        IQueryable<COM_MasterLC_Details> currentdeailslc = db.COM_MasterLC_Detailss.Where(p => p.MasterLCID == cOM_MasterLC.MasterLCID);

                //        foreach (COM_MasterLC_Details ss in currentdeailslc)
                //        {
                //            db.COM_MasterLC_Detailss.Remove(ss);
                //        }

                //        foreach (COM_MasterLC_Details ss in cOM_MasterLC.COM_MasterLC_Details)
                //        {
                //            ss.DateAdded = DateTime.Now;
                //            ss.DateUpdated = DateTime.Now;

                //            //db.VoucherSubs.Add(ss);
                //            db.COM_MasterLC_Detailss.Add(ss);



                //        }
                //        db.Entry(cOM_MasterLC).State = EntityState.Modified;

                //    }
                //    else
                //    {
                //        cOM_MasterLC.DateAdded = DateTime.Now;
                //        cOM_MasterLC.DateUpdated = DateTime.Now;
                //        cOM_MasterLC.comid = int.Parse(Session["comid"].ToString());
                //        cOM_MasterLC.userid = HttpContext.Session.GetString("userid");

                //        foreach (COM_MasterLCExport ss in cOM_MasterLC.COM_MasterLCExport)
                //        {
                //            ss.DateAdded = DateTime.Now;
                //            ss.DateUpdated = DateTime.Now;

                //            //db.VoucherSubs.Add(ss);
                //            //db.COM_MasterLCExports.Add(ss);



                //        }

                //        foreach (COM_MasterLC_Details ss in cOM_MasterLC.COM_MasterLC_Details)
                //        {
                //            ss.DateAdded = DateTime.Now;
                //            ss.DateUpdated = DateTime.Now;

                //            //db.VoucherSubs.Add(ss);
                //            //db.COM_MasterLCExports.Add(ss);



                //        }

                //        db.COM_MasterLCs.Add(cOM_MasterLC);
                //    }

                //    db.SaveChanges();

                //    return Json(new { Success = 1, MasterLCID = cOM_MasterLC.MasterLCID, ex = "" });

                //    //return RedirectToAction("Index");
                //}

                var errors = ModelState.Where(x => x.Value.Errors.Any())
               .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    if (cOM_MasterLC.MasterLCID > 0)
                    {
                        cOM_MasterLC.DateAdded = DateTime.Now;
                        cOM_MasterLC.DateUpdated = DateTime.Now;
                        cOM_MasterLC.comid = (HttpContext.Session.GetString("comid"));

                        if (cOM_MasterLC.userid == null)
                        {
                            cOM_MasterLC.userid = HttpContext.Session.GetString("userid");
                        }
                        cOM_MasterLC.useridUpdate = HttpContext.Session.GetString("userid");





                        foreach (var item in cOM_MasterLC.COM_MasterLC_Details)
                        {

                            if (item.MasterLCDetailsID > 0)
                            {
                                if (item.isDelete == true)
                                {
                                    var z = item.MasterLCDetailsID;
                                    db.Entry(item).State = EntityState.Deleted;
                                    //db.COM_MasterLC_Detailss.Remove(item);

                                }
                                else
                                {
                                    item.useridUpdate = HttpContext.Session.GetString("userid");
                                    db.Entry(item).State = EntityState.Modified;
                                }



                            }
                            else
                            {
                                if (item.isDelete == true)
                                {
                                }
                                else
                                {
                                    item.userid = HttpContext.Session.GetString("userid");
                                    db.Entry(item).State = EntityState.Added;

                                    //db.COM_MasterLC_Detailss.Add(item);

                                }
                            }
                            //db.SaveChanges();

                        }
                        db.SaveChanges();
                        db.Entry(cOM_MasterLC).State = EntityState.Modified;



                        TempData["Message"] = "Data Update Successfully";

                        db.SaveChanges();
                        TempData["Status"] = "2";

                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cOM_MasterLC.MasterLCID.ToString(), "Update");


                        //return Json(new { Success = 1, MasterLCId = cOM_MasterLC.MasterLCID, ex = TempData["Message"].ToString() });
                    }
                    else
                    {
                        cOM_MasterLC.DateAdded = DateTime.Now;
                        cOM_MasterLC.DateUpdated = DateTime.Now;
                        cOM_MasterLC.comid = (HttpContext.Session.GetString("comid"));
                        cOM_MasterLC.userid = HttpContext.Session.GetString("userid");



                        db.COM_MasterLCs.Add(cOM_MasterLC);

                        TempData["Message"] = "Data Save Successfully";

                        db.SaveChanges();
                        TempData["Status"] = "1";

                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cOM_MasterLC.MasterLCID.ToString(), "Save");

                        //return Json(new { Success = 1, MasterLCId = cOM_MasterLC.MasterLCID, ex = TempData["Message"].ToString() });

                    }


                    //JsonResult x = await Create();

                    return Json(new { Success = 1, MasterLCId = cOM_MasterLC.MasterLCID, ex = TempData["Message"].ToString() });

                    // return RedirectToAction("Index");
                }
                else
                {
                    return Json(new { Success = 0, MasterLCId = cOM_MasterLC.MasterLCID, ex = "Unable to Save / Update" });

                }


                //ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
                //ViewBag.CommercialCompanyId = new SelectList(db.SisterConcernCompany, "CommercialCompanyId", "CompanyName");
                //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
                //ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch");
                //ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName");
                //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");
                //ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName");
                //ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitName");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errors = ex.InnerException.InnerException.Message
                    //ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                //return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            //return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });

        }

        // GET: COM_MasterLC/Edit/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("comid") == "0")
            {
                return NotFound();

            }

            ViewBag.Title = "Edit";

            if (id == null)
            {
                return BadRequest();
            }
            COM_MasterLC cOM_MasterLC = db.COM_MasterLCs.Include(m => m.COM_MasterLC_Details).Where(m => m.MasterLCID.ToString() == id.ToString()).FirstOrDefault();
            //COM_MasterLC cOM_MasterLC = db.COM_MasterLCs.Find(id);
            if (cOM_MasterLC == null)
            {
                return NotFound();
            }



            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", cOM_MasterLC.BuyerID);
            ViewBag.BuyerGroupID = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName", cOM_MasterLC.BuyerGroupID);

            ViewBag.CommercialCompanyId = new SelectList(db.SisterConcernCompany, "CommercialCompanyId", "CompanyName", cOM_MasterLC.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_MasterLC.CurrencyId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_MasterLC.DestinationId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_MasterLC.LienBankId);
            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_MasterLC.OpeningBankId);
            ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", cOM_MasterLC.BankAccountId);

            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_MasterLC.PortOfLoadingId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", cOM_MasterLC.PortOfDischargeId);

            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_MasterLC.UnitMasterId);


            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_MasterLC.TradeTermId);
            ViewBag.LCNatureId = new SelectList(db.LCNatures, "LCNatureId", "LCNatureName", cOM_MasterLC.LCNatureId);
            ViewBag.LCTypeId = new SelectList(db.LCTypes, "LCTypeId", "LCTypeName", cOM_MasterLC.LCTypeId);


            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_MasterLC.TradeTermId);
            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_MasterLC.TradeTermId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_MasterLC.TradeTermId);
            ViewBag.LCStatusId = new SelectList(db.LCStatus, "LCStatusId", "LCStatusName", cOM_MasterLC.TradeTermId);

            //ViewBag.exportorder = (from e in db.ExportOrders
            //                       join s in db.StyleInformation
            //                       on e.StyleID equals s.StyleId
            //                       where s.BuyerId == cOM_MasterLC.BuyerID
            //                       select e).ToList();
            ///Session["comid"].ToString()
            ///
            ViewBag.exportorder = (from exportorder in db.ExportOrders.Where(m => m.comid == HttpContext.Session.GetString("comid"))
                                   join s in db.StyleInformation
                                   on exportorder.StyleID equals s.StyleId
                                   where s.BuyerId == cOM_MasterLC.BuyerID
                                   && !db.COM_MasterLCExports.Any(f => f.ExportOrderID == exportorder.ExportOrderID && exportorder.comid == HttpContext.Session.GetString("comid"))
                                   select exportorder).ToList();

            return View("Create", cOM_MasterLC);
        }

        // POST: COM_MasterLC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(/*Include =*/ "MasterLCID,LCRefNo,LCType,BuyerLCRef,LCOpenDate,LCExpirydate,TotalLCQty,UnitMasterId,LCValue,LCStatus,LCNature,Tenor,TradeTerm,Addedby,DateAdded,Updatedby,DateUpdated,userid,comid,isDelete,BuyerID,DestinationId,CommercialCompanyId,CurrencyId,OpeningBankId,LienBankId,PortOfLoadingId")] COM_MasterLC cOM_MasterLC)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cOM_MasterLC).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", cOM_MasterLC.BuyerID);
        //    ViewBag.CommercialCompanyId = new SelectList(db.SisterConcernCompany, "CommercialCompanyId", "CompanyName", cOM_MasterLC.CommercialCompanyId);
        //    ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_MasterLC.CurrencyId);
        //    ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_MasterLC.DestinationId);
        //    ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_MasterLC.LienBankId);
        //    ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_MasterLC.OpeningBankId);
        //ViewBag.BankAccId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", cOM_MasterLC.BankAccountId);

        //    ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_MasterLC.PortOfLoadingId);
        //    ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", cOM_MasterLC.PortOfDischargeId);

        //    ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_MasterLC.UnitMasterId);


        //    ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_MasterLC.TradeTermId);
        //    ViewBag.LCNatureId = new SelectList(db.LCNatures, "LCNatureId", "LCNatureName", cOM_MasterLC.LCNatureId);
        //    ViewBag.LCTypeId = new SelectList(db.LCTypes, "LCTypeId", "LCTypeName", cOM_MasterLC.LCTypeId);


        //    return View(cOM_MasterLC);
        //}

        /// /for excel row convert into a column
        /// </summary>
        /// <returns></returns>

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        [HttpPost]
        [Obsolete]
        public ActionResult UploadFiles(IList<IFormFile> fileData)
        {
            try
            {

                #region excelupload
                //var userid = HttpContext.Session.GetString("userid");
                var userid = HttpContext.Session.GetString("userid");
                var conString = db.Database.GetDbConnection().ConnectionString;


                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }



                IList<IFormFile> files = HttpContext.Request.Form.Files.ToList();
                //string filePath=string.Empty ;

                //var upload = Path.Combine("C:\\D drive");
                foreach (IFormFile file in files)
                {
                    //string upload = Path.Combine("~/Content/Upload/");
                    //string uploadlocation = Path.Combine("Content/Upload/");
                    string uploadlocation = Path.GetFullPath("Content/Upload/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();


                    //if (file.Length > 0)
                    //{
                    //    var filePath = Path.Combine(upload, file.FileName);

                    //    var fileStream = new FileStream(filePath, FileMode.Create);
                    //    file.CopyTo(fileStream);


                    //    string extension = Path.GetExtension(file.FileName);

                    ReadExcelFile(userid, conString, filePath);

                    //}
                }


                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;



                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }


        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        [HttpPost]
        public ActionResult UploadFilesPOList()
        {
            try
            {

                #region excelupload
                //var userid = HttpContext.Session.GetString("userid");
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }

                //if (Request.Files.Count > 0)
                //{
                //    HttpFileCollectionBase files = Request.Files;

                //    for (int i = 0; i < files.Count; i++)
                //    {
                //        HttpPostedFileBase postedFile = files[i];

                //        string filePath = string.Empty;
                //        if (postedFile != null)
                //        {
                //            string path = Server.MapPath("~/Content/Upload/");
                //            if (!Directory.Exists(path))
                //            {
                //                Directory.CreateDirectory(path);
                //            }

                //            filePath = path + Path.GetFileName(postedFile.FileName);
                //            string extension = Path.GetExtension(postedFile.FileName);
                //            postedFile.SaveAs(filePath);

                //            string conString = string.Empty;
                //            switch (extension)
                //            {
                //                case ".xls": //Excel 97-03.
                //                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                //                    break;
                //                case ".xlsx": //Excel 07 and above.
                //                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                //                    break;
                //            }

                //            DataTable dt0 = new DataTable();
                //            DataTable dt1 = new DataTable();

                //            conString = string.Format(conString, filePath);

                //            using (OleDbConnection connExcel = new OleDbConnection(conString))
                //            {
                //                using (OleDbCommand cmdExcel = new OleDbCommand())
                //                {
                //                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                //                    {
                //                        cmdExcel.Connection = connExcel;

                //                        //Get the name of First Sheet.
                //                        connExcel.Open();

                //                        //dt.Load(cmdExcel.ExecuteReader());

                //                        DataTable dtExcelSchema;// DataTable mySheets = myConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" }); 
                //                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //                        DataTable dtExcelSchemaFinal = new DataTable();



                //                        dtExcelSchemaFinal = dtExcelSchema.Clone();

                //                        foreach (DataRow drtableOld in dtExcelSchema.Rows)
                //                        {
                //                            string strSheetTableName = drtableOld["TABLE_NAME"].ToString();

                //                            if (strSheetTableName.Contains("Filter"))

                //                            {

                //                            }
                //                            else
                //                            {
                //                                dtExcelSchemaFinal.ImportRow(drtableOld);
                //                            }
                //                        }




                //                        string sheetName0 = dtExcelSchemaFinal.Rows[0]["TABLE_NAME"].ToString();
                //                        string sheetName1 = dtExcelSchemaFinal.Rows[1]["TABLE_NAME"].ToString();

                //                        //connExcel.Close();

                //                        //Read Data from First Sheet.
                //                        //connExcel.Open();
                //                        cmdExcel.CommandText = "SELECT SL as Id,SL,*,'" + userid + "' as userid  From [" + sheetName0 + "] where len(pono) > 2";

                //                        odaExcel.SelectCommand = cmdExcel;
                //                        odaExcel.Fill(dt0);

                //                        //cmdExcel.CommandText = "SELECT SL as Id,SL,*,'" + userid + "' as userid  From [" + sheetName1 + "] where len(Field) > 2";
                //                        //odaExcel.SelectCommand = cmdExcel;
                //                        //odaExcel.Fill(dt1);

                //                        connExcel.Close();
                //                    }
                //                }
                //            }


                //            #region details ///details table function///

                //            string table_Details = "Temp_COM_MasterLC_Detail";
                //            string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //            SqlConnection con = new SqlConnection(connectionString);
                //            SqlCommand cmd = new SqlCommand("delete from dbo." + table_Details + " where userid   in ('" + userid + "', '')", con);
                //            con.Open();
                //            cmd.ExecuteNonQuery();
                //            //Response.Redirect("done.aspx");
                //            con.Close();



                //            conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //            using (SqlConnection conn = new SqlConnection(conString))
                //            {



                //                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                //                {
                //                    //bulkCopy.DestinationTableName = table;
                //                    DestinationTableName = "dbo." + table_Details // "+"_Temp
                //                };
                //                conn.Open();




                //                DataTable schema = conn.GetSchema("Columns", new[] { null, null, table_Details, null });
                //                foreach (DataColumn sourceColumn in dt0.Columns)
                //                {
                //                    foreach (DataRow row in schema.Rows)
                //                    {
                //                        if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                //                        {
                //                            bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                //                            break;
                //                        }
                //                        //bulkCopy.ColumnMappings.Add("userid", (string)row["COLUMN_NAME"]);
                //                    }
                //                }
                //                bulkCopy.WriteToServer(dt0);

                //                conn.Close();
                //            }
                //            #endregion

                //            ///details table function///
                //            //}

                //            #region Master //// master table function

                //            string table_Master = "Temp_COM_MasterLC_Master";
                //            //String connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //            //SqlConnection con = new SqlConnection(connectionString);
                //            cmd = new SqlCommand("delete from dbo." + table_Master + " where  userid is null or userid   in ('" + userid + "', '')", con);
                //            con.Open();
                //            cmd.ExecuteNonQuery();
                //            //Response.Redirect("done.aspx");
                //            con.Close();



                //            //conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //            //using (SqlConnection conn = new SqlConnection(conString))
                //            //{



                //            //    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                //            //    {
                //            //        //bulkCopy.DestinationTableName = table;
                //            //        DestinationTableName = "dbo." + table_Master // "+"_Temp
                //            //    };
                //            //    conn.Open();


                //            //    DataTable calculatetable = CustomTable(dt1);  ////for convert row into a column and save into a datatable

                //            //    DataTable schema = conn.GetSchema("Columns", new[] { null, null, table_Master, null });
                //            //    foreach (DataColumn sourceColumn in calculatetable.Columns)
                //            //    {
                //            //        foreach (DataRow row in schema.Rows)
                //            //        {
                //            //            if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                //            //            {
                //            //                bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                //            //                break;
                //            //            }
                //            //            //bulkCopy.ColumnMappings.Add("userid", (string)row["COLUMN_NAME"]);
                //            //        }
                //            //    }
                //            //    bulkCopy.WriteToServer(calculatetable);
                //            //}
                //            #endregion


                //        }

                //    }

                //}




                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;

                List<Temp_COM_MasterLC_Detail> data = db.Temp_COM_MasterLC_Details.Where(m => m.userid == userid.ToString()).ToList();
                // return Json(data, JsonRequestBehavior.AllowGet);
                return Json(data);

                //List<Temp_COM_MasterLC_Detail> data = db.Temp_COM_MasterLC_Details.Where(m => m.userid == userid.ToString()).ToList();
                //return Json(new { Success = 1, data, ex = "" });

                //var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcGetExcelUploadData]  @comid, @userid , @tablename ", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("tablename", tablename))).ToList();
                //return View("Create", Importmaster);
                //return RedirectToAction("Create");
                //return View(Importmaster);
                //return Json(new { Success = 1 ,  });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }

        // GET: COM_MasterLC/Delete/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Delete(int? id)
        {

            if (HttpContext.Session.GetString("comid") == "0")
            {
                return NotFound();

            }

            ViewBag.Title = "Delete";

            if (id == null)
            {
                return BadRequest();
            }
            COM_MasterLC cOM_MasterLC = db.COM_MasterLCs.Where(m => m.MasterLCID.ToString() == id.ToString()).FirstOrDefault();
            if (cOM_MasterLC == null)
            {
                return NotFound();
            }



            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", cOM_MasterLC.BuyerID);
            ViewBag.BuyerGroupID = new SelectList(db.BuyerGroups, "BuyerGroupID", "BuyerGroupName", cOM_MasterLC.BuyerGroupID);

            ViewBag.CommercialCompanyId = new SelectList(db.SisterConcernCompany, "CommercialCompanyId", "CompanyName", cOM_MasterLC.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_MasterLC.CurrencyId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_MasterLC.DestinationId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_MasterLC.LienBankId);
            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_MasterLC.OpeningBankId);
            ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", cOM_MasterLC.BankAccountId);

            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_MasterLC.PortOfLoadingId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", cOM_MasterLC.PortOfDischargeId);

            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_MasterLC.UnitMasterId);


            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_MasterLC.TradeTermId);
            ViewBag.LCNatureId = new SelectList(db.LCNatures, "LCNatureId", "LCNatureName", cOM_MasterLC.LCNatureId);
            ViewBag.LCTypeId = new SelectList(db.LCTypes, "LCTypeId", "LCTypeName", cOM_MasterLC.LCTypeId);


            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_MasterLC.TradeTermId);
            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_MasterLC.TradeTermId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_MasterLC.TradeTermId);
            ViewBag.LCStatusId = new SelectList(db.LCStatus, "LCStatusId", "LCStatusName", cOM_MasterLC.TradeTermId);

            //ViewBag.exportorder = (from e in db.ExportOrders
            //                       join s in db.StyleInformation
            //                       on e.StyleID equals s.StyleId
            //                       where s.BuyerId == cOM_MasterLC.BuyerID
            //                       select e).ToList();
            ///Session["comid"].ToString()
            ///
            ViewBag.exportorder = (from exportorder in db.ExportOrders.Where(m => m.comid == HttpContext.Session.GetString("comid"))
                                   join s in db.StyleInformation
                                   on exportorder.StyleID equals s.StyleId
                                   where s.BuyerId == cOM_MasterLC.BuyerID
                                   && !db.COM_MasterLCExports.Any(f => f.ExportOrderID == exportorder.ExportOrderID && exportorder.comid == HttpContext.Session.GetString("comid"))
                                   select exportorder).ToList();

            return View("Create", cOM_MasterLC);
        }

        // POST: COM_MasterLC/Delete/5
        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                COM_MasterLC cOM_MasterLC = db.COM_MasterLCs.Find(id);
                //db.COM_MasterLC_Detailss.RemoveRange(cOM_MasterLC.COM_MasterLC_Details);


                db.COM_MasterLCs.Remove(cOM_MasterLC);

                TempData["Message"] = "Data Delete Successfully";

                db.SaveChanges();
                TempData["Status"] = "3";

                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cOM_MasterLC.MasterLCID.ToString(), "Delete");


                return Json(new { Success = 1, TermsId = cOM_MasterLC.MasterLCID, ex = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }


        // [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintMLC(int? id, string type)
        {
            try
            {
                var dbcommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                var ReportPath = "CommercialReport/rptMasterLC.rdlc";
                var SqlCmd = "Exec  [rptMasterLC] '" + id + "','" + HttpContext.Session.GetString("comid") + "'";
                var ConstrName = dbcommercial;
                var ReportType = "PDF";

                return Redirect("https://localhost:44383/ReportViewer/GenerateReport?ReportPath=" + ReportPath + "&SqlCmd=" + SqlCmd + "&DbName=" + ConstrName + "&ReportType=" + ReportType);



                //string ReportPath = "~/Report/CommercialReport/rptMasterLC.rdlc";
                //string SQLQuery = "Exec  [rptMasterLC] '" + id + "','" + HttpContext.Session.GetString("comid") + "'";
                //string DataSourceName = "DataSet1";
                ////string FormCaption = "Report :: Sales Acknowledgement ...";


                ////postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec  rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                //HttpContext.Session.SetObject("rptList", postData);

                ////Common.Classes.clsMain.intHasSubReport = 0;
                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = SQLQuery;
                //clsReport.strDSNMain = DataSourceName;

                // return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult PrintMLCPI(int? id, string type)
        {
            try
            {
                var dbcommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptMasterLCWisePIList.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec  [rptMasterLCWisePIList] '" + id + "','" + HttpContext.Session.GetString("comid") + "'");

                string ReportPath = "~/Report/CommercialReport/rptMasterLCWisePIList.rdlc";
                string SQLQuery = "Exec  [rptMasterLCWisePIList] '" + id + "','" + HttpContext.Session.GetString("comid") + "'";
                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec  rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult PrintMLCWE(int? id, string type)
        {

            try
            {
                var dbcommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptMasterLCWiseExport.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec  [rptMasterLCWiseExport] '" + id + "','" + HttpContext.Session.GetString("comid") + "'");

                string ReportPath = "~/Report/CommercialReport/rptMasterLCWiseExport.rdlc";
                string SQLQuery = "Exec  [rptMasterLCWiseExport] '" + id + "','" + HttpContext.Session.GetString("comid") + "'";
                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec  rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult PrintMLCSC(int? id, string type)
        {
            try
            {

                var dbcommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                var ReportPath = "CommercialReport/rptMasterLCSalesContact.rdlc";
                var SqlCmd = "Exec  [rptMasterLCSalesContact] '" + 1 + "','" + HttpContext.Session.GetString("comid") + "'";
                var ConstrName = "ApplicationServices";
                var ReportType = "PDF";

                return Redirect("https://localhost:44383/ReportViewer/GenerateReport?ReportPath=" + ReportPath + "&SqlCmd=" + SqlCmd + "&DbName=" + ConstrName + "&ReportType=" + ReportType);


                //var dbcommercial = db.Database.GetDbConnection().Database;

                ////Session["rptList"] = null;
                //clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptMasterLCSalesContact.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec  [rptMasterLCSalesContact] '" + id + "','" + HttpContext.Session.GetString("comid") + "'");

                //string ReportPath = "~/Report/CommercialReport/rptMasterLCSalesContact.rdlc";
                //string SQLQuery = "Exec  [rptMasterLCSalesContact] '" + id + "','" + HttpContext.Session.GetString("comid") + "'";
                //string DataSourceName = "DataSet1";
                ////string FormCaption = "Report :: Sales Acknowledgement ...";


                ////postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec  rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                //HttpContext.Session.SetObject("rptList", postData);

                ////Common.Classes.clsMain.intHasSubReport = 0;
                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = SQLQuery;
                //clsReport.strDSNMain = DataSourceName;

                //return RedirectToActionPermanent("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {


            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var x = cell.StyleIndex;

            if (cell.CellValue == null) { return ""; }

            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                string a = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                return a;
            }
            else
            {
                return value;
            }
        }

        static void ReadExcelFile(string userid, string conString, string filepath)
        {
            try
            {


                DataTable dt0 = new DataTable();
                DataTable dt1 = new DataTable();


                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filepath, false))
                {

                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();


                    foreach (var everysheet in sheets)
                    {
                        string relationshipId = everysheet.Id.Value;
                        string SheetName = everysheet.Name;

                        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();


                        if (SheetName == "BuyerPO")
                        {
                            foreach (Cell cell in rows.ElementAt(0))
                            {

                                dt0.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                            }

                            int countcolumn = dt0.Columns.Count;
                            dt0.Columns.Add("userid");



                            foreach (Row row in rows) //this will also include your header row...
                            {
                                if (row.RowIndex > 0 && row.RowIndex < rows.Count() - 2)
                                {
                                    DataRow tempRow = dt0.NewRow();

                                    for (int i = 0; i < countcolumn; i++)
                                    {
                                        tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));

                                        Console.WriteLine(dt0.Columns[i].ColumnName.ToUpper().Contains("DATE".ToUpper()));

                                        if (dt0.Columns[i].ColumnName.ToUpper().Contains("DATE".ToUpper()))
                                        {
                                            if (tempRow[i].ToString().Length > 1 && tempRow[i].ToString().IsNumeric())
                                            {
                                                tempRow[i] = string.Format("{0}", DateTime.FromOADate(int.Parse(tempRow[i].ToString())));
                                            }
                                        }
                                        //else if (dt0.Columns[i].ColumnName.ToUpper().Contains("userid".ToUpper()))
                                        //{
                                        //    tempRow[i] = userid.ToString();
                                        //}

                                    }
                                    //Console.WriteLine(row.RowIndex.ToString());
                                    tempRow["userid"] = userid;
                                    dt0.Rows.Add(tempRow);
                                }


                            }

                            dt0.Rows.RemoveAt(0); //...so i'm taking it out here.

                        }
                        else if (SheetName == "LCInfo")
                        {

                            foreach (Cell cell in rows.ElementAt(0))
                            {
                                dt1.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                            }

                            int countcolumn = dt1.Columns.Count;
                            //dt1.Columns.Add("userid");



                            foreach (Row row in rows) //this will also include your header row...
                            {
                                if (row.RowIndex > 0 && row.RowIndex < rows.Count() - 2)
                                {
                                    DataRow tempRow = dt1.NewRow();

                                    for (int i = 0; i < countcolumn; i++)
                                    {
                                        tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                                    }
                                    Console.WriteLine(row.RowIndex.ToString());
                                    dt1.Rows.Add(tempRow);
                                }
                            }

                            dt1.Rows.RemoveAt(0); //...so i'm taking it out here.
                        }
                    }
                }



                #region details ///details table function///
                //var conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                string table_Details = "Temp_COM_MasterLC_details";
                //string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection con = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("delete from dbo." + table_Details + " where userid   in ('" + userid + "', '')", con);
                con.Open();
                cmd.ExecuteNonQuery();
                //Response.Redirect("done.aspx");
                con.Close();



                using (SqlConnection conn = new SqlConnection(conString))
                {



                    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                    {
                        //bulkCopy.DestinationTableName = table;
                        DestinationTableName = "dbo." + table_Details // "+"_Temp
                    };
                    conn.Open();




                    DataTable schema = conn.GetSchema("Columns", new[] { null, null, table_Details, null });
                    foreach (DataColumn sourceColumn in dt0.Columns)
                    {
                        foreach (DataRow row in schema.Rows)
                        {
                            if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                            {
                                bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                                break;
                            }
                            //bulkCopy.ColumnMappings.Add("userid", (string)row["COLUMN_NAME"]);
                        }
                    }
                    bulkCopy.WriteToServer(dt0);

                    conn.Close();
                }
                #endregion



                #region Master //// master table function

                string table_Master = "Temp_COM_MasterLC_Masters";
                //String connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //SqlConnection con = new SqlConnection(connectionString);
                cmd = new SqlCommand("delete from dbo." + table_Master + " where  userid is null or userid   in ('" + userid + "', '')", con);
                con.Open();
                cmd.ExecuteNonQuery();
                //Response.Redirect("done.aspx");
                con.Close();




                using (SqlConnection conn = new SqlConnection(conString))
                {



                    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                    {
                        //bulkCopy.DestinationTableName = table;
                        DestinationTableName = "dbo." + table_Master // "+"_Temp
                    };
                    conn.Open();


                    DataTable calculatetable = CustomTable(dt1, userid);  ////for convert row into a column and save into a datatable

                    DataTable schema = conn.GetSchema("Columns", new[] { null, null, table_Master, null });
                    foreach (DataColumn sourceColumn in calculatetable.Columns)
                    {
                        foreach (DataRow row in schema.Rows)
                        {
                            if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                            {
                                bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                                break;
                            }
                            //bulkCopy.ColumnMappings.Add("userid", (string)row["COLUMN_NAME"]);
                        }
                    }
                    bulkCopy.WriteToServer(calculatetable);
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }


        public static DataTable CustomTable(DataTable excelTable, string currentuserid)
        {
            DataTable table = new DataTable();


            for (int index = 0; index < excelTable.Rows.Count; index++)
            {
                DataRow excelRow = excelTable.Rows[index];

                //var col  = table.Columns.Add("Category", typeof(String));
                string x = excelTable.Rows[index][1].ToString();
                if (x.Length > 1)
                {
                    //var col = 
                    table.Columns.Add(x, typeof(string));
                }




            }
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("SL", typeof(int));
            table.Columns.Add("userid", typeof(string));



            table.Rows.Add();
            DataRow fahad = table.Rows[0];
            object userid = currentuserid;
            fahad["SL"] = 1;
            fahad["Id"] = 1;
            fahad["userid"] = currentuserid;

            int i = 0;
            for (int index = 0; index < excelTable.Rows.Count; index++)
            {
                var colname = table.Columns[i].ColumnName.ToUpper();

                DataRow excelRow = excelTable.Rows[index];
                string x = excelTable.Rows[index][2].ToString();


                if (colname.Contains("date".ToUpper()))
                {
                    if (x.Length > 1 && x.IsNumeric())
                    {

                        fahad[i] = string.Format("{0}", DateTime.FromOADate(int.Parse(excelRow["Information"].ToString())));

                        //i++;
                    }
                }
                else
                {

                    //var col  = table.Columns.Add("Category", typeof(String));

                    if (x.Length > 1)
                    {

                        fahad[i] = string.Format("{0}", excelRow["Information"].ToString());

                    }

                }
                i++;

            }
            return table;
        } /// <summary>

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
