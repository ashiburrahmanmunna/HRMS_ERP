using GTCommercial.Models;
using GTCommercial.Models.Common;
using Microsoft.Reporting.WebForms;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class SalesController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();


        //private int comid = int.Parse(httpre Session["comid"].ToString());
        //
        // GET: /Sales/
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


            //DateTime? startdate, DateTime? enddate
            //ViewBag.Datetime = DateTime.UtcNow;
            //ViewBag.startdate = startdate;
            //ViewBag.enddate = enddate;

            //var a = ;
            // return View(db.SalesMains.Where(p => p.ComId == AppData.intComId).ToList());
            return View(db.SalesMains.Where(p => (p.SalesDate >= dtFrom && p.SalesDate <= dtTo) && (p.ComId == AppData.intComId.ToString())).ToList()); //p.ComId == AppData.intComId && 

            //obj => obj.StartDate >= startDate && obj.EndDate < endDateExclusive

            // ViewBag.Customer = entity.Customers.Where(i => i.MobileNumber == 12321 && i.Nmae == "abc").firstordefault();

            //var a = db.SalesMains.ToList();
            //return View(a);
        }

        //
        // GET: /Sales/Details/5

        //[OutputCache(Duration = 100, VaryByParam  = "id")]
        //[OutputCache(CacheProfile ="Admin")]
        public ViewResult Details(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            SalesMain salesmain = db.SalesMains.Find(id);
            return View(salesmain);
        }


        public ViewResult PrintView(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            SalesMain salesmain = db.SalesMains.Find(id);
            return View(salesmain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //SalesMain salesmain = db.SalesMains.Find(id);
            //return View(salesmain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "Sales", new { id = id });
        }

        // make pdf
        public ActionResult ExportPdf(int id)
        {
            // go to new page( will not show ) and make it pdf
            return new ActionAsPdf("PrintView", new { id = id })
            {
                FileName = Server.MapPath("~/Content/FileName.pdf")
            };
        }
        public ActionResult Print(int? id, string type)
        {
            AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
            //Session["rptList"] = null;
            clsReport.rptList = null;

            //ReportItem item = new ReportItem();
            //string query = "";
            //string path = "";

            //query = "Exec rptInvoice "+ id +" ,'" + System.Web.HttpContext.Current.Session["ComId"] + "'";
            //path = "~/Report/rptInvoice.rdlc";
            //item.Databinds(path, query);
            //clsReport.rptList.Add(new subReport("rptSaleID_PM", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_PM '" + id + "','" + System.Web.HttpContext.Current.Session["ComId"] + "'"));

            //return RedirectToAction("Index", "ReportViewer");


            //[] @ComId tinyint, @Flag varchar(15), @Currency smallInt
            Session["ReportPath"] = "~/Report/rptInvoice.rdlc";
            HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptInvoice] '" + id + "','" + AppData.intComId + "','" + AppData.AppPath.ToString() + "'";
            string ReportPath = "~/Report/rptInvoice.rdlc";
            string SQLQuery = "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptInvoice] '" + id + "','" + AppData.intComId + "','" + AppData.AppPath.ToString() + "'";
            string DataSourceName = "DataSet1";
            //string FormCaption = "Report :: Sales Acknowledgement ...";


            postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));

            //clsReport.rptList.Add(new subReport("rptInvoice_BankDetails", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_BankDetails '" + (gridList.ActiveRow.Cells["termsidBank"].Value.ToString()) + "'," + AppData.intComId + ",'" + AppData.AppPath.ToString() + "'"));
            //clsReport.rptList.Add(new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "'," + AppData.intComId + ",'" + AppData.AppPath.ToString() + "'"));
            //clsReport.rptList.Add(new subReport("rptSaleID_PM", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_PM '" + id + "'," + AppData.intComId + ""));


            HttpContext.Session.SetObject("rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = ReportPath;
            clsReport.strQueryMain = SQLQuery;
            clsReport.strDSNMain = DataSourceName;

            return RedirectToAction("Index", "ReportViewer");


        }

        // pdf page
        //[HttpGet]
        //public ActionResult DataShow(int studentId)
        //{
        //    ResultStudentInfoViewModel studentInfo = studentManager.GetStudentByIdForPdf(studentId);
        //    ViewBag.RegNo = studentInfo.RegNo;
        //    ViewBag.Name = studentInfo.StudentName;
        //    ViewBag.Email = studentInfo.Email;
        //    ViewBag.Department = studentInfo.Department;

        //    ViewBag.Result = studentManager.GetResultByStudentId(studentId);
        //    return View();
        //}

        //
        // GET: /Sales/Create

        //[OutputCache(Duration=100)]
        //[OutputCache(Duration = 200, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Create()
        {

            try
            {

                object a = Session["isProductSearch"];
                ViewBag.Title = "Entry";

                ViewBag.SalesType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
                ViewBag.Customer = new SelectList(db.Customers.Where(c => c.CustomerId > 0 && c.comid.ToString() == (AppData.intComId)), "CustomerId", "CustomerName");
                ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
                IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
                ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                ViewBag.ProductSearch = Productresult;
                ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
                ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

                ViewBag.SalesTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
                List<ProductSerialtemp> ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
                ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                ViewBag.ProductSerialSearch = ProductSerialresult;

                ViewBag.PaymentTypes = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeShortName");
                ViewBag.AccountHead = new SelectList(db.ChartOfAccount.Where(c => c.ComId.ToString() == (AppData.intComId) && c.AccName.Contains("CASH") && c.AccName.Contains("103070") && c.AccType == "L"), "AccId", "AccName");




                return View();

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                Console.WriteLine(ex.Message);
                return View();
            }
            //this.ViewBag.Product = new SelectList(db.Products.Where(c => c.ProductId > 0), "ProductId", "ProductName");
            //this.ViewBag.Barcode = new SelectList(db.Products.Where(c => c.ProductId > 0), "ProductId", "ProductBarcode");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);




            //this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");
            //this.ViewBag.ProductSerial = new SelectList(db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]"), "ProductSerialId", "ProductSerialNo");


            //db.Database.SqlQuery<ProductSerialtemp>("prcgetSerialNo @param1, @param2", new SqlParameter("param1", param1),new SqlParameter("param2", param2));

            //this.ViewBag.ProductSerial = new SelectList(db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @param1, @param2" , new SqlParameter("param1", param1), new SqlParameter("param2", param2)), "ProductSerialId", "ProductSerialNo");


            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);

            //this.ViewBag.ProductSerialSearch = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @param1, @param2", new SqlParameter("param1", param1), new SqlParameter("param2", param2))).ToList();

            //string param1 = "2"; string param2 = "2";

            //var productserial = db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]").ToList();

            //var asdf = StoreProcedureSerial();

            //this.ViewBag.query = from ug in db.Products
            //            from ugp in db.ProductSerial.Where(x => x.ProductId == ug.ProductId).DefaultIfEmpty()
            //            select new
            //            {
            //                ProductName = ug.ProductName,
            //                ProductBarcode = ug.ProductBarcode,
            //                CategoryId = ug.CategoryId,
            //                ProductId = ug.ProductId,
            //                ProductImage = ug.ProductImage,
            //                ProductSerialNo = ugp != null ? ugp.ProductSerialNo : "" //this is to handle nulls as even when Price is non-nullable prop it may come as null from SQL (result of Left Outer Join)
            //            };

            //var o = db.Products//.Where(x => x.SingerID == id)
            //    .Select(Products => new
            //    {
            //        ProductName = Products.ProductName,
            //        ProductBarcode = Products.ProductBarcode,
            //        CategoryId = Products.CategoryId,
            //        ProductId = Products.ProductId,
            //        ProductImage = Products.ProductImage,
            //        ProductSerialNo = ""
            //    });

            //var o = (from p in db.Products
            //         from s in db.ProductSerial
            //         where p.ProductId == s.ProductId
            //         select new { ProductName = p.ProductName, ProductBarcode = p.ProductBarcode, CategoryId = p.CategoryId , ProductId = p.ProductId , ProductImage = p.ProductImage , ProductSerialNo = s.ProductSerialNo }).ToList();

            //this.ViewBag.ProductSearch = o;

            return View();


        }
        public class ProductSerialtemp
        {
            public int ProductId { get; set; }

            public int ProductSerialId { get; set; }
            public string ProductSerialNo { get; set; }
        }

        public DataSet StoreProcedureSerial()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Comid", Session["comid"].ToString())
                };
                ObjectContext dbcontext = new ObjectContext("name=MasterDetailsEntities");
                DataSet dataset = ExecuteStoredProcedure(dbcontext, "dbo.prcgetSerialNo", parameters);
                return dataset;
            }
            catch (Exception ex)
            {

                throw (ex);
            }

        }

        //public JsonResult tblproductsearch()
        //{
        //    try
        //    {

        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;


        //        //context.ContextOptions.ProxyCreationEnabled = false;
        //        //context.ContextOptions.LazyLoadingEnabled = false;

        //        var product = db.Products;
        //        //List<SelectListItem> licitiesa = new List<SelectListItem>();

        //        //if (product != null)
        //        //{
        //        //    Console.WriteLine("QueryOk");
        //        //    foreach (var x in product)
        //        //    {
        //        //        licitiesa.Add(new SelectListItem { Text = x.CostPrice.ToString(), Value = x.ProductId.ToString() });
        //        //    }
        //        //}
        //        ////var query = from c in db.Products
        //        ////            where c.ProductId == int.Parse(id)
        //        ////            select c;



        //        //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

        //        return Json(product, JsonRequestBehavior.AllowGet);
        //        //return Json("tesst", JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //[OutputCache(Duration = 200, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult CreateSales()
        {

            object a = Session["isProductSearch"];
            ViewBag.Title = "Create";
            ViewBag.SalesType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.Product = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(db.Products, "ProductId", "ProductBarcode");
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.ProductSerial = new SelectList(db.ProductSerial, "ProductSerialId", "ProductSerialNo");
            ViewBag.SalesTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");




            return View();


        }


        // POST: /Sales/Create
        /// <summary>
        /// This method is used for Creating and Updating  Sales Information 
        /// (Sales Contains: 1.SalesMain and *SalesSub )
        /// </summary>
        /// <param name="salesmain">
        /// </param>
        /// <returns>
        /// Returns Json data Containing Success Status, New Sales ID and Exeception 
        /// </returns>
        [HttpPost]
        public JsonResult Create(SalesMain salesmain)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    // If sales main has SalesID then we can understand we have existing sales Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (salesmain.SalesId > 0)
                    {

                        IQueryable<SalesSub> CurrentsalesSUb = db.SalesSubs.Where(p => p.SalesId == salesmain.SalesId);
                        IQueryable<SalesTermsSub> CurrentsalesTermsSUb = db.SalesTermsSubs.Where(p => p.SalesId == salesmain.SalesId);
                        IQueryable<SalesPaymentSub> CurrentsalesPaymentSUb = db.SalesPaymentSubs.Where(p => p.SalesId == salesmain.SalesId);


                        foreach (SalesSub ss in CurrentsalesSUb)
                        {
                            db.SalesSubs.Remove(ss);
                        }

                        foreach (SalesTermsSub sss in CurrentsalesTermsSUb)
                        {
                            db.SalesTermsSubs.Remove(sss);
                        }

                        foreach (SalesPaymentSub ssss in CurrentsalesPaymentSUb)
                        {
                            db.SalesPaymentSubs.Remove(ssss);
                        }

                        foreach (SalesSub ss in salesmain.SalesSubs)
                        {
                            db.SalesSubs.Add(ss);
                        }
                        ///terms subs
                        if (salesmain.SalesTermsSubs == null)
                        { }
                        else
                        {

                            foreach (SalesTermsSub sss in salesmain.SalesTermsSubs)
                            {
                                db.SalesTermsSubs.Add(sss);
                            }
                        }
                        ///payments
                        if (salesmain.SalesPaymentSubs == null)
                        { }
                        else
                        {

                            foreach (SalesPaymentSub ssss in salesmain.SalesPaymentSubs)
                            {
                                db.SalesPaymentSubs.Add(ssss);
                            }
                        }


                        db.Entry(salesmain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.SalesMains.Add(salesmain);
                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, SalesID = salesmain.SalesId, ex = "" });
                }
            }
            catch (Exception ex)
            {

                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        //
        // GET: /Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            //string cultureinfo = "bd-BD";
            ////string cultureinfo = "th-TH";

            //CultureInfo culture = new CultureInfo(cultureinfo, false);
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            //HttpContext.Session["isBarcodeSearch"] = true;
            //HttpContext.Session["isProductSearch"] = true;
            //HttpContext.Session["isIMEISearch"] = true;


            if (id == null)
            {
                return BadRequest();
            }


            SalesMain salesmain = db.SalesMains.Find(id);

            if (salesmain.isPost == true)
            {
                return BadRequest();
            }

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (salesmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";



            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", salesmain.SalesSubs.);
            ViewBag.SalesType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(db.Customers.Where(c => c.CustomerId > 0 && c.comid.ToString() == (AppData.intComId)), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
            IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.SalesTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            List<ProductSerialtemp> ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(db.ChartOfAccount.Where(c => c.ComId.ToString() == (AppData.intComId) && c.AccName.Contains("CASH") && c.AccName.Contains("103070") && c.AccType == "L"), "AccId", "AccName");



            //Call Create View
            return View("Create", salesmain);
        }





        // GET: /Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //SalesMain salesmain = db.SalesMains.Find(id);
            //return View(salesmain);

            HttpContext.Session["isBarcodeSearch"] = true;
            HttpContext.Session["isProductSearch"] = true;
            HttpContext.Session["isIMEISearch"] = true;


            if (id == null)
            {
                return BadRequest();
            }


            SalesMain salesmain = db.SalesMains.Find(id);

            if (salesmain.isPost == true)
            {
                return BadRequest();
            }

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (salesmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";



            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", salesmain.SalesSubs.);
            ViewBag.SalesType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(db.Customers.Where(c => c.CustomerId > 0 && c.comid.ToString() == (AppData.intComId)), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
            IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.SalesTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            List<ProductSerialtemp> ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", (AppData.intComId)), new SqlParameter("userid", Session["userid"]))).ToList();
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(db.ChartOfAccount.Where(c => c.ComId.ToString() == (AppData.intComId) && c.AccName.Contains("CASH") && c.AccName.Contains("103070") && c.AccType == "L"), "AccId", "AccName");


            //Call Create View
            return View("Create", salesmain);
        }




        // POST: /Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //SalesSub salessub = db.SalesSubs.Find(id);
                //db.SalesSubs.Remove(salessub);

                SalesMain salesmain = db.SalesMains.Find(id);
                db.SalesMains.Remove(salesmain);
                db.SaveChanges();
                return Json(new { Success = 1, SalesID = salesmain.SalesId, ex = "" });

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
            List<TermsSub> terms = db.TermsSub.Where(x => x.TermsId == id).ToList();

            List<SelectListItem> termssubslists = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (terms != null)
            {
                foreach (TermsSub x in terms)
                {
                    termssubslists.Add(new SelectListItem { Text = x.TermsDescription.ToString(), Value = x.Terms.ToString() });
                }
            }
            return Json(new SelectList(termssubslists, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getProduct(int id)
        {
            List<Product> product = db.Products.Where(x => x.CategoryId == id).ToList();

            List<SelectListItem> licities = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (Product x in product)
                {
                    licities.Add(new SelectListItem { Text = x.ProductName, Value = x.ProductId.ToString() });
                }
            }
            return Json(new SelectList(licities, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getAccountHead(string id)
        {
            List<ChartOfAccount> chartofaccounts = db.ChartOfAccount.Where(x => x.AccType == "L" && x.AccCode.Contains("103070")).ToList();

            List<SelectListItem> licoa = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (chartofaccounts != null)
            {
                foreach (ChartOfAccount x in chartofaccounts)
                {
                    licoa.Add(new SelectListItem { Text = x.AccName, Value = x.AccId.ToString() });
                }
            }
            return Json(new SelectList(licoa, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        
        public JsonResult getBarcode(int id)
        {
            List<Product> product = db.Products.Where(x => x.ProductId == id).ToList();

            List<SelectListItem> barcodelist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (Product x in product)
                {
                    barcodelist.Add(new SelectListItem { Text = x.ProductBarcode, Value = x.ProductId.ToString() });
                }
            }
            return Json(new SelectList(barcodelist, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getProductSerial(int id)
        {
            List<ProductSerial> product = db.ProductSerial.Where(x => x.ProductId == id).ToList();

            List<SelectListItem> productseriallist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (ProductSerial x in product)
                {
                    productseriallist.Add(new SelectListItem { Text = x.ProductSerialNo, Value = x.ProductSerialId.ToString() });
                }
            }
            return Json(new SelectList(productseriallist, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        //public JsonResult getProductDetails(int id)
        //{
        //    var productDetails = db.Products.Where(x => x.ProductId == id).ToList();

        //    List<SelectListItem> liproductDetails = new List<SelectListItem>();

        //    //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
        //    if (productDetails != null)
        //    {
        //        foreach (var x in productDetails)
        //        {
        //            liproductDetails.Add(new SelectListItem { Text = x.ProductName, Value = x.ProductId.ToString() });
        //        }
        //    }
        //    return Json(new SelectList(liproductDetails, "Value", "Text", JsonRequestBehavior.AllowGet));
        //}


        [HttpPost]
        public JsonResult ProductInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                //context.ContextOptions.ProxyCreationEnabled = false;
                //context.ContextOptions.LazyLoadingEnabled = false;

                Product product = db.Products.Where(y => y.ProductId == id).SingleOrDefault();
                //List<SelectListItem> licitiesa = new List<SelectListItem>();

                //if (product != null)
                //{
                //    Console.WriteLine("QueryOk");
                //    foreach (var x in product)
                //    {
                //        licitiesa.Add(new SelectListItem { Text = x.CostPrice.ToString(), Value = x.ProductId.ToString() });
                //    }
                //}
                ////var query = from c in db.Products
                ////            where c.ProductId == int.Parse(id)
                ////            select c;



                //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

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
        public JsonResult CustomerInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                Customer customer = db.Customers.Where(y => y.CustomerId == id).SingleOrDefault();
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


        //[HttpGet]
        //[Route("MyReport")]
        //public ActionResult GetReport()
        //{
        //    var reportViewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
        //    reportViewer.LocalReport.ReportPath = "Reports/MyReport.rdlc";

        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("NameOfDataSource1", rptInvoice));
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("NameOfDataSource2", rptInvoice));

        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;

        //    var bytes = reportViewer.LocalReport.Render("application/pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

        //    return File(bytes, "application/pdf");
        //}

        public ActionResult Report(int id, string type)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "rptInvoice.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }


            List<Customer> cm = new List<Customer>();
            using (GTRDBContext dc = new GTRDBContext())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                dc.Configuration.LazyLoadingEnabled = false;

                cm = dc.Customers.ToList();
            }
            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            lr.DataSources.Add(rd);
            string reportType = type;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + type + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out string mimeType,
                out string encoding,
                out string fileNameExtension,
                out string[] streams,
                out Warning[] warnings);
            return File(renderedBytes, mimeType);
        }



        public ActionResult StoreProcedureReport()
        {
            string userid = HttpContext.Session.GetString("userid");
            string comid = Session["comid"].ToString();


            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userid", userid),
                new SqlParameter("@comid", comid)
            };

            ObjectContext dbcontext = new ObjectContext("name=MasterDetailsEntities");




            DataSet dataset = ExecuteStoredProcedure(dbcontext, "dbo.prcgetCustomer", parameters);
            return View();
        }
        public static DataSet ExecuteStoredProcedure(ObjectContext db, string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            EntityConnection entityConnection = (EntityConnection)db.Connection;
            System.Data.Common.DbConnection conn = entityConnection.StoreConnection;
            ConnectionState initialState = conn.State;


            DataSet dataSet = new DataSet();

            try
            {
                if (initialState != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (System.Data.Common.DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }


                    //var dataReader = cmd.ExecuteReader();
                    //var dataTable = new DataTable();
                    //dataTable.Load(dataReader);

                    //dataSet.Tables.Add(dt);



                    using (System.Data.Common.DbDataReader reader = cmd.ExecuteReader())
                    {

                        DataTable dt = new DataTable();

                        //while (reader.Read())
                        //{
                        //    dt.Load(reader);

                        //}
                        while (!reader.IsClosed)
                        {
                            dataSet.Tables.Add().Load(reader);
                        }

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
                {
                    conn.Close();
                }
            }
            return dataSet;
        }


        //public ActionResult CascadeCombo(string Flag, Int64 Id)
        //{
        //    try
        //    {
        //        //DataSet dsList = Issue.prcGetSpecificComboData(int.Parse(Id.ToString()), Flag);
        //        return Json(prcLoadCombo(dsList.Tables[0]));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}