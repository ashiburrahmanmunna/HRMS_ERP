using DataTablesParser;
using GTERP.BLL;
using GTERP.Interfaces;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRVariables;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class IssuesController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        private readonly ILogger<IssuesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IIssuesRepository _issuesRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEmpReleaseRepository _empReleaseRepository;

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }
        POSRepository POS;
        PermissionLevel PL;
        public IssuesController(
            IConfiguration configuration,
            GTRDBContext context,
            TransactionLogRepository tran,
            POSRepository _POS,
            PermissionLevel _pl,
            IIssuesRepository issuesRepository,
            ISectionRepository sectionRepository,
            IDepartmentRepository departmentRepository,
            ISupplierRepository supplierRepository,
            IEmpReleaseRepository empReleaseRepository,
            ILogger<IssuesController> logger
            )
        {
            db = context;
            tranlog = tran;
            //Repository = rep;
            POS = _POS;
            PL = _pl;
            _configuration = configuration;
            _issuesRepository = issuesRepository;
            _sectionRepository = sectionRepository;
            _departmentRepository = departmentRepository;
            _supplierRepository = supplierRepository;
            _empReleaseRepository = empReleaseRepository;
            _logger = logger;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
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
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            List<string> moduleUser = PL.GetModuleUser().ToList();

            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                if (moduleUser.Contains(c.UserID))
                {
                    var le = new AspnetUserList();
                    le.Email = c.UserName;
                    le.UserId = c.UserID;
                    le.UserName = c.UserName;
                    l.Add(le);
                }
            }

            ViewBag.Userlist =  new SelectList(l, "UserId", "UserName", userid);
           // ViewBag.Userlist = _issuesRepository.UserList();


            var lastissueMain = _issuesRepository.lastissueMain();

            if (lastissueMain != null)
            {

                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdIf();
            }
            else
            {
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();

            }

            return View();
        }


        public IActionResult Get(string UserList, string FromDate, string ToDate, string PrdUnitId, int isAll)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

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

                Microsoft.Extensions.Primitives.StringValues y = "";

                var x = Request.Form.TryGetValue("search[value]", out y);

                UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");

                if (permission.IsMedical)
                {
                    if (y.ToString().Length > 0)
                    {
                        var query = _issuesRepository.Query1();

                        var parser = new Parser<IssueResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {

                        if (PrdUnitId != null && UserList != null)
                        {
                            var querytest = _issuesRepository.QueryTest(UserList);
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (PrdUnitId != null && UserList == null)
                        {
                            var querytest = _issuesRepository.QueryTest2();

                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (PrdUnitId == null && UserList != null)
                        {

                            var querytest = _issuesRepository.QueryTest3(UserList);
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else
                        {

                            var querytest = _issuesRepository.QueryTest4();

                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }

                    }
                }
                else if (permission.IsProduction)
                {
                    if (y.ToString().Length > 0)
                    {
                        var query = _issuesRepository.Query2();


                        var parser = new Parser<IssueResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {

                        if (PrdUnitId != null && UserList != null)
                        {
                            var querytest = _issuesRepository.QueryTest5(UserList);

                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (PrdUnitId != null && UserList == null)
                        {
                            var querytest = _issuesRepository.QueryTest6();

                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (PrdUnitId == null && UserList != null)
                        {

                            var querytest = _issuesRepository.QueryTest7(UserList);
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else
                        {

                            var querytest = _issuesRepository.QueryTest8();
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }

                    }
                }
                else
                {
                    if (y.ToString().Length > 0)
                    {


                        var query = _issuesRepository.Query3();


                        var parser = new Parser<IssueResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (PrdUnitId != null && UserList != null)
                        {
                            var querytest = _issuesRepository.QueryTest9(UserList);
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (PrdUnitId != null && UserList == null)
                        {
                            var querytest = _issuesRepository.QueryTest10();
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (PrdUnitId == null && UserList != null)
                        {

                            var querytest = _issuesRepository.QueryTest11(UserList);
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else
                        {

                            var querytest = _issuesRepository.QueryTest12();
                            var parser = new Parser<IssueResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });
                //throw ex;
            }

        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueMain = _issuesRepository.Details(id);
            if (issueMain == null)
            {
                return NotFound();
            }

            return View(issueMain);
        }

        public async Task<IActionResult> GetCurrencyRate(int id)
        {
            var CurrencyRate = _issuesRepository.GetCurrency(id);
            return Json(CurrencyRate);
        }

        public IActionResult GetDepartmentByStoreReqId(int id)
        {
            var Department = _issuesRepository.GetDepartmentByStore(id);
            return Json(Department);
        }

        public JsonResult GetStoreRequisitionDataById(int? StoreReqId)
        {
            try
            {
                //List<PurchaseOrderDetailsModel> purchaseRequisitionMain = new List<PurchaseOrderDetailsModel>();


                List<IssueDetailsModel> IssueDetailsInformation = _issuesRepository.GetStoreRequisitionDataById(StoreReqId);

                return Json(IssueDetailsInformation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult GetSubStoreRequisitionDataById(int? StoreReqId)
        {
            try
            {
                List<IssueDetailsModel> IssueDetailsInformation = _issuesRepository.GetSubStoreRequisitionDataById(StoreReqId);
                return Json(IssueDetailsInformation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // GET: Issues/Create
        public IActionResult Create(bool isSubStore, bool? isRateCheck)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");

            IssueMain issuereq = new IssueMain();
            issuereq.IssueDate = DateTime.Now.Date;
            issuereq.INDate = DateTime.Now.Date;
            ViewBag.Title = "Create";
            ViewBag.IsSubStore = isSubStore;
            ViewBag.IsRateCheck = isRateCheck;

            // set products in session
            POS.SetProductInSession();

            if (isSubStore)
            {
                issuereq.IsSubStore = true;

                ViewData["SectId"] = _sectionRepository.GetSectionList();
                ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
                //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                ViewData["PaymentTypeId"] = _issuesRepository.PaymentTypeId();
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                ViewData["StoreReqId"] = _issuesRepository.StoreRequisitionList();
                ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WarehouseId > 0 && x.WhType == "L" && x.IsSubWarehouse == true);
                return View(issuereq);
            }
            else
            {
                issuereq.IsSubStore = false;

                ViewData["SectId"] = _sectionRepository.GetSectionList();
                ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
                //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                ViewData["PaymentTypeId"] = _issuesRepository.PaymentTypeId();
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                ViewData["StoreReqId"] = _issuesRepository.StoreRequisitionList();
                ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L");
                return View(issuereq);
            }


            //ViewBag.Title = "Create";
            //ViewData["SectId"] = new SelectList(db.Section.Where(x => x.ComId == comid), "SectId", "SectName");
            //ViewData["CurrencyId"] = new SelectList(db.Currency, "CurrencyId", "CurCode");
            //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName");
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid), "PrdUnitId", "PrdUnitName");
            //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid && x.Complete == 0 && x.Status > 0 && x.IsSubStore == false), "StoreReqId", "SRNo");
            //ViewData["WarehouseId"] = new SelectList(db.Warehouses.Where(x => x.comid == comid && x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
            //this.ViewBag.WarehouseList = db.Warehouses.Where(x => x.comid == comid && x.WhType == "L");
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName");
            //return View(issuereq);
        }

        public IActionResult DirectIssue(bool isSubStore)
        {

            var comid = (HttpContext.Session.GetString("comid"));
            var userid = (HttpContext.Session.GetString("userid"));
            IssueMain issueMain = new IssueMain();


            var lastissueMain = _issuesRepository.lastissueMain();

            if (lastissueMain != null)
            {

                issueMain.IssueDate = lastissueMain.IssueDate;
                issueMain.ManualSRRDate = lastissueMain.ManualSRRDate;
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdIf();
            }
            else
            {

                issueMain.IssueDate = DateTime.Now.Date;
                issueMain.INDate = DateTime.Now.Date;
                issueMain.ManualSRRDate = DateTime.Now.Date;
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();


            }


            ViewBag.IsSubStore = isSubStore;

            if (isSubStore)
            {
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.ComId == comid && x.WarehouseId != 0 && x.WhType == "L" && x.IsSubWarehouse == true);
            }
            else
            {
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.ComId == comid && x.WarehouseId != 0 && x.WhType == "L");
            }

            // set products in session
            POS.SetProductInSession();

            ViewBag.Title = "Create";
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            // ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName");
            ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _issuesRepository.PaymentTypeId();

            //ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo");
            //ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo");
            ViewData["SupplierId"] = _supplierRepository.GetSupplierList();
            ViewData["WarehouseId"] = _issuesRepository.WareHouseId();

            ViewData["BOMMainId"] = _issuesRepository.BOMMainId();

            ViewData["Employees"] = _empReleaseRepository.EmpList();
            //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x => x.comid == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");

            #region CategoryId viewbag selectlist
            List<Category> categorydb = PL.GetCategory().Where(c => c.CategoryId > 0).ToList();

            List<SelectListItem> categoryid = new List<SelectListItem>();
            //categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });

            var permission = HttpContext.Session.GetObject<UserPermission>("userpermission");
            if (!permission.IsProduction && !permission.IsMedical)
            {
                categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });
            }

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (categorydb != null)
            {
                foreach (Category x in categorydb)
                {
                    categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                }
            }
            ViewData["CategoryId"] = (categoryid);
            #endregion


            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid).Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
            ViewData["PurposeId"] = _issuesRepository.PrdUnitIdElse();

            ViewData["ProductId"] = new SelectList(db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");

            ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");

            ViewData["Patient"] = _issuesRepository.PatientId();

            ViewData["EmpId"] = _empReleaseRepository.EmpList();


            ViewData["DoctorId"] = _issuesRepository.DoctocId(); // only for medical
            return View(issueMain);
        }

        public IActionResult SubStoreCreate()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");

            IssueMain issuereq = new IssueMain();
            issuereq.IssueDate = DateTime.Now.Date;
            issuereq.IsSubStore = true;


            ViewBag.Title = "Create";
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _issuesRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
            ViewData["StoreReqId"] = _issuesRepository.StoreRequisitionList();

            ViewData["WarehouseId"] = _issuesRepository.WareHouseId2();
            this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L");
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName");
            return View(issuereq);
        }

        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IssueMain issueMain)
        {
            try
            {
                var result = "";

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var pcname = HttpContext.Session.GetString("pcname");
                var nowdate = DateTime.Now;

                // Duplicate Issue no check

                var duplicateDocument = _issuesRepository.DuplicateDocument(issueMain);
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = issueMain.IssueNo + " already exist. Document No can not Duplicate." });
                }

                var activefiscalmonth = _issuesRepository.AccFiscalMonth(issueMain);
                if (activefiscalmonth == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                }
                var activefiscalyear = _issuesRepository.AccFiscalYear(issueMain);
                if (activefiscalyear == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                }


                var lockCheck = _issuesRepository.LockCheck(issueMain);

                if (lockCheck != null)
                {
                    return Json(new { Success = 0, ex = "Store Lock this date!!!" });
                }

                //foreach (var item in issueMain.IssueSub)
                //{
                //    if (item.IssueQty==0)
                //    {
                //        item.TotalValue = item.Rate;
                //    }
                //}


                using (var tr = db.Database.BeginTransaction())
                {
                    if (issueMain != null)
                    {

                        try
                        {
                            _issuesRepository.CreateIssueMain(issueMain);

                        }
                        catch (SqlException ex)
                        {

                            Console.WriteLine(ex.Message);
                            tr.Rollback();
                            return Json(new { Success = 0, ex = ex });

                        }
                    }
                    tr.Commit();
                }



                //if (issueMain.IssueMainId > 0)
                //{

                //    var CurrentIssueSub = db.IssueSub.Include(x => x.IssueSubWarehouse).Where(p => p.IssueMainId == issueMain.IssueMainId);
                //    //db.IssueSub.RemoveRange(CurrentIssueSub);
                //    //db.SaveChanges();

                //    foreach (IssueSub ss in CurrentIssueSub)
                //    {
                //        db.IssueSub.Remove(ss);
                //    }
                //    db.SaveChanges();


                //    foreach (IssueSub item in issueMain.IssueSub)
                //    {
                //        if (item.IssueSubId > 0)
                //        {
                //            //foreach (IssueSubWarehouse itemwarehouse in item.IssueSubWarehouse)
                //            //{
                //            //    if (itemwarehouse.IssueSubWarehouseId > 0)
                //            //    {
                //            //        db.Entry(itemwarehouse).State = EntityState.Modified;
                //            //    }
                //            //    else
                //            //    {
                //            //        db.IssueSubWarehouse.Add(itemwarehouse);
                //            //    }
                //            //}

                //            //item.DateUpdated = date;
                //            //item.UpdateByUserId = userid;
                //            //db.Entry(item).State = EntityState.Modified;


                //            ////////////////////////////////////////////////////// fahad



                //            //foreach (Acc_VoucherSubCheckno ss in CurrentVoucherCheck)
                //            //    db.Acc_VoucherSubs.Remove(ss);
                //            //db.SaveChanges();


                //            foreach (IssueSub ss in issueMain.IssueSub)
                //            {
                //                //var CurrentIssueSubWarehouse = db.IssueSubWarehouse.Where(p => p.IssueSubId == ss.IssueSubId);
                //                //db.IssueSubWarehouse.RemoveRange(CurrentIssueSubWarehouse);
                //                //db.SaveChanges();



                //                //////foreach (IssueSubWarehouse sss in ss.IssueSubWarehouse)
                //                //////{
                //                //////    //sss.IssueSubId = issuesubid;
                //                //////    sss.IssueSubWarehouseId = 0;

                //                //////    //db.IssueSubWarehouse.Add(sss);
                //                //////}

                //                ////////if (ss.VoucherSubId > 0)
                //                ////////{
                //                ////////db.Entry(ss).State = EntityState.Modified;
                //                //////ss.IssueSubId = 0;
                //                ////////db.IssueSub.Add(ss);
                //                ////////db.SaveChanges();
                //                ////////db.Entry(ss).GetDatabaseValues();
                //                ////////int issuesubid = ss.IssueSubId;
                //                // Yes it's here
                //                //}
                //                //else
                //                //{
                //                //    //db.Acc_VoucherSubs.Add(ss);
                //                //    db.Acc_VoucherSubs.Add(ss);
                //                //}


                //                foreach (IssueSubWarehouse sss in ss.IssueSubWarehouse)
                //                {
                //                    //sss.IssueSubId = issuesubid;
                //                    sss.IssueSubWarehouseId = 0;

                //                    //db.IssueSubWarehouse.Add(sss);
                //                }
                //                item.IssueSubId = 0;
                //                db.IssueSub.Add(item);

                //            }
                //            ////////////////////////////////////////////////////////////////////////// fahad

                //        }
                //        else
                //        {
                //            //item.ComId = comid;
                //            //item.UserId = userid;
                //            //item.PcName = pcname;
                //            //item.DateAdded = date;
                //            db.IssueSub.Add(item);
                //        }
                //    }
                //    issueMain.UpdateByUserId = userid;
                //    issueMain.DateUpdated = date;
                //    db.Entry(issueMain).State = EntityState.Modified;
                //    result = "2";
                //}
                //else
                //{
                //    issueMain.ComId = comid;
                //    issueMain.UserId = userid;
                //    issueMain.PcName = pcname;
                //    issueMain.DateAdded = date;
                //    //issueMain.IssueSub.FirstOrDefault().ComId = comid;
                //    //issueMain.IssueSub.FirstOrDefault().UserId = userid;
                //    //issueMain.IssueSub.FirstOrDefault().PcName = pcname;
                //    //issueMain.IssueSub.FirstOrDefault().DateAdded = date;

                //    db.Add(issueMain);
                //    result = "1";
                //}
                //db.SaveChanges();
                //}
                ViewData["SectId"] = _sectionRepository.GetSectionList();
                ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                ViewData["PurReqId"] = _issuesRepository.PrdRqId(issueMain);
                //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);
                return Json(new { Success = result, ex = "" });
            }
            catch (Exception ex)
            {

                //throw ex;
                return Json(new { Success = 0, ex = ex });
            }
        }

        public IActionResult GetCategoryProductInfo(int id)
        {
            var prdunits = db.PrdUnits.Find(id);
            string comid = HttpContext.Session.GetString("comid");

            if (prdunits == null)
            {

                prdunits = db.PrdUnits.Where(x => x.PrdUnitId > 0).OrderBy(x => x.PrdUnitId).FirstOrDefault();
            }
            if (prdunits.PrdUnitName.ToUpper().Contains("MEDICAL"))// only for medical
            {
                var categorys = db.Categories.Where(X => X.ComId == comid)
                    .Select(c => new
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name
                    }).Where(c => c.Name.ToUpper().Contains("MEDICAL")).ToList();

                var products = db.Products.Where(X => X.comid == comid)
                    .Select(c => new
                    {
                        ProductId = c.ProductId,
                        CategoryId = c.CategoryId,
                        Name = c.ProductName + " [ " + c.ProductCode + " ]"
                    }).Where(p => p.CategoryId == categorys.FirstOrDefault().CategoryId).ToList();

                var warehouses = db.Warehouses.Where(X => X.ComId == comid).Where(x => x.WhType == "L" && x.IsMedicalWarehouse == true && x.IsConsumableWarehouse == true)                  //.Where(x => x.WhName.ToUpper().Contains("Med"))
                .Select(w => new
                {
                    WarehouseId = w.WarehouseId,
                    WhName = w.WhName
                }).ToList();

                return Json(new { Categorys = categorys, Products = products, Warehouses = warehouses });
            }
            if (prdunits.PrdUnitName.ToUpper().Contains("UNIT 1") || prdunits.PrdUnitName.ToUpper().Contains("UNIT 2"))// only for medical
            {
                var categorys = db.Categories.Where(X => X.ComId == comid).Select(c => new
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList();

                var products = db.Products.Where(X => X.comid == comid).Take(1) //.Include(a=>a.vPrimaryCategory)
                    .Select(c => new
                    {
                        ProductId = c.ProductId,
                        CategoryId = c.CategoryId,
                        Name = c.ProductName
                    }).Where(p => p.CategoryId == categorys.FirstOrDefault().CategoryId).ToList();

                var wh = prdunits.PrdUnitName.ToUpper().Contains("UNIT 1") ? "DAP-1" : "DAP-2";

                var warehouses = db.Warehouses.Where(X => X.ComId == comid)
                    .Where(x => x.WhType == "L" && x.WhName == wh && x.IsProductionWarehouse == true && x.IsConsumableWarehouse == true)     //.Where(x => x.WhName.ToUpper().Contains("Med"))
                    .Select(w => new
                    {
                        WarehouseId = w.WarehouseId,
                        WhName = w.WhName
                    }).ToList();

                return Json(new { Categorys = categorys, Products = products, Warehouses = warehouses });

            }
            else
            {

                #region CategoryId viewbag selectlist
                //List<Category> categorydb = POS.GetCategory(comid).ToList();

                //List<SelectListItem> categoryid = new List<SelectListItem>();
                //categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
                //categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                //if (categorydb != null)
                //{
                //    foreach (Category x in categorydb)
                //    {
                //        categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                //    }
                //}
                //ViewData["CategoryId"] = (categoryid);
                #endregion

                var categorys = db.Categories.Where(X => X.ComId == comid).Select(c => new
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList();


                //categorys.Add(new { CategoryId = -1, Name = "--Please Select--" });
                categorys.Add(new { CategoryId = 0, Name = "=ALL=" });

                var categorylist = categorys.OrderBy(x => x.CategoryId);


                var warehouses = db.Warehouses.Where(X => X.ComId == comid).Where(x => x.WhType == "L" && x.WarehouseId != 0 && x.IsConsumableWarehouse == false).Select(w => new
                {
                    WarehouseId = w.WarehouseId,
                    WhName = w.WhName,
                    WhType = w.WhType,
                    ParentId = w.ParentId
                }).ToList();
                //db.Warehouses.Where(x => x.comid == comid && x.WhType == "L" && x.ParentId != null)
                //db.Products.Where(p => p.CategoryId == categorys.FirstOrDefault().CategoryId).ToList();
                return Json(new { Categorys = categorylist, Products = "", Warehouses = warehouses });
            }

        }

        public ActionResult Print(int? id, string type)
        {

            string callBackUrl = _issuesRepository.Print(id, type);
            return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }

        public IActionResult GetInventory(int prdId, int whId)
        {
            string comid = HttpContext.Session.GetString("comid");
            //var inv = db.Inventory.Where(i => i.WareHouseId == whId && i.ProductId == prdId && i.comid==comid)
            //   .Select(w => new
            //   {
            //       WareHouseId = w.WareHouseId,
            //       ProductId = w.ProductId,
            //       CurrentStock = w.CurrentStock
            //   }).FirstOrDefault();
            //return Json(inv);


            var inv = db.Products.Include(x => x.vProductUnit).Include(x => x.CostCalculated).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                WareHouseId = whId,
                AvgRate = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CalculatedPrice).FirstOrDefault().ToString(), //lastpurchaseprice,
                CurrentStock = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Sum(x => x.CurrQty).ToString()
            }).Where(p => p.ProductId == prdId).FirstOrDefault();// ToList();

            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            return Json(inv);

        }

        public IActionResult GetBOMProduct(int prdUnitId)
        {
            string comid = HttpContext.Session.GetString("comid");
            var bomSub = (from bom in db.BOMSub
                          join bomMain in db.BOMMain on bom.BOMMainId equals bomMain.BOMMainId
                          join p in db.Products on bom.InvProductId equals p.ProductId
                          join u in db.Unit on p.UnitId equals u.UnitId
                          join w in db.Warehouses on bom.WarehouseId equals w.WarehouseId
                          where bomMain.PrdUnitId == prdUnitId && bom.ComId == comid

                          select new
                          {
                              BOMMainId = bom.BOMMainId,
                              BOMSubId = bom.BOMSubId,
                              ProductId = bom.InvProductId,
                              Consumption = bom.Consumption,
                              Remarks = bom.Remarks,
                              ProductName = p.ProductName,
                              UnitName = u.UnitName,
                              WarehouseId = w.WarehouseId,
                              WhName = w.WhName,
                              SLNo = bom.SLNo

                          }).OrderBy(x => x.SLNo).ToList();

            return Json(bomSub);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> View(int? id, bool isRateCheck)
        {
            try
            {


                if (id == null)
                {
                    return NotFound();
                }
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var pcname = HttpContext.Session.GetString("pcname");

                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L");


                ViewBag.Title = "Edit";
                var issueMain = await db.IssueMain
                    .Include(p => p.IssueSub)
                    .ThenInclude(p => p.vProduct)
                    .ThenInclude(p => p.vProductUnit)
                    .Include(g => g.IssueSub)
                    .ThenInclude(g => g.vWarehouse)
                    .Include(p => p.IssueSub)
                    .ThenInclude(p => p.IssueSubWarehouse)
                    .ThenInclude(p => p.vWarehouse)
                    .FirstOrDefaultAsync(m => m.IssueMainId == id);
                if (issueMain == null)
                {
                    return NotFound();
                }

                // set products in session
                POS.SetProductInSession();

                var quary = $"EXEC IssueDetailsInformation '{comid}','{userid}',{issueMain.StoreReqId},{issueMain.IssueMainId}";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@comid", comid);
                parameters[1] = new SqlParameter("@userid", userid);
                parameters[2] = new SqlParameter("@StoreReqId", issueMain.StoreReqId.ToString());
                parameters[3] = new SqlParameter("@IssueId", issueMain.IssueMainId);
                parameters[4] = new SqlParameter("@IsSubStore", issueMain.IsSubStore);
                List<IssueDetailsModel> IssueDetailsInformation = Helper.ExecProcMapTList<IssueDetailsModel>("IssueDetailsInformation", parameters);

                //if (IssueDetailsInformation.Count > 0)
                //{
                //    foreach (var DBitem in issueMain.IssueSub)
                //    {

                //        var abc = IssueDetailsInformation.Where(x => x.ProductId == DBitem.ProductId);
                //        //if (abc.FirstOrDefault().RemainingReqQty != null)
                //        {
                //            //DBitem.RemainingReqQty = abc.FirstOrDefault().RemainingReqQty ? 0;
                //            DBitem.RemainingReqQty = abc != null ? abc.FirstOrDefault().RemainingReqQty : 0;

                //        }
                //    }

                //}

                ViewBag.IsSubStore = issueMain.IsSubStore;
                ViewBag.IsRateCheck = isRateCheck;

                if (issueMain.IsSubStore)
                {
                    ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", issueMain.SectId);
                    ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", issueMain.DeptId);
                    //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                    ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", issueMain.CurrencyId);
                    //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                    ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitShortName", issueMain.PrdUnitId);
                    //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid), "StoreReqId", "SRNo", issueMain.StoreReqId);

                    var storeRequisitions = db.StoreRequisitionMain
                        .Where(x => x.ComId == comid && x.Complete == 0 && x.Status > 0 && x.IsSubStore == true)
                        .Select(s => new { Value = s.StoreReqId, Text = s.SRNo + " [S.S.]" });

                    ViewData["StoreReqId"] = new SelectList(storeRequisitions, "Value", "Text", issueMain.StoreReqId);

                    //Need to change himu

                    //ViewData["WarehouseId"] = new SelectList(db.Warehouses.Where(x => x.comid == comid && x.WhType == "L" && x.ParentId != null && x.IsSubWarehouse == true), "WarehouseId", "WhShortName" );

                    ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid && x.Complete == 0 && x.Status > 0), "StoreReqId", "SRNo", issueMain.StoreReqId);
                    ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.ComId == comid && x.WarehouseId != 0 && x.WhType == "L" && x.ParentId != null && x.IsSubWarehouse == true), "WarehouseId", "WhShortName");
                    //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);
                    if (issueMain.IsDirectIssue)
                    {
                        ViewData["Patient"] = new SelectList(db.Cat_Variable.Where(c => c.VarType == "ReleationType").OrderBy(c => c.SLNo), "VarName", "VarName");
                        var empInfo = (from emp in db.HR_Emp_Info
                                       join d in db.Cat_Department on emp.DeptId equals d.DeptId
                                       join s in db.Cat_Section on emp.SectId equals s.SectId
                                       join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                                       where emp.ComId == comid
                                       select new
                                       {
                                           Value = emp.EmpId,
                                           Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                                       }).ToList();

                        ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", issueMain.EmpId);
                        ViewData["BOMMainId"] = new SelectList(db.BOMMain.Where(x => x.ComId == comid), "BOMMainId", "AssembleName", issueMain.BOMMainId);

                        var doctorInfo = (from emp in db.HR_Emp_Info
                                          join d in db.Cat_Department on emp.DeptId equals d.DeptId
                                          join s in db.Cat_Section on emp.SectId equals s.SectId
                                          join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                                          where emp.ComId == comid && des.DesigName.ToUpper().Contains("MEDICAL OFFICER")
                                          orderby emp.EmpCode
                                          select new
                                          {
                                              Value = emp.EmpId,
                                              Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                                          }).ToList();

                        ViewData["DoctorId"] = new SelectList(doctorInfo, "Value", "Text", issueMain.DoctorId);

                        #region CategoryId viewbag selectlist
                        List<Category> categorydb = POS.GetCategory(comid).ToList();

                        List<SelectListItem> categoryid = new List<SelectListItem>();
                        //categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
                        categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



                        //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                        if (categorydb != null)
                        {
                            foreach (Category x in categorydb)
                            {
                                categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                            }
                        }
                        ViewData["CategoryId"] = (categoryid);
                        #endregion

                        return View("ViewDirectIssue", issueMain);
                    }
                    return View("ViewCreate", issueMain);
                }
                else
                {
                    ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", issueMain.SectId);
                    ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", issueMain.DeptId);
                    //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                    ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", issueMain.CurrencyId);
                    //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                    ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitShortName", issueMain.PrdUnitId);
                    //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid), "StoreReqId", "SRNo", issueMain.StoreReqId);
                    var storeRequisitions = db.StoreRequisitionMain
                       .Where(x => x.ComId == comid && x.Complete == 0 && x.Status > 0 && x.IsSubStore == true)
                       .Select(s => new { Value = s.StoreReqId, Text = s.SRNo });

                    ViewData["StoreReqId"] = new SelectList(storeRequisitions, "Value", "Text", issueMain.StoreReqId);

                    ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
                    //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);

                    if (issueMain.IsDirectIssue)
                    {
                        ViewData["Patient"] = new SelectList(db.Cat_Variable.Where(c => c.VarType == "ReleationType").OrderBy(c => c.SLNo), "VarName", "VarName");
                        var empInfo = (from emp in db.HR_Emp_Info
                                       join d in db.Cat_Department on emp.DeptId equals d.DeptId
                                       join s in db.Cat_Section on emp.SectId equals s.SectId
                                       join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                                       where emp.ComId == comid
                                       select new
                                       {
                                           Value = emp.EmpId,
                                           Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                                       }).ToList();

                        ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", issueMain.EmpId);

                        var doctorInfo = (from emp in db.HR_Emp_Info
                                          join d in db.Cat_Department on emp.DeptId equals d.DeptId
                                          join s in db.Cat_Section on emp.SectId equals s.SectId
                                          join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                                          where emp.ComId == comid && des.DesigName.ToUpper().Contains("MEDICAL OFFICER")
                                          orderby emp.EmpCode
                                          select new
                                          {
                                              Value = emp.EmpId,
                                              Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                                          }).ToList();

                        ViewData["DoctorId"] = new SelectList(doctorInfo, "Value", "Text", issueMain.DoctorId);
                        ViewData["BOMMainId"] = new SelectList(db.BOMMain.Where(x => x.ComId == comid), "BOMMainId", "AssembleName", issueMain.BOMMainId);

                        #region CategoryId viewbag selectlist
                        List<Category> categorydb = POS.GetCategory(comid).ToList();

                        List<SelectListItem> categoryid = new List<SelectListItem>();
                        //categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
                        categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



                        //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                        if (categorydb != null)
                        {
                            foreach (Category x in categorydb)
                            {
                                categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                            }
                        }
                        ViewData["CategoryId"] = (categoryid);
                        #endregion


                        return View("ViewDirectIssue", issueMain);

                    }
                    return View("ViewCreate", issueMain);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        // GET: Issues/Edit/5
        public IActionResult Edit(int? id, bool isRateCheck)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var pcname = HttpContext.Session.GetString("pcname");

                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L");


                ViewBag.Title = "Edit";
                var issueMain = _issuesRepository.FindData2(id);
                if (issueMain == null)
                {
                    return NotFound();
                }

                // set products in session
                POS.SetProductInSession();


                List<IssueDetailsModel> IssueDetailsInformation = _issuesRepository.IssueDetailsInformation(id);

                //if (IssueDetailsInformation.Count > 0)
                //{
                //    foreach (var DBitem in issueMain.IssueSub)
                //    {

                //        var abc = IssueDetailsInformation.Where(x => x.ProductId == DBitem.ProductId);
                //        //if (abc.FirstOrDefault().RemainingReqQty != null)
                //        {
                //            //DBitem.RemainingReqQty = abc.FirstOrDefault().RemainingReqQty ? 0;
                //            DBitem.RemainingReqQty = abc != null ? abc.FirstOrDefault().RemainingReqQty : 0;

                //        }
                //    }

                //}

                ViewBag.IsSubStore = issueMain.IsSubStore;
                ViewBag.IsRateCheck = isRateCheck;

                if (issueMain.IsSubStore)
                {
                    ViewData["SectId"] = _sectionRepository.GetSectionList();
                    ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
                    //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                    ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                    //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                    ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                    //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid), "StoreReqId", "SRNo", issueMain.StoreReqId);

                    var storeRequisitions = _issuesRepository.StoreRequisition();

                    ViewData["StoreReqId"] = _issuesRepository.StoreReqId(id);

                    //Need to change himu

                    //ViewData["WarehouseId"] = new SelectList(db.Warehouses.Where(x => x.comid == comid && x.WhType == "L" && x.ParentId != null && x.IsSubWarehouse == true), "WarehouseId", "WhShortName" );

                    ViewData["StoreReqId"] = _issuesRepository.StoreReqId(id);
                    ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
                    //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);
                    if (issueMain.IsDirectIssue)
                    {
                        ViewData["Patient"] = _issuesRepository.PatientId();


                        ViewData["EmpId"] = _empReleaseRepository.EmpList();
                        ViewData["BOMMainId"] = _issuesRepository.BOMMainId();


                        ViewData["DoctorId"] = _issuesRepository.DoctocId();

                        #region CategoryId viewbag selectlist

                        ViewData["CategoryId"] = _issuesRepository.CategoryId();
                        #endregion

                        return View("DirectIssue", issueMain);
                    }
                    return View("Create", issueMain);
                }
                else
                {
                    ViewData["SectId"] = _sectionRepository.GetSectionList();
                    ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
                    //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                    ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                    //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                    ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                    //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid), "StoreReqId", "SRNo", issueMain.StoreReqId);
                    var storeRequisitions = _issuesRepository.StoreRequisition();

                    ViewData["StoreReqId"] = _issuesRepository.StoreReqId(id);

                    ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
                    //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);

                    if (issueMain.IsDirectIssue)
                    {
                        ViewData["Patient"] = _issuesRepository.PatientId();

                        ViewData["EmpId"] = _empReleaseRepository.EmpList();
                        ViewData["DoctorId"] = _issuesRepository.DoctocId();
                        ViewData["BOMMainId"] = _issuesRepository.BOMMainId();

                        #region CategoryId viewbag selectlist

                        ViewData["CategoryId"] = _issuesRepository.CategoryId();
                        #endregion


                        return View("DirectIssue", issueMain);

                    }
                    return View("Create", issueMain);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IssueMain issueMain)
        {
            if (id != issueMain.IssueMainId)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");

            if (ModelState.IsValid)
            {
                try
                {
                    _issuesRepository.Update(issueMain);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueMainExists(issueMain.IssueMainId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
            //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
            ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
            ViewData["StoreReqId"] = _issuesRepository.StoreReqId(id);
            ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "City", issueMain.SupplierId);
            return View(issueMain);
        }

        // GET: Issues/Delete/5
        public IActionResult Delete(int? id, bool isRateCheck)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");

            this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L");


            ViewBag.Title = "Delete";
            var issueMain = _issuesRepository.FindData2(id);
            if (issueMain == null)
            {
                return NotFound();
            }

            // set products in session
            POS.SetProductInSession();

            ViewBag.IsSubStore = issueMain.IsSubStore;
            ViewBag.IsRateCheck = isRateCheck;

            if (issueMain.IsSubStore)
            {
                ViewData["SectId"] = _sectionRepository.GetSectionList();
                ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
                //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");
                ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid), "StoreReqId", "SRNo", issueMain.StoreReqId);

                var storeRequisitions = _issuesRepository.StoreRequisition();

                ViewData["StoreReqId"] = _issuesRepository.StoreRequisitionList();

                //ViewData["WarehouseId"] = new SelectList(db.Warehouses.Where(x => x.comid == comid && x.WhType == "L" && x.ParentId != null && x.IsSubWarehouse == true), "WarehouseId", "WhShortName");

                ViewData["StoreReqId"] = _issuesRepository.StoreReqId(id);
                ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
                //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);
                if (issueMain.IsDirectIssue)
                {
                    ViewData["Patient"] = _issuesRepository.PatientId();
                    ViewData["EmpId"] = _empReleaseRepository.EmpList();
                    ViewData["DoctorId"] = _issuesRepository.DoctocId();
                    ViewData["BOMMainId"] = _issuesRepository.BOMMainId();

                    #region CategoryId viewbag selectlist
                    var categoryid = _issuesRepository.CategoryId();
                    ViewData["CategoryId"] = (categoryid);
                    #endregion

                    return View("DirectIssue", issueMain);
                }
                return View("Create", issueMain);
            }
            else
            {
                ViewData["SectId"] = _sectionRepository.GetSectionList();
                //ViewData["SubSectId"] = new SelectList(db.Cat_SubSection.Where(x => x.ComId == comid), "SubSectId", "SubSectName");

                ViewData["CurrencyId"] = _issuesRepository.CurrencyId();
                //ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", issueMain.PaymentTypeId);
                ViewData["PrdUnitId"] = _issuesRepository.PrdUnitIdElse();
                //ViewData["StoreReqId"] = new SelectList(db.StoreRequisitionMain.Where(x => x.ComId == comid), "StoreReqId", "SRNo", issueMain.StoreReqId);
                var storeRequisitions = _issuesRepository.StoreRequisition();

                ViewData["StoreReqId"] = _issuesRepository.StoreReqId(id);

                ViewData["WarehouseId"] = _issuesRepository.WareHouseId();
                //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", issueMain.SupplierId);
                if (issueMain.IsDirectIssue)
                {
                    ViewData["Patient"] = _issuesRepository.PatientId();

                    ViewData["EmpId"] = _empReleaseRepository.EmpList();


                    ViewData["DoctorId"] = _issuesRepository.DoctocId();
                    ViewData["BOMMainId"] = _issuesRepository.BOMMainId();
                    #region CategoryId viewbag selectlist

                    ViewData["CategoryId"] = _issuesRepository.CategoryId();
                    #endregion
                    return View("DirectIssue", issueMain);
                }
                return View("Create", issueMain);
            }
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {


                //var issueMain = await db.IssueMain.FindAsync(id);
                var issueMain = _issuesRepository.FindData(id);
                if (issueMain == null)
                {
                    return NotFound();
                }
                _issuesRepository.DeleteData(id);

                return Json(new { Success = 1, ex = "Data Delete Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex });

                throw ex;
            }
            //return RedirectToAction(nameof(Index));
        }

        public JsonResult GetProducts(int? id)
        {
            var product = _issuesRepository.GetProducts(id);
            return Json(new { item = product });
        }

        private bool IssueMainExists(int id)
        {
            return _issuesRepository.IssueMainExists(id);
        }

        [HttpPost, ActionName("PrintIssueSummary")]
        public JsonResult PrintIssueSummary(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId)
        {
            try
            {

                var redirectUrl = _issuesRepository.PrintIssueSummary(rptFormat, action, FromDate, ToDate, PrdUnitId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }

        [HttpPost, ActionName("PrintIssueDetails")]
        public JsonResult IssueDetailsReport(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId)
        {
            try
            {
                string redirectUrl = "";
                redirectUrl = _issuesRepository.IssueDetailsReport(rptFormat, action, FromDate, ToDate, PrdUnitId);

                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }

        [HttpPost, ActionName("PrintIssueVoucher")]
        public JsonResult PrintIssueVoucher(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId)
        {
            try
            {
                string redirectUrl = _issuesRepository.PrintIssueVoucher(rptFormat, action, FromDate, ToDate, PrdUnitId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }

        [HttpPost, ActionName("PrintMissingSequence")]
        public JsonResult PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, string PrdUnitId)
        {
            try
            {

                string redirectUrl = _issuesRepository.PrintMissingSequence(rptFormat, action, Type, FromNo, ToNo, PrdUnitId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }
        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

    }
}
