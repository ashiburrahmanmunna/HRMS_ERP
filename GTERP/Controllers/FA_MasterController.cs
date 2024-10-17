#region Using Directive
using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace GTERP.Controllers
{

    [OverridableAuthorize]
    public class FA_MasterController : Controller
    {
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; }

        public FA_MasterController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            //Repository = rep;
        }

        // GET: FA_Master
        public async Task<IActionResult> Index()
        {
            var comid = HttpContext.Session.GetString("comid");


            var gTRDBContext = _context.fA_Masters
            .Include(f => f.DepreciationFrequency)
            .Include(f => f.DepreciationMethod)
            .Include(f => f.Product)
            .Include(f => f.DepreciationHead)
            .Include(f => f.AccumulatedDepreciationHead)
            .Where(x => x.ComId == comid);
            return Json(new { list = await gTRDBContext.ToListAsync() });

        }

        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Products.Include(y => y.vPrimaryCategory);
                var query = from e in _context.fA_Masters.Where(x => x.ComId == comid).OrderByDescending(x => x.FA_MasterId)

                            select new FAMasterListView
                            {
                                FA_MasterId = e.FA_MasterId.ToString(),
                                ProductCode = e.Product != null ? e.Product.ProductCode : "",
                                ProductName = e.Product != null ? e.Product.ProductName : "",
                                Percentage = e.Percentage.ToString(),
                                AssetCode = e.AssetCode,
                                DepreciationCode = e.DepreciationHead != null ? e.DepreciationHead.AccCode : "=N/A=",
                                AccDepreciationAccountName = e.AccumulatedDepreciationHead != null ? e.AccumulatedDepreciationHead.AccName : "=N/A=",
                                AccDepreciationCode = e.AccumulatedDepreciationHead != null ? e.AccumulatedDepreciationHead.AccCode : "=N/A=",
                                ComId = e.ComId,
                                Title = e.DepreciationFrequency != null ? e.DepreciationFrequency.Title : "",
                                DMName = e.DepreciationMethod != null ? e.DepreciationMethod.DMName : "",
                                IsInProcess = e.IsInProcess,
                            };



                var parser = new Parser<FAMasterListView>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public class FAMasterListView
        {
            public string FA_MasterId { get; set; }
            public string Percentage { get; set; }
            public string ProductCode { get; set; }
            public string AssetCode { get; set; }
            public string DepreciationCode { get; set; }
            public string AccDepreciationAccountName { get; set; }
            public string AccDepreciationCode { get; set; }
            public string ProductName { get; set; }
            public string ComId { get; set; }
            public string Title { get; set; }
            public string DMName { get; set; }
            public bool IsInProcess { get; set; }
        }


        public async Task<IActionResult> FAMasterList()
        {

            return View();
        }
        public async Task<IActionResult> FADetailCreate()
        {
            var comid = HttpContext.Session.GetString("comid");

            var pendingAssets = from m in _context.fA_Masters
                                join p in _context.Products
                                on m.ProductId equals p.ProductId
                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId });


            ViewData["Title"] = "Create";
            ViewData["FoD"] = new SelectList(_context.depreciationFrequencies, "FoDId", "Title");
            ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
            ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                            join p in _context.Products
                                            on m.ProductId equals p.ProductId
                                            select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text");
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())).Select(x => new { Text = x.ProductCode + " " + x.ProductName, Value = x.ProductId }), "Value", "Text");

            //ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
            ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text");

            ViewData["Departments"] = new SelectList(_context.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptName, x.DeptId }), "DeptId", "DeptName");
            //ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false), "FA_DetailsId", "AssetItem");
            //ViewData["DepreciationExpenseAccountId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            //ViewData["AccumulateDepreciationAccountId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

            ViewData["gTRDBContext"] = _context.fA_Masters
            .Include(f => f.DepreciationFrequency)
            .Include(f => f.DepreciationMethod)
            .Include(f => f.Product)
            .Include(f => f.DepreciationHead)
            .Include(f => f.AccumulatedDepreciationHead)
            .Where(x => x.ComId == comid);

            return View();
        }
        public async Task<IActionResult> FASalesCreate()
        {
            ViewData["Title"] = "Create";
            var comid = HttpContext.Session.GetString("comid");
            ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                            join p in _context.Products
                                            on m.ProductId equals p.ProductId
                                            select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text");
            ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false), "FA_DetailsId", "AssetItem");
            return View();
        }
        public async Task<IActionResult> FASalesList()
        {

            return View();
        }
        public async Task<IActionResult> FADetailList()
        {

            return View();
        }
        public async Task<IActionResult> getsolditem(int id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var gTRDBContext = _context.FA_Details.Include(f => f.FA_Master)
                                                    .Where(x => x.FA_MasterId == id)
                                                    .Select(x => new { Text = x.AssetItem, Value = x.FA_DetailsId })
                                                    .ToList();
            return Json(new { list = gTRDBContext });
        }
        public ActionResult GetAssetItems(int id)
        {
            var comid = HttpContext.Session.GetString("comid");
            //var gTRDBContext = _context.fA_Masters.Include(f => f.DepreciationFrequency).Include(f => f.DepreciationMethod).Include(f => f.Product).Where(x => x.ComId == comid);

            var gTRDBContext = _context.FA_Details
                         .Include(x => x.Emp_Info)
                         .Include(x => x.Cat_Section)
                         .Include(x => x.FA_Master).ThenInclude(f => f.DepreciationFrequency)
                         .Include(x => x.FA_Master).ThenInclude(f => f.DepreciationMethod)
                         .Include(x => x.FA_Master).ThenInclude(f => f.Product).Where(x => x.ComId == comid)
                         .Include(x => x.DepreciationSchedules)//.Where(b=>b.JournalEntry == true).FirstOrDefault())
                         .Select(p => new
                         {
                             Text = p.AssetItem + " " + p.FA_Master.Product.ProductName,
                             //p.AssetItem,
                             p.PurchaseDate,
                             p.PurchaseValue,
                             p.IssueDate,
                             EmpName = p.Cat_Section.SectName,
                             p.UsefullLife,
                             p.RemainingYear,
                             p.FA_Dep_Status.Title,
                             p.FA_Dep_StatusId,
                             p.IsInActive,
                             p.EVAULife,
                             p.FA_DetailsId,
                             p.IsDepRunning,
                             JournalEntry = p.DepreciationSchedules.FirstOrDefault() == null ? false : p.DepreciationSchedules.FirstOrDefault().JournalEntry
                         }).ToList(); //.Where(p => p.FA_DetailsId. == id)

            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            return Json(gTRDBContext);
        }

        public async Task<IActionResult> GetSoldAssetList()
        {

            var comid = HttpContext.Session.GetString("comid");
            //var gTRDBContext = _context.fA_Masters.Include(f => f.DepreciationFrequency).Include(f => f.DepreciationMethod).Include(f => f.Product).Where(x => x.ComId == comid);

            var gTRDBContext = _context.fA_Sells.Where(x => x.FA_Master.ComId == comid)
                .Include(x => x.FA_Details).ThenInclude(f => f.FA_Master)
                .Include(x => x.FA_Master).ThenInclude(f => f.DepreciationMethod)
                .Include(x => x.FA_Master).ThenInclude(f => f.Product)
                .Select(p => new
                {
                    p.SellsPrice,
                    p.Description,
                    p.FA_Details.AssetItem,
                    p.FA_Master.AssetCode,
                    p.IsDepRunning,
                    p.FA_Dep_StatusId,
                    p.FA_SellId

                }).ToList(); //.Where(p => p.FA_DetailsId. == id)

            //var gTRDBContexttt = _context.fA_Sells
            //            .Include(x => x.FA_Details)                       
            //            .Include(x => x.FA_Master).ThenInclude(f => f.DepreciationFrequency)
            //            .Include(x => x.FA_Master).ThenInclude(f => f.DepreciationMethod)
            //            .Include(x => x.FA_Master).ThenInclude(f => f.Product).Where(x => x.comid == comid)
            //            .Include(x => x.DepreciationSchedules)//.Where(b=>b.JournalEntry == true).FirstOrDefault())
            //            .Select(p => new
            //            {
            //                Text = p.AssetItem + " " + p.FA_Master.Product.ProductName,
            //                 //p.AssetItem,
            //                 p.PurchaseDate,
            //                p.PurchaseValue,
            //                p.IssueDate,
            //                EmpName = p.Cat_Section.SectName,
            //                p.UsefullLife,
            //                p.RemainingYear,
            //                p.FA_Dep_Status.Title,
            //                p.FA_Dep_StatusId,
            //                p.IsInActive,
            //                p.EVAULife,
            //                p.FA_DetailsId,
            //                p.IsDepRunning,
            //                JournalEntry = p.DepreciationSchedules.FirstOrDefault() == null ? false : p.DepreciationSchedules.FirstOrDefault().JournalEntry
            //            }).ToList(); //.Where(p => p.FA_DetailsId. == id)



            return Json(gTRDBContext);



            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            //var gTRDBContext = _context.fA_Sells.Include(f => f.FA_Master).ThenInclude(x=>x.FA_Details);
            //return Json(new { list = await gTRDBContext.ToListAsync() });
        }

        // GET: FA_Master/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fA_Master = await _context.fA_Masters
                .Include(f => f.DepreciationFrequency)
                .Include(f => f.DepreciationMethod)
                .Include(f => f.Product)
                .FirstOrDefaultAsync(m => m.FA_MasterId == id);
            if (fA_Master == null)
            {
                return NotFound();
            }

            return View(fA_Master);
        }
        public JsonResult getOnChangeProduct(int consumptionid)
        {
            var comid = HttpContext.Session.GetString("comid");
            var result = from product in _context.Products
                         join acc in _context.Acc_ChartOfAccounts
                         on product.AccIdConsumption equals acc.AccId
                         where product.ProductId == consumptionid
                         select new
                         {
                             product.ProductName,
                             product.ProductCode,
                             acc.AccCode,
                             acc.AccName
                         };


            return Json(new { list = result });
        }
        public JsonResult getOnChangeFA_MasterIdp(int id)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (id == -1)
            {
                return Json(new { list = "no" });
            }
            var result = from asset in _context.fA_Masters
                             //join acc in _context.Acc_ChartOfAccounts
                             //on product.AccIdConsumption equals acc.AccId
                         where asset.FA_MasterId == id
                         select new
                         {
                             asset.Product.ProductName,
                             asset.AssetCode,
                             asset.DepreciationMethod.DMName,
                             asset.DepreciationFrequency.Title,
                             asset.Percentage,
                             year = (100 / (asset.Percentage > 0 ? asset.Percentage : 1)),
                             month = (100 * 12 / (asset.Percentage > 0 ? asset.Percentage : 1)),
                             lastassetcode = (asset.FA_Details == null ? "=N/A=" : asset.FA_Details.OrderByDescending(x => x.FA_DetailsId).FirstOrDefault().AssetItem)
                             //"Year : " + (100 / (asset.Percentage > 0 ? asset.Percentage : 1 )) + "|| Month : " + (100*12 / (asset.Percentage > 0 ? asset.Percentage : 1)) //// simple single line if condtion check 

                         };


            return Json(new { list = result });
        }
        private void GenerateSchedule(out DateTime nextdepdate, out DateTime curentDepDate, FA_Details asset)
        {
            DateTime FirstDepDate;

            if (asset.FA_Master.DMId == 1)
            {
                var d = asset.FA_Master.DepreciationFrequency.DSADay;
                FirstDepDate = asset.IssueDate.AddDays(d);
                curentDepDate = FirstDepDate.AddDays(d);
                nextdepdate = curentDepDate.AddDays(d);
            }
            else
            {
                var d = asset.FA_Master.DepreciationFrequency.DSAMonth;
                FirstDepDate = asset.IssueDate.AddMonths(d);
                curentDepDate = FirstDepDate.AddMonths(d);
                nextdepdate = curentDepDate.AddMonths(d);
            }
            var model = new DepreciationSchedule();
            model.ScheduleDate = FirstDepDate;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProcess(List<FA_ProcessRecord> assetListToProcess)
        {
            if (assetListToProcess.Count > 0)
            {

                foreach (var e in assetListToProcess)
                {
                    var Entries = _context.FA_ProcessRecords.Where(x => x.FA_DetailsId == e.FA_DetailsId).ToList();

                    if (Entries.Count > 0)
                    {
                        var LastEntry = Entries.OrderByDescending(x => x.DEntryDate).Select(x => new { x.DEntryDate, x.FA_DetailsId, x.FA_Details }).First();

                        if (LastEntry != null && LastEntry.DEntryDate != e.DEntryDate && LastEntry.FA_DetailsId != e.FA_DetailsId)
                        {

                            e.DEntryDate = LastEntry.DEntryDate.AddMonths(1);
                            e.NextDepDate = e.DEntryDate.AddMonths(1);
                            _context.FA_ProcessRecords.Add(e);


                        }
                    }

                    else
                    {
                        e.DEntryDate = e.DSDate;
                        e.NextDepDate = e.DEntryDate.AddMonths(1);
                        var d = _context.FA_Details.Find(e.FA_DetailsId);
                        d.FA_Dep_Status.Title = "Depreciation is continuing";
                        d.IsDepRunning = true;
                        _context.Entry(d).State = EntityState.Modified;
                        _context.FA_ProcessRecords.Add(e);

                    }


                }
                _context.SaveChanges();

                return Json(new { result = 1 });
            }
            return Json(new { result = 0 });
        }

        // GET: FA_Master/Create
        public IActionResult Create()
        {
            var comid = HttpContext.Session.GetString("comid");

            var pendingAssets = from m in _context.fA_Masters
                                join p in _context.Products
                                on m.ProductId equals p.ProductId
                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId });


            ViewData["Title"] = "Create";
            ViewData["FoD"] = new SelectList(_context.depreciationFrequencies, "FoDId", "Title");
            ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
            ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                            join p in _context.Products
                                            on m.ProductId equals p.ProductId
                                            select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text");
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())).Select(x => new { Text = x.ProductCode + " " + x.ProductName, Value = x.ProductId }), "Value", "Text");
            //ViewData["ProductId"] = new SelectList(from product in _context.Products join grr in _context.GoodsReceiveSub
            //                                        on product.ProductId equals grr.ProductId
            //                                       where product.CategoryId == 21
            //                                       select (new { Text = product.AssetCode + " " + product.ProductName, Value = product.ProductId }), "Value", "Text");
            ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
            ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text");

            ViewData["Departments"] = new SelectList(_context.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptName, x.DeptId }), "DeptId", "DeptName");
            //ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false), "FA_DetailsId", "AssetItem");
            ViewData["DepreciationExpenseAccountId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            ViewData["AccumulateDepreciationAccountId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

            ViewData["gTRDBContext"] = _context.fA_Masters
            .Include(f => f.DepreciationFrequency)
            .Include(f => f.DepreciationMethod)
            .Include(f => f.Product)
            .Include(f => f.DepreciationHead)
            .Include(f => f.AccumulatedDepreciationHead)
            .Where(x => x.ComId == comid);

            return View("FAMasterCreate");
        }

        // POST: FA_Master/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<FA_Master> assetlist)
        {

            try
            {
                // if (ModelState.IsValid)
                //{
                foreach (var asset in assetlist)
                {
                    if (asset.FA_MasterId > 0)
                    {
                        asset.DateUpdated = DateTime.Now;
                        asset.UpdateByUserId = HttpContext.Session.GetString("userid");

                        if (asset.IsDelete == true)
                        {
                            _context.Remove(asset).State = EntityState.Deleted;

                        }
                        else
                        {
                            _context.Entry(asset).State = EntityState.Modified;

                        }

                    }
                    else
                    {
                        asset.CreatedByUserId = HttpContext.Session.GetString("userid");
                        asset.DateAdded = DateTime.Now;
                        _context.Add(asset);
                    }
                }
                var comid = HttpContext.Session.GetString("comid");
                ViewData["FoD"] = new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text");
                ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
                ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                                join p in _context.Products
                                                on m.ProductId equals p.ProductId
                                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text");
                //ViewData["ProductId"] = new SelectList(from product in _context.Products
                //                                       join grr in _context.GoodsReceiveSub
                // on product.ProductId equals grr.ProductId
                //                                       where product.CategoryId == 21
                //                                       select (new { Text = product.AssetCode + " " + product.ProductName, Value = product.ProductId }), "Value", "Text");
                ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())), "ProductId", "ProductName");
                ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
                ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text");
                ViewData["Departments"] = new SelectList(_context.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptName, x.DeptId }), "DeptId", "DeptName");
                //ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false), "FA_DetailsId", "AssetItem");
                ViewBag.DepreciationExpenseAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                ViewBag.AccumulateDepreciationAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                _context.SaveChanges();
                return Json(new { success = 1, error = 0, message = "" });
                //}
            }
            catch (Exception e)
            {

                return Json(new { success = 0, error = 1, message = e.Message });

            }
            //return View(fA_Master);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignAsset(List<FA_Details> assetListToAssign)
        {


            try
            {
                var comid = HttpContext.Session.GetString("comid");
                // if (ModelState.IsValid)
                //{
                foreach (var asset in assetListToAssign)
                {
                    DateTime now = asset.IssueDate;
                    //var firstdayofmonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));//feb
                    var firstdayofmonth = new DateTime(now.Year, now.Month, 1);//feb
                    var firstdayofnextmonth = firstdayofmonth.AddMonths(1);//mar
                    //firstdayofnextmonth = new DateTime(firstdayofnextmonth.Year, firstdayofnextmonth.Month, DateTime.DaysInMonth(firstdayofnextmonth.Year, firstdayofnextmonth.Month));
                    ///If the Issue date 
                    if (asset.IssueDate.Day < 10)
                    {

                        asset.DepCalFromDate = firstdayofmonth;
                    }
                    if (asset.IssueDate.Day >= 10)
                    {
                        asset.DepCalFromDate = firstdayofnextmonth;
                    }
                    if (asset.FA_DetailsId > 0)
                    {
                        asset.DateUpdated = DateTime.Now;
                        asset.UpdateByUserId = HttpContext.Session.GetString("userid");
                        if (asset.IsDelete == true)
                        {
                            if (asset.CalculateDepreciation == true)
                            {
                                var l = _context.DepreciationSchedules.Where(x => x.FA_DetailsId == asset.FA_DetailsId).ToList();
                                if (l.Count > 0)
                                {
                                    _context.DepreciationSchedules.RemoveRange(l);
                                    _context.SaveChanges();
                                }

                            }
                            _context.FA_Details.Remove(asset);

                        }
                        else
                        {
                            if (asset.CalculateDepreciation == true)
                            {
                                var l = _context.DepreciationSchedules.Where(x => x.FA_DetailsId == asset.FA_DetailsId).ToList();
                                if (l.Count == 0)
                                {
                                    var query = $"Exec CalculateDepreciation '{comid}',{asset.FA_DetailsId}";
                                    //run procedure to save new depreciation schedule
                                    SqlParameter[] parameters = new SqlParameter[2];
                                    parameters[0] = new SqlParameter("@comid", comid);
                                    parameters[1] = new SqlParameter("@FA_DetailsId", asset.FA_DetailsId);

                                    Helper.ExecProc("[CalculateDepreciation]", parameters);

                                }
                            }
                            if (asset.CalculateDepreciation == false)
                            {
                                var l = _context.DepreciationSchedules.Where(x => x.FA_DetailsId == asset.FA_DetailsId).ToList();
                                //check condition if there is already depreciation schedule but modify with false 
                                if (l.Count > 0)
                                {
                                    _context.DepreciationSchedules.RemoveRange(l);
                                    _context.SaveChanges();
                                }
                                asset.FA_Dep_StatusId = 2;
                                asset.IsDepRunning = false;
                            }
                            _context.Entry(asset).State = EntityState.Modified;


                        }
                    }
                    else
                    {
                        asset.CreatedByUserId = HttpContext.Session.GetString("userid");
                        asset.DateAdded = DateTime.Now;


                        if (asset.CalculateDepreciation == true)
                        {

                            asset.FA_Dep_StatusId = 1;
                            asset.IsDepRunning = true;
                            _context.FA_Details.Add(asset);
                            _context.SaveChanges();

                            var id = asset.FA_DetailsId;
                            var query = $"Exec CalculateDepreciation '{comid}',{id}";
                            //run procedure to save new depreciation schedule
                            SqlParameter[] parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@comid", comid);
                            parameters[1] = new SqlParameter("@FA_DetailsId", id);

                            Helper.ExecProc("[CalculateDepreciation]", parameters);

                        }
                        else
                        {
                            asset.FA_Dep_StatusId = 2;
                            asset.IsDepRunning = false;
                            _context.FA_Details.Add(asset);
                            _context.SaveChanges();
                        }

                    }
                }
                ViewData["FoD"] = new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text");
                ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
                ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                                join p in _context.Products
                                                on m.ProductId equals p.ProductId
                                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text"); ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
                ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text");
                //ViewData["ProductId"] = new SelectList(from product in _context.Products
                //                                       join grr in _context.GoodsReceiveSub
                // on product.ProductId equals grr.ProductId
                //                                       where product.CategoryId == 21
                //                                       select (new { Text = product.AssetCode + " " + product.ProductName, Value = product.ProductId }), "Value", "Text");
                ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())), "ProductId", "ProductName");
                //ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false), "FA_DetailsId", "AssetItem");
                ViewBag.DepreciationExpenseAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                ViewBag.AccumulateDepreciationAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                ViewData["Departments"] = new SelectList(_context.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptName, x.DeptId }), "DeptId", "DeptName");
                _context.SaveChanges();
                return Json(new { success = 1, error = 0, message = "" });
                //}
            }
            catch (Exception e)
            {

                return Json(new { success = 0, error = 1, message = e.Message });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TemAssignAsset(List<Tem_FA_Details> assetListToAssign)
        {
            try
            {
                var list = new List<TemDepSchedule>();
                var comid = HttpContext.Session.GetString("comid");
                // if (ModelState.IsValid)
                //{
                foreach (var asset in assetListToAssign)
                {
                    DateTime now = asset.IssueDate;
                    //var firstdayofmonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));//feb
                    var firstdayofmonth = new DateTime(now.Year, now.Month, 1);//feb
                    var firstdayofnextmonth = firstdayofmonth.AddMonths(1);//mar
                    //firstdayofnextmonth = new DateTime(firstdayofnextmonth.Year, firstdayofnextmonth.Month, DateTime.DaysInMonth(firstdayofnextmonth.Year, firstdayofnextmonth.Month));
                    ///If the Issue date 
                    if (asset.IssueDate.Day < 10)
                    {

                        asset.DepCalFromDate = firstdayofmonth;
                    }
                    if (asset.IssueDate.Day >= 10)
                    {
                        asset.DepCalFromDate = firstdayofnextmonth;
                    }


                    if (asset.CalculateDepreciation == true)
                    {

                        asset.FA_Dep_StatusId = 1;
                        asset.IsDepRunning = true;
                        if (asset.FA_DetailsId > 0)
                        {
                            var record = _context.Tem_FA_Details.Find(asset.FA_DetailsId);
                            if (record != null)
                            {
                                _context.Remove(record).State = EntityState.Deleted;
                                _context.SaveChanges();
                            }
                            var records = _context.TemDepSchedules.Where(x => x.FA_DetailsId == asset.FA_DetailsId).ToList();
                            if (records.Count > 0)
                            {
                                _context.RemoveRange(records);
                                _context.SaveChanges();
                            }

                            asset.FA_DetailsId = 0;
                        }

                        _context.Tem_FA_Details.Add(asset);


                        _context.SaveChanges();

                        var id = asset.FA_DetailsId;
                        var query = $"Exec [CalculateDepreciationTem] '{comid}',{id}";
                        //run procedure to save new depreciation schedule
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@comid", comid);
                        parameters[1] = new SqlParameter("@FA_DetailsId", id);

                        Helper.ExecProc("[CalculateDepreciationTem]", parameters);

                        SqlParameter[] parameter = new SqlParameter[1];
                        parameter[0] = new SqlParameter("@FA_DetailsId", id);


                        list = Helper.ExecProcMapTList<TemDepSchedule>("[GetTemDepSchedule]", parameter);
                        //_context.TemDepSchedules.Where(x => x.FA_DetailsId == id).ToList();
                    }
                    else
                    {
                        asset.FA_Dep_StatusId = 2;
                        asset.IsDepRunning = false;
                        asset.FA_DetailsId = 0;
                        _context.Tem_FA_Details.Add(asset);
                        _context.SaveChanges();
                    }


                }

                _context.SaveChanges();
                return Json(new { success = 1, error = 0, list = list, message = "" });
                //}
            }
            catch (Exception e)
            {

                return Json(new { success = 0, error = 1, list = "", message = e.Message });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TemAssignSaleAsset(List<Tem_FA_Sell> assetListToAssign)
        {
            try
            {
                var list = new List<TemDepScheduleSale>();
                var comid = HttpContext.Session.GetString("comid");
                // if (ModelState.IsValid)
                //{
                foreach (var asset in assetListToAssign)
                {
                    DateTime now = asset.SalesDate;
                    //var firstdayofmonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));//feb
                    var firstdayofmonth = new DateTime(now.Year, now.Month, 1);//feb
                    var firstdayofnextmonth = firstdayofmonth.AddMonths(1);//mar
                    //firstdayofnextmonth = new DateTime(firstdayofnextmonth.Year, firstdayofnextmonth.Month, DateTime.DaysInMonth(firstdayofnextmonth.Year, firstdayofnextmonth.Month));
                    ///If the Issue date 
                    if (asset.SalesDate.Day < 10)
                    {

                        asset.DepCalFromDate = firstdayofmonth;
                    }
                    if (asset.SalesDate.Day >= 10)
                    {
                        asset.DepCalFromDate = firstdayofnextmonth;
                    }


                    if (asset.CalculateDepreciation == true)
                    {

                        asset.FA_Dep_StatusId = 1;
                        asset.IsDepRunning = true;
                        if (asset.FA_SellId > 0)
                        {
                            var record = _context.Tem_FA_Details.Find(asset.FA_SellId);
                            if (record != null)
                            {
                                _context.Remove(record).State = EntityState.Deleted;
                                _context.SaveChanges();
                            }
                            var records = _context.TemDepScheduleSales.Where(x => x.FA_SellId == asset.FA_SellId).ToList();
                            if (records.Count > 0)
                            {
                                _context.RemoveRange(records);
                                _context.SaveChanges();
                            }

                            asset.FA_SellId = 0;
                        }

                        _context.Tem_FA_Sell.Add(asset);


                        _context.SaveChanges();

                        var id = asset.FA_SellId;
                        var query = $"Exec [CalculateDepreciationSaleTem] '{comid}',{id}";
                        //run procedure to save new depreciation schedule
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@comid", comid);
                        parameters[1] = new SqlParameter("@FA_SaleId", id);

                        Helper.ExecProc("[CalculateDepreciationSaleTem]", parameters);

                        SqlParameter[] parameter = new SqlParameter[1];
                        parameter[0] = new SqlParameter("@FA_SaleId", id);


                        list = Helper.ExecProcMapTList<TemDepScheduleSale>("[GetTemDepScheduleSale]", parameter);
                        //_context.TemDepSchedules.Where(x => x.FA_DetailsId == id).ToList();
                    }
                    else
                    {
                        asset.FA_Dep_StatusId = 2;
                        asset.IsDepRunning = false;
                        asset.FA_SellId = 0;
                        _context.Tem_FA_Sell.Add(asset);
                        _context.SaveChanges();
                    }


                }

                _context.SaveChanges();
                return Json(new { success = 1, error = 0, list = list, message = "" });
                //}
            }
            catch (Exception e)
            {

                return Json(new { success = 0, error = 1, list = "", message = e.Message });

            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartDepSchedule(int id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var asset = _context.FA_Details.Where(x => x.FA_DetailsId == id).FirstOrDefault();
            asset.FA_Dep_StatusId = 1;
            asset.IsDepRunning = true;
            _context.Entry(asset).State = EntityState.Modified;
            _context.SaveChanges();

            var query = $"Exec [CalculateDepreciation] '{comid}',{id}";
            //run procedure to save new depreciation schedule
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@FA_DetailsId", id);

            Helper.ExecProc("[CalculateDepreciation]", parameters);
            return Json(new { success = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartDepScheduleSale(int id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var asset = _context.fA_Sells.Where(x => x.FA_SellId == id).FirstOrDefault();
            asset.FA_Dep_StatusId = 1;
            asset.IsDepRunning = true;
            _context.Entry(asset).State = EntityState.Modified;
            _context.SaveChanges();

            var query = $"Exec [CalculateDepreciationSale] '{comid}',{id}";
            //run procedure to save new depreciation schedule
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@FA_SaleId", id);

            Helper.ExecProc("[CalculateDepreciationSale]", parameters);
            return Json(new { success = 1 });
        }

        public IActionResult SetTemp()
        {
            var list = _context.FA_Details.Where(x => x.IsDepRunning == true).Where(x => x.UsefullLife > 0).Select(x => x.FA_DetailsId).ToList();
            var comid = HttpContext.Session.GetString("comid");
            foreach (var item in list)
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@comid", comid);
                parameters[1] = new SqlParameter("@FA_DetailsId", item);
                Helper.ExecProc("[CalculateDepreciation]", parameters);
            }




            return Json(true);
        }

        public async Task<IActionResult> SeeDepSchedule(int id)
        {
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@FA_DetailsId", id);


            var list = Helper.ExecProcMapTList<DepreciationSchedule>("[GetDepSchedule]", parameter);
            return Json(new { success = 1, list = list });
        }

        public async Task<IActionResult> SeeDepScheduleSale(int id)
        {
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@FA_SaleId", id);


            var list = Helper.ExecProcMapTList<DepreciationScheduleSales>("[GetTemDepScheduleSale]", parameter);
            return Json(new { success = 1, list = list });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeleAsset(List<FA_Sell> assetListToSale)
        {


            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");
                // if (ModelState.IsValid)
                //{
                foreach (var asset in assetListToSale)
                {
                    DateTime now = asset.SalesDate;
                    //var firstdayofmonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));//feb
                    var firstdayofmonth = new DateTime(now.Year, now.Month, 1);//feb
                    var firstdayofnextmonth = firstdayofmonth.AddMonths(1);//mar
                    //firstdayofnextmonth = new DateTime(firstdayofnextmonth.Year, firstdayofnextmonth.Month, DateTime.DaysInMonth(firstdayofnextmonth.Year, firstdayofnextmonth.Month));
                    ///If the Issue date 
                    if (asset.SalesDate.Day < 10)
                    {
                        asset.DepCalFromDate = firstdayofmonth;
                    }
                    if (asset.SalesDate.Day >= 10)
                    {
                        asset.DepCalFromDate = firstdayofnextmonth;
                    }
                    if (asset.FA_SellId > 0)
                    {
                        asset.DateUpdated = DateTime.Now;
                        asset.UpdateByUserId = HttpContext.Session.GetString("userid");
                        if (asset.IsDelete == true)
                        {
                            if (asset.CalculateDepreciation == true)
                            {
                                var l = _context.DepreciationScheduleSales.Where(x => x.FA_SellId == asset.FA_SellId).ToList();
                                if (l.Count > 0)
                                {
                                    _context.DepreciationScheduleSales.RemoveRange(l);
                                    _context.SaveChanges();
                                }

                            }
                            _context.fA_Sells.Remove(asset);

                        }
                        else
                        {
                            if (asset.CalculateDepreciation == true)
                            {
                                var l = _context.DepreciationScheduleSales.Where(x => x.FA_SellId == asset.FA_SellId).ToList();
                                if (l.Count == 0)
                                {
                                    var query = $"Exec CalculateDepreciationSale '{comid}',{asset.FA_SellId}";
                                    //run procedure to save new depreciation schedule
                                    SqlParameter[] parameters = new SqlParameter[2];
                                    parameters[0] = new SqlParameter("@comid", comid);
                                    parameters[1] = new SqlParameter("@FA_SaleId", asset.FA_SellId);

                                    Helper.ExecProc("[CalculateDepreciationSale]", parameters);

                                }
                            }
                            if (asset.CalculateDepreciation == false)
                            {
                                var l = _context.DepreciationScheduleSales.Where(x => x.FA_SellId == asset.FA_SellId).ToList();
                                //check condition if there is already depreciation schedule but modify with false 
                                if (l.Count > 0)
                                {
                                    _context.DepreciationScheduleSales.RemoveRange(l);
                                    _context.SaveChanges();
                                }
                                asset.FA_Dep_StatusId = 2;
                                asset.IsDepRunning = false;
                            }
                            _context.Entry(asset).State = EntityState.Modified;


                        }
                    }
                    else
                    {
                        asset.CreatedByUserId = HttpContext.Session.GetString("userid");
                        asset.DateAdded = DateTime.Now;


                        if (asset.CalculateDepreciation == true)
                        {

                            asset.FA_Dep_StatusId = 1;
                            asset.IsDepRunning = true;
                            _context.fA_Sells.Add(asset);
                            _context.SaveChanges();

                            var id = asset.FA_SellId;
                            var query = $"Exec CalculateDepreciationSale '{comid}',{id}";
                            //run procedure to save new depreciation schedule
                            SqlParameter[] parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@comid", comid);
                            parameters[1] = new SqlParameter("@FA_SaleId", id);

                            Helper.ExecProc("[CalculateDepreciationSale]", parameters);

                        }
                        else
                        {
                            asset.FA_Dep_StatusId = 2;
                            asset.IsDepRunning = false;
                            _context.fA_Sells.Add(asset);
                            _context.SaveChanges();
                        }

                    }
                }


                ViewData["FoD"] = new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text");
                ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
                ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                                join p in _context.Products
                                                on m.ProductId equals p.ProductId
                                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text"); ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
                ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text");
                //ViewData["ProductId"] = new SelectList(from product in _context.Products
                //                                       join grr in _context.GoodsReceiveSub
                // on product.ProductId equals grr.ProductId
                //                                       where product.CategoryId == 21
                //                                       select (new { Text = product.AssetCode + " " + product.ProductName, Value = product.ProductId }), "Value", "Text");
                ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())), "ProductId", "ProductName");
                //ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x=>x.IsInActive==false), "FA_DetailsId", "AssetItem");

                ViewBag.DepreciationExpenseAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                ViewBag.AccumulateDepreciationAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");


                return Json(new { success = 1, error = 0, message = "" });
                //}
            }
            catch (Exception e)
            {

                return Json(new { success = 0, error = 1, message = e.Message });

            }
        }

        public async Task<IActionResult> GetDepProccess()
        {
            var comid = HttpContext.Session.GetString("comid");
            var quary = $"EXEC DepreciationProcess '{comid}'";

            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@comid", comid);
            // parameters[1] = new SqlParameter("@userid", userid);
            //parameters[2] = new SqlParameter("@PurReqId", PurReqId);
            List<DepViewModel> deplist = Helper.ExecProcMapTList<DepViewModel>("DepreciationProcess", parameters);

            return Json(new { list = deplist });
        }

        // GET: FA_Master/Edit/5
        public async Task<IActionResult> Edit(string type, int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit";
            var comid = HttpContext.Session.GetString("comid");
            ViewData["FoD"] = new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text");
            ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
            //ViewData["FA"] = new SelectList(from m in _context.fA_Masters
            //                                join p in _context.Products
            //                                on m.ProductId equals p.ProductId
            //                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text");
            //ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");

            //ViewData["ProductId"] = new SelectList(from product in _context.Products
            //                                       join grr in _context.GoodsReceiveSub
            //     on product.ProductId equals grr.ProductId
            //                                       where product.CategoryId == 21
            //                                       select (new { Text = product.AssetCode + " " + product.ProductName, Value = product.ProductId }), "Value", "Text");
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())), "ProductId", "ProductName");

            ViewBag.DepreciationExpenseAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            ViewBag.AccumulateDepreciationAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

            var fA_Master = new FA_Master();
            var fA_Details = new FA_Details();
            var fA_Sells = new FA_Sell();

            if (type == "master")
            {
                fA_Master = await _context.fA_Masters.FindAsync(id);
                if (fA_Master == null)
                {
                    return NotFound();
                }

                return View("FAMasterCreate", fA_Master);
                //return Json(new { list = fA_Master });
            }
            else if (type == "details")
            {
                fA_Details = await _context.FA_Details.FindAsync(id);

                if (fA_Details == null)
                {
                    return NotFound();
                }
                ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                                join p in _context.Products
                                                on m.ProductId equals p.ProductId
                                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text", fA_Details.FA_MasterId);
                ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text", fA_Details.FA_DetailsId);
                ViewData["Departments"] = new SelectList(_context.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptName, x.DeptId }), "DeptId", "DeptName", fA_Details.AssignToDept);
                return View("FADetailCreate", fA_Details);
                //return Json(new { list = fA_Details });

            }
            else if (type == "sales")
            {
                fA_Sells = await _context.fA_Sells.FindAsync(id);
                if (fA_Sells == null)
                {
                    return NotFound();
                }
                ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                                join p in _context.Products
                                                on m.ProductId equals p.ProductId
                                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text", fA_Sells.FA_MasterId);
                ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false).Where(x => x.FA_MasterId == fA_Sells.FA_MasterId), "FA_DetailsId", "AssetItem", fA_Sells.FA_DetailsId);
                ViewData["DetailsId"] = fA_Sells.FA_DetailsId;
                return View("FASalesCreate", fA_Sells);
                //return Json(new { list = fA_Sells });

            }



            return Json(new { list = fA_Master });
        }

        // POST: FA_Master/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FA_MasterId,ProductId,AssetCode,DMId,Parcentage,DepreciationExpenseAccountId,AccumulateDepreciationAccountId,FoD,ComId")] FA_Master fA_Master)
        {
            if (id != fA_Master.FA_MasterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fA_Master);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FA_MasterExists(fA_Master.FA_MasterId))
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
            var comid = HttpContext.Session.GetString("comid");
            ViewData["FoD"] = new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text");
            ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName");
            ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                            join p in _context.Products
                                            on m.ProductId equals p.ProductId
                                            select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text"); ViewData["Employees"] = new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
            ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text");
            ViewBag.DepreciationExpenseAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            ViewBag.AccumulateDepreciationAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");


            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())), "ProductId", "ProductName");
            return View("FAMasterCreate", fA_Master);
        }

        // GET: FA_Master/Delete/5
        public async Task<IActionResult> Delete(string type, int? id)
        {

            ViewData["Title"] = "Delete";
            var comid = HttpContext.Session.GetString("comid");

            ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                            join p in _context.Products
                                            on m.ProductId equals p.ProductId
                                            select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text");


            if (id == null)
            {
                return NotFound();
            }

            var fA_Master = new FA_Master();
            var fA_Details = new FA_Details();
            var fA_Sells = new FA_Sell();

            if (type == "master")
            {
                fA_Master = await _context.fA_Masters.FindAsync(id);
                if (fA_Master == null)
                {
                    return NotFound();
                }
                ViewData["FoD"] = new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text", fA_Master.FOD);
                ViewData["DMId"] = new SelectList(_context.DepreciationMethods, "DMId", "DMName", fA_Master.DMId);
                ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.comid == comid).Where(x => x.vPrimaryCategory.Name.ToUpper().Contains("Fixed Asset".ToUpper())), "ProductId", "ProductName");

                ViewBag.DepreciationExpenseAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemDepExp == true || c.AccCode.Contains("4-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", fA_Master.AccId_DepreciationExpense);
                ViewBag.AccumulateDepreciationAccountId = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.IsItemAccmulateddDep == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", fA_Master.AccId_AccumulatedDepreciation);


                return View("FAMasterCreate", fA_Master);
            }
            else if (type == "details")
            {
                fA_Details = await _context.FA_Details.FindAsync(id);
                ViewBag.Title = "Delete";
                if (fA_Details == null)
                {
                    return NotFound();
                }
                ViewData["Section"] = new SelectList(_context.Cat_Section.Where(c => c.ComId == comid).Select(x => new { x.SectId, Text = x.SectName }), "SectId", "Text", fA_Details.FA_DetailsId);
                ViewData["Departments"] = new SelectList(_context.Cat_Department.Where(c => c.ComId == comid).Select(x => new { x.DeptName, x.DeptId }), "DeptId", "DeptName", fA_Details.AssignToDept);
                return View("FADetailCreate", fA_Details);

            }
            else if (type == "sales")
            {
                fA_Sells = await _context.fA_Sells.FindAsync(id);
                if (fA_Sells == null)
                {
                    return NotFound();
                }
                ViewData["FA"] = new SelectList(from m in _context.fA_Masters
                                                join p in _context.Products
                                                on m.ProductId equals p.ProductId
                                                select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId }), "Value", "Text", fA_Sells.FA_MasterId);
                ViewData["Details"] = new SelectList(_context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false), "FA_DetailsId", "AssetItem", fA_Sells.FA_DetailsId);
                ViewData["DetailsId"] = fA_Sells.FA_DetailsId;
                return View("FASalesCreate", fA_Sells);

            }

            return NotFound();
        }

        public IActionResult InitializeSelectList()
        {
            var comid = HttpContext.Session.GetString("comid");
            var master = from m in _context.fA_Masters
                         join p in _context.Products
                         on m.ProductId equals p.ProductId
                         select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId });


            //from m in _context.fA_Masters
            //         join p in _context.Products
            //         on m.ProductId equals p.ProductId
            //         join grr in _context.GoodsReceiveSub
            //          on p.ProductId equals grr.ProductId
            //         where m.ComId == comid
            //         select (new { Text = m.AssetCode + " " + p.ProductName, Value = m.FA_MasterId });
            var sale = _context.FA_Details.Where(x => x.ComId == comid).Where(x => x.IsInActive == false).Select(x => new { Text = x.AssetItem, Value = x.FA_DetailsId });

            return Json(new { master = master, sale = sale });
        }

        // POST: FA_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string type, int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            try
            {


                if (type == "master")
                {
                    var fA_Master = await _context.fA_Masters.FindAsync(id);
                    _context.fA_Masters.Remove(fA_Master);
                    await _context.SaveChangesAsync();
                    return Json(new { result = 1 });

                }
                else if (type == "details")
                {
                    var fA_Details = await _context.FA_Details.FindAsync(id);
                    _context.FA_Details.Remove(fA_Details);
                    await _context.SaveChangesAsync();
                    return Json(new { result = 1 });

                }
                else if (type == "sales")
                {
                    var fA_Sell = await _context.fA_Sells.FindAsync(id);
                    _context.fA_Sells.Remove(fA_Sell);
                    await _context.SaveChangesAsync();
                    return Json(new { result = 1 });

                }


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return Json(new { result = 3, ex = e.InnerException.Message });
            }

            return Json(new { result = 0 });

        }

        private bool FA_MasterExists(int id)
        {
            return _context.fA_Masters.Any(e => e.FA_MasterId == id);
        }

        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");

            var reportname = "rptFixedAsset";
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            HttpContext.Session.SetString("reportquery", "Exec [Acc_rptFixedAssets] '" + comid + "','" + id + "'");

            string filename = _context.fA_Masters.Where(x => x.FA_MasterId == id).Single().ToString();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            HttpContext.Session.SetObject("rptList", postData);

            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;


            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
            string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return Redirect(callBackUrl);

        }


    }
}
