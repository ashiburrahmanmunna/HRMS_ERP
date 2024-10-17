using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GTERP.Controllers
{
    //[OverridableAuthorize]
    public class BankAccountNoLienBanksController : Controller
    {
        private GTRDBContext db;
        public BankAccountNoLienBanksController(GTRDBContext context)
        {
            db = context;
        }
        //Log log;
        //UserLog //UserLog;
        public BankAccountNoLienBanksController()
        {
            //log = new Log();
            //UserLog = new //UserLog();
        }

        // GET: BankAccountNoes
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            var BankAccountNoLienBanks = db.BankAccountNoLienBanks.ToList();
            return View(BankAccountNoLienBanks);
        }



        // GET: BankAccountNoes/Create
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");

            return View();
        }

        // POST: BankAccountNoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*//[Bind(Include = "LienBankAccountId,BankAccountNumber,CommercialCompanyId,LienBankId")]*/ BankAccountNoLienBank bankAccountNoLienBank)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                if (bankAccountNoLienBank.LienBankAccountId > 0)
                {
                    db.Entry(bankAccountNoLienBank).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), bankAccountNoLienBank.LienBankAccountId.ToString(), "Update");
                }
                else
                {
                    db.BankAccountNoLienBanks.Add(bankAccountNoLienBank);
                    db.SaveChanges();
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), bankAccountNoLienBank.LienBankAccountId.ToString(), "Create");
                }
               

                //}
                ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", bankAccountNoLienBank.CommercialCompanyId);
                ViewBag.LienBankId = new SelectList(db.OpeningBanks, "LienBankId", "LienBankName", bankAccountNoLienBank.LienBankId);
                //return RedirectToAction("Index");
                //return View(bankAccountNoLienBank);
                
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";                
                TempData["Status"] = "0";
            }
            return RedirectToAction("Index");
        }

        // GET: BankAccountNoes/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            BankAccountNoLienBank bankAccountNoLienBank = db.BankAccountNoLienBanks.Where(x => x.LienBankAccountId == id).FirstOrDefault();
            if (bankAccountNoLienBank == null)
            {
                return NotFound();
            }
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", bankAccountNoLienBank.CommercialCompanyId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", bankAccountNoLienBank.LienBankId);


            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", bankAccountNoLienBank.SupplierId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", bankAccountNoLienBank.BuyerId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", bankAccountNoLienBank.CountryId);
            return View("Create", bankAccountNoLienBank);
        }

        // GET: BankAccountNoes/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            BankAccountNoLienBank bankAccountNoLienBank = db.BankAccountNoLienBanks.Find(id);
            if (bankAccountNoLienBank == null)
            {
                return NotFound();
            }
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", bankAccountNoLienBank.CommercialCompanyId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", bankAccountNoLienBank.LienBankId);


            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", bankAccountNoLienBank.SupplierId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", bankAccountNoLienBank.BuyerId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", bankAccountNoLienBank.CountryId);

            return View("Create", bankAccountNoLienBank);
        }

        // POST: BankAccountNoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BankAccountNoLienBank bankAccountNoLienBank = db.BankAccountNoLienBanks.Find(id);
                db.BankAccountNoLienBanks.Remove(bankAccountNoLienBank);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, bankAccountNoLienBank.LienBankAccountId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }

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
