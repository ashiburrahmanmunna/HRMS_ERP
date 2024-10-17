using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{

    [OverridableAuthorize]
    public class ReorderLevelController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public ReorderLevelController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }



        // GET: ReorderLevel
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";

            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@ComId", comid);
            var data = Helper.ExecProcMapTList<ReorderLevelViewModel>("prcGet_ReorderLevel", parameter);


            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(data);
        }


        // GET: ReorderLevel/Create
        public IActionResult Create()
        {

            ViewBag.Title = "Create";
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.WareHouseId = new SelectList(db.Warehouses.Where(x => x.ComId == comid && x.IsSubWarehouse == true && x.IsMedicalWarehouse == true), "WarehouseId", "WhName");
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.comid == comid && x.vPrimaryCategory.CategoryCode.Contains("Medical")).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
            return View();
        }

        // POST: ReorderLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("ReorderLevelId,ComId,DeptCode,DeptName,Pcname,LuserId,DeptBangla,Slno,ParentId,Buid,Buname,DptSrtName,IsStrDpt")]*/ ReorderLevel ReorderLevel)
        {
            if (ModelState.IsValid)
            {
                ReorderLevel.UserId = HttpContext.Session.GetString("userid");
                ReorderLevel.ComId = HttpContext.Session.GetString("comid");
                if (ReorderLevel.ReorderLevelId > 0)
                {
                    db.Entry(ReorderLevel).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ReorderLevel.ReorderLevelId.ToString(), "Update", ReorderLevel.ProductId.ToString() + ReorderLevel.WarehouseId.ToString());

                }
                else
                {
                    db.Add(ReorderLevel);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ReorderLevel.ProductId.ToString() + ReorderLevel.WarehouseId.ToString(), "Create", ReorderLevel.ProductId.ToString() + ReorderLevel.WarehouseId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(ReorderLevel);
        }

        // GET: ReorderLevel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            string comid = HttpContext.Session.GetString("comid");
            var ReorderLevel = await db.ReorderLevel.FindAsync(id);
            //ViewBag.ReorderLevelId = new SelectList(db.ReorderLevel, "ReorderLevelId", "DeptName", ReorderLevel.ParentId);
            if (ReorderLevel == null)
            {
                return NotFound();
            }

            ViewBag.WareHouseId = new SelectList(db.Warehouses.Where(x => x.ComId == comid && x.IsSubWarehouse == true && x.IsMedicalWarehouse == true), "WarehouseId", "WhName", ReorderLevel.WarehouseId);
            //ViewBag.ProductId = new SelectList(db.Products.Where(x => x.comid == comid), "ProductId", "ProductName", ReorderLevel.ProductId);
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.comid == comid && x.vPrimaryCategory.CategoryCode.Contains("Medical")).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text", ReorderLevel.ProductId);
            return View("Create", ReorderLevel);
        }

        // GET: ReorderLevel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string comid = HttpContext.Session.GetString("comid");
            var ReorderLevel = await db.ReorderLevel
                .FirstOrDefaultAsync(m => m.ReorderLevelId == id);

            if (ReorderLevel == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            ViewBag.WareHouseId = new SelectList(db.Warehouses.Where(x => x.ComId == comid && x.IsSubWarehouse == true && x.IsMedicalWarehouse == true), "WarehouseId", "WhName", ReorderLevel.WarehouseId);
            //ViewBag.ProductId = new SelectList(db.Products.Where(x => x.comid == comid), "ProductId", "ProductName", ReorderLevel.ProductId);
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.comid == comid && x.vPrimaryCategory.CategoryCode.Contains("Medical")).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text", ReorderLevel.ProductId);
            //ViewBag.ReorderLevelId = new SelectList(db.ReorderLevel, "ReorderLevelId", "DeptName", ReorderLevel.ParentId);
            return View("Create", ReorderLevel);
        }

        // POST: ReorderLevel/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var ReorderLevel = await db.ReorderLevel.FindAsync(id);
                db.ReorderLevel.Remove(ReorderLevel);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ReorderLevel.ProductId.ToString() + ReorderLevel.WarehouseId.ToString(), "Delete", ReorderLevel.ProductId.ToString() + ReorderLevel.WarehouseId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ReorderLevelId = ReorderLevel.ReorderLevelId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int ReorderLevelId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.ReorderLevel.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.ReorderLevel.Any(d => d.DeptName == DeptName));
        //}


        private bool ReorderLevelExists(int id)
        {
            return db.ReorderLevel.Any(e => e.ReorderLevelId == id);
        }
    }
}