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
    //[OverridableAuthorize]
    public class RawMaterialReceiveController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public RawMaterialReceiveController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: RawMaterialStock
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(await db.RawMaterialStock.Include(p => p.YearName).Include(p => p.MonthName).Include(p => p.Warehouse).Include(p => p.Product).ToListAsync());
        }



        // GET: RawMaterialStock/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["WarehouseId"] = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            ViewData["ReceiveWarehouseId"] = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName");

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");

            return View();
        }

        // POST: RawMaterialStock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RawMaterialStock material)
        {
            if (ModelState.IsValid)
            {
                //var exist = await db.RawMaterialStock
                //    .Where(p => p.StockId != material.StockId
                //    && p.FiscalYearId == material.FiscalYearId && p.FiscalMonthId==material.FiscalMonthId).FirstOrDefaultAsync();
                //if (exist != null)
                //{
                //    TempData["Message"] = "Data Already Exist";
                //    TempData["Status"] = "2";
                //    ViewBag.Title = "Edit";
                //    //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName", material.PrdUnitId);
                //    ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", material.ProductId);

                //    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", material.FiscalYearId);
                //    ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", material.FiscalMonthId);

                //    return View("Create", exist);
                //}
                material.UserId = HttpContext.Session.GetString("userid");
                material.ComId = HttpContext.Session.GetString("comid");

                var preStock = await db.RawMaterialStock.Where(s => s.WarehouseId == material.WarehouseId && s.ProductId == material.ProductId).FirstOrDefaultAsync();

                material.StockQty = preStock != null ? (float)preStock.StockQty + material.ReceiveQty : material.ReceiveQty;

                if (material.StockId > 0)
                {

                    db.Entry(material).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), material.StockId.ToString(), "Update", material.StockId.ToString());

                }
                else
                {
                    db.Add(material);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), material.StockId.ToString(), "Create", material.StockId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: RawMaterialStock/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var material = await db.RawMaterialStock.FindAsync(id);
            ViewData["WarehouseId"] = new SelectList(db.Warehouses, "WarehouseId", "WhName", material.WarehouseId);
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName", material.PrdUnitId);
            ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", material.ProductId);

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", material.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", material.FiscalMonthId);

            if (material == null)
            {
                return NotFound();
            }
            return View("Create", material);
        }

        // GET: RawMaterialStock/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await db.RawMaterialStock
                .FirstOrDefaultAsync(m => m.StockId == id);

            if (material == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewData["WarehouseId"] = new SelectList(db.Warehouses, "WarehouseId", "WhName", material.WarehouseId);
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName", material.PrdUnitId);
            ViewData["ProductId"] = new SelectList(db.Products.Take(10), "ProductId", "ProductName", material.ProductId);

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", material.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", material.FiscalMonthId);

            return View("Create", material);
        }

        // POST: RawMaterialStock/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var material = await db.RawMaterialStock.FindAsync(id);
                db.RawMaterialStock.Remove(material);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), material.StockId.ToString(), "Delete", material.StockId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, StockId = material.StockId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int RawMaterialStockId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.RawMaterialStock.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.RawMaterialStock.Any(d => d.DeptName == DeptName));
        //}

        public IActionResult GetStcok(int warehouseId, int productId)
        {
            var data = db.RawMaterialStock.Where(r => r.WarehouseId == warehouseId && r.ProductId == productId).FirstOrDefault();
            var stock = data != null ? data.StockQty : 0;
            return Json(stock);
        }


        private bool RawMaterialStockExists(int id)
        {
            return db.RawMaterialStock.Any(e => e.StockId == id);
        }
    }
}