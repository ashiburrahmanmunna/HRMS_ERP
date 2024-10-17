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
    public class ExportRealizationMastersController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: ExportRealizationMasters
        public ActionResult Index()
        {
            var exportRealizationMasters = db.ExportRealizationMasters.Include(e => e.BuyerInformation).Include(e => e.COM_MasterLC).Include(e => e.SupplierInformation);
            return View(exportRealizationMasters.ToList());
        }

        // GET: ExportRealizationMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportRealizationMaster exportRealizationMaster = db.ExportRealizationMasters.Find(id);
            if (exportRealizationMaster == null)
            {
                return NotFound();
            }
            return View(exportRealizationMaster);
        }

        // GET: ExportRealizationMasters/Create
        public ActionResult Create()
        {
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");
            return View();
        }

        // POST: ExportRealizationMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "RealizationId,RealizationNo,RealizationDate,MasterLCId,SupplierId,BuyerId,TotalExportInvoice,TotalOrderQty,TotalValue,RealizedAmount,PendingValue,Addedby,Dateadded,Updatedby,Dateupdated,comid,userid")] ExportRealizationMaster exportRealizationMaster)
        {
            if (ModelState.IsValid)
            {
                db.ExportRealizationMasters.Add(exportRealizationMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", exportRealizationMaster.BuyerId);
            ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", exportRealizationMaster.MasterLCId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", exportRealizationMaster.SupplierId);
            return View(exportRealizationMaster);
        }

        // GET: ExportRealizationMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportRealizationMaster exportRealizationMaster = db.ExportRealizationMasters.Find(id);
            if (exportRealizationMaster == null)
            {
                return NotFound();
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", exportRealizationMaster.BuyerId);
            ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", exportRealizationMaster.MasterLCId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", exportRealizationMaster.SupplierId);
            return View(exportRealizationMaster);
        }

        // POST: ExportRealizationMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "RealizationId,RealizationNo,RealizationDate,MasterLCId,SupplierId,BuyerId,TotalExportInvoice,TotalOrderQty,TotalValue,RealizedAmount,PendingValue,Addedby,Dateadded,Updatedby,Dateupdated,comid,userid")] ExportRealizationMaster exportRealizationMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exportRealizationMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", exportRealizationMaster.BuyerId);
            ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", exportRealizationMaster.MasterLCId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", exportRealizationMaster.SupplierId);
            return View(exportRealizationMaster);
        }

        // GET: ExportRealizationMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportRealizationMaster exportRealizationMaster = db.ExportRealizationMasters.Find(id);
            if (exportRealizationMaster == null)
            {
                return NotFound();
            }
            return View(exportRealizationMaster);
        }

        // POST: ExportRealizationMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExportRealizationMaster exportRealizationMaster = db.ExportRealizationMasters.Find(id);
            db.ExportRealizationMasters.Remove(exportRealizationMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
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
