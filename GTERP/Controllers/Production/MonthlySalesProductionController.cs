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
    public class MonthlySalesProductionController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public MonthlySalesProductionController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: MonthlySalesProduction
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(await db.MonthlySalesProductions.Include(p => p.YearName).Include(p => p.MonthName).Include(p => p.Unit).ToListAsync());
        }



        // GET: MonthlySalesProduction/Create
        public IActionResult Create()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName");
            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName");
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName");

            var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true && f.ComId == comid).FirstOrDefault();
            if (fiscalYear != null)
            {
                //var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date && f.ComId == comid).FirstOrDefault();
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(y => y.FYId.ToString() == fiscalYear.FYId.ToString()).OrderBy(x => x.FiscalMonthId), "FiscalMonthId", "MonthName");
                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalYear.FiscalYearId).OrderBy(y => y.FiscalYearId), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);

                return View();
            }

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");

            return View();
        }

        // POST: MonthlySalesProduction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlySalesProduction MonthlySalesProduction)
        {
            var title = "";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (ModelState.IsValid)
                {

                    //var exist = await db.MonthlySalesProductions
                    //    .Where(p => p.MonthlySalesProductionId != MonthlySalesProduction.MonthlySalesProductionId
                    //    && p.FiscalYearId == MonthlySalesProduction.FiscalYearId && p.PrdUnitId==MonthlySalesProduction.PrdUnitId).FirstOrDefaultAsync();
                    //if (exist!=null)
                    //{
                    //    TempData["Message"] = "Data Already Exist";
                    //    TempData["Status"] = "2";
                    //    ViewBag.Title = "Edit";
                    //    ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName", MonthlySalesProduction.PrdUnitId);

                    //    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);

                    //    return View("Create",exist);
                    //}
                    MonthlySalesProduction.UserId = HttpContext.Session.GetString("userid");
                    MonthlySalesProduction.ComId = HttpContext.Session.GetString("comid");



                    if (MonthlySalesProduction.MonthlySalesProductionId > 0)
                    {
                        title = "Edit";

                        db.Entry(MonthlySalesProduction).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), MonthlySalesProduction.MonthlySalesProductionId.ToString(), "Update", MonthlySalesProduction.MonthlySalesProductionId.ToString());

                    }
                    else
                    {
                        title = "Create";

                        db.Add(MonthlySalesProduction);
                        await db.SaveChangesAsync();

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), MonthlySalesProduction.MonthlySalesProductionId.ToString(), "Create", MonthlySalesProduction.MonthlySalesProductionId.ToString());

                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(MonthlySalesProduction);
            }
            catch (Exception ex)
            {
                ViewBag.Title = title;


                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", MonthlySalesProduction.PrdUnitId);

                //ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);

                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(y => y.ComId == comid && y.FYId == MonthlySalesProduction.YearName.FYId).OrderBy(x => x.FiscalMonthId), "FiscalMonthId", "MonthName", MonthlySalesProduction.FiscalMonthId);
                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.Where(x => x.ComId == comid).OrderBy(y => y.FiscalYearId), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);



                //ModelState.AddModelError("CustomError", ex.Message != null ? ex.InnerException != null ? ex.InnerException.ToString() : ex.Message : ex.Message) ;

                ModelState.AddModelError("CustomError", ex.Message != null ? ex.InnerException != null ? "Duplicate Entry Found." : ex.Message : ex.Message);


                return View(MonthlySalesProduction);
            }
        }


        [HttpGet]
        public IActionResult GetFiscalMonth(int id)
        {
            var data = db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(m => m.FYId == id).ToList();
            return Json(data);
        }

        // GET: MonthlySalesProduction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Edit";
            var MonthlySalesProduction = await db.MonthlySalesProductions
                //.Include(x=>x.YearName)
                //.Include(x=>x.MonthName)
                .FindAsync(id);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", MonthlySalesProduction.PrdUnitId);

            //ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);

            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(y => y.ComId == comid && y.FYId == MonthlySalesProduction.YearName.FYId).OrderBy(x => x.FiscalMonthId), "FiscalMonthId", "MonthName", MonthlySalesProduction.FiscalMonthId);
            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.Where(x => x.ComId == comid).OrderBy(y => y.FiscalYearId), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);


            if (MonthlySalesProduction == null)
            {
                return NotFound();
            }
            return View("Create", MonthlySalesProduction);
        }

        // GET: MonthlySalesProduction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");


            var MonthlySalesProduction = await db.MonthlySalesProductions.FindAsync(id);
            //.FirstOrDefaultAsync(m => m.MonthlySalesProductionId == id);

            if (MonthlySalesProduction == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", MonthlySalesProduction.PrdUnitId);

            //ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(y => y.ComId == comid).OrderBy(x => x.FiscalMonthId), "FiscalMonthId", "MonthName", MonthlySalesProduction.FiscalMonthId);
            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.Where(x => x.ComId == comid).OrderBy(y => y.FiscalYearId), "FiscalYearId", "FYName", MonthlySalesProduction.FiscalYearId);

            return View("Create", MonthlySalesProduction);
        }

        // POST: MonthlySalesProduction/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var MonthlySalesProduction = await db.MonthlySalesProductions.FindAsync(id);
                db.MonthlySalesProductions.Remove(MonthlySalesProduction);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), MonthlySalesProduction.MonthlySalesProductionId.ToString(), "Delete", MonthlySalesProduction.MonthlySalesProductionId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, MonthlySalesProductionId = MonthlySalesProduction.MonthlySalesProductionId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int MonthlySalesProductionId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.MonthlySalesProductions.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.MonthlySalesProductions.Any(d => d.DeptName == DeptName));
        //}


        private bool MonthlySalesProductionExists(int id)
        {
            return db.MonthlySalesProductions.Any(e => e.MonthlySalesProductionId == id);
        }
    }
}