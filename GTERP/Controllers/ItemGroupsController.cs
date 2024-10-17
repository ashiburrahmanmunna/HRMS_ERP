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
    public class ItemGroupsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;

        public ItemGroupsController()
        {
            //UserLog = new //UserLog();

        }

        // GET: ItemGroups
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            return View(db.ItemGroups.ToList());
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        // GET: ItemGroups/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ItemGroupId,ItemGroupCode,ItemGroupHSCode,ItemGroupName,ItemGroupShortName,ItemMargin")] ItemGroup itemGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (itemGroup.ItemGroupId > 0)
                    {
                        ViewBag.Title = "Edit";
                        itemGroup.comid = int.Parse(AppData.intComId);

                        db.Entry(itemGroup).State = EntityState.Modified;

                        TempData["Message"] = "Data Update Successfully";

                        db.SaveChanges();
                        TempData["Status"] = "2";

                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), itemGroup.ItemGroupId.ToString(), "Update");


                    }
                    else
                    {
                        ViewBag.Title = "Create";

                        itemGroup.comid = int.Parse(AppData.intComId);

                        db.ItemGroups.Add(itemGroup);
                        TempData["Message"] = "Data Save Successfully";

                        db.SaveChanges();


                        TempData["Status"] = "1";
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), itemGroup.ItemGroupId.ToString(), "Create");




                    }
                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                TempData["Message"] = "Unable to Save / Update";
                itemGroup.ItemGroupId = 0;
                TempData["Status"] = "0";

                return View(itemGroup);
                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        // GET: ItemGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return BadRequest();
            }
            ItemGroup itemGroup = db.ItemGroups.Find(id);
            if (itemGroup == null)
            {
                return NotFound();
            }
            return View("Create", itemGroup);

        }



        // GET: ItemGroups/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return BadRequest();
            }
            ItemGroup itemGroup = db.ItemGroups.Find(id);
            if (itemGroup == null)
            {
                return NotFound();
            }

            return View("Create", itemGroup);
        }

        // POST: ItemGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ItemGroup itemGroup = db.ItemGroups.Find(id);
                db.ItemGroups.Remove(itemGroup);
                db.SaveChanges();
                return Json(new { Success = 1, id = itemGroup.ItemGroupId, ex = "Data Delete Successfully" });
            }
            catch (Exception ex)
            {

                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";

                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });

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
