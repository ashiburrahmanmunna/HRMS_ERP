using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{

    [OverridableAuthorize]
    public class UseUtilitiesController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;
        private PermissionLevel PL;

        public UseUtilitiesController(GTRDBContext context, TransactionLogRepository tran, PermissionLevel _PL)
        {
            tranlog = tran;
            db = context;
            PL = _PL;
        }

        // GET: UseUtilities
        public async Task<IActionResult> Index(int? unitId, int? yearId, int? monthId)
        {
            var data = db.UseUtilities.Include(p => p.YearName).Include(p => p.MonthName).Include(p => p.RawMaterial).Include(p => p.Unit).OrderBy(u => u.UtilitiesId);

            if (unitId == null || yearId == null || monthId == null)
            {
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");

                var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
                if (fiscalYear != null)
                {
                    var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                    ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(m => m.FYId == fiscalYear.FiscalYearId).OrderBy(y => y.FiscalMonthId).Where(f => f.FYId == fiscalYear.FYId), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);

                    return View(await data.Where(u => u.FiscalYearId == fiscalYear.FiscalYearId && u.FiscalMonthId == fiscalMonth.FiscalMonthId).ToListAsync());

                }

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName");
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName");

                TempData["Message"] = "Data Load Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

                return View(new List<UseUtilities>());
            }
            else
            {
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", unitId);
                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", yearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(m => m.FYId == yearId).OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", monthId);

                TempData["Message"] = "Data Load Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
                return View(await data.Where(u => u.FiscalYearId == yearId && u.FiscalMonthId == monthId && u.PrdUnitId == unitId).ToListAsync());
            }

        }



        // GET: UseUtilities/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");

            var products = db.Products.Where(p => new[] { 18, 19, 37, 39, 40, 41 }.Contains(p.CategoryId))
                .Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId });

            //ViewData["ProductId"] = new SelectList(products, "ProductId", "ProductName");
            ViewData["RawMaterialId"] = new SelectList(products, "Value", "Text");

            var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            if (fiscalYear != null)
            {
                var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId).Where(f => f.FYId == fiscalYear.FYId), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);
                return View(new UseUtilities());
            }

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName");


            return View(new UseUtilities());
        }

        [HttpGet]
        public IActionResult GetFiscalMonth(int id)
        {
            var data = db.Acc_FiscalMonths.OrderBy(y => y.MonthName).Where(m => m.FYId == id).ToList();
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetRatio(int unitid, int productid)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = db.UseUtilities
                .Where(m => m.RawMaterialId == productid && m.PrdUnitId == unitid && m.ComId == comid)
                .OrderByDescending(u => u.UtilitiesId)
                // .Select(u=> new {design=u.StdDesignVal, budget=u.FiscalYearBudgetVal  })
                .FirstOrDefault();
            return Json(data);
        }

        // POST: UseUtilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UseUtilities useUtilities)
        {
            if (ModelState.IsValid)
            {

                var exist = await db.UseUtilities
                    .Where(p => p.UtilitiesId != useUtilities.UtilitiesId && p.FiscalMonthId == useUtilities.FiscalMonthId
                    && p.FiscalYearId == useUtilities.FiscalYearId && p.PrdUnitId == useUtilities.PrdUnitId && p.RawMaterialId == useUtilities.RawMaterialId).FirstOrDefaultAsync();


                if (exist != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    TempData["Status"] = "2";
                    ViewBag.Title = "Edit";

                    ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName", useUtilities.PrdUnitId);
                    var products = db.Products;
                    //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", useUtilities.ProductId);
                    ViewData["RawMaterialId"] = new SelectList(products, "ProductId", "ProductName", useUtilities.RawMaterialId);
                    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", useUtilities.FiscalYearId);
                    ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", useUtilities.FiscalMonthId);

                    return View("Create", exist);
                }
                useUtilities.UserId = HttpContext.Session.GetString("userid");
                useUtilities.ComId = HttpContext.Session.GetString("comid");


                if (useUtilities.UtilitiesId > 0)
                {
                    useUtilities.UpdatedbyUserId = HttpContext.Session.GetString("userid");
                    useUtilities.DateUpdated = DateTime.Now;
                    db.Entry(useUtilities).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), useUtilities.UtilitiesId.ToString(), "Update", useUtilities.UtilitiesId.ToString());

                }
                else
                {
                    useUtilities.AddedbyUserId = HttpContext.Session.GetString("userid");
                    useUtilities.DateAdded = DateTime.Now;
                    db.Add(useUtilities);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), useUtilities.UtilitiesId.ToString(), "Create", useUtilities.UtilitiesId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(useUtilities);
        }

        // GET: UseUtilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var useUtilities = await db.UseUtilities.FindAsync(id);
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", useUtilities.PrdUnitId);

            var products = db.Products.Where(p => new[] { 18, 19, 37, 39, 40, 41 }.Contains(p.CategoryId))
              .Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId });

            // ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", useUtilities.ProductId);
            ViewData["RawMaterialId"] = new SelectList(products, "Value", "Text", useUtilities.RawMaterialId);
            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", useUtilities.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", useUtilities.FiscalMonthId);


            if (useUtilities == null)
            {
                return NotFound();
            }
            return View("Create", useUtilities);
        }

        // GET: UseUtilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useUtilities = await db.UseUtilities
                .FirstOrDefaultAsync(m => m.UtilitiesId == id);

            if (useUtilities == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", useUtilities.PrdUnitId);
            var products = db.Products.Where(p => new[] { 18, 19, 37, 39, 40, 41 }.Contains(p.CategoryId))
              .Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId });

            //ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", useUtilities.ProductId);
            ViewData["RawMaterialId"] = new SelectList(products, "Value", "Text", useUtilities.RawMaterialId);
            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", useUtilities.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", useUtilities.FiscalMonthId);
            return View("Create", useUtilities);
        }

        // POST: UseUtilities/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var useUtilities = await db.UseUtilities.FindAsync(id);
                db.UseUtilities.Remove(useUtilities);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), useUtilities.UtilitiesId.ToString(), "Delete", useUtilities.UtilitiesId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, UtilitiesId = useUtilities.UtilitiesId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int UseUtilitiesId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.UseUtilities.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.UseUtilities.Any(d => d.DeptName == DeptName));
        //}


        private bool UseUtilitiesExists(int id)
        {
            return db.UseUtilities.Any(e => e.UtilitiesId == id);
        }
    }
}