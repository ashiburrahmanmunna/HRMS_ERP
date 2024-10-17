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
    public class SuppliersController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: Suppliers
        public ActionResult Index()
        {

            //var products = db.Products
            //.Include(p => p.vPrimaryCategory)
            //.Where(p => p.ProductId > 1 && p.comid.ToString() == AppData.intComId.ToString());
            //return View(products.ToList());

            //int comid = int.Parse(AppData.intComId);

            var suppliers = db.Suppliers.Where(s => s.comid.ToString() == (AppData.intComId)); //.Where(s => s.comid == int.Parse(Session["comid"].ToString()))
           //var x = suppliers.ToList();
            return View(suppliers.ToList());
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.Title = "Create";
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "SupplierId,SupplierCode,SupplierName,EmailId,CountryId,PrimaryAddress,SecoundaryAddress,PostalCode,City,PhoneNo,IsInActive,OpBalance")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {

                if (supplier.SupplierId > 0)
                {

                    supplier.comid = int.Parse(AppData.intComId);
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    supplier.comid = int.Parse(AppData.intComId); // Session["comid"].ToString()
                    db.Suppliers.Add(supplier);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplier.CountryId);
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplier.CountryId);
            ViewBag.Title = "Edit";
            return View("Create", supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "SupplierId,SupplierCode,SupplierName,EmailId,CountryId,PrimaryAddress,SecoundaryAddress,PostalCode,City,PhoneNo,IsInActive,OpBalance")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.comid = int.Parse(AppData.intComId);
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplier.CountryId);
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", supplier.CountryId);
            ViewBag.Title = "Delete";
            return View("Create", supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {

                Supplier supplier = db.Suppliers.Find(id);
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
                return Json(new { Success = 1, SupplierId = supplier.SupplierId, ex = "" });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
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
