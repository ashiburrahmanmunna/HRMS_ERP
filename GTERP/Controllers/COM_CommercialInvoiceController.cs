using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;
using GTCommercial.Models.Common;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class COM_CommercialInvoiceController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();
        string comid = AppData.intComId;


        // GET: COM_CommercialInvoice
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Index(string UserList, int? supplierid, string FromDate, string ToDate)
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


            //var cOM_CommercialInvoices = db.COM_CommercialInvoices.Include(c => c.CommercialCompany).Include(c => c.Currency).Include(c => c.SupplierInformations).Include(c => c.UnitMaster);
            //return View(cOM_CommercialInvoices.ToList());


            List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria", new SqlParameter("Criteria", "CommercialInvoice"))).ToList();
            ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName");


            if (UserList == null)
            {
                UserList = HttpContext.Session.GetString("userid");
            }


            var x = db.COM_CommercialInvoices.Where(p => (p.userid.ToString() == "") && (p.CommercialInvoiceDate >= dtFrom && p.CommercialInvoiceDate <= dtTo)).ToList();

            if (UserList == "")
            {
                x = db.COM_CommercialInvoices.Where(p => (p.CommercialInvoiceDate >= dtFrom && p.CommercialInvoiceDate <= dtTo)).ToList();
                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


            }
            else
            {
                x = db.COM_CommercialInvoices.Where(p => (p.userid.ToString() == UserList.ToString()) && (p.CommercialInvoiceDate >= dtFrom && p.CommercialInvoiceDate <= dtTo)).ToList();
                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.SupplierId.ToString() == supplierid.ToString()) && (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


            }


            return View(x);
        }

        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }



        // GET: COM_CommercialInvoice/Create
        [AllowAnonymous]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Create(int? supplierid, int? Flag)
        {

            try
            {
                ViewBag.SupplierId = supplierid;
                if (supplierid == null)
                {
                    supplierid = 0;
                }
                ViewBag.teststatus = db.DocumentStatuss;

                ViewBag.CommercialCompanyID = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName").ToList();
                ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode").ToList(); ;
                ViewBag.SupplierID = new SelectList(db.SupplierInformations, "ContactID", "SupplierName").ToList(); ;
                ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId").ToList(); ;
                ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo").ToList(); 
                ViewBag.DocumentStatusId = new SelectList(db.DocumentStatuss, "DocumentStatusId", "DocumentStatusName").ToList(); ;

                ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName");
                ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName");


                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");
                ViewBag.MachinaryLCId = new SelectList(db.COM_MachinaryLC_Masters, "MachinaryLCId", "MachinaryLCNo").ToList();
                ViewBag.CommercialLCTypeId = new SelectList(db.CommercialLCTypes, "CommercialLCTypeId", "CommercialLCTypeName").ToList();



                SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "BBLC" };
                SelectListItem selListItem1 = new SelectListItem() { Value = "1", Text = "Regular LC" };


                List<SelectListItem> newList = new List<SelectListItem>();

                //Add select list item to list of selectlistitems
                newList.Add(selListItem);
                newList.Add(selListItem1);


                ViewBag.LCType = newList;


                ViewBag.Title = "Create";
                #region excelupload
                if (Flag == 1)
                {
                    var userid = Session["userid"];

                    if (userid.ToString() == "" || userid == null)
                    {

                        return BadRequest();
                    }



                    List<Temp_COM_ProformaInvoice> InvoiceList = db.Temp_COM_ProformaInvoices.Where(m => m.userid == userid.ToString()).ToList();


                    List<COM_ProformaInvoice> InvoiceListForView = new List<COM_ProformaInvoice>();

                    int supplierinformationid = 0;
                    int commercialcompanyid = 0;
                    int currencyid = 0;
                    string unitmasterid = "Pcs";


                    foreach (Temp_COM_ProformaInvoice item in InvoiceList)
                    {

                        SupplierInformation supplier = db.SupplierInformations.Where(m => m.SupplierName.ToUpper().Contains(item.Supplier)).FirstOrDefault();
                        if (supplier != null)
                        {
                            supplierinformationid = supplier.ContactID;
                        }

                        CommercialCompany company = db.CommercialCompany.Where(m => m.CompanyName.ToUpper().Contains(item.Company)).FirstOrDefault();
                        if (company != null)
                        {
                            commercialcompanyid = company.CommercialCompanyId;
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

                            CommercialCompanyId = commercialcompanyid,
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
                            isDelete = false
                        };

                        //COM_CNFBillImportDetail.COM_CNFExpanseTypes = new COM_CNFExpanseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };
                        InvoiceListForView.Add(COM_CNFBillImportDetail);
                    }



                    return View("Create", InvoiceListForView);

                }
                #endregion
                else
                {
                    //COM_CommercialInvoice_Single_List multiobject = new COM_CommercialInvoice_Single_List();
                    //multiobject.COM_CommercialInvoice = new COM_CommercialInvoice();
                    //multiobject.COM_CommercialInvoice_List = new List<COM_CommercialInvoice>();
                    COM_CommercialInvoice singledata = new COM_CommercialInvoice();
                    singledata.CommercialCompanyID = 0;
                    List<COM_CommercialInvoice> multidata = db.COM_CommercialInvoices.Where(c => c.CommercialInvoiceId ==0).ToList();


                    return View();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // POST: COM_CommercialInvoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(List<COM_CommercialInvoice> COM_CommercialInvoices)
        {
            try
            {
                if (AppData.intComId == "0" || AppData.intComId == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Session Expired. Please Login Again for This Transaction").Message.ToString() });

                }

                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
                
                    //if (ModelState.IsValid)
                    //{
                    var text = "";
                    foreach (var item in COM_CommercialInvoices)
                    {

                        //db.Entry(item).State = EntityState.Modified;
                        //db.SaveChanges();

                        //WeekdaySectionWise data = db.WeekdaySectionWise.FirstOrDefault(s => s.ComId == item.ComId && s.SectionId == item.SectionId && s.FromDate.Date >= item.FromDate.Date && s.ToDate.Date <= item.ToDate.Date);
                        if (item.CommercialInvoiceId > 0)
                        {
                            if (item.isDelete == false)
                            {

                                var asdf = db.COM_CommercialInvoice_Subs.Where(x => x.CommercialInvoiceId == item.CommercialInvoiceId);
                                db.COM_CommercialInvoice_Subs.RemoveRange(asdf);

                                item.COM_CommercialInvoice_Subs = new List<COM_CommercialInvoice_Sub>();
                                for (int i = 0; i < item.ItemDescArray.Length; i++)
                                {
                                    text += item.ItemDescArray[i] + ",";

                                    COM_CommercialInvoice_Sub COM_CommercialInvoice_Subs = new COM_CommercialInvoice_Sub { ItemDescId = int.Parse(item.ItemDescArray[i]) }; //InvoiceId = 1,

                                    item.COM_CommercialInvoice_Subs.Add(COM_CommercialInvoice_Subs);

                                }


                                db.Entry(item).State = EntityState.Modified;
                                item.DateAdded = DateTime.Now;
                                item.DateUpdated = DateTime.Now;
                                item.comid = int.Parse(comid);
                                item.userid = HttpContext.Session.GetString("userid");



                            }
                            else
                            {
                                db.Entry(item).State = EntityState.Deleted;
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            item.DateAdded = DateTime.Now;
                            item.DateUpdated = DateTime.Now;
                            item.comid = int.Parse(AppData.intComId);
                            item.userid = HttpContext.Session.GetString("userid");
                            text = "";
                            if (item.isDelete == false)
                            {

                                item.COM_CommercialInvoice_Subs = new List<COM_CommercialInvoice_Sub>();
                                for (int i = 0; i < item.ItemDescArray.Length; i++)
                                {


                                    text += item.ItemDescArray[i] + ",";


                                    COM_CommercialInvoice_Sub COM_CommercialInvoice_Subs = new COM_CommercialInvoice_Sub { ItemDescId = int.Parse(item.ItemDescArray[i]) }; //InvoiceId = 1,

                                    item.COM_CommercialInvoice_Subs.Add(COM_CommercialInvoice_Subs);

                                }
                                item.ItemDescList = text.TrimEnd(',');

                                db.COM_CommercialInvoices.Add(item);
                                db.SaveChanges();
                            }
                            //else
                            //{
                            //    db.COM_CommercialInvoices.Add(item);
                            //    //db.WeekdaySectionWise.Add(item);
                            //    db.SaveChanges();

                            //}
                            //message = "Weekend save succeded";
                        }
                    }

                    //db.ExportOrders.Add(exportOrder);
                    //db.SaveChanges();
                    return Json(new { Success = 1, ex = "Data Save Successfully" });
                //}
                //return Json(new { Success = 0, CommercialInvoiceId = 1, ex = errors });

                //return View(exportOrder);
                //return Json(new { Success = 0, ProductId = 0, ex = "" });


            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message.ToString()
                });

            }
            //return Json(new { Success = 1, CommercialInvoiceId = 1, ex = "" });


        }

        // GET: COM_CommercialInvoice/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_CommercialInvoice cOM_CommercialInvoice = db.COM_CommercialInvoices.Find(id);
            if (cOM_CommercialInvoice == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            ViewBag.CommercialCompanyID = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CommercialInvoice.CommercialCompanyID);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_CommercialInvoice.CurrencyId);
            ViewBag.SupplierID = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_CommercialInvoice.SupplierID);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_CommercialInvoice.UnitMasterId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_CommercialInvoice.BBLCId);
            ViewBag.MachinaryLCId = new SelectList(db.COM_MachinaryLC_Masters, "MachinaryLCId", "MachinaryLCNo", cOM_CommercialInvoice.MachinaryLCId);

            ViewBag.DocumentStatusId = new SelectList(db.DocumentStatuss, "DocumentStatusId", "DocumentStatusName" , cOM_CommercialInvoice.DocumentStatusId);


            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_CommercialInvoice.ItemGroupId);
            ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_CommercialInvoice.ItemDescId);
            ViewBag.CommercialLCTypeId = new SelectList(db.CommercialLCTypes, "CommercialLCTypeId", "CommercialLCTypeName", cOM_CommercialInvoice.CommercialLCTypeId);





            string itemdesc = cOM_CommercialInvoice.ItemDescList;

            if (itemdesc == null)
            {

                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");
            }
            else
            {
                string[] split = itemdesc.Split(',');
                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName", split);


            }


            //ViewBag.MachinaryLCId = new SelectList(db.COM_MachinaryLC_Masters, "MachinaryLCId", "MachinaryLCNo", cOM_CommercialInvoice.MachinaryLCId).ToList();




            //SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "BBLC" };
            //SelectListItem selListItem1 = new SelectListItem() { Value = "1", Text = "Regular LC" };


            //List<SelectListItem> newList = new List<SelectListItem>();

            ////Add select list item to list of selectlistitems
            //newList.Add(selListItem);
            //newList.Add(selListItem1);


            //ViewBag.LCType = new SelectList(newList, "Value", "Text", cOM_CommercialInvoice.LCType).ToList(); 


            return View(cOM_CommercialInvoice);
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_CommercialInvoice cOM_CommercialInvoice = db.COM_CommercialInvoices.Find(id);
            if (cOM_CommercialInvoice == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";

            ViewBag.CommercialCompanyID = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CommercialInvoice.CommercialCompanyID);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_CommercialInvoice.CurrencyId);
            ViewBag.SupplierID = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_CommercialInvoice.SupplierID);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_CommercialInvoice.UnitMasterId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_CommercialInvoice.BBLCId);
            ViewBag.DocumentStatusId = new SelectList(db.DocumentStatuss, "DocumentStatusId", "DocumentStatusName", cOM_CommercialInvoice.DocumentStatusId);


            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_CommercialInvoice.ItemGroupId);
            ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_CommercialInvoice.ItemDescId);
            ViewBag.CommercialLCTypeId = new SelectList(db.CommercialLCTypes, "CommercialLCTypeId", "CommercialLCTypeName", cOM_CommercialInvoice.CommercialLCTypeId);


            string itemdesc = cOM_CommercialInvoice.ItemDescList;

            if (itemdesc == null)
            {

                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName");
            }
            else
            {
                string[] split = itemdesc.Split(',');
                ViewBag.ItemDescArray = new MultiSelectList(db.ItemDescs, "ItemDescId", "ItemDescName", split);


            }


            ViewBag.MachinaryLCId = new SelectList(db.COM_MachinaryLC_Masters, "MachinaryLCId", "MachinaryLCNo", cOM_CommercialInvoice.MachinaryLCId).ToList();




            SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "BBLC" };
            SelectListItem selListItem1 = new SelectListItem() { Value = "1", Text = "Regular LC" };


            List<SelectListItem> newList = new List<SelectListItem>();

            //Add select list item to list of selectlistitems
            newList.Add(selListItem);
            newList.Add(selListItem1);


            ViewBag.LCType = newList;


            return View("Edit",cOM_CommercialInvoice);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                var asdf = db.COM_CommercialInvoice_Subs.Where(x => x.CommercialInvoiceId == id);
                db.COM_CommercialInvoice_Subs.RemoveRange(asdf);

                COM_CommercialInvoice COM_CommercialInvoice = db.COM_CommercialInvoices.Find(id);
      

                db.COM_CommercialInvoices.Remove(COM_CommercialInvoice);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = COM_CommercialInvoice.CommercialInvoiceId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }

        }


        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult BBLCInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                COM_BBLC_Master BBLCMaster = db.COM_BBLC_Master.Where(y => y.BBLCId == id).SingleOrDefault();
                //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

                return Json(BBLCMaster, JsonRequestBehavior.AllowGet);
                //return Json("tesst", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new SelectList(product, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult MachinaryLCInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                COM_MachinaryLC_Master COM_MachinaryLC_Masters = db.COM_MachinaryLC_Masters.Where(y => y.MachinaryLCId == id).SingleOrDefault();
                //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

                return Json(COM_MachinaryLC_Masters, JsonRequestBehavior.AllowGet);
                //return Json("tesst", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new SelectList(product, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult getBBLC(int id)
        {
            List<COM_BBLC_Master> bblcmasterlist = db.COM_BBLC_Master.Where(x => x.CommercialCompanyId == id).ToList();

            List<SelectListItem> bblclist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (bblcmasterlist != null)
            {
                foreach (COM_BBLC_Master x in bblcmasterlist)
                {
                    bblclist.Add(new SelectListItem { Text = x.BBLCNo, Value = x.BBLCId.ToString() });
                }
            }
            return Json(new SelectList(bblclist, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult getregularlc(int id)
        {
            List<COM_MachinaryLC_Master> bblcmasterlist = db.COM_MachinaryLC_Masters.Where(x => x.CommercialCompanyId == id).ToList();

            List<SelectListItem> regularlclist = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (bblcmasterlist != null)
            {
                foreach (COM_MachinaryLC_Master x in bblcmasterlist)
                {
                    regularlclist.Add(new SelectListItem { Text = x.MachinaryLCNo, Value = x.MachinaryLCId.ToString() });
                }
            }
            return Json(new SelectList(regularlclist, "Value", "Text", JsonRequestBehavior.AllowGet));
        }



        // POST: COM_CommercialInvoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COM_CommercialInvoice cOM_CommercialInvoice)
        {
            try
            {

       
            if (ModelState.IsValid)
            {
                var text = "";
                List<COM_CommercialInvoice_Sub> com_commercial_itemgrouplist = new List<COM_CommercialInvoice_Sub>();

                var existitemdescid = db.COM_CommercialInvoice_Subs.Where(x => x.CommercialInvoiceId == cOM_CommercialInvoice.CommercialInvoiceId);
                foreach (COM_CommercialInvoice_Sub ss in existitemdescid)
                {
                    db.COM_CommercialInvoice_Subs.Remove(ss);
                }


                    for (int i = 0; i < cOM_CommercialInvoice.ItemDescArray.Length; i++)
                    {


                        text += cOM_CommercialInvoice.ItemDescArray[i] + ",";


                        COM_CommercialInvoice_Sub itemgroupsingle = new COM_CommercialInvoice_Sub();
                        itemgroupsingle.CommercialInvoiceId = cOM_CommercialInvoice.CommercialInvoiceId;
                        itemgroupsingle.ItemDescId = int.Parse(cOM_CommercialInvoice.ItemDescArray[i]);
                        com_commercial_itemgrouplist.Add(itemgroupsingle);
                    }


                    db.COM_CommercialInvoice_Subs.AddRange(com_commercial_itemgrouplist);


                    cOM_CommercialInvoice.ItemDescList = text.TrimEnd(',');



                    cOM_CommercialInvoice.DateAdded = DateTime.Now;
                cOM_CommercialInvoice.DateUpdated = DateTime.Now;
                cOM_CommercialInvoice.userid = HttpContext.Session.GetString("userid");
                cOM_CommercialInvoice.comid =  int.Parse(AppData.intComId);


                db.Entry(cOM_CommercialInvoice).State = EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CommercialCompanyID = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CommercialInvoice.CommercialCompanyID);
            //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_CommercialInvoice.CurrencyId);
            //ViewBag.SupplierID = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_CommercialInvoice.SupplierID);
            //ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitGroupId", cOM_CommercialInvoice.UnitMasterId);

                ViewBag.CommercialCompanyID = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CommercialInvoice.CommercialCompanyID);
                ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_CommercialInvoice.CurrencyId);
                ViewBag.SupplierID = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_CommercialInvoice.SupplierID);
                ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", cOM_CommercialInvoice.UnitMasterId);
                ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_CommercialInvoice.BBLCId);
                ViewBag.MachinaryLCId = new SelectList(db.COM_MachinaryLC_Masters, "MachinaryLCId", "MachinaryLCNo", cOM_CommercialInvoice.MachinaryLCId);

                ViewBag.DocumentStatusId = new SelectList(db.DocumentStatuss, "DocumentStatusId", "DocumentStatusName", cOM_CommercialInvoice.DocumentStatusId);


                ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_CommercialInvoice.ItemGroupId);
                ViewBag.ItemDescId = new SelectList(db.ItemDescs, "ItemDescId", "ItemDescName", cOM_CommercialInvoice.ItemDescId);
                ViewBag.CommercialLCTypeId = new SelectList(db.CommercialLCTypes, "CommercialLCTypeId", "CommercialLCTypeName", cOM_CommercialInvoice.CommercialLCTypeId);



                return View(cOM_CommercialInvoice);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCI(int? id, string type)
        {
            try
            {

                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice] '" + id + "','" + AppData.intComId + "'");
                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice.rdlc");
                string SQLQuery = "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice] '" + id + "','" + AppData.intComId + "'";
                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
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
