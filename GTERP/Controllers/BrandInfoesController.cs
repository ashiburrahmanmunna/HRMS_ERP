using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;
using GTERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class BrandInfoesController : Controller
    {
        private GTRDBContext db;
        public BrandInfoesController(GTRDBContext context)
        {
            db = context;
        }
        //UserLog //UserLog;
        public BrandInfoesController()
        {
            //UserLog = new //UserLog();
        }

        // GET: BrandInfoes
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            return View(db.BrandInfo.ToList());
        }



        // GET: BrandInfoes/Create
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        // POST: BrandInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*/*Include =*/*/ "BrandInfoId,BrandName")] BrandInfo BrandInfos)
        {
            try
            {

                //if (ModelState.IsValid)
                //{
                if (BrandInfos.BrandInfoId > 0)
                {
                    BrandInfos.DateAdded = DateTime.Now;
                    //BrandInfos.DateUpdated = DateTime.Now;

                    //BrandInfo.comid = int.Parse(AppData.intComId.ToString());
                    //BrandInfo.userid = HttpContext.Session.GetString("userid");

                    db.Entry(BrandInfos).State = EntityState.Modified;

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), BrandInfos.BrandInfoId.ToString(), "Update");

                }
                else
                {
                    BrandInfos.DateAdded = DateTime.Now;
                    //BrandInfos.DateUpdated = DateTime.Now;
                    //BrandInfo.comid = int.Parse(AppData.intComId.ToString());
                    //BrandInfo.userid = HttpContext.Session.GetString("userid");
                    db.BrandInfo.Add(BrandInfos);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), BrandInfos.BrandInfoId.ToString(), "Create");
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
                return View(BrandInfos);

            }
        }

        // GET: BrandInfoes/Edit/5
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";

            BrandInfo BrandInfo = db.BrandInfo.Find(id);
            if (BrandInfo == null)
            {
                return NotFound();
            }
            return View("Create", BrandInfo);
        }

        // POST: BrandInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(/*Include =*/ "BrandInfoId,BrandName")] BrandInfo BrandInfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(BrandInfo).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(BrandInfo);
        //}

        // GET: BrandInfoes/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            BrandInfo BrandInfo = db.BrandInfo.Find(id);
            if (BrandInfo == null)
            {
                return NotFound();
            }
            return View("Create", BrandInfo);
        }

        // POST: BrandInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BrandInfo BrandInfo = db.BrandInfo.Find(id);
                db.BrandInfo.Remove(BrandInfo);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, id = BrandInfo.BrandInfoId, ex = TempData["Message"].ToString() });

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
