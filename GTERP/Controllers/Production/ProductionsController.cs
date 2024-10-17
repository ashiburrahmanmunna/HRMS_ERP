using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GTERP.Controllers
{
    [OverridableAuthorize]
    public class ProductionsController : Controller
    {
        private readonly GTRDBContext _context;
        //public CommercialRepository Repository { get; set; }
        public ProductionsController(GTRDBContext context)
        {
            _context = context;
            //Repository = rep;
        }

        // GET: Productions
        public async Task<IActionResult> Index(DateTime? from, DateTime? to)
        {
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
            if (from != null && to != null)
            {
                var data = _context.Production.Include(p => p.YearName).Include(p => p.MonthName).Include(p => p.Unit).Include(p => p.Product).Where(p => p.ProductionDate >= from && p.ProductionDate <= to);
                return View(await data.ToListAsync());
            }
            else
            {
                var data = _context.Production.Include(p => p.YearName).Include(p => p.MonthName).Include(p => p.Unit).Include(p => p.Product).Where(p => p.ProductionDate.Date == DateTime.Now.Date);
                return View(await data.ToListAsync());
            }
        }

        public JsonResult GetMonthByYear(int FiscalYearId)
        {
            var MonthInfo = _context.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(y => y.FYId == FiscalYearId).Select(m => new { m.FiscalMonthId, m.MonthName }).ToList();
            return Json(MonthInfo);
        }

        // GET: Productions/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(p => p.PrdUnitId >= 3 && p.PrdUnitId <= 4), "PrdUnitId", "PrdUnitName");
            ViewData["ProductId"] = new SelectList(_context.Products.Take(10), "ProductId", "ProductName");
            var fiscalYear = _context.Acc_FiscalYears.Where(f => f.isRunning == true).FirstOrDefault();
            if (fiscalYear != null) ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
            else ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");

            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");

            // Previous data
            //var data = _context.Production.Where(p=>p.ProductionDate.Year==DateTime.Now.Year).OrderByDescending(p => p.ProductionDate).FirstOrDefault();
            //var prd= new Production();
            //if (data!=null)
            //{
            //    prd.MonthlySeedQty = data.MonthlySeedQty;
            //    prd.YearlySeedQty = data.YearlySeedQty;
            //    prd.MonthlyBagQty = data.MonthlyBagQty;
            //    prd.YearlyBagQty = data.YearlyBagQty;
            //    return View(prd);
            //}
            //else
            //{
            //    return View(prd);
            //}
            return View(new Production());
        }

        // POST: Productions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(Production production)
        {
            if (ModelState.IsValid)
            {
                production.userid = HttpContext.Session.GetString("userid");
                production.comid = HttpContext.Session.GetString("comid");

                var exist = _context.Production.Where(p => p.ProductionId != production.ProductionId && p.PrdUnitId == production.PrdUnitId && p.FiscalMonthId == production.FiscalMonthId && p.ProductionDate.Date == production.ProductionDate.Date).FirstOrDefault();
                if (exist != null)
                {
                    ViewBag.Title = "Edit";

                    ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(p => p.PrdUnitId == 3 || p.PrdUnitId == 4), "PrdUnitId", "PrdUnitName", exist.ProductId);
                    ViewData["ProductId"] = new SelectList(_context.Products.Take(1), "ProductId", "ProductName", exist.ProductId);
                    ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", exist.FiscalYearId);
                    ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", exist.FiscalMonthId);
                    TempData["Message"] = "Already already exist";
                    TempData["Status"] = "2";
                    return View("Create", exist);
                }

                var monthProd = _context.Production.Where(p => p.ProductionId != production.ProductionId
                                && p.PrdUnitId == production.PrdUnitId &&
                                p.FiscalMonthId == production.FiscalMonthId).OrderBy(p => p.ProductionDate).LastOrDefault();

                var yearProd = _context.Production.Where(p => p.ProductionId != production.ProductionId
                                && p.PrdUnitId == production.PrdUnitId &&
                                p.FiscalYearId == production.FiscalYearId).OrderBy(p => p.ProductionDate).LastOrDefault();



                if (monthProd != null)
                {
                    production.MonthlySeedQty = production.DailySeedQty + monthProd.MonthlySeedQty;
                    production.MonthlyBagQty = production.DailyBagQty + monthProd.MonthlyBagQty;
                    production.MonthlySalesSeedQty = production.DailySalesSeedQty + monthProd.MonthlySalesSeedQty;
                    production.MonthlySalesBagQty = production.DailySalesBagQty + monthProd.MonthlySalesBagQty;
                }
                else
                {
                    production.MonthlySeedQty = production.DailySeedQty;
                    production.MonthlyBagQty = production.DailyBagQty;
                    production.MonthlySalesSeedQty = production.DailySalesSeedQty;
                    production.MonthlySalesBagQty = production.DailySalesBagQty;
                }

                if (yearProd != null)
                {
                    production.YearlySeedQty = production.DailySeedQty + yearProd.YearlySeedQty;
                    production.YearlyBagQty = production.DailyBagQty + yearProd.YearlyBagQty;
                    production.YearlySalesSeedQty = production.DailySalesSeedQty + production.YearlySalesSeedQty;
                    production.YearlySalesBagQty = production.DailySalesBagQty + production.YearlySalesBagQty;
                }
                else
                {
                    production.YearlySeedQty = production.DailySeedQty;
                    production.YearlyBagQty = production.DailyBagQty;
                    production.YearlySalesSeedQty = production.DailySalesSeedQty;
                    production.YearlySalesBagQty = production.DailySalesBagQty;
                }

                var stock = _context.Production.Where(p => p.PrdUnitId == production.PrdUnitId
                                            && p.ProductionDate.Date < production.ProductionDate.Date)
                    .OrderByDescending(p => p.ProductionDate).FirstOrDefault();

                if (stock != null)
                {
                    production.ClosingSeedStock = stock.ClosingSeedStock + production.DailySeedQty - production.DailyBagQty;
                    production.ClosingBagStock = stock.ClosingBagStock + production.DailyBagQty - production.DailySalesBagQty;
                }
                else
                {
                    production.ClosingSeedStock = production.DailySeedQty - production.DailyBagQty;
                    production.ClosingBagStock = production.DailyBagQty - production.DailySalesBagQty;
                }

                //production.MonthlySeedQty = monthProd != null ? (production.DailySeedQty + monthProd.MonthlySeedQty) : production.DailySeedQty;
                //production.YearlySeedQty = yearProd != null ? (production.DailySeedQty + yearProd.YearlySeedQty) : production.DailySeedQty;



                //production.MonthlyBagQty = monthProd != null ? (production.DailyBagQty + monthProd.MonthlyBagQty) : production.DailyBagQty;
                //production.YearlyBagQty = yearProd != null ? (production.DailyBagQty + yearProd.YearlyBagQty) : production.DailyBagQty;

                if (production.ProductionId > 0)
                {
                    production.UpdatedbyUserId = HttpContext.Session.GetString("userid");
                    production.DateUpdated = DateTime.Now;

                    _context.Entry(production).State = EntityState.Modified;


                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), department.DeptId.ToString(), "Update", department.DeptName.ToString());

                }
                else
                {

                    production.DateAdded = DateTime.Now;
                    _context.Add(production);


                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), department.DeptId.ToString(), "Create", department.DeptName.ToString());

                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(production);
        }

        // for multi input
        //[HttpPost] 
        ////[ValidateAntiForgeryToken]
        //public IActionResult CreateOld(List<Production> productions)
        //{
        //    try
        //    {
        //        string comid = HttpContext.Session.GetString("comid");
        //        string userid = HttpContext.Session.GetString("userid");
        //        var errors = ModelState.Where(x => x.Value.Errors.Any())
        //       .Select(x => new { x.Key, x.Value.Errors });
        //        if (ModelState.IsValid)
        //        {

        //            foreach (var item in productions)
        //            {
        //                if (item.ProductionId > 0)
        //                {
        //                    item.UpdatedbyUserId = userid;
        //                    item.DateUpdated = DateTime.Now;
        //                    _context.Entry(item).State = EntityState.Modified;
        //                }
        //                else
        //                {
        //                    item.userid = userid;
        //                    item.comid = comid;
        //                    item.DateAdded = DateTime.Now;
        //                    item.AddedbyUserId = userid;
        //                    _context.Add(item);
        //                }
        //                _context.SaveChanges();
        //            }


        //            return Json(new { success = 1, ex = "Production data Save/Update Successfully" });
        //        }
        //        return Json(new { success = 2, ex = "Model State Not valid" });

        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { success = 3, ex = e.Message });
        //    }
        //}


        //[HttpPost]
        //public IActionResult LoadProduction(Production production)
        //{
        //    var exits = _context.Production
        //        .Where(p => p.ProductionDate.Date >= production.ProductionDate.Date &&
        //        p.ProductionDate.Date <= production.ProductionDate.Date).ToList();


        //    var datedif = (production.ProductionToDate - production.ProductionDate).Value.TotalDays+1;
        //    var currentDate = production.ProductionDate;

        //    var productions = new List<Production>();

        //    for (int i = 1; i <= datedif; i++)
        //    {
        //        var exist = _context.Production.Where(p => p.ProductionDate == currentDate && p.PrdUnitId==production.PrdUnitId).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            productions.Add(exist);
        //        }
        //        else
        //        {
        //            var prod = new Production();
        //            prod.ProductionId = 0;
        //            prod.PrdUnitId = production.PrdUnitId;
        //            prod.ProductId = production.ProductId;
        //            prod.ProductionDate = currentDate;
        //            prod.DailySeedQty = production.DailySeedQty;
        //            prod.DailyBagQty = production.DailyBagQty;
        //            prod.PhosphoricPerTon = production.PhosphoricPerTon;
        //            prod.AmmoniaPerTon = production.AmmoniaPerTon;
        //            prod.SulfuricPerTon = production.SulfuricPerTon;
        //            prod.SandPerTon = production.SandPerTon;
        //            productions.Add(prod);
        //        }
        //        currentDate= currentDate.AddDays(1);
        //    }

        //    return Json(productions);
        //}


        // GET: Productions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var production = await _context.Production.FindAsync(id);

            var nextData = _context.Production.Where(p => p.ProductionDate > production.ProductionDate).FirstOrDefault();
            if (nextData != null)
            {
                TempData["Message"] = "You can not Update this data ";
                TempData["Status"] = "2";
                return RedirectToAction(nameof(Index));
            }
            if (production == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(p => p.PrdUnitId >= 3 && p.PrdUnitId <= 4), "PrdUnitId", "PrdUnitName", production.PrdUnitId);
            ViewData["ProductId"] = new SelectList(_context.Products.Take(10), "ProductId", "ProductName", production.ProductId);

            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", production.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", production.FiscalMonthId);
            return View("Create", production);
        }

        // POST: Productions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Production production)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var errors = ModelState.Where(x => x.Value.Errors.Any())
           .Select(x => new { x.Key, x.Value.Errors });
            if (ModelState.IsValid)
            {
                if (production.ProductionId > 0)
                {
                    production.UpdatedbyUserId = userid;
                    production.DateUpdated = DateTime.Now;
                    _context.Entry(production).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "1";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits, "PrdUnitId", "PrdUnitName", production.PrdUnitId);
            ViewData["ProductId"] = new SelectList(_context.Products.Take(10), "ProductId", "ProductName", production.ProductId);

            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", production.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", production.FiscalMonthId);
            return View(production);
        }

        // GET: Productions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var production = await _context.Production
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.ProductionId == id);
            var nextData = _context.Production.Where(p => p.ProductionDate > production.ProductionDate).FirstOrDefault();
            if (nextData != null)
            {
                TempData["Message"] = "You can not Update this data ";
                TempData["Status"] = "2";
                return RedirectToAction(nameof(Index));
            }
            if (production == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits, "PrdUnitId", "PrdUnitName", production.PrdUnitId);
            ViewData["ProductId"] = new SelectList(_context.Products.Take(10), "ProductId", "ProductName", production.ProductId);

            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", production.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", production.FiscalMonthId);
            return View("Edit", production);
        }

        // POST: Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var production = await _context.Production.FindAsync(id);
                _context.Production.Remove(production);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Busstop.BusStopId.ToString(), "Delete", cat_Busstop.BusStopName);
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ProductionId = production.ProductionId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //private List<Production> Calculation(Production prd, string action)
        //{
        //    var productions = new List<Production>();
        //    var fDayOfYear = new DateTime(prd.ProductionDate.Year, 1, 1); // firs date of production year
        //    var fDayOfMonth = new DateTime(prd.ProductionDate.Year, prd.ProductionDate.Month, 1); // firs date of production month

        //    if (action == "Create")
        //    {
        //        if (prd.ProductionDate.Date == fDayOfYear.Date)
        //        {

        //        }
        //        else
        //        {
        //            var pre = _context.Production
        //            .Where(p => p.ProductionDate.Date == prd.ProductionDate.Date.AddDays(-1)).FirstOrDefault();

        //            if (pre != null && pre.ProductionDate.Date >= fDayOfYear.Date)
        //            {
        //                prd.MonthlySeedQty += pre.MonthlySeedQty;
        //                prd.YearlySeedQty += pre.YearlySeedQty;

        //                prd.MonthlyBagQty += pre.MonthlyBagQty;
        //                prd.YearlyBagQty += pre.YearlyBagQty;
        //            }
        //        }



        //    }
        //    else if(action == "Update")
        //    {

        //    }
        //    else if(action == "Delete")
        //    {

        //    }

        //    return new List<Production>();
        //}

        private bool ProductionExists(int id)
        {
            return _context.Production.Any(e => e.ProductionId == id);
        }

        public class ProductionData
        {
            public float? MonthlySeedQty { get; set; } = 0;
            public float? YearlySeedQty { get; set; } = 0;
            public float? MonthlyBagQty { get; set; } = 0;
            public float? YearlyBagQty { get; set; } = 0;

            public float? MonthlySalesSeedQty { get; set; } = 0;
            public float? YearlySalesSeedQty { get; set; } = 0;
            public float? MonthlySalesBagQty { get; set; } = 0;
            public float? YearlySalesBagQty { get; set; } = 0;

            public float? ClosingSeedStock { get; set; } = 0;
            public float? ClosingBagStock { get; set; } = 0;



        }

        public IActionResult GetPreviousInfo(int unitId, int monthId, int yearId)
        {
            var month = _context.Production.Where(p => p.PrdUnitId == unitId && p.FiscalMonthId == monthId).OrderByDescending(p => p.ProductionDate).FirstOrDefault();
            var year = _context.Production.Where(p => p.PrdUnitId == unitId && p.FiscalYearId == yearId).OrderByDescending(p => p.ProductionDate).FirstOrDefault();
            var stock = _context.Production.Where(p => p.PrdUnitId == unitId).OrderByDescending(p => p.ProductionDate).FirstOrDefault();
            var data = new ProductionData();

            if (month != null)
            {
                data.MonthlySeedQty = month.MonthlySeedQty;
                data.MonthlyBagQty = month.MonthlyBagQty;
                data.MonthlySalesSeedQty = month.MonthlySalesSeedQty;
                data.MonthlySalesBagQty = month.MonthlySalesBagQty;


            }

            if (year != null)
            {
                data.YearlySeedQty = year.YearlySeedQty;
                data.YearlyBagQty = year.YearlyBagQty;
                data.YearlySalesSeedQty = year.YearlySalesSeedQty;
                data.YearlySalesBagQty = year.YearlySalesBagQty;
            }

            if (stock != null)
            {
                data.ClosingSeedStock = stock.ClosingSeedStock;
                data.ClosingBagStock = stock.ClosingBagStock;
            }

            //.Where(p => p.PrdUnitId==unitId && p.FiscalMonthId==monthId && p.FiscalYearId==yearId).OrderByDescending(p=>p.PrdutionDate).LastOrDefault();
            return Json(data);
        }

        public static List<SelectListItem> ReportTypes = new List<SelectListItem>()
        {
            new SelectListItem() {Text="Daily Production Sales & Store Report", Value="rptDailyProduction"},
            new SelectListItem() {Text="Fax Report", Value="rptFaxReport"},
            new SelectListItem() {Text="MIS Report", Value="rptMISReport"},
            new SelectListItem() {Text="Down Time Report", Value="rptDownTimeUnit"},
            new SelectListItem() {Text="Usage Ration Report", Value="rptURationReport"},
            new SelectListItem() {Text="Production Report", Value="rptProduction"},
            new SelectListItem() {Text="Sales Report", Value="rptSales"}
        };

        public ActionResult Report()
        {
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(p => p.PrdUnitId == 3 || p.PrdUnitId == 4), "PrdUnitId", "PrdUnitName");
            ViewData["ProductId"] = new SelectList(_context.Products.Take(10), "ProductId", "ProductName");
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
            ViewBag.ReportType = new SelectList(ReportTypes, "Value", "Text");

            return View();
        }
        public JsonResult ProductionReport(string rptFormat, string reportType, string yearId, string monthid, string productionDate, string fromDate, string toDate, string prdUnitId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                if (reportType == "rptDailyProduction")
                {
                    reportname = "rptDailyProductionSalesAndStore";
                    filename = "rptDailyProductionSalesAndStore_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptDailyProductioSalesAndStore '" + comid + "', '" + productionDate + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptFaxReport")
                {
                    reportname = "rptFaxReport";
                    filename = "rptFaxReport_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptFaxReport '" + comid + "', '" + productionDate + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptMISReport")
                {
                    reportname = "rptMISReport";
                    filename = "rptMISReport_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptMISReport '" + comid + "', '" + yearId + "', '" + monthid + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptDownTimeUnit" && prdUnitId == "3")
                {
                    reportname = "rptDownTimeUnit1";
                    filename = "rptDownTimeUnit1_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptDownTimeReport '" + comid + "','" + prdUnitId + "','" + yearId + "', '" + monthid + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptDownTimeUnit" && prdUnitId == "4")
                {
                    reportname = "rptDownTimeUnit2";
                    filename = "rptDownTimeUnit2_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptDownTimeReport '" + comid + "','" + prdUnitId + "','" + yearId + "', '" + monthid + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptURationReport")
                {
                    if (prdUnitId == "3")
                    {
                        reportname = "rptURationReportUnit1";
                        filename = "rptURationReport_List" + DateTime.Now.Date.ToString();
                        var query = "Exec Production_rptURation '" + comid + "', '" + prdUnitId + "', '" + yearId + "', '" + monthid + "'";
                        HttpContext.Session.SetString("reportquery", query);
                        HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                    }
                    else if (prdUnitId == "4")
                    {
                        reportname = "rptURationReportUnit2";
                        filename = "rptURationReport_List" + DateTime.Now.Date.ToString();
                        var query = "Exec Production_rptURation '" + comid + "', '" + prdUnitId + "', '" + yearId + "', '" + monthid + "'";
                        HttpContext.Session.SetString("reportquery", query);
                        HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                    }
                    else
                    {
                        reportname = "rptURationReport";
                        filename = "rptURationReport_List" + DateTime.Now.Date.ToString();
                        var query = "Exec Production_rptURation '" + comid + "', '" + prdUnitId + "', '" + yearId + "', '" + monthid + "'";
                        HttpContext.Session.SetString("reportquery", query);
                        HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                    }

                }
                else if (reportType == "rptProduction")
                {
                    reportname = "rptProduction";
                    filename = "rptProduction_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptProduction '" + comid + "', '" + yearId + "', '" + monthid + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptSales")
                {
                    reportname = "rptSales";
                    filename = "rptSales_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Production_rptSales '" + comid + "', '" + yearId + "', '" + monthid + "', '" + productionDate + "'";
                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Production/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }

                string DataSourceName = "DataSet1";
                GTERP.Models.Common.clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                GTERP.Models.Common.clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                GTERP.Models.Common.clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                ////string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;



                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }


                return Json(new { Url = redirectUrl });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
