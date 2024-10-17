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
    public class InvoiceController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();

        // GET: /Invoice/
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
            // return View(db.InvoiceMains.Where(p => p.ComId == AppData.intComId).ToList());
            return View(db.InvoiceMains.Where(p => (p.InvoiceDate >= dtFrom && p.InvoiceDate <= dtTo) && (p.userid.ToString() == AppData.userid.ToString())).ToList()); //p.ComId == AppData.intComId && 
        }


        public ViewResult Details(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            InvoiceMain Invoicemain = db.InvoiceMains.Find(id);
            return View(Invoicemain);
        }


        public ViewResult PrintView(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            InvoiceMain Invoicemain = db.InvoiceMains.Find(id);
            return View(Invoicemain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //InvoiceMain Invoicemain = db.InvoiceMains.Find(id);
            //return View(Invoicemain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "Invoice", new { id = id });
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
            //string FormCaption = "Report :: Invoice Acknowledgement ...";


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

       
        public ActionResult Create(string Packageid)
        {

            try
            {

                //object a = Session["isProductSearch"];
                ViewBag.Title = "Entry";


                ViewBag.Package = new SelectList(db.SoftwarePackages.Where(c => c.SoftwarePackageId > -1), "SoftwarePackageId", "Name");
                ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
                ViewBag.InvoiceTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");


                SoftwarePackage packageinfo = db.SoftwarePackages.Where(m => m.SoftwarePackageId.ToString() == Packageid.ToString()).FirstOrDefault();

                //InvoiceMain test = new InvoiceMain();
                //test.InvoiceId = 1;

                //InvoiceSub testsub = new InvoiceSub();
                //testsub.Add(new InvoiceSub { InvoiceId =  1, SoftwarePackageId = int.Parse(Packageid.ToString()) , Qty = 1 , UnitPrice = 100 ,Amount = 1 *100  ,ProductDescription = "test fahad"});

                //testsub.InvoiceId = 1;
                //testsub.SoftwarePackageId = int.Parse(Packageid.ToString());
                //testsub.Qty = 1;
                //testsub.UnitPrice = 100;
                //testsub.Amount = testsub.Qty * testsub.UnitPrice;
                //testsub.ProductDescription = "fahad test";
                //testsub.vsoftwarepackage.Name = packageinfo.Name;

                //IList<InvoiceSub> invoicesubsa = new List<InvoiceSub>();
                //test.InvoiceTermsSubs.Add(testsub);



                //ViewBag.Customer = new SelectList(db.Customers.Where(c => c.CustomerId > 0 && c.comid.ToString() == (AppData.intComId)), "CustomerId", "CustomerName");
                //ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

                //IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
                //ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                //ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                //ViewBag.ProductSearch = Productresult;
                //ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");

                //List<ProductSerialtemp> ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
                //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                //ViewBag.ProductSerialSearch = ProductSerialresult;

                //ViewBag.PaymentTypes = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeShortName");
                //ViewBag.AccountHead = new SelectList(db.ChartOfAccount.Where(c => c.ComId.ToString() == (AppData.intComId) && c.AccName.Contains("CASH") && c.AccName.Contains("103070") && c.AccType == "L"), "AccId", "AccName");
                int  duration = 30;
                DateTime startDate = DateTime.Parse(DateTime.Now.Date.ToString("dd-MMM-yy"));
                DateTime expiryDate = startDate.AddDays(duration);
                var x ="Billing Duration From " +  DateTime.Now.Date.ToString("dd-MMM-yy") + " To " + expiryDate.ToString("dd-MMM-yy")  + " , For " + duration.ToString() + " Days";

                var invoicemain = new InvoiceMain { InvoiceDate = DateTime.Now.Date , CustomerName = Session["userphoneno"].ToString(), InvoiceNo  = "INV#" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"))}; //InvoiceId = 1,
                invoicemain.InvoiceSubs = new List<InvoiceSub>
                {
                    new InvoiceSub {ActiveFromDate = startDate, ActiveToDate = expiryDate , ActiveYesNo = true, SoftwarePackageId = int.Parse(Packageid) , vsoftwarepackage = new SoftwarePackage{ SoftwarePackageId = packageinfo.SoftwarePackageId , Name = packageinfo.Name }, Qty = 1 , UnitPrice = 100 , Amount =1 * 100 , ProductDescription = x} //InvoiceId = 1,
                    //, new InvoiceSub {Id = 2, Name = "Gavin", ParentId = 12}
                };
                invoicemain.InvoiceTermsSubs = new List<InvoiceTermsSub>
                {
                    new InvoiceTermsSub {} //InvoiceId = 1
                    //, new InvoiceSub {Id = 2, Name = "Gavin", ParentId = 12}
                };



                return View("Create", invoicemain);


                //var pageRoute = "~/Invoice/" + "BkashPayment" + ".html";
                //var staticPageToRender = new FilePathResult(pageRoute, "text/html");
                //return staticPageToRender;


            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                Console.WriteLine(ex.Message);
                return View();
            }
            
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
        
        public ActionResult CreateInvoice()
        {

            object a = Session["isProductSearch"];
            ViewBag.Title = "Create";
             
            ViewBag.Customer = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.Product = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(db.Products, "ProductId", "ProductBarcode");
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.ProductSerial = new SelectList(db.ProductSerial, "ProductSerialId", "ProductSerialNo");
            ViewBag.InvoiceTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");




            return View();


        }
        
        [HttpPost]
        [Authorize]
        public JsonResult Create(InvoiceMain Invoicemain)
        {
            try
            {
                Invoicemain.ComId = Session["comid"].ToString();
                if (Invoicemain.ComId == null || Invoicemain.ComId == "")
                {
                    Invoicemain.ComId = "0";
                }


                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    // If Invoice main has InvoiceID then we can understand we have existing Invoice Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (Invoicemain.InvoiceId > 0)
                    {

                        IQueryable<InvoiceSub> CurrentInvoiceSUb = db.InvoiceSubs.Where(p => p.InvoiceId == Invoicemain.InvoiceId);
                        IQueryable<InvoiceTermsSub> CurrentInvoiceTermsSUb = db.InvoiceTermsSubs.Where(p => p.InvoiceId == Invoicemain.InvoiceId);


                        foreach (InvoiceSub ss in CurrentInvoiceSUb)
                        {
                            db.InvoiceSubs.Remove(ss);
                        }

                        foreach (InvoiceTermsSub sss in CurrentInvoiceTermsSUb)
                        {
                            db.InvoiceTermsSubs.Remove(sss);
                        }

 

                        foreach (InvoiceSub ss in Invoicemain.InvoiceSubs)
                        {
                            db.InvoiceSubs.Add(ss);
                        }
                        ///terms subs
                        if (Invoicemain.InvoiceTermsSubs == null)
                        { }
                        else
                        {

                            foreach (InvoiceTermsSub sss in Invoicemain.InvoiceTermsSubs)
                            {
                                db.InvoiceTermsSubs.Add(sss);
                            }
                        }


                        db.Entry(Invoicemain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        Invoicemain.InvoiceDate = DateTime.Now;
                        db.InvoiceMains.Add(Invoicemain);
                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, InvoiceID = Invoicemain.InvoiceId, ex = "" });
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
        // GET: /Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            

            if (id == null)
            {
                return BadRequest();
            }


            InvoiceMain Invoicemain = db.InvoiceMains.Find(id);

            if (Invoicemain.isPost == true)
            {
                return BadRequest();
            }

            //InvoiceMain Invoicemain = db.InvoiceMains.Find(id);



            if (Invoicemain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";



            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", Invoicemain.InvoiceSubs.);
             
            ViewBag.Customer = new SelectList(db.Customers.Where(c => c.CustomerId > 0 && c.comid.ToString() == (AppData.intComId)), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
            IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.InvoiceTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            List<ProductSerialtemp> ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(db.ChartOfAccount.Where(c => c.ComId.ToString() == (AppData.intComId) && c.AccName.Contains("CASH") && c.AccName.Contains("103070") && c.AccType == "L"), "AccId", "AccName");



            //Call Create View
            return View("Create", Invoicemain);
        }





        // GET: /Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //InvoiceMain Invoicemain = db.InvoiceMains.Find(id);
            //return View(Invoicemain);

            HttpContext.Session["isBarcodeSearch"] = true;
            HttpContext.Session["isProductSearch"] = true;
            HttpContext.Session["isIMEISearch"] = true;


            if (id == null)
            {
                return BadRequest();
            }


            InvoiceMain Invoicemain = db.InvoiceMains.Find(id);

            if (Invoicemain.isPost == true)
            {
                return BadRequest();
            }

            //InvoiceMain Invoicemain = db.InvoiceMains.Find(id);



            if (Invoicemain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";

             
            ViewBag.Customer = new SelectList(db.Customers.Where(c => c.CustomerId > 0 && c.comid.ToString() == (AppData.intComId)), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
            IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(db.Warehouses.Where(c => c.WarehouseId > 0), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.InvoiceTerms = new SelectList(db.TermsMain.Where(c => c.comid.ToString() == (AppData.intComId)), "TermsId", "TermsName");
            List<ProductSerialtemp> ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", (AppData.intComId)), new SqlParameter("userid", Session["userid"]))).ToList();
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(db.ChartOfAccount.Where(c => c.ComId.ToString() == (AppData.intComId) && c.AccName.Contains("CASH") && c.AccName.Contains("103070") && c.AccType == "L"), "AccId", "AccName");


            //Call Create View
            return View("Create", Invoicemain);
        }




        // POST: /Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //InvoiceSub Invoicesub = db.InvoiceSubs.Find(id);
                //db.InvoiceSubs.Remove(Invoicesub);

                InvoiceMain Invoicemain = db.InvoiceMains.Find(id);
                db.InvoiceMains.Remove(Invoicemain);
                db.SaveChanges();
                return Json(new { Success = 1, InvoiceID = Invoicemain.InvoiceId, ex = "" });

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
            //id = 2;  /// terms purpose.

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




      
        [HttpPost]
        public JsonResult ProductInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                Product product = db.Products.Where(y => y.ProductId == id).SingleOrDefault();
             

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



    }
}