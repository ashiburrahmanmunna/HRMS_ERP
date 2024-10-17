using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.Models;

namespace MasterDetail.Controllers
{
    public class CompanysController : Controller
    {
        private MasterDetailContext db = new MasterDetailContext();

        // GET: Companys
        public ActionResult Index()
        {
            var Companys = db.Companys;//.Include(s => s.vBusinessTypeCompany);//.Include(s => s.vCompany).Include(s => s.vCountryCompany);
            return View(Companys.ToList());
        }

        // GET: Companys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Company Company = db.Companys.Find(id);
            if (Company == null)
            {
                return NotFound();
            }
            return View(Company);
        }

        // GET: Companys/Create
        public ActionResult Create()
        {
            ViewBag.BusinessTypeId = new SelectList(db.BusinessType, "BusinessTypeId", "Name");
            ViewBag.ComId = new SelectList(db.Companys, "ComId", "CompanyName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View();
        }

        // POST: Companys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ComId,CompanyCode,CompanyName,CompanyShortName,PrimaryAddress,SecoundaryAddress,comPhone,comPhone2,comFax,comEmail,comWeb,BusinessTypeId,ComId,CountryId,DecimalField,ContPerson,ContDesig,IsShowCurrencySymbol,IsInActive,isBarcode,isProduct,isCorporate,isPOSprint,IsService,isOldDue,isShortcutSale,isRestaurantSale,isTouch,isShoeSale,isIMEISale,isMultipleWh,ComImageHeader,ComLogo")] Company Company)
        {
            if (ModelState.IsValid)
            {
                db.Companys.Add(Company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessTypeId = new SelectList(db.BusinessType, "BusinessTypeId", "Name");
            ViewBag.ComId = new SelectList(db.Companys, "ComId", "CompanyName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View(Company);
        }

        // GET: Companys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Company Company = db.Companys.Find(id);
            if (Company == null)
            {
                return NotFound();
            }
            ViewBag.BusinessTypeId = new SelectList(db.BusinessType, "BusinessTypeId", "Name");
            ViewBag.ComId = new SelectList(db.Companys, "ComId", "CompanyName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View(Company);
        }

        // POST: Companys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "ComId,CompanyCode,CompanyName,CompanyShortName,PrimaryAddress,SecoundaryAddress,comPhone,comPhone2,comFax,comEmail,comWeb,BusinessTypeId,ComId,CountryId,DecimalField,ContPerson,ContDesig,IsShowCurrencySymbol,IsInActive,isBarcode,isProduct,isCorporate,isPOSprint,IsService,isOldDue,isShortcutSale,isRestaurantSale,isTouch,isShoeSale,isIMEISale,isMultipleWh,ComImageHeader,ComLogo")] Company Company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessTypeId = new SelectList(db.BusinessType, "BusinessTypeId", "Name");
            ViewBag.ComId = new SelectList(db.Companys, "ComId", "CompanyName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View(Company);
        }

        // GET: Companys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Company Company = db.Companys.Find(id);
            if (Company == null)
            {
                return NotFound();
            }
            return View(Company);
        }

        // POST: Companys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company Company = db.Companys.Find(id);
            db.Companys.Remove(Company);
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
