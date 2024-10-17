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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class BusinessTypesController : Controller
    {
        private GTRDBContext db;
        public BusinessTypesController(GTRDBContext context)
        {
            db = context;
        }

        // GET: BusinessTypes
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            return View(db.BusinessType.ToList());
        }

        // GET: BusinessTypes/Details/5
        //[OverridableAuthorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            BusinessType businessType = db.BusinessType.Find(id);
            if (businessType == null)
            {
                return NotFound();
            }
            return View(businessType);
        }

        // GET: BusinessTypes/Create
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*/*Include =*/*/ "BusinessTypeId,BusinessTypeCode,Name")] BusinessType businessType)
        {
            if (ModelState.IsValid)
            {
                db.BusinessType.Add(businessType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(businessType);
        }

        // GET: BusinessTypes/Edit/5
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            BusinessType businessType = db.BusinessType.Find(id);
            if (businessType == null)
            {
                return NotFound();
            }
            return View(businessType);
        }

        // POST: BusinessTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*/*Include =*/ */"BusinessTypeId,BusinessTypeCode,Name")] BusinessType businessType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(businessType);
        }

        // GET: BusinessTypes/Delete/5
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            BusinessType businessType = db.BusinessType.Find(id);
            if (businessType == null)
            {
                return NotFound();
            }
            return View(businessType);
        }

        // POST: BusinessTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessType businessType = db.BusinessType.Find(id);
            db.BusinessType.Remove(businessType);
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
