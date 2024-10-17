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
    public class COM_MachineryLCMasterController_Sayeed : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: COM_MachineryLCMaster
        public ActionResult Index()
        {
            var cOM_MachineryLCMasters = db.COM_MachineryLCMasters.Include(c => c.SupplierInformation);
            return View(cOM_MachineryLCMasters.ToList());
        }

        // GET: COM_MachineryLCMaster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_MachineryLCMaster cOM_MachineryLCMaster = db.COM_MachineryLCMasters.Find(id);
            if (cOM_MachineryLCMaster == null)
            {
                return NotFound();
            }
            return View(cOM_MachineryLCMaster);
        }

        // GET: COM_MachineryLCMaster/Create
        public ActionResult Create(int? SupplierId)
        {
            ViewBag.Title = "Create";
            ViewBag.SupplierId = SupplierId;
            if (SupplierId==null)
            {
                SupplierId = 0;
            }

            COM_MachineryLCMaster machineryLCMaster = new COM_MachineryLCMaster();

            machineryLCMaster.COM_MachineryLCDetailses = db.COM_MachineryLCDetailses.Where(m => m.WorkOrderId == SupplierId).ToList();
            //ViewBag.WorkOrder = db.WorkOrderMasters.Where(w => w.WorkOrderId == SupplierId).ToList();



            ViewBag.WorkOrder = (from WorkOrder in db.WorkOrderMasters.Where(m => m.comid.ToString() == AppData.intComId)
                                where WorkOrder.SupplierId == SupplierId select WorkOrder).Distinct().ToList();



            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");
            ViewBag.InsuranceCompanyId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");
            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName");
            return View(machineryLCMaster);
        }

        // POST: COM_MachineryLCMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(COM_MachineryLCMaster cOM_MachineryLCMaster)
        {
            //if (ModelState.IsValid)
            //{
                db.COM_MachineryLCMasters.Add(cOM_MachineryLCMaster);
                db.SaveChanges();
                //return RedirectToAction("Index");
            //}

            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_MachineryLCMaster.SupplierId);
            ViewBag.InsuranceCompanyId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");
            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName",cOM_MachineryLCMaster.PaymentTermsId);
            return View(cOM_MachineryLCMaster);
        }

        // GET: COM_MachineryLCMaster/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_MachineryLCMaster cOM_MachineryLCMaster = db.COM_MachineryLCMasters.Find(id);
            if (cOM_MachineryLCMaster == null)
            {
                return NotFound();
            }
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_MachineryLCMaster.SupplierId);
            return View(cOM_MachineryLCMaster);
        }

        // POST: COM_MachineryLCMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "MachineryLCId,LCNumber,LCDate,SupplierId,PaymentTermsId,DefferredPaymentDays,ShipDate,InsuranceCompanyId,InsurancePayStatus,ImportBillNo,ImportBillDate,BillMacturityDate,BillPayDate,TotalBillAmount,Addedby,DateAdded,Updatedby,DateUpdated,comid,userid")] COM_MachineryLCMaster cOM_MachineryLCMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOM_MachineryLCMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_MachineryLCMaster.SupplierId);
            return View(cOM_MachineryLCMaster);
        }

        // GET: COM_MachineryLCMaster/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_MachineryLCMaster cOM_MachineryLCMaster = db.COM_MachineryLCMasters.Find(id);
            if (cOM_MachineryLCMaster == null)
            {
                return NotFound();
            }
            return View(cOM_MachineryLCMaster);
        }

        // POST: COM_MachineryLCMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COM_MachineryLCMaster cOM_MachineryLCMaster = db.COM_MachineryLCMasters.Find(id);
            db.COM_MachineryLCMasters.Remove(cOM_MachineryLCMaster);
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
