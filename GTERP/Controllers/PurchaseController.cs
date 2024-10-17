using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using Rotativa;
using GTCommercial.Models;
using System.Net;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web;
using GTCommercial.AppData;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Data.Entity.Core.Objects;
using GTCommercial.Models.Common;
using System.Data.Entity.Validation;
using System.Globalization;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class PurchaseController : Controller
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();


        //private int comid = int.Parse(httpre Session["comid"].ToString());
        //
        // GET: /Purchase/
        public ViewResult Index(string FromDate, string ToDate)
        {

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);



            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);

            }

            //var a = ;
            // return View(db.PurchaseMains.Where(p => p.ComId == AppData.intComId).ToList());
            return View(db.PurchaseMains.Where(p => p.ComId == AppData.intComId.ToString() && (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo)).ToList());
        }

        //
        // GET: /Purchase/Details/5

        //[OutputCache(Duration = 100, VaryByParam  = "id")]
        //[OutputCache(CacheProfile ="Admin")]
        public ViewResult Details(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            PurchaseMain purchasemain = db.PurchaseMains.Find(id);
            return View(purchasemain);
        }


        public ViewResult PrintView(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            PurchaseMain purchasemain = db.PurchaseMains.Find(id);
            return View(purchasemain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //return View(purchasemain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "Purchase", new { id = id });
        }

        // make pdf
        public ActionResult ExportPdf(int id)
        {
            // go to new page( will not show ) and make it pdf
            return new ActionAsPdf("PrintView", new { id = id })
            {
                FileName = Server.MapPath("~/Content/FileName.pdf")
            }; ;
        }
        public ActionResult Print(int? id, string type)
        {
            //Session["rptList"] = null;
            clsReport.rptList = null;

            Session["ReportPath"] = "~/Report/rptInvoice.rdlc";
            HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptInvoice] '" + id + "'," + AppData.intComId + ",'" + AppData.AppPath.ToString() + "'";
            string ReportPath = "~/Report/rptInvoice.rdlc";
            string SQLQuery = "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptInvoice] '" + id + "'," + AppData.intComId + ",'" + AppData.AppPath.ToString() + "'";
            string DataSourceName = "DataSet1";
            //string FormCaption = "Report :: Purchase Acknowledgement ...";


            postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "'," + Session["comid"].ToString() + ",''"));

            HttpContext.Session.SetObject("rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = ReportPath;
            clsReport.strQueryMain = SQLQuery;
            clsReport.strDSNMain = DataSourceName;

            return RedirectToAction("Index", "ReportViewer");


        }

        public ActionResult Create()
        {

            var a = Session["isProductSearch"];
            ViewBag.Title = "Entry";

            this.ViewBag.PurchaseType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(db.Suppliers.Where(c => c.SupplierId > 0 && c.comid.ToString() == (AppData.intComId)), "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

            var Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            this.ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            this.ViewBag.ProductSearch = Productresult;

            this.ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

            this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);
            var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.ProductSerialSearch = ProductSerialresult;

            return View();


        }
        public class ProductSerialtemp
        {
            public int ProductId { get; set; }

            public int ProductSerialId { get; set; }
            public string ProductSerialNo { get; set; }
        }

        public ActionResult CreatePurchase()
        {

            var a = Session["isProductSearch"];
            ViewBag.Title = "Create";
            this.ViewBag.PurchaseType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            this.ViewBag.Product = new SelectList(db.Products, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(db.Products, "ProductId", "ProductBarcode");
            this.ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            this.ViewBag.ProductSerial = new SelectList(db.ProductSerial, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");


            return View();


        }


        // POST: /Purchase/Create
        /// <summary>
        /// This method is used for Creating and Updating  Purchase Information 
        /// (Purchase Contains: 1.PurchaseMain and *PurchaseSub )
        /// </summary>
        /// <param name="purchasemain">
        /// </param>
        /// <returns>
        /// Returns Json data Containing Success Status, New Purchase ID and Exeception 
        /// </returns>
        [HttpPost]
        public JsonResult Create(PurchaseMain purchasemain)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                //if (ModelState.IsValid)
                //if (errors.Count() < 2)

                {

                    // If sales main has PurchaseID then we can understand we have existing sales Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (purchasemain.PurchaseId > 0)
                    {
                        //var CurrentProductSerial = db.ProductSerial.Where(p => p.PurchaseId == purchasemain.PurchaseId);
                        var CurrentPurchaseSub = db.PurchaseSubs.Where(p => p.PurchaseId == purchasemain.PurchaseId);
                        var CurrentsalesTermsSUb = db.PurchaseTermsSubs.Where(p => p.PurchaseId == purchasemain.PurchaseId);
                        //PurchaseSub
                        //foreach (ProductSerial ss in CurrentProductSerial)
                        //db.ProductSerial.Remove(ss);

                        foreach (PurchaseSub ss in CurrentPurchaseSub)
                            db.PurchaseSubs.Remove(ss);


                        foreach (PurchaseSub ss in purchasemain.PurchaseSubs)
                            db.PurchaseSubs.Add(ss);


                        foreach (PurchaseTermsSub sss in CurrentsalesTermsSUb)
                            db.PurchaseTermsSubs.Remove(sss);


                        if (purchasemain.PurchaseTermsSubs == null)
                        {
                        }
                        else
                        {

                            foreach (PurchaseTermsSub sss in purchasemain.PurchaseTermsSubs)
                                db.PurchaseTermsSubs.Add(sss);
                        }





                        db.Entry(purchasemain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.PurchaseMains.Add(purchasemain);
                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, PurchaseID = purchasemain.PurchaseId, ex = "" });
                }
            }
            catch (Exception ex)
            {

                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        //
        // GET: /Purchase/Edit/5
        public ActionResult Edit(int? id)
        {



            if (id == null)
            {
                return BadRequest();
            }


            PurchaseMain purchasemain = db.PurchaseMains.Find(id);

            if (purchasemain.isPost == true)
            {
                return BadRequest();
            }

            //PurchaseMain purchasemain = db.PurchaseMains.Find(id);



            if (purchasemain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";



            ///this.ViewData["Supplier"] = new SelectList(db.Suppliers.ToList(), "SupplierId", "SupplierName");


            ////ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", purchasemain.PurchaseSubs.);
            //this.ViewBag.PurchaseType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            //this.ViewBag.Supplier = new SelectList(db.Suppliers.Where(c => c.SupplierId > 0), "SupplierId", "SupplierName");
            //this.ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            //this.ViewBag.Product = new SelectList(db.Products.Where(c => c.ProductId > 0), "ProductId", "ProductName");
            //this.ViewBag.Barcode = new SelectList(db.Products.Where(c => c.ProductId > 0), "ProductId", "ProductBarcode");
            //this.ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            //this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");
            //this.ViewBag.PurchaseTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);


            this.ViewBag.PurchaseType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(db.Suppliers.Where(c => c.SupplierId > 0 && c.comid.ToString() == (AppData.intComId)), "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

            var Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            this.ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            this.ViewBag.ProductSearch = Productresult;

            this.ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

            this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);
            var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.ProductSerialSearch = ProductSerialresult;



            //Call Create View
            return View("Create", purchasemain);
        }





        // GET: /Purchase/Delete/5
        public ActionResult Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //PurchaseMain purchasemain = db.PurchaseMains.Find(id);
            //return View(purchasemain);

            HttpContext.Session["isBarcodeSearch"] = true;
            HttpContext.Session["isProductSearch"] = true;
            HttpContext.Session["isIMEISearch"] = true;


            if (id == null)
            {
                return BadRequest();
            }


            PurchaseMain purchasemain = db.PurchaseMains.Find(id);

            if (purchasemain.isPost == true)
            {
                return BadRequest();
            }

            //PurchaseMain purchasemain = db.PurchaseMains.Find(id);



            if (purchasemain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";


            this.ViewBag.PurchaseType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(db.Suppliers.Where(c => c.SupplierId > 0 && c.comid.ToString() == (AppData.intComId)), "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

            var Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            this.ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            this.ViewBag.ProductSearch = Productresult;

            this.ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

            this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);
            var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.ProductSerialSearch = ProductSerialresult;



            //Call Create View
            return View("Create", purchasemain);
        }




        // POST: /Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //PurchaseSub PurchaseSub = db.PurchaseSubs.Find(id);
                //db.PurchaseSubs.Remove(PurchaseSub);

                PurchaseMain purchasemain = db.PurchaseMains.Find(id);
                db.PurchaseMains.Remove(purchasemain);
                db.SaveChanges();
                return Json(new { Success = 1, PurchaseID = purchasemain.PurchaseId, ex = "" });

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
            db.Dispose();
            base.Dispose(disposing);
        }



        public JsonResult getTerms(int id)
        {
            var terms = db.TermsSub.Where(x => x.TermsId == id).ToList();

            List<SelectListItem> termssubslists = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (terms != null)
            {
                foreach (var x in terms)
                {
                    termssubslists.Add(new SelectListItem { Text = x.TermsDescription.ToString(), Value = x.Terms.ToString() });
                }
            }
            return Json(new SelectList(termssubslists, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getProduct(int id)
        {
            var product = db.Products.Where(x => x.CategoryId == id).ToList();

            List<SelectListItem> licities = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (var x in product)
                {
                    licities.Add(new SelectListItem { Text = x.ProductName, Value = x.ProductId.ToString() });
                }
            }
            return Json(new SelectList(licities, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        public JsonResult getBarcode(int id)
        {
            var product = db.Products.Where(x => x.ProductId == id).ToList();

            List<SelectListItem> barcodelist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (var x in product)
                {
                    barcodelist.Add(new SelectListItem { Text = x.ProductBarcode, Value = x.ProductId.ToString() });
                }
            }
            return Json(new SelectList(barcodelist, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getProductSerial(int id)
        {
            var product = db.ProductSerial.Where(x => x.ProductId == id).ToList();

            List<SelectListItem> productseriallist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (var x in product)
                {
                    productseriallist.Add(new SelectListItem { Text = x.ProductSerialNo, Value = x.ProductSerialId.ToString() });
                }
            }
            return Json(new SelectList(productseriallist, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        [HttpPost]
        public JsonResult ProductInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                //context.ContextOptions.ProxyCreationEnabled = false;
                //context.ContextOptions.LazyLoadingEnabled = false;

                var product = db.Products.Where(y => y.ProductId == id).SingleOrDefault();

                return Json(product, JsonRequestBehavior.AllowGet);
                //return Json("tesst", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new SelectList(product, "Value", "Text", JsonRequestBehavior.AllowGet));
        }




        [HttpPost]
        public JsonResult SupplierInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var customer = db.Suppliers.Where(y => y.SupplierId == id).SingleOrDefault();
                return Json(customer, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        public static class DropDownList<T>
        {
            public static SelectList LoadItems(IList<T> collection, string value, string text)
            {
                return new SelectList(collection, value, text);
            }
        }


        public ActionResult StoreProcedureReport()
        {
            int userid = 1;
            int comid = 1;


            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@userid", userid));
            parameters.Add(new SqlParameter("@comid", comid));

            ObjectContext dbcontext = new ObjectContext("name=MasterDetailsEntities");




            var dataset = ExecuteStoredProcedure(dbcontext, "dbo.prcgetSupplier", parameters); ////store procedure wise reporting // fahad most important
            return View();
        }
        public static DataSet ExecuteStoredProcedure(ObjectContext db, string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            var entityConnection = (EntityConnection)db.Connection;
            var conn = entityConnection.StoreConnection;
            var initialState = conn.State;


            DataSet dataSet = new DataSet();

            try
            {
                if (initialState != ConnectionState.Open)
                    conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }



                    using (var reader = cmd.ExecuteReader())
                    {

                        DataTable dt = new DataTable();

                        //while (reader.Read())
                        //{
                        //    dt.Load(reader);

                        //}
                        while (!reader.IsClosed)
                            dataSet.Tables.Add().Load(reader);

                        //dataSet.Tables.Add(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (initialState != ConnectionState.Open)
                    conn.Close();
            }
            return dataSet;
        }


    }
}