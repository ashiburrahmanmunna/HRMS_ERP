using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GTERP.Models.Common;
using GTERP.BLL;
using Newtonsoft.Json;
using DataTablesParser;
using static GTERP.Controllers.CompanyPermissionsController;
using GTERP.Services;
using Microsoft.Data.SqlClient;


namespace GTERP.Controllers.Account
{
    [OverridableAuthorize]
    public class Acc_Voucher_PFController : Controller
    {
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db;
        public clsProcedure clsProc { get; }
        public CommercialRepository Repository { get; set; } ///for report service

        public Acc_Voucher_PFController(GTRDBContext context , CommercialRepository repository, clsProcedure _clsProc, TransactionLogRepository tran)//for report service
        {
            db = context;
            Repository = repository; ///for report service
            clsProc = _clsProc;
            tranlog = tran;
        }




        [HttpPost, ActionName("SetSessionInd")]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionInd(string reporttype, string action, string reportid)
        {
            try
            {

                //Session["ReportType"] = reporttype;
                HttpContext.Session.SetString("ReportType", reporttype);
                return Json(new { Success = 1 });

            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Set").Message.ToString() });
        }

        //private int comid = int.Parse(httpreHttpContext.Session.GetString("comid"););
        //
        // GET: /Voucher/
        public ViewResult Index(string FromDate, string ToDate , string UserList)
        {



            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
           
            //var gTRDBContext = db.StoreRequisitionMain.Where(x => x.ComId == comid).Include(s => s.ApprovedBy).Include(s => s.Department).Include(s => s.PrdUnit).Include(s => s.Purpose).Include(s => s.RecommenedBy);

            ///////////get user list from the server //////

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            //Uri url = new Uri(string.Format("https://localhost:44336/api/user/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            Uri url = new Uri(string.Format("http://gtrbd.net:92/api/User/GetUsersCompanies"));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<CompanyPermissionsController.AspnetUserList>();



            var le = new CompanyPermissionsController.AspnetUserList();
            le.Email = "--All User--";
            le.UserId = "1";
            le.UserName = "--All User--";
            l.Add(le);



            foreach (var c in list)
            {
                le = new CompanyPermissionsController.AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }

            ViewBag.Userlist = new SelectList(l, "UserId", "UserName", userid);






            var transactioncomid = HttpContext.Session.GetString("comid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

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
            if (UserList == null)
            {
                UserList = userid;
            }

            ViewBag.Acc_VoucherNoPrefix = db.Acc_VoucherNoPrefixes.Include(x=>x.vVoucherTypes).Where(x => x.isVisible == true && x.vVoucherTypes.isSystem == false).ToList();

            //transactioncomid = "1";
            //var a = ;
            // return View(db.Acc_VoucherMain_PF.Where(p => p.ComId == transactioncomid).ToList());

            var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            if (fiscalYear != null)
            {
                //var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(x=>x.FYId==fiscalYear.FiscalYearId).OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");

            }
            else
            {
                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");
            }

            ViewBag.IntegrationSettingMainId = new SelectList(db.Cat_Integration_Setting_Mains.Where(v => v.ComId == comid && v.IntegrationTableName == "HR_ProcessedDataSal").OrderBy(x => x.MainSLNo), "IntegrationSettingMainId", "IntegrationSettingName");


            if (UserList == null)
            { 
                var X = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList();
                return View(X);
            }
            else
            {
                if (UserList == "1")
                {
                    var X = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList();

                    if (X.Count > 0)
                    {
                        return View(X);

                    }
                    else
                    {
                        X = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid).OrderByDescending(x => x.VoucherId).Take(5).ToList();
                        return View(X);

                    }
                }
                else
                {
                    var X = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.userid == UserList).ToList();

                    if (X.Count > 0)
                    {
                        return View(X);

                    }
                    else
                    {
                        X = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && p.userid == UserList).OrderByDescending(x => x.VoucherId).Take(5).ToList();
                        return View(X);

                    }
                }

            }

            return View();

        }


        [HttpGet]
        public IActionResult GetFiscalMonth(int id)
        {
            var data = db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(m => m.FYId == id).ToList();
            return Json(data);
        }

        //
        // GET: /Voucher/Details/5

        //[OutputCache(Duration = 100, VaryByParam  = "id")]
        //[OutputCache(CacheProfile ="Admin")]
        public ViewResult Details(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
            return View(Vouchermain);
        }


        public ViewResult PrintView(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
            return View(Vouchermain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //return View(Vouchermain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "Voucher", new { id = id });
        }

        // make pdf
        // need change
        //////public ActionResult ExportPdf(int id)
        //////{

        //////    // go to new page( will not show ) and make it pdf
        //////    return new ActionAsPdf("PrintView", new { id = id })
        //////    {
        //////        FileName = Server.MapPath("~/Content/FileName.pdf")
        //////    }; ;
        //////}
        ///

        public ActionResult PrintCheck(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            var abcvouchermain = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.comid == comid).FirstOrDefault();

            var reportname = "rptChk_janata";// db.Acc_VoucherMain_PF.Where(x => x.VoucherId== id).Select(x => x.VoucherNo).FirstOrDefault();

            if (abcvouchermain.Acc_VoucherType != null)
            {
                if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                        reportname = "rptChk_janata";
                }
            }


            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = db.Acc_VoucherMain_PF.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            //var Currency = "18";
            HttpContext.Session.SetString("reportquery", "Exec acc_rptCheckPrint  '" + id + "','" + comid + "' , 'ChequeNo'");

            string filename = db.Acc_VoucherSubChecnos.Where(x => x.VoucherId == id).Select(x => x.ChkNo).FirstOrDefault();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            string DataSourceName = "DataSet1";

            
            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;


            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            var abcd = HttpContext.Session.GetString("ReportType");

            if (abcd != null)
            {
                ReportType = abcd;

            }
            else
            {
                ReportType = "PDF";

            }


