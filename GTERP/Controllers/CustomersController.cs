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
    public class CustomersController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Where(c => c.comid.ToString() == (AppData.intComId)).ToList(); //.Include(c => c.vCustomerCountry)
            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
 
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.Title = "Create";
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "CustomerId,CustomerCode,CustomerName,EmailId,CountryId,PrimaryAddress,SecoundaryAddress,PostalCode,City,PhoneNo,IsInActive,OpBalance")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                if (customer.CustomerId > 0)
                {

                    customer.comid = int.Parse(AppData.intComId);
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    customer.comid = int.Parse(AppData.intComId);
                    db.Customers.Add(customer);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
            return View("Create", customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CustomerId,CustomerCode,CustomerName,EmailId,CountryId,PrimaryAddress,SecoundaryAddress,PostalCode,City,PhoneNo,IsInActive,OpBalance")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.comid = int.Parse(AppData.intComId);
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
            ViewBag.Title = "Delete";
            return View("Create", customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {

            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return Json(new { Success = 1, CustomerId = customer.CustomerId, ex = "" });

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
