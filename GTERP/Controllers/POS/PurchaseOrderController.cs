#region Assembly refference
using DataTablesParser;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;
#endregion

namespace GTERP.Controllers.POS
{
    public class PurchaseOrderController : Controller
    {
        #region Feild and constructor
        private readonly GTRDBContext db;
        private readonly IConfiguration _configuration;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; }
        [OverridableAuthorize]


        public PurchaseOrderController(GTRDBContext context, IConfiguration configuration)
        {
            db = context;
            _configuration = configuration;
            //Repository = rep;
        }

        #endregion

        // GET: PurchaseOrder
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
            //Uri url = new Uri(string.Format("https://localhost:44336/api/user/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }

            ViewBag.Userlist = new SelectList(l, "UserId", "UserName", userid);

            //return View(await gTRDBContext.ToListAsync());
            return View();

        }

        public partial class PurchaseOrderResult
        {
            public int PurOrderMainId { get; set; }

            public string PONo { get; set; }

            public string Refference { get; set; }

            public string PODate { get; set; }

            public string Department { get; set; }

            public string SupplierName { get; set; }
            public string TypeName { get; set; }
            public string CurCode { get; set; }
            public float ConvertionRate { get; set; }
            public float TotalPOValue { get; set; }
            public float? Deduction { get; set; }
            public float? NetPOValue { get; set; }


            public string SectName { get; set; }

            public string Status { get; set; }


            public string LastDateOfDelivery { get; set; }

            public string ExpectedRecivedDate { get; set; }
        }
        public IActionResult Get(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
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

                if (y.ToString().Length > 0)
                {


                    var query = from e in db.PurchaseOrderMain.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.PurOrderMainId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                select new PurchaseOrderResult
                                {
                                    PurOrderMainId = e.PurOrderMainId,
                                    PONo = e.PONo,
                                    PODate = e.PODate.ToString("dd-MMM-yy"),
                                    Department = e.Department.DeptName,
                                    SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                    TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                    CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                    ConvertionRate = e.ConvertionRate,
                                    TotalPOValue = e.TotalPOValue,
                                    Deduction = e.Deduction,
                                    NetPOValue = e.NetPOValue,
                                    SectName = e.Section != null ? e.Section.SectName : "",
                                    Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                    LastDateOfDelivery = e.LastDateOfDelivery.ToString("dd-MMM-yy"),
                                    ExpectedRecivedDate = ((DateTime)e.ExpectedRecivedDate).ToString("dd-MMM-yy")
                                };


                    var parser = new Parser<PurchaseOrderResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = from e in db.PurchaseOrderMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                        //.Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.PurOrderMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderResult
                                        {
                                            PurOrderMainId = e.PurOrderMainId,
                                            PONo = e.PONo,
                                            PODate = e.PODate.ToString("dd-MMM-yy"),
                                            Department = e.Department.DeptName,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            LastDateOfDelivery = e.LastDateOfDelivery.ToString("dd-MMM-yy"),
                                            ExpectedRecivedDate = ((DateTime)e.ExpectedRecivedDate).ToString("dd-MMM-yy")
                                        };


                        var parser = new Parser<PurchaseOrderResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = from e in db.PurchaseOrderMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        // .Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.PurOrderMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderResult
                                        {
                                            PurOrderMainId = e.PurOrderMainId,
                                            PONo = e.PONo,
                                            PODate = e.PODate.ToString("dd-MMM-yy"),
                                            Department = e.Department.DeptName,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            LastDateOfDelivery = e.LastDateOfDelivery.ToString("dd-MMM-yy"),
                                            ExpectedRecivedDate = ((DateTime)e.ExpectedRecivedDate).ToString("dd-MMM-yy")
                                        };


                        var parser = new Parser<PurchaseOrderResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = from e in db.PurchaseOrderMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                        .OrderByDescending(x => x.PurOrderMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderResult
                                        {
                                            PurOrderMainId = e.PurOrderMainId,
                                            PONo = e.PONo,
                                            PODate = e.PODate.ToString("dd-MMM-yy"),
                                            Department = e.Department.DeptName,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            LastDateOfDelivery = e.LastDateOfDelivery.ToString("dd-MMM-yy"),
                                            ExpectedRecivedDate = ((DateTime)e.ExpectedRecivedDate).ToString("dd-MMM-yy")
                                        };


                        var parser = new Parser<PurchaseOrderResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in db.PurchaseOrderMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))

                        .OrderByDescending(x => x.PurOrderMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderResult
                                        {
                                            PurOrderMainId = e.PurOrderMainId,
                                            PONo = e.PONo,
                                            PODate = e.PODate.ToString("dd-MMM-yy"),
                                            Department = e.Department.DeptName,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            LastDateOfDelivery = e.LastDateOfDelivery.ToString("dd-MMM-yy"),
                                            ExpectedRecivedDate = ((DateTime)e.ExpectedRecivedDate).ToString("dd-MMM-yy")
                                        };


                        var parser = new Parser<PurchaseOrderResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });
                //throw ex;
            }

        }



        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrderMain = await db.PurchaseOrderMain
                .Include(p => p.Section)
                .Include(p => p.Currency)
                .Include(p => p.PaymentType)
                .Include(p => p.PrdUnit)
                .Include(p => p.PurchaseRequisitionMain)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.PurOrderMainId == id);
            if (purchaseOrderMain == null)
            {
                return NotFound();
            }

            return View(purchaseOrderMain);
        }

        public async Task<IActionResult> GetCurrencyRate(int id)
        {
            var CurrencyRate = db.Currency.Where(c => c.CurrencyId == id);
            return Json(CurrencyRate);
        }

        public IActionResult GetDepartmentByPurReqId(int id)
        {
            var Department = db.PurchaseRequisitionMain.Where(p => p.PurReqId == id).Select(p => new
            {
                DeptName = p.Department.DeptName,
                p.PurReqId
            }).FirstOrDefault();
            return Json(Department);
        }

        public JsonResult GetPurchaseRequisitionDataById(int? PurReqId)
        {
            //List<PurchaseOrderDetailsModel> purchaseRequisitionMain = new List<PurchaseOrderDetailsModel>();


            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            var quary = $"EXEC PurchaseOrderDetailsInformation '{comid}','{userid}',{PurReqId}";

            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@PurReqId", PurReqId);
            List<PurchaseOrderDetailsModel> PurchaseOrderDetailsInformation = Helper.ExecProcMapTList<PurchaseOrderDetailsModel>("PurchaseOrderDetailsInformation", parameters);


            return Json(PurchaseOrderDetailsInformation);
        }
        public class PurchaseOrderDetailsModel
        {
            public int? PurOrderMainId { get; set; }
            public int? PurOrderSubId { get; set; }
            public int? ProductId { get; set; }
            public int? UnitId { get; set; }
            public string SLNo { get; set; }
            public string ProductName { get; set; }
            public string UnitName { get; set; }
            public decimal? PurReqQty { get; set; }
            public decimal? RequisitionQty { get; set; }
            public decimal? RemainingReqQty { get; set; }
            public decimal? PurchaseQty { get; set; }
            public decimal? Rate { get; set; }
            public decimal? TotalValue { get; set; }
            public string Remarks { get; set; }
            public int? PurReqId { get; set; }
            public int? PurReqSubId { get; set; }
        }
        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            PurchaseOrderMain abc = new PurchaseOrderMain();
            abc.PODate = DateTime.Now.Date;
            abc.LastDateOfDelivery = DateTime.Now.Date;
            abc.ExpectedRecivedDate = DateTime.Now.Date;



            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";
            ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName");
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName");
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName");
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName");
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo");
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");
            ViewData["DistrictId"] = new SelectList(db.Cat_District, "DistId", "DistName");
            return View(abc);
        }

        // POST: PurchaseOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(PurchaseOrderMain purchaseOrderMain)
        {
            var result = "";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");

            var duplicateDocument = db.PurchaseOrderMain.Where(i => i.PONo == purchaseOrderMain.PONo && i.PurReqId != purchaseOrderMain.PurReqId && i.ComId == comid).FirstOrDefault();
            if (duplicateDocument != null)
            {
                return Json(new { Success = 0, ex = purchaseOrderMain.PONo + " already exist. Document No can not be Duplicate." });
            }



            DateTime date = purchaseOrderMain.PODate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var activefiscalmonth = db.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
            if (activefiscalmonth == null)
            {
                return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
            }
            var activefiscalyear = db.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
            if (activefiscalyear == null)
            {
                return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
            }


            var lockCheck = db.HR_ProcessLock
                .Where(p => p.LockType.Contains("Store Lock") && p.DtDate.Date <= purchaseOrderMain.PODate.Date && p.DtToDate.Value.Date >= purchaseOrderMain.PODate.Date
                    && p.IsLock == true).FirstOrDefault();

            if (lockCheck != null)
            {
                return Json(new { Success = 0, ex = "Store Lock this date!!!" });
            }

            if (ModelState.IsValid)
            {


                var nowdate = DateTime.Now;
                if (purchaseOrderMain.PurOrderMainId > 0)
                {

                    purchaseOrderMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                    purchaseOrderMain.FiscalYearId = activefiscalyear.FiscalYearId;

                    foreach (PurchaseOrderSub item in purchaseOrderMain.PurchaseOrderSub)
                    {
                        if (item.PurOrderSubId > 0)
                        {
                            item.DateUpdated = nowdate;
                            item.UpdateByUserId = userid;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            item.ComId = comid;
                            item.UserId = userid;
                            item.PcName = pcname;
                            item.DateAdded = nowdate;
                            db.PurchaseOrderSub.Add(item);
                        }
                    }
                    purchaseOrderMain.UpdateByUserId = userid;
                    purchaseOrderMain.DateUpdated = nowdate;
                    db.Entry(purchaseOrderMain).State = EntityState.Modified;
                    result = "2";
                }
                else
                {

                    purchaseOrderMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                    purchaseOrderMain.FiscalYearId = activefiscalyear.FiscalYearId;

                    purchaseOrderMain.ComId = comid;
                    purchaseOrderMain.UserId = userid;
                    purchaseOrderMain.PcName = pcname;
                    purchaseOrderMain.DateAdded = date;
                    purchaseOrderMain.PurchaseOrderSub.FirstOrDefault().ComId = comid;
                    purchaseOrderMain.PurchaseOrderSub.FirstOrDefault().UserId = userid;
                    purchaseOrderMain.PurchaseOrderSub.FirstOrDefault().PcName = pcname;
                    purchaseOrderMain.PurchaseOrderSub.FirstOrDefault().DateAdded = date;

                    db.Add(purchaseOrderMain);
                    result = "1";
                }
                db.SaveChanges();
            }
            ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", purchaseOrderMain.DeptId);
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", purchaseOrderMain.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", purchaseOrderMain.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", purchaseOrderMain.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", purchaseOrderMain.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", purchaseOrderMain.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", purchaseOrderMain.SupplierId);
            ViewData["DistrictId"] = new SelectList(db.Cat_District.Where(x => x.ComId == comid), "DistId", "DistName");
            return Json(new { Success = result, ex = "" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateSupplier(Supplier supplier)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
           .Select(x => new { x.Key, x.Value.Errors });
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            if (ModelState.IsValid)
            {


                var date = DateTime.Now;
                if (supplier.SupplierId > 0)
                {
                    supplier.DateUpdated = DateTime.Now;
                    supplier.UpdateByUserId = userid;
                    db.Entry(supplier).State = EntityState.Modified;

                }
                else
                {
                    supplier.ComId = comid;
                    supplier.UserId = userid;
                    supplier.DateAdded = date;
                    db.Add(supplier);
                }
                db.SaveChanges();
                return Json(new { Success = 1, ex = "Successfully Added", data = supplier });
            }
            return Json(new { Success = 3, ex = "Model State Not valid", data = "" });
        }



        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            ViewBag.Title = "Edit";
            var purchaseOrderMain = await db.PurchaseOrderMain
                .Include(p => p.PurchaseOrderSub)
                .ThenInclude(p => p.vProduct)
                .ThenInclude(p => p.vProductUnit)
                .FirstOrDefaultAsync(m => m.PurOrderMainId == id && m.Status == 0);
            if (purchaseOrderMain == null)
            {
                return NotFound();
            }
            ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", purchaseOrderMain.DeptId);
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", purchaseOrderMain.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", purchaseOrderMain.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", purchaseOrderMain.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", purchaseOrderMain.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo", purchaseOrderMain.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", purchaseOrderMain.SupplierId);
            ViewData["DistrictId"] = new SelectList(db.Cat_District.Where(x => x.ComId == comid), "DistId", "DistName");
            return View("Create", purchaseOrderMain);
        }

        // POST: PurchaseOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrderMain purchaseOrderMain)
        {
            if (id != purchaseOrderMain.PurOrderMainId)
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
                    db.Update(purchaseOrderMain);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderMainExists(purchaseOrderMain.PurOrderMainId))
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
            ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", purchaseOrderMain.DeptId);
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", purchaseOrderMain.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", purchaseOrderMain.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", purchaseOrderMain.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", purchaseOrderMain.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo", purchaseOrderMain.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", purchaseOrderMain.SupplierId);
            return View(purchaseOrderMain);
        }

        // GET: PurchaseOrder/Delete/5s
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrderMain = await db.PurchaseOrderMain
                 .Include(p => p.PurchaseOrderSub)
                 .ThenInclude(p => p.vProduct)
                 .ThenInclude(p => p.vProductUnit)
                 .FirstOrDefaultAsync(m => m.PurOrderMainId == id && m.Status == 0);

            if (purchaseOrderMain == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            ViewBag.Title = "Delete";
            ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", purchaseOrderMain.DeptId);
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", purchaseOrderMain.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", purchaseOrderMain.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", purchaseOrderMain.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", purchaseOrderMain.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo", purchaseOrderMain.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", purchaseOrderMain.SupplierId);


            return View("Create", purchaseOrderMain);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            var purchaseOrderMain = db.PurchaseOrderMain.Include(a => a.PurchaseOrderSub).Where(a => a.PurReqId == id).FirstOrDefault();
            db.PurchaseOrderMain.Remove(purchaseOrderMain);
            var result = db.SaveChanges();
            if (result > 1) return Json(true);
            else return Json(false);
        }

        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptPO";

            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            HttpContext.Session.SetString("reportquery", "Exec [rptPODetails] '" + comid + "', 'PONW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.PurchaseOrderMain.Where(x => x.PurOrderMainId == id).Select(x => x.PONo).Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();


            string DataSourceName = "DataSet1";
            HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            /////////////////////// sub report test to our report server


            //var subReport = new SubReport();
            //var subReportObject = new List<SubReport>();

            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ChequeNo'";
            //subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            //subReportObject.Add(subReport);


            //subReport = new SubReport();
            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ReceiptPerson'";
            //subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            //subReportObject.Add(subReport);


            //var jsonData = JsonConvert.SerializeObject(subReportObject);

            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);
            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
            //return Redirect(callBackUrl);


            string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = type });//, new { id = 1 }
            return Redirect(redirectUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }

        private bool PurchaseOrderMainExists(int id)
        {
            return db.PurchaseOrderMain.Any(e => e.PurOrderMainId == id);
        }
    }
}
