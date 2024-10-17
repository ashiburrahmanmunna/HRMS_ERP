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
    public class TypesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: Types
        public ActionResult Index()
        {
            return View(db.SalesType.ToList());
        }

        // GET: Types/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SalesType types = db.SalesType.Find(id);
            if (types == null)
            {
                return NotFound();
            }
            return View(types);
        }

        // GET: Types/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "SalesTypeId,TypeName,TypeShortName")] SalesType SalesType)
        {
            if (ModelState.IsValid)
            {
                db.SalesType.Add(SalesType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(SalesType);
        }

        // GET: Types/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SalesType SalesType = db.SalesType.Find(id);
            if (SalesType == null)
            {
                return NotFound();
            }
            return View(SalesType);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "SalesTypeId,TypeName,TypeShortName")] SalesType SalesType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(SalesType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(SalesType);
        }

        // GET: Types/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SalesType SalesType = db.SalesType.Find(id);
            if (SalesType == null)
            {
                return NotFound();
            }
            return View(SalesType);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesType SalesType = db.SalesType.Find(id);
            db.SalesType.Remove(SalesType);
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
