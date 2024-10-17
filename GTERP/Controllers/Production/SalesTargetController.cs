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
    public class SalesTargetController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;
        PermissionLevel PL;
        public SalesTargetController(GTRDBContext context, TransactionLogRepository tran, PermissionLevel pl)
        {
            tranlog = tran;
            db = context;
            PL = pl;
        }

        // GET: SalesTargetSetup
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(await db.SalesTargetSetup.Include(p => p.YearName).Include(m => m.MonthName).Include(p => p.Unit).OrderByDescending(o => o.SaleTargetSetId).ToListAsync());
        }



        // GET: SalesTargetSetup/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName");
            string comid = HttpContext.Session.GetString("comid");
            var data = db.SalesTargetSetup.Where(s => s.ComId == comid).OrderByDescending(p => p.SaleTargetSetId).FirstOrDefault();
            data.SaleTargetSetId = 0;
            var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            if (fiscalYear != null)
            {
                var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FiscalYearId), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId).Where(f => f.FYId == fiscalYear.FYId), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);
                return View(data);
            }

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName");

            return View(data);
        }

        // POST: SalesTargetSetup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesTargetSetup salesTarget)
        {
            if (ModelState.IsValid)
            {

                var exist = await db.SalesTargetSetup
                    .Where(p => p.SaleTargetSetId != salesTarget.SaleTargetSetId
                    && p.FiscalYearId == salesTarget.FiscalYearId && p.FiscalMonthId == salesTarget.FiscalMonthId && p.PrdUnitId == salesTarget.PrdUnitId).FirstOrDefaultAsync();
                if (exist != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    TempData["Status"] = "2";
                    ViewBag.Title = "Edit";
                    ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", salesTarget.PrdUnitId);
                    //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", salesTarget.ProductId);

                    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", salesTarget.FiscalYearId);
                    ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId).Where(f => f.FYId == salesTarget.FiscalYearId), "FiscalMonthId", "MonthName", salesTarget.FiscalMonthId);

                    return View("Create", exist);
                }
                salesTarget.UserId = HttpContext.Session.GetString("userid");
                salesTarget.ComId = HttpContext.Session.GetString("comid");

                //salesTarget.FiscalMonthGoal = (float)Math.Round((salesTarget.FiscalYearGoal / 12),2);


                if (salesTarget.SaleTargetSetId > 0)
                {
                    salesTarget.DateUpdated = DateTime.Now;
                    salesTarget.UpdatedbyUserId = HttpContext.Session.GetString("userid");
                    db.Entry(salesTarget).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), salesTarget.SaleTargetSetId.ToString(), "Update", salesTarget.SaleTargetSetId.ToString());

                }
                else
                {
                    salesTarget.AddedbyUserId = HttpContext.Session.GetString("userid");
                    salesTarget.DateAdded = DateTime.Now;
                    db.Add(salesTarget);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), salesTarget.SaleTargetSetId.ToString(), "Create", salesTarget.SaleTargetSetId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(salesTarget);
        }


        [HttpGet]
        public IActionResult GetFiscalMonth(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId).Where(m => m.FYId == id && m.ComId == comid).ToList();
            return Json(data);
        }

        // GET: SalesTargetSetup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var salesTarget = await db.SalesTargetSetup.FindAsync(id);
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", salesTarget.PrdUnitId);
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", salesTarget.ProductId);

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", salesTarget.FiscalYearId);
            ViewBag.FiscalMOnthId = new SelectList(db.Acc_FiscalMonths.Where(m => m.FYId == salesTarget.FiscalYearId).OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", salesTarget.FiscalYearId);

            if (salesTarget == null)
            {
                return NotFound();
            }
            return View("Create", salesTarget);
        }

        // GET: SalesTargetSetup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesTarget = await db.SalesTargetSetup
                .FirstOrDefaultAsync(m => m.SaleTargetSetId == id);

            if (salesTarget == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", salesTarget.PrdUnitId);
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", salesTarget.ProductId);

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", salesTarget.FiscalYearId);
            ViewBag.FiscalMOnthId = new SelectList(db.Acc_FiscalMonths.Where(m => m.FYId == salesTarget.FiscalYearId).OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", salesTarget.FiscalMonthId);

            return View("Create", salesTarget);
        }

        // POST: SalesTargetSetup/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var salesTarget = await db.SalesTargetSetup.FindAsync(id);
                db.SalesTargetSetup.Remove(salesTarget);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), salesTarget.SaleTargetSetId.ToString(), "Delete", salesTarget.SaleTargetSetId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, SaleTargetSetId = salesTarget.SaleTargetSetId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int SalesTargetSetupId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.SalesTargetSetup.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.SalesTargetSetup.Any(d => d.DeptName == DeptName));
        //}


        private bool SalesTargetSetupExists(int id)
        {
            return db.SalesTargetSetup.Any(e => e.SaleTargetSetId == id);
        }
    }
}