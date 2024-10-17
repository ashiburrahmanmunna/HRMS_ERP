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
    public class StyleInformationsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: StyleInformations
        public ActionResult Index()
        {
            var styleInformation = db.StyleInformation.Include(s => s.BrandInfo).Include(s => s.BuyerInformation).Include(s => s.Company).Include(s => s.Currency).Include(s => s.ProductCategory).Include(s => s.ProductGroup);
            return View(styleInformation.ToList());
        }

        // GET: StyleInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            StyleInformation styleInformation = db.StyleInformation.Find(id);
            if (styleInformation == null)
            {
                return NotFound();
            }
            return View(styleInformation);
        }

        // GET: StyleInformations/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewBag.BrandInfoId = new SelectList(db.BrandInfo, "BrandInfoId", "BrandName");
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "CategoryName");
            ViewBag.ProductGroupId = new SelectList(db.ProductGroup, "ProductGroupId", "ProductGroupName");
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitName");
            ViewBag.StyleStatusId = new SelectList(db.StyleStatus, "StyleStatusId", "StyleStatusName");


            return View();
        }

        // POST: StyleInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "StyleId,StyleName,StyleCode,CommercialCompanyId,BuyerId,OrderQty,UnitMasterId,FOB,CurrencyId,SalesCost,StyleStatusId,FirstShipDate,LastShipDate,ProductCategoryId,BrandInfoId,ProductGroupId,AddByUserId,DateAdded,UpdateByUserId,Dateupdated,Fabrication,HSCode")] StyleInformation styleInformation)
        {
            try
            {

     
            if (ModelState.IsValid)
            {
                if (styleInformation.StyleId > 0)
                {
                    styleInformation.DateUpdated = DateTime.Now;
                    db.Entry(styleInformation).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    styleInformation.DateAdded = DateTime.Now;
                    styleInformation.comid = int.Parse(AppData.intComId);
                    styleInformation.userid = HttpContext.Session.GetString("userid");

                    db.StyleInformation.Add(styleInformation);

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.BrandInfoId = new SelectList(db.BrandInfo, "BrandInfoId", "BrandName", styleInformation.BrandInfoId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", styleInformation.BuyerId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", styleInformation.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", styleInformation.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "CategoryName", styleInformation.ProductCategoryId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroup, "ProductGroupId", "ProductGroupName", styleInformation.ProductGroupId);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitName", styleInformation.UnitMasterId);
            ViewBag.StyleStatusId = new SelectList(db.StyleStatus, "StyleStatusId", "StyleStatusName", styleInformation.StyleStatusId);
            return View(styleInformation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: StyleInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            StyleInformation styleInformation = db.StyleInformation.Find(id);
            if (styleInformation == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.BrandInfoId = new SelectList(db.BrandInfo, "BrandInfoId", "BrandName", styleInformation.BrandInfoId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", styleInformation.BuyerId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", styleInformation.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", styleInformation.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "CategoryName", styleInformation.ProductCategoryId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroup, "ProductGroupId", "ProductGroupName", styleInformation.ProductGroupId);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitName", styleInformation.UnitMasterId);
            ViewBag.StyleStatusId = new SelectList(db.StyleStatus, "StyleStatusId", "StyleStatusName", styleInformation.StyleStatusId);
            return View("Create",styleInformation);
        }

        // POST: StyleInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "StyleId,StyleName,StyleCode,CommercialCompanyId,BuyerId,OrderQty,UOM,FOB,CurrencyId,SalesCost,StyleStatusId,FirstShipDate,LastShipDate,ProductCategoryId,BrandInfoId,ProductGroupId,AddByUserId,DateAdded,UpdateByUserId,Dateupdated")] StyleInformation styleInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(styleInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandInfoId = new SelectList(db.BrandInfo, "BrandInfoId", "BrandName", styleInformation.BrandInfoId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", styleInformation.BuyerId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", styleInformation.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", styleInformation.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "CategoryName", styleInformation.ProductCategoryId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroup, "ProductGroupId", "ProductGroupName", styleInformation.ProductGroupId);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitName", styleInformation.UnitMasterId);
            ViewBag.StyleStatusId = new SelectList(db.StyleStatus, "StyleStatusId", "StyleStatusName",  styleInformation.StyleStatusId);
            return View(styleInformation);
        }

        // GET: StyleInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            StyleInformation styleInformation = db.StyleInformation.Find(id);
            if (styleInformation == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.BrandInfoId = new SelectList(db.BrandInfo, "BrandInfoId", "BrandName", styleInformation.BrandInfoId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", styleInformation.BuyerId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", styleInformation.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", styleInformation.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "CategoryName", styleInformation.ProductCategoryId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroup, "ProductGroupId", "ProductGroupName", styleInformation.ProductGroupId);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitName", styleInformation.UnitMasterId);
            ViewBag.StyleStatusId = new SelectList(db.StyleStatus, "StyleStatusId", "StyleStatusName", styleInformation.StyleStatusId);

            return View("Create", styleInformation);
        }

        // POST: StyleInformations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                StyleInformation styleInformation = db.StyleInformation.Find(id);
                db.StyleInformation.Remove(styleInformation);
                db.SaveChanges();

                return Json(new { Success = 1, id = styleInformation.StyleId, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0,ex = ex.Message.ToString() });

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
