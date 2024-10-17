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
    public class UnitGroupsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: UnitGroups
        public ActionResult Index()
        {
            return View(db.UnitGroups.ToList());
        }

        // GET: UnitGroups/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            UnitGroup unitGroup = db.UnitGroups.Find(id);
            if (unitGroup == null)
            {
                return NotFound();
            }
            return View(unitGroup);
        }

        // GET: UnitGroups/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            return View();
        }

        // POST: UnitGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "UnitGroupId")] UnitGroup unitGroup)
        {
            if (ModelState.IsValid)
            {
                db.UnitGroups.Add(unitGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitGroup);
        }

        // GET: UnitGroups/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            UnitGroup unitGroup = db.UnitGroups.Find(id);
            if (unitGroup == null)
            {
                return NotFound();
            }
            return View(unitGroup);
        }

        // POST: UnitGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "UnitGroupId")] UnitGroup unitGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitGroup);
        }

        // GET: UnitGroups/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            UnitGroup unitGroup = db.UnitGroups.Find(id);
            if (unitGroup == null)
            {
                return NotFound();
            }
            return View(unitGroup);
        }

        // POST: UnitGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UnitGroup unitGroup = db.UnitGroups.Find(id);
            db.UnitGroups.Remove(unitGroup);
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
