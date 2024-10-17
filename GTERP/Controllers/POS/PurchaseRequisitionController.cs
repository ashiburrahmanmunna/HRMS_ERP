using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class PurchaseRequisitionController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        private readonly IConfiguration _configuration;

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }

        public PurchaseRequisitionController(GTRDBContext gtrdb, TransactionLogRepository tran, IConfiguration configuration)
        {
            db = gtrdb;
            tranlog = tran;
            _configuration = configuration;
        }

        // GET: PurchaseRequisition
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

        public partial class PurchaseReQuisitionResult
        {
            public int PurReqId { get; set; }
            public string PRNo { get; set; }
            [Display(Name = "Product Unit")]
            public string PrdUnitName { get; set; }

            [Display(Name = "Requisition Ref")]
            public string ReqRef { get; set; }

            [Display(Name = "Requisition Date")]
            public string ReqDate { get; set; }

            [Display(Name = "Board Meeting Date")]
            public string BoardMeetingDate { get; set; }

            public string PurposeName { get; set; }


            public string DeptName { get; set; }


            public string ApprovedBy { get; set; }


            public string RecommenedBy { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            public string Remarks { get; set; }

            [Display(Name = "Required Date")]
            public string RequiredDate { get; set; }




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


                    var query = from e in db.PurchaseRequisitionMain.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.PurReqId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                select new PurchaseReQuisitionResult
                                {
                                    PurReqId = e.PurReqId,
                                    PRNo = e.PRNo,
                                    RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                    ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                    ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                    RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                    BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                    PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                    DeptName = e.Department != null ? e.Department.DeptName : "",
                                    Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                    Remarks = e.Remarks,
                                    ReqRef = e.ReqRef,
                                };


                    var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = from e in db.PurchaseRequisitionMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                        //.Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.PurReqId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseReQuisitionResult
                                        {
                                            PurReqId = e.PurReqId,
                                            PRNo = e.PRNo,
                                            RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                            ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                            ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                            RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                            BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                            PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                            DeptName = e.Department != null ? e.Department.DeptName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            Remarks = e.Remarks,
                                            ReqRef = e.ReqRef,
                                        };


                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = from e in db.PurchaseRequisitionMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        // .Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.PurReqId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseReQuisitionResult
                                        {
                                            PurReqId = e.PurReqId,
                                            PRNo = e.PRNo,
                                            RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                            ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                            ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                            RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                            BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                            PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                            DeptName = e.Department != null ? e.Department.DeptName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            Remarks = e.Remarks,
                                            ReqRef = e.ReqRef,
                                        };


                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = from e in db.PurchaseRequisitionMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                        .OrderByDescending(x => x.PurReqId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseReQuisitionResult
                                        {
                                            PurReqId = e.PurReqId,
                                            PRNo = e.PRNo,
                                            RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                            ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                            ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                            RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                            BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                            PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                            DeptName = e.Department != null ? e.Department.DeptName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            Remarks = e.Remarks,
                                            ReqRef = e.ReqRef,
                                        };


                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in db.PurchaseRequisitionMain
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))

                        .OrderByDescending(x => x.PurReqId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseReQuisitionResult
                                        {
                                            PurReqId = e.PurReqId,
                                            PRNo = e.PRNo,
                                            RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                            ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                            ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                            RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                            BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                            PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                            DeptName = e.Department != null ? e.Department.DeptName : "",
                                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                            Remarks = e.Remarks,
                                            ReqRef = e.ReqRef,
                                        };


                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
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



        // GET: PurchaseRequisition/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseRequisitionMain = await db.PurchaseRequisitionMain
                .Include(p => p.ApprovedBy)
                .Include(p => p.Department)
                .Include(p => p.PrdUnit)
                .Include(p => p.Purpose)
                .Include(p => p.RecommenedBy)
                .FirstOrDefaultAsync(m => m.PurReqId == id);
            if (purchaseRequisitionMain == null)
            {
                return NotFound();
            }

            return View(purchaseRequisitionMain);
        }

        public ActionResult GetProductInfo(int id)
        {
            var comid = HttpContext.Session.GetString("comid");

            decimal? lastpurchaseprice;
            lastpurchaseprice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id && x.GoodsReceiveMain.Status > 0).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate).FirstOrDefault();
            if (lastpurchaseprice == null)
            {
                lastpurchaseprice = 0;
            }

            var ProductData = db.Products.Include(x => x.vProductUnit).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                LastPurchasePrice = lastpurchaseprice
            }).Where(p => p.ProductId == id).FirstOrDefault();// ToList();

            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            return Json(ProductData);
        }

        // GET: PurchaseRequisition/Create
        public IActionResult Create()
        {

            PurchaseRequisitionMain purchasereq = new PurchaseRequisitionMain();
            purchasereq.ReqDate = DateTime.Now.Date;
            purchasereq.BoardMeetingDate = DateTime.Now.Date;
            purchasereq.RequiredDate = DateTime.Now.Date;



            InitViewBag("Create");
            return View(purchasereq);
        }

        [HttpPost]
        public IActionResult CreateProduct(Models.Product product)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    if (product.ProductId > 0)
                    {
                        product.DateUpdated = DateTime.Now;
                        product.comid = (HttpContext.Session.GetString("comid"));

                        if (product.userid == null)
                        {
                            product.userid = HttpContext.Session.GetString("userid");
                        }
                        product.useridUpdate = HttpContext.Session.GetString("userid");

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();

                        TempData["Status"] = "2";
                        TempData["Message"] = "Data Update Successfully";

                        return Json(new { Success = 2, data = product, ex = TempData["Message"].ToString() });
                    }
                    else
                    {
                        product.userid = HttpContext.Session.GetString("userid");
                        product.comid = (HttpContext.Session.GetString("comid"));
                        product.DateAdded = DateTime.Now;
                        product.ProductImage = null;

                        db.Products.Add(product);
                        db.SaveChanges();
                        TempData["Status"] = "1";
                        TempData["Message"] = "Data Save Successfully";

                        return Json(new { Success = 1, data = product, ex = TempData["Message"].ToString() }); ;
                    }
                }
                else
                {
                    return Json(new { Success = 3, ex = "Model State Not Valid" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = 3, ex = e.Message });

            }


        }

        public JsonResult ProductInfo(int id)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));



                var product = db.Products.Where(y => y.ProductId == id && y.comid == comid).SingleOrDefault();
                var unit = db.Unit.Where(y => y.UnitId == product.UnitId).SingleOrDefault();


                return Json(unit);
                //return Json("tesst" );

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
            //return Json(new SelectList(product, "Value", "Text" ));
        }

        // POST: PurchaseRequisition/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            var ex = "";
            try
            {
                //if (ModelState.IsValid)
                //{

                #region Mandatory Parameter

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var AddDate = DateTime.Now;
                var UpdateDate = DateTime.Now;
                var PcName = HttpContext.Session.GetString("pcname");
                #endregion

                var duplicateDocument = db.PurchaseRequisitionMain.Where(i => i.PRNo == purchaseRequisitionMain.PRNo && i.PurReqId != purchaseRequisitionMain.PurReqId && i.ComId == comid).FirstOrDefault();
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = purchaseRequisitionMain.PRNo + " already exist. Document No can not be Duplicate." });
                }


                DateTime date = purchaseRequisitionMain.ReqDate;
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


                //if (ModelState.IsValid)
                //{
                #region Edit request 
                if (purchaseRequisitionMain.PurReqId > 0)
                {

                    purchaseRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                    purchaseRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;


                    purchaseRequisitionMain.ComId = comid;
                    purchaseRequisitionMain.UpdateByUserId = userid;
                    purchaseRequisitionMain.DateUpdated = UpdateDate;
                    IQueryable<PurchaseRequisitionSub> PurchaseRequisitionSub = db.PurchaseRequisitionSub.Where(p => p.PurReqId == purchaseRequisitionMain.PurReqId);

                    //foreach (PurchaseRequisitionSub prsdelete in PurchaseRequisitionSub)
                    //{
                    //    db.PurchaseRequisitionSub.Remove(prsdelete);
                    //}
                    var sl = 0;
                    foreach (PurchaseRequisitionSub item in purchaseRequisitionMain.PurchaseRequisitionSub)
                    {
                        sl++;
                        if (item.PurReqSubId > 0)
                        {
                            if (item.IsDelete != true)
                            {
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Entry(item).State = EntityState.Deleted;
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            try
                            {
                                var sub = new PurchaseRequisitionSub();
                                sub.ComId = comid;
                                sub.DateAdded = item.DateAdded;
                                sub.DateUpdated = item.DateUpdated;
                                sub.Note = item.Note;
                                sub.PcName = PcName;
                                sub.ProductId = item.ProductId;
                                sub.PurReqId = item.PurReqId;
                                sub.PurReqQty = item.PurReqQty;
                                sub.PurReqSubId = item.PurReqSubId;
                                sub.RemainingReqQty = item.RemainingReqQty;
                                sub.SLNo = sl;
                                sub.UpdateByUserId = item.UpdateByUserId;

                                sub.UserId = userid;

                                db.PurchaseRequisitionSub.Add(sub);

                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                            }

                        }

                    }

                    db.Entry(purchaseRequisitionMain).State = EntityState.Modified;
                    db.SaveChanges();
                }
                #endregion

                #region Create Request

                else
                {
                    using (var tr = db.Database.BeginTransaction())
                    {

                        purchaseRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                        purchaseRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;

                        purchaseRequisitionMain.ComId = comid;
                        purchaseRequisitionMain.UserId = userid;
                        purchaseRequisitionMain.DateAdded = AddDate;
                        var main = new PurchaseRequisitionMain();
                        main = purchaseRequisitionMain;
                        var sl = 0;
                        foreach (var item in purchaseRequisitionMain.PurchaseRequisitionSub)
                        {
                            sl++;
                            var sub = new PurchaseRequisitionSub();
                            sub.ComId = comid;
                            sub.DateAdded = AddDate;
                            sub.DateUpdated = item.DateUpdated;
                            sub.Note = item.Note;
                            sub.PcName = PcName;
                            sub.ProductId = item.ProductId;
                            sub.PurReqId = purchaseRequisitionMain.PurReqId;
                            sub.PurReqQty = item.PurReqQty;
                            sub.LastPurchasePrice = item.LastPurchasePrice;
                            sub.RemainingReqQty = item.RemainingReqQty;
                            sub.SLNo = sl;
                            sub.UpdateByUserId = item.UpdateByUserId;
                            sub.UserId = userid;
                        }

                        db.PurchaseRequisitionMain.Add(main);
                        db.SaveChanges();

                        tr.Commit();
                    }

                }
                #endregion


                return Json(new { Success = 1, PurReqId = purchaseRequisitionMain.PurReqId, ex = "" });
                //}
                //}



            }
            catch (Exception e)
            {
                ex = e.Message;
                //return Json(new { Success = 0, error = 1, ex = e.Message });
            }
            #region ViewBag Initialization

            //ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Select(x => new { x.EmpId, x.EmpCode, x.EmpName }), "EmpId", "EmpName", purchaseRequisitionMain.ApprovedByEmpId);
            //ViewData["CategoryId"] = new SelectList(db.Categories.Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
            //ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", purchaseRequisitionMain.DepartmentId);
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", purchaseRequisitionMain.PrdUnitId);
            //ViewData["PurposeId"] = new SelectList(db.Purpose.Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName", purchaseRequisitionMain.PurposeId);

            //ViewData["ProductId"] = new SelectList(db.Products.Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");

            //ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName"); 
            #endregion

            return Json(new { Success = 0, error = 1, ex = ex });

        }
        //[ValidateAntiForgeryToken]

        public JsonResult GetProducts(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            IEnumerable<object> product;
            if (id != null)
            {
                if (id == 0 || id == -1)
                {
                    //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                    product = new SelectList(db.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                }
                else
                {
                    product = new SelectList(db.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
                    //product = db.Products.Where(x => x.CategoryId == id).Select(x => new { x.ProductId, x.ProductName }).ToList();
                }
            }
            else
            {
                //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                product = new SelectList(db.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

            }
            return Json(new { item = product });
        }

        [HttpPost]
        public ActionResult DeletePrSub(int prsubid)
        {
            try
            {
                var sub = db.PurchaseRequisitionSub.Find(prsubid);
                db.PurchaseRequisitionSub.Remove(sub);
                db.SaveChanges();
                return Json(new { error = 0, success = 1, message = "This record deleted successfully" });
            }
            catch (Exception ex)
            {
                var m = $" Message:{ex.Message}\nInner Exception:{ex.InnerException.Message}";
                return Json(new { error = 1, success = 0, message = m });
            }

        }

        // GET: PurchaseRequisition/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";

            PurchaseRequisitionMain purchaseRequisitionMain = await
                db.PurchaseRequisitionMain.Include(p => p.PurchaseRequisitionSub).
                ThenInclude(p => p.vProduct)
                .ThenInclude(p => p.vProductUnit)
                .Where(p => p.PurReqId == id && p.Status == 0)
                .FirstOrDefaultAsync();
            // PurchaseRequisitionMain purchaseRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
            if (purchaseRequisitionMain == null)
            {
                return NotFound();
            }
            InitViewBag("Edit", id, purchaseRequisitionMain);
            //return Json(new {data= purchaseRequisitionMain });
            return View("Create", purchaseRequisitionMain);
        }

        // POST: PurchaseRequisition/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseRequisitionMain purchaseRequisitionMain)
        {
            var comid = HttpContext.Session.GetString("comid");

            if (id != purchaseRequisitionMain.PurReqId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(purchaseRequisitionMain);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseRequisitionMainExists(purchaseRequisitionMain.PurReqId))
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
            ViewData["ApprovedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmployeeId", "EmployeeName", purchaseRequisitionMain.ApprovedByEmpId);
            ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", purchaseRequisitionMain.DepartmentId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitShortName", purchaseRequisitionMain.PrdUnitId);
            ViewData["PurposeId"] = new SelectList(db.Purpose, "PurposeId", "PurposeName", purchaseRequisitionMain.PurposeId);
            ViewData["RecommenedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmployeeId", "EmployeeName", purchaseRequisitionMain.RecommenedByEmpId);
            ViewData["ProductId"] = new SelectList(db.Products.Where(x => x.comid == comid), "ProductId", "ProductName");
            ViewData["UnitId"] = new SelectList(db.Unit, "UnitId", "UnitName");
            ViewData["SectId"] = new SelectList(db.Cat_Section.Select(x => new { x.SectId, x.SectName }), "SectId", "SectName", purchaseRequisitionMain.SectId);
            return View(purchaseRequisitionMain);
        }

        // GET: PurchaseRequisition/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            PurchaseRequisitionMain purchaseRequisitionMain = await
                db.PurchaseRequisitionMain.Include(p => p.PurchaseRequisitionSub).
                ThenInclude(p => p.vProduct)
                .ThenInclude(p => p.vProductUnit)
                .Where(p => p.PurReqId == id && p.Status == 0)
                .FirstOrDefaultAsync();
            // PurchaseRequisitionMain purchaseRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
            if (purchaseRequisitionMain == null)
            {
                return NotFound();
            }
            InitViewBag("Delete", id, purchaseRequisitionMain);
            //return Json(new {data= purchaseRequisitionMain });
            return View("Create", purchaseRequisitionMain);

        }

        // POST: PurchaseRequisition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var storeRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
                db.PurchaseRequisitionMain.Remove(storeRequisitionMain);
                await db.SaveChangesAsync();


                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.PurReqId.ToString(), "Delete", storeRequisitionMain.PurReqId.ToString());

                return Json(new { Success = 1, VoucherID = storeRequisitionMain.PurReqId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });
            }
        }
        private void InitViewBag(string title, int? id = null, PurchaseRequisitionMain purchaseRequisitionMain = null)
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = title;
            if (title == "Create")
            {
                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
                ViewData["CategoryId"] = new SelectList(db.Categories.Where(c => c.ComId == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
                ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName");
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(c => c.ComId == comid).Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
                ViewData["PurposeId"] = new SelectList(db.Purpose.Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");
                //ViewData["RecommenedByEmpId"] = new SelectList(db.Employee.Select(x => new { x.}), "EmployeeId", "EmployeeName");
                //ViewData["ProductId"] = new SelectList(db.Products.Select(x => new { x.ProductId,x.ProductName}), "ProductId", "ProductName");
                //ViewData["ProductBrand"] = new SelectList(db.Products.Where(x => x.ProductBrand != null).Select(x => new { x.}), "ProductBrand", "ProductBrand");
                ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");
                ViewData["SectId"] = new SelectList(db.Cat_Section.Select(x => new { x.SectId, x.SectName }), "SectId", "SectName");

            }
            else if (title == "Edit" || title == "Delete")
            {
                ViewData["CategoryId"] = new SelectList(db.Categories.Where(c => c.ComId == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
                ViewBag.reqsub = db.PurchaseRequisitionSub.Where(r => r.PurReqId == id).ToList();
                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text", purchaseRequisitionMain.ApprovedByEmpId);
                ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(c => c.ComId == comid), "DeptId", "DeptName", purchaseRequisitionMain.DepartmentId);
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitShortName", purchaseRequisitionMain.PrdUnitId);
                ViewData["PurposeId"] = new SelectList(db.Purpose, "PurposeId", "PurposeName", purchaseRequisitionMain.PurposeId);
                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text", purchaseRequisitionMain.RecommenedByEmpId);
                ViewData["ProductId"] = new SelectList(db.Products.Where(c => c.comid == comid), "ProductId", "ProductName");
                ViewData["UnitId"] = new SelectList(db.Unit, "UnitId", "UnitName");
                ViewData["SectId"] = new SelectList(db.Cat_Section.Select(x => new { x.SectId, x.SectName }), "SectId", "SectName");
            }

        }
        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptPR";

            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            HttpContext.Session.SetString("reportquery", "Exec [rptPRDetails] '" + comid + "', 'PRNW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id).Select(x => x.PRNo).Single();
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
            //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);

            string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });//, new { id = 1 }
            return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }
        private bool PurchaseRequisitionMainExists(int id)
        {
            return db.PurchaseRequisitionMain.Any(e => e.PurReqId == id);
        }
    }
}
