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
    public class SupplierInformationsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;
        public SupplierInformationsController()
        {
            //UserLog = new //UserLog();
        }

        // GET: SupplierInformations
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            var supplierInformations = db.SupplierInformations.Include(s => s.Country);
            return View(supplierInformations.ToList());
        }



        // GET: SupplierInformations/Create
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View();
        }

        // POST: SupplierInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ContactID,SupplierName,CountryId,ContactPerson,Address,Address2,LocalOffice,Phone,Fax,Email,Addedby,Dateadded,Updatedby,Dateupdated , Merchandiser")] SupplierInformation supplierInformation)
        {
            try
            {

      
            if (ModelState.IsValid)
            {
                    if (supplierInformation.ContactID > 0)
                    {
                        supplierInformation.userid = HttpContext.Session.GetString("userid");
                        supplierInformation.DateUpdated = DateTime.Now;
                        supplierInformation.comid = int.Parse(AppData.intComId);
                        db.Entry(supplierInformation).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), supplierInformation.ContactID.ToString(), "Update");
                    }
                    else
                    {
                        supplierInformation.userid = HttpContext.Session.GetString("userid");
                        supplierInformation.comid = int.Parse(AppData.intComId);
                        supplierInformation.DateAdded = DateTime.Now;
                        supplierInformation.DateUpdated = DateTime.Now;

                        db.SupplierInformations.Add(supplierInformation);
                        db.SaveChanges();
                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), supplierInformation.ContactID.ToString(), "Create");
                     }

                    return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryCode", supplierInformation.CountryId);
            return View(supplierInformation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: SupplierInformations/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            //int ContactID = int.Parse(id);

            SupplierInformation supplierInformation = db.SupplierInformations.Where(m => m.ContactID.ToString() == id.ToString()).FirstOrDefault(); //Find(id);// 
            if (supplierInformation == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplierInformation.CountryId);
            return View("Create", supplierInformation);
        }

        // POST: SupplierInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OverridableAuthorize]
        public ActionResult Edit([Bind(/*Include =*/ "ContactID,SupplierName,CountryId,ContactPerson,Address,LocalOffice,Phone,Fax,Email,Addedby,Dateadded,Updatedby,Dateupdated")] SupplierInformation supplierInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplierInformation.CountryId);
            return View(supplierInformation);
        }

        // GET: SupplierInformations/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            SupplierInformation supplierInformation = db.SupplierInformations.Where(m => m.ContactID.ToString() == id.ToString()).FirstOrDefault(); //Find(id);//
            if (supplierInformation == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplierInformation.CountryId);
            return View("Create", supplierInformation);
        }

        // POST: SupplierInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {

                SupplierInformation supplierInformation = db.SupplierInformations.Where(m => m.ContactID.ToString() == id.ToString()).FirstOrDefault(); //Find(id);//

                db.SupplierInformations.Remove(supplierInformation);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, ContactID = supplierInformation.ContactID, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message != null)
                {
                    return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });

                }
                else
                {
                    return Json(new { Success = 0, ex = ex.Message.ToString() });

                }
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");
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
