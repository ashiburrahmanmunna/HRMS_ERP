using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers
{
    [OverridableAuthorize]
    public class PurchaseController : Controller
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();
        private TransactionLogRepository tranlog;
        private POSRepository POS;
        private readonly IConfiguration _configuration;

        public PurchaseController(IConfiguration configuration, GTRDBContext context, TransactionLogRepository tran, POSRepository pos)
        {
            tranlog = tran;
            POS = pos;
            db = context;
            _configuration = configuration;
        }


        public class GetUserModel
        {
            public string AppKey { get; set; }

        }
        //private string comid = int.Parse(httpre HttpContext.Session.GetString("comid").ToString());
        //
        // GET: /Purchase/
        public ViewResult Index(string FromDate, string ToDate, string UserList, int? SupplierId)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

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


            if (UserList == null)
            {
                UserList = HttpContext.Session.GetString("userid");
            }
            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = appKey.ToString();
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            //Uri url = new Uri(string.Format("http://101.2.165.187:82/api/user/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }

            ViewBag.Userlist = new SelectList(l, "UserId", "UserName", userid);


            ViewBag.SupplierList = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");


            //var a = ;
            // return View(db.PurchaseMains.Where(p => p.ComId == AppData.AppData.intComId).ToList());
            //var a = db.PurchaseMains.Where(p => p.comid == AppData.AppData.intcomid && (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo)).ToList();
            return View();
        }

        public class PurchaseResult
        {
            public int PurchaseId { get; set; }
            public string PurchaseNo { get; set; }
            public string SupplierName { get; set; }
            public string PurchasePerson { get; set; }
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime PurchaseDate { get; set; }

            public string ReferenceNo { get; set; }
            public float NetAmount { get; set; }

            public float PaidAmount { get; set; }
            public float Due { get; set; }
            [NotMapped]
            public string PurchaseDateFormatted
            {
                get
                {
                    return string.Format("{0:d-MMM-yyyy}", PurchaseDate);
                }
            }
            //public string PurchaseDateFormatted
            //{
            //    get
            //    {
            //        return String.Format("{0:d-MMM-yyyy}", PurchaseDate);
            //    }
            //    set
            //    {

            //    }
            //}
        }
        public IActionResult Get(string UserList, string FromDate, string ToDate, string SupplierList, int isAll)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

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

                Microsoft.Extensions.Primitives.StringValues y = "";

                var x = Request.Form.TryGetValue("search[value]", out y);

                if (y.ToString().Length > 0)
                {
                    var query = from e in db.PurchaseMains.Where(x => x.comid == comid).OrderByDescending(x => x.PurchaseId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                select new PurchaseResult
                                {
                                    PurchaseId = e.PurchaseId,
                                    PurchaseNo = e.PurchaseNo,
                                    SupplierName = e.SupplierName,

                                    PurchasePerson = e.PurchasePerson,
                                    PurchaseDate = Convert.ToDateTime(e.PurchaseDate),
                                    //PurchaseDateFormatted = Convert.ToDateTime(e.PurchaseDate).ToString("dd-MMM-yy"),
                                    ReferenceNo = e.ReferenceNo,
                                    NetAmount = e.NetAmount,
                                    PaidAmount = e.PaidAmt,
                                    Due = e.DueAmt
                                };



                    var parser = new Parser<PurchaseResult>(Request.Form, query);
                    //  .SetConverter(x => x.PurchaseDate, x => x.PurchaseDate.ToString("dd-MM-yyyy"));
                    //.SetConverter(x => x.PurchaseDate, x => GTRDBContext.Format(x.PurchaseDate, "M/dd/yyyy"));

                    return Json(parser.Parse());
                }
                else
                {
                    //var query = db.PurchaseMains.Where(x => x.comid == comid)
                    //.Where(p => (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo)).AsQueryable();


                    //if (UserList != null)
                    //{
                    //    query.Where(p => p.userid.Contains(UserList));
                    //}
                    //if (SupplierList != null)
                    //{
                    //    query.Where(p => p.userid.Contains(SupplierList));
                    //}

                    //var finalquery = from e in query
                    //.OrderByDescending(x => x.PurchaseId)
                    //    let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                    //select new PurchaseResult
                    //{
                    //    PurchaseId = e.PurchaseId,
                    //    PurchaseNo = e.PurchaseNo,
                    //    SupplierName = e.SupplierName,

                    //    PurchasePerson = e.PurchasePerson,
                    //    PurchaseDate = Convert.ToDateTime(e.PurchaseDate),
                    //    PurchaseDateFormatted = Convert.ToDateTime(e.PurchaseDate).ToString("dd-MMM-yy"),
                    //    ReferenceNo = e.ReferenceNo,
                    //    NetAmount = e.NetAmount,
                    //    PaidAmount = e.PaidAmt,
                    //    Due = e.DueAmt
                    //};

                    if (SupplierList != null && UserList != null)
                    {
                        var querytest = from e in db.PurchaseMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.userid.ToLower().Contains(UserList.ToLower()))
                        .Where(p => p.SupplierId == int.Parse(SupplierList))

                        .OrderByDescending(x => x.PurchaseId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseResult
                                        {
                                            PurchaseId = e.PurchaseId,
                                            PurchaseNo = e.PurchaseNo,
                                            SupplierName = e.SupplierName,
                                            PurchasePerson = e.PurchasePerson,
                                            PurchaseDate = Convert.ToDateTime(e.PurchaseDate),
                                            //PurchaseDateFormatted = Convert.ToDateTime(e.PurchaseDate).ToString("dd-MMM-yy"),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<PurchaseResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (SupplierList != null && UserList == null)
                    {
                        var querytest = from e in db.PurchaseMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.SupplierId == int.Parse(SupplierList))

                        .OrderByDescending(x => x.PurchaseId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseResult
                                        {
                                            PurchaseId = e.PurchaseId,
                                            PurchaseNo = e.PurchaseNo,
                                            SupplierName = e.SupplierName,
                                            PurchasePerson = e.PurchasePerson,
                                            PurchaseDate = Convert.ToDateTime(e.PurchaseDate),
                                            //PurchaseDateFormatted = Convert.ToDateTime(e.PurchaseDate).ToString("dd-MMM-yy"),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<PurchaseResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (SupplierList == null && UserList != null)
                    {

                        var querytest = from e in db.PurchaseMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.userid.ToLower().Contains(UserList.ToLower()))

                        .OrderByDescending(x => x.PurchaseId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseResult
                                        {
                                            PurchaseId = e.PurchaseId,
                                            PurchaseNo = e.PurchaseNo,
                                            SupplierName = e.SupplierName,
                                            PurchasePerson = e.PurchasePerson,
                                            PurchaseDate = Convert.ToDateTime(e.PurchaseDate),
                                            //PurchaseDateFormatted = Convert.ToDateTime(e.PurchaseDate).ToString("dd-MMM-yy"),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<PurchaseResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in db.PurchaseMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.PurchaseDate >= dtFrom && p.PurchaseDate <= dtTo))

                        .OrderByDescending(x => x.PurchaseId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new PurchaseResult
                                        {
                                            PurchaseId = e.PurchaseId,
                                            PurchaseNo = e.PurchaseNo,
                                            SupplierName = e.SupplierName,
                                            PurchasePerson = e.PurchasePerson,
                                            PurchaseDate = Convert.ToDateTime(e.PurchaseDate),
                                            //PurchaseDateFormatted = Convert.ToDateTime(e.PurchaseDate).ToString("dd-MMM-yy"),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<PurchaseResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }




                    //  .SetConverter(x => x.PurchaseDate, x => x.PurchaseDate.ToString("dd-MM-yyyy"));
                    //.SetConverter(x => x.PurchaseDate, x => GTRDBContext.Format(x.PurchaseDate, "M/dd/yyyy"));

                }
                return Json(new { Success = "1" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        public ViewResult PrintView(int id)
        {

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





        public ActionResult Create()
        {


            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();


            var a = HttpContext.Session.GetString("isProductSearch");
            ViewBag.Title = "Entry";

            this.ViewBag.PurchaseType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(POS.GetSupplier(comid), "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);// db.Products.Where(c => c.ProductId > 0 && c.comid == comid);
            this.ViewBag.Product = new SelectList(Productresult.Where(x => x.comid == comid), "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(Productresult.Where(x => x.comid == comid), "ProductId", "ProductBarcode");
            this.ViewBag.ProductSearch = Productresult;
            this.ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyName");
            this.ViewBag.ProductSerial = new SelectList(POS.GetProductSerial(comid), "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.ProductSerialSearch = ProductSerialresult;

            this.ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            this.ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");


            return View();


        }


        public ActionResult CreatePurchase()
        {

            var a = HttpContext.Session.GetString("isProductSearch");
            ViewBag.Title = "Create";
            this.ViewBag.PurchaseType = new SelectList(db.SalesType, "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            this.ViewBag.Product = new SelectList(db.Products, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(db.Products, "ProductId", "ProductBarcode");
            this.ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyName");
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
                        db.PurchaseTermsSubs.RemoveRange(CurrentsalesTermsSUb);
                        db.SaveChanges();


                        foreach (PurchaseSub ss in CurrentPurchaseSub)
                        {
                            db.PurchaseSubs.Remove(ss);

                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null)
                            {
                                inv.PurQty = inv.PurQty - ss.Qty;
                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                db.Entry(inv).State = EntityState.Modified;
                            }
                        }

                        foreach (PurchaseSub ss in purchasemain.PurchaseSubs)
                        {
                            db.PurchaseSubs.Add(ss);
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null)
                            {
                                inv.PurQty = inv.PurQty + ss.Qty;
                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);
                                db.Entry(inv).State = EntityState.Modified;
                            }

                        }

                        //foreach (PurchaseTermsSub sss in CurrentsalesTermsSUb)
                        //    db.PurchaseTermsSubs.Remove(sss);



                        if (purchasemain.PurchaseTermsSubs == null)
                        {
                        }
                        else
                        {

                            foreach (PurchaseTermsSub sss in purchasemain.PurchaseTermsSubs)
                                db.PurchaseTermsSubs.Add(sss);
                        }
                        //db.SaveChanges();





                        db.Entry(purchasemain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.PurchaseMains.Add(purchasemain);


                        foreach (PurchaseSub ss in purchasemain.PurchaseSubs)
                        {
                            db.PurchaseSubs.Add(ss);
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                            {

                                inv.PurQty = inv.PurQty + ss.Qty;
                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                db.Entry(inv).State = EntityState.Modified;
                            }

                        }


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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PurchaseMain purchasemain = await

            db.PurchaseMains
            .Include(x => x.PurchaseSubs).ThenInclude(x => x.vProductName)
            .Include(x => x.PurchaseSubs).ThenInclude(x => x.ProductSerials)
            .Include(x => x.PurchaseTermsSubs)
            .Include(x => x.PurchasePaymentSubs).ThenInclude(x => x.vChartofAccounts)
            .Include(x => x.PurchasePaymentSubs).ThenInclude(x => x.vPaymentType)
            .Where(x => x.PurchaseId == id).FirstOrDefaultAsync();



            if (purchasemain.isPost == true)
            {
                return NotFound();
            }


            if (purchasemain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();





            this.ViewBag.PurchaseType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(POS.GetSupplier(comid).Where(c => c.SupplierId > 0 && c.ComId == comid), "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");

            var Productresult = POS.GetProducts(comid);
            this.ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            this.ViewBag.ProductSearch = Productresult;

            this.ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyName");

            this.ViewBag.ProductSerial = new SelectList(POS.GetProductSerial(comid), "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(db.TermsMain.Where(c => c.comid == comid), "TermsId", "TermsName");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.ProductSerialSearch = ProductSerialresult;

            this.ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            this.ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");



            //Call Create View
            return View("Create", purchasemain);
        }





        // GET: /Purchase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //PurchaseMain purchasemain = db.PurchaseMains.Find(id);
            //return View(purchasemain);

            HttpContext.Session.SetString("isBarcodeSearch", "true");
            HttpContext.Session.SetString("isProductSearch", "true");
            HttpContext.Session.SetString("isIMEISearch", "true");


            if (id == null)
            {
                return NotFound();
            }


            PurchaseMain purchasemain = await

            db.PurchaseMains
            .Include(x => x.PurchaseSubs).ThenInclude(x => x.vProductName)
            .Include(x => x.PurchaseSubs).ThenInclude(x => x.ProductSerials)
            .Include(x => x.PurchaseTermsSubs)
            .Include(x => x.PurchasePaymentSubs).ThenInclude(x => x.vChartofAccounts)
            .Include(x => x.PurchasePaymentSubs).ThenInclude(x => x.vPaymentType)
            .Where(x => x.PurchaseId == id).FirstOrDefaultAsync();

            if (purchasemain.isPost == true)
            {
                return NotFound();
            }


            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();
            //PurchaseMain purchasemain = db.PurchaseMains.Find(id);



            if (purchasemain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";


            this.ViewBag.PurchaseType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            this.ViewBag.Supplier = new SelectList(POS.GetSupplier(comid), "SupplierId", "SupplierName");
            this.ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");

            var Productresult = POS.GetProducts(comid);
            this.ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            this.ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            this.ViewBag.ProductSearch = Productresult;

            this.ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            this.ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyName");

            this.ViewBag.ProductSerial = new SelectList(POS.GetProductSerial(comid), "ProductSerialId", "ProductSerialNo");
            this.ViewBag.PurchaseTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            //this.ViewBag.ProductSerialSearch = db.ProductSerial.Where(c => c.ProductSerialId > 0);
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            this.ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            this.ViewBag.ProductSerialSearch = ProductSerialresult;

            this.ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            this.ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");

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

                //PurchaseMain purchasemain = db.PurchaseMains.Find(id);


                PurchaseMain purchasemain = db.PurchaseMains
                 .Include(x => x.PurchaseSubs).ThenInclude(x => x.vProductName)
                 .Include(x => x.PurchaseSubs).ThenInclude(x => x.ProductSerials)
                 .Include(x => x.PurchaseTermsSubs)
                 .Include(x => x.PurchasePaymentSubs).ThenInclude(x => x.vChartofAccounts)
                 .Where(x => x.PurchaseId == id).FirstOrDefault();

                db.PurchaseMains.Remove(purchasemain);

                foreach (var ss in purchasemain.PurchaseSubs)
                {


                    Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                    if (inv != null)
                    {
                        inv.PurQty = inv.PurQty - ss.Qty;
                        //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                        db.Entry(inv).State = EntityState.Modified;
                    }
                }

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
            return Json(new SelectList(termssubslists, "Value", "Text"));
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
            return Json(new SelectList(licities, "Value", "Text"));
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
            return Json(new SelectList(barcodelist, "Value", "Text"));
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
            return Json(new SelectList(productseriallist, "Value", "Text"));
        }


        [HttpPost]
        public JsonResult ProductInfo(int id)
        {
            try
            {


                var product = db.Products.Where(y => y.ProductId == id).SingleOrDefault();

                return Json(product);
                //return Json("tesst" );

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
            //return Json(new SelectList(product, "Value", "Text" ));
        }




        [HttpPost]
        public JsonResult SupplierInfo(int id)
        {
            try
            {




                var customer = db.Suppliers.Where(y => y.SupplierId == id).SingleOrDefault();
                return Json(customer);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
        }


        public static class DropDownList<T>
        {
            public static SelectList LoadItems(IList<T> collection, string value, string text)
            {
                return new SelectList(collection, value, text);
            }
        }



    }
}