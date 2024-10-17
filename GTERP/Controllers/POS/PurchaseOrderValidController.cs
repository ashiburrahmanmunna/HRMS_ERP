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
    public class PurchaseOrderValidController : Controller
    {
        #region Feild and constructor
        private readonly GTRDBContext db;
        private readonly IConfiguration _configuration;

        public PurchaseOrderValidController(GTRDBContext context, IConfiguration configuration)
        {
            db = context;
            _configuration = configuration;
        }

        #endregion

        // GET: PurchaseOrderValid
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

        public partial class PurchaseOrderValidResult
        {
            public int PurOrderValidMainId { get; set; }

            public string POValidNo { get; set; }

            public string Refference { get; set; }

            public DateTime PODate { get; set; }

            public string Department { get; set; }

            public string SupplierName { get; set; }
            public string TypeName { get; set; }
            public string CurCode { get; set; }
            public float ConvertionRate { get; set; }
            public float TotalPOValue { get; set; }
            public float? Deduction { get; set; }
            public float? NetPOValue { get; set; }


            public string SectName { get; set; }


            public DateTime? GateInHouseDate { get; set; }

            public DateTime? ExpectedReciveDate { get; set; }
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


                    var query = from e in db.PurchaseOrderValidMains.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.PurOrderValidMainId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                select new PurchaseOrderValidResult
                                {
                                    PurOrderValidMainId = e.PurOrderValidMainId,
                                    POValidNo = e.POValidNo,
                                    PODate = e.PODate,
                                    Department = e.Department,
                                    SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                    TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                    CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                    ConvertionRate = e.ConvertionRate,
                                    TotalPOValue = e.TotalPOValue,
                                    Deduction = e.Deduction,
                                    NetPOValue = e.NetPOValue,
                                    SectName = e.Section != null ? e.Section.SectName : "",
                                    GateInHouseDate = e.GateInHouseDate,
                                    ExpectedReciveDate = e.ExpectedReciveDate
                                };


                    var parser = new Parser<PurchaseOrderValidResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = from e in db.PurchaseOrderValidMains
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                        //.Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.PurOrderValidMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderValidResult
                                        {
                                            PurOrderValidMainId = e.PurOrderValidMainId,
                                            POValidNo = e.POValidNo,
                                            PODate = e.PODate,
                                            Department = e.Department,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            GateInHouseDate = e.GateInHouseDate,
                                            ExpectedReciveDate = e.ExpectedReciveDate
                                        };


                        var parser = new Parser<PurchaseOrderValidResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = from e in db.PurchaseOrderValidMains
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        // .Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.PurOrderValidMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderValidResult
                                        {
                                            PurOrderValidMainId = e.PurOrderValidMainId,
                                            POValidNo = e.POValidNo,
                                            PODate = e.PODate,
                                            Department = e.Department,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            GateInHouseDate = e.GateInHouseDate,
                                            ExpectedReciveDate = e.ExpectedReciveDate
                                        };


                        var parser = new Parser<PurchaseOrderValidResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = from e in db.PurchaseOrderValidMains
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                        .OrderByDescending(x => x.PurOrderValidMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderValidResult
                                        {
                                            PurOrderValidMainId = e.PurOrderValidMainId,
                                            POValidNo = e.POValidNo,
                                            PODate = e.PODate,
                                            Department = e.Department,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            GateInHouseDate = e.GateInHouseDate,
                                            ExpectedReciveDate = e.ExpectedReciveDate
                                        };


                        var parser = new Parser<PurchaseOrderValidResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in db.PurchaseOrderValidMains
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.PODate >= dtFrom && p.PODate <= dtTo))

                        .OrderByDescending(x => x.PurOrderValidMainId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseOrderValidResult
                                        {
                                            PurOrderValidMainId = e.PurOrderValidMainId,
                                            POValidNo = e.POValidNo,
                                            PODate = e.PODate,
                                            Department = e.Department,
                                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                            ConvertionRate = e.ConvertionRate,
                                            TotalPOValue = e.TotalPOValue,
                                            Deduction = e.Deduction,
                                            NetPOValue = e.NetPOValue,
                                            SectName = e.Section != null ? e.Section.SectName : "",
                                            GateInHouseDate = e.GateInHouseDate,
                                            ExpectedReciveDate = e.ExpectedReciveDate
                                        };


                        var parser = new Parser<PurchaseOrderValidResult>(Request.Form, querytest);
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
            //List<PurchaseOrderValidDetailsModel> purchaseRequisitionMain = new List<PurchaseOrderValidDetailsModel>();


            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            var quary = $"EXEC PurchaseOrderValidDetailsInformation '{comid}','{userid}',{PurReqId}";

            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@PurReqId", PurReqId);
            List<PurchaseOrderValidDetailsModel> PurchaseOrderValidDetailsInformation = Helper.ExecProcMapTList<PurchaseOrderValidDetailsModel>("PurchaseOrderValidDetailsInformation", parameters);


            return Json(PurchaseOrderValidDetailsInformation);
        }
        public class PurchaseOrderValidDetailsModel
        {
            public int? PurOrderValidMainId { get; set; }
            public int? PurOrderValidSubId { get; set; }
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
        // GET: PurchaseOrderValid/Create
        public IActionResult Create()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName");
            ViewData["CurrencyId"] = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName");
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName");
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo");
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName");

            //ViewData["SupplierList"] = new SelectList(db.Suppliers.Where(x => x.comid == comid && x.IsInActive == false), "SupplierId", "SupplierName");
            this.ViewBag.SupplierList = db.Suppliers.Where(x => x.ComId == comid && x.IsInActive == false);

            return View();
        }

        // POST: PurchaseOrderValid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(PurchaseOrderValidMain PurchaseOrderValidMains)
        {
            var result = "";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            if (ModelState.IsValid)
            {


                var date = DateTime.Now;
                if (PurchaseOrderValidMains.PurOrderValidMainId > 0)
                {
                    foreach (PurchaseOrderValidSub item in PurchaseOrderValidMains.PurchaseOrderValidSub)
                    {
                        if (item.PurOrderValidSubId > 0)
                        {
                            item.DateUpdated = date;
                            item.UpdateByUserId = userid;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            item.ComId = comid;
                            item.UserId = userid;
                            item.PcName = pcname;
                            item.DateAdded = date;
                            db.PurchaseOrderValidSubs.Add(item);
                        }
                    }
                    PurchaseOrderValidMains.UpdateByUserId = userid;
                    PurchaseOrderValidMains.DateUpdated = date;
                    db.Entry(PurchaseOrderValidMains).State = EntityState.Modified;
                    result = "2";
                }
                else
                {
                    PurchaseOrderValidMains.ComId = comid;
                    PurchaseOrderValidMains.UserId = userid;
                    PurchaseOrderValidMains.PcName = pcname;
                    PurchaseOrderValidMains.DateAdded = date;
                    //PurchaseOrderValidMains.PurchaseOrderValidSub.FirstOrDefault().ComId = comid;
                    //PurchaseOrderValidMains.PurchaseOrderValidSub.FirstOrDefault().UserId = userid;
                    //PurchaseOrderValidMains.PurchaseOrderValidSub.FirstOrDefault().PcName = pcname;
                    //PurchaseOrderValidMains.PurchaseOrderValidSub.FirstOrDefault().DateAdded = date;

                    db.Add(PurchaseOrderValidMains);
                    result = "1";
                }
                db.SaveChanges();
            }
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", PurchaseOrderValidMains.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency, "CurrencyId", "CurCode", PurchaseOrderValidMains.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", PurchaseOrderValidMains.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", PurchaseOrderValidMains.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", PurchaseOrderValidMains.PurReqId);
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", PurchaseOrderValidMains.SupplierId);
            this.ViewBag.SupplierList = db.Suppliers.Where(x => x.ComId == comid && x.IsInActive == false);

            return Json(new { Success = result, ex = "" });
        }

        // GET: PurchaseOrderValid/Edit/5
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
            var PurchaseOrderValidMains = await db.PurchaseOrderValidMains
                .Include(p => p.PurchaseOrderValidSub)
                .ThenInclude(p => p.vProduct)
                .ThenInclude(p => p.vProductUnit)
                .Include(p => p.PurchaseOrderValidSub)
                .ThenInclude(p => p.PurchaseOrderValidSubSupplier)

                .FirstOrDefaultAsync(m => m.PurOrderValidMainId == id && m.Status == 0);
            if (PurchaseOrderValidMains == null)
            {
                return NotFound();
            }
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", PurchaseOrderValidMains.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency, "CurrencyId", "CurCode", PurchaseOrderValidMains.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", PurchaseOrderValidMains.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", PurchaseOrderValidMains.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", PurchaseOrderValidMains.PurReqId);
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", PurchaseOrderValidMains.SupplierId);
            this.ViewBag.SupplierList = db.Suppliers.Where(x => x.ComId == comid && x.IsInActive == false);
            return View("Create", PurchaseOrderValidMains);
        }

        // POST: PurchaseOrderValid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrderValidMain PurchaseOrderValidMains)
        {
            if (id != PurchaseOrderValidMains.PurOrderValidMainId)
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
                    db.Update(PurchaseOrderValidMains);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderValidMainsExists(PurchaseOrderValidMains.PurOrderValidMainId))
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
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", PurchaseOrderValidMains.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency, "CurrencyId", "CurCode", PurchaseOrderValidMains.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", PurchaseOrderValidMains.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", PurchaseOrderValidMains.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", PurchaseOrderValidMains.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", PurchaseOrderValidMains.SupplierId);
            return View(PurchaseOrderValidMains);
        }

        // GET: PurchaseOrderValid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            ViewBag.Title = "Delete";
            var PurchaseOrderValidMains = await db.PurchaseOrderValidMains
                .Include(p => p.PurchaseOrderValidSub)
                .ThenInclude(p => p.vProduct)
                .ThenInclude(p => p.vProductUnit)
                .Include(p => p.PurchaseOrderValidSub)
                .ThenInclude(p => p.PurchaseOrderValidSubSupplier)

                .FirstOrDefaultAsync(m => m.PurOrderValidMainId == id && m.Status == 0);
            if (PurchaseOrderValidMains == null)
            {
                return NotFound();
            }
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", PurchaseOrderValidMains.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency, "CurrencyId", "CurCode", PurchaseOrderValidMains.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", PurchaseOrderValidMains.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", PurchaseOrderValidMains.PrdUnitId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", PurchaseOrderValidMains.PurReqId);
            //ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.comid == comid), "SupplierId", "SupplierName", PurchaseOrderValidMains.SupplierId);
            this.ViewBag.SupplierList = db.Suppliers.Where(x => x.ComId == comid && x.IsInActive == false);
            return View("Create", PurchaseOrderValidMains);
        }

        // POST: PurchaseOrderValid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var PurchaseOrderValidMains = await db.PurchaseOrderValidMains.FindAsync(id);
            db.PurchaseOrderValidMains.Remove(PurchaseOrderValidMains);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOrderValidMainsExists(int id)
        {
            return db.PurchaseOrderValidMains.Any(e => e.PurOrderValidMainId == id);
        }
    }
}
