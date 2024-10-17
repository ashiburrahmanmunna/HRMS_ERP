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
    public class NotifyPartysController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;
        public NotifyPartysController()
        {
            //UserLog = new //UserLog();
        }

        // GET: NotifyPartys
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            var NotifyPartys = db.NotifyPartys.ToList();
            return View(NotifyPartys);
        }




        // GET: NotifyPartys/Create
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName");
            return View();
        }

        // POST: NotifyPartys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "NotifyPartyId,NotifyPartyName,NotifyPartyNameSearch,Address1,Address2,PhoneNo,Email,BuyerId,CountryId,PortOfDischargeId,ShopCode,ShippedTo,comid,userid,DateAdded,UpdateByUserId,Dateupdated")] NotifyParty notifyParty)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    if (notifyParty.NotifyPartyId > 0)
                    {
                        db.Entry(notifyParty).State = EntityState.Modified;
                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), notifyParty.NotifyPartyId.ToString(), "Update");
                    }
                    else
                    {
                        db.NotifyPartys.Add(notifyParty);
                        db.SaveChanges();
                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), notifyParty.NotifyPartyId.ToString(), "Create");
                    }

                     db.SaveChanges();
                    return RedirectToAction("Index");
                //}
            }
            catch (Exception ex)
            {
                ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", notifyParty.BuyerId);
                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", notifyParty.CountryId);
                ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", notifyParty.PortOfDischargeId);
                TempData["Message"] = "Unable to Save / Update";
                notifyParty.NotifyPartyId= 0;
                TempData["Status"] = "0";
                throw ex;

       
            }

            return View(notifyParty);



        }

        // GET: NotifyPartys/Edit/5
        //[OverridableAuthorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            NotifyParty notifyParty = db.NotifyPartys.Find(id);
            if (notifyParty == null)
            {
                return NotFound();
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", notifyParty.BuyerId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", notifyParty.CountryId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", notifyParty.PortOfDischargeId);
            return View("Create", notifyParty);
        }

        // POST: NotifyPartys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(/*Include =*/ "NotifyPartyId,NotifyPartyName,Address1,Address2,PhoneNo,Email,BuyerId,CountryId,PortOfDischargeId,ShopCode,ShippedTo,comid,userid,DateAdded,UpdateByUserId,Dateupdated")] NotifyParty notifyParty)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(notifyParty).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", notifyParty.BuyerId);
        //    ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", notifyParty.CountryId);
        //    ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", notifyParty.PortOfDischargeId);
        //    return View(notifyParty);
        //}

        // GET: NotifyPartys/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            NotifyParty notifyParty = db.NotifyPartys.Find(id);
            if (notifyParty == null)
            {
                return NotFound();
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", notifyParty.BuyerId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", notifyParty.CountryId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", notifyParty.PortOfDischargeId);

            return View("Create",notifyParty);
        }

        // POST: NotifyPartys/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(long id)
        {

            try
            {
                NotifyParty notifyParty = db.NotifyPartys.Find(id);
                db.NotifyPartys.Remove(notifyParty);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, ContactID = notifyParty.NotifyPartyId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
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
