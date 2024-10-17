using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace GTERP.Controllers
{
    [OverridableAuthorize]
    public class PurchaseReturnController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();
        private TransactionLogRepository tranlog;
        private POSRepository POS;



        public PurchaseReturnController(GTRDBContext context, TransactionLogRepository tran, POSRepository pos)
        {
            tranlog = tran;
            POS = pos;
            db = context;

        }

        //private string comid = int.Parse(httpre HttpContext.Session.GetString("comid").ToString());
        //
        // GET: /PurchaseReturn/
        public ViewResult Index(string FromDate, string ToDate, string UserList, int? SupplierId)
        {
            var comid = HttpContext.Session.GetString("comid");

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

            //List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria , @comid", new SqlParameter("Criteria", "PosPurchaseReturn"), new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
            //ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName", UserList);

            ViewBag.SupplierList = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");

            //return View(db.PurchaseReturnMains.Where(p => (p.PurchaseReturnDate >= dtFrom && p.PurchaseReturnDate <= dtTo) && (p.comid == AppData.AppData.intcomid)).ToList()); //p.ComId == AppData.AppData.intComId && 


            return View();
        }




        //
        // GET: /PurchaseReturn/Details/5

        //[OutputCache(Duration = 100, VaryByParam  = "id")]
        //[OutputCache(CacheProfile ="Admin")]
        public ViewResult Details(int id)
        {

            PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);
            return View(PurchaseReturnmain);
        }


        public ViewResult PrintView(int id)
        {

            PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);
            return View(PurchaseReturnmain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);
            //return View(PurchaseReturnmain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "PurchaseReturn", new { id = id });
        }

        // make pdf


        public ActionResult Create(int? PurchaseId)
        {

            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");



                ViewBag.Title = "Entry";
                ViewBag.PurchaseId = PurchaseId;
                if (PurchaseId == null)
                {
                    PurchaseId = 0;
                }

                var Productresult = POS.GetProducts(comid);
                var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
                object a = HttpContext.Session.GetString("isProductSearch");
                //ViewBag.Title = "Entry";

                if (PurchaseId > 0)
                {
                    PurchaseMain Purchasemainsdata = new PurchaseMain();
                    Purchasemainsdata = db.PurchaseMains.Where(m => m.PurchaseId == PurchaseId).FirstOrDefault();

                    PurchaseReturnMain PurchaseReturnMains = new PurchaseReturnMain();

                    PurchaseReturnMains.PurchaseId = Purchasemainsdata.PurchaseId;
                    PurchaseReturnMains.ReferenceNo = Purchasemainsdata.ReferenceNo;

                    PurchaseReturnMains.PurchaseReturnNo = ""; //Purchasemainsdata.LCRefNo;
                    PurchaseReturnMains.PurchaseReturnDate = DateTime.Now.Date;
                    PurchaseReturnMains.SupplierId = Purchasemainsdata.SupplierId;



                    PurchaseReturnMains.comid = Purchasemainsdata.comid;
                    PurchaseReturnMains.ttlCountQty = Purchasemainsdata.ttlCountQty;
                    PurchaseReturnMains.ttlSumQty = Purchasemainsdata.ttlSumQty;
                    PurchaseReturnMains.ttlUnitPrice = Purchasemainsdata.ttlUnitPrice;
                    PurchaseReturnMains.ttlIndDisAmt = Purchasemainsdata.ttlIndDisAmt;
                    PurchaseReturnMains.ttlIndPrice = Purchasemainsdata.ttlIndPrice;
                    PurchaseReturnMains.ttlSumAmt = Purchasemainsdata.ttlSumAmt;
                    PurchaseReturnMains.DisPer = Purchasemainsdata.DisPer;
                    PurchaseReturnMains.DisAmt = Purchasemainsdata.DisAmt;
                    PurchaseReturnMains.ServiceCharge = Purchasemainsdata.ServiceCharge;

                    PurchaseReturnMains.NetAmount = Purchasemainsdata.NetAmount;
                    PurchaseReturnMains.PaidAmt = Purchasemainsdata.PaidAmt;
                    PurchaseReturnMains.DueAmt = Purchasemainsdata.DueAmt;


                    PurchaseReturnMains.CurrencyRate = Purchasemainsdata.CurrencyRate;
                    PurchaseReturnMains.NetAmountBDT = Purchasemainsdata.NetAmountBDT;
                    PurchaseReturnMains.Remarks = Purchasemainsdata.Remarks;

                    PurchaseReturnMains.CountryId = Purchasemainsdata.CountryId;
                    PurchaseReturnMains.PrimaryAddress = Purchasemainsdata.PrimaryAddress;
                    PurchaseReturnMains.SecoundaryAddress = Purchasemainsdata.SecoundaryAddress;
                    PurchaseReturnMains.PhoneNo = Purchasemainsdata.PhoneNo;

                    PurchaseReturnMains.EmailId = Purchasemainsdata.EmailId;
                    PurchaseReturnMains.City = Purchasemainsdata.City;
                    PurchaseReturnMains.PostalCode = Purchasemainsdata.PostalCode;
                    PurchaseReturnMains.ttlIndVat = Purchasemainsdata.ttlIndVat;

                    PurchaseReturnMains.TotalVat = Purchasemainsdata.TotalVat;

                    PurchaseReturnMains.SupplierName = Purchasemainsdata.SupplierName;
                    PurchaseReturnMains.Shipping = Purchasemainsdata.Shipping;
                    PurchaseReturnMains.ChkPer = Purchasemainsdata.ChkPer;





                    List<PurchaseReturnSub> PurchaseReturnlist = new List<PurchaseReturnSub>();
                    foreach (var item in Purchasemainsdata.PurchaseSubs)
                    {
                        PurchaseReturnSub PurchaseReturnSubs = new PurchaseReturnSub();

                        PurchaseReturnSubs.SalesTypeId = item.SalesTypeId;
                        // PurchaseReturnSubs.vSalesTypes = db.SalesType.Where(x => x.SalesTypeId== item.SalesTypeId).FirstOrDefault();

                        PurchaseReturnSubs.ProductId = item.ProductId;
                        PurchaseReturnSubs.vProductName = db.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

                        PurchaseReturnSubs.ProductDescription = item.ProductDescription;
                        PurchaseReturnSubs.ProductSerialId = item.ProductSerialId;
                        PurchaseReturnSubs.vProductSerial = db.ProductSerial.Where(x => x.ProductSerialId == item.ProductSerialId).FirstOrDefault();



                        PurchaseReturnSubs.WarehouseId = item.WarehouseId;
                        PurchaseReturnSubs.vWarehouse = db.Warehouses.Where(x => x.WarehouseId == item.WarehouseId).FirstOrDefault();

                        PurchaseReturnSubs.Size = item.Size;
                        PurchaseReturnSubs.Carton = item.Carton;
                        PurchaseReturnSubs.PCTN = item.PCTN;
                        PurchaseReturnSubs.Qty = item.Qty;
                        PurchaseReturnSubs.UnitPrice = item.UnitPrice;
                        PurchaseReturnSubs.IndVatPer = item.IndVatPer;
                        PurchaseReturnSubs.IndVat = item.IndVat;
                        PurchaseReturnSubs.IndDisPer = item.IndDisPer;
                        PurchaseReturnSubs.IndDisAmt = item.IndDisAmt;
                        PurchaseReturnSubs.IndPrice = item.IndPrice;
                        PurchaseReturnSubs.IndChkPer = item.IndChkPer;
                        PurchaseReturnSubs.Amount = item.Amount;
                        PurchaseReturnSubs.RowNo = item.RowNo;
                        PurchaseReturnSubs.isChecked = false;

                        PurchaseReturnlist.Add(PurchaseReturnSubs);
                    }
                    PurchaseReturnMains.PurchaseReturnSubs = PurchaseReturnlist;





                    //List<PurchaseReturnTermsSub> PurchaseReturnTermlist = new List<PurchaseReturnTermsSub>();
                    //foreach (var item in Purchasemainsdata.PurchaseTermsSubs)
                    //{
                    //    PurchaseReturnTermsSub PurchaseReturntermsSubs = new PurchaseReturnTermsSub();

                    //    PurchaseReturntermsSubs.Terms = item.Terms;
                    //    PurchaseReturntermsSubs.RowNo = item.RowNo;
                    //    PurchaseReturntermsSubs.Description = item.Description;

                    //    PurchaseReturnTermlist.Add(PurchaseReturntermsSubs);
                    //}
                    //PurchaseReturnMains.PurchaseReturnTermsSubs = PurchaseReturnTermlist;


                    //List<PurchaseReturnPaymentSub> PurchaseReturnPaymentlist = new List<PurchaseReturnPaymentSub>();
                    //foreach (var item in Purchasemainsdata.PurchasePaymentSubs)
                    //{
                    //    PurchaseReturnPaymentSub PurchaseReturnPaymentSubs = new PurchaseReturnPaymentSub();

                    //    PurchaseReturnPaymentSubs.AccId = item.AccId;
                    //    PurchaseReturnPaymentSubs.vChartofAccounts = db.Acc_ChartOfAccounts.Where(x => x.AccId == item.AccId).FirstOrDefault();

                    //    PurchaseReturnPaymentSubs.Amount = item.Amount;
                    //    PurchaseReturnPaymentSubs.PaymentCardNo = item.PaymentCardNo;
                    //    PurchaseReturnPaymentSubs.PaymentTypeId = item.PaymentTypeId;
                    //    PurchaseReturnPaymentSubs.vPaymentType = db.PaymentTypes.Where(x => x.PaymentTypeId == item.PaymentTypeId).FirstOrDefault();



                    //    PurchaseReturnPaymentSubs.RowNo = item.RowNo;


                    //    PurchaseReturnPaymentlist.Add(PurchaseReturnPaymentSubs);
                    //}
                    //PurchaseReturnMains.PurchaseReturnPaymentSubs = PurchaseReturnPaymentlist;


                    ///for gettting the last inputed data related buyer



                    ViewBag.PurchaseId = new SelectList(POS.GetPurchaseList(comid), "PurchaseId", "PurchaseNo", PurchaseReturnMains.PurchaseId);
                    ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
                    ViewBag.Supplier = new SelectList(POS.GetSupplier(comid), "SupplierId", "SupplierName", PurchaseReturnMains.SupplierId);
                    ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");

                    ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                    ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                    ViewBag.ProductSearch = Productresult;
                    ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
                    ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");

                    ViewBag.PurchaseReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");

                    ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                    ViewBag.ProductSerialSearch = ProductSerialresult;

                    ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
                    ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");




                    return View(PurchaseReturnMains);
                }


                ViewBag.PurchaseId = new SelectList(POS.GetPurchaseList(comid), "PurchaseId", "PurchaseNo");

                ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
                ViewBag.Supplier = new SelectList(POS.GetSupplier(comid), "SupplierId", "SupplierName");
                ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
                //var Productresult = POS.GetProducts(comid);
                ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                ViewBag.ProductSearch = Productresult;
                ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
                ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");

                ViewBag.PurchaseReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
                //var ProductSerialresult = POS.GetSerialNoProcedure(comid,userid);
                ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                ViewBag.ProductSerialSearch = ProductSerialresult;

                ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
                ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");




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

        public ActionResult CreatePurchaseReturn()
        {

            object a = HttpContext.Session.GetString("isProductSearch");
            ViewBag.Title = "Create";
            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierId", "SupplierName");
            ViewBag.Category = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.Product = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(db.Products, "ProductId", "ProductBarcode");
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");
            ViewBag.ProductSerial = new SelectList(db.ProductSerial, "ProductSerialId", "ProductSerialNo");
            ViewBag.PurchaseReturnTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");




            return View();


        }


        // POST: /PurchaseReturn/Create
        /// <summary>
        /// This method is used for Creating and Updating  PurchaseReturn Information 
        /// (PurchaseReturn Contains: 1.PurchaseReturnMain and *PurchaseReturnSub )
        /// </summary>
        /// <param name="PurchaseReturnmain">
        /// </param>
        /// <returns>
        /// Returns Json data Containing Success Status, New PurchaseReturn ID and Exeception 
        /// </returns>
        [HttpPost]
        public JsonResult Create(PurchaseReturnMain PurchaseReturnmain)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    // If PurchaseReturn main has PurchaseReturnID then we can understand we have existing PurchaseReturn Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (PurchaseReturnmain.PurchaseReturnId > 0)
                    {

                        IQueryable<PurchaseReturnSub> CurrentPurchaseReturnSUb = db.PurchaseReturnSubs.Where(p => p.PurchaseReturnId == PurchaseReturnmain.PurchaseReturnId);
                        IQueryable<PurchaseReturnTermsSub> CurrentPurchaseReturnTermsSUb = db.PurchaseReturnTermsSubs.Where(p => p.PurchaseReturnId == PurchaseReturnmain.PurchaseReturnId);
                        IQueryable<PurchaseReturnPaymentSub> CurrentPurchaseReturnPaymentSUb = db.PurchaseReturnPaymentSubs.Where(p => p.PurchaseReturnId == PurchaseReturnmain.PurchaseReturnId);


                        foreach (PurchaseReturnSub ss in CurrentPurchaseReturnSUb)
                        {
                            db.PurchaseReturnSubs.Remove(ss);


                            ///inventory calculation after remove the data
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            inv.PurRetQty = inv.PurRetQty - ss.Qty;
                            db.Entry(inv).State = EntityState.Modified;
                        }

                        foreach (PurchaseReturnTermsSub sss in CurrentPurchaseReturnTermsSUb)
                        {
                            db.PurchaseReturnTermsSubs.Remove(sss);
                        }

                        foreach (PurchaseReturnPaymentSub ssss in CurrentPurchaseReturnPaymentSUb)
                        {
                            db.PurchaseReturnPaymentSubs.Remove(ssss);
                        }

                        foreach (PurchaseReturnSub ss in PurchaseReturnmain.PurchaseReturnSubs)
                        {
                            db.PurchaseReturnSubs.Add(ss);

                            ///inventory calculation after add the data
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            inv.PurRetQty = inv.PurRetQty + ss.Qty;
                            //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);//fahad
                            inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                            db.Entry(inv).State = EntityState.Modified;
                        }
                        ///terms subs
                        if (PurchaseReturnmain.PurchaseReturnTermsSubs == null)
                        { }
                        else
                        {

                            foreach (PurchaseReturnTermsSub sss in PurchaseReturnmain.PurchaseReturnTermsSubs)
                            {
                                db.PurchaseReturnTermsSubs.Add(sss);
                            }
                        }
                        ///payments
                        if (PurchaseReturnmain.PurchaseReturnPaymentSubs == null)
                        { }
                        else
                        {

                            foreach (PurchaseReturnPaymentSub ssss in PurchaseReturnmain.PurchaseReturnPaymentSubs)
                            {
                                db.PurchaseReturnPaymentSubs.Add(ssss);
                            }
                        }


                        db.Entry(PurchaseReturnmain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.PurchaseReturnMains.Add(PurchaseReturnmain);


                        ///inventory calculation after Added New data in Save mode
                        foreach (PurchaseReturnSub ss in PurchaseReturnmain.PurchaseReturnSubs)
                        {
                            db.PurchaseReturnSubs.Add(ss);

                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                            {

                                inv.PurRetQty = inv.PurRetQty + ss.Qty;
                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty); // fahad
                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                db.Entry(inv).State = EntityState.Modified;
                            }

                        }

                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, PurchaseReturnID = PurchaseReturnmain.PurchaseReturnId, ex = "" });
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
        // GET: /PurchaseReturn/Edit/5
        public ActionResult Edit(int? id)
        {
            //string cultureinfo = "bd-BD";
            ////string cultureinfo = "th-TH";

            //CultureInfo culture = new CultureInfo(cultureinfo, false);
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            //HttpContext.Session.SetString("isBarcodeSearch", "true");
            //HttpContext.Session.SetString("isProductSearch", "true");
            //HttpContext.Session.SetString("isIMEISearch", "true");


            if (id == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");


            PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);

            if (PurchaseReturnmain.isPost == true)
            {
                return NotFound();
            }

            //PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);



            if (PurchaseReturnmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";



            ///this.ViewData["Supplier"] = new SelectList(db.Suppliers.ToList(), "SupplierId", "SupplierName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", PurchaseReturnmain.PurchaseReturnSubs.);
            ViewBag.PurchaseId = new SelectList(POS.GetPurchaseList(comid), "PurchaseId", "PurchaseNo", PurchaseReturnmain.PurchaseId);

            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Supplier = new SelectList(POS.GetSupplier(comid), "SupplierId", "SupplierName");
            ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", PurchaseReturnmain.CountryId);
            ViewBag.PurchaseReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");



            //Call Create View
            return View("Create", PurchaseReturnmain);
        }





        // GET: /PurchaseReturn/Delete/5
        public ActionResult Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);
            //return View(PurchaseReturnmain);

            HttpContext.Session.SetString("isBarcodeSearch", "true");
            HttpContext.Session.SetString("isProductSearch", "true");
            HttpContext.Session.SetString("isIMEISearch", "true");


            if (id == null)
            {
                return NotFound();
            }


            PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);

            if (PurchaseReturnmain.isPost == true)
            {
                return NotFound();
            }

            //PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);



            if (PurchaseReturnmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";


            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();

            ///this.ViewData["Supplier"] = new SelectList(db.Suppliers.ToList(), "SupplierId", "SupplierName");

            ViewBag.PurchaseId = new SelectList(POS.GetPurchaseList(comid), "PurchaseId", "PurchaseNo", PurchaseReturnmain.PurchaseId);
            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", PurchaseReturnmain.PurchaseReturnSubs.);
            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Supplier = new SelectList(POS.GetSupplier(comid), "SupplierId", "SupplierName");
            ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");
            ViewBag.PurchaseReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");


            //Call Create View
            return View("Create", PurchaseReturnmain);
        }




        // POST: /PurchaseReturn/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //PurchaseReturnSub PurchaseReturnsub = db.PurchaseReturnSubs.Find(id);
                //db.PurchaseReturnSubs.Remove(PurchaseReturnsub);

                PurchaseReturnMain PurchaseReturnmain = db.PurchaseReturnMains.Find(id);

                foreach (var ss in PurchaseReturnmain.PurchaseReturnSubs)
                {


                    Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                    inv.PurRetQty = inv.PurRetQty - ss.Qty;
                    //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty); //fahad
                    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                    db.Entry(inv).State = EntityState.Modified;
                }

                db.PurchaseReturnMains.Remove(PurchaseReturnmain);

                db.SaveChanges();
                return Json(new { Success = 1, PurchaseReturnID = PurchaseReturnmain.PurchaseReturnId, ex = "" });

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



                //return Json(new SelectList(licitiesa, "Value", "Text" ));

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




                Supplier Supplier = db.Suppliers.Where(y => y.SupplierId == id).SingleOrDefault();
                return Json(Supplier);

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