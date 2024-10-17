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
    public class SalesReturnController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();
        private TransactionLogRepository tranlog;
        private POSRepository POS;


        public SalesReturnController(GTRDBContext context, TransactionLogRepository tran, POSRepository pos)
        {
            tranlog = tran;
            POS = pos;
            db = context;

        }

        //
        // GET: /SalesReturn/
        public ViewResult Index(string FromDate, string ToDate, string UserList, int? CustomerId)
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

            //List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria , @comid", new SqlParameter("Criteria", "PosSalesReturn"), new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
            //ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName", UserList);

            ViewBag.CustomerList = new SelectList(db.Customers.Where(x => x.comid == comid), "CustomerId", "CustomerName");


            return View();
        }




        public ViewResult Details(int id)
        {

            SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);
            return View(SalesReturnmain);
        }


        public ViewResult PrintView(int id)
        {

            SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);
            return View(SalesReturnmain);
        }


        // post for export pdf

        // [HttpGet, ActionName("Index")]
        public ActionResult asdf(int? id)
        {
            //SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);
            //return View(SalesReturnmain);
            // go to export pdf action
            // ViewBag.Students = studentManager.GetAllStudentsForDropDown();
            return RedirectToAction("ExportPdf", "SalesReturn", new { id = id });
        }

        // make pdf



        public ActionResult Create(int? SalesId)
        {

            try
            {



                ViewBag.Title = "Entry";
                ViewBag.SalesId = SalesId;
                if (SalesId == null)
                {
                    SalesId = 0;
                }



                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");





                //ViewBag.ExportDetails = db.SalesMains.Where(m => m.SalesId == 0).ToList();
                var Productresult = POS.GetProducts(comid);
                var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
                object a = HttpContext.Session.GetString("isProductSearch");
                //ViewBag.Title = "Entry";

                if (SalesId > 0)
                {
                    SalesMain salesmainsdata = new SalesMain();
                    salesmainsdata = db.SalesMains.Where(m => m.SalesId == SalesId).FirstOrDefault();

                    SalesReturnMain SalesReturnMains = new SalesReturnMain();

                    SalesReturnMains.SalesId = salesmainsdata.SalesId;
                    SalesReturnMains.ReferenceNo = salesmainsdata.ReferenceNo;

                    SalesReturnMains.SalesReturnNo = ""; //salesmainsdata.LCRefNo;
                    SalesReturnMains.SalesReturnDate = DateTime.Now.Date;
                    SalesReturnMains.CustomerId = salesmainsdata.CustomerId;



                    SalesReturnMains.comid = salesmainsdata.comid;
                    SalesReturnMains.ttlCountQty = salesmainsdata.ttlCountQty;
                    SalesReturnMains.ttlSumQty = salesmainsdata.ttlSumQty;
                    SalesReturnMains.ttlUnitPrice = salesmainsdata.ttlUnitPrice;
                    SalesReturnMains.ttlIndDisAmt = salesmainsdata.ttlIndDisAmt;
                    SalesReturnMains.ttlIndPrice = salesmainsdata.ttlIndPrice;
                    SalesReturnMains.ttlSumAmt = salesmainsdata.ttlSumAmt;
                    SalesReturnMains.DisPer = salesmainsdata.DisPer;
                    SalesReturnMains.DisAmt = salesmainsdata.DisAmt;
                    SalesReturnMains.ServiceCharge = salesmainsdata.ServiceCharge;

                    SalesReturnMains.NetAmount = salesmainsdata.NetAmount;
                    SalesReturnMains.PaidAmt = salesmainsdata.PaidAmt;
                    SalesReturnMains.DueAmt = salesmainsdata.DueAmt;


                    SalesReturnMains.CurrencyRate = salesmainsdata.CurrencyRate;
                    SalesReturnMains.NetAmountBDT = salesmainsdata.NetAmountBDT;
                    SalesReturnMains.Remarks = salesmainsdata.Remarks;

                    SalesReturnMains.CountryId = salesmainsdata.CountryId;
                    SalesReturnMains.PrimaryAddress = salesmainsdata.PrimaryAddress;
                    SalesReturnMains.SecoundaryAddress = salesmainsdata.SecoundaryAddress;
                    SalesReturnMains.PhoneNo = salesmainsdata.PhoneNo;

                    SalesReturnMains.EmailId = salesmainsdata.EmailId;
                    SalesReturnMains.City = salesmainsdata.City;
                    SalesReturnMains.PostalCode = salesmainsdata.PostalCode;
                    SalesReturnMains.ttlIndVat = salesmainsdata.ttlIndVat;

                    SalesReturnMains.TotalVat = salesmainsdata.TotalVat;

                    SalesReturnMains.CustomerName = salesmainsdata.CustomerName;
                    SalesReturnMains.Shipping = salesmainsdata.Shipping;
                    SalesReturnMains.ChkPer = salesmainsdata.ChkPer;





                    List<SalesReturnSub> SalesReturnlist = new List<SalesReturnSub>();
                    foreach (var item in salesmainsdata.SalesSubs)
                    {
                        SalesReturnSub SalesReturnSubs = new SalesReturnSub();

                        SalesReturnSubs.SalesTypeId = item.SalesTypeId;
                        SalesReturnSubs.vSalesTypes = db.SalesType.Where(x => x.SalesTypeId == item.SalesTypeId).FirstOrDefault();

                        SalesReturnSubs.ProductId = item.ProductId;
                        SalesReturnSubs.vProductName = db.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

                        SalesReturnSubs.ProductDescription = item.ProductDescription;
                        SalesReturnSubs.ProductSerialId = item.ProductSerialId;
                        SalesReturnSubs.vProductSerial = db.ProductSerial.Where(x => x.ProductSerialId == item.ProductSerialId).FirstOrDefault();



                        SalesReturnSubs.WarehouseId = item.WarehouseId;
                        SalesReturnSubs.vWarehouse = db.Warehouses.Where(x => x.WarehouseId == item.WarehouseId).FirstOrDefault();

                        SalesReturnSubs.Size = item.Size;
                        SalesReturnSubs.Carton = item.Carton;
                        SalesReturnSubs.PCTN = item.PCTN;
                        SalesReturnSubs.Qty = item.Qty;
                        SalesReturnSubs.UnitPrice = item.UnitPrice;
                        SalesReturnSubs.IndVatPer = item.IndVatPer;
                        SalesReturnSubs.IndVat = item.IndVat;
                        SalesReturnSubs.IndDisPer = item.IndDisPer;
                        SalesReturnSubs.IndDisAmt = item.IndDisAmt;
                        SalesReturnSubs.IndPrice = item.IndPrice;
                        SalesReturnSubs.IndChkPer = item.IndChkPer;
                        SalesReturnSubs.Amount = item.Amount;
                        SalesReturnSubs.RowNo = item.RowNo;
                        SalesReturnSubs.isChecked = false;

                        SalesReturnlist.Add(SalesReturnSubs);
                    }
                    SalesReturnMains.SalesReturnSubs = SalesReturnlist;





                    List<SalesReturnTermsSub> SalesReturnTermlist = new List<SalesReturnTermsSub>();
                    foreach (var item in salesmainsdata.SalesTermsSubs)
                    {
                        SalesReturnTermsSub SalesReturntermsSubs = new SalesReturnTermsSub();

                        SalesReturntermsSubs.Terms = item.Terms;
                        SalesReturntermsSubs.RowNo = item.RowNo;
                        SalesReturntermsSubs.Description = item.Description;

                        SalesReturnTermlist.Add(SalesReturntermsSubs);
                    }
                    SalesReturnMains.SalesReturnTermsSubs = SalesReturnTermlist;


                    List<SalesReturnPaymentSub> SalesReturnPaymentlist = new List<SalesReturnPaymentSub>();
                    foreach (var item in salesmainsdata.SalesPaymentSubs)
                    {
                        SalesReturnPaymentSub SalesReturnPaymentSubs = new SalesReturnPaymentSub();

                        SalesReturnPaymentSubs.AccId = item.AccId;
                        SalesReturnPaymentSubs.vChartofAccounts = db.Acc_ChartOfAccounts.Where(x => x.AccId == item.AccId).FirstOrDefault();

                        SalesReturnPaymentSubs.Amount = item.Amount;
                        SalesReturnPaymentSubs.PaymentCardNo = item.PaymentCardNo;
                        SalesReturnPaymentSubs.PaymentTypeId = item.PaymentTypeId;
                        SalesReturnPaymentSubs.vPaymentType = db.PaymentTypes.Where(x => x.PaymentTypeId == item.PaymentTypeId).FirstOrDefault();



                        SalesReturnPaymentSubs.RowNo = item.RowNo;


                        SalesReturnPaymentlist.Add(SalesReturnPaymentSubs);
                    }
                    SalesReturnMains.SalesReturnPaymentSubs = SalesReturnPaymentlist;


                    ///for gettting the last inputed data related buyer



                    ViewBag.SalesId = new SelectList(POS.GetSalesList(comid), "SalesId", "SalesNo", SalesReturnMains.SalesId);
                    ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
                    ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName", SalesReturnMains.CustomerId);
                    ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");

                    ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                    ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                    ViewBag.ProductSearch = Productresult;
                    ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
                    ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");

                    ViewBag.SalesReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");

                    ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                    ViewBag.ProductSerialSearch = ProductSerialresult;

                    ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
                    ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");




                    return View(SalesReturnMains);
                }


                ViewBag.SalesId = new SelectList(POS.GetSalesList(comid), "SalesId", "SalesNo");

                ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
                ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName");
                ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
                //var Productresult = POS.GetProducts(comid);
                ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
                ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
                ViewBag.ProductSearch = Productresult;
                ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
                ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");

                ViewBag.SalesReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
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


            return View();


        }
        public class ProductSerialtemp
        {
            public int ProductId { get; set; }

            public int ProductSerialId { get; set; }
            public string ProductSerialNo { get; set; }
        }



        public ActionResult CreateSalesReturn()
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
            ViewBag.SalesReturnTerms = new SelectList(db.TermsMain, "TermsId", "TermsName");




            return View();


        }



        [HttpPost]
        public JsonResult Create(SalesReturnMain SalesReturnmain)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    // If SalesReturn main has SalesReturnID then we can understand we have existing SalesReturn Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (SalesReturnmain.SalesReturnId > 0)
                    {

                        IQueryable<SalesReturnSub> CurrentSalesReturnSUb = db.SalesReturnSubs.Where(p => p.SalesReturnId == SalesReturnmain.SalesReturnId);
                        IQueryable<SalesReturnTermsSub> CurrentSalesReturnTermsSUb = db.SalesReturnTermsSubs.Where(p => p.SalesReturnId == SalesReturnmain.SalesReturnId);
                        IQueryable<SalesReturnPaymentSub> CurrentSalesReturnPaymentSUb = db.SalesReturnPaymentSubs.Where(p => p.SalesReturnId == SalesReturnmain.SalesReturnId);


                        foreach (SalesReturnSub ss in CurrentSalesReturnSUb)
                        {
                            db.SalesReturnSubs.Remove(ss);


                            ///inventory calculation after remove the data
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            inv.SalesRetQty = inv.SalesRetQty - ss.Qty;
                            db.Entry(inv).State = EntityState.Modified;
                        }

                        foreach (SalesReturnTermsSub sss in CurrentSalesReturnTermsSUb)
                        {
                            db.SalesReturnTermsSubs.Remove(sss);
                        }

                        foreach (SalesReturnPaymentSub ssss in CurrentSalesReturnPaymentSUb)
                        {
                            db.SalesReturnPaymentSubs.Remove(ssss);
                        }

                        foreach (SalesReturnSub ss in SalesReturnmain.SalesReturnSubs)
                        {
                            db.SalesReturnSubs.Add(ss);

                            ///inventory calculation after add the data
                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            inv.SalesRetQty = inv.SalesRetQty + ss.Qty;
                            //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                            inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                            db.Entry(inv).State = EntityState.Modified;
                        }
                        ///terms subs
                        if (SalesReturnmain.SalesReturnTermsSubs == null)
                        { }
                        else
                        {

                            foreach (SalesReturnTermsSub sss in SalesReturnmain.SalesReturnTermsSubs)
                            {
                                db.SalesReturnTermsSubs.Add(sss);
                            }
                        }
                        ///payments
                        if (SalesReturnmain.SalesReturnPaymentSubs == null)
                        { }
                        else
                        {

                            foreach (SalesReturnPaymentSub ssss in SalesReturnmain.SalesReturnPaymentSubs)
                            {
                                db.SalesReturnPaymentSubs.Add(ssss);
                            }
                        }


                        db.Entry(SalesReturnmain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.SalesReturnMains.Add(SalesReturnmain);


                        ///inventory calculation after Added New data in Save mode
                        foreach (SalesReturnSub ss in SalesReturnmain.SalesReturnSubs)
                        {
                            db.SalesReturnSubs.Add(ss);

                            Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();
                            if (inv != null) ///added by fahad for solving error if no ware house found then no update of the data
                            {

                                inv.SalesRetQty = inv.SalesRetQty + ss.Qty;
                                //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                                inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                                db.Entry(inv).State = EntityState.Modified;
                            }

                        }

                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, SalesReturnID = SalesReturnmain.SalesReturnId, ex = "" });
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
        // GET: /SalesReturn/Edit/5
        public ActionResult Edit(int? id)
        {



            if (id == null)
            {
                return NotFound();
            }


            SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);

            if (SalesReturnmain.isPost == true)
            {
                return NotFound();
            }

            //SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);



            if (SalesReturnmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();

            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", SalesReturnmain.SalesReturnSubs.);
            ViewBag.SalesId = new SelectList(POS.GetSalesList(comid), "SalesId", "SalesNo", SalesReturnmain.SalesId);

            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", SalesReturnmain.CountryId);
            ViewBag.SalesReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");



            //Call Create View
            return View("Create", SalesReturnmain);
        }





        // GET: /SalesReturn/Delete/5
        public ActionResult Delete(int? id)
        {
            //ViewBag.Title = "Delete";

            //SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);
            //return View(SalesReturnmain);

            HttpContext.Session.SetString("isBarcodeSearch", "true");
            HttpContext.Session.SetString("isProductSearch", "true");
            HttpContext.Session.SetString("isIMEISearch", "true");


            if (id == null)
            {
                return NotFound();
            }


            SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);

            if (SalesReturnmain.isPost == true)
            {
                return NotFound();
            }

            //SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);



            if (SalesReturnmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";

            var comid = HttpContext.Session.GetString("comid").ToString();
            var userid = HttpContext.Session.GetString("userid").ToString();

            ///this.ViewData["Customer"] = new SelectList(db.Customers.ToList(), "CustomerId", "CustomerName");

            ViewBag.SalesId = new SelectList(POS.GetSalesList(comid), "SalesId", "SalesNo", SalesReturnmain.SalesId);
            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", SalesReturnmain.SalesReturnSubs.);
            ViewBag.SalesType = new SelectList(POS.GetSalesType(), "SalesTypeId", "TypeShortName");
            ViewBag.Customer = new SelectList(POS.GetCustomer(comid), "CustomerId", "CustomerName");
            ViewBag.Category = new SelectList(POS.GetCategory(comid), "CategoryId", "Name");
            var Productresult = POS.GetProducts(comid);
            ViewBag.Product = new SelectList(Productresult, "ProductId", "ProductName");
            ViewBag.Barcode = new SelectList(Productresult, "ProductId", "ProductBarcode");
            ViewBag.ProductSearch = Productresult;
            ViewBag.Warehouse = new SelectList(POS.GetWarehouse(comid), "WarehouseId", "WhName");
            ViewBag.Country = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName");
            ViewBag.SalesReturnTerms = new SelectList(POS.GetTerms(comid), "TermsId", "TermsName");
            var ProductSerialresult = POS.GetSerialNoProcedure(comid, userid);
            ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ProductSerialSearch = ProductSerialresult;

            ViewBag.PaymentTypes = new SelectList(POS.GetPaymentTypes(), "PaymentTypeId", "TypeShortName");
            ViewBag.AccountHead = new SelectList(POS.GetChartOfAccountCash(comid), "AccId", "AccName");


            //Call Create View
            return View("Create", SalesReturnmain);
        }




        // POST: /SalesReturn/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                //SalesReturnSub SalesReturnsub = db.SalesReturnSubs.Find(id);
                //db.SalesReturnSubs.Remove(SalesReturnsub);

                SalesReturnMain SalesReturnmain = db.SalesReturnMains.Find(id);

                foreach (var ss in SalesReturnmain.SalesReturnSubs)
                {


                    Inventory inv = db.Inventory.Where(x => x.ProductId == ss.ProductId && x.WareHouseId == ss.WarehouseId).FirstOrDefault();

                    inv.SalesRetQty = inv.SalesRetQty - ss.Qty;
                    //inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty);
                    inv.CurrentStock = (inv.OpStock + inv.PurQty + inv.PurExcQty + inv.SalesRetQty + inv.GoodsReceiveQty + inv.IssueRtnQty) - (inv.SalesQty + inv.SalesExcQty + inv.PurRetQty + inv.IssueQty + inv.GoodsRcvRtnQty);

                    db.Entry(inv).State = EntityState.Modified;
                }

                db.SalesReturnMains.Remove(SalesReturnmain);

                db.SaveChanges();
                return Json(new { Success = 1, SalesReturnID = SalesReturnmain.SalesReturnId, ex = "" });

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





    }
}