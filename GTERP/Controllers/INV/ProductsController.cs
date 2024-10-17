using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace GTERP.Controllers.INV
{
    [OverridableAuthorize]

    public class ProductsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        private TransactionLogRepository tranlog;

        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;
        // GET: Products
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }

        public ProductsController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
            //Repository = rep;
        }

        public IActionResult Index()
        {
            var comid = (HttpContext.Session.GetString("comid"));

            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.ComId == comid), "CategoryId", "Name");

            ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(p => p.comid == comid).Where(c => c.ProductMainGroupId > 0).Select(s => new { Text = s.Name + " - [ " + s.ProductMainGroupCode + " ]", Value = s.ProductMainGroupId }).ToList(), "Value", "Text");
            ViewBag.WarehouseId = new SelectList(db.Warehouses.Where(p => p.ComId == comid && p.WhType == "L").Select(s => new { Text = s.WhName + " - [ " + s.WhShortName + " ]", Value = s.WarehouseId }).ToList(), "Value", "Text", 1);
            //ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid), "ProductMainGroupId", "Name");

            //            ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemInventory == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] Old Code[-" + s.AccCode_Old + "]", Value = s.AccId }).ToList(), "Value", "Text");
            ////          ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == 1).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            //            ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] Old Code [ - " + s.AccCode_Old + "]", Value = s.AccId }).ToList(), "Value", "Text");

            ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemInventory == true && c.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [" + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemConsumed == true && c.AccType == "L" || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [ " + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text");


            //string cultureinfo = "bd-BD";
            //string cultureinfo = "th-TH";

            //CultureInfo culture = new CultureInfo(cultureinfo, false);
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            //var products = db.Products
            //    .Include(p => p.vPrimaryCategory)
            //    .Where(p => p.ProductId > 1 && p.comid == comid).Take(5);
            //return View(products.ToList());
            return View();
        }




        [HttpPost, ActionName("SetSessionProductReport")]
        ////[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionProductReport(string rptFormat, string action, string ProductMainGroupId, string CategoryId, string AccIdInventory, string AccIdConsumption, string ProductId, string FromDate, string ToDate, string WarehouseId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintProductList")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptProductList";
                    filename = "Product_List_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec Inv_rptProductList '" + comid + "','" + ProductMainGroupId + "', '" + CategoryId + "' ,'" + AccIdInventory + "' ,'" + AccIdConsumption + "','" + WarehouseId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                }
                else if (action == "PrintAbsoluteItem")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptAbsoluteItem";
                    filename = "AbsoluteItem_List_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec Inv_rptAbsoluteItem '" + comid + "','" + ProductMainGroupId + "', '" + CategoryId + "' ,'" + AccIdInventory + "' ,'" + AccIdConsumption + "','" + WarehouseId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                }
                else if (action == "PrintSlowMovingItem")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptSlowMovingItem";
                    filename = "SlowMovingItem_List_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec Inv_rptSlowMovingItem '" + comid + "','" + ProductMainGroupId + "', '" + CategoryId + "' ,'" + AccIdInventory + "' ,'" + AccIdConsumption + "','" + WarehouseId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                }
                else if (action == "PrintYearlyInventoryReport")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptYearlyInventoryReport";
                    filename = "YearlyInventoryReport_List_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec Inv_rptYearlyInventoryReport '" + comid + "','" + ProductMainGroupId + "', '" + CategoryId + "' ,'" + AccIdInventory + "' ,'" + AccIdConsumption + "','" + WarehouseId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                }
                else if (action == "PrintLedger")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptPrdLedger_Dap";
                    filename = "ProductLedger_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec rptPrdLedger '" + comid + "', '" + FromDate + "','" + ToDate + "', '" + ProductId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");

                }
                else if (action == "KardexCard")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptKardexCard";
                    filename = "KardexCard_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec rptPrdLedger '" + comid + "', '" + FromDate + "','" + ToDate + "', '" + ProductId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");

                }
                else if (action == "prdregistervalue")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptPrdRegister_Value";
                    filename = "PrdRegister_Value_" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec rptPrdRegister_Value '" + comid + "', '" + FromDate + "','" + ToDate + "', '" + ProductId + "', '" + WarehouseId + "' , '" + CategoryId + "', 0");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");

                }
                else if (action == "rptPrdAvgRateCalculation")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptPrdAvgRateCalculation";
                    filename = "rptPrdAvgRateCalculation" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec [rptPrdAvgRateCalculation] '" + comid + "', '" + FromDate + "','" + ToDate + "', '" + WarehouseId + "' ,'0','0','0','0','" + ProductId + "'");

                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");

                }
                else if (action == "PrintInventoryReport")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptInventoryReport";
                    filename = "rptInventoryReport" + DateTime.Now.Date;
                    HttpContext.Session.SetString("reportquery", "Exec [Inv_rptInventoryReport] '" + comid + "', '" + FromDate + "','" + ToDate + "', '" + WarehouseId + "'");

                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");

                }



                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;



                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }



        // GET: Products/Create
        public IActionResult Create()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            var userid = (HttpContext.Session.GetString("userid"));

            ViewBag.Title = "Create";




            var userwiseproductlastdata = db.Products.Where(x => x.comid == comid && x.userid == userid).OrderByDescending(D => D.ProductId).FirstOrDefault();


            if (userwiseproductlastdata != null)
            {
                userwiseproductlastdata.ProductId = 0;

                ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.ComId == comid), "CategoryId", "Name", userwiseproductlastdata.CategoryId);
                ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid).Select(s => new { Text = s.Name + " - [ " + s.ProductMainGroupCode + " ]", Value = s.ProductMainGroupId }).ToList(), "Value", "Text", userwiseproductlastdata.ProductMainGroupId);
                ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(c => c.SubCategoryId == 0 && c.comid == comid), "SubCategoryId", "SubCategoryName", userwiseproductlastdata.SubCategoryId);

                ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", 18);
                ViewBag.UnitId = new SelectList(db.Unit.Where(u => u.UnitId > -1), "UnitId", "UnitName", userwiseproductlastdata.UnitId);
                ViewBag.Warehouses = db.Warehouses.Where(u => u.WarehouseId > 0 && u.ComId == comid && u.WhType == "L").ToList();


                //ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemInventory == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                //ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemInventory == true && c.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [" + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text", userwiseproductlastdata.AccIdInventory);
                ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemConsumed == true && c.AccType == "L" || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [ " + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text", userwiseproductlastdata.AccIdConsumption);


                return View(userwiseproductlastdata);


            }
            else
            {
                //ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid), "ProductMainGroupId", "Name");
                ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.ComId == comid), "CategoryId", "Name");
                ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid).Select(s => new { Text = s.Name + " - [ " + s.ProductMainGroupCode + " ]", Value = s.ProductMainGroupId }).ToList(), "Value", "Text");
                ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(c => c.SubCategoryId == 0 && c.comid == comid), "SubCategoryId", "SubCategoryName");

                ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", 18);
                ViewBag.UnitId = new SelectList(db.Unit.Where(u => u.UnitId > -1), "UnitId", "UnitName");
                ViewBag.Warehouses = db.Warehouses.Where(u => u.WarehouseId > 0 && u.ComId == comid && u.WhType == "L").ToList();


                //ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemInventory == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                //ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemInventory == true && c.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [" + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemConsumed == true && c.AccType == "L" || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [ " + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text");

            }











            return View();
        }





        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(string producta, string filename, IFormFile file, string imageDatatest)
        {

            var comid = (HttpContext.Session.GetString("comid"));
            var uploadlocation = "/Content/img/Products/";

            //Product product = JsonConvert.DeserializeObject<Product>(producta);
            var master = JObject.Parse(producta);
            var y = master["Product"].ToString();
            try
            {
                Product product = JsonConvert.DeserializeObject<Product>(y);

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });






                //if (ModelState.IsValid)
                //{
                if (product.ProductId > 0)
                {

                    //product.ProductImage = null;
                    product.DateUpdated = DateTime.Now;
                    product.comid = (HttpContext.Session.GetString("comid"));

                    if (product.userid == null)
                    {
                        product.userid = HttpContext.Session.GetString("userid");
                    }
                    product.useridUpdate = HttpContext.Session.GetString("userid");

                    if (product.ProductImage.Length > 10)
                    {
                    }
                    else
                    {
                        product.ProductImage = null;
                        //db.Entry(product).State = EntityState.Modified;
                        //db.SaveChanges();
                    }


                    foreach (Inventory inv in product.InventorySubs)
                    {
                        if (inv.InventoryId > 0)
                        {

                            inv.comid = comid;
                            inv.useridUpdate = HttpContext.Session.GetString("userid");
                            inv.DateUpdated = DateTime.Now;


                            db.Inventory.Attach(inv);
                            db.Entry(inv).Property(x => x.OpStock).IsModified = true;
                            db.Entry(inv).Property(x => x.DateUpdated).IsModified = true;
                            db.Entry(inv).Property(x => x.useridUpdate).IsModified = true;


                            //db.Inventory.Update(inv);
                            //db.Entry(inv).State = EntityState.Modified;
                            db.SaveChanges();




                            //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                            inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                            db.Entry(inv).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                        else
                        {
                            inv.ProductId = product.ProductId;
                            inv.comid = comid;
                            inv.userid = HttpContext.Session.GetString("userid");
                            inv.DateAdded = DateTime.Now;

                            db.Inventory.Add(inv);
                            db.SaveChanges();



                            //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                            inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                            db.Entry(inv).State = EntityState.Modified;
                            db.SaveChanges();

                        }


                    }


                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    if (file != null && file.Length > 0)
                    {
                        if (file.Length > 0)
                        {
                            fileName = Path.GetFileName(file.FileName);
                            Extension = Path.GetExtension(fileName);

                            product.ImagePath = uploadlocation;// + product.ProductId.ToString() + Extension.ToString();
                            product.FileExtension = Extension;

                            _FileName = product.ProductId.ToString() + Extension;
                            _path = uploadlocation + _FileName;

                            byte[] fileData = null;
                            using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
                            {
                                fileData = binaryreader.ReadBytes((int)file.Length);
                            }

                            Image cropimage = HandleImageUpload(fileData, "wwwroot" + _path);
                            MemoryStream ms = new MemoryStream();
                            cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] imageData = ms.ToArray();


                            product.ProductImage = imageData;
                            //string imageUrls = "/Content/img/Products/" + _FileName;



                            product.comid = (HttpContext.Session.GetString("comid"));
                            db.Entry(product).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        //if (product.ProductImage.Length > 10)
                        //{ 
                        //}
                        //else
                        //{
                        //    product.ProductImage = null;
                        //    //db.Entry(product).State = EntityState.Modified;
                        //    //db.SaveChanges();
                        //}
                    }
                    TempData["Status"] = "2";
                    TempData["Message"] = "Data Update Successfully";


                }
                else
                {




                    //clsMain.Dimensions dimen = new clsMain.Dimensions();
                    //_currentBitmap = Image.FromStream(file.InputStream);
                    //Bitmap cropimage = clsMain.ConstrainProportions(_currentBitmap, 300, dimen);



                    //Image image = Image.FromStream(file.InputStream);


                    //var TestImages = Bitmap.FromStream(file.InputStream);
                    //Bitmap cropimagetest = clsMain.ConstrainProportions(_currentBitmap, 300, dimen);


                    //MemoryStream ms = new MemoryStream();
                    //cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //byte[] imageData = ms.ToArray();




                    //var dataurl = Request["image-data"];
                    //var data = dataurl.Substring(dataurl.IndexOf(",") + 1);
                    ///var newfile = Convert.FromBase64String(data);


                    //product.ProductImage = imageData; //new byte[file.Length];
                    //file.InputStream.Read(product.ProductImage, 0, file.Length);

                    product.userid = HttpContext.Session.GetString("userid");
                    product.comid = (HttpContext.Session.GetString("comid"));
                    product.DateAdded = DateTime.Now;
                    product.ProductImage = null;

                    db.Products.Add(product);
                    db.SaveChanges();

                    foreach (Inventory inv in product.InventorySubs)
                    {
                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);

                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                        db.Entry(inv).State = EntityState.Modified;
                    }

                    db.Entry(product).GetDatabaseValues();
                    int id = product.ProductId; // Yes it's here

                    if (file != null && file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        product.ImagePath = "Content/img/Products/";// + product.ProductId.ToString() + Extension.ToString();
                        product.FileExtension = Extension;


                        _FileName = id.ToString() + Extension;
                        _path = uploadlocation + _FileName;
                        byte[] fileData = null;
                        using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)file.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageData = ms.ToArray();




                        //byte[] imageDatatestbytes = Convert.FromBase64String(imageDatatest);
                        //product.ProductImage = imageDatatestbytes;



                        product.ProductImage = imageData;
                        //string imageUrls = "/Content/img/Products/" + _FileName;


                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }



                    //_FileName = Path.GetFileName(DateTime.Now.ToBinary() + "-" + file.FileName);





                    //if (file != null && file.Length > 0)
                    //{
                    //    // extract only the fieldname



                    //    // store the file inside ~/App_Data/Content/img folder
                    //    //var path = Path.Combine(Server.MapPath("~/Content/img"), id.ToString() + Extension); //fileName
                    //    ////file.SaveAs(path);
                    //    //cropimage.Save(path);
                    //    //string filePath = "~/ProfilePic/" + imageData.ToString();
                    //    //file.WriteAllBytes(Server.MapPath(filePath), imgArray);

                    //    //db.Entry(product).State = EntityState.Modified;
                    //    //db.SaveChangesAsync();

                    //}
                    TempData["Status"] = "1";
                    TempData["Message"] = "Data Save Successfully";
                }

                return Json(new { Success = 1, ProductId = product.ProductId, ex = TempData["Message"].ToString() });




                //return RedirectToAction("Index");

                //}

            }
            catch (Exception ex)
            {
                //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.comid == (HttpContext.Session.GetString("comid"))), "CategoryId", "Name");
                //ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(c => c.SubCategoryId > 0 && c.comid == (HttpContext.Session.GetString("comid"))), "SubCategoryId", "SubCategoryName");

                //ViewBag.CountryId = new SelectList(db.Countries.Where(x=>x.isActive == true), "CountryId", "CurrencyShortName");
                //ViewBag.Warehouses = db.Warehouses.Where(u => u.WarehouseId > 0 && u.comid == comid && u.WhType == "L");
                TempData["Status"] = "3";
                TempData["Message"] = "Unable to Save / Update";

                return Json(new { Success = 0, ProductId = 0, ex = "Unable to Save / Update" + ex.Message });
                //return Json(new
                //{
                //    success = false,
                //    errors = ex.InnerException.InnerException.Message
                //    //ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                //});


            }


        }

        private Image RezizeImage(Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    //gr.Clear(Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new Rectangle(0, 0, nnx, nny),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        private MemoryStream BytearrayToStream(byte[] arr)
        {
            return new MemoryStream(arr, 0, arr.Length);
        }

        private Image HandleImageUpload(byte[] binaryImage, string path)
        {
            Image img = RezizeImage(Image.FromStream(BytearrayToStream(binaryImage)), 300, 300);
            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            return img;
        }

        // GET: Products/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = (HttpContext.Session.GetString("comid"));
            Product product = await (db.Products.Include(x => x.InventorySubs).ThenInclude(x => x.Warehouses).Where(c => c.ProductId == id && c.comid == comid)).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }


            //var files = Directory.GetFiles("~/Content/img/", product.ProductId + ".*");
            //if (files.Length > 0)
            //{
            //    var Extension =  Path.GetExtension(files[0].ToString());
            //    // at least one matching file exists
            //    // file name is files[0]
            //}


            ViewBag.Title = "Edit";



            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.ComId == comid), "CategoryId", "Name", product.CategoryId);
            //ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid), "ProductMainGroupId", "Name", product.ProductMainGroupId);
            ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid).Select(s => new { Text = s.Name + " - [ " + s.ProductMainGroupCode + " ]", Value = s.ProductMainGroupId }).ToList(), "Value", "Text", product.ProductMainGroupId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(c => c.SubCategoryId > 0 && c.comid == comid && c.CategoryId == product.CategoryId), "SubCategoryId", "SubCategoryName", product.SubCategoryId);

            ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", product.CountryId);
            ViewBag.UnitId = new SelectList(db.Unit.Where(u => u.UnitId > -1), "UnitId", "UnitName", product.UnitId);

            //ViewBag.Warehouses = db.Warehouses.Where(u => u.WarehouseId > 0 && u.comid == comid && u.WhType == "L");


            ViewBag.Warehouses = (from remainwarehouse in db.Warehouses.Where(m => m.WhType == "L" && m.ComId == comid)
                                  where !db.Inventory.Where(s => s.ProductId.ToString() == id.ToString()).Any(f => f.WareHouseId == remainwarehouse.WarehouseId && remainwarehouse.ComId == comid)
                                  select remainwarehouse).ToList();

            //ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemInventory == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text" , product.AccIdInventory);
            ////ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == 1).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdConsumption);
            //ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdInventory);

            ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemInventory == true && c.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [" + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdInventory);
            ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemConsumed == true && c.AccType == "L" || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [ " + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdConsumption);

            //return View(product);
            return View("Create", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit([Bind("ProductId,ProductName,ProductBarcode,CategoryId,CountryId,Description,CostPrice,SalePrice,vatPercentage,VatAmount,UnitId,ProductImage,ImagePath,FileExtension,comid,userid,useridUpdate,DateAdded,DateUpdated")] Product product, IFormFile file)
        //{
        //    var uploadlocation = "Content/img/Products/";

        //    product.DateUpdated = DateTime.Now;
        //    product.comid = (HttpContext.Session.GetString("comid"));

        //    if (product.userid == null)
        //    {
        //        product.userid = HttpContext.Session.GetString("userid");
        //    }
        //    product.useridUpdate = HttpContext.Session.GetString("userid");

        //    if (ModelState.IsValid)
        //    {

        //        //if (file == null)
        //        //{

        //        //}
        //        //else
        //        //{
        //        //    product.ProductImage = new byte[file.Length];
        //        //    file.InputStream.Read(product.ProductImage, 0, file.Length);
        //        //}




        //        db.Entry(product).State = EntityState.Modified;
        //        await db.SaveChangesAsync();

        //        if (file != null && file.Length > 0)
        //        {
        //            fileName = Path.GetFileName(file.FileName);
        //            Extension = Path.GetExtension(fileName);

        //            product.ImagePath = "Content/img/Products/";// + product.ProductId.ToString() + Extension.ToString();
        //            product.FileExtension = Extension;

        //            _FileName = product.ProductId.ToString() + Extension;
        //            _path = uploadlocation + _FileName;// Path.Combine(Server.MapPath("~/Content/img/Products"), _FileName);
        //            byte[] fileData = null;
        //            using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
        //            {
        //                fileData = binaryreader.ReadBytes((int)file.Length);
        //            }

        //            Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
        //            MemoryStream ms = new MemoryStream();
        //            cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //            byte[] imageData = ms.ToArray();


        //            product.ProductImage = imageData;

        //            db.Entry(product).State = EntityState.Modified;
        //            await db.SaveChangesAsync();
        //        }

        //        //byte[] imageDatatestbytes = Convert.FromBase64String(imageDatatest);
        //        //product.ProductImage = imageDatatestbytes;
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.comid == (HttpContext.Session.GetString("comid"))), "CategoryId", "Name" , product.CategoryId);
        //    ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(c => c.SubCategoryId > 0 && c.comid == (HttpContext.Session.GetString("comid"))), "SubCategoryId", "SubCategoryName", product.SubCategoryId);

        //    ViewBag.CountryId = new SelectList(db.Countries.Where(x=>x.isActive == true), "CountryId", "CurrencyShortName", product.CountryId);

        //    return View(product);
        //}

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product product = db.Products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);

            if (id == null)
            {
                return NotFound();
            }
            var comid = (HttpContext.Session.GetString("comid"));

            Product product = await db.Products.Where(c => c.ProductId == id && c.comid == comid).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";



            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0 && c.ComId == comid), "CategoryId", "Name", product.CategoryId);
            //ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid), "ProductMainGroupId", "Name", product.ProductMainGroupId);
            ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups.Where(c => c.ProductMainGroupId > 0 && c.comid == comid).Select(s => new { Text = s.Name + " - [ " + s.ProductMainGroupCode + " ]", Value = s.ProductMainGroupId }).ToList(), "Value", "Text", product.ProductMainGroupId);


            ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(c => c.SubCategoryId > 0 && c.comid == comid && c.CategoryId == product.CategoryId), "SubCategoryId", "SubCategoryName", product.SubCategoryId);

            ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", product.CountryId);
            ViewBag.UnitId = new SelectList(db.Unit.Where(u => u.UnitId > -1), "UnitId", "UnitName");
            ViewBag.Warehouses = db.Warehouses.Where(u => u.WarehouseId > 0 && u.ComId == comid && u.WhType == "L");


            //ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemInventory == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdInventory);
            //ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.comid == comid).Where(c => c.isItemConsumed == true || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdConsumption);



            ViewBag.AccIdInventory = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemInventory == true && c.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [" + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdInventory);
            ViewBag.AccIdConsumption = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid).Where(c => c.isItemConsumed == true && c.AccType == "L" || c.AccCode.Contains("1-2-")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ] - Old Code - [ " + s.AccCode_Old + " ]", Value = s.AccId }).ToList(), "Value", "Text", product.AccIdConsumption);

            //return View(product);
            return View("Create", product);
        }

        // POST: Products/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {

                var comid = (HttpContext.Session.GetString("comid"));

                //Product product = db.Products.Find(id);
                Product product = (db.Products.Include(x => x.InventorySubs).ThenInclude(x => x.Warehouses).Where(c => c.ProductId == id && c.comid == comid)).FirstOrDefault();

                db.Products.Remove(product);
                db.SaveChanges();


                string fullPath = ("~/wwwroot/" + product.ImagePath + product.ProductId + product.FileExtension);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return Json(new { Success = 1, ProductId = product.ProductId, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });

            //return RedirectToAction("Index");
        }

        public JsonResult getSubCategory(int id)
        {
            List<SubCategory> product = db.SubCategory.Where(x => x.CategoryId == id).ToList();

            List<SelectListItem> lisubcategory = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (SubCategory x in product)
                {
                    lisubcategory.Add(new SelectListItem { Text = x.SubCategoryName, Value = x.SubCategoryId.ToString() });
                }
            }
            return Json(new SelectList(lisubcategory, "Value", "Text"));
        }


        public class ProductResult
        {
            public int ProductId { get; set; }
            public string ProducgGroupMain { get; set; }
            public string ProductMainGroupCode { get; set; }

            public string CategoryName { get; set; }
            public string ProductName { get; set; }
            public string ProductCode { get; set; }
            public string ProductBarcode { get; set; }
            public string Description { get; set; }

            public decimal CostPrice { get; set; }
            public decimal SalePrice { get; set; }
            public string COAINV { get; set; }
            public string COACON { get; set; }

            public string ImagePath { get; set; }
            public string ProductImage { get; set; }

            public List<WarehouseResult> WarehouseList { get; set; }

            //public List<CostCalculatedResult> CostCalculatedPriceList { get; set; }


            public decimal? SingleCalculatedPrice { get; set; }


            //public DateTime BirthDate { get; set; }
            //public string BirthDateFormatted
            //{
            //    get
            //    {
            //        return String.Format("{0:M/d/yyyy}", BirthDate);
            //    }
            //}
        }

        public class WarehouseResult
        {
            public string WhShortName { get; set; }
            public decimal CurrentStock { get; set; }


        }
        //public class CostCalculatedResult
        //{
        //    public int CostCalculatedId { get; set; }
        //    public decimal? CalculatedPrice { get; set; }


        //}

        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Products.Include(y => y.vPrimaryCategory);
                var query = from e in db.Products.Where(x => x.ProductId > 0 && x.comid == comid).OrderByDescending(x => x.ProductId)
                                //let FullName = e.ProductName + " " + e.ProductCode
                                //let CostCalculatedValueasdf = e.CostCalculated != null ? e.CostCalculated.Select(x => new CostCalculatedResult { CostCalculatedId = x.CostCalculatedId, CalculatedPrice = x.CalculatedPrice }).OrderByDescending(x => x.CostCalculatedId).ToList(): null
                            let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                            //let WarehouseQty = e.InventorySubs != null ? e.InventorySubs.Select(x => new WarehouseResult { WhShortName = x.Warehouses.WhShortName, CurrentStock = x.CurrentStock }).ToList() : null
                            // let WarehouseQty =  e.CostCalculated != null ? e.CostCalculated.OrderByDescending(x=>x.CostCalculatedId).Select(x => new WarehouseResult { WhShortName = x.Warehouses.WhShortName, CurrentStock = x.CurrQty+x.PrevQty }).ToList() : null

                            select new ProductResult
                            {
                                ProductId = e.ProductId,
                                ProducgGroupMain = e.vProductMainGroup.Name,
                                ProductMainGroupCode = e.vProductMainGroup.ProductMainGroupCode,

                                CategoryName = e.vPrimaryCategory.Name,
                                ProductName = e.ProductName,

                                ProductCode = e.ProductCode,
                                ProductBarcode = e.ProductBarcode,
                                Description = e.Description,
                                CostPrice = e.CostPrice,
                                SalePrice = e.SalePrice,

                                COAINV = e.ChartOfAccountsInventory.AccCode,
                                COACON = e.ChartOfAccountsConsumption.AccCode,

                                ImagePath = ImagePath,
                                ProductImage = e.ProductImage != null ? Convert.ToBase64String(e.ProductImage) : null,//(asset.ProductImage != null) ? (asset.ProductImage) : null,
                                WarehouseList = null,// WarehouseQty,

                                //SingleCalculatedPrice = 0//CostCalculatedValueasdf != null ? CostCalculatedValueasdf.CalculatedPrice : 0//CostCalculatedValue.LastOrDefault().CalculatedPrice
                                //SingleCalculatedPrice = CostCalculatedValueasdf.FirstOrDefault() != null ? CostCalculatedValueasdf.FirstOrDefault().CalculatedPrice : 0

                                //CostCalculatedPriceList = CostCalculatedValueasdf //!= null ? CostCalculatedValueasdf : null
                                SingleCalculatedPrice = e.CostCalculated.OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CalculatedPrice).FirstOrDefault()
                            };



                var parser = new Parser<ProductResult>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //[HttpPost]
        //public IActionResult Get()
        //{
        //    try
        //    {


        //        IQueryable<Product> query = db.Products;
        //        int totalCount = query.Count();

        //        #region Filtering
        //        // Apply filters for searching
        //        if (requestModel.Search.Value != string.Empty)
        //        {
        //            string value = requestModel.Search.Value.Trim();
        //            query = query.Where(p => p.ProductId.ToString().Contains(value) ||
        //                                     p.vPrimaryCategory.Name.Contains(value) ||

        //                                                           p.ProductName.Contains(value) ||
        //                                                           p.ProductBarcode.Contains(value) ||
        //                                                           p.ProductCode.Contains(value) ||
        //                                                           p.Description.Contains(value)



        //                              //p.COM_GroupLC_Subs.FirstOrDefault().COM_GroupLC_Mains.GroupLCRefName.Contains(value) ||
        //                              //p.OpeningBank.OpeningBankName.Contains(value) ||

        //                              ////p.MasterLCValueManual.ToString().Contains(value) ||


        //                              ////p.LCValue.ToString().Contains(value) ||
        //                              ////p.Currency.CurCode.ToString().Contains(value) ||
        //                              //p.Destinations.DestinationName.ToString().Contains(value) ||

        //                              ////p.LienBank.LienBankName.ToString().Contains(value) ||
        //                              //p.PortOfLoading.PortOfLoadingName.ToString().Contains(value) ||
        //                              ////p.UnitMaster.UnitName.ToString().Contains(value) ||
        //                              //p.LCRefNo.Contains(value) ||
        //                              ////p.LCType.LCTypeName.Contains(value) ||
        //                              ////p.LCOpenDate.ToString().Contains(value) ||

        //                              ////p.LCExpirydate.ToString().Contains(value) ||
        //                              ////p.LCStatus.LCStatusName.ToString().Contains(value) ||

        //                              ////p.LCNature.LCNatureName.ToString().Contains(value) ||




        //                              //  //p.Tenor.ToString().Contains(value) ||
        //                              //  //p.TradeTerms.TradeTermName.ToString().Contains(value) ||
        //                              //  //p.PaymentTerms.PaymentTermsName.ToString().Contains(value) ||

        //                              //  //p.DayList.DayListName.ToString().Contains(value) ||
        //                              //  p.FileNo.ToString().Contains(value) ||

        //                              //  p.LCNOforImport.ToString().Contains(value)



        //                              );
        //        }

        //        int filteredCount = query.Count();

        //        #endregion Filtering

        //        #region Sorting
        //        // Sorting
        //        IOrderedEnumerable<Column> sortedColumns = requestModel.Columns.GetSortedColumns();
        //        string orderByString = string.Empty;

        //        foreach (Column column in sortedColumns)
        //        {
        //            orderByString += orderByString != string.Empty ? "," : "";
        //            orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
        //        }

        //        //query = query.OrderBy(orderByString == string.Empty ? "MasterLCID asc" : orderByString);

        //        #endregion Sorting


        //        int takelength = 0;
        //        if (requestModel.Length == -1)
        //        {
        //            takelength = totalCount;
        //        }
        //        else
        //        {
        //            takelength = requestModel.Length;
        //        }

        //        // Paging
        //        query = query.Skip(requestModel.Start).Take(takelength);


        //        var data = query.Select(asset => new
        //        {
        //            ProductId = asset.ProductId,
        //            CategoryName = asset.vPrimaryCategory.Name,
        //            ProductName = asset.ProductName,
        //            ProductBarcode = asset.ProductBarcode,
        //            COAINV = asset.ChartOfAccountsInventory.AccCode,
        //            COACON = asset.ChartOfAccountsConsumption.AccCode,

        //            //ExportPONo = (asset.ExportInvoiceDetails != null) ? asset.ExportInvoiceDetails.FirstOrDefault().COM_MasterLC_Detail.ExportPONo : "",
        //            ProductCode = asset.ProductCode,
        //            Description = asset.Description,
        //            CostPrice = asset.CostPrice,
        //            SalePrice = asset.SalePrice,
        //            ImagePath = asset.ImagePath + asset.ProductId + asset.FileExtension,
        //            //ProductImage = Convert.ToBase64String(asset.ProductImage) ,//(asset.ProductImage != null) ? (asset.ProductImage) : null,
        //            WarehouseQty = asset.InventorySubs.Select(x => new { x.Warehouses.WhShortName, x.CurrentStock })
        //            //,TotalQty = (asset.InventorySubs != null) ?  asset.InventorySubs.Sum(x => x.CurrentStock) : 0
        //             // ,TotalQty = (asset.InventorySubs != null) ? asset.InventorySubs.Sum(x => x.CurrentStock) : 0


        //        }).ToList();

        //        //data.ForEach(s => s.ProductImage = (s.ProductImage == null ? "noimage" : Convert.ToBase64String(s.ProductImage)));

        //        return Json(new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount));


        //        //int filteredCount;
        //        //IOrderedEnumerable<Column> sortedColumns;
        //        //string orderByString;
        //        //int takelength = 0;
        //        //int totalCount = 0;
        //        //var comid = Session["comid"];

        //        //if (requestModel.Search.Value != string.Empty)
        //        //{
        //        //IQueryable<Product> enumer = db.Products;//.Where(p => p.userid.ToString().Equals(UserList) && (p.InvoiceDate.Date >= Convert.ToDateTime(FromDate.ToString()) && p.InvoiceDate.Date <= Convert.ToDateTime(ToDate.ToString())));

        //        //    //var sql = "select r.buyerid,BuyerGroupName,BuyerName,DynamicReportCaption,destinationnamesearch from dynamicreports r inner join BuyerInformations b on b.BuyerId = r.BuyerId inner join buyergroups g on g.buyergroupid = b.BuyerGroupId left outer join Destinations d on d.DestinationID = r.DestinationId where BuyerName like '%H&M %' order by BuyerGroupName ";
        //        //    //var querysql = sql;


        //        //    //IQueryable<Product> query = from o in db.Products.Where(x => x.comid == 4)
        //        //    //                                            //join od in db.BuyerInformation on o.BuyerId equals od.BuyerId //inner join
        //        //    //                                            //join c in db.Destinations on o.DestinationId equals c.DestinationID //outer join
        //        //    //                                            //from t in co.DefaultIfEmpty()
        //        //    //                                            //join e in db.DynamicReports on o.DestinationId equals e.DestinationId  into eo //outer join
        //        //    //                                        join e in db.DynamicReports on new { o.DestinationId, o.BuyerId } equals new { e.DestinationId, e.BuyerId } into eo //outer join
        //        //    //                                        from z in eo.DefaultIfEmpty()
        //        //    //                                        //join et in db.DynamicReports on od.BuyerId equals et.BuyerId into fo 
        //        //    //                                        //from zz in fo.DefaultIfEmpty()
        //        //    //                                            //where (o.comid == 4)
        //        //    //                                            //orderby o.ProductId descending
        //        //    //                                        select o;

        //        //    totalCount = query.Count();


        //        //    #region Filtering
        //        //    string value = requestModel.Search.Value.Trim();
        //        //    query = query.Where(p => p.ProductId.ToString().Contains(value) ||
        //        //                             p.vPrimaryCategory.Name.Contains(value) ||

        //        //                             p.ProductName.Contains(value) ||
        //        //                             p.ProductBarcode.Contains(value) ||
        //        //                             p.ProductCode.Contains(value) ||
        //        //                             p.Description.Contains(value) ||

        //        //                             //p.ExportInvoiceDetails.FirstOrDefault().COM_MasterLC_Detail.ExportPONo.Contains(value) ||

        //        //                             //p.ProductImage.ToString().Contains(value)
        //        //                             //p.ImagePath.ToString().Contains(value) ||
        //        //                             //p.FileExtension.ToString().Contains(value) ||



        //        //                             p.CostPrice.ToString().Contains(value) ||

        //        //                             //p.CostPrice.ToString().Contains(value) ||
        //        //                             //p.CostPrice.ToString().Contains(value) ||


        //        //                             p.SalePrice.ToString().Contains(value)



        //        //                             //p.Destination.DestinationNameSearch.ToString().Contains(value)

        //        //                             );
        //        //    //&& (p.InvoiceDate.Date >= Convert.ToDateTime(FromDate) && p.InvoiceDate.Date <= Convert.ToDateTime(ToDate))
        //        //    //&& (p.userid.ToString() == UserList.ToString()) 

        //        //    filteredCount = query.Count();
        //        //    #endregion Filtering

        //        //    #region Sorting
        //        //    // Sorting
        //        //    sortedColumns = requestModel.Columns.GetSortedColumns();
        //        //    orderByString = string.Empty;


        //        //    foreach (Column column in sortedColumns)
        //        //    {
        //        //        orderByString += orderByString != string.Empty ? "," : "";
        //        //        orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
        //        //    }

        //        //    query = query.OrderBy(orderByString == string.Empty ? "ProductId asc" : orderByString);

        //        //    #endregion Sorting


        //        //    if (requestModel.Length == -1)
        //        //    {
        //        //        takelength = totalCount;
        //        //    }
        //        //    else
        //        //    {
        //        //        takelength = requestModel.Length;
        //        //    }



        //        //    query = query.Skip(requestModel.Start).Take(takelength);

        //        //    //error checking purpose. if error arise then we can find the error by running this code
        //        //    var dataenmertest = query.ToList();
        //        //    var listfahad = new List<dynamic>();
        //        //    int i = 0;
        //        //    foreach (var asset in dataenmertest)
        //        //    {


        //        //        var listofwarehouse = new List<dynamic>();
        //        //        int totalqty = 0;
        //        //        ///multiple line ware house added
        //        //        foreach (var item in asset.InventorySubs)
        //        //        {
        //        //            listofwarehouse.Add(new
        //        //            {
        //        //                warehouseshortname = item.Warehouses.WhShortName,
        //        //                inventoryqty = item.CurrentStock
        //        //            });
        //        //            totalqty = totalqty + int.Parse(item.CurrentStock.ToString());

        //        //        }
        //        //        ///total qty added
        //        //        listofwarehouse.Add(new
        //        //        {
        //        //            warehouseshortname = "Total",
        //        //            inventoryqty = totalqty
        //        //        });


        //        //        listfahad.Add(new
        //        //        {
        //        //            ProductId = asset.ProductId,
        //        //            CategoryName = asset.vPrimaryCategory.Name,
        //        //            ProductName = asset.ProductName,
        //        //            ProductBarcode = asset.ProductBarcode,
        //        //            //ExportPONo = (asset.ExportInvoiceDetails != null) ? asset.ExportInvoiceDetails.FirstOrDefault().COM_MasterLC_Detail.ExportPONo : "",
        //        //            ProductCode = asset.ProductCode,
        //        //            Description = asset.Description,
        //        //            CostPrice = asset.CostPrice,
        //        //            SalePrice = asset.SalePrice,
        //        //            ImagePath = asset.ImagePath + asset.ProductId + asset.FileExtension,
        //        //            ProductImage = asset.ProductImage,
        //        //            totalInv = new List<dynamic>() { listofwarehouse }




        //        //            //if (asset.InventorySubs != null)
        //        //            //{
        //        //            //    foreach (var x in asset.InventorySubs)
        //        //            //    {
        //        //            //        termssubslists.Add(new SelectListItem { Text = x.Warehouses.WhShortName.ToString(), Value = x.CurrentStock.ToString() });

        //        //            //    };
        //        //            //}


        //        //            //, DestinationNameSearch = asset.Destination.DestinationNameSearch
        //        //        });

        //        //        i++;
        //        //    }




        //        //    //}).ToList();

        //        //    return Json(new DataTablesResponse(requestModel.Draw, listfahad, filteredCount, totalCount), JsonRequestBehavior.AllowGet);


        //        //}
        //        //else
        //        //{
        //        //    //IEnumerable<Product> enumer = db.Products;//.Where(p => p.userid.ToString().Equals(UserList) && (p.InvoiceDate.Date >= Convert.ToDateTime(FromDate.ToString()) && p.InvoiceDate.Date <= Convert.ToDateTime(ToDate.ToString())));




        //        //    #region Filtering
        //        //    // Apply filters for searching

        //        //    totalCount = enumer.Count();
        //        //    filteredCount = enumer.Count();

        //        //    #endregion Filtering

        //        //    #region Sorting

        //        //    // Sorting

        //        //    sortedColumns = requestModel.Columns.GetSortedColumns();
        //        //    orderByString = string.Empty;

        //        //    foreach (Column column in sortedColumns)
        //        //    {
        //        //        orderByString += orderByString != string.Empty ? "," : "";
        //        //        orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
        //        //    }
        //        //    #endregion Sorting

        //        //    if (requestModel.Length == -1)
        //        //    {
        //        //        takelength = totalCount;
        //        //    }
        //        //    else
        //        //    {
        //        //        takelength = requestModel.Length;
        //        //    }

        //        //    // Paging
        //        //    enumer = enumer.Skip(requestModel.Start).Take(takelength);






        //        //    var dataenmer = enumer.Select(asset => new
        //        //    {
        //        //        ProductId = asset.ProductId,
        //        //        CategoryName = asset.vPrimaryCategory.Name,
        //        //        ProductName = asset.ProductName,
        //        //        ProductBarcode = asset.ProductBarcode,
        //        //        //ExportPONo = (asset.ExportInvoiceDetails != null) ? asset.ExportInvoiceDetails.FirstOrDefault().COM_MasterLC_Detail.ExportPONo : "",
        //        //        ProductCode = asset.ProductCode,
        //        //        Description = asset.Description,
        //        //        CostPrice = asset.CostPrice,
        //        //        SalePrice = asset.SalePrice,
        //        //        ImagePath = asset.ImagePath + asset.ProductId + asset.FileExtension,
        //        //        ProductImage = asset.ProductImage,
        //        //        totalInv = asset.InventorySubs

        //        //    }).ToList();

        //        //    return Json(new DataTablesResponse(requestModel.Draw, dataenmer, filteredCount, totalCount), JsonRequestBehavior.AllowGet);


        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}



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
