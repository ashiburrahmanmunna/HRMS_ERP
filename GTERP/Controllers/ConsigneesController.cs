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
    public class ConsigneesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;
        public ConsigneesController()
        {
            //UserLog = new //UserLog();
        }

        // GET: Consignees
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            return View(db.Consignees.ToList());
        }



        // GET: Consignees/Create
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");


            return View();
        }

        // POST: Consignees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ConsigneeId,ConsigneeName,CountryId,Code,BranchAddress,PhoneNo,Remarks")] Consignee Consignee)
        {
            try
            {


                if (Consignee.ConsigneeId > 0)
                {
                    Consignee.DateUpdated = DateTime.Now;
                    Consignee.DateAdded = DateTime.Now;
                    Consignee.userid = HttpContext.Session.GetString("userid");
                    Consignee.comid = int.Parse(AppData.intComId);
                    db.Entry(Consignee).State = EntityState.Modified;
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Consignee.ConsigneeId.ToString(), "Update");

                }
                else
                {
                    Consignee.DateAdded = DateTime.Now;
                    Consignee.DateUpdated = DateTime.Now;
                    Consignee.userid = HttpContext.Session.GetString("userid");
                    Consignee.comid = int.Parse(AppData.intComId);
                    db.Consignees.Add(Consignee);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Consignee.ConsigneeId.ToString(), "Create");

                }
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: Consignees/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            Consignee Consignee = db.Consignees.Find(id);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", Consignee.CountryId);

            if (Consignee == null)
            {
                return NotFound();
            }
            return View("Create", Consignee);
        }

        // POST: Consignees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(/*Include =*/ "ConsigneeId,ConsigneeName,CountryId,Code,BranchAddress,PhoneNo,Remarks")] Consignee Consignee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(Consignee).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(Consignee);
        //}

        // GET: Consignees/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            Consignee Consignee = db.Consignees.Find(id);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", Consignee.CountryId);
            if (Consignee == null)
            {
                return NotFound();
            }
            return View("Create", Consignee);
        }

        // POST: Consignees/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Consignee Consignee = db.Consignees.Find(id);
                db.Consignees.Remove(Consignee);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, id = Consignee.ConsigneeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() }); ;
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
