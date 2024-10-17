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
    //[OverridableAuthorize]
    public class EcommerceController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();
        private TransactionLogRepository tranlog;
        private POSRepository POS;
        private readonly IConfiguration _configuration;

        public EcommerceController(IConfiguration configuration, GTRDBContext context, TransactionLogRepository tran, POSRepository pos)
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
        // GET: /Sales/
        public ViewResult Index(string FromDate, string ToDate, string UserList, int? CustomerId)
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



            ///////////get user list from the server //////

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = appKey.ToString();
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);

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


            //List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria , @comid", new SqlParameter("Criteria", "PosSales"), new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
            //ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName", UserList);

            ViewBag.CustomerList = new SelectList(db.Customers.Where(x => x.comid == comid), "CustomerId", "CustomerName");

            return View();
        }



        public class SalesResult
        {
            public int SalesId { get; set; }
            public string SalesNo { get; set; }
            public string CustomerName { get; set; }
            public string SalesPerson { get; set; }
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime SalesDate { get; set; }
            public string ReferenceNo { get; set; }
            public decimal NetAmount { get; set; }

            public decimal PaidAmount { get; set; }
            public decimal Due { get; set; }
            [NotMapped]
            public string SalesDateFormatted
            {
                get
                {
                    return string.Format("{0:d-MMM-yyyy}", SalesDate);
                }
            }
        }
        public IActionResult Get(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
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


                    var query = from e in db.SalesMains//.Where(x => x.comid == comid)
                                .OrderByDescending(x => x.SalesId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                select new SalesResult
                                {
                                    SalesId = e.SalesId,
                                    SalesNo = e.SalesNo,
                                    CustomerName = e.CustomerName,
                                    SalesPerson = e.SalesPerson,
                                    SalesDate = Convert.ToDateTime(e.SalesDate),
                                    ReferenceNo = e.ReferenceNo,
                                    NetAmount = e.NetAmount,
                                    PaidAmount = e.PaidAmt,
                                    Due = e.DueAmt
                                };


                    var parser = new Parser<SalesResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = from e in db.SalesMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.SalesDate >= dtFrom && p.SalesDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.userid.ToLower().Contains(UserList.ToLower()))
                        .Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.SalesId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new SalesResult
                                        {
                                            SalesId = e.SalesId,
                                            SalesNo = e.SalesNo,
                                            CustomerName = e.CustomerName,
                                            SalesPerson = e.SalesPerson,
                                            SalesDate = Convert.ToDateTime(e.SalesDate),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<SalesResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = from e in db.SalesMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.SalesDate >= dtFrom && p.SalesDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.SalesId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new SalesResult
                                        {
                                            SalesId = e.SalesId,
                                            SalesNo = e.SalesNo,
                                            CustomerName = e.CustomerName,
                                            SalesPerson = e.SalesPerson,
                                            SalesDate = Convert.ToDateTime(e.SalesDate),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<SalesResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = from e in db.SalesMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.SalesDate >= dtFrom && p.SalesDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.userid.ToLower().Contains(UserList.ToLower()))


                        .OrderByDescending(x => x.SalesId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new SalesResult
                                        {
                                            SalesId = e.SalesId,
                                            SalesNo = e.SalesNo,
                                            CustomerName = e.CustomerName,
                                            SalesPerson = e.SalesPerson,
                                            SalesDate = Convert.ToDateTime(e.SalesDate),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<SalesResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in db.SalesMains
                        .Where(x => x.comid == comid)
                        .Where(p => (p.SalesDate >= dtFrom && p.SalesDate <= dtTo))

                        .OrderByDescending(x => x.SalesId)
                                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new SalesResult
                                        {
                                            SalesId = e.SalesId,
                                            SalesNo = e.SalesNo,
                                            CustomerName = e.CustomerName,
                                            SalesPerson = e.SalesPerson,
                                            SalesDate = Convert.ToDateTime(e.SalesDate),
                                            ReferenceNo = e.ReferenceNo,
                                            NetAmount = e.NetAmount,
                                            PaidAmount = e.PaidAmt,
                                            Due = e.DueAmt
                                        };

                        var parser = new Parser<SalesResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }

                }
                return Json(new { Success = "1" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewResult Circle()
        {

            return View();
        }


        public async Task<IActionResult> GetData(int pageIndex, int pageSize)
        {
            var comid = HttpContext.Session.GetString("comid");

            //System.Threading.Thread.Sleep(2000);
            //var query = (from c in db.Products
            //             orderby c.ProductId ascending
            //             select c).Where(x=>x.comid== comid)
            //             .Skip(pageIndex * pageSize)
            //             .Take(pageSize);

            List<GTERP.Models.Product> scrollproduct = await db.Products.Where(x => x.comid == comid && x.ProductId > 0).OrderBy(x => x.ProductId).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();//

            return Json(scrollproduct);
        }


        public ViewResult PrintView(int id)
        {

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

        public IActionResult Create()
        {

            try
            {

                //HttpContext.Session.SetString("userid", "4864add7-0ab2-4c4f-9eb8-6b63a425e665");
                //HttpContext.Session.SetString("comid", "dapa26-414a-44e4-a287-18e846b51d99");

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");


                object a = HttpContext.Session.GetString("isProductSearch");
                ViewBag.Title = "Entry";

                //ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
                //ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName");
                ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
                //var Productresult = POS.GetProducts(comid);
                //ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                ////ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                //ViewBag.ProductSearch = Productresult;
                //ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
                //ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");

                //ViewBag.SalesTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
                //var ProductSerialresult = POS.GetSerialNoProcedure(comid,userid);
                //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                //ViewBag.ProductSerialSearch = ProductSerialresult;

                //ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
                //ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");




                return View();

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                Console.WriteLine(ex.Message);
                return View();
            }


            return View();


        }



        public class ProductResult
        {


            public int CategoryId { get; set; }
            public int ProductId { get; set; }
            public string ProductImage { get; set; }
            public string CategoryName { get; set; }
            public string ProductName { get; set; }
            public string ProductCode { get; set; }
            public string ProductBarcode { get; set; }
            public string Description { get; set; }

            public decimal CostPrice { get; set; }
            public decimal SalePrice { get; set; }
            public string BlankData { get; set; }
            public string ImagePath { get; set; }






        }

        public IActionResult GetProductListClick(int CategoryId)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Products.Include(y => y.vPrimaryCategory);

                if (CategoryId > 0)
                {
                    var query = from e in db.Products.Where(x => x.ProductId > 0 && x.CategoryId == CategoryId && x.comid == comid).OrderByDescending(x => x.ProductId)
                                    //let FullName = e.ProductName + " " + e.ProductCode
                                let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                //let WarehouseQty = e.InventorySubs != null ? e.InventorySubs.Select(x => new WarehouseResult { WhShortName = x.Warehouses.WhShortName, CurrentStock = x.CurrentStock }).ToList() : null
                                select new ProductResult
                                {
                                    CategoryId = e.CategoryId,
                                    ProductId = e.ProductId,

                                    ProductImage = e.ProductImage != null ? Convert.ToBase64String(e.ProductImage) : null,
                                    //CategoryName = e.vPrimaryCategory.Name,
                                    ProductName = e.ProductName,

                                    //ProductCode = e.ProductCode,
                                    ProductBarcode = e.ProductBarcode,
                                    Description = e.Description,
                                    //CostPrice = e.CostPrice,
                                    SalePrice = e.SalePrice,
                                    BlankData = null,

                                    ImagePath = ImagePath
                                    //ProductImage = e.ProductImage != null ? Convert.ToBase64String(e.ProductImage) : null,//(asset.ProductImage != null) ? (asset.ProductImage) : null
                                    //WarehouseList = WarehouseQty

                                };



                    var parser = new Parser<ProductResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {

                    var query = from e in db.Products.Where(x => x.ProductId > 0 && x.comid == comid).OrderByDescending(x => x.ProductId)
                                    //let FullName = e.ProductName + " " + e.ProductCode
                                let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                //let WarehouseQty = e.InventorySubs != null ? e.InventorySubs.Select(x => new WarehouseResult { WhShortName = x.Warehouses.WhShortName, CurrentStock = x.CurrentStock }).ToList() : null
                                select new ProductResult
                                {
                                    CategoryId = e.CategoryId,
                                    ProductId = e.ProductId,

                                    ProductImage = e.ProductImage != null ? Convert.ToBase64String(e.ProductImage) : null,
                                    //CategoryName = e.vPrimaryCategory.Name,
                                    ProductName = e.ProductName,

                                    //ProductCode = e.ProductCode,
                                    ProductBarcode = e.ProductBarcode,
                                    Description = e.Description,
                                    //CostPrice = e.CostPrice,
                                    SalePrice = e.SalePrice,
                                    BlankData = null,

                                    ImagePath = ImagePath
                                    //ProductImage = e.ProductImage != null ? Convert.ToBase64String(e.ProductImage) : null,//(asset.ProductImage != null) ? (asset.ProductImage) : null
                                    //WarehouseList = WarehouseQty

                                };



                    var parser = new Parser<ProductResult>(Request.Form, query);

                    return Json(parser.Parse());
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class ProductSerialtemp
        {
            public int ProductId { get; set; }

            public int ProductSerialId { get; set; }
            public string ProductSerialNo { get; set; }
        }


        public ActionResult CreateSales()
        {

            object a = HttpContext.Session.GetString("isProductSearch");
            ViewBag.Title = "Create";
            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.Product = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(db.Products, "ProductId", "ProductBarcode");
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");
            ViewBag.ProductSerial = new SelectList(db.ProductSerial, "ProductSerialId", "ProductSerialNo");
            ViewBag.SalesTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");




            return View();


        }

















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


                            ///inventory calculation after remove the data
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null)
                            {
                                inv.SalesQty = inv.SalesQty - ss.Qty;
                                db.Entry(inv).State = EntityState.Modified;
                            }
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

                            ///inventory calculation after add the data
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null)
                            {
                                inv.SalesQty = inv.SalesQty + ss.Qty;
                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                db.Entry(inv).State = EntityState.Modified;
                            }
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


                        ///inventory calculation after Added New data in Save mode
                        foreach (SalesSub ss in salesmain.SalesSubs)
                        {
                            db.SalesSubs.Add(ss);
                            ////if no warehouse found then it will throw error
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                            {
                                inv.SalesQty = inv.SalesQty + ss.Qty;
                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                db.Entry(inv).State = EntityState.Modified;
                            }

                        }

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

            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        //
        // GET: /Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SalesMain salesmain = await

                db.SalesMains
                .Include(x => x.SalesSubs).ThenInclude(x => x.vProductName)
                .Include(x => x.SalesSubs).ThenInclude(x => x.vProductSerial)
                .Include(x => x.SalesTermsSubs)
                .Include(x => x.SalesPaymentSubs).ThenInclude(x => x.vChartofAccounts)
                .Where(x => x.SalesId == id).FirstOrDefaultAsync();

            if (salesmain.isPost == true)
            {
                return NotFound();
            }

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (salesmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();


            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", salesmain.SalesSubs.);
            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            //ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", salesmain.CountryId);
            ViewBag.SalesTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");



            //Call Create View
            return View("Create", salesmain);
        }





        // GET: /Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //SalesMain salesmain = db.SalesMains.Find(id);
            //return View(salesmain);

            HttpContext.Session.SetString("isBarcodeSearch", "true");
            HttpContext.Session.SetString("isProductSearch", "true");
            HttpContext.Session.SetString("isIMEISearch", "true");


            if (id == null)
            {
                return NotFound();
            }

            SalesMain salesmain = await

                db.SalesMains
                .Include(x => x.SalesSubs).ThenInclude(x => x.vProductName)
                .Include(x => x.SalesSubs).ThenInclude(x => x.vProductSerial)
                .Include(x => x.SalesTermsSubs)
                .Include(x => x.SalesPaymentSubs).ThenInclude(x => x.vChartofAccounts)
                .Where(x => x.SalesId == id).FirstOrDefaultAsync();

            if (salesmain.isPost == true)
            {
                return NotFound();
            }

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (salesmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");


            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", salesmain.SalesSubs.);
            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            //ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");
            ViewBag.SalesTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");


            //Call Create View
            return View("Create", salesmain);
        }




        // POST: /Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //SalesSub salessub = db.SalesSubs.Find(id);
                //db.SalesSubs.Remove(salessub);

                SalesMain salesmain = db.SalesMains.Find(id);
                db.SalesMains.Remove(salesmain);
                foreach (var ss in salesmain.SalesSubs)
                {


                    Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                    if (inv != null)
                    {
                        inv.SalesQty = inv.SalesQty - ss.Qty;
                        inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                        db.Entry(inv).State = EntityState.Modified;
                    }
                }

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
            return Json(new SelectList(termssubslists, "Value", "Text"));
        }

        public JsonResult getProduct(int id)
        {
            List<GTERP.Models.Product> product = db.Products.Where(x => x.CategoryId == id).ToList();

            List<SelectListItem> licities = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (GTERP.Models.Product x in product)
                {
                    licities.Add(new SelectListItem { Text = x.ProductName, Value = x.ProductId.ToString() });
                }
            }
            return Json(new SelectList(licities, "Value", "Text"));
        }
        public JsonResult getAccountHead(string id)
        {
            List<Acc_ChartOfAccount> chartofaccounts = db.Acc_ChartOfAccounts.Where(x => x.AccType == "L" && x.AccCode.Contains("103070")).ToList();

            List<SelectListItem> licoa = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (chartofaccounts != null)
            {
                foreach (Acc_ChartOfAccount x in chartofaccounts)
                {
                    licoa.Add(new SelectListItem { Text = x.AccName, Value = x.AccId.ToString() });
                }
            }
            return Json(new SelectList(licoa, "Value", "Text"));
        }

        public JsonResult getBarcode(int id)
        {
            List<GTERP.Models.Product> product = db.Products.Where(x => x.ProductId == id).ToList();

            List<SelectListItem> barcodelist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (product != null)
            {
                foreach (GTERP.Models.Product x in product)
                {
                    barcodelist.Add(new SelectListItem { Text = x.ProductBarcode, Value = x.ProductId.ToString() });
                }
            }
            return Json(new SelectList(barcodelist, "Value", "Text"));
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
            return Json(new SelectList(productseriallist, "Value", "Text"));
        }


        [HttpPost]
        public JsonResult ProductInfo(int id)
        {
            try
            {



                GTERP.Models.Product product = db.Products.Where(y => y.ProductId == id).SingleOrDefault();


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
        public JsonResult CustomerInfo(int id)
        {
            try
            {

                Customer customer = db.Customers.Where(y => y.CustomerId == id).SingleOrDefault();
                return Json(customer);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
        }



        public string GenerateReport(string reportPath, string sqlCmd, string conStringName, string reportFileType, string subReportObject = null)
        {
            var result = "";
            if (subReportObject == null)
            {
                result = $"https://localhost:44383/ReportViewer/GenerateReport?ReportPath= { reportPath }&SqlCmd={ sqlCmd }&DbName={conStringName}&ReportType={ reportFileType}";
                // result = $"https://118.67.215.76/GTREPORT/ReportViewer/GenerateReport?ReportPath= { reportPath }&SqlCmd={ sqlCmd }&DbName={conStringName}&ReportType={ reportFileType}";
                // result = $"https://101.2.165.189/DAPERPREPORT/ReportViewer/GenerateReport?ReportPath= { reportPath }&SqlCmd={ sqlCmd }&DbName={conStringName}&ReportType={ reportFileType}";
                //result = $"http://www.gtrbd.net/DAPERPREPORT/ReportViewer/GenerateReport?ReportPath= { reportPath }&SqlCmd={ sqlCmd }&DbName={conStringName}&ReportType={ reportFileType}";
            }
            else
            {
                //result = $"https://101.2.165.189/DAPERPREPORT/ReportViewer/GenerateReport?ReportPath= { reportPath }&SqlCmd={ sqlCmd }&DbName={conStringName}&ReportType={ reportFileType}&SubReport={subReportObject}";
                result = $"https://localhost:44383/ReportViewer/GenerateReport?ReportPath= { reportPath }&SqlCmd={ sqlCmd }&DbName={conStringName}&ReportType={ reportFileType}&SubReport={subReportObject}";
            }
            return result;

        }

        public class SubReport
        {
            public int Id { get; set; }
            public string strRptPathSub { get; set; } // Sub Report Path name
            public string strRFNSub { get; set; }   // Relational Field Name 
            public string strDSNSub { get; set; }   // DSN Name Sub Report
            public string strQuerySub { get; set; } // Query string Sub Report
        }

        public ActionResult Print(int? id, string type)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            string ConstrName = "ApplicationServices";
            string ReportType = "PDF";
            string ReportPath = "~/ReportViewer/POS/rptInvoice.rdlc";
            string SQLQuery = "Exec [rptInvoice] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.AppPath.ToString() + "'";

            /////////////////////// sub report test to our report server



            var subReport = new SubReport();
            var subReportObject = new List<SubReport>();

            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "";
            subReport.strQuerySub = "Exec rptInvoice_Terms '" + id + "','" + comid + "',''";
            subReport.strRptPathSub = "rptInvoice_Terms";

            subReportObject.Add(subReport);
            var jsonData = JsonConvert.SerializeObject(subReportObject);

            var callBackUrl = GenerateReport(ReportPath, SQLQuery, ConstrName, ReportType, jsonData);

            return Redirect(callBackUrl);

            /////////////////////// sub report test to our report server
        }

    }
}