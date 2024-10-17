using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ProductCategoriesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;
        public ProductCategoriesController()
        {
            //UserLog = new //UserLog();
        }

        // GET: ProductCategories
        public ActionResult Index()
        {
            return View(db.ProductCategory.ToList());
        }



        // GET: ProductCategories/Create
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ProductCategoryId,CategoryName")] ProductCategory ProductCategorys)
        {
            try
            {

            //if (ModelState.IsValid)
            //{
                if (ProductCategorys.ProductCategoryId > 0)
                {
                    ProductCategorys.DateAdded = DateTime.Now;
                    ProductCategorys.DateUpdated = DateTime.Now;

                    //ProductCategory.comid = int.Parse(AppData.intComId.ToString());
                    //ProductCategory.userid = HttpContext.Session.GetString("userid");

                    db.Entry(ProductCategorys).State = EntityState.Modified;
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ProductCategorys.ProductCategoryId.ToString(), "Update");

                }
                else
                {
                    ProductCategorys.DateAdded = DateTime.Now;
                    ProductCategorys.DateUpdated = DateTime.Now;
                    //ProductCategory.comid = int.Parse(AppData.intComId.ToString());
                    //ProductCategory.userid = HttpContext.Session.GetString("userid");
                    db.ProductCategory.Add(ProductCategorys);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ProductCategorys.ProductCategoryId.ToString(), "Create");
                }
                db.SaveChanges();

                return RedirectToAction("Index");
                //}

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                TempData["Status"] = "0";
                throw ex;
                return View(ProductCategorys);

            }
        }

        // GET: ProductCategories/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";

            ProductCategory ProductCategory = db.ProductCategory.Find(id);
            if (ProductCategory == null)
            {
                return NotFound();
            }
            return View("Create", ProductCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(/*Include =*/ "ProductCategoryId,CategoryName")] ProductCategory ProductCategory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ProductCategory).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(ProductCategory);
        //}

        // GET: ProductCategories/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            ProductCategory ProductCategory = db.ProductCategory.Find(id);
            if (ProductCategory == null)
            {
                return NotFound();
            }
            return View("Create", ProductCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ProductCategory ProductCategory = db.ProductCategory.Find(id);
                db.ProductCategory.Remove(ProductCategory);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");

                return Json(new { Success = 1, id = ProductCategory.ProductCategoryId, ex = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
