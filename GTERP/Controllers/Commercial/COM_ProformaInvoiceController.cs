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
    public class COM_ProformaInvoiceController : Controller
    {
        private GTRDBContext db;

        public COM_ProformaInvoiceController(GTRDBContext context)
        {
            db = context;
        }

        // GET: COM_ProformaInvoice
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Index(int? supplierid, string UserList, string FromDate, string ToDate)
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
            //List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria", new SqlParameter("Criteria", "PIUser"))).ToList();
            List<AspnetUserList> AspNetUserList = new List<AspnetUserList>();
            ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName");


            if (UserList == null)
            {
                UserList = HttpContext.Session.GetString("userid");
            }

            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            if (supplierid == null)
            {
                supplierid = 0;
            }
            // return View(db.ExportOrders.Where(p => (p.ShipDate >= dtFrom && p.ShipDate <= dtTo) && (p.StyleID.ToString() == supplierid.ToString()) && p.isDelete == false).ToList()); //p.ComId == HttpContext.Session.GetString("comid") && 


            var x = db.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();

            //var x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.SupplierId.ToString() == supplierid.ToString()) && (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();

            if (supplierid == 0)
            {
                if (UserList == "")
                {
                    x = db.COM_ProformaInvoices.Where(p => (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();
                    //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


                }
                else
                {
                    x = db.COM_ProformaInvoices.Where(p => (p.PIDate >= dtFrom && p.PIDate <= dtTo) && (p.UserId.ToString() == UserList.ToString())).ToList();

                }

                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


            }
            else
            {
                if (UserList == "")
                {
                    x = db.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();
                    //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.SupplierId.ToString() == supplierid.ToString()) && (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();
                }
                else
                {
                    x = db.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo) && (p.UserId.ToString() == UserList.ToString())).ToList();

                }

            }


            //List<COM_ProformaInvoice> proformainvoicelist = new List<COM_ProformaInvoice>();


            //foreach (var item in x)
            //{
            //    COM_ProformaInvoice proforma = new COM_ProformaInvoice();

            //    proforma.ExpenseHeadID = item.ExpenseHeadID;
            //    proforma.Amount = 0;
            //    proforma.IsCheck = false;
            //    proforma.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };

            //    proformainvoicelist.Add(proforma);

            //}

            return View(x); //p.ComId == HttpContext.Session.GetString("comid") && 
        }
        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult PIDailyReceiving(int? supplierid, string FromDate, string ToDate)
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
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            if (supplierid == null)
            {
                supplierid = 0;
            }


            //List<PIDailyReceivingModel> ProductSerialresult = (db.Database.SqlQuery<PIDailyReceivingModel>("[rptPIDailyReceiving]  @comid, @userid,@dtFrom,@dtTo,@SupplierId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]) , new SqlParameter("dtFrom", dtFrom) , new SqlParameter("dtTo", dtTo), new SqlParameter("SupplierId", supplierid))).ToList();

            List<PIDailyReceivingModel> ProductSerialresult = new List<PIDailyReceivingModel>();
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.PIDailyReceiving = ProductSerialresult;


            return View(); //p.ComId == HttpContext.Session.GetString("comid") && 



        }

        public class PIDailyReceivingModel
        {
            public string FileNo { get; set; }

            public string Factory { get; set; }

            public string SupplierName { get; set; }

            public string PINo { get; set; }

            public string OrderNo { get; set; }
            public decimal ImportQty { get; set; }
            public string UnitName { get; set; } ///TotalValue	ReceivingDate	PIStatus
            public decimal TotalValue { get; set; }
            public string ReceivingDate { get; set; }
            public string PIStatus { get; set; }
            public string BBLCNo { get; set; }
            public string StatusColor { get; set; }
            public string LcOpeningDate { get; set; }



        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Download(string file)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "Download\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }

        // GET: COM_ProformaInvoice/Details/5


        // GET: COM_ProformaInvoice/Create
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Create(int? supplierid, int? Flag)
        {
            try
            {

                ViewBag.SupplierId = supplierid;
                if (supplierid == null)
                {
                    supplierid = 0;
                }

                ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");
                ViewBag.CurrencyId = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
                ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");
                ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId");

                ViewBag.EmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName");


                ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName");
                ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName");


                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");
                ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");


                ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName");
                ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName");


                ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");

                ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber");
                ViewBag.LienBankAccountId = new SelectList(db.BankAccountNoLienBanks, "LienBankAccountId", "BankAccountNumber");


                //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName");


                ViewBag.Title = "Create";
                #region excelupload
                if (Flag == 1)
                {
                    var userid = HttpContext.Session.GetString("userid");

                    if (userid.ToString() == "" || userid == null)
                    {

                        return BadRequest();
                    }



                    List<Temp_COM_ProformaInvoice> InvoiceList = db.Temp_COM_ProformaInvoices.Where(m => m.userid == userid.ToString()).ToList();


                    List<COM_ProformaInvoice> InvoiceListForView = new List<COM_ProformaInvoice>();

                    int supplierinformationid = 0;
                    int SisterConcernCompanyid = 0;
                    int currencyid = 0;
                    string unitmasterid = "Pcs";


                    foreach (Temp_COM_ProformaInvoice item in InvoiceList)
                    {

                        SupplierInformation supplier = db.SupplierInformations.Where(m => m.SupplierName.ToUpper().Contains(item.Supplier)).FirstOrDefault();
                        if (supplier != null)
                        {
                            supplierinformationid = supplier.ContactID;
                        }

                        SisterConcernCompany company = db.SisterConcernCompany.Where(m => m.CompanyName.ToUpper().Contains(item.Company)).FirstOrDefault();
                        if (company != null)
                        {
                            SisterConcernCompanyid = company.SisterConcernCompanyId;
                        }

                        Currency currency = db.Currency.Where(m => m.CurCode.ToUpper().Contains(item.Currency)).FirstOrDefault();
                        if (currency != null)
                        {
                            currencyid = currency.CurrencyId;
                        }

                        UnitMaster unitmaster = db.UnitMasters.Where(m => m.UnitMasterId.ToUpper().Contains(item.Unit)).FirstOrDefault();
                        if (unitmaster != null)
                        {
                            unitmasterid = unitmaster.UnitMasterId;
                        }

                        COM_ProformaInvoice COM_CNFBillImportDetail = new COM_ProformaInvoice
                        {
                            PINo = item.PINo,
                            PIDate = (DateTime.Parse(item.PIDate.ToString()).Date),// if (item.ShipDate == null) { return DateTime.Now.Date; } else { return item.ShipDate; };

                            CommercialCompanyId = SisterConcernCompanyid,
                            CurrencyId = currencyid,
                            SupplierId = supplierinformationid,


                            Company = company,
                            Currency = currency,
                            SupplierInformation = supplier,

                            UnitMaster = unitmaster,


                            ImportPONo = item.ImportPONo,
                            FileNo = item.FileNo,
                            LCAF = item.LCAF,

                            ItemGroupName = item.ItemGroupName,
                            ItemDescription = item.ItemDescription,
                            UnitMasterId = item.Unit, //"Pcs"
                            ImportQty = decimal.Parse(item.ImportQty),
                            ImportRate = decimal.Parse(item.ImportRate),
                            TotalValue = decimal.Parse(item.TotalValue),
                            IsDelete = false
                        };

                        //COM_CNFBillImportDetail.COM_CNFExpanseTypes = new COM_CNFExpanseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };
                        InvoiceListForView.Add(COM_CNFBillImportDetail);
                    }



                    return View("Create", InvoiceListForView);

                }
                #endregion
                else
                {
                    List<COM_ProformaInvoice> asdf = db.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString())).ToList();
                    return View(asdf);
                }

                //ViewBag.unitmaster = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitName");
                //ViewBag.destination = new SelectList(db.Destinations.Where(c => c.DestinationID > 0), "DestinationId", "DestinationNameSearch");

                //ViewBag.ExportOrderStatusId = new SelectList(db.ExportOrderStatus.Where(c => c.ExportOrderStatusId > 0), "ExportOrderStatusId", "ExportOrderStatusName");
                //ViewBag.ExportOrderCategoryId = new SelectList(db.ExportOrderCategorys.Where(c => c.ExportOrderCategoryId > 0), "ExportOrderCategoryId", "ExportOrderCategoryName");
                //ViewBag.ShipModeId = new SelectList(db.ShipModes.Where(c => c.ShipModeId > 0), "ShipModeId", "ShipModeName");





            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // POST: COM_ProformaInvoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(/*Include =*/ "ExportOrderID,StyleID,BuyerContactPONo,POLineNo,PoDate,DestinationID,OrderQty,UnitMasterId,Rate,CM,ShipMode,ExFactoryDate,ShipDate,AddedBy,DateAdded,UpdatedBy,DateUpdated,ExportOrderStatus,ExportOrderCategory,Remark")] ExportOrder exportOrder)
        public ActionResult Create(List<COM_ProformaInvoice> COM_ProformaInvoices)
        {
            try
            {
                if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
                {
                    return NotFound();

                }

                if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }

                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });





                {
                    //if (ModelState.IsValid)
                    {
                        var text = "";
                        List<COM_ProformaInvoice_Sub> com_proforma_itemgrouplist = new List<COM_ProformaInvoice_Sub>();

                        foreach (var item in COM_ProformaInvoices)
                        {
                            //WeekdaySectionWise data = db.WeekdaySectionWise.FirstOrDefault(s => s.ComId == item.ComId && s.SectionId == item.SectionId && s.FromDate.Date >= item.FromDate.Date && s.ToDate.Date <= item.ToDate.Date);
                            if (item.PIId > 0)
                            {
                                if (item.IsDelete == false)
                                {
                                    db.Entry(item).State = EntityState.Modified;
                                    //item.DateAdded = DateTime.Now;
                                    item.DateUpdated = DateTime.Now;
                                    item.ComId = (HttpContext.Session.GetString("comid"));


                                    var asdf = db.COM_ProformaInvoice_Subs.Where(x => x.PIId == item.PIId);
                                    db.COM_ProformaInvoice_Subs.RemoveRange(asdf);

                                    item.COM_ProformaInvoice_Subs = new List<COM_ProformaInvoice_Sub>();
                                    for (int i = 0; i < item.ItemDescArray.Length; i++)
                                    {


                                        text += item.ItemDescArray[i] + ",";


                                        COM_ProformaInvoice_Sub COM_ProformaInvoice_Subs = new COM_ProformaInvoice_Sub { ItemDescId = int.Parse(item.ItemDescArray[i]) }; //InvoiceId = 1,

                                        item.COM_ProformaInvoice_Subs.Add(COM_ProformaInvoice_Subs);




                                        //COM_ProformaInvoice_Sub itemgroupsingle = new COM_ProformaInvoice_Sub();

                                        //itemgroupsingle.PIId = item.PIId;
                                        //itemgroupsingle.ItemDescId = int.Parse(item.ItemDescArray[i]);
                                        ////com_proforma_itemgrouplist.Add(itemgroupsingle);

                                        //item.COM_ProformaInvoice_Subs.Add(itemgroupsingle);
                                    }


                                }
                                else
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                    db.SaveChanges();
                                }




                                //var devicetype = "";
                                //if (request.Browser.IsMobileDevice)
                                //{
                                //    devicetype = "Mobile";
                                //    //mobile
                                //}
                                //else
                                //{
                                //    devicetype = "Computer";

                                //    //laptop or desktop
                                //}
                                //UserTransactionLog usertran = new UserTransactionLog();
                                //usertran.IPAddress = GTRGetIPAddress();
                                //usertran.MacAddress = GTRGetMacAddress();
                                //usertran.ComId = 0;
                                //usertran.userid = HttpContext.Session.GetString("userid");
                                //usertran.DeviceType = devicetype;
                                //usertran.FlagValue = "1";
                                //usertran.WebLink = HttpContext.Request.Url.AbsoluteUri;


                                //message = "Weekend update succeded";
                            }
                            else
                            {
                                item.DateAdded = DateTime.Now;
                                //item.DateUpdated = DateTime.Now;
                                item.ComId = (HttpContext.Session.GetString("comid"));
                                item.UserId = HttpContext.Session.GetString("userid");
                                text = "";
                                if (item.IsDelete == false)
                                {
                                    //item.ItemDescArray = item.ItemDescList.Split(',');


                                    item.COM_ProformaInvoice_Subs = new List<COM_ProformaInvoice_Sub>();
                                    for (int i = 0; i < item.ItemDescArray.Length; i++)
                                    {


                                        text += item.ItemDescArray[i] + ",";


                                        COM_ProformaInvoice_Sub COM_ProformaInvoice_Subs = new COM_ProformaInvoice_Sub { ItemDescId = int.Parse(item.ItemDescArray[i]) }; //InvoiceId = 1,

                                        item.COM_ProformaInvoice_Subs.Add(COM_ProformaInvoice_Subs);




                                        //COM_ProformaInvoice_Sub itemgroupsingle = new COM_ProformaInvoice_Sub();

                                        //itemgroupsingle.PIId = item.PIId;
                                        //itemgroupsingle.ItemDescId = int.Parse(item.ItemDescArray[i]);
                                        ////com_proforma_itemgrouplist.Add(itemgroupsingle);

                                        //item.COM_ProformaInvoice_Subs.Add(itemgroupsingle);
                                    }


                                    //foreach (var item in Import)
                                    //{
                                    //    COM_CNFBillImportDetails COM_CNFBillImportDetail = new COM_CNFBillImportDetails();
                                    //    COM_CNFBillImportDetail.ExpenseHeadID = item.ExpenseHeadID;
                                    //    COM_CNFBillImportDetail.Amount = 0;
                                    //    COM_CNFBillImportDetail.IsCheck = false;
                                    //    COM_CNFBillImportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };
                                    //    Importmaster.COM_CNFBillImportDetails.Add(COM_CNFBillImportDetail);
                                    //}





                                    //db.COM_ProformaInvoice_Subs.AddRange(com_proforma_itemgrouplist);


                                    item.ItemDescList = text.TrimEnd(',');
                                    //item.COM_ProformaInvoice_Subs.Add(com_proforma_itemgrouplist);


                                    db.COM_ProformaInvoices.Add(item);
                                    //db.WeekdaySectionWise.Add(item);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //db.ExportOrders.Add(item);
                                    ////db.WeekdaySectionWise.Add(item);
                                    //db.SaveChanges();

                                }
                                //message = "Weekend save succeded";
                            }
                        }

                        //db.ExportOrders.Add(exportOrder);
                        //db.SaveChanges();
                        return Json(new { Success = 1, PIId = 0, ex = "Data Save Successfully" });
                    }

                    //return View(exportOrder);
                    return Json(new { Success = 0, PIId = 0, ex = "Unable To Save.." });

                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message.ToString()
                });

            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult UploadFiles()
        {
            try
            {

                #region excelupload
                //var userid = Session["userid"];
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }

                //if (Request.Files.Count > 0)
                //{
                //    HttpFileCollectionBase files = Request.Files;

                //    for (int i = 0; i < files.Count; i++)
                //    {
                //        HttpPostedFileBase postedFile = files[i];

                //        string filePath = string.Empty;
                //        if (postedFile != null)
                //        {
                //            string path = Server.MapPath("~/Content/Upload/");
                //            if (!Directory.Exists(path))
                //            {
                //                Directory.CreateDirectory(path);
                //            }

                //            filePath = path + Path.GetFileName(postedFile.FileName);
                //            string extension = Path.GetExtension(postedFile.FileName);
                //            postedFile.SaveAs(filePath);

                //            string conString = string.Empty;
                //            switch (extension)
                //            {
                //                case ".xls": //Excel 97-03.
                //                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                //                    break;
                //                case ".xlsx": //Excel 07 and above.
                //                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                //                    break;
                //            }

                //            DataTable dt0 = new DataTable();

                //            conString = string.Format(conString, filePath);

                //            using (OleDbConnection connExcel = new OleDbConnection(conString))
                //            {
                //                using (OleDbCommand cmdExcel = new OleDbCommand())
                //                {
                //                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                //                    {
                //                        cmdExcel.Connection = connExcel;

                //                        //Get the name of First Sheet.
                //                        connExcel.Open();

                //                        //dt.Load(cmdExcel.ExecuteReader());

                //                        DataTable dtExcelSchema;// DataTable mySheets = myConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" }); 
                //                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //                        DataTable dtExcelSchemaFinal = new DataTable();



                //                        dtExcelSchemaFinal = dtExcelSchema.Clone();

                //                        foreach (DataRow drtableOld in dtExcelSchema.Rows)
                //                        {
                //                            string strSheetTableName = drtableOld["TABLE_NAME"].ToString();

                //                            if (strSheetTableName.Contains("Filter"))

                //                            {

                //                            }
                //                            else
                //                            {
                //                                dtExcelSchemaFinal.ImportRow(drtableOld);
                //                            }
                //                        }


                //                        string sheetName0 = dtExcelSchemaFinal.Rows[0]["TABLE_NAME"].ToString();


                //                        //connExcel.Close();

                //                        //Read Data from First Sheet.
                //                        //connExcel.Open();
                //                        cmdExcel.CommandText = "SELECT SL as Id,SL,*,'" + userid + "' as userid  From [" + sheetName0 + "] where len(PINo) > 2";
                //                        odaExcel.SelectCommand = cmdExcel;
                //                        odaExcel.Fill(dt0);
                //                        connExcel.Close();
                //                    }
                //                }
                //            }


                //            #region details ///details table function///

                //            string table_Details = "Temp_COM_ProformaInvoice";
                //            string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //            SqlConnection con = new SqlConnection(connectionString);
                //            SqlCommand cmd = new SqlCommand("delete from dbo." + table_Details + " where userid   in ('" + userid + "', '')", con);
                //            con.Open();
                //            cmd.ExecuteNonQuery();
                //            //Response.Redirect("done.aspx");
                //            con.Close();



                //            conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                //            using (SqlConnection conn = new SqlConnection(conString))
                //            {
                //                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                //                {
                //                    //bulkCopy.DestinationTableName = table;
                //                    DestinationTableName = "dbo." + table_Details // "+"_Temp
                //                };
                //                conn.Open();




                //                DataTable schema = conn.GetSchema("Columns", new[] { null, null, table_Details, null });
                //                foreach (DataColumn sourceColumn in dt0.Columns)
                //                {
                //                    foreach (DataRow row in schema.Rows)
                //                    {
                //                        if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                //                        {
                //                            bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                //                            break;
                //                        }
                //                        //bulkCopy.ColumnMappings.Add("userid", (string)row["COLUMN_NAME"]);
                //                    }
                //                }
                //                bulkCopy.WriteToServer(dt0);

                //                conn.Close();
                //            }
                //            #endregion

                //            ///details table function///
                //            //}




                //        }

                //    }

                //}



                #endregion

                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;

                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }
        // GET: COM_ProformaInvoice/Edit/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            COM_ProformaInvoice cOM_ProformaInvoice = db.COM_ProformaInvoices.Where(m => m.PIId.ToString() == id.ToString()).FirstOrDefault();
            if (cOM_ProformaInvoice == null)
            {
                return NotFound();
            }
            ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", cOM_ProformaInvoice.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_ProformaInvoice.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_ProformaInvoice.SupplierId);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_ProformaInvoice.UnitMasterId);
            ViewBag.EmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName", cOM_ProformaInvoice.EmployeeId);

            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_ProformaInvoice.ItemGroupId);
            ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_ProformaInvoice.ItemDescId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_ProformaInvoice.GroupLCId);

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_ProformaInvoice.PaymentTermsId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_ProformaInvoice.DayListId);


            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_ProformaInvoice.OpeningBankId);

            //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks.Where(x => x.OpeningBankId == cOM_ProformaInvoice.BankAccountNos.OpeningBankId), "OpeningBankId", "OpeningBankName");
            ViewBag.BankAccountId = new SelectList(db.BankAccountNos.Where(x => x.BankAccountId == cOM_ProformaInvoice.BankAccountId), "BankAccountId", "BankAccountNumber", cOM_ProformaInvoice.BankAccountId);
            ViewBag.LienBankAccountId = new SelectList(db.BankAccountNoLienBanks, "LienBankAccountId", "BankAccountNumber", cOM_ProformaInvoice.LienBankAccountId);
            //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName" , cOM_ProformaInvoice.PINatureId);

            string itemdesc = cOM_ProformaInvoice.ItemDescList;

            if (itemdesc == null)
            {

                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");
            }
            else
            {
                string[] split = itemdesc.Split(',');
                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName", split);


            }

            //foreach (string item in split)
            //{

            //}



            //ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");

            return View("Edit", cOM_ProformaInvoice);
        }

        // POST: COM_ProformaInvoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COM_ProformaInvoice cOM_ProformaInvoice)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    var text = "";
                    List<COM_ProformaInvoice_Sub> com_proforma_itemgrouplist = new List<COM_ProformaInvoice_Sub>();

                    var existitemdescid = db.COM_ProformaInvoice_Subs.Where(x => x.PIId == cOM_ProformaInvoice.PIId);
                    foreach (COM_ProformaInvoice_Sub ss in existitemdescid)
                    {
                        db.COM_ProformaInvoice_Subs.Remove(ss);
                    }



                    for (int i = 0; i < cOM_ProformaInvoice.ItemDescArray.Length; i++)
                    {


                        text += cOM_ProformaInvoice.ItemDescArray[i] + ",";


                        COM_ProformaInvoice_Sub itemgroupsingle = new COM_ProformaInvoice_Sub();
                        itemgroupsingle.PIId = cOM_ProformaInvoice.PIId;
                        itemgroupsingle.ItemDescId = int.Parse(cOM_ProformaInvoice.ItemDescArray[i]);
                        com_proforma_itemgrouplist.Add(itemgroupsingle);
                    }

                    db.COM_ProformaInvoice_Subs.AddRange(com_proforma_itemgrouplist);


                    cOM_ProformaInvoice.ItemDescList = text.TrimEnd(',');


                    //var values = "1,2";
                    //$('#StudentId').val([1, 2]).trigger('focus').change();


                    cOM_ProformaInvoice.DateUpdated = DateTime.Now;
                    cOM_ProformaInvoice.UpdateByUserId = HttpContext.Session.GetString("userid");

                    db.Entry(cOM_ProformaInvoice).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return Json(new { Success = 1, PIId = 0, ex = "" });
                }
                //ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", cOM_ProformaInvoice.SisterConcernCompanyId);
                //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_ProformaInvoice.CurrencyId);
                //ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_ProformaInvoice.SupplierId);
                //ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitGroupId", cOM_ProformaInvoice.UnitMasterId);


                //ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_ProformaInvoice.ItemGroupId);
                //ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_ProformaInvoice.ItemDescId);



                ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", cOM_ProformaInvoice.CommercialCompanyId);
                ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_ProformaInvoice.CurrencyId);
                ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_ProformaInvoice.SupplierId);
                ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_ProformaInvoice.UnitMasterId);
                ViewBag.EmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName", cOM_ProformaInvoice.EmployeeId);

                ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_ProformaInvoice.ItemGroupId);
                ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_ProformaInvoice.ItemDescId);
                ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_ProformaInvoice.GroupLCId);

                ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_ProformaInvoice.PaymentTermsId);
                ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_ProformaInvoice.DayListId);


                ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_ProformaInvoice.OpeningBankId);

                //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks.Where(x => x.OpeningBankId == cOM_ProformaInvoice.BankAccountNos.OpeningBankId), "OpeningBankId", "OpeningBankName");
                ViewBag.BankAccountId = new SelectList(db.BankAccountNos.Where(x => x.BankAccountId == cOM_ProformaInvoice.BankAccountId), "BankAccountId", "BankAccountNumber", cOM_ProformaInvoice.BankAccountId);
                ViewBag.LienBankAccountId = new SelectList(db.BankAccountNoLienBanks, "LienBankAccountId", "BankAccountNumber", cOM_ProformaInvoice.LienBankAccountId);
                //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName", cOM_ProformaInvoice.PINatureId);

                string itemdesc = cOM_ProformaInvoice.ItemDescList;

                if (itemdesc == null)
                {

                    ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");
                }
                else
                {
                    string[] split = itemdesc.Split(',');
                    ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName", split);


                }
                ViewBag.Title = "Edit";
                return View();
                //return Json(new { Success = 1, PIId = 0, ex = "Unable to Update the Date" });
            }
            catch (Exception ex)
            {
                return View();
                //throw ex;
                //return Json(new { Success = 1, PIId = 0, ex = ex.Message });
            }

        }

        // GET: COM_ProformaInvoice/Delete/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            COM_ProformaInvoice cOM_ProformaInvoice = db.COM_ProformaInvoices.Where(m => m.PIId.ToString() == id.ToString()).FirstOrDefault();
            if (cOM_ProformaInvoice == null)
            {
                return NotFound();
            }
            ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", cOM_ProformaInvoice.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency.OrderBy(x => x.isDefault), "CurrencyId", "CurCode", cOM_ProformaInvoice.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_ProformaInvoice.SupplierId);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_ProformaInvoice.UnitMasterId);
            ViewBag.EmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName", cOM_ProformaInvoice.EmployeeId);
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_ProformaInvoice.ItemGroupId);
            ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_ProformaInvoice.ItemDescId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_ProformaInvoice.GroupLCId);

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_ProformaInvoice.PaymentTermsId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_ProformaInvoice.DayListId);


            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_ProformaInvoice.OpeningBankId);

            //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks.Where(x => x.OpeningBankId == cOM_ProformaInvoice.BankAccountNos.OpeningBankId), "OpeningBankId", "OpeningBankName");
            ViewBag.BankAccountId = new SelectList(db.BankAccountNos.Where(x => x.BankAccountId == cOM_ProformaInvoice.BankAccountId), "BankAccountId", "BankAccountNumber", cOM_ProformaInvoice.BankAccountId);
            ViewBag.LienBankAccountId = new SelectList(db.BankAccountNoLienBanks, "LienBankAccountId", "BankAccountNumber", cOM_ProformaInvoice.LienBankAccountId);
            //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName" , cOM_ProformaInvoice.PINatureId);


            string itemdesc = cOM_ProformaInvoice.ItemDescList;

            if (itemdesc == null)
            {
                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");


            }
            else
            {
                string[] split = itemdesc.Split(',');
                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName", split);


            }

            return View("Edit", cOM_ProformaInvoice);
        }

        // POST: COM_ProformaInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                COM_ProformaInvoice cOM_ProformaInvoice = db.COM_ProformaInvoices.Find(id);
                db.COM_ProformaInvoices.Remove(cOM_ProformaInvoice);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = cOM_ProformaInvoice.PIId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public JsonResult PI_Duplicate_Check(string id)
        {
            //try
            //{
            //    db.Configuration.ProxyCreationEnabled = false;
            //    db.Configuration.LazyLoadingEnabled = false;


            //    List<COM_ProformaInvoice> COM_ProformaInvoice = db.COM_ProformaInvoices.Where(x => x.PINo == id && x.comid == int.Parse(HttpContext.Session.GetString("comid"))).ToList();
            //    //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

            //    return Json (new { success = 1, COM_ProformaInvoice }, JsonRequestBehavior.AllowGet);
            //    //return Json("tesst", JsonRequestBehavior.AllowGet);

            //}
            //catch (Exception ex)
            //{
            //    return Json(new { success = 0, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                //db.Configuration.ProxyCreationEnabled = false;
                //db.Configuration.LazyLoadingEnabled = false;
                int comid = int.Parse(HttpContext.Session.GetString("comid"));
                List<COM_ProformaInvoice> COM_ProformaInvoicelist = db.COM_ProformaInvoices.ToList(); //x.PINo == id &&  //.Where(x =>  x.comid == comid)

                List<SelectListItem> pilist = new List<SelectListItem>();

                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (COM_ProformaInvoicelist != null)
                {
                    foreach (COM_ProformaInvoice x in COM_ProformaInvoicelist)
                    {
                        pilist.Add(new SelectListItem { Text = x.PINo, Value = x.PIId.ToString() });
                    }
                }

                return Json(new SelectList(pilist, "Value", "Text"));
            }
            catch (Exception ex)
            {

                return Json(new { success = 0, values = ex.Message.ToString() });

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
