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
using GTERP.AppData;
using GTERP.Controllers.Common;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GTCommercial.Controllers
{
  //[OverridableAuthorize]
    public class COM_BBLC_MasterController : Controller
    {
        private GTRDBContext db;
        public COM_BBLC_MasterController(GTRDBContext context)
        {
            db = context;
        }
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private string variablerefno;
        private string variableprintdate;


        // GET: COM_BBLC_Master
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public ActionResult Index(string UserList, string FromDate, string ToDate)
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

            List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria", new SqlParameter("Criteria", "BBLCUser"))).ToList();
            ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName");


            if (UserList == null)
            {
                UserList = HttpContext.Session.GetString("userid");
            }

            var cOM_BBLC_Master = db.COM_BBLC_Master.Where(p => p.LcOpeningDate >= dtFrom && p.LcOpeningDate <= dtTo);


            if (UserList == "")
            {
                cOM_BBLC_Master = db.COM_BBLC_Master.Where(p => p.LcOpeningDate >= dtFrom && p.LcOpeningDate <= dtTo);
                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


            }
            else
            {
                cOM_BBLC_Master = db.COM_BBLC_Master.Where(p => p.LcOpeningDate >= dtFrom && p.LcOpeningDate <= dtTo && (p.userid.ToString() == UserList.ToString()));
            }


            return View(cOM_BBLC_Master.ToList());
        }


        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
        }

        //GetBBLCListByMasterBBLCId
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public JsonResult GetBBLCListByMasterBBLCId(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.MasterLCID = id;
            //List<cOM_BBLC_Master> asdf = db.cOM_BBLC_Masters.Where(p => (p.MasterLCID.ToString() == id.ToString())).ToList();
            List<COM_BBLC_Master> BBLCMaster = db.COM_BBLC_Master.Where(m => m.BBLCId == id).ToList();
            List<BBLCListClass> data = new List<BBLCListClass>();

            foreach (var item in BBLCMaster)
            {
                BBLCListClass asdf = new BBLCListClass();
                //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                asdf.BBLCNo = item.BBLCNo;
                asdf.Importer = item.Company.CompanyName;
                asdf.Exporter = item.SupplierInformation.SupplierName; //DateTime.Parse(item.InvoiceDate.ToString()).ToString("dd-MMM-yy");
                asdf.PINumber = item.COM_BBLC_Details.FirstOrDefault().COM_ProformaInvoice.PINo;
                asdf.MasterContractNo = item.COM_GroupLC_Main.COM_GroupLC_Sub.FirstOrDefault().COM_MasterLC.BuyerLCRef;
                asdf.BBLCValue = item.TotalValue;
                asdf.InvoiceNo = "";// item.COM_CommercialInvoice.FirstOrDefault().CommercialInvoiceNo;


                data.Add(asdf);
            }

            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class BBLCListClass
        {
            public string BBLCNo { get; set; }

            /// </summary>
            public string Importer { get; set; }
            public string Exporter { get; set; }

            public string PINumber { get; set; }
            public string MasterContractNo { get; set; }

            public decimal? BBLCValue { get; set; }
            public string InvoiceNo { get; set; }

        }



        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]

        public ActionResult BBLCReport()
        {
            ViewBag.ReportType = "PDF";
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo");
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
            ViewBag.PIId = new SelectList(db.COM_ProformaInvoices.Where(m => m.GroupLCId == 0), "PIId", "PINo");

            var cOM_BBLC_Master = db.COM_BBLC_Master.Take(20);
            return View(cOM_BBLC_Master.ToList());
            

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        [HttpPost]
        public JsonResult UpdateBBLCRefNo(int BBLCId ,string RefNo , string PrintDate , string Percentage)
        {
            var userid = HttpContext.Session.GetString("userid");
            if (userid == null || userid == "")
            {
                return Json(new { Success = 0, data = "Please Login again for this transaction." }, JsonRequestBehavior.AllowGet);
            }
            COM_BBLC_Master existRefNo  = db.COM_BBLC_Master.Where(a => a.RefNo == RefNo && a.BBLCId != BBLCId).FirstOrDefault();

            if (existRefNo == null)
            {


                COM_BBLC_Master abc = db.COM_BBLC_Master.Where(a => a.BBLCId == BBLCId).FirstOrDefault();

                abc.RefNo = RefNo;
                abc.PrintDate = PrintDate;
                abc.Percentage = Percentage;
                abc.useridUpdateRefNo = userid;



                //Specify the fields that should be updated.
                db.Entry(abc).Property(x => x.RefNo).IsModified = true;
                db.Entry(abc).Property(x => x.PrintDate).IsModified = true;
                db.Entry(abc).Property(x => x.Percentage).IsModified = true;
                db.Entry(abc).Property(x => x.useridUpdateRefNo).IsModified = true;



                db.SaveChanges();
            }
            else
            {
                //return Json("", JsonRequestBehavior.AllowGet);
                //return Json(new { Success = 0, data = existRefNo.BBLCNo }, JsonRequestBehavior.AllowGet);
                return Json(new { Success = 0, data = existRefNo.BBLCNo });
            }


            //return Json(new { Success = 1 , data = "" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = 1 , data = "" });
        }

        public JsonResult getProformaInvoiceByGroupLC(int? id)
        {
            List<COM_ProformaInvoice> COM_ProformaInvoices = null;
            if (id == null)
            {
                COM_ProformaInvoices = db.COM_ProformaInvoices.Where(x => x.GroupLCId == null).ToList();

            }
            else
            {
               COM_ProformaInvoices = db.COM_ProformaInvoices.Where(x => x.GroupLCId == id).ToList();
            }


            List<SelectListItem> liproformainvoice = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (COM_ProformaInvoices != null)
            {
                foreach (COM_ProformaInvoice x in COM_ProformaInvoices)
                {
                    liproformainvoice.Add(new SelectListItem { Text = x.PINo, Value = x.PIId.ToString() });
                }
            }
            //return Json(new SelectList(liproformainvoice, "Value", "Text", JsonRequestBehavior.AllowGet));
            return Json(new SelectList(liproformainvoice, "Value", "Text"));
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public JsonResult GetGroupLCById(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.GroupLCId = id;
            //List<cOM_BBLC_Master> asdf = db.cOM_BBLC_Masters.Where(p => (p.GroupLCId.ToString() == id.ToString())).ToList();
            List<COM_GroupLC_Main> cOM_BBLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            List<GroupLCInfo> data = new List<GroupLCInfo>();
            foreach (var item in cOM_BBLC_Masters)
            {
                GroupLCInfo asdf = new GroupLCInfo();
                asdf.GroupLCId = item.GroupLCId;
                asdf.GroupLCRef = item.GroupLCRefName;
                asdf.BuyerName = item.BuyerInformation.BuyerName;
                asdf.Beneficiary = "Benificary";
                asdf.ContactRef = item.GroupLCRefName;
                asdf.LCDate = DateTime.Parse(item.FirstShipDate.ToString()).ToString("dd-MMM-yy");
                asdf.TotalValue = decimal.Parse(item.TotalGroupLCValueManual.ToString());

                data.Add(asdf);
            }

            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class GroupLCInfo
        {

            /// </summary>
            public int GroupLCId { get; set; }
            public string GroupLCRef { get; set; }
            public string BuyerName { get; set; }
            public string Beneficiary { get; set; }
            public string ContactRef { get; set; }
            public string LCDate { get; set; }
            public decimal TotalValue { get; set; }
        }

        // GET: COM_BBLC_Master/Create
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public ActionResult Create(int? SupplierId)
        {
            ViewBag.Title = "Create";
            ViewBag.SupplierId = SupplierId;
            if (SupplierId == null)
            {
                SupplierId = 0;
            }

            ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == 0).ToList();

            ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == SupplierId && m.ItemGroupId != null).ToList();

            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch");
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName");
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", SupplierId);

            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName");
            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName");

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName");
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName");
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName");


            return View();
        }

        // POST: COM_BBLC_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(COM_BBLC_Master cOM_BBLC_Master)
        {
            try
            {
                if (AppData.intComId == "0" || AppData.intComId == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }
                ViewBag.Title = "Create";
                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                //if (ModelState.IsValid)
                {

                    if (cOM_BBLC_Master.BBLCId > 0)
                    {
                        cOM_BBLC_Master.DateUpdated = DateTime.Now;
                        //cOM_BBLC_Master.DateAdded = DateTime.Now;
                        cOM_BBLC_Master.comid = int.Parse(HttpContext.Session.GetString("comid"));
                        cOM_BBLC_Master.useridUpdate = HttpContext.Session.GetString("userid");


                        if (cOM_BBLC_Master.userid == null || cOM_BBLC_Master.userid == "0")
                        {

                            cOM_BBLC_Master.userid = HttpContext.Session.GetString("userid");

                        }
                        if (cOM_BBLC_Master.comid == null || cOM_BBLC_Master.comid == 0)
                        {
                            
                            cOM_BBLC_Master.comid = int.Parse(HttpContext.Session.GetString("comid"));

                        }

                        IQueryable<COM_BBLC_Details> grouplcsub = db.COM_BBLC_Details.Where(p => p.BBLCId == cOM_BBLC_Master.BBLCId);

                        foreach (COM_BBLC_Details ss in grouplcsub)
                        {
                            db.COM_BBLC_Details.Remove(ss);
                        }

                        if (cOM_BBLC_Master.COM_BBLC_Details != null)
                        {
                            foreach (COM_BBLC_Details ss in cOM_BBLC_Master.COM_BBLC_Details)
                            {
                                //ss.DateAdded = DateTime.Now;
                                //ss.DateUpdated = DateTime.Now;

                                //db.VoucherSubs.Add(ss);
                                db.COM_BBLC_Details.Add(ss);
                            }
                        }
                        db.Entry(cOM_BBLC_Master).State = EntityState.Modified;

                    }
                    else
                    {
                        cOM_BBLC_Master.DateAdded = DateTime.Now;
                        cOM_BBLC_Master.DateUpdated = DateTime.Now;
                        cOM_BBLC_Master.comid = int.Parse(HttpContext.Session.GetString("comid"));
                        cOM_BBLC_Master.userid = (HttpContext.Session.GetString("userid"));

                        //cOM_BBLC_Master.userid = HttpContext.Session.GetString("userid");

                        foreach (COM_BBLC_Details ss in cOM_BBLC_Master.COM_BBLC_Details)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            //db.cOM_BBLC_MasterExports.Add(ss);
                        }

                        db.COM_BBLC_Master.Add(cOM_BBLC_Master);
                    }

                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                return Json(new { Success = 1, BBLCId = cOM_BBLC_Master.BBLCId, ex = "" });

                //ViewBag.GroupLCId = new SelectList(db.cOM_BBLC_Masters, "GroupLCId", "GroupLCRefName", cOM_BBLC_Master.GroupLCId);
                //ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_BBLC_Master.CommercialCompanyId);
                //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_BBLC_Master.CurrencyId);
                //ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_BBLC_Master.DestinationID);
                //ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_BBLC_Master.PortOfLoadingId);
                //ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_BBLC_Master.ShipModeId);
                //ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_BBLC_Master.SupplierId);
                //return View(cOM_BBLC_Master);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    errors = ex.Message
                    //ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        // GET: COM_BBLC_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_BBLC_Master cOM_BBLC_Master = db.COM_BBLC_Master.Find(id);
            if (cOM_BBLC_Master == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            //ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == cOM_BBLC_Master.SupplierId).ToList();

            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_BBLC_Master.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_BBLC_Master.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_BBLC_Master.CurrencyId);
            ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_BBLC_Master.DestinationID);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_BBLC_Master.PortOfLoadingId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_BBLC_Master.ShipModeId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_BBLC_Master.SupplierId);

            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_BBLC_Master.OpeningBankId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_BBLC_Master.LienBankId);
            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_BBLC_Master.TradeTermId);

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_BBLC_Master.PaymentTermsId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_BBLC_Master.DayListId);
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_BBLC_Master.ItemGroupId);



            ViewBag.PIInformation = (from proformainvoice in db.COM_ProformaInvoices.Where(m => m.comid.ToString() == AppData.intComId && m.ItemGroupId != null)
                                         //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                     where proformainvoice.SupplierId == cOM_BBLC_Master.SupplierId &&
                                     !db.COM_BBLC_Details.Any(f => f.PIId == proformainvoice.PIId)
                                     select proformainvoice).Distinct().ToList();


            //ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == cOM_BBLC_Master.GroupLCId).ToList();

            List<COM_GroupLC_Main> cOM_BBLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == cOM_BBLC_Master.GroupLCId).ToList();
            List<GroupLCInfo> data = new List<GroupLCInfo>();
            foreach (var item in cOM_BBLC_Masters)
            {
                GroupLCInfo asdf = new GroupLCInfo();
                asdf.GroupLCId = item.GroupLCId;
                asdf.GroupLCRef = item.GroupLCRefName;
                asdf.BuyerName = item.BuyerInformation.BuyerName;
                asdf.Beneficiary = "Benificary";
                asdf.ContactRef = item.GroupLCRefName;
                asdf.LCDate = DateTime.Parse(item.FirstShipDate.ToString()).ToString("dd-MMM-yy");
                asdf.TotalValue = decimal.Parse(item.TotalGroupLCValueManual.ToString());

                data.Add(asdf);
            }


            //List<COM_GroupLC_Main> cOM_BBLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            //List<GroupLCInfo> data = new List<GroupLCInfo>();
            //foreach (var item in cOM_BBLC_Masters)
            //{
            //    GroupLCInfo asdf = new GroupLCInfo();
            //    asdf.GroupLCId = item.GroupLCId;
            //    asdf.GroupLCRef = item.GroupLCRefName;
            //    asdf.BuyerName = item.BuyerInformation.BuyerName;
            //    asdf.Beneficiary = "Benificary";
            //    asdf.ContactRef = item.GroupLCRefName;
            //    asdf.LCDate = (item.FirstShipDate.ToString());
            //    asdf.TotalValue = decimal.Parse(item.TotalGroupLCValue.ToString());

            //    data.Add(asdf);
            //}
            ViewBag.GroupLC = data;

            return View("Create", cOM_BBLC_Master);
        }


        // GET: COM_BBLC_Master/Delete/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_BBLC_Master cOM_BBLC_Master = db.COM_BBLC_Master.Find(id);
            if (cOM_BBLC_Master == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";

            //ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == cOM_BBLC_Master.SupplierId).ToList();

            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_BBLC_Master.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_BBLC_Master.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_BBLC_Master.CurrencyId);
            ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_BBLC_Master.DestinationID);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_BBLC_Master.PortOfLoadingId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_BBLC_Master.ShipModeId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_BBLC_Master.SupplierId);

            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_BBLC_Master.OpeningBankId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_BBLC_Master.LienBankId);
            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_BBLC_Master.TradeTermId);

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_BBLC_Master.PaymentTermsId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_BBLC_Master.DayListId);
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_BBLC_Master.ItemGroupId);


            ViewBag.PIInformation = (from proformainvoice in db.COM_ProformaInvoices.Where(m => m.comid.ToString() == AppData.intComId && m.ItemGroupId != null)
                                         //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                     where proformainvoice.SupplierId == cOM_BBLC_Master.SupplierId &&
                                     !db.COM_BBLC_Details.Any(f => f.PIId == proformainvoice.PIId)
                                     select proformainvoice).Distinct().ToList();

            //ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == cOM_BBLC_Master.GroupLCId).ToList();

            List<COM_GroupLC_Main> cOM_BBLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            List<GroupLCInfo> data = new List<GroupLCInfo>();
            foreach (var item in cOM_BBLC_Masters)
            {
                GroupLCInfo asdf = new GroupLCInfo();
                asdf.GroupLCId = item.GroupLCId;
                asdf.GroupLCRef = item.GroupLCRefName;
                asdf.BuyerName = item.BuyerInformation.BuyerName;
                asdf.Beneficiary = "Benificary";
                asdf.ContactRef = item.GroupLCRefName;
                asdf.LCDate = (item.FirstShipDate.ToString());
                asdf.TotalValue = decimal.Parse(item.TotalGroupLCValueManual.ToString());

                data.Add(asdf);
            }
            ViewBag.GroupLC = data;

            return View("Create", cOM_BBLC_Master);
        }


        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        // POST: COM_BBLC_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {


                IQueryable<COM_BBLC_Details> grouplcsub = db.COM_BBLC_Details.Where(p => p.BBLCId == id);

                db.COM_BBLC_Details.RemoveRange(grouplcsub);

                //foreach (COM_BBLC_Details ss in grouplcsub)
                //{
                //    db.COM_BBLC_Details.Remove(ss);
                //}

                COM_BBLC_Master COM_BBLC_Master = db.COM_BBLC_Master.Find(id);
                db.COM_BBLC_Master.Remove(COM_BBLC_Master);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = COM_BBLC_Master.BBLCId, ex = "" });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintBBLCOpen(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptOpenBBLBank.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptBBLCOpen_Import] '" + HttpContext.Session.GetString("MultiSelectData") + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "' ,  '" + AppData.Percentage + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptOpenBBLBank.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptBBLCOpen_Import] '" + HttpContext.Session.GetString("MultiSelectData") + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "' ,  '" + AppData.Percentage + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                string filename = db.COM_GroupLC_Mains.Where(x => x.GroupLCId == id).Select(x => x.GroupLCRefName + "_" + AppData.RefNo + "_CollectionOfProceeds").Single();

                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", "").Replace("\t", ""));
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", "").Replace("\t", "")); 

                //clsReport.strReportPathMain = Session["ReportPath"].ToString();
                //clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintCOP(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptCollectionOfProceeds.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);


                HttpContext.Session.SetString("ReportPath", "~/ Report / CommercialReport / rptCollectionOfProceeds.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_CollectionOfProceeds").Single();
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));



                //clsReport.strReportPathMain = Session["ReportPath"].ToString();
                //clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintFBOE(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptBillOfExchange1ST.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptBillOfExchange1ST.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_BillOfExchange").Single();
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));

                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));

                //clsReport.strReportPathMain = Session["ReportPath"].ToString();
                //clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintSBOE(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptBillOfExchange2ND.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptBillOfExchange2ND.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_BillOfExchange2nd").Single();
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));



                //clsReport.strReportPathMain = Session["ReportPath"].ToString();
                //clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintCI(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptDocumentsCommercialInvoice.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);

                HttpContext.Session.SetString("ReportPath", "~/ Report / CommercialReport / rptDocumentsCommercialInvoice.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);



                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_CommercialInvoice").Single();
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));

                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));


                //clsReport.strReportPathMain = Session["ReportPath"].ToString();
                //clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintPL(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptPackingList.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptPackingList.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_PackingList").Single();
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));


                //clsReport.strReportPathMain = Session["ReportPath"].ToString();
                //clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintBC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                //HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptBenificiaryCertificate.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                //HttpContext.Session.SetString("DataSourceName", "DataSet1");

                //HttpContext.Session.SetObject("rptList", postData);

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptBenificiaryCertificate.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_BenificiaryCertificate").Single();
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));


                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintDC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptDeliveryChallan.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_DeliveryChallan").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));


                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintTC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptTruckChallan.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "' , '" + AppData.Percentage + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo+"_TruckChallan").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));


                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintFTT(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptFTTissuance.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomationForPI] '" + HttpContext.Session.GetString("MultiSelectData") + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "' , '" + AppData.Percentage + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_GroupLC_Mains.Where(x => x.GroupLCId == id).Select(x => x.GroupLCRefName + "_" + AppData.RefNo + "_FTT").FirstOrDefault();
                if (filename == null)
                {
                    filename = "FTT";
                }
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", "").Replace("\t", "")) ;

               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintFDD(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptFDDissuance.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomationForPI] '"+HttpContext.Session.GetString("MultiSelectData")+ "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "' , '" + AppData.Percentage + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");  

                 HttpContext.Session.SetObject("rptList", postData);

                string filename = db.COM_GroupLC_Mains.Where(x => x.GroupLCId == id).Select(x => x.GroupLCRefName + "_" + AppData.RefNo + "_FTT").FirstOrDefault();
                if (filename == null)
                {
                    filename = "FTT";
                }
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", "").Replace("\t", ""));


               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintBILLDISCOUNTING(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptBILLDISCOUNTING.rdlc");
                //HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + Session["MultiSelectData"] + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "' , '" + AppData.Percentage + "'");

                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "' , '" + AppData.Percentage + "'");

                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = "BillDiscounting";// db.COM_GroupLC_Mains.Where(x => x.GroupLCId == id).Select(x => x.GroupLCRefName + "_" + AppData.RefNo + "_BillDiscounting").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", "").Replace("\t", ""));

               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintAllReport(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptAllReport.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "' , '" + AppData.Percentage + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "AllDocuments_BBLC").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));

                //AppData.intHasSubReport = 1;
                HttpContext.Session.SetObject("rptList", postData);


               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintCOO(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptCertificateOfOrigin.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_CertificateOfOrigin").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));


               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintPSIC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptPreShipmentInspecitonCertificate.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_BBLC_Master.Where(x => x.BBLCId == id).Select(x => x.BBLCNo + "_" + AppData.RefNo + "_PreShipmentInspectionCertificate").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace("\"", "").Replace(",", ""));

               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost, ActionName("SetSession")]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSession(string reporttype, string action, int? reportid, string refno, string printdate, string truckno, string drivername, string drivermobileno, int? grouplcid, string multipleproformainvoice, string Percentage)
        {
            try
            {
                Session["ReportType"] = reporttype;
                Session["MultiSelectData"] = multipleproformainvoice;
                Session["GroupLCId"] = grouplcid;




                AppData.RefNo = refno;
                AppData.PrintDate = printdate;
                AppData.Percentage = Percentage;


                AppData.TruckNo = truckno;
                AppData.DriverName = drivername;
                AppData.DriverMobileNo = drivermobileno;

                //return Json(new { Success = 1, TermsId = param, ex = "" });

                var redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintBBLCOpen")
                {
                    redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { id = grouplcid }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                }
                else
                {
                    //var vals = reportid.Split(',')[0];
                    redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { id = reportid }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }

                }


                return Json(new { Url = redirectUrl });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            //return RedirectToAction("Index");

        }


        [HttpPost, ActionName("SetSessionBBLCReport")]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionBBLCReport(string reporttype, string action, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                Session["ReportType"] = reporttype;
                //Session["MultiSelectData"] = multipleproformainvoice;
                //Session["GroupLCId"] = grouplcid;


                //AppData.RefNo = refno;
                //AppData.PrintDate = printdate;

                //AppData.TruckNo = truckno;
                //AppData.DriverName = drivername;
                //AppData.DriverMobileNo = drivermobileno;

                //return Json(new { Success = 1, TermsId = param, ex = "" });

                var redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintBBLCReport")
                {
                    redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                }
                else
                {
                    //var vals = reportid.Split(',')[0];
                    redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }

                }


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





        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        ////[OverridableAuthorize]
        public ActionResult PrintBBLCReport(int? id, string type, DateTime FromDate, DateTime ToDate)
        {
            try
            {


                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath","~/Report/ImportReport/rptBBLCReport.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptBBLCReport] " + AppData.intComId + " , '" + FromDate + "' ,'" + ToDate + "' ");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

               clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

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

