using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    [OverridableAuthorize]
    public class ProductionTargetController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;
        private PermissionLevel PL;

        public ProductionTargetController(GTRDBContext context, TransactionLogRepository tran, PermissionLevel _pl)
        {
            tranlog = tran;
            db = context;
            PL = _pl;
        }

        // GET: ProductionTargetSetup
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(await db.ProductionTargetSetup.Include(p => p.YearName).Include(p => p.MonthName).Include(p => p.Unit).OrderByDescending(o => o.PrdTargetSetId).ToListAsync());
        }



        // GET: ProductionTargetSetup/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName");
            string comid = HttpContext.Session.GetString("comid");
            var data = db.ProductionTargetSetup.Where(s => s.ComId == comid).OrderByDescending(p => p.PrdTargetSetId).FirstOrDefault();
            var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            data.PrdTargetSetId = 0;
            if (fiscalYear != null)
            {
                var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId).Where(f => f.FYId == fiscalYear.FYId), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);
                return View(data);
            }

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName");

            return View(data);
        }

        // POST: ProductionTargetSetup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionTargetSetup prdTarget)
        {
            if (ModelState.IsValid)
            {
                var exist = await db.ProductionTargetSetup
                    .Where(p => p.PrdTargetSetId != prdTarget.PrdTargetSetId
                    && p.FiscalYearId == prdTarget.FiscalYearId && p.FiscalMonthId == prdTarget.FiscalMonthId && p.PrdUnitId == prdTarget.PrdUnitId).FirstOrDefaultAsync();
                if (exist != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    TempData["Status"] = "2";
                    ViewBag.Title = "Edit";
                    ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName", prdTarget.PrdUnitId);
                    // //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", prdTarget.ProductId);

                    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYName), "FiscalYearId", "FYName", prdTarget.FiscalYearId);
                    ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.MonthName), "FiscalMonthId", "MonthName", prdTarget.FiscalMonthId);

                    return View("Create", exist);
                }
                prdTarget.UserId = HttpContext.Session.GetString("userid");
                prdTarget.ComId = HttpContext.Session.GetString("comid");

                //prdTarget.PrdCapacityYear = (float)Math.Round((prdTarget.PrdCapacityYear / 12), 2);

                if (prdTarget.PrdTargetSetId > 0)
                {
                    prdTarget.DateUpdated = DateTime.Now;
                    prdTarget.UpdatedbyUserId = HttpContext.Session.GetString("userid");
                    db.Entry(prdTarget).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), prdTarget.PrdTargetSetId.ToString(), "Update", prdTarget.PrdTargetSetId.ToString());

                }
                else
                {
                    prdTarget.AddedbyUserId = HttpContext.Session.GetString("userid");
                    prdTarget.DateAdded = DateTime.Now;
                    db.Add(prdTarget);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), prdTarget.PrdTargetSetId.ToString(), "Create", prdTarget.PrdTargetSetId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(prdTarget);
        }

        //[HttpGet]
        //public IActionResult GetData(int unitid, int productid)
        //{
        //    string comid = HttpContext.Session.GetString("comid");
        //    var data = db.ProductionTargetSetup
        //        .Where(m => m.p == productid && m.PrdUnitId == unitid && m.ComId == comid)
        //        .OrderByDescending(u => u.UtilitiesId)
        //        // .Select(u=> new {design=u.StdDesignVal, budget=u.FiscalYearBudgetVal  })
        //        .FirstOrDefault();
        //    return Json(data);
        //}


        [HttpGet]
        public IActionResult GetFiscalMonth(int id)
        {
            var data = db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId).Where(m => m.FYId == id).ToList();
            return Json(data);
        }

        // GET: ProductionTargetSetup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var prdTarget = await db.ProductionTargetSetup.FindAsync(id);
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", prdTarget.PrdUnitId);
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", prdTarget.ProductId);

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", prdTarget.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", prdTarget.FiscalMonthId);

            if (prdTarget == null)
            {
                return NotFound();
            }
            return View("Create", prdTarget);
        }

        // GET: ProductionTargetSetup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prdTarget = await db.ProductionTargetSetup
                .FirstOrDefaultAsync(m => m.PrdTargetSetId == id);

            if (prdTarget == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", prdTarget.PrdUnitId);
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", prdTarget.ProductId);

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", prdTarget.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", prdTarget.FiscalMonthId);

            return View("Create", prdTarget);
        }

        // POST: ProductionTargetSetup/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var prdTarget = await db.ProductionTargetSetup.FindAsync(id);
                db.ProductionTargetSetup.Remove(prdTarget);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), prdTarget.PrdTargetSetId.ToString(), "Delete", prdTarget.PrdTargetSetId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, PrdTargetSetId = prdTarget.PrdTargetSetId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int ProductionTargetSetupId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.ProductionTargetSetup.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.ProductionTargetSetup.Any(d => d.DeptName == DeptName));
        //}


        private bool ProductionTargetSetupExists(int id)
        {
            return db.ProductionTargetSetup.Any(e => e.PrdTargetSetId == id);
        }
    }
}