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
    public class ProductGroupsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;
        public ProductGroupsController()
        {
            //UserLog = new //UserLog();
        }

        // GET: ProductGroups
        public ActionResult Index()
        {
            return View(db.ProductGroup.ToList());
        }



        // GET: ProductGroups/Create
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        // POST: ProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ProductGroupId,ProductGroupName")] ProductGroup ProductGroups)
        {
            try
            {

                //if (ModelState.IsValid)
                //{
                if (ProductGroups.ProductGroupId > 0)
                {
                    ProductGroups.DateAdded = DateTime.Now;
                    //ProductGroups.DateUpdated = DateTime.Now;

                    //ProductGroup.comid = int.Parse(AppData.intComId.ToString());
                    //ProductGroup.userid = HttpContext.Session.GetString("userid");

                    db.Entry(ProductGroups).State = EntityState.Modified;
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ProductGroups.ProductGroupId.ToString(), "Update");

                }
                else
                {
                    ProductGroups.DateAdded = DateTime.Now;
                    //ProductGroups.DateUpdated = DateTime.Now;
                    //ProductGroup.comid = int.Parse(AppData.intComId.ToString());
                    //ProductGroup.userid = HttpContext.Session.GetString("userid");
                    db.ProductGroup.Add(ProductGroups);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ProductGroups.ProductGroupId.ToString(), "Create");
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
                return View(ProductGroups);

            }
        }

        // GET: ProductGroups/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";

            ProductGroup ProductGroup = db.ProductGroup.Find(id);
            if (ProductGroup == null)
            {
                return NotFound();
            }
            return View("Create", ProductGroup);
        }

        // POST: ProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "ProductGroupId,ProductGroupName")] ProductGroup ProductGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ProductGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ProductGroup);
        }

        // GET: ProductGroups/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            ProductGroup ProductGroup = db.ProductGroup.Find(id);
            if (ProductGroup == null)
            {
                return NotFound();
            }
            return View("Create", ProductGroup);
        }

        // POST: ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ProductGroup ProductGroup = db.ProductGroup.Find(id);
                db.ProductGroup.Remove(ProductGroup);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, id = ProductGroup.ProductGroupId, ex = TempData["Message"].ToString() });

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
