using DataTablesParser;
using GTERP.BLL;
using GTERP.Interfaces;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRVariables;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class InventoryController : Controller
    {

        #region Declarations 


        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        POSRepository POS;
        PermissionLevel PL;
        private object tranlog;
        private object licities;
        private readonly TransactionLogRepository _tranlog;
        private readonly ILogger<InventoryController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly GTRDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUrlHelper _urlHelper;
        private readonly IStoreRequisitionRepository _IStoreRequisitionRepository;
        private readonly IPurchaseRequisitionRepository _purchaseRequisitionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IEmpInfoRepository _empInfoRepository;
        private readonly IGoodsReceiveRepository _goodsReceiveRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEmpReleaseRepository _empReleaseRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPostDocumentRepository _postDocumentRepository;

        #endregion

        #region Constructor

        public InventoryController(
            TransactionLogRepository tran,
            GTRDBContext context,
            ILogger<InventoryController> logger,
            IActionContextAccessor actionContextAccessor,
            IWebHostEnvironment env,
            POSRepository _POS,
            PermissionLevel _pl,
            IConfiguration configuration,
            IUrlHelperFactory urlHelperFactory,
            IStoreRequisitionRepository IStoreRequisitionRepository,
            IPurchaseRequisitionRepository purchaseRequisitionRepository,
            IDepartmentRepository departmentRepository,
            IUnitRepository unitRepository,
            ISectionRepository sectionRepository,
            IEmpInfoRepository empInfoRepository,
            IGoodsReceiveRepository goodsReceiveRepository,
            ISupplierRepository supplierRepository,
            IEmpReleaseRepository empReleaseRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            IPostDocumentRepository postDocumentRepository

            )
        {
            _tranlog = tran;
            _context = context;
            _logger = logger;
            _env = env;
            POS = _POS;
            PL = _pl;
            _configuration = configuration;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _IStoreRequisitionRepository = IStoreRequisitionRepository;
            _purchaseRequisitionRepository = purchaseRequisitionRepository;
            _departmentRepository = departmentRepository;
            _unitRepository = unitRepository;
            _sectionRepository = sectionRepository;
            _empInfoRepository = empInfoRepository;
            _goodsReceiveRepository = goodsReceiveRepository;
            _supplierRepository = supplierRepository;
            _empReleaseRepository = empReleaseRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _postDocumentRepository = postDocumentRepository;
        }

        #endregion

        #region StoreRequisition
        // GET: Inventory/StoreRequisitionList
        public async Task<IActionResult> StoreRequisitionList()
        {


            ViewBag.Userlist = _IStoreRequisitionRepository.GetUserList();
            return View();

        }

        //GET: Inventory/StoreRequisitionDetails/5
        public async Task<IActionResult> StoreRequisitionDetails(int? id)
        {
            var storeRequisitionMain = _IStoreRequisitionRepository.GetStoreRequisitionDetails(id);
            if (storeRequisitionMain == null)
            {
                return NotFound();
            }

            return View(storeRequisitionMain);
        }

        //GET: Inventory/CreateStoreRequisition
        public IActionResult CreateStoreRequisition()
        {
            var laststorereq = _IStoreRequisitionRepository.LastStorereq();
            var userwiseSRR = _IStoreRequisitionRepository.UserWiseSRR();

            if (userwiseSRR != null)
            {
                userwiseSRR.StoreReqId = 0;
                userwiseSRR.Status = 0;
                InitViewBag("Create");
                return View(userwiseSRR);
            }
            else
            {
                StoreRequisitionMain storereq = new StoreRequisitionMain();
                storereq.ReqDate = DateTime.Now.Date;
                storereq.INDate = DateTime.Now.Date;
                storereq.Remarks = "";
                storereq.BoardMeetingDate = DateTime.Now.Date;

                InitViewBag("Create");
                return View(storereq);

            }

        }
        //GET: Inventory/StoreRequisitionSubStoreCreate
        public IActionResult StoreRequisitionSubStoreCreate()
        {
            StoreRequisitionMain storereq = new StoreRequisitionMain();
            storereq.ReqDate = DateTime.Now.Date;
            storereq.BoardMeetingDate = DateTime.Now.Date;
            storereq.IsSubStore = true;

            InitViewBag("Create");
            return View(storereq);
        }



        public IActionResult GetStoreRequisition(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
        {
            try
            {


                Microsoft.Extensions.Primitives.StringValues y = "";

                var x = Request.Form.TryGetValue("search[value]", out y);

                UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");


                if (permission.IsMedical)
                {
                    if (y.ToString().Length > 0)
                    {



                        var query = _IStoreRequisitionRepository.storeReQuisitionResultsForMedical();


                        var parser = new Parser<StoreReQuisitionResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (CustomerList != null && UserList != null)
                        {
                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForMedicalByUserAndCustomer(UserList, FromDate, ToDate, CustomerList);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList != null && UserList == null)
                        {
                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForMedicalByCustomer(UserList, FromDate, ToDate, CustomerList);

                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList == null && UserList != null)
                        {

                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForMedicalByUser(UserList, FromDate, ToDate, CustomerList);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }
                        else
                        {

                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForMedicalByDate(FromDate, ToDate);

                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }

                    }
                }
                else if (permission.IsProduction)
                {
                    if (y.ToString().Length > 0)
                    {


                        var query = _IStoreRequisitionRepository.storeReQuisitionResultsForProduction();


                        var parser = new Parser<StoreReQuisitionResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (CustomerList != null && UserList != null)
                        {
                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForProductionByUserAndCustomer(UserList, FromDate, ToDate, CustomerList);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList != null && UserList == null)
                        {
                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForProductionByCustomer(UserList, FromDate, ToDate, CustomerList);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList == null && UserList != null)
                        {

                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForProductionByUser(UserList, FromDate, ToDate, CustomerList);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }
                        else
                        {

                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsForProductionByDate(FromDate, ToDate);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }

                    }
                }

                else
                {
                    if (y.ToString().Length > 0)
                    {


                        var query = _IStoreRequisitionRepository.storeReQuisitionResults();


                        var parser = new Parser<StoreReQuisitionResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (CustomerList != null && UserList != null)
                        {
                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsByUserAndCustomer(UserList, FromDate, ToDate, CustomerList);


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList != null && UserList == null)
                        {
                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsByCustomer(UserList, FromDate, ToDate, CustomerList);

                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList == null && UserList != null)
                        {

                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsByUser(UserList, FromDate, ToDate, CustomerList);

                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }
                        else
                        {

                            var querytest = _IStoreRequisitionRepository.storeReQuisitionResultsByDate(FromDate, ToDate);
                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }

                    }
                }



            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });
                throw ex;
            }

        }


        //POST: Inventory/CreateStoreRequisition

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStoreRequisition(StoreRequisitionMain storeRequisitionMain)
        {
            var ex = "";
            try
            {
                #region Mandatory Parameter

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var AddDate = DateTime.Now;
                var UpdateDate = DateTime.Now;
                var PcName = HttpContext.Session.GetString("pcname");
                #endregion

                var duplicateDocument = _context.StoreRequisitionMain.Where(i => i.SRNo == storeRequisitionMain.SRNo && i.StoreReqId != storeRequisitionMain.StoreReqId && i.ComId == comid).FirstOrDefault();
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = storeRequisitionMain.SRNo + " already exist. Document No can not be Duplicate." });
                }


                DateTime date = storeRequisitionMain.ReqDate;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
                if (activefiscalmonth == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                }
                var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
                if (activefiscalyear == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                }


                if (ModelState.IsValid)
                {
                    #region Edit request 
                    if (storeRequisitionMain.StoreReqId > 0)
                    {

                        storeRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                        storeRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;

                        storeRequisitionMain.ComId = comid;
                        storeRequisitionMain.UpdateByUserId = userid;
                        storeRequisitionMain.DateUpdated = UpdateDate;
                        IQueryable<StoreRequisitionSub> StoreRequisitionSubs = _context.StoreRequisitionSub.Where(p => p.StoreReqId == storeRequisitionMain.StoreReqId);


                        var sl = 0;
                        foreach (StoreRequisitionSub item in storeRequisitionMain.StoreRequisitionSub)
                        {
                            item.ComId = comid;
                            item.UserId = userid;

                            sl++;
                            if (item.StoreReqSubId > 0)
                            {
                                item.DateUpdated = DateTime.Now;
                                item.UpdateByUserId = userid;
                                if (item.IsDelete != true)
                                {

                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }

                            }
                            else
                            {
                                try
                                {

                                    if (item.IsDelete != true)
                                    {
                                        var sub = new StoreRequisitionSub();
                                        sub.ComId = comid;
                                        sub.DateAdded = DateTime.Now;
                                        sub.DateUpdated = item.DateUpdated;
                                        sub.Note = item.Note;
                                        sub.PcName = PcName;
                                        sub.ProductId = item.ProductId;
                                        sub.StoreReqId = item.StoreReqId;
                                        sub.StoreReqQty = item.StoreReqQty;
                                        sub.StoreReqSubId = item.StoreReqSubId;
                                        sub.RemainingReqQty = item.RemainingReqQty;
                                        sub.SLNo = sl;
                                        sub.UpdateByUserId = item.UpdateByUserId;

                                        sub.UserId = userid;

                                        _context.StoreRequisitionSub.Add(sub);
                                    }
                                }
                                catch (Exception e)
                                {

                                    Console.WriteLine(e.Message);
                                }

                            }

                        }

                        _context.Entry(storeRequisitionMain).State = EntityState.Modified;
                        _context.SaveChanges();


                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "1";
                        //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.StoreReqId.ToString(), "Update", storeRequisitionMain.StoreReqId.ToString() + " ReqNo" + storeRequisitionMain.SRNo);
                        return Json(new { Success = 2, StoreReqId = storeRequisitionMain.StoreReqId, ex = "" });
                    }
                    #endregion

                    #region Create Request

                    else
                    {
                        using (var tr = _context.Database.BeginTransaction())
                        {





                            storeRequisitionMain.ComId = comid;
                            storeRequisitionMain.UserId = userid;
                            storeRequisitionMain.DateAdded = AddDate;
                            storeRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;
                            storeRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;

                            var main = new StoreRequisitionMain();
                            main = storeRequisitionMain;
                            var sl = 0;
                            foreach (var item in storeRequisitionMain.StoreRequisitionSub)
                            {
                                sl++;
                                var sub = new StoreRequisitionSub();
                                sub.ComId = comid;
                                sub.DateAdded = AddDate;
                                sub.DateUpdated = item.DateUpdated;
                                sub.Note = item.Note;
                                sub.PcName = PcName;
                                sub.ProductId = item.ProductId;
                                sub.StoreReqId = storeRequisitionMain.StoreReqId;
                                sub.StoreReqQty = item.StoreReqQty;
                                sub.RemainingReqQty = item.RemainingReqQty;
                                sub.SLNo = sl;
                                sub.UpdateByUserId = item.UpdateByUserId;
                                sub.UserId = userid;
                            }

                            _context.StoreRequisitionMain.Add(main);
                            _context.SaveChanges();

                            TempData["Message"] = "Data Save Successfully";
                            TempData["Status"] = "1";

                            tr.Commit();
                        }
                        return Json(new { Success = 1, StoreReqId = storeRequisitionMain.StoreReqId, ex = "" });
                    }
                    #endregion


                    return Json(new { Success = 1, StoreReqId = storeRequisitionMain.StoreReqId, ex = "" });




                }
            }
            catch (Exception e)
            {
                ex = e.Message;

            }

            return Json(new { Success = 0, error = 1, ex = ex });

        }


        public ActionResult PrintStoreRequisition(int? id, string type)
        {

            string redirectUrl = _IStoreRequisitionRepository.PrintReport(id, type);
            return Redirect(redirectUrl);
        }


        //GET: StoreRequisition/Edit/5
        public IActionResult StoreRequisitionEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";



            var storeRequisitionMain = _IStoreRequisitionRepository.StoreRequisitionMain(id);


            if (storeRequisitionMain == null)
            {
                return NotFound();
            }
            // InitViewBag("Edit", id, storeRequisitionMain);

            if (storeRequisitionMain.IsSubStore)
            {
                return View("Create", storeRequisitionMain);
            }
            else
            {
                return View("Create", storeRequisitionMain);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreRequisitionEdit(int id, StoreRequisitionMain storeRequisitionMain)
        {
            if (id != storeRequisitionMain.StoreReqId)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeRequisitionMain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreRequisitionMainExists(storeRequisitionMain.StoreReqId))
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
            ViewData["ApprovedByEmpId"] = _IStoreRequisitionRepository.ApprovedByEmpId(storeRequisitionMain);
            ViewData["DepartmentId"] = _IStoreRequisitionRepository.DepartmentId(storeRequisitionMain);
            ViewData["PrdUnitId"] = _IStoreRequisitionRepository.PrdUnitId(storeRequisitionMain);
            ViewData["PurposeId"] = _IStoreRequisitionRepository.PurposeId(storeRequisitionMain);
            ViewData["RecommenedByEmpId"] = _IStoreRequisitionRepository.RecommenedByEmpId(storeRequisitionMain);

            return View(storeRequisitionMain);
        }

        //GET: StoreRequisition/Delete/5
        public IActionResult StoreRequisitionDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Delete";


            var storeRequisitionMain = _IStoreRequisitionRepository.StoreRequisitionMain(id);

            //InitViewBag("Delete", id, storeRequisitionMain);

            if (storeRequisitionMain == null)
            {
                return NotFound();
            }
            if (storeRequisitionMain.IsSubStore)
            {
                return View("SubStoreCreate", storeRequisitionMain);
            }
            else
            {
                return View("Create", storeRequisitionMain);
            }
        }
        [HttpPost, ActionName("StoreRequisitionDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var storeRequisitionMain = _IStoreRequisitionRepository.FindById(id);
                _context.StoreRequisitionMain.Remove(storeRequisitionMain);
                await _context.SaveChangesAsync();


                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.StoreReqId.ToString(), "Delete", storeRequisitionMain.StoreReqId.ToString());

                return Json(new { Success = 1, VoucherID = storeRequisitionMain.StoreReqId, ex = "" });

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
            //return RedirectToAction(nameof(Index));
        }
        private bool StoreRequisitionMainExists(int id)
        {
            return _context.StoreRequisitionMain.Any(e => e.StoreReqId == id);
        }


        #endregion

        #region Purchase Requisition
        public async Task<IActionResult> PurchaseRequisitionList()
        {


            ViewBag.Userlist =  _IStoreRequisitionRepository.GetUserList();
            //return View(await gTRDBContext.ToListAsync());
            return View();
        }
        public IActionResult PurchaseRequisitionGet(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
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


                    var query = _purchaseRequisitionRepository.Get();


                    var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = _purchaseRequisitionRepository.QueryTest(UserList);

                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = _purchaseRequisitionRepository.QueryTestElse();
                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = _purchaseRequisitionRepository.QueryTest(UserList);

                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = _purchaseRequisitionRepository.QueryTestElse();

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
        public async Task<IActionResult> PurchaseRequisitionDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseRequisitionMain = _purchaseRequisitionRepository.Details(id);
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
            lastpurchaseprice = _context.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id && x.GoodsReceiveMain.Status > 0).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate).FirstOrDefault();
            if (lastpurchaseprice == null)
            {
                lastpurchaseprice = 0;
            }

            var ProductData = _context.Products.Include(x => x.vProductUnit).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                LastPurchasePrice = lastpurchaseprice
            }).Where(p => p.ProductId == id).FirstOrDefault();

            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            return Json(ProductData);
        }
        public IActionResult CreatePurchaseRequisition()
        {

            PurchaseRequisitionMain purchasereq = new PurchaseRequisitionMain();
            purchasereq.ReqDate = DateTime.Now.Date;
            purchasereq.BoardMeetingDate = DateTime.Now.Date;
            purchasereq.RequiredDate = DateTime.Now.Date;

            InitViewBag("Create");
            return View(purchasereq);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePurchaseRequisition(PurchaseRequisitionMain purchaseRequisitionMain)
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

                var duplicateDocument = _context.PurchaseRequisitionMain.Where(i => i.PRNo == purchaseRequisitionMain.PRNo && i.PurReqId != purchaseRequisitionMain.PurReqId && i.ComId == comid).FirstOrDefault();
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = purchaseRequisitionMain.PRNo + " already exist. Document No can not be Duplicate." });
                }


                DateTime date = purchaseRequisitionMain.ReqDate;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
                if (activefiscalmonth == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                }
                var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
                if (activefiscalyear == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                }


                //if (ModelState.IsValid)
                //{
                #region Edit request 
                if (purchaseRequisitionMain.PurReqId > 0)
                {

                    _purchaseRequisitionRepository.EditRequest(purchaseRequisitionMain);
                }
                #endregion

                #region Create Request

                else
                {
                    using (var tr = _context.Database.BeginTransaction())
                    {

                        _purchaseRequisitionRepository.SavePurchaseElse(purchaseRequisitionMain);
                        tr.Commit();
                    }

                }
                #endregion

                return Json(new { Success = 1, PurReqId = purchaseRequisitionMain.PurReqId, ex = "" });

            }
            catch (Exception e)
            {
                ex = e.Message;
                //return Json(new { Success = 0, error = 1, ex = e.Message });
            }


            return Json(new { Success = 0, error = 1, ex = ex });

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
                        _purchaseRequisitionRepository.UpdateProduct(product);
                        TempData["Status"] = "2";
                        TempData["Message"] = "Data Update Successfully";

                        return Json(new { Success = 2, data = product, ex = TempData["Message"].ToString() });
                    }
                    else
                    {
                        _purchaseRequisitionRepository.AddProduct(product);
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



                var product = _purchaseRequisitionRepository.getProduct(id);
                var unit = _purchaseRequisitionRepository.getUnit(id);


                return Json(unit);
                //return Json("tesst" );

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
            //return Json(new SelectList(product, "Value", "Text" ));
        }

        public JsonResult GetProducts(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            IEnumerable<object> product;
            if (id != null)
            {
                if (id == 0 || id == -1)
                {

                    product = new SelectList(_context.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                }
                else
                {
                    product = new SelectList(_context.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                }
            }
            else
            {

                product = new SelectList(_context.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

            }
            return Json(new { item = product });
        }
        [HttpPost]
        public ActionResult DeletePrSub(int prsubid)
        {
            try
            {
                _purchaseRequisitionRepository.DeletePrSub(prsubid);
                return Json(new { error = 0, success = 1, message = "This record deleted successfully" });
            }
            catch (Exception ex)
            {
                var m = $" Message:{ex.Message}\nInner Exception:{ex.InnerException.Message}";
                return Json(new { error = 1, success = 0, message = m });
            }

        }

        public async Task<IActionResult> EditPurchaseRequisition(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";

            PurchaseRequisitionMain purchaseRequisitionMain = _purchaseRequisitionRepository.Edit(id);

            if (purchaseRequisitionMain == null)
            {
                return NotFound();
            }
            InitViewBag("Edit", id, purchaseRequisitionMain);
            //return Json(new {data= purchaseRequisitionMain });
            return View("CreatePurchaseRequisition", purchaseRequisitionMain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPurchaseRequisition(int id, PurchaseRequisitionMain purchaseRequisitionMain)
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
                    _purchaseRequisitionRepository.UpdatePurchase(purchaseRequisitionMain);
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
            ViewData["ApprovedByEmpId"] = _purchaseRequisitionRepository.ApprovedByEmpId(purchaseRequisitionMain);
            ViewData["DepartmentId"] = _departmentRepository.GetDepartmentList();
            ViewData["PrdUnitId"] = _purchaseRequisitionRepository.PrdUnitId(purchaseRequisitionMain);
            ViewData["PurposeId"] = _purchaseRequisitionRepository.PurposeId(purchaseRequisitionMain);
            ViewData["RecommenedByEmpId"] = _purchaseRequisitionRepository.RecommendedEmpId(purchaseRequisitionMain);
            ViewData["ProductId"] = _purchaseRequisitionRepository.ProductId();
            ViewData["UnitId"] = _unitRepository.GetUnitList();
            ViewData["SectId"] = _purchaseRequisitionRepository.SectId(purchaseRequisitionMain);
            return View(purchaseRequisitionMain);
        }

        public async Task<IActionResult> DeletePurchaseRequisition(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            PurchaseRequisitionMain purchaseRequisitionMain = _purchaseRequisitionRepository.Edit(id);
            // PurchaseRequisitionMain purchaseRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
            if (purchaseRequisitionMain == null)
            {
                return NotFound();
            }
            InitViewBag("Delete", id, purchaseRequisitionMain);
            //return Json(new {data= purchaseRequisitionMain });
            return View("CreatePurchaseRequisition", purchaseRequisitionMain);

        }

        // POST: PurchaseRequisition/Delete/5
        [HttpPost, ActionName("DeletePurchaseRequisition")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePurchaseRequisitionConfirmed(int id)
        {
            try
            {
                var storeRequisitionMain = _context.PurchaseRequisitionMain.Find(id);
                _purchaseRequisitionRepository.DeletePurchase(id);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.PurReqId.ToString(), "Delete", storeRequisitionMain.PurReqId.ToString());

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
                ViewData["Employees"] = _empReleaseRepository.EmpList();
                ViewData["CategoryId"] = _goodsReceiveRepository.CategoryId();
                ViewData["DepartmentId"] = _departmentRepository.GetDepartmentList();
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnit();
                ViewData["PurposeId"] = _goodsReceiveRepository.PurposeId();
                //ViewData["RecommenedByEmpId"] = new SelectList(db.Employee.Select(x => new { x.}), "EmployeeId", "EmployeeName");
                //ViewData["ProductId"] = new SelectList(db.Products.Select(x => new { x.ProductId,x.ProductName}), "ProductId", "ProductName");
                //ViewData["ProductBrand"] = new SelectList(db.Products.Where(x => x.ProductBrand != null).Select(x => new { x.}), "ProductBrand", "ProductBrand");
                ViewData["UnitId"] = _unitRepository.GetUnitList();
                ViewData["SectId"] = _sectionRepository.GetSectionList();

            }
            else if (title == "Edit" || title == "Delete")
            {
                ViewData["CategoryId"] = _goodsReceiveRepository.CategoryId();
                ViewBag.reqsub = _context.PurchaseRequisitionSub.Where(r => r.PurReqId == id).ToList();
                ViewData["Employees"] = _empReleaseRepository.EmpList();
                ViewData["DepartmentId"] = _departmentRepository.GetDepartmentList();
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnit();
                ViewData["PurposeId"] = _goodsReceiveRepository.PurposeId();
                ViewData["ProductId"] = new SelectList(_context.Products.Where(c => c.comid == comid), "ProductId", "ProductName");
                ViewData["UnitId"] = _unitRepository.GetUnitList();
                ViewData["SectId"] = _sectionRepository.GetSectionList();
            }

        }
        public ActionResult PrintPurchaseRequisition(int? id, string type)
        {
            string callBackUrl = _purchaseRequisitionRepository.Print(id, type);
            return Redirect(callBackUrl);
        }
        private bool PurchaseRequisitionMainExists(int id)
        {
            return _context.PurchaseRequisitionMain.Any(e => e.PurReqId == id);
        }
        #endregion


        #region goodsReceive


        [HttpPost]
        public IActionResult CreateProductForGoodsReceive(Models.Product product)
        {
            try
            {


                if (ModelState.IsValid)
                {


                    var status = _goodsReceiveRepository.ProductSave(product);
                    if (status == 1)
                    {

                        TempData["Status"] = "1";
                        TempData["Message"] = "Data Save Successfully";

                        return Json(new { Success = 1, data = product, ex = TempData["Message"].ToString() });
                    }

                    if (status == 2)
                    {
                        TempData["Status"] = "2";
                        TempData["Message"] = "Data Update Successfully";

                        return Json(new { Success = 2, data = product, ex = TempData["Message"].ToString() });
                    }

                    else
                    {
                        return Json(new { Success = 3, ex = "Model State Not Valid" });
                    }


                }
                else
                {
                    return Json(new { Success = 3, ex = "Model State Not Valid" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = 3, ex = e.InnerException });

            }
        }


        // GET: Inventory/GoodsReceiveList
        public IActionResult GoodsReceiveList()
        {

            ViewBag.Userlist =  _IStoreRequisitionRepository.GetUserList();
            ViewBag.PrdUnitId = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");


            return View();
        }


        public IActionResult GetGoodReceive(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
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
                    var query = _goodsReceiveRepository.GoodsReceiveResults();


                    var parser = new Parser<GoodsReceiveResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {
                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = _goodsReceiveRepository.GoodsReceiveResultsByUserAndCustomer(UserList, FromDate, ToDate, CustomerList);

                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = _goodsReceiveRepository.GoodsReceiveResultsByCustomer(UserList, FromDate, ToDate, CustomerList);


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = _goodsReceiveRepository.GoodsReceiveResultsByUser(UserList, FromDate, ToDate, CustomerList);


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = _goodsReceiveRepository.GoodsReceiveResultsByDate(FromDate, ToDate);


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
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


        public JsonResult PurchaseOrderDetailsById(int? POId)
        {

            var OrderDetails = _goodsReceiveRepository.GetData().Where(o => o.PurOrderMainId == POId).Select(o => new
            {

                o.PurOrderMainId,
                o.PurchaseOrderSub.FirstOrDefault().PurOrderSubId,
                o.PurchaseOrderSub.FirstOrDefault().PurReqSubId,
                o.PurchaseOrderSub.FirstOrDefault().vProduct.ProductName,
                o.PurchaseOrderSub.FirstOrDefault().vProduct.vProductUnit.UnitName,
                o.ConvertionRate,
                o.CurrencyId,
                o.Currency.CurCode,
                o.Deduction,
                o.Department,
                //ExpectedReciveDateString = o.ExpectedReciveDate.ToString(),
                //GateInHouseDateString = o.GateInHouseDate.ToString(),
                //o.ExpectedReciveDate,
                //o.GateInHouseDate,
                o.LastDateOfDelivery,
                o.ExpectedRecivedDate,
                o.NetPOValue,
                o.PaymentTypeId,
                o.PaymentType.TypeName,
                o.PODate,
                o.PONo,
                o.PORef,
                o.PurReqId,
                o.PurchaseRequisitionMain.PRNo,
                o.PrdUnitId,
                o.PrdUnit.PrdUnitName,
                o.Remarks,
                o.SectId,
                o.Section.SectName,
                o.SupplierId,
                o.Supplier.SupplierName,
                o.TermsAndCondition
            }).FirstOrDefault();

            return Json(OrderDetails);
        }



        public JsonResult PurchaseOrderSubDataByPOMId(int PurOrderMainId)
        {
            try
            {
                var GoodsReceiveDetailsInformation = _goodsReceiveRepository.PurchaseOrderSubDataByPOMId(PurOrderMainId);

                return Json(GoodsReceiveDetailsInformation);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public async Task<IActionResult> GoodsReceiveDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GoodsReceiveMain goodsReceiveMain = await _goodsReceiveRepository.GetGoodsReceiveMainById(id);
            if (goodsReceiveMain == null)
            {
                return NotFound();
            }

            return View(goodsReceiveMain);
        }


        public IActionResult DirectGrr()
        {

            var comid = (HttpContext.Session.GetString("comid"));
            var userid = (HttpContext.Session.GetString("userid"));

            this.ViewBag.AccountMain = _goodsReceiveRepository.AccountMain();


            GoodsReceiveMain goodsreceive = new GoodsReceiveMain();



            var lastgoodsreceiveMain = _goodsReceiveRepository.LastGoodsReceiveMain();

            if (lastgoodsreceiveMain != null)
            {
                goodsreceive = lastgoodsreceiveMain;



                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", lastgoodsreceiveMain.PrdUnitId);
            }
            else
            {

                goodsreceive.GRRDate = DateTime.Now.Date;
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
            }


            //goodsreceive.GRRDate = DateTime.Now.Date;
            goodsreceive.GRRMainId = 0;  // for create data added himu
            goodsreceive.ChallanDate = DateTime.Now.Date;
            goodsreceive.CertificateDate = DateTime.Now.Date;
            goodsreceive.LPDate = DateTime.Now.Date;


            //this.ViewBag.WarehouseList = db.Warehouses.Where(x => x.comid == comid && x.WhType == "L");
            this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

            ViewBag.Title = "Create";
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentTypeId();
            ViewData["SupplierId"] = _supplierRepository.GetSupplierList();
            ViewData["WarehouseId"] = _goodsReceiveRepository.WarehouseId();


            ViewData["Employees"] = _empReleaseRepository.EmpList();
            //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x => x.comid == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");

            #region CategoryId viewbag selectlist
            List<Category> categorydb = PL.GetCategory().Where(c => c.CategoryId > 0).ToList();

            List<SelectListItem> categoryid = new List<SelectListItem>();
            categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });

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
            ViewData["PurposeId"] = _goodsReceiveRepository.PurposeId();

            ViewData["ProductId"] = _goodsReceiveRepository.ProductId();

            ViewData["UnitId"] = _unitRepository.GetUnitList();

            return View(goodsreceive);
        }



        public JsonResult GetProductsGRR(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            IEnumerable<object> product;
            if (id != null)
            {
                if (id == 0 || id == -1)
                {
                    product = _purchaseRequisitionRepository.ProductList();

                }
                else
                {
                    product = _goodsReceiveRepository.ProductByCategoryId(id);
                    //product = db.Products.Where(x => x.CategoryId == id).Select(x => new { x.ProductId, x.ProductName }).ToList();
                }
            }
            else
            {
                //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                product = new SelectList(_context.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

            }
            return Json(new { item = product });
        }



        [HttpPost]
        public ActionResult DirectGrrGetProductInfo(int id, int whId)
        {
            var comid = HttpContext.Session.GetString("comid");

            //decimal? lastpurchaseprice;
            //lastpurchaseprice=  db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id && x.GoodsReceiveMain.Status > 0).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate).FirstOrDefault();
            //if (lastpurchaseprice == null)
            //{
            //    lastpurchaseprice = 0;
            //}

            var ProductData = _context.Products.Include(x => x.vProductUnit).Include(x => x.CostCalculated).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                //AvgRate = p.CostCalculated.Where(x=>x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CalculatedPrice).FirstOrDefault().ToString(), //lastpurchaseprice,
                //StockQty = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CurrQty).FirstOrDefault().ToString(),
                AvgRate = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CalculatedPrice).FirstOrDefault().ToString(), //lastpurchaseprice,
                StockQty = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Sum(x => x.CurrQty).ToString()
            }).Where(p => p.ProductId == id).FirstOrDefault();// ToList();

            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            return Json(ProductData);
        }

        // GET: GoodsReceive/Create
        public IActionResult Create()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L");


            GoodsReceiveMain goodsreceive = new GoodsReceiveMain();
            goodsreceive.GRRDate = DateTime.Now.Date;
            goodsreceive.ChallanDate = DateTime.Now.Date;
            goodsreceive.CertificateDate = DateTime.Now.Date;
            goodsreceive.LPDate = DateTime.Now.Date;

            ViewBag.Title = "Create";
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnit();
            ViewData["PurReqId"] = _goodsReceiveRepository.PurReqId3();
            ViewData["PurOrderMainId"] = _goodsReceiveRepository.PurOrderMainId3();
            ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId3();
            ViewData["WarehouseId"] = _goodsReceiveRepository.WarehouseId();

            return View(goodsreceive);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GoodsReceiveMain goodsReceiveMain)
        {
            try
            {
                //var errors = ModelState.Where(x => x.Value.Errors.Any())
                //               .Select(x => new { x.Key, x.Value.Errors });

                var result = "";

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var pcname = HttpContext.Session.GetString("pcname");
                var nowdate = DateTime.Now;

                var duplicateDocument = await _goodsReceiveRepository.DuplicateData(goodsReceiveMain);
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = goodsReceiveMain.GRRNo + " already exist." });
                }



                var activefiscalmonth = _goodsReceiveRepository.FiscalMonth(goodsReceiveMain);
                if (activefiscalmonth == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                }
                var activefiscalyear = _goodsReceiveRepository.FiscalYear(goodsReceiveMain);
                if (activefiscalyear == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                }

                var lockCheck = _goodsReceiveRepository.LockCheck(goodsReceiveMain);
                if (lockCheck != null)
                {
                    return Json(new { Success = 0, ex = "Store Lock this date!!!" });
                }

                /////for rectifying the entry value + and value -
                foreach (var item in goodsReceiveMain.GoodsReceiveSub)
                {
                    if (item.Quality == 0 || item.Quality == 0)
                    {
                        item.TotalValue = item.Rate;
                    }

                }


                using (var tr = _context.Database.BeginTransaction())
                {
                    if (goodsReceiveMain != null)
                    {
                        try
                        {
                            _goodsReceiveRepository.CreateIfElsePart(goodsReceiveMain);

                        }
                        catch (SqlException ex)
                        {

                            Console.WriteLine(ex.Message);
                            tr.Rollback();
                            return Json(new { Success = 0, ex = ex.Message.ToString() });

                        }
                    }
                    tr.Commit();
                }

                ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId2(goodsReceiveMain);
                ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentId2(goodsReceiveMain);
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnitId2(goodsReceiveMain);
                ViewData["PurOrderMainId"] = _goodsReceiveRepository.PurOrderMainId2(goodsReceiveMain);
                ViewData["PurReqId"] = _goodsReceiveRepository.PurReqId2(goodsReceiveMain);
                ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId2(goodsReceiveMain);
                ViewData["WarehouseId"] = _goodsReceiveRepository.WareHouseId2();
                return Json(new { Success = result, ex = "Data Save Successfully!" });
            }
            catch (Exception ex)
            {

                //throw ex;
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        public async Task<IActionResult> View(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "View";

            var goodsreceive = _goodsReceiveRepository.FindById(id);

            if (goodsreceive == null)
            {
                return NotFound();
            }

            if (goodsreceive.IsDirectGRR == true)
            {
                var goodsReceiveMain = _goodsReceiveRepository.GetGoodsReceiveMain2(id);

                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

                ViewData["Employees"] = _empReleaseRepository.EmpList();

                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
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


                //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x=>x.comid==comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
                ViewData["DeptId"] = _goodsReceiveRepository.DepartmentList2(goodsReceiveMain);
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnitId2(goodsReceiveMain);
                ViewData["PurposeId"] = _goodsReceiveRepository.PurposeId();
                ViewData["ProductId"] = _goodsReceiveRepository.ProductId();
                ViewData["UnitId"] = _unitRepository.GetUnitList();
                ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentId2(goodsReceiveMain);
                ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId2(goodsReceiveMain);
                ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId2(goodsReceiveMain);
                ViewData["WarehouseId"] = _goodsReceiveRepository.WareHouseId2();
                this.ViewBag.AccountMain = _goodsReceiveRepository.AccountMain();

                return View("DirectGrr", goodsReceiveMain);
            }
            else
            {

                var goodsReceiveMain = await _goodsReceiveRepository.GetDelete(id);


                //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.DeptId);
                ViewData["DeptId"] = _goodsReceiveRepository.DepartmentList2(goodsReceiveMain);
                ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId2(goodsReceiveMain);
                ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentId2(goodsReceiveMain);
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnitId2(goodsReceiveMain);
                ViewData["PurOrderMainId"] = _goodsReceiveRepository.PurOrderMainId2(goodsReceiveMain);
                //ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
                ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId2(goodsReceiveMain);
                ViewData["WarehouseId"] = _goodsReceiveRepository.WareHouseId2();


                return View("Create", goodsReceiveMain);
            }
        }

        // GET: GoodsReceive/Edit/5
        public async Task <IActionResult> Edit(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            var goodsreceive = _goodsReceiveRepository.FindById(id);

            if (goodsreceive == null)
            {
                return NotFound();
            }

            if (goodsreceive.IsDirectGRR == true)
            {
                var goodsReceiveMain = await _goodsReceiveRepository.GetDelete(id);
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

                ViewData["Employees"] = _empReleaseRepository.EmpList();

                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
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


                //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x=>x.comid==comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
                ViewData["DeptId"] = _goodsReceiveRepository.DepartmentList2(goodsReceiveMain);
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnitId2(goodsReceiveMain);
                //ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
                ViewData["PurposeId"] = _goodsReceiveRepository.PurposeId();
                ViewData["ProductId"] = _goodsReceiveRepository.ProductId();
                ViewData["UnitId"] = _unitRepository.GetUnitList();
                ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentId2(goodsReceiveMain);
                ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId2(goodsReceiveMain);
                ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId2(goodsReceiveMain);
                ViewData["WarehouseId"] = _goodsReceiveRepository.WareHouseId2();
                this.ViewBag.AccountMain = _goodsReceiveRepository.AccountMain();

                return View("DirectGrr", goodsReceiveMain);
            }
            else
            {

                var goodsReceiveMain = await _goodsReceiveRepository.GetDelete(id);

                //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.DeptId);
                ViewData["DeptId"] = _goodsReceiveRepository.DepartmentList2(goodsReceiveMain);
                ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId2(goodsReceiveMain);
                ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentId2(goodsReceiveMain);
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnitId2(goodsReceiveMain);
                ViewData["PurOrderMainId"] = _goodsReceiveRepository.PurOrderMainId2(goodsReceiveMain);
                //ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
                ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId2(goodsReceiveMain);
                ViewData["WarehouseId"] = _goodsReceiveRepository.WareHouseId2();


                return View("Create", goodsReceiveMain);
            }
        }

        // POST: GoodsReceive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GoodsReceiveMain goodsReceiveMain)
        {
            if (id != goodsReceiveMain.GRRMainId)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            if (ModelState.IsValid)
            {
                try
                {
                    _goodsReceiveRepository.Update(goodsReceiveMain);
                   
                }
                catch (Exception e)
                {
                    if (!GoodsReceiveMainExists(goodsReceiveMain.GRRMainId))
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
            //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.SectId);
            ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId2(goodsReceiveMain);
            ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentId2(goodsReceiveMain);
            ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnitId2(goodsReceiveMain);
            ViewData["PurOrderMainId"] = _goodsReceiveRepository.PurOrderMainId2(goodsReceiveMain);
            ViewData["PurReqId"] = _goodsReceiveRepository.PurReqId2(goodsReceiveMain);
            ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId2(goodsReceiveMain);
            ViewData["WarehouseId"] = _goodsReceiveRepository.WareHouseId2();
            return View(goodsReceiveMain);
        }

        // GET: GoodsReceive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            var comid = HttpContext.Session.GetString("comid");

            var goodsReceiveMain = await _goodsReceiveRepository.GetDelete(id);
            if (goodsReceiveMain == null)
            {
                return NotFound();
            }
            //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.SectId);
            ViewData["CurrencyId"] = _goodsReceiveRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _goodsReceiveRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnit();
            ViewData["PurOrderMainId"] = _goodsReceiveRepository.PurOrderMainId(id);
            ViewData["PurReqId"] = _goodsReceiveRepository.PurReqId(id);
            ViewData["SupplierId"] = _goodsReceiveRepository.SupplierId(id);
            ViewData["WarehouseId"] = _goodsReceiveRepository.WarehouseId();
            this.ViewBag.AccountMain = _goodsReceiveRepository.AccountMain();


            if (goodsReceiveMain.IsDirectGRR == true)
            {
                goodsReceiveMain = await _goodsReceiveRepository.GetDelete(id);
                // goodsReceiveMain = await db.GoodsReceiveMain               
                //.Include(g => g.GoodsReceiveSub)
                //.ThenInclude(g => g.vProduct)
                //.ThenInclude(g => g.vProductUnit)
                //.Where(g => g.GRRMainId == id).FirstOrDefaultAsync();


                ViewData["Employees"] = _empReleaseRepository.EmpList();
                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
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
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

                ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
                ViewData["PrdUnitId"] = _goodsReceiveRepository.PrdUnit();
                ViewData["PurposeId"] = _goodsReceiveRepository.PurposeId();

                ViewData["ProductId"] = _goodsReceiveRepository.ProductId();

                ViewData["UnitId"] = _unitRepository.GetUnitList();

                return View("DirectGrr", goodsReceiveMain);
            }
            else
            {
                return View("Create", goodsReceiveMain);
            }


            //db.GoodsReceiveMain.Remove(goodsReceiveMain);
            //await db.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }



        [HttpPost, ActionName("Delete")]
        //public JsonResult DeleteConfirmed(int id)
        public async Task<IActionResult> DeleteGoodConfirmed(int id)
        {
            try
            {
                var goodsReceiveMain = _context.GoodsReceiveMain.Find(id);
                _goodsReceiveRepository.DeleteGoodReceive(id);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), goodsReceiveMain.GRRMainId.ToString(), "Delete", goodsReceiveMain.GRRNo);

                return Json(new { Success = 1, VoucherID = goodsReceiveMain.GRRMainId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }


            // return RedirectToAction("Index");
        }




        private bool GoodsReceiveMainExists(int id)
        {
            return _context.GoodsReceiveMain.Any(e => e.GRRMainId == id);
        }






        public ActionResult Print(int? id, string type)
        {
            string redirectUrl = _goodsReceiveRepository.printGoodsReceive(id, type);
            return Redirect(redirectUrl);
        }
        [HttpPost, ActionName("PrintGrrSummary")]
        public JsonResult GrrSummaryReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {



                string redirectUrl = _goodsReceiveRepository.GrrSummaryReport(rptFormat, action, FromDate, ToDate, PrdUnitId);
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


        [HttpPost, ActionName("PrintGrrDetails")]
        public JsonResult GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {
                string redirectUrl = _goodsReceiveRepository.GrrDetailsReport(rptFormat, action, FromDate, ToDate, PrdUnitId);
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



        [HttpPost, ActionName("PrintGrrVoucherLocal")]
        public JsonResult PrintGrrVoucherLocal(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId) 
        {
            try
            {
                string redirectUrl = _goodsReceiveRepository.PrintGrrVoucherLocal(rptFormat, action, FromDate, ToDate, PrdUnitId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
          

        }

        [HttpPost, ActionName("PrintGrrVoucherForeign")]
        public JsonResult PrintGrrVoucherForeign(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {



                string redirectUrl = _goodsReceiveRepository.PrintGrrVoucherForeign(rptFormat, action, FromDate, ToDate, PrdUnitId);
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


        [HttpPost, ActionName("PrintMissingSequence")]
        public JsonResult PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, int PrdUnitId)
        {
            try
            {
                string redirectUrl = _goodsReceiveRepository.PrintMissingSequence(rptFormat, action, Type, FromNo, ToNo, PrdUnitId);
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

        #endregion

        #region Purchase Order

        // GET: PurchaseOrder
        public IActionResult PurchaseOrderList()
        {
            ViewBag.Userlist = _purchaseOrderRepository.Userlist();
            return View();
        }
        
        public IActionResult GetPurchaseOrder(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
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
                    var query = from e in _context.PurchaseOrderMain.Where(x => x.ComId == comid)
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
                        var querytest = from e in _context.PurchaseOrderMain
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
                        var querytest = from e in _context.PurchaseOrderMain
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
                        var querytest = from e in _context.PurchaseOrderMain
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
                        var querytest = from e in _context.PurchaseOrderMain
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
        public IActionResult PurchaseOrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchaseOrderMain = _purchaseOrderRepository.PurchaseOrderMain(id);
            if (purchaseOrderMain == null)
            {
                return NotFound();
            }
            return View(purchaseOrderMain);
        }

        public IActionResult GetCurrencyRate(int id)
        {
            var CurrencyRate = _context.Currency.Where(c => c.CurrencyId == id);
            return Json(CurrencyRate);
        }

        public IActionResult GetDepartmentByPurReqId(int id)
        {
            var Department = _context.PurchaseRequisitionMain.Where(p => p.PurReqId == id).Select(p => new
            {
                DeptName = p.Department.DeptName,
                p.PurReqId
            }).FirstOrDefault();
            return Json(Department);
        }

        public JsonResult GetPurchaseRequisitionDataById(int? PurReqId)
        {
            var PurchaseOrderDetailsInformation = _purchaseOrderRepository.GetPurchaseRequisitionDataById(PurReqId);
            return Json(PurchaseOrderDetailsInformation);
        }
        
        // GET: PurchaseOrder/Create
        public IActionResult CreatePurchaseOrder()
        {
            PurchaseOrderMain abc = new PurchaseOrderMain();
            abc.PODate = DateTime.Now.Date;
            abc.LastDateOfDelivery = DateTime.Now.Date;
            abc.ExpectedRecivedDate = DateTime.Now.Date;

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";
            ViewData["DeptId"] = _purchaseOrderRepository.DeptId();
            ViewData["SectId"] = _purchaseOrderRepository.SectId();
            ViewData["CurrencyId"] = _purchaseOrderRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _purchaseOrderRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _purchaseOrderRepository.PrdUnitId();
            ViewData["PurReqId"] = _purchaseOrderRepository.PurReqId();
            ViewData["SupplierId"] = _purchaseOrderRepository.SupplierId();
            ViewData["DistrictId"] = _purchaseOrderRepository.DistrictId();
            return View(abc);
        }

        // POST: PurchaseOrder/Create        
        [HttpPost]
        public IActionResult CreatePurchaseOrder(PurchaseOrderMain purchaseOrderMain)
        {
            var result = "";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");

            var duplicateDocument = _purchaseOrderRepository.duplicateDocument(purchaseOrderMain);
            if (duplicateDocument != null)
            {
                return Json(new { Success = 0, ex = purchaseOrderMain.PONo + " already exist. Document No can not be Duplicate." });
            }

            var activefiscalmonth = _purchaseOrderRepository.FiscalMonth(purchaseOrderMain);
            if (activefiscalmonth == null)
            {
                return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
            }
            var activefiscalyear = _purchaseOrderRepository.FiscalYear(purchaseOrderMain);
            if (activefiscalyear == null)
            {
                return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
            }

            var lockCheck = _purchaseOrderRepository.LockCheck(purchaseOrderMain);

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
                            _purchaseOrderRepository.UpdatePurchaseOrderSub(item);
                        }
                        else
                        {
                            item.ComId = comid;
                            item.UserId = userid;
                            item.PcName = pcname;
                            item.DateAdded = nowdate;
                            _purchaseOrderRepository.CreatePurchaseOrderSub(item);
                        }
                    }
                    purchaseOrderMain.UpdateByUserId = userid;
                    purchaseOrderMain.DateUpdated = nowdate;
                    _purchaseOrderRepository.Update(purchaseOrderMain);
                    result = "2";
                }
                else
                {
                    DateTime date = purchaseOrderMain.PODate;
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

                    _purchaseOrderRepository.Add(purchaseOrderMain);
                    result = "1";
                }
                _context.SaveChanges();
            }
            ViewData["DeptId"] = _purchaseOrderRepository.DeptId();
            ViewData["SectId"] = _purchaseOrderRepository.SectId();
            ViewData["CurrencyId"] = _purchaseOrderRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _purchaseOrderRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _purchaseOrderRepository.PrdUnitId();
            ViewData["PurReqId"] = _purchaseOrderRepository.PurReqId();
            ViewData["SupplierId"] = _purchaseOrderRepository.SupplierId();
             ViewData["DistrictId"] = _purchaseOrderRepository.DistrictId();
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
                    _purchaseOrderRepository.UpdateSupplier(supplier);
                }
                else
                {
                    supplier.ComId = comid;
                    supplier.UserId = userid;
                    supplier.DateAdded = date;
                    _purchaseOrderRepository.CreateSupplier(supplier);
                }
                _context.SaveChanges();
                return Json(new { Success = 1, ex = "Successfully Added", data = supplier });
            }
            return Json(new { Success = 3, ex = "Model State Not valid", data = "" });
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> EditPurchaseOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            ViewBag.Title = "Edit";
            var purchaseOrderMain = _purchaseOrderRepository.PurchaseOrderMain(id);
            if (purchaseOrderMain == null)
            {
                return NotFound();
            }
            ViewData["DeptId"] = _purchaseOrderRepository.DeptId();
            ViewData["SectId"] = _purchaseOrderRepository.SectId();
            ViewData["CurrencyId"] = _purchaseOrderRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _purchaseOrderRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _purchaseOrderRepository.PrdUnitId();
            ViewData["PurReqId"] = _purchaseOrderRepository.PurReqId();
            ViewData["SupplierId"] = _purchaseOrderRepository.SupplierId();
            ViewData["DistrictId"] = _purchaseOrderRepository.DistrictId();
            return View("CreatePurchaseOrder", purchaseOrderMain);
        }

        // POST: PurchaseOrder/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPurchaseOrder(int id, PurchaseOrderMain purchaseOrderMain)
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
                    _purchaseOrderRepository.Update(purchaseOrderMain);
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
            ViewData["DeptId"] = _purchaseOrderRepository.DeptId();
            ViewData["SectId"] = _purchaseOrderRepository.SectId();
            ViewData["CurrencyId"] = _purchaseOrderRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _purchaseOrderRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _purchaseOrderRepository.PrdUnitId();
            ViewData["PurReqId"] = _purchaseOrderRepository.PurReqId();
            ViewData["SupplierId"] = _purchaseOrderRepository.SupplierId();
            ViewData["DistrictId"] = _purchaseOrderRepository.DistrictId();
            return View(purchaseOrderMain);
        }

        // GET: PurchaseOrder/Delete/5s
        public async Task<IActionResult> DeletePurchaseOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrderMain = _purchaseOrderRepository.PurchaseOrderMain(id);

            if (purchaseOrderMain == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var pcname = HttpContext.Session.GetString("pcname");
            ViewBag.Title = "Delete";
            ViewData["DeptId"] = _purchaseOrderRepository.DeptId();
            ViewData["SectId"] = _purchaseOrderRepository.SectId();
            ViewData["CurrencyId"] = _purchaseOrderRepository.CurrencyId();
            ViewData["PaymentTypeId"] = _purchaseOrderRepository.PaymentTypeId();
            ViewData["PrdUnitId"] = _purchaseOrderRepository.PrdUnitId();
            ViewData["PurReqId"] = _purchaseOrderRepository.PurReqId();
            ViewData["SupplierId"] = _purchaseOrderRepository.SupplierId();
            ViewData["DistrictId"] = _purchaseOrderRepository.DistrictId();
            return View("CreatePurchaseOrder", purchaseOrderMain);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("DeletePurchaseOrder")]
        public JsonResult DeletePurchaseOrderConfirmed(int id)
        {
            var purchaseOrderMain = _purchaseOrderRepository.FindByIdPMain(id);
            _purchaseOrderRepository.Delete(purchaseOrderMain);
            var result = _context.SaveChanges();
            if (result > 1) return Json(true);
            else return Json(false);
        }

        public ActionResult PrintPurchaseOrder(int? id, string type)
        {

            string redirectUrl = _purchaseOrderRepository.PrintPurchaseOrder(id,type);
            return Redirect(redirectUrl);
        }

        private bool PurchaseOrderMainExists(int id)
        {
            return _context.PurchaseOrderMain.Any(e => e.PurOrderMainId == id);
        }
        #endregion

        #region Post Document

        public ViewResult PostDocumentList(string FromDate, string ToDate, string criteria, string DocType, int DeptId, int PrdUnitId)
        {
            var transactioncomid = HttpContext.Session.GetString("comid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            if (criteria == null)
            {
                criteria = "UnPost";
            }

            UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");
            if (permission.IsProduction)
            {
                ViewData["DocType"] = _postDocumentRepository.DocTypeListProductionList1();
            }
            else
            {
                ViewData["DocType"] = _postDocumentRepository.DocTypeList1();
            }

            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["PrdUnitId"] = _purchaseOrderRepository.PrdUnitId();


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

           

            var lvList = new List<HR_Leave_Avail>();

            ViewBag.Isleave = false;
            ViewBag.Title = criteria;
            List<DocumentList> doclist = _postDocumentRepository.GetDocument(FromDate, ToDate, criteria, DocType, DeptId, PrdUnitId);
            if (DocType == "Leave")
            {
                ViewBag.Isleave = true;
                if (criteria == null)
                {
                    criteria = "Pending";
                }

                ViewBag.Title = criteria;
                if (criteria == "All")
                {
                    lvList = _postDocumentRepository.Leave1(DeptId);
                }
                else if (criteria == "Pending")
                {
                    ViewBag.Title = criteria;
                    lvList = _postDocumentRepository.Leave2(DeptId);
                }
                else if (criteria == "Approved")
                {
                    lvList = _postDocumentRepository.Leave3(DeptId);
                }
                else if (criteria == "DisApproved")
                {
                    lvList = _postDocumentRepository.Leave4(DeptId);
                }
                _postDocumentRepository.Leave();
            }

            return View(doclist);
        }

        public ActionResult Print(int? id, string type, string docname)
        {
            try
            {

                 string redirectUrl = _postDocumentRepository.Print(id, type, docname);
                 return Redirect(redirectUrl);

                ///return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Obsolete]
        [ValidateAntiForgeryToken]
        public JsonResult SetProcess(string[] docid, string criteria, string[] doctype)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                //using (var tr = db.Database.BeginTransaction())
                //{
                string data = "";
                //try
                //{
                if (criteria.ToUpper().ToString() == "Post".ToUpper())
                {
                    if (docid.Count() > 0)
                    {
                        for (var i = 0; i < docid.Count(); i++)
                        {
                            string docidsingle = docid[i];
                            string doctypesingle = doctype[i];

                            if (doctypesingle == "SRR")
                            {
                                var singlevoucher = _context.StoreRequisitionMain.Where(x => x.StoreReqId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 1;
                                _context.Entry(singlevoucher).State = EntityState.Modified;

                            }
                            else if (doctypesingle == "ISSUE")
                            {
                                var singlevoucher = _context.IssueMain.Include(x => x.IssueSub).ThenInclude(x => x.IssueSubWarehouse).Where(x => x.IssueMainId == int.Parse(docidsingle)).FirstOrDefault();
                                var singlestorerequisition = _context.StoreRequisitionMain.Where(x => x.StoreReqId == singlevoucher.StoreReqId).FirstOrDefault();
                                using (var tr = _context.Database.BeginTransaction())
                                {
                                    try
                                    {

                                        if (singlevoucher.IsDirectIssue)
                                        {

                                            foreach (IssueSub ss in singlevoucher.IssueSub)
                                            {
                                                Inventory inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                                                CostCalculated cstcal = _context.CostCalculated.Where(x => x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).FirstOrDefault();

                                                if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                                                {
                                                    //if (inv.CurrentStock >= ss.IssueQty)
                                                    if (cstcal.WarehouseId == 1 && cstcal.ProductId == ss.ProductId && cstcal.WarehouseId == ss.WarehouseId && cstcal.CurrQty == -(ss.IssueQty) && cstcal.IssueMainId == ss.IssueMainId)
                                                    {
                                                        return Json(new { Success = "3", ex = "Duplicate Data Found.Can't do Process. Please contact with your Administrator." });
                                                    }

                                                    if (cstcal.PrevQty + cstcal.CurrQty >= ss.IssueQty || cstcal.ProductId == 13729 || cstcal.ProductId == 13730)/// special for production finishing goods
                                                    {

                                                        inv.IssueQty = inv.IssueQty + ss.IssueQty;
                                                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                                        _context.Entry(inv).State = EntityState.Modified;
                                                    }
                                                    else
                                                    {
                                                        var productname = _context.Products.Where(x => x.ProductId == inv.ProductId).Select(x => x.ProductName).FirstOrDefault();
                                                        var warehousename = _context.Warehouses.Where(x => x.WarehouseId == inv.WareHouseId).Select(x => x.WhName).FirstOrDefault();

                                                        var msg = productname + " not Stock in the " + warehousename + ". Warehouse Qty is " + (cstcal.PrevQty + cstcal.CurrQty) + ". Total Shortage Qty is " + (ss.IssueQty - (cstcal.PrevQty + cstcal.CurrQty)) + ".";
                                                        return Json(new { Success = "3", ex = msg });
                                                    }

                                                }
                                                else
                                                {
                                                    //var comid = HttpContext.Session.GetString("comid");
                                                    //var userid = HttpContext.Session.GetString("userid");
                                                    //var productname = _context.Products.Where(x => x.ProductId == inv.ProductId).Select(x => x.ProductName).FirstOrDefault();
                                                    //var msg = "No warehouse found for this "+ productname;
                                                    //return Json(new { Success = "3", ex = msg });

                                                    Inventory insertinv = new Inventory();

                                                    insertinv.ProductId = int.Parse(ss.ProductId.ToString());
                                                    insertinv.WareHouseId = int.Parse(ss.WarehouseId.ToString());
                                                    insertinv.OpStock = 0;

                                                    insertinv.PurQty = 0;
                                                    insertinv.PurRetQty = 0;
                                                    insertinv.PurExcQty = 0;

                                                    insertinv.SalesQty = 0;
                                                    insertinv.SalesRetQty = 0;
                                                    insertinv.SalesExcQty = 0;

                                                    insertinv.ChallanQty = 0;
                                                    insertinv.EnStock = 0;
                                                    insertinv.CurrentStock = 0;

                                                    insertinv.GoodsReceiveQty = 0;
                                                    insertinv.IssueQty = 0;
                                                    insertinv.GoodsRcvRtnQty = 0;
                                                    insertinv.IssueRtnQty = 0;

                                                    insertinv.comid = comid;
                                                    insertinv.userid = userid;
                                                    insertinv.useridUpdate = userid;
                                                    insertinv.DateAdded = DateTime.Now;
                                                    insertinv.DateUpdated = DateTime.Now;
                                                    insertinv.OpeningStockDate = ss.IssueMain.IssueDate;
                                                    insertinv.Remarks = "Auto From Issue Post";
                                                    _context.Entry(insertinv).State = EntityState.Added;
                                                    _context.SaveChanges();

                                                    inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                                                    inv.IssueQty = inv.IssueQty + ss.IssueQty;
                                                    //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                    _context.Entry(inv).State = EntityState.Modified;
                                                }
                                            }


                                        }
                                        else
                                        {


                                            foreach (IssueSub ss in singlevoucher.IssueSub)
                                            {
                                                foreach (IssueSubWarehouse wh in ss.IssueSubWarehouse)
                                                {
                                                    Inventory inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == wh.WarehouseId).FirstOrDefault();
                                                    if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                                                    {

                                                        inv.IssueQty = inv.IssueQty + wh.IssueQty;
                                                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                                        _context.Entry(inv).State = EntityState.Modified;
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        //var comid = HttpContext.Session.GetString("comid");
                                                        //var userid = HttpContext.Session.GetString("userid");

                                                        Inventory insertinv = new Inventory();

                                                        insertinv.ProductId = int.Parse(wh.ProductId.ToString());
                                                        insertinv.WareHouseId = int.Parse(wh.WarehouseId.ToString());
                                                        insertinv.OpStock = 0;

                                                        insertinv.PurQty = 0;
                                                        insertinv.PurRetQty = 0;
                                                        insertinv.PurExcQty = 0;

                                                        insertinv.SalesQty = 0;
                                                        insertinv.SalesRetQty = 0;
                                                        insertinv.SalesExcQty = 0;

                                                        insertinv.ChallanQty = 0;
                                                        insertinv.EnStock = 0;
                                                        insertinv.CurrentStock = 0;

                                                        insertinv.GoodsReceiveQty = 0;
                                                        insertinv.IssueQty = 0;
                                                        insertinv.GoodsRcvRtnQty = 0;
                                                        insertinv.IssueRtnQty = 0;

                                                        insertinv.comid = comid;
                                                        insertinv.userid = userid;
                                                        insertinv.useridUpdate = userid;
                                                        insertinv.DateAdded = DateTime.Now;
                                                        insertinv.DateUpdated = DateTime.Now;
                                                        insertinv.OpeningStockDate = ss.IssueMain.IssueDate;
                                                        insertinv.Remarks = "Auto From Issue Post";
                                                        _context.Entry(insertinv).State = EntityState.Added;
                                                        _context.SaveChanges();

                                                        inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == wh.WarehouseId).FirstOrDefault();

                                                        inv.IssueQty = inv.IssueQty + wh.IssueQty;
                                                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                        _context.Entry(inv).State = EntityState.Modified;
                                                        _context.SaveChanges();
                                                    }

                                                }
                                            }





                                        }
                                        singlevoucher.Status = 1;
                                        _context.Entry(singlevoucher).State = EntityState.Modified;

                                        #region For Substore auto receive - GRR
                                        ////newly added code by fahad for check and perfection the code
                                        /////////////if sub store issue is enable then auto qty increase in the sub store receiving qty to issue product and balance calculation 

                                        if (singlestorerequisition != null)
                                        {///need to check those code today 19-sep-2020
                                            if (singlestorerequisition.IsSubStore == true)
                                            {
                                                var subwarehouseid = singlestorerequisition.SubWarehouseId;



                                                foreach (IssueSub ss in singlevoucher.IssueSub)
                                                {
                                                    //foreach (IssueSubWarehouse wh in ss.IssueSubWarehouse)
                                                    //{
                                                    Inventory inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == subwarehouseid).FirstOrDefault();
                                                    if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                                                    {

                                                        inv.GoodsReceiveQty = inv.GoodsReceiveQty + ss.IssueQty;
                                                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                                        _context.Entry(inv).State = EntityState.Modified;
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        //var comid = HttpContext.Session.GetString("comid");
                                                        //var userid = HttpContext.Session.GetString("userid");

                                                        Inventory insertinv = new Inventory();

                                                        insertinv.ProductId = int.Parse(ss.ProductId.ToString());
                                                        insertinv.WareHouseId = int.Parse(subwarehouseid.ToString());
                                                        insertinv.OpStock = 0;

                                                        insertinv.PurQty = 0;
                                                        insertinv.PurRetQty = 0;
                                                        insertinv.PurExcQty = 0;

                                                        insertinv.SalesQty = 0;
                                                        insertinv.SalesRetQty = 0;
                                                        insertinv.SalesExcQty = 0;

                                                        insertinv.ChallanQty = 0;
                                                        insertinv.EnStock = 0;
                                                        insertinv.CurrentStock = ss.IssueQty;

                                                        insertinv.GoodsReceiveQty = ss.IssueQty;
                                                        insertinv.IssueQty = 0;
                                                        insertinv.GoodsRcvRtnQty = 0;
                                                        insertinv.IssueRtnQty = 0;

                                                        insertinv.comid = comid;
                                                        insertinv.userid = userid;
                                                        insertinv.useridUpdate = userid;
                                                        insertinv.DateAdded = DateTime.Now;
                                                        insertinv.DateUpdated = DateTime.Now;
                                                        insertinv.OpeningStockDate = ss.IssueMain.IssueDate;
                                                        insertinv.Remarks = "Auto From SubStore Issue Post";
                                                        _context.Entry(insertinv).State = EntityState.Added;

                                                        _context.SaveChanges();



                                                        ////need to check from here for auto posting to grr if store req issubstoreissue == true // fahad need to check
                                                        //inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == subwarehouseid && x.comid == comid).FirstOrDefault();


                                                        ////inv.GoodsReceiveQty = inv.GoodsReceiveQty + wh.IssueQty;
                                                        ////inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                                        //_context.Entry(inv).State = EntityState.Modified;
                                                        //_context.SaveChanges();




                                                    }

                                                    //_context.SaveChanges();
                                                    //}
                                                    //exec CustBill_Corporate._contexto.[prcAvgCostingProcess] 'grr' , 'grrid' , comid
                                                    ///@Type varchar(30), @Id bigint , @ComId varchar(200) --, @ProductId int

                                                    //exec CustBill_Corporate._contexto.[prcAvgCostingProcess] 'grr' , 'grrid' , comid
                                                }






                                                //var a = _context.Database.ExecuteSqlCommand("Exec prcAvgCostingProcess @Type ,@Id, @ComId ", new SqlParameter("@Type", "Issue"), new SqlParameter("@Id", singlevoucher.IssueMainId), new SqlParameter("@ComId", singlevoucher.ComId));


                                            }
                                        }

                                    }
                                    catch (SqlException ex)
                                    {

                                        Console.WriteLine(ex.Message);
                                        tr.Rollback();

                                        return Json(new { Success = "3", ex = "Something Wrong" });

                                    }

                                    tr.Commit();


                                    if (singlevoucher != null)
                                    {
                                        var query = $"Exec prcAvgCostingProcess {"Issue"},{singlevoucher.IssueMainId},'{singlevoucher.ComId}'";

                                        SqlParameter[] sqlParameter = new SqlParameter[4];
                                        sqlParameter[0] = new SqlParameter("@Type", "Issue");
                                        sqlParameter[1] = new SqlParameter("@Id", singlevoucher.IssueMainId);
                                        sqlParameter[2] = new SqlParameter("@comId", singlevoucher.ComId);
                                        sqlParameter[3] = new SqlParameter("@userid", userid);


                                        Helper.ExecProc("prcAvgCostingProcess", sqlParameter);
                                        //var a = _context.Database.ExecuteSqlCommand("Exec prcAvgCostingProcess @Type ,@Id, @ComId ", new SqlParameter("@Type", "Issue"), new SqlParameter("@Id", singlevoucher.IssueMainId), new SqlParameter("@ComId", singlevoucher.ComId));

                                    }


                                    if (singlestorerequisition != null)
                                    {
                                        var query = $"Exec prcAvgCostingProcess {"'StoreReq'"},{singlestorerequisition.StoreReqId},'{singlevoucher.ComId}'";
                                        SqlParameter[] sqlParameter1 = new SqlParameter[4];
                                        sqlParameter1[0] = new SqlParameter("@Type", "StoreReq");
                                        sqlParameter1[1] = new SqlParameter("@Id", singlestorerequisition.StoreReqId);
                                        sqlParameter1[2] = new SqlParameter("@comId", singlevoucher.ComId);
                                        sqlParameter1[3] = new SqlParameter("@userid", userid);


                                        Helper.ExecProc("prcAvgCostingProcess", sqlParameter1);

                                    }


                                    #endregion

                                }


                            }
                            else if (doctypesingle == "PR")
                            {
                                var singlevoucher = _context.PurchaseRequisitionMain.Where(x => x.PurReqId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 1;
                                _context.Entry(singlevoucher).State = EntityState.Modified;


                            }
                            else if (doctypesingle == "PO")
                            {
                                var singlevoucher = _context.PurchaseOrderMain.Where(x => x.PurOrderMainId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 1;
                                _context.Entry(singlevoucher).State = EntityState.Modified;


                            }
                            else if (doctypesingle == "GRR")
                            {
                                //var statusupdate = _context.GoodsReceiveMain.Where(x => x.GRRMainId == int.Parse(docidsingle)).FirstOrDefault();
                                //var goodsreceivesub = _context.GoodsReceiveSub.Include(x => x.GoodsReceiveSubWarehouse).Where(x => x.GRRMainId == int.Parse(docidsingle));
                                var singlevoucher = _context.GoodsReceiveMain.Include(x => x.GoodsReceiveSub).ThenInclude(x => x.GoodsReceiveSubWarehouse).Where(x => x.GRRMainId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 1;
                                _context.Entry(singlevoucher).State = EntityState.Modified;

                                if (singlevoucher.IsDirectGRR == true)
                                {

                                    foreach (GoodsReceiveSub ss in singlevoucher.GoodsReceiveSub)
                                    {

                                        Inventory inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                                        if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                                        {

                                            inv.GoodsReceiveQty = inv.GoodsReceiveQty + ss.Quality;
                                            //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                            inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                            _context.Entry(inv).State = EntityState.Modified;
                                        }
                                        else
                                        {


                                            Inventory insertinv = new Inventory();

                                            insertinv.ProductId = int.Parse(ss.ProductId.ToString());
                                            insertinv.WareHouseId = int.Parse(ss.WarehouseId.ToString());
                                            insertinv.OpStock = 0;

                                            insertinv.PurQty = 0;
                                            insertinv.PurRetQty = 0;
                                            insertinv.PurExcQty = 0;

                                            insertinv.SalesQty = 0;
                                            insertinv.SalesRetQty = 0;
                                            insertinv.SalesExcQty = 0;

                                            insertinv.ChallanQty = 0;
                                            insertinv.EnStock = 0;
                                            insertinv.CurrentStock = 0;

                                            insertinv.GoodsReceiveQty = 0;
                                            insertinv.IssueQty = 0;
                                            insertinv.GoodsRcvRtnQty = 0;
                                            insertinv.IssueRtnQty = 0;

                                            insertinv.comid = comid;
                                            insertinv.userid = userid;
                                            insertinv.useridUpdate = userid;
                                            insertinv.DateAdded = DateTime.Now;
                                            insertinv.DateUpdated = DateTime.Now;
                                            insertinv.OpeningStockDate = ss.GoodsReceiveMain.GRRDate;
                                            insertinv.Remarks = "Auto From GRR Post";
                                            _context.Entry(insertinv).State = EntityState.Added;
                                            _context.SaveChanges();

                                            inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                                            inv.GoodsReceiveQty = inv.GoodsReceiveQty + ss.Quality;
                                            //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                            inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                            _context.Entry(inv).State = EntityState.Modified;
                                        }
                                        _context.SaveChanges();
                                    }



                                }
                                else
                                {
                                    foreach (GoodsReceiveSub ss in singlevoucher.GoodsReceiveSub)
                                    {
                                        foreach (GoodsReceiveSubWarehouse wh in ss.GoodsReceiveSubWarehouse)
                                        {
                                            Inventory inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == wh.WarehouseId).FirstOrDefault();
                                            if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                                            {

                                                inv.GoodsReceiveQty = inv.GoodsReceiveQty + wh.GRRQty;
                                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                                _context.Entry(inv).State = EntityState.Modified;
                                            }
                                            else
                                            {


                                                Inventory insertinv = new Inventory();

                                                insertinv.ProductId = int.Parse(wh.ProductId.ToString());
                                                insertinv.WareHouseId = int.Parse(wh.WarehouseId.ToString());
                                                insertinv.OpStock = 0;

                                                insertinv.PurQty = 0;
                                                insertinv.PurRetQty = 0;
                                                insertinv.PurExcQty = 0;

                                                insertinv.SalesQty = 0;
                                                insertinv.SalesRetQty = 0;
                                                insertinv.SalesExcQty = 0;

                                                insertinv.ChallanQty = 0;
                                                insertinv.EnStock = 0;
                                                insertinv.CurrentStock = 0;

                                                insertinv.GoodsReceiveQty = 0;
                                                insertinv.IssueQty = 0;
                                                insertinv.GoodsRcvRtnQty = 0;
                                                insertinv.IssueRtnQty = 0;

                                                insertinv.comid = comid;
                                                insertinv.userid = userid;
                                                insertinv.useridUpdate = userid;
                                                insertinv.DateAdded = DateTime.Now;
                                                insertinv.DateUpdated = DateTime.Now;
                                                insertinv.OpeningStockDate = ss.GoodsReceiveMain.GRRDate;
                                                insertinv.Remarks = "Auto From GRR Post";
                                                _context.Entry(insertinv).State = EntityState.Added;
                                                _context.SaveChanges();

                                                inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == wh.WarehouseId).FirstOrDefault();

                                                inv.GoodsReceiveQty = inv.GoodsReceiveQty + wh.GRRQty;
                                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                                _context.Entry(inv).State = EntityState.Modified;
                                                _context.SaveChanges();




                                            }
                                        }
                                        //exec CustBill_Corporate._contexto.[prcAvgCostingProcess] 'grr' , 'grrid' , comid
                                        ///@Type varchar(30), @Id bigint , @ComId varchar(200) --, @ProductId int

                                        //exec CustBill_Corporate._contexto.[prcAvgCostingProcess] 'grr' , 'grrid' , comid
                                    }
                                }




                                var query = $"Exec prcAvgCostingProcess {"GoodsReceive"},{singlevoucher.GRRMainId},'{singlevoucher.ComId}'";

                                SqlParameter[] sqlParameter = new SqlParameter[4];
                                sqlParameter[0] = new SqlParameter("@Type", "GoodsReceive");
                                sqlParameter[1] = new SqlParameter("@Id", singlevoucher.GRRMainId);
                                sqlParameter[2] = new SqlParameter("@comId", singlevoucher.ComId);
                                sqlParameter[3] = new SqlParameter("@userid", userid);


                                Helper.ExecProc("prcAvgCostingProcess", sqlParameter);

                                //var a = _context.Database.ExecuteSqlCommand("Exec prcAvgCostingProcess @Type ,@Id, @ComId ", new SqlParameter("@Type", "GoodsReceive"), new SqlParameter("@Id", singlevoucher.GRRMainId), new SqlParameter("@ComId", singlevoucher.ComId));


                                //statusupdate.Status = 1;
                                //_context.Entry(statusupdate).State = EntityState.Modified;
                            }
                            else if (doctypesingle == "Leave")
                            {
                                var lvAvail = _context.HR_Leave_Avail.Where(x => x.LvId == int.Parse(docidsingle)).FirstOrDefault();
                                lvAvail.Status = 1;
                                _context.Entry(lvAvail).State = EntityState.Modified;
                            }
                            _context.SaveChanges();
                        }
                    }
                }
                else if (criteria.ToUpper().ToString() == "UnPost".ToUpper())
                {
                    if (docid.Count() > 0)
                    {
                        for (var i = 0; i < docid.Count(); i++)
                        {
                            string docidsingle = docid[i];
                            string doctypesingle = doctype[i];


                            if (doctypesingle == "SRR")
                            {
                                var singlevoucher = _context.StoreRequisitionMain.Where(x => x.StoreReqId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 0;
                                _context.Entry(singlevoucher).State = EntityState.Modified;

                            }
                            else if (doctypesingle == "ISSUE")
                            {
                                using (var tr = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var singlevoucher = _context.IssueMain.Include(x => x.IssueSub).ThenInclude(x => x.IssueSubWarehouse).Where(x => x.IssueMainId == int.Parse(docidsingle)).FirstOrDefault();
                                        singlevoucher.Status = 0;
                                        _context.Entry(singlevoucher).State = EntityState.Modified;

                                        if (singlevoucher.IsDirectIssue)
                                        {
                                            foreach (var ss in singlevoucher.IssueSub.OrderByDescending(i => i.IssueSubId))
                                            {

                                                //Inventory inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                                                //if (inv != null)
                                                //{
                                                //    inv.IssueQty = inv.IssueQty - ss.IssueQty;
                                                //    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                //    _context.Entry(inv).State = EntityState.Modified;
                                                //    _context.SaveChanges();
                                                //}
                                                ///// we need to throw or run some process to calculate the inventory qty properly // by fahad // by himu 7 mar 21
                                                ///


                                                CostCalculated costcalculated_context = _context.CostCalculated.Where(x => x.comid == singlevoucher.ComId && x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.IssueMainId == singlevoucher.IssueMainId && x.isDelete == false).FirstOrDefault();



                                                if (costcalculated_context != null)
                                                {

                                                    ///////Himu coding for checking the greater number of document which contains same product
                                                    CostCalculated nextTranCheck =
                                                        _context.CostCalculated
                                                        .Include(x => x.vIssueMain)
                                                        .Include(x => x.vGoodsReceiveMain)
                                                        .Include(x => x.vStoreRequsitionMain)
                                                        .Where(x => x.comid == singlevoucher.ComId && x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.isDelete == false && x.IssueMainId != int.Parse(docidsingle) && x.CostCalculatedId > costcalculated_context.CostCalculatedId).OrderByDescending(i => i.CostCalculatedId).FirstOrDefault();


                                                    var message = "Please Unpost Last Document. Other wise you can not post the document";
                                                    if (nextTranCheck != null)
                                                    {
                                                        if (nextTranCheck.vIssueMain != null)
                                                        {
                                                            message = message + " Issue No : " + nextTranCheck.vIssueMain.IssueNo.ToString();
                                                        }
                                                        else if (nextTranCheck.vGoodsReceiveMain != null)
                                                        {
                                                            message = message + " GRR / MR No : " + nextTranCheck.vGoodsReceiveMain.GRRNo.ToString();
                                                        }
                                                        else if (nextTranCheck.vStoreRequsitionMain != null)
                                                        {
                                                            message = message + " Store Req. No : " + nextTranCheck.vStoreRequsitionMain.SRNo.ToString();
                                                        }
                                                        else
                                                        {
                                                            message = " Nothing Found";
                                                        }



                                                        return Json(new { Success = "3", ex = message });
                                                    }

                                                    costcalculated_context.isDelete = true;
                                                    _context.Entry(costcalculated_context).State = EntityState.Modified;
                                                    _context.SaveChanges();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            foreach (var ss in singlevoucher.IssueSub.OrderByDescending(c => c.IssueSubId))
                                            {
                                                foreach (IssueSubWarehouse wh in ss.IssueSubWarehouse)
                                                {
                                                    //Inventory inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == wh.WarehouseId).FirstOrDefault();

                                                    //if (inv != null)
                                                    //{
                                                    //    inv.IssueQty = inv.IssueQty - wh.IssueQty;
                                                    //    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                    //    _context.Entry(inv).State = EntityState.Modified;
                                                    //    _context.SaveChanges();
                                                    //}
                                                    /////himu  coding for auto grr unpost and 
                                                    ///


                                                    var strReq = _context.StoreRequisitionMain.Where(x => x.StoreReqId == singlevoucher.StoreReqId).FirstOrDefault();
                                                    if (strReq != null && strReq.SubWarehouseId != null)
                                                    {
                                                        CostCalculated costcalcualtedStorereq = _context.CostCalculated.Where(x => x.comid == singlevoucher.ComId && x.ProductId == wh.ProductId && x.WarehouseId == strReq.SubWarehouseId && x.StoreReqId == strReq.StoreReqId && x.isDelete == false).FirstOrDefault();
                                                        if (costcalcualtedStorereq != null)
                                                        {
                                                            ///////Himu coding for checking the greater number of document which contains same product
                                                            CostCalculated nextTranCheck =
                                                                _context.CostCalculated
                                                                .Include(x => x.vIssueMain)
                                                                .Include(x => x.vGoodsReceiveMain)
                                                                .Include(x => x.vStoreRequsitionMain)
                                                                .Where(x => x.comid == singlevoucher.ComId && x.ProductId == wh.ProductId && x.WarehouseId == strReq.SubWarehouseId && x.isDelete == false && x.StoreReqId != strReq.StoreReqId && x.CostCalculatedId > costcalcualtedStorereq.CostCalculatedId).OrderByDescending(i => i.CostCalculatedId).FirstOrDefault();

                                                            var message = "Please Unpost Last Document. Other wise you can not post the document";
                                                            if (nextTranCheck != null)
                                                            {
                                                                if (nextTranCheck.vIssueMain != null)
                                                                    message = message + " Issue No : " + nextTranCheck.vIssueMain.IssueNo.ToString();
                                                                else if (nextTranCheck.vGoodsReceiveMain != null)
                                                                    message = message + " GRR / MR No : " + nextTranCheck.vGoodsReceiveMain.GRRNo.ToString();
                                                                else if (nextTranCheck.vStoreRequsitionMain != null)
                                                                    message = message + " Store Req. No : " + nextTranCheck.vStoreRequsitionMain.SRNo.ToString();
                                                                else
                                                                    message = " Nothing Found";

                                                                return Json(new { Success = "3", ex = message });
                                                            }
                                                            costcalcualtedStorereq.isDelete = true;
                                                            _context.Entry(costcalcualtedStorereq).State = EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }
                                                    }



                                                    /////
                                                    CostCalculated costcalculated_context = _context.CostCalculated.Where(x => x.comid == singlevoucher.ComId && x.ProductId == wh.ProductId && x.WarehouseId == wh.WarehouseId && x.IssueMainId == singlevoucher.IssueMainId && x.isDelete == false).FirstOrDefault();
                                                    if (costcalculated_context != null)
                                                    {
                                                        ///////Himu coding for checking the greater number of document which contains same product
                                                        CostCalculated nextTranCheck =
                                                            _context.CostCalculated
                                                            .Include(x => x.vIssueMain)
                                                            .Include(x => x.vGoodsReceiveMain)
                                                            .Include(x => x.vStoreRequsitionMain)
                                                            .Where(x => x.comid == singlevoucher.ComId && x.ProductId == wh.ProductId && x.WarehouseId == wh.WarehouseId && x.isDelete == false && x.IssueMainId != singlevoucher.IssueMainId && x.CostCalculatedId > costcalculated_context.CostCalculatedId).OrderByDescending(i => i.CostCalculatedId).FirstOrDefault();

                                                        var message = "Please Unpost Last Document. Other wise you can not post the document";
                                                        if (nextTranCheck != null)
                                                        {
                                                            if (nextTranCheck.vIssueMain != null)
                                                                message = message + " Issue No : " + nextTranCheck.vIssueMain.IssueNo.ToString();
                                                            else if (nextTranCheck.vGoodsReceiveMain != null)
                                                                message = message + " GRR / MR No : " + nextTranCheck.vGoodsReceiveMain.GRRNo.ToString();
                                                            else if (nextTranCheck.vStoreRequsitionMain != null)
                                                                message = message + " Store Req. No : " + nextTranCheck.vStoreRequsitionMain.SRNo.ToString();
                                                            else
                                                                message = " Nothing Found";

                                                            return Json(new { Success = "3", ex = message });
                                                        }
                                                        costcalculated_context.isDelete = true;
                                                        _context.Entry(costcalculated_context).State = EntityState.Modified;
                                                        _context.SaveChanges();
                                                    }

                                                }
                                            }
                                        }


                                        var singlestorerequisition = _context.StoreRequisitionMain.Where(x => x.StoreReqId == singlevoucher.StoreReqId).FirstOrDefault();
                                        if (singlestorerequisition != null)
                                        {///need to check those code today 19-sep-2020
                                            if (singlestorerequisition.IsSubStore == true)
                                            {
                                                foreach (var ss in singlevoucher.IssueSub)
                                                {
                                                    ////foreach (IssueSubWarehouse wh in ss.IssueSubWarehouse)
                                                    ////{
                                                    //Inventory inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == singlestorerequisition.SubWarehouseId).FirstOrDefault();

                                                    //if (inv != null)
                                                    //{
                                                    //    inv.GoodsReceiveQty = inv.GoodsReceiveQty - ss.IssueQty;
                                                    //    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                    //    _context.Entry(inv).State = EntityState.Modified;
                                                    //    _context.SaveChanges();
                                                    //}



                                                    CostCalculated costcalculated_context = _context.CostCalculated.Where(x => x.comid == singlestorerequisition.ComId && x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.StoreReqId == singlestorerequisition.StoreReqId && x.isDelete == false).FirstOrDefault();
                                                    if (costcalculated_context != null)
                                                    {
                                                        ///////Himu coding for checking the greater number of document which contains same product
                                                        CostCalculated nextTranCheck =
                                                            _context.CostCalculated
                                                            .Include(x => x.vIssueMain)
                                                            .Include(x => x.vGoodsReceiveMain)
                                                            .Include(x => x.vStoreRequsitionMain)
                                                            .Where(x => x.comid == singlestorerequisition.ComId && x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.isDelete == false && x.StoreReqId != singlestorerequisition.StoreReqId && x.CostCalculatedId > costcalculated_context.CostCalculatedId).OrderByDescending(i => i.CostCalculatedId).FirstOrDefault();

                                                        var message = "Please Unpost Last Document. Other wise you can not post the document";
                                                        if (nextTranCheck != null)
                                                        {
                                                            if (nextTranCheck.vIssueMain != null)
                                                                message = message + " Issue No : " + nextTranCheck.vIssueMain.IssueNo.ToString();
                                                            else if (nextTranCheck.vGoodsReceiveMain != null)
                                                                message = message + " GRR / MR No : " + nextTranCheck.vGoodsReceiveMain.GRRNo.ToString();
                                                            else if (nextTranCheck.vStoreRequsitionMain != null)
                                                                message = message + " Store Req. No : " + nextTranCheck.vStoreRequsitionMain.SRNo.ToString();
                                                            else
                                                                message = " Nothing Found";

                                                            return Json(new { Success = "3", ex = message });
                                                        }
                                                        costcalculated_context.isDelete = true;
                                                        _context.Entry(costcalculated_context).State = EntityState.Modified;
                                                    }
                                                    _context.SaveChanges();


                                                    //}
                                                }
                                            }
                                        }
                                    }
                                    catch (SqlException ex)
                                    {

                                        Console.WriteLine(ex.Message);
                                        tr.Rollback();

                                        return Json(new { Success = "3", ex = "Something Wrong" });

                                    }

                                    tr.Commit();
                                }


                            }
                            else if (doctypesingle == "PR")
                            {
                                var singlevoucher = _context.PurchaseRequisitionMain.Where(x => x.PurReqId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 0;
                                _context.Entry(singlevoucher).State = EntityState.Modified;


                            }
                            else if (doctypesingle == "PO")
                            {
                                var singlevoucher = _context.PurchaseOrderMain.Where(x => x.PurOrderMainId == int.Parse(docidsingle)).FirstOrDefault();
                                singlevoucher.Status = 0;
                                _context.Entry(singlevoucher).State = EntityState.Modified;


                            }
                            else if (doctypesingle == "GRR")
                            {
                                using (var tr = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var singlevoucher = _context.GoodsReceiveMain.Include(x => x.GoodsReceiveSub).ThenInclude(x => x.GoodsReceiveSubWarehouse).Where(x => x.GRRMainId == int.Parse(docidsingle)).FirstOrDefault();
                                        singlevoucher.Status = 0;
                                        _context.Entry(singlevoucher).State = EntityState.Modified;


                                        if (singlevoucher.IsDirectGRR)
                                        {

                                            foreach (var ss in singlevoucher.GoodsReceiveSub)
                                            {

                                                //Inventory inv = _context.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                                                //if (inv != null)
                                                //{
                                                //    inv.GoodsReceiveQty = inv.GoodsReceiveQty - ss.Quality;
                                                //    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                //    _context.Entry(inv).State = EntityState.Modified;
                                                //}
                                                //_context.SaveChanges();



                                                CostCalculated costcalculated_context = _context.CostCalculated.Where(x => x.comid == singlevoucher.ComId && x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.GRRMainId == singlevoucher.GRRMainId && x.isDelete == false).FirstOrDefault();
                                                if (costcalculated_context != null)
                                                {
                                                    ///////Himu coding for checking the greater number of document which contains same product
                                                    CostCalculated nextTranCheck =
                                                        _context.CostCalculated
                                                        .Include(x => x.vIssueMain)
                                                        .Include(x => x.vGoodsReceiveMain)
                                                        .Include(x => x.vStoreRequsitionMain)
                                                        .Where(x => x.comid == singlevoucher.ComId && x.ProductId == ss.ProductId && x.WarehouseId == ss.WarehouseId && x.isDelete == false && x.GRRMainId != singlevoucher.GRRMainId && x.CostCalculatedId > costcalculated_context.CostCalculatedId).OrderByDescending(i => i.CostCalculatedId).FirstOrDefault();

                                                    var message = "Please Unpost Last Document. Other wise you can not post the document";
                                                    if (nextTranCheck != null)
                                                    {
                                                        if (nextTranCheck.vIssueMain != null)
                                                            message = message + " Issue No : " + nextTranCheck.vIssueMain.IssueNo.ToString();
                                                        else if (nextTranCheck.vGoodsReceiveMain != null)
                                                            message = message + " GRR / MR No : " + nextTranCheck.vGoodsReceiveMain.GRRNo.ToString();
                                                        else if (nextTranCheck.vStoreRequsitionMain != null)
                                                            message = message + " Store Req. No : " + nextTranCheck.vStoreRequsitionMain.SRNo.ToString();
                                                        else
                                                            message = " Nothing Found";

                                                        return Json(new { Success = "3", ex = message });
                                                    }
                                                    costcalculated_context.isDelete = true;
                                                    _context.Entry(costcalculated_context).State = EntityState.Modified;
                                                }
                                                _context.SaveChanges();


                                            }
                                        }
                                        else
                                        {


                                            foreach (var ss in singlevoucher.GoodsReceiveSub)
                                            {
                                                foreach (GoodsReceiveSubWarehouse wh in ss.GoodsReceiveSubWarehouse)
                                                {
                                                    //Inventory inv = _context.Inventory.Where(x => x.ProductId == wh.ProductId && x.WareHouseId == wh.WarehouseId).FirstOrDefault();

                                                    //if (inv != null)
                                                    //{
                                                    //    inv.GoodsReceiveQty = inv.GoodsReceiveQty - wh.GRRQty;
                                                    //    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                                    //    _context.Entry(inv).State = EntityState.Modified;
                                                    //}

                                                    //_context.SaveChanges();

                                                    CostCalculated costcalculated_context = _context.CostCalculated.Where(x => x.comid == singlevoucher.ComId && x.ProductId == wh.ProductId && x.WarehouseId == wh.WarehouseId && x.GRRMainId == singlevoucher.GRRMainId && x.isDelete == false).FirstOrDefault();
                                                    if (costcalculated_context != null)
                                                    {
                                                        ///////Himu coding for checking the greater number of document which contains same product
                                                        CostCalculated nextTranCheck =
                                                            _context.CostCalculated
                                                            .Include(x => x.vIssueMain)
                                                            .Include(x => x.vGoodsReceiveMain)
                                                            .Include(x => x.vStoreRequsitionMain)
                                                            .Where(x => x.comid == singlevoucher.ComId && x.ProductId == wh.ProductId && x.WarehouseId == wh.WarehouseId && x.isDelete == false && x.GRRMainId != singlevoucher.GRRMainId && x.CostCalculatedId > costcalculated_context.CostCalculatedId).OrderByDescending(i => i.CostCalculatedId).FirstOrDefault();

                                                        var message = "Please Unpost Last Document. Other wise you can not post the document";
                                                        if (nextTranCheck != null)
                                                        {
                                                            if (nextTranCheck.vIssueMain != null)
                                                                message = message + " Issue No : " + nextTranCheck.vIssueMain.IssueNo.ToString();
                                                            else if (nextTranCheck.vGoodsReceiveMain != null)
                                                                message = message + " GRR / MR No : " + nextTranCheck.vGoodsReceiveMain.GRRNo.ToString();
                                                            else if (nextTranCheck.vStoreRequsitionMain != null)
                                                                message = message + " Store Req. No : " + nextTranCheck.vStoreRequsitionMain.SRNo.ToString();
                                                            else
                                                                message = " Nothing Found";

                                                            return Json(new { Success = "3", ex = message });
                                                        }
                                                        costcalculated_context.isDelete = true;
                                                        _context.Entry(costcalculated_context).State = EntityState.Modified;
                                                    }
                                                    _context.SaveChanges();

                                                }
                                            }

                                        }
                                    }
                                    catch (SqlException ex)
                                    {

                                        Console.WriteLine(ex.Message);
                                        tr.Rollback();

                                        return Json(new { Success = "3", ex = "Something Wrong" });

                                    }

                                    tr.Commit();
                                }




                            }
                            else if (doctypesingle == "Leave")
                            {
                                var lvAvail = _context.HR_Leave_Avail.Where(x => x.LvId == int.Parse(docidsingle)).FirstOrDefault();
                                lvAvail.Status = 2;
                                _context.Entry(lvAvail).State = EntityState.Modified;
                            }
                            _context.SaveChanges();


                        }
                    }

                }


                //}
                //catch (SqlException ex)
                //{

                //    Console.WriteLine(ex.Message);
                //    tr.Rollback();
                //    return Json(new { Success = 0, ex = ex });

                //}
                //tr.Commit();

                return Json(new { Success = "1", ex = "Data Post/Unpost Successfully" });

                //}
            }
            catch (Exception ex)
            {
                return Json(new { Success = "3", ex = ex.Message });
                throw ex;

            }


        }

        public string prcSaveData(Acc_VoucherMain model)
        {
            ArrayList arQuery = new ArrayList();

            try
            {
                var sqlQuery = "";
                // Count total Debit & Credit
                //foreach (var item in model.Collection)
                //{
                //    if (item.IsCheck == true)
                //    {
                //        sqlQuery = " Update tblAcc_Voucher_Main Set IsPosted = 1 ,LuserIdCheck = " + Session["Luserid"].ToString() + "   Where ComId = " + HttpContext.Session.GetString("comid").ToString() + " And docid = " + (item.docid) + "";
                //        arQuery.Add(sqlQuery);
                //    }
                //}
                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
                return "Data Posted Successfuly";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            finally
            {
                //clsCon = null;
            }
        }
        #endregion
    }
}
