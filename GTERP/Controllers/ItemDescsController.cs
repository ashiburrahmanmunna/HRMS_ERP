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
    public class ItemDescsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;

        public ItemDescsController()
        {
            //UserLog = new //UserLog();

        }

        // GET: ItemDescs
        public ActionResult Index()
        {
            return View(db.ItemDescs.ToList());
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        // GET: ItemDescs/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            var comid = int.Parse(AppData.intComId);
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups.Where(m => m.comid == comid), "ItemGroupId", "ItemGroupName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ItemDescId,ItemDescCode,ItemDescHSCode,ItemDescName,ItemDescshortName,ItemGroupId")] ItemDesc ItemDesc)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (ItemDesc.ItemDescId > 0)
                    {
                        ViewBag.Title = "Edit";
                        ItemDesc.comid = int.Parse(AppData.intComId);

                        db.Entry(ItemDesc).State = EntityState.Modified;

                        TempData["Message"] = "Data Update Successfully";

                        db.SaveChanges();
                        TempData["Status"] = "2";

                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ItemDesc.ItemDescId.ToString(), "Update");


                    }
                    else
                    {
                        ViewBag.Title = "Create";

                        ItemDesc.comid = int.Parse(AppData.intComId);

                        db.ItemDescs.Add(ItemDesc);
                        TempData["Message"] = "Data Save Successfully";

                        db.SaveChanges();


                        TempData["Status"] = "1";
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ItemDesc.ItemDescId.ToString(), "Create");




                    }
                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                TempData["Message"] = "Unable to Save / Update";
                ItemDesc.ItemDescId = 0;
                TempData["Status"] = "0";

                return View(ItemDesc);
                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        // GET: ItemDescs/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return BadRequest();
            }
            ItemDesc ItemDesc = db.ItemDescs.Find(id);
            if (ItemDesc == null)
            {
                return NotFound();
            }
            var comid = int.Parse(AppData.intComId);
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups.Where(m => m.comid == comid), "ItemGroupId", "ItemGroupName", ItemDesc.ItemGroupId);

            return View("Create", ItemDesc);

        }



        // GET: ItemDescs/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return BadRequest();
            }
            ItemDesc ItemDesc = db.ItemDescs.Find(id);
            if (ItemDesc == null)
            {
                return NotFound();
            }
            var comid = int.Parse(AppData.intComId);

            ViewBag.ItemGroupId = new SelectList(db.ItemGroups.Where(m => m.comid == comid), "ItemGroupId", "ItemGroupName", ItemDesc.ItemGroupId);

            return View("Create", ItemDesc);
        }

        // POST: ItemDescs/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ItemDesc ItemDesc = db.ItemDescs.Find(id);
                db.ItemDescs.Remove(ItemDesc);
                db.SaveChanges();
                return Json(new { Success = 1, id = ItemDesc.ItemDescId, ex = "Data Delete Successfully" });
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