            string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
            return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }

        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            var abcvouchermain = db.Acc_VoucherMain_PF.Include(x=>x.Acc_VoucherType).Where(x=>x.VoucherId == id && x.comid == comid).FirstOrDefault();

            var reportname = "rptShowVoucher";// db.Acc_VoucherMain_PF.Where(x => x.VoucherId== id).Select(x => x.VoucherNo).FirstOrDefault();

            if (abcvouchermain.Acc_VoucherType != null)
            {
                if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    reportname = "rptShowVoucher_VBP";
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Journal".ToUpper())
                {
                    reportname = "rptShowVoucher_Journal";

                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Receipt".ToUpper())
                {
                    reportname = "rptShowVoucher_MoneyReceipt";

                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    reportname = "rptChk_janata";
                }
            }
            //if (reportname == null)
            //{
            //    reportname = "rptShowVoucher";
            //}

            //HttpContext.Session.SetString("PrintFileName",
            //int WarehouseCount = db.Acc_VoucherMain_PF.Where(x => x.VoucherId == id).Count(); 
            //if (WarehouseCount > 0) { reportname = "rptShowVoucher_SubRpt"; }

            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = db.Acc_VoucherMain_PF.Include(x=>x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            var Currency = "18";
            HttpContext.Session.SetString("reportquery", "Exec Acc_rptVoucher_PF 0, 'VID','All', '"+ comid + "' , '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.Acc_VoucherMain_PF.Where(x => x.VoucherId == id).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();


            string DataSourceName = "DataSet1";
            //string FormCaption = "Report :: Sales Acknowledgement ...";


            //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" +HttpContext.Session.GetString("comid"); + "',''"));

            HttpContext.Session.SetObject("rptList", postData);
            

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain =HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            var abcd = HttpContext.Session.GetString("ReportType");

            if (abcd != null)
            { 
                ReportType = abcd;

            }
            else
            {
                ReportType = "PDF";

            }


            /////////////////////// sub report test to our report server


            var subReport = new SubReport();
            var subReportObject = new List<SubReport>();

            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "";
            subReport.strQuerySub = "Exec [rptShowVoucher_Referance_PF] '" + id + "','" + comid + "','ChequeNo'";
            subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            subReportObject.Add(subReport);


            subReport = new SubReport();
            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "";
            subReport.strQuerySub = "Exec [rptShowVoucher_Referance_PF] '" + id + "','" + comid + "','ReceiptPerson'";
            subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            subReportObject.Add(subReport);


            var jsonData = JsonConvert.SerializeObject(subReportObject);

            string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);
            return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }

        public JsonResult CallComboSubSectionList()
        {
            try
            {
                //var SubSectionList = new SelectList(db.Cat_SubSection.Where(c => c.SubSectId > 0), "SubSectId", "SubSectName").ToList();

                var SubSectionList = db.Cat_SubSection.Select(e => new
                {
                    value = e.SubSectId,
                    display = e.SubSectName
                }).ToList();


                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                //MyObj SubsectionList = json_serializer.Deserialize<MyObj>(SubSectionList.ToList());
                // = SubsectionList.value;
                //refresh_token = SubsectionList.display;


                return Json(SubSectionList);

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }

        struct MyObj
        {
            public int value { get; set; }
            public string display { get; set; }


        }

        //public ActionResult MyReport(string namedParameter1, string namedParameter2)
        //{
        //    //var model = this.GetReportViewerModel(Request);
        //    //model.ReportPath = "/Folder/Report File Name";
        //    //model.AddParameter("Parameter1", namedParameter1);
        //    //model.AddParameter("Parameter2", namedParameter2);

        //    return View("ReportViewer", model);
        //}


        public ActionResult Create(string Type , int VoucherId = 0 , int FiscalMonthId = 0 , int? IntegrationSettingMainId = 0)
        {
            try
            {


                //ViewBag.Title = "Entry";
                var transactioncomid =HttpContext.Session.GetString("comid");
                var lastvoucher = db.Acc_VoucherMain_PF.Where(x => x.Acc_VoucherType.VoucherTypeNameShort == Type && x.comid == transactioncomid).OrderByDescending(x => x.VoucherId).FirstOrDefault();



                if (Type == null)
                {
                    Type = "VPC";
                }
                string vouchertypeid = db.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x=>x.VoucherTypeId.ToString()).FirstOrDefault();
                // HttpContext.Session.SetString("defaultcurrencyname",
                Acc_VoucherMain_PF vouchersamplemodel = new Acc_VoucherMain_PF();

                if (lastvoucher != null)
                {
                    vouchersamplemodel.VoucherDate = lastvoucher.VoucherDate;
                    vouchersamplemodel.LastVoucherNo = lastvoucher.VoucherNo;
                }
                else
                {
                    vouchersamplemodel.VoucherDate = DateTime.Now.Date;
                }
      


                //Acc_VoucherSub_PF VoucherSubs = new Acc_VoucherSub_PF();
                //VoucherSubs.VoucherSubSections = new List<Acc_VoucherSub_PFSection>();
                //VoucherSubs.VoucherSubChecnoes = new List<Acc_VoucherSub_PFChecno>();


                //vouchersamplemodel.VoucherSubs.Add(VoucherSubs);


                var transactioncompany = db.Companys.Include(x=>x.vCountryCompany).Where(c => c.CompanyCode == transactioncomid).FirstOrDefault();
                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

                this.ViewBag.Acc_VoucherType = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName", vouchertypeid);
                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

                vouchersamplemodel.Acc_VoucherType = db.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

                ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.comid == transactioncomid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
                                                                                                                                                ///////account head parent data for dropdown
                var ChartOfAccountParent = db.Acc_ChartOfAccounts_PF.Where(c => c.AccId > 0 && c.AccType == "G" && c.comid == transactioncomid).Select(s => new { Text = s.AccName, Value = s.AccId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "Value", "Text");

                this.ViewBag.Acc_FiscalYear = new SelectList(db.Acc_FiscalYears.Where(x=>x.comid == transactioncomid), "Value", "Text");
                this.ViewBag.Acc_FiscalMonth = new SelectList(db.Acc_FiscalMonths.Where(x => x.ComId == transactioncomid).Take(0), "Value", "Text");

                var HR_Emp_Info = db.HR_Emp_Info.Where(c => c.EmpId > 0 && c.ComId == transactioncomid).Select(s => new { Text = s.EmpCode + " - " + s.EmpName + " - " + s.Cat_Designation.DesigName, Value = s.EmpId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.EmpId = new SelectList(HR_Emp_Info, "Value", "Text");

                var Customer = db.Customers.Take(1).Where(c => c.CustomerId > 0 && c.comid == transactioncomid).Select(s => new { Text = s.CustomerCode + " - " + s.CustomerName, Value = s.CustomerId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.CustomerId = new SelectList(Customer, "Value", "Text");

                var Supplier = db.Suppliers.Take(1).Where(c => c.SupplierId > 0 && c.comid == transactioncomid).Select(s => new { Text = s.SupplierCode + s.SupplierName, Value = s.SupplierId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.SupplierId = new SelectList(Supplier, "Value", "Text");


                if (Type == "VPC")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }





                    
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        ///////only cash item when multi debit credit of then it enable

                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}

                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                            .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b=>b.ParentChartOfAccount)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                            .Include(x => x.Acc_VoucherType)

                            .Where(x=>x.VoucherId  == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VRC")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }





                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                        .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                        .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                        .Include(x => x.Acc_VoucherType)
                        .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VRB")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                            .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                            .Include(x => x.Acc_VoucherType)

                            .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VPB")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0  && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }





                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");








                        try
                        {



                            ///integration Part -- Need more work by Himu
                            if (FiscalMonthId > 0 && IntegrationSettingMainId > 0)
                            {

                                string comid = HttpContext.Session.GetString("comid");

                                var query = $"Exec Integration_Process '{comid}',{IntegrationSettingMainId},{FiscalMonthId} ";


                                SqlParameter[] sqlParameter = new SqlParameter[3];
                                sqlParameter[0] = new SqlParameter("@ComId", comid);
                                sqlParameter[1] = new SqlParameter("@IntegrationSettingMainId", IntegrationSettingMainId);
                                sqlParameter[2] = new SqlParameter("@FiscalMonthid", FiscalMonthId);
                                Helper.ExecProc("Integration_Process", sqlParameter);

                                var fiscalmonthname = db.Acc_FiscalMonths.Where(x => x.FiscalMonthId == FiscalMonthId && x.ComId == comid).Select(x => x.MonthName).FirstOrDefault();

                                var abcdefgh = db.Cat_PFIntegrationSummary.Include(x => x.Acc_ChartOfAccounts).ThenInclude(x => x.ParentChartOfAccount).Where(x => x.FiscalMonthId == FiscalMonthId && x.DataType == IntegrationSettingMainId.ToString() && x.Acc_ChartOfAccounts.AccType == "L").ToList(); //&& (x.TKCreditLocal + x.TKCreditLocal) > 0
                                vouchersamplemodel.VoucherDesc = "Being the amount paid against salary and overtime for the month of " + fiscalmonthname + " as per approved bill attached herewith.";
                                vouchersamplemodel.VoucherSubs = new List<Acc_VoucherSub_PF>();
                                foreach (var item in abcdefgh)
                                {
                                    if ((item.TKDebitLocal + item.TKCreditLocal) > 0)
                                    {
                                        Acc_VoucherSub_PF abc = new Acc_VoucherSub_PF();
                                        abc.AccId = item.AccId;
                                        abc.TKDebit = item.TKDebitLocal;
                                        abc.TKCredit = item.TKCreditLocal;
                                        abc.TKDebitLocal = item.TKDebitLocal;
                                        abc.TKCreditLocal = item.TKCreditLocal;
                                        abc.Note1 = item.Note1; ///IF CT INFORMATION IS THERE --- CAUSE NO OTHER COLUMN HAVE FOR CT IN INTEGRATION TABLE
                                        abc.Note2 = item.Note2; ///IF CT INFORMATION IS THERE --- CAUSE NO OTHER COLUMN HAVE FOR CT IN INTEGRATION TABLE
                                        abc.Note3 = item.Note3;
                                        abc.SLNo = int.Parse(item.SLNo);
                                        abc.Acc_ChartOfAccount_PF = item.Acc_ChartOfAccounts;
                                        vouchersamplemodel.VoucherSubs.Add(abc);
                                    }
                                };
                            }
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }








                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                         .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                         .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                         .Include(x => x.Acc_VoucherType)

                         .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VCR")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                        .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                        .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                        .Include(x => x.Acc_VoucherType)
                        .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true || p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }

                }
                else if (Type == "VJR")
                {

                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    if (VoucherId == 0)
                    {


                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        try
                        {
                            ///integration Part -- Need more work for issue and mrr /grr
                            if (FiscalMonthId > 0 && IntegrationSettingMainId > 0)
                            {

                                string comid = HttpContext.Session.GetString("comid");

                                var query = $"Exec Integration_Process '{comid}',{IntegrationSettingMainId},{FiscalMonthId} ";


                                SqlParameter[] sqlParameter = new SqlParameter[3];
                                sqlParameter[0] = new SqlParameter("@ComId", comid);
                                sqlParameter[1] = new SqlParameter("@IntegrationSettingMainId", IntegrationSettingMainId);
                                sqlParameter[2] = new SqlParameter("@FiscalMonthid", FiscalMonthId);
                                Helper.ExecProc("Integration_Process", sqlParameter);

                                var fiscalmonthname = db.Acc_FiscalMonths.Where(x => x.FiscalMonthId == FiscalMonthId && x.ComId == comid).Select(x => x.MonthName).FirstOrDefault();


                                var SETTINGSNAME = db.Cat_Integration_Setting_Mains.Where(x => x.IntegrationSettingMainId == IntegrationSettingMainId).FirstOrDefault().IntegrationSettingName;



                                var abcdefgh = db.Cat_PFIntegrationSummary.Include(x => x.Acc_ChartOfAccounts).ThenInclude(x => x.ParentChartOfAccount).Where(x => x.FiscalMonthId == FiscalMonthId && x.DataType == IntegrationSettingMainId.ToString() && x.Acc_ChartOfAccounts.AccType == "L").ToList(); //&& (x.TKCreditLocal + x.TKCreditLocal) > 0
                                vouchersamplemodel.VoucherDesc = SETTINGSNAME.ToString() + " for the month of " + fiscalmonthname + " .";
                                vouchersamplemodel.VoucherSubs = new List<Acc_VoucherSub_PF>();
                                foreach (var item in abcdefgh)
                                {
                                    if ((item.TKDebitLocal + item.TKCreditLocal) > 0)
                                    {
                                        Acc_VoucherSub_PF abc = new Acc_VoucherSub_PF();
                                        abc.AccId = item.AccId;
                                        abc.TKDebit = item.TKDebitLocal;
                                        abc.TKCredit = item.TKCreditLocal;
                                        abc.TKDebitLocal = item.TKDebitLocal;
                                        abc.TKCreditLocal = item.TKCreditLocal;
                                        abc.Note1 = item.Note1; ///IF CT INFORMATION IS THERE --- CAUSE NO OTHER COLUMN HAVE FOR CT IN INTEGRATION TABLE
                                        abc.Note2 = item.Note2; ///IF CT INFORMATION IS THERE --- CAUSE NO OTHER COLUMN HAVE FOR CT IN INTEGRATION TABLE
                                        abc.Note3 = item.Note3;
                                        abc.SLNo = int.Parse(item.SLNo);
                                        abc.Acc_ChartOfAccount_PF = item.Acc_ChartOfAccounts;
                                        vouchersamplemodel.VoucherSubs.Add(abc);
                                    }
                                };
                            }
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                        .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                        .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                        .Include(x => x.Acc_VoucherType)
                        .Where(x => x.VoucherId == VoucherId).FirstOrDefault();


                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" ).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }

                }
                else if (Type == "VLC")
                {

                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                                               .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                                               .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                                               .Include(x => x.Acc_VoucherType)
                                               .Where(x => x.VoucherId == VoucherId).FirstOrDefault();


                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }


                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }


                //    .Select(e => new
                //{
                //    value = e.SubSectId,
                //    display = e.SubSectName
                //}).ToList();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public class VoucherChartOfAccount
        {
            //public IEnumerable<Acc_ChartOfAccount_PF> coa { get; set; }
            public int AccId { get; set; }
            public string AccCode { get; set; }
            public string AccName { get; set; }


            public string ParentName { get; set; }
            public string Parentcode { get; set; }

            public decimal Balance { get; set; }

            public int IsChkBalance { get; set; }

            public int CountryId { get; set; }
            public int CountryIdLocal { get; set; }
            public decimal AmountLocalBuy { get; set; }
            public decimal AmountLocalSale { get; set; }
            public int isCredit { get; set; }


        }

        public class Acc_ChartOfAccount_PF_view
        {
            public int AccId { get; set; }
            public int? ParentId { get; set; }
            public int IsBankItem { get; set; }
            public int IsCashItem { get; set; }
            public int IsChkRef { get; set; }
        }

        [HttpPost]
        public JsonResult AccountInfo(int id)
        {
            try
            {
                // 
                //


                //var transactioncomid =HttpContext.Session.GetString("comid");
                //var transactioncompany = db.Companys.Where(c => c.ComId == transactioncomid).FirstOrDefault();
                //Acc_ChartOfAccount_PF chartofaccount = new Acc_ChartOfAccount_PF();


                //if (transactioncompany.isMultiDebitCredit == true)
                //{

                //}
                //else
                //{

                //}

                Acc_ChartOfAccount_PF_view abc = new Acc_ChartOfAccount_PF_view();
                Acc_ChartOfAccount_PF chartofaccount = db.Acc_ChartOfAccounts_PF.Where(y => y.AccId == id).SingleOrDefault();

                if (chartofaccount != null)
                {

                    abc.AccId = chartofaccount.AccId;
                    abc.ParentId = chartofaccount.ParentID;

                    abc.IsChkRef = chartofaccount.IsChkRef == true ? 1 : 0;
                    abc.IsBankItem = chartofaccount.IsBankItem == true ? 1 : 0;
                    abc.IsCashItem = chartofaccount.IsCashItem == true ? 1 : 0;



                }



                //return Json(chartofaccount);
                return Json(abc);


            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
        }



        // POST: /Voucher/Create

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[RequestSizeLimit(73400320)]
        //public JsonResult Create(Acc_VoucherMain_PF acc_voucherMain)
        public IActionResult Create(Acc_VoucherMain_PF acc_voucherMain/*, List<HR_Loan_Data_HouseBuilding> details*/)
        {
            try
            {

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");


                var lockCheck = db.HR_ProcessLock
                .Where(p => p.LockType.Contains("Account Lock") && p.DtDate.Date <= acc_voucherMain.VoucherDate.Date && p.DtToDate.Value.Date >= acc_voucherMain.VoucherDate.Date
                    && p.IsLock == true).FirstOrDefault();

                if (lockCheck != null)
                {
                    return Json(new { Success = 0, ex = "Account Lock this date!!!" });
                }


                acc_voucherMain.VAmount = double.Parse(clsProc.GTRFormatCurrencyBDT(acc_voucherMain.VAmount.ToString()));
                acc_voucherMain.vAmountInWords = clsProc.GTRInwordsFormatBD(acc_voucherMain.VAmount.ToString(), "", "");
                acc_voucherMain.DateUpdated = DateTime.Now.Date;
                acc_voucherMain.useridUpdate = userid;


                if (acc_voucherMain.VoucherTypeId == 3)
                {
                    var abcsum = 0.00;
                    var bankacccode = "";
                    foreach (var item in acc_voucherMain.VoucherSubs)
                    {

                        bankacccode = db.Acc_ChartOfAccounts_PF.Where(p => p.comid == comid && p.AccType == "L" && p.IsBankItem == true && p.AccId == item.AccId).Select(x => x.AccCode).FirstOrDefault();
                        if (bankacccode != null)
                        {
                            if (bankacccode.Contains("1-1-11"))
                            {
                                abcsum = +item.TKCredit;
                            }
                        }

                    }

                    //acc_voucherMain.VoucherSubs = 
                    //var inwordsfigure = acc_voucherMain.VoucherSubs.Where(x => x.Acc_ChartOfAccounts_PF.AccCode.Contains("1-1-11")).Sum(x => x.TKCredit);
                    acc_voucherMain.vAmountInWords = clsProc.GTRInwordsFormatBD(abcsum.ToString(), "", "");
                }


                //if (Vouchermainabc != null)
                //{
                //    var JObject = new JObject();
                //    var d = JObject.Parse(Vouchermainabc);
                //    string objct = d["Acc_VoucherMain_PF"].ToString();
                //    Acc_VoucherMain_PF model = JsonConvert.DeserializeObject<Acc_VoucherMain_PF>(objct);


                //    Acc_VoucherMain_PF acc_voucherMain = model;


                    acc_voucherMain.VoucherInputDate = DateTime.Now.Date;

                    //var errors = ModelState.Where(x => x.Value.Errors.Any())
                    //.Select(x => new { x.Key, x.Value.Errors });

                    //if (ModelState.IsValid)
                    //if (errors.Count() < 2)

                    {
                    //var monthid = acc_voucherMain.VoucherDate.Month;// && x.dtFrom.ToString() == monthid.ToString()

                    //var monthlist = 

                    DateTime date = acc_voucherMain.VoucherInputDate;
                    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


                    //List<Acc_FiscalMonth> listactivefiscalmonth = db.Acc_FiscalMonths.Where(x => x.ComId == comid && x.FYId == voucherfiscalyear && x.MonthName == ).ToList();
                    //var activefiscalmonth = listactivefiscalmonth.Where(x => Convert.ToDateTime(x.dtFrom).Month == monthid).FirstOrDefault();
                    var activefiscalmonth = db.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
                    //listactivefiscalmonth.Where(x => Convert.ToDateTime(x.dtFrom).Month == monthid).FirstOrDefault();

                    if (activefiscalmonth == null)
                    {

                        return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                    }
                    var activefiscalyear = db.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
                    if (activefiscalyear == null)
                    {

                        return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                    }



                    // If sales main has VoucherID then we can understand we have existing sales Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (acc_voucherMain.VoucherId > 0)
                        {

                        ViewBag.Title = "Edit";

                        //var CurrentProductSerial = db.ProductSerial.Where(p => p.VoucherId == acc_voucherMain.VoucherId);
                        var CurrentVoucherSub = db.Acc_VoucherSub_PF.Include(x=>x.VoucherSubChecnoes).Include(x=>x.VoucherSubSections).Where(p => p.VoucherId == acc_voucherMain.VoucherId);
                        var CurrentVoucherSubcheckno = db.Acc_VoucherSubChecnos.Where(p => p.VoucherId == acc_voucherMain.VoucherId);

                        //var CurrentVoucherSubSection = db.Acc_VoucherSub_PFSections.Where(p => p.VoucherId == acc_voucherMain.VoucherId);
                        //var CurrentVoucherCheck = db.Acc_VoucherSubChecnos.Where(p => p.VoucherId == acc_voucherMain.VoucherId);
                        acc_voucherMain.useridUpdate = userid;
                        acc_voucherMain.DateUpdated = DateTime.Now;


                        //Acc_VoucherSub_PF
                        //foreach (ProductSerial ss in CurrentProductSerial)
                        //db.ProductSerial.Remove(ss);

                        //db.Acc_VoucherSub_PF.Remove(CurrentVoucherSub);
                        //db.SaveChanges();

                        //foreach (Acc_VoucherSub_PF ss in CurrentVoucherSub)
                        //{
                        //    //    foreach (Acc_VoucherSub_PFCheckno sss in ss.VoucherSubChecnoes)
                        //    //    db.Acc_VoucherSubChecnos.Remove(sss);

                        //    //    foreach (Acc_VoucherSub_PFSection ssss in ss.VoucherSubSections)
                        //    //    db.Acc_VoucherSub_PFSections.Remove(ssss);


                        //     db.Acc_VoucherSub_PF.Remove(ss);

                        //}

                        
                        db.Acc_VoucherSubChecnos.RemoveRange(CurrentVoucherSubcheckno);
                        db.Acc_VoucherSub_PF.RemoveRange(CurrentVoucherSub);
                        db.SaveChanges();

                        //foreach (Acc_VoucherSub_PFCheckno ss in CurrentVoucherCheck)
                        //    db.Acc_VoucherSub_PF.Remove(ss);
                        //db.SaveChanges();


                        foreach (Acc_VoucherSub_PF ss in acc_voucherMain.VoucherSubs)
                        {
                            //if (ss.VoucherSubId > 0)
                            //{
                            //db.Entry(ss).State = EntityState.Modified;
                            ss.VoucherSubId = 0;
                            db.Acc_VoucherSub_PF.Add(ss);
                            //db.SaveChanges();

                            //}
                            //else
                            //{
                            //    //db.Acc_VoucherSub_PF.Add(ss);
                            //    db.Acc_VoucherSub_PF.Add(ss);
                            //}


                            //foreach (Acc_VoucherSub_PFSection sss in ss.VoucherSubSections)
                            //{
                            //    sss.VoucherSubId = ss.VoucherSubId;
                            //    db.Acc_VoucherSub_PFSections.Add(sss);




                            //}


                            //foreach (Acc_VoucherSub_PFCheckno ssss in ss.VoucherSubChecnoes)
                            //{
                            //    ssss.VoucherSubId = ss.VoucherSubId;
                            //    db.Acc_VoucherSubChecnos.Add(ssss);

                            //}

                        }
                        db.SaveChanges();
                        //foreach (Acc_VoucherSub_PFSection sss in CurrentVoucherSubSection)
                        //db.Acc_VoucherSub_PFSections.Remove(sss);


                        //foreach (Acc_VoucherSub_PFChecno ssss in CurrentVoucherCheck)
                        //db.Acc_VoucherSubChecnos.Remove(ssss);


                        //if (acc_voucherMain.VoucherSubs == null)
                        //{
                        //}
                        //else
                        //{
                        //    foreach (Acc_VoucherSub_PFSection subsection in acc_voucherMain.VoucherSubs.)
                        //    db.Acc_VoucherSub_PFSections.Add(subsection);
                        //}


                        //if (acc_voucherMain.VoucherSubs == null)
                        //{
                        //}
                        //else
                        //{

                        //    //foreach (Acc_VoucherSub_PFChecno checkno in acc_voucherMain.vVouchersubMain.VoucherSubChecnos)
                        //    //    db.Acc_VoucherSubChecnos.Add(checkno);
                        //}
                        acc_voucherMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                        acc_voucherMain.FiscalYearId = activefiscalyear.FiscalYearId;


                        db.Entry(acc_voucherMain).State = EntityState.Modified;



                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), acc_voucherMain.VoucherId.ToString(), "Update", acc_voucherMain.VoucherNo.ToString());

                        db.SaveChanges();

                    }
                    //Perform Save
                    else
                    {
                        ViewBag.Title = "Create";

                            acc_voucherMain.userid = userid;// HttpContext.Session.GetString("userid");
                            acc_voucherMain.comid = comid;
                            acc_voucherMain.DateAdded = DateTime.Now;
                            acc_voucherMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                            acc_voucherMain.FiscalYearId = activefiscalyear.FiscalYearId;


                        //var voucherfiscalyear = db.Acc_FiscalYears.Where(x => x.OpeningDate >= acc_voucherMain.VoucherDate).FirstOrDefault();
                        //var voucherfiscalyear = db.Acc_FiscalYears.Where(x => x.OpeningDate >= acc_voucherMain.VoucherDate && x.ClosingDate <= acc_voucherMain.VoucherDate).FirstOrDefault();
                        var voucherfiscalyear = db.Acc_FiscalYears.Where(x => x.comid == comid && x.OpeningDate <= acc_voucherMain.VoucherDate && x.ClosingDate >= acc_voucherMain.VoucherDate).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()

                        ////voucherfiscalyear.FiscalYearId,
                        ///
                        var x = VoucherNoMaker(acc_voucherMain.comid, acc_voucherMain.VoucherTypeId, acc_voucherMain.VoucherDate , activefiscalyear.FiscalYearId, activefiscalmonth.FiscalMonthId); // nned to work.. //// activefiscalyear.FiscalYearId 
                        acc_voucherMain.VoucherNo = x[0];
                            acc_voucherMain.VoucherSerialId = int.Parse(x[1]);

                            db.Acc_VoucherMain_PF.Add(acc_voucherMain);

                        db.SaveChanges();

                        db.Entry(acc_voucherMain).GetDatabaseValues();
                        int id = acc_voucherMain.VoucherId; // Yes it's here

                        var abc = acc_voucherMain.VoucherSubs.ToList();
                        foreach (var itemabc in abc)
                        {
                            var xyz = itemabc.VoucherSubChecnoes.ToList();
                            xyz.ForEach(m => m.VoucherId = id);
                            db.SaveChanges();

                        }
                    }




                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), acc_voucherMain.VoucherId.ToString(), "Create", acc_voucherMain.VoucherNo.ToString());


                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                }
                return Json(new { Success = 1, VoucherID = acc_voucherMain.VoucherId, ex = "" });

                //}
                //return Json(new { Success = 0, ex = "Data Empty or Null" });

            }
            catch (Exception ex)
            {

                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            
        }


        public string[] VoucherNoMaker(string comid , int vouchertypeid , DateTime voucherdate , int fiscalyearid , int fiscalmonthid )
        {
            string[] FinalAccCode = new string[2];
            var input = 0;
            int length = 6;
            int maxvoucherid = 0;
            var maxnowithpadleftresult = "";

            string voucernocreatestyle =  db.Companys.Include(x=>x.vAcc_VoucherNoCreatedTypes).Where(c => c.CompanySecretCode == comid).Select(c => c.vAcc_VoucherNoCreatedTypes.VoucherNoCreatedTypeName).FirstOrDefault();
            string vouchertypeShortPrefix = db.Acc_VoucherNoPrefixes.Where(x => x.VoucherTypeId == vouchertypeid && x.comid == comid).Select(x => x.VoucherShortPrefix).FirstOrDefault();
            length = db.Acc_VoucherNoPrefixes.Where(x => x.VoucherTypeId == vouchertypeid && x.comid == comid).Select(x => x.Length).FirstOrDefault();


            if (voucernocreatestyle == "LifeTime")
            {
                Acc_FiscalYear fiscalyearname = db.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalyearid).FirstOrDefault();
                var fiscalyearjoin = Convert.ToDateTime(fiscalyearname.OpDate).Year.ToString().Substring(2, 2) + "/" + Convert.ToDateTime(fiscalyearname.ClDate).Year.ToString().Substring(2, 2);
                var fiscalmonthname = db.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => Convert.ToDateTime(x.dtFrom).Month).FirstOrDefault().ToString().PadLeft(2, '0');



                maxvoucherid = db.Acc_VoucherMain_PF.Where(x => x.comid == comid && x.VoucherTypeId == vouchertypeid).Max(x => x.VoucherSerialId);
                input = maxvoucherid+1;
                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
                FinalAccCode[0] = vouchertypeShortPrefix+"-" + maxnowithpadleftresult + "-" + fiscalmonthname + "-" + fiscalyearjoin;
                FinalAccCode[1] = input.ToString();
            }
            else if (voucernocreatestyle == "Yearly")
            {
                Acc_FiscalYear fiscalyearname = db.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalyearid).FirstOrDefault();
                var fiscalyearjoin = Convert.ToDateTime(fiscalyearname.OpDate).Year.ToString().Substring(2, 2) + "/" + Convert.ToDateTime(fiscalyearname.ClDate).Year.ToString().Substring(2, 2);
                var fiscalmonthname = db.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => Convert.ToDateTime(x.dtFrom).Month).FirstOrDefault().ToString().PadLeft(2, '0');


                maxvoucherid = db.Acc_VoucherMain_PF.Where(x => x.comid == comid && x.VoucherTypeId == vouchertypeid && x.FiscalYearId == fiscalyearid).DefaultIfEmpty().Max(p => p == null ? 0 : p.VoucherSerialId);
                input = maxvoucherid + 1;
                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
                FinalAccCode[0] = vouchertypeShortPrefix + "-" + maxnowithpadleftresult;// + "-" + fiscalyearjoin; //stop for dap project
                FinalAccCode[1] = input.ToString();
            }
            else if (voucernocreatestyle == "Monthly")
            {
                Acc_FiscalYear fiscalyearname = db.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalyearid).FirstOrDefault();
                var fiscalyearjoin = Convert.ToDateTime(fiscalyearname.OpDate).Year.ToString().Substring(2,2) + "/" + Convert.ToDateTime(fiscalyearname.ClDate).Year.ToString().Substring(2, 2);
                var fiscalmonthname = db.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => Convert.ToDateTime(x.dtFrom).Month).FirstOrDefault().ToString().PadLeft(2, '0');


                maxvoucherid = (db.Acc_VoucherMain_PF.Where(x => x.comid == comid && x.VoucherTypeId == vouchertypeid && x.FiscalMonthId == fiscalmonthid).Max(x => (int?)x.VoucherSerialId)) ?? 0;
                maxvoucherid = input = maxvoucherid + 1;
                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
                FinalAccCode[0] = vouchertypeShortPrefix + "-" + maxnowithpadleftresult + "-" + fiscalmonthname + "-" + fiscalyearjoin;
                FinalAccCode[1] = input.ToString();

            }
            else
            {
                FinalAccCode = null;
            }

            return FinalAccCode;
        }

        //
        // GET: /Voucher/Edit/5
        public ActionResult Edit(string Type, int? VoucherId)
        {

            try
            {
                ViewBag.Title = "Entry";
                var transactioncomid =HttpContext.Session.GetString("comid");

                if (Type == null)
                {
                    Type = "VPC";
                }
                string vouchertypeid = db.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();
                
                Acc_VoucherMain_PF vouchersamplemodel = new Acc_VoucherMain_PF();
                var transactioncompany = db.Companys.Where(c => c.CompanyCode == transactioncomid).FirstOrDefault();
                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

                this.ViewBag.Acc_VoucherType = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName", vouchertypeid);
                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

                vouchersamplemodel.Acc_VoucherType = db.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

                ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.comid == transactioncomid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
                                                                                                                                                ///////account head parent data for dropdown
                var ChartOfAccountParent = db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid).Where(c => c.AccId > 0 && c.AccType == "G" && c.comid == transactioncomid).Select(s => new { Text = s.AccName, Value = s.AccId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "Value", "Text");


                var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                if (transactioncompany.isMultiDebitCredit == true)
                {
                    this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                }
                else
                {
                    this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                }





                ///////only cash item when multi debit credit of then it enable
                this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                //string Type = null;

                //if (Type == null)
                //{
                //    Type = "VPC";
                //}
                //var a = Session["isProductSearch"];

                //var transactioncomid =HttpContext.Session.GetString("comid");
                //var transactioncompany = db.Companys.Where(c => c.ComId == transactioncomid).FirstOrDefault();
                //Session["defaultcurrency"] = transactioncompany.CountryId;
                ////ViewBag.Title = "Edit";

                //this.ViewBag.Acc_VoucherType = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName");
                ////this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

                //var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                //this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF, "AccId", "AccName");
                ////this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                //this.ViewBag.AccountSearch = COAParent;

                //var ChartOfAccountParent = db.Acc_ChartOfAccounts_PF.Where(c => c.AccId > 0 && c.AccType == "G"); //&& c.ComId == (transactioncomid)
                //this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "AccId", "AccName");



                //this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName");

                ////this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");

                ////var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", HttpContext.Session.GetString("comid")), new SqlParameter("userid", Session["userid"]))).ToList();
                ////this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                ////this.ViewBag.ProductSerialSearch = ProductSerialresult;
                //           this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);



                ////Call Create View
                //return View("Create", Vouchermain);
                return View();
            }
            catch (Exception ex)
            {
                string abcd = ex.InnerException.InnerException.Message.ToString();
                throw ex;
            }
        }

        public ActionResult CreateCopy(string Type, int? VoucherId)
        {

            try
            {


                ViewBag.Title = "Entry";
                var transactioncomid = HttpContext.Session.GetString("comid");

                if (Type == null)
                {
                    Type = "VPC";
                }
                string vouchertypeid = db.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();
                // HttpContext.Session.SetString("defaultcurrencyname",
                Acc_VoucherMain_PF vouchersamplemodel = new Acc_VoucherMain_PF();

                //Acc_VoucherSub_PF VoucherSubs = new Acc_VoucherSub_PF();
                //VoucherSubs.VoucherSubSections = new List<Acc_VoucherSub_PFSection>();
                //VoucherSubs.VoucherSubChecnoes = new List<Acc_VoucherSub_PFChecno>();


                //vouchersamplemodel.VoucherSubs.Add(VoucherSubs);


                var transactioncompany = db.Companys.Include(x => x.vCountryCompany).Where(c => c.CompanyCode == transactioncomid).FirstOrDefault();
                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

                this.ViewBag.Acc_VoucherType = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName", vouchertypeid);
                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

                vouchersamplemodel.Acc_VoucherType = db.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

                ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.comid == transactioncomid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
                                                                                                                                     ///////account head parent data for dropdown
                var ChartOfAccountParent = db.Acc_ChartOfAccounts_PF.Where(c => c.AccId > 0 && c.AccType == "G" && c.comid == transactioncomid).Select(s => new { Text = s.AccName, Value = s.AccId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "Value", "Text");

                this.ViewBag.Acc_FiscalYear = new SelectList(db.Acc_FiscalYears.Where(x => x.comid == transactioncomid), "Value", "Text");
                this.ViewBag.Acc_FiscalMonth = new SelectList(db.Acc_FiscalMonths.Where(x => x.ComId == transactioncomid).Take(0), "Value", "Text");

                var HR_Emp_Info = db.HR_Emp_Info.Where(c => c.EmpId > 0 && c.ComId == transactioncomid).Select(s => new { Text = s.EmpCode + " - " + s.EmpName + " - " + s.Cat_Designation.DesigName , Value = s.EmpId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.EmpId = new SelectList(HR_Emp_Info, "Value", "Text");

                var Customer = db.Customers.Take(1).Where(c => c.CustomerId > 0 && c.comid == transactioncomid).Select(s => new { Text = s.CustomerCode + " - " + s.CustomerName, Value = s.CustomerId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.CustomerId = new SelectList(Customer, "Value", "Text");

                var Supplier = db.Suppliers.Take(1).Where(c => c.SupplierId > 0 && c.comid == transactioncomid).Select(s => new { Text = s.SupplierCode + s.SupplierName, Value = s.SupplierId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.SupplierId = new SelectList(Supplier, "Value", "Text");


                if (Type == "VPC")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }






                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        ///////only cash item when multi debit credit of then it enable

                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}

                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                            .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                            .Include(x => x.Acc_VoucherType)

                            .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VRC")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }





                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                        .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                        .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                        .Include(x => x.Acc_VoucherType)
                        .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VRB")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                            .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                            .Include(x => x.Acc_VoucherType)

                            .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VPB")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }





                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    //this.ViewBag.AccountSearch = Acc_ChartOfAccount_PF;

                    //List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid",HttpContext.Session.GetString("comid");), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                    //this.ViewBag.AccountSearch = COAParent;
                    //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");



                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        //if (VoucherId == null)
                        //{
                        //    return BadRequest();
                        //}
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                         .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                         .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                         .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                         .Include(x => x.Acc_VoucherType)

                         .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        //if (Vouchermain.isPosted == true)
                        //{
                        //    return BadRequest();
                        //}
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        Vouchermain.VoucherNo = null;
                        Vouchermain.VoucherDate = DateTime.Now.Date;


                        Vouchermain.VoucherSubs.ToList().ForEach
                                    (c => {
                                        c.VoucherId = 0;
                                    });

                        

                        foreach (var item in Vouchermain.VoucherSubs)
                        {
                            item.VoucherSubId = 0;
                            item.VoucherSubChecnoes = null;
                            item.VoucherSubSections = null;


                            //foreach (var item1 in item.VoucherSubChecnoes)
                            //{
                            //    item1.VoucherId = 0;
                            //    item1.VoucherSubId = 0;
                            //}

                            //foreach (var item2 in item.VoucherSubSections)
                            //{
                            //    item2.VoucherId = 0;
                            //    item2.VoucherSubId = 0;
                            //}

                        }
                


                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }
                else if (Type == "VCR")
                {
                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                        .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                        .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                        .Include(x => x.Acc_VoucherType)
                        .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true || p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }

                }
                else if (Type == "VJR")
                {

                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                        .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                        .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                        .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                        .Include(x => x.Acc_VoucherType)
                        .Where(x => x.VoucherId == VoucherId).FirstOrDefault();


                        //if (Vouchermain.isPosted == true)
                        //{
                        //    return BadRequest();
                        //}
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }
                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }

                }
                else if (Type == "VLC")
                {

                    ViewBag.Title = "Create";


                    var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                    if (transactioncompany.isMultiDebitCredit == true)
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }
                    else
                    {
                        this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == true || x.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    }


                    ///////only cash item when multi debit credit of then it enable
                    this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                    this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x => x.ComId == transactioncomid);

                    if (VoucherId == 0)
                    {
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", transactioncompany.CountryId);
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                        return View(vouchersamplemodel);

                    }
                    else
                    {
                        Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                                               .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                                               .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                                               .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                                               .Include(x => x.Acc_VoucherType)
                                               .Where(x => x.VoucherId == VoucherId).FirstOrDefault();


                        if (Vouchermain.isPosted == true)
                        {
                            return BadRequest();
                        }
                        //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);
                        if (Vouchermain == null)
                        {
                            return NotFound();
                        }


                        ViewBag.Title = "Edit";
                        this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
                        this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

                        return View(Vouchermain);

                    }
                }


                //    .Select(e => new
                //{
                //    value = e.SubSectId,
                //    display = e.SubSectName
                //}).ToList();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        // GET: /Voucher/Delete/5
        public ActionResult Delete(int? VoucherId)
        {
            try
            {
                var transactioncomid =HttpContext.Session.GetString("comid");
                if (VoucherId == null)
                {
                    return BadRequest();
                }


                 Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF
                .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount_PF).ThenInclude(b => b.ParentChartOfAccount)
                .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)

                .Include(x => x.Acc_VoucherType)
                .Where(x => x.VoucherId == VoucherId).FirstOrDefault();

                if (Vouchermain.isPosted == true)
                {
                    return BadRequest();
                }

                //Acc_VoucherMain_PF Vouchermain = db.Acc_VoucherMain_PF.Find(id);


                if (Vouchermain == null)
                {
                    return NotFound();
                }
                ViewBag.Title = "Delete";

                
                var transactioncompany = db.Companys.Where(c => c.CompanyCode == transactioncomid).FirstOrDefault();
                //HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
                //HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

                this.ViewBag.Acc_VoucherType = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName", Vouchermain.VoucherTypeId);
                //vouchersamplemodel.VoucherTypeId = int.Parse(vouchervouchertypeid.ToString());

                //vouchersamplemodel.Acc_VoucherType = db.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

                ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.comid == transactioncomid), "PrdUnitId", "PrdUnitName" , Vouchermain.PrdUnitId); //&& c.ComId == (transactioncomid)
                                                                                                                                                ///////account head parent data for dropdown
                var ChartOfAccountParent = db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid).Where(c => c.AccId > 0 && c.AccType == "G" && c.comid == transactioncomid).Select(s => new { Text = s.AccName, Value = s.AccId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "Value", "Text");


                var Acc_ChartOfAccount_PF = db.Acc_ChartOfAccounts_PF.Where(c => c.comid == transactioncomid && c.AccId > 0 && c.AccType == "L"); //&& c.ComId == (transactioncomid)
                if (transactioncompany.isMultiDebitCredit == true)
                {
                    this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                }
                else
                {
                    this.ViewBag.Account = new SelectList(Acc_ChartOfAccount_PF.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                }


                ///////only cash item when multi debit credit of then it enable
                this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts_PF.Where(p => p.comid == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text" , Vouchermain.AccId);
                this.ViewBag.SubSectionList = db.Cat_SubSection.Where(x=>x.ComId == transactioncomid);


                this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CurrencyShortName", Vouchermain.CountryId);
                Vouchermain.AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();


                var HR_Emp_Info = db.HR_Emp_Info.Where(c => c.EmpId > 0 && c.ComId == transactioncomid).Select(s => new { Text = s.EmpCode + " - " + s.EmpName + " - " + s.Cat_Designation.DesigName, Value = s.EmpId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.EmpId = new SelectList(HR_Emp_Info, "Value", "Text");

                var Customer = db.Customers.Take(1).Where(c => c.CustomerId > 0 && c.comid == transactioncomid).Select(s => new { Text = s.CustomerCode + " - " + s.CustomerName, Value = s.CustomerId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.CustomerId = new SelectList(Customer, "Value", "Text");

                var Supplier = db.Suppliers.Take(1).Where(c => c.SupplierId > 0 && c.comid == transactioncomid).Select(s => new { Text = s.SupplierCode + s.SupplierName, Value = s.SupplierId }).ToList(); //&& c.ComId == (transactioncomid)
                this.ViewBag.SupplierId = new SelectList(Supplier, "Value", "Text");


                //Call Create View
                return View("Create", Vouchermain);
            }
            catch (Exception ex)
            {
                string abcd = ex.InnerException.InnerException.Message.ToString();
                throw ex;
            }


        }
    



        // POST: /Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        //public JsonResult DeleteConfirmed(int id)
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                //Acc_VoucherSub_PF Acc_VoucherSub_PF = db.Acc_VoucherSub_PF.Find(id);
                //db.Acc_VoucherSub_PF.Remove(Acc_VoucherSub_PF);

                Acc_VoucherMain_PF Vouchermain = await db.Acc_VoucherMain_PF.FindAsync(id);



                var CurrentVoucherSub = db.Acc_VoucherSub_PF.Include(x => x.VoucherSubChecnoes).Include(x => x.VoucherSubSections).Where(p => p.VoucherId == Vouchermain.VoucherId);
                db.Acc_VoucherSub_PF.RemoveRange(CurrentVoucherSub);


                db.Acc_VoucherMain_PF.Remove(Vouchermain);
                await db.SaveChangesAsync();


                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Vouchermain.VoucherId.ToString(), "Delete", Vouchermain.VoucherNo);



                return Json(new { Success = 1, VoucherID = Vouchermain.VoucherId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }

            
            // return RedirectToAction("Index");
        }
        public IActionResult VoucherTypeByIntegrationId(int? id)
        {
            if (id == null)
            {
                return Json(new { Success = 1, VoucherTypeName = "VJR" });
            }
            var comid = HttpContext.Session.GetString("comid");
            var exist = db.Cat_Integration_Setting_Mains.Where(x => x.IntegrationSettingMainId == id && x.Acc_VoucherType != null).FirstOrDefault();

            if (exist == null)
            {

                return Json(new { Success = 1, VoucherTypeName = "VJR" });
            }

            var VoucherTypeName = db.Cat_Integration_Setting_Mains.Include(x=>x.Acc_VoucherType).Where(x=>x.IntegrationSettingMainId == id).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;


            if (VoucherTypeName == null)
            {
                return Json(new { Success = 2, VoucherTypeName = VoucherTypeName});
            }
            else
            {
                return Json(new { Success = 1, VoucherTypeName = VoucherTypeName});
            }

        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }



        //public JsonResult getTerms(int id)
        //{
        //    var terms = db.TermsSub.Where(x => x.TermsId == id).ToList();

        //    List<SelectListItem> termssubslists = new List<SelectListItem>();

        //    //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
        //    if (terms != null)
        //    {
        //        foreach (var x in terms)
        //        {
        //            termssubslists.Add(new SelectListItem { Text = x.TermsDescription.ToString(), Value = x.Terms.ToString() });
        //        }
        //    }
        //    return Json(new SelectList(termssubslists, "Value", "Text"));
        //}

        //public JsonResult getProduct(int id)
        //{
        //    var product = db.Products.Where(x => x.CategoryId == id).ToList();

        //    List<SelectListItem> licities = new List<SelectListItem>();

        //    //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
        //    if (product != null)
        //    {
        //        foreach (var x in product)
        //        {
        //            licities.Add(new SelectListItem { Text = x.ProductName, Value = x.ProductId.ToString() });
        //        }
        //    }
        //    return Json(new SelectList(licities, "Value", "Text"));
        //}


        //public JsonResult getBarcode(int id)
        //{
        //    var product = db.Products.Where(x => x.ProductId == id).ToList();

        //    List<SelectListItem> barcodelist = new List<SelectListItem>();

        //    //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
        //    if (product != null)
        //    {
        //        foreach (var x in product)
        //        {
        //            barcodelist.Add(new SelectListItem { Text = x.ProductBarcode, Value = x.ProductId.ToString() });
        //        }
        //    }
        //    return Json(new SelectList(barcodelist, "Value", "Text"));
        //}

        //public JsonResult getProductSerial(int id)
        //{
        //    var product = db.ProductSerial.Where(x => x.ProductId == id).ToList();

        //    List<SelectListItem> productseriallist = new List<SelectListItem>();

        //    //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
        //    if (product != null)
        //    {
        //        foreach (var x in product)
        //        {
        //            productseriallist.Add(new SelectListItem { Text = x.ProductSerialNo, Value = x.ProductSerialId.ToString() });
        //        }
        //    }
        //    return Json(new SelectList(productseriallist, "Value", "Text"));
        //}


        //[HttpPost]
        //public JsonResult ProductInfo(int id)
        //{
        //    try
        //    {

        //        // 
        //        //


        //        //context.ContextOptions.ProxyCreationEnabled = false;
        //        //context.ContextOptions.LazyLoadingEnabled = false;

        //        var product = db.Products.Where(y => y.ProductId == id).SingleOrDefault();

        //        return Json(product);
        //        //return Json("tesst" );

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, values = ex.Message.ToString() });
        //    }
        //    //return Json(new SelectList(product, "Value", "Text" ));
        //}




        //[HttpPost]
        //public JsonResult SupplierInfo(int id)
        //{
        //    try
        //    {

        //        // 
        //        //

        //        var customer = db.Suppliers.Take(1).Where(y => y.SupplierId == id).SingleOrDefault();
        //        return Json(customer);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, values = ex.Message.ToString() });
        //    }
        //}


        //public static class DropDownList<T>
        //{
        //    public static SelectList LoadItems(IList<T> collection, string value, string text)
        //    {
        //        return new SelectList(collection, value, text);
        //    }
        //}


        //public ActionResult StoreProcedureReport()
        //{
        //    int userid = 1;
        //    int comid = 1;


        //    List<SqlParameter> parameters = new List<SqlParameter>();

        //    parameters.Add(new SqlParameter("@userid", userid));
        //    parameters.Add(new SqlParameter("@comid", comid));

        //    // need change
        //    ////ObjectContext dbcontext = new ObjectContext("name=MasterDetailsEntities");




        //    ////var dataset = ExecuteStoredProcedure(dbcontext, "dbo.prcgetSupplier", parameters);
        //    return View();
        //}

      


    }
}