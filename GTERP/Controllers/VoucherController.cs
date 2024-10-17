using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Rotativa;
using GTCommercial.Models;
using System.Net;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Data.Entity.Core.Objects;
using GTCommercial.Models.Common;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class VoucherController : Controller
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();


        //private int comid = int.Parse(httpre Session["comid"].ToString());
        //
        // GET: /Voucher/
        public ViewResult Index(string FromDate, string ToDate)
        {

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));



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
            AppData.intComId = "1";
            //var a = ;
            // return View(db.VoucherMains.Where(p => p.ComId == AppData.intComId).ToList());
            return View(db.VoucherMains.Where(p => p.ComId.ToString() == AppData.intComId && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList());
        }

        //
        // GET: /Voucher/Details/5

        //[OutputCache(Duration = 100, VaryByParam  = "id")]
        //[OutputCache(CacheProfile ="Admin")]
        public ViewResult Details(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            VoucherMain Vouchermain = db.VoucherMains.Find(id);
            return View(Vouchermain);
        }


        public ViewResult PrintView(int id)
        {
            //SqlCacheDependency sqldependency = new SqlCacheDependency("", "tbl");
            VoucherMain Vouchermain = db.VoucherMains.Find(id);
            return View(Vouchermain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //return View(Vouchermain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "Voucher", new { id = id });
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
            //string FormCaption = "Report :: Voucher Acknowledgement ...";


            postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "'," + Session["comid"].ToString() + ",''"));

            HttpContext.Session.SetObject("rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = ReportPath;
            clsReport.strQueryMain = SQLQuery;
            clsReport.strDSNMain = DataSourceName;

            return RedirectToAction("Index", "ReportViewer");


        }

        public async Task<JsonResult> CallComboSubSectionList()
        {
            try
            {
                //var SubSectionList = new SelectList(db.SubSections.Where(c => c.SubSectId > 0), "SubSectId", "SubSectName").ToList();

                var SubSectionList = db.SubSections.Select(e => new
                {
                    value = e.SubSectId,
                    display = e.SubSectName
                }).ToList();


                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                //MyObj SubsectionList = json_serializer.Deserialize<MyObj>(SubSectionList.ToList());
                // = SubsectionList.value;
                //refresh_token = SubsectionList.display;


                return Json(SubSectionList, JsonRequestBehavior.AllowGet);
                    
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }

        struct MyObj
        {
            public int value { get; set; }
            public string display { get; set; }


        }
        public ActionResult Create(string Type)
        {
            try
            {



            if (Type == null)
            {
                Type = "VPC";
            }
            var a = Session["isProductSearch"];
            var transactioncomid = Session["comid"].ToString();
            var transactioncompany = db.Companys.Where(c=>c.ComId.ToString() == transactioncomid).FirstOrDefault();
            Session["defaultcurrency"] = transactioncompany.CountryId;
            ViewBag.Title = "Entry";

            this.ViewBag.VoucherType = new SelectList(db.VoucherTypes, "VoucherTypeId", "VoucherTypeName");
            this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

            var ChartOfAccount = db.ChartOfAccount.Where(c => c.AccId > 0 && c.AccType == "L"); //&& c.ComId.ToString() == (AppData.intComId)
            this.ViewBag.Account = new SelectList(ChartOfAccount, "AccId", "AccName");
            //this.ViewBag.AccountSearch = ChartOfAccount;

            List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid", Session["comid"].ToString()), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
            this.ViewBag.AccountSearch = COAParent;

            var ChartOfAccountParent = db.ChartOfAccount.Where(c => c.AccId > 0 && c.AccType == "G"); //&& c.ComId.ToString() == (AppData.intComId)
            this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "AccId", "AccName");



            this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

            this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");

            //var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
            //this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            //this.ViewBag.ProductSerialSearch = ProductSerialresult;
            this.ViewBag.SubSectionList = db.SubSections;
            
            //    .Select(e => new
            //{
            //    value = e.SubSectId,
            //    display = e.SubSectName
            //}).ToList();
            return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public class VoucherChartOfAccount
        {
            //public IEnumerable<ChartOfAccount> coa { get; set; }
            public int AccId { get; set; }
            public string AccCode { get; set; }
            public string AccName { get; set; }


            public string ParentName { get; set; }
            public string Parentcode { get; set; }

            public decimal Balance { get; set; }

            public int IsChkBalance { get; set; }

            public int CountryId { get; set; }
            public int CountryIdLocal { get; set; }
            public decimal AmountLocalBuy { get; set; }
            public decimal AmountLocalSale { get; set; }
            public int isCredit { get; set; }


        }

        //public class  VoucherChartOfAccount : ChartOfAccount
        //{
        //    public string ParentName { get; set; }
        //    //public string ParentCode { get; set; }
        //    //public string Balance { get; set; }

        //    public Boolean IsChkBalance { get; set; }

        //    public int CountryId { get; set; }
        //    //public int CountryIdLocal { get; set; }
        //    public decimal AmountLocalBuy { get; set; }
        //    public decimal AmountLocalSale { get; set; }
        //    public Boolean isCredit { get; set; }





        //    //public int AccId { get; set; }

        //    //public string AccName { get; set; }
        //    //public string ProductSerialNo { get; set; }
        //}

        [HttpPost]
        public JsonResult AccountInfo(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                ChartOfAccount chartofaccount = db.ChartOfAccount.Where(y => y.AccId == id).SingleOrDefault();
                return Json(chartofaccount, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }



        // POST: /Voucher/Create

        [HttpPost]
        public JsonResult Create(VoucherMain Vouchermain)
        {
            try
            {
                Vouchermain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                //if (ModelState.IsValid)
                //if (errors.Count() < 2)

                {

                    // If sales main has VoucherID then we can understand we have existing sales Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (Vouchermain.VoucherId > 0)
                    {
                        //var CurrentProductSerial = db.ProductSerial.Where(p => p.VoucherId == Vouchermain.VoucherId);
                        var CurrentVoucherSub = db.VoucherSubs.Where(p => p.VoucherId == Vouchermain.VoucherId);
                        var CurrentVoucherSubSection = db.VoucherSubSections.Where(p => p.VoucherId == Vouchermain.VoucherId);
                        var CurrentVoucherCheck = db.VoucherSubChecnos.Where(p => p.VoucherId == Vouchermain.VoucherId);


                        //VoucherSub
                        //foreach (ProductSerial ss in CurrentProductSerial)
                        //db.ProductSerial.Remove(ss);

                        foreach (VoucherSub ss in CurrentVoucherSub)
                        db.VoucherSubs.Remove(ss);


                        foreach (VoucherSub ss in Vouchermain.VoucherSubs)
                        {
                            //db.VoucherSubs.Add(ss);
                            db.VoucherSubs.Add(ss);

                            foreach (VoucherSubSection sss in ss.VoucherSubSections)
                            {
                                db.VoucherSubSections.Add(sss);

                                foreach (VoucherSubChecno ssss in ss.VoucherSubChecnoes)
                                {
                                    db.VoucherSubChecnos.Add(ssss);

                                }


                            }

                        }
                        //foreach (VoucherSubSection sss in CurrentVoucherSubSection)
                        //db.VoucherSubSections.Remove(sss);


                        //foreach (VoucherSubChecno ssss in CurrentVoucherCheck)
                        //db.VoucherSubChecnos.Remove(ssss);


                        //if (Vouchermain.VoucherSubs == null)
                        //{
                        //}
                        //else
                        //{
                        //    foreach (VoucherSubSection subsection in Vouchermain.VoucherSubs.)
                        //    db.VoucherSubSections.Add(subsection);
                        //}


                        //if (Vouchermain.VoucherSubs == null)
                        //{
                        //}
                        //else
                        //{

                        //    //foreach (VoucherSubChecno checkno in Vouchermain.vVouchersubMain.VoucherSubChecnos)
                        //    //    db.VoucherSubChecnos.Add(checkno);
                        //}



                        db.Entry(Vouchermain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.VoucherMains.Add(Vouchermain);
                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, VoucherID = Vouchermain.VoucherId, ex = "" });
                }
            }
            catch (Exception ex)
            {

                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        //
        // GET: /Voucher/Edit/5
        public ActionResult Edit(int? id)
        {

            try
            {

          

            if (id == null)
            {
                return BadRequest();
            }


            VoucherMain Vouchermain = db.VoucherMains.Find(id);

            if (Vouchermain.isPosted == true)
            {
                return BadRequest();
            }

            //VoucherMain Vouchermain = db.VoucherMains.Find(id);



            if (Vouchermain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

                string Type = null;

                if (Type == null)
                {
                    Type = "VPC";
                }
                var a = Session["isProductSearch"];
                var transactioncomid = Session["comid"].ToString();
                var transactioncompany = db.Companys.Where(c => c.ComId.ToString() == transactioncomid).FirstOrDefault();
                Session["defaultcurrency"] = transactioncompany.CountryId;
                ViewBag.Title = "Entry";

                this.ViewBag.VoucherType = new SelectList(db.VoucherTypes, "VoucherTypeId", "VoucherTypeName");
                this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

                var ChartOfAccount = db.ChartOfAccount.Where(c => c.AccId > 0 && c.AccType == "L"); //&& c.ComId.ToString() == (AppData.intComId)
                this.ViewBag.Account = new SelectList(ChartOfAccount, "AccId", "AccName");
                //this.ViewBag.AccountSearch = ChartOfAccount;

                List<VoucherChartOfAccount> COAParent = (db.Database.SqlQuery<VoucherChartOfAccount>("[prcGetVoucherAccounts]  @comid,@Type,@dtvoucher", new SqlParameter("comid", Session["comid"].ToString()), new SqlParameter("Type", Type), new SqlParameter("dtvoucher", DateTime.Now.Date.ToString("dd-MMM-yy")))).ToList();
                this.ViewBag.AccountSearch = COAParent;

                var ChartOfAccountParent = db.ChartOfAccount.Where(c => c.AccId > 0 && c.AccType == "G"); //&& c.ComId.ToString() == (AppData.intComId)
                this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "AccId", "AccName");



                this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

                this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");

                //var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]))).ToList();
                //this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                //this.ViewBag.ProductSerialSearch = ProductSerialresult;
                this.ViewBag.SubSectionList = db.SubSections;



                //Call Create View
                return View("Create", Vouchermain);
            }
            catch (Exception ex)
            {
                string abcd = ex.InnerException.InnerException.Message.ToString();
                throw ex;
            }
        }





        // GET: /Voucher/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {



                if (id == null)
                {
                    return BadRequest();
                }


                VoucherMain Vouchermain = db.VoucherMains.Find(id);

                if (Vouchermain.isPosted == true)
                {
                    return BadRequest();
                }

                //VoucherMain Vouchermain = db.VoucherMains.Find(id);



                if (Vouchermain == null)
                {
                    return NotFound();
                }
                ViewBag.Title = "Delete";



                var a = Session["isProductSearch"];


                this.ViewBag.VoucherType = new SelectList(db.VoucherTypes, "VoucherTypeId", "VoucherTypeName");
                this.ViewBag.Category = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");

                var ChartOfAccount = db.ChartOfAccount.Where(c => c.AccId > 0 && c.AccType == "L"); //&& c.ComId.ToString() == (AppData.intComId)
                this.ViewBag.Account = new SelectList(ChartOfAccount, "AccId", "AccName");
                this.ViewBag.AccountSearch = ChartOfAccount;

                var ChartOfAccountParent = db.ChartOfAccount.Where(c => c.AccId > 0 && c.AccType == "G"); //&& c.ComId.ToString() == (AppData.intComId)
                this.ViewBag.AccountParent = new SelectList(ChartOfAccountParent, "AccId", "AccName");

                this.ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
                this.ViewBag.ProductSerial = new SelectList(db.ProductSerial.Where(c => c.ProductSerialId > 0), "ProductSerialId", "ProductSerialNo");





                //Call Create View
                return View("Create", Vouchermain);
            }
            catch (Exception ex)
            {
                string abcd = ex.InnerException.InnerException.Message.ToString();
                throw ex;
            }


        }




        // POST: /Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //VoucherSub VoucherSub = db.VoucherSubs.Find(id);
                //db.VoucherSubs.Remove(VoucherSub);

                VoucherMain Vouchermain = db.VoucherMains.Find(id);
                db.VoucherMains.Remove(Vouchermain);
                db.SaveChanges();
                return Json(new { Success = 1, VoucherID = Vouchermain.VoucherId, ex = "" });

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




            var dataset = ExecuteStoredProcedure(dbcontext, "dbo.prcgetSupplier", parameters);
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