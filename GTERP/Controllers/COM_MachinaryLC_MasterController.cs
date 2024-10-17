using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;
using GTCommercial.Models.Common;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class COM_MachinaryLC_MasterController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private string variablerefno;
        private string variableprintdate;


        // GET: COM_MachinaryLC_Master
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Index()
        {
            var cOM_MachinaryLC_Master = db.COM_MachinaryLC_Masters.Include(c => c.COM_MachinaryLC_Details).Include(c => c.Company).Include(c => c.Currency).Include(c => c.Destination).Include(c => c.PortOfLoading).Include(c => c.ShipMode).Include(c => c.SupplierInformation);
            return View(cOM_MachinaryLC_Master.ToList());
        }



        //GetMachinaryLCListByMasterMachinaryLCId
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public JsonResult GetMachinaryLCListByMasterMachinaryLCId(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.MasterLCID = id;
            //List<cOM_MachinaryLC_Master> asdf = db.cOM_MachinaryLC_Masters.Where(p => (p.MasterLCID.ToString() == id.ToString())).ToList();
            List<COM_MachinaryLC_Master> MachinaryLCMaster = db.COM_MachinaryLC_Masters.Where(m => m.MachinaryLCId == id).ToList();
            List<MachinaryLCListClass> data = new List<MachinaryLCListClass>();

            foreach (var item in MachinaryLCMaster)
            {
                MachinaryLCListClass asdf = new MachinaryLCListClass();
                //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                asdf.MachinaryLCNo = item.MachinaryLCNo;
                asdf.Importer = item.Company.CompanyName;
                asdf.Exporter = item.SupplierInformation.SupplierName; //DateTime.Parse(item.InvoiceDate.ToString()).ToString("dd-MMM-yy");
                asdf.PINumber = item.COM_MachinaryLC_Details.FirstOrDefault().COM_ProformaInvoice.PINo;
                //asdf.MasterContractNo = item.COM_GroupLC_Main.COM_GroupLC_Sub.FirstOrDefault().COM_MasterLC.BuyerLCRef;
                asdf.MachinaryLCValue = item.TotalValue;
                asdf.InvoiceNo = "";// item.COM_CommercialInvoice.FirstOrDefault().CommercialInvoiceNo;


                data.Add(asdf);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class MachinaryLCListClass
        {
            public string MachinaryLCNo { get; set; }

            /// </summary>
            public string Importer { get; set; }
            public string Exporter { get; set; }

            public string PINumber { get; set; }
            public string MasterContractNo { get; set; }

            public decimal? MachinaryLCValue { get; set; }
            public string InvoiceNo { get; set; }

        }



        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public ActionResult MachinaryLCReport()
        {
            ViewBag.ReportType = "PDF";
            ViewBag.MachinaryLCId = new SelectList(db.COM_MachinaryLC_Masters, "MachinaryLCId", "MachinaryLCNo");
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
            ViewBag.PIId = new SelectList(db.COM_ProformaInvoices.Where(m => m.GroupLCId == 0), "PIId", "PINo");

            var cOM_MachinaryLC_Master = db.COM_MachinaryLC_Masters;
            return View(cOM_MachinaryLC_Master.ToList());

        }

        public JsonResult getProformaInvoiceByGroupLC(int id)
        {
            List<COM_ProformaInvoice> COM_ProformaInvoices = db.COM_ProformaInvoices.Where(x => x.GroupLCId == id).ToList();

            List<SelectListItem> liproformainvoice = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (COM_ProformaInvoices != null)
            {
                foreach (COM_ProformaInvoice x in COM_ProformaInvoices)
                {
                    liproformainvoice.Add(new SelectListItem { Text = x.PINo, Value = x.PIId.ToString() });
                }
            }
            return Json(new SelectList(liproformainvoice, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public JsonResult GetGroupLCById(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.GroupLCId = id;
            //List<cOM_MachinaryLC_Master> asdf = db.cOM_MachinaryLC_Masters.Where(p => (p.GroupLCId.ToString() == id.ToString())).ToList();
            List<COM_GroupLC_Main> cOM_MachinaryLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            List<GroupLCInfo> data = new List<GroupLCInfo>();
            foreach (var item in cOM_MachinaryLC_Masters)
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

            return Json(data, JsonRequestBehavior.AllowGet);
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

        // GET: COM_MachinaryLC_Master/Create
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Create(int? SupplierId)
        {
            ViewBag.Title = "Create";
            ViewBag.SupplierId = SupplierId;
            if (SupplierId == null)
            {
                SupplierId = 0;
            }

            ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == 0).ToList();

            ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m=>m.SupplierId == SupplierId && (m.ItemGroups.ItemGroupName.ToUpper().Contains("MACHINERY") || m.ItemGroups.ItemGroupName.ToUpper().Contains("YARN") || m.ItemGroups.ItemGroupName.ToUpper().Contains("CHEMICAL"))).ToList();

            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch");
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName");
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName" , SupplierId);

            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName");
            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName");

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName");
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName");
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName");


            //COM_MachinaryLC_Master asdf = new COM_MachinaryLC_Master();
            //asdf.Balance = 0;
            //asdf.IncreaseValue = 0;
            //asdf.DecreaseValue = 0;
            //asdf.NetValue = 0;
            //asdf.MachinaryLCValue = 0;



            return View();
        }

        // POST: COM_MachinaryLC_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(COM_MachinaryLC_Master cOM_MachinaryLC_Master)
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

                    if (cOM_MachinaryLC_Master.MachinaryLCId > 0)
                    {
                        cOM_MachinaryLC_Master.DateUpdated = DateTime.Now;
                        //cOM_MachinaryLC_Master.DateAdded = DateTime.Now;
                        cOM_MachinaryLC_Master.comid = int.Parse(Session["comid"].ToString());
                        cOM_MachinaryLC_Master.useridUpdate = HttpContext.Session.GetString("userid");


                        if (cOM_MachinaryLC_Master.userid == null || cOM_MachinaryLC_Master.userid == "0")
                        {

                            cOM_MachinaryLC_Master.userid = HttpContext.Session.GetString("userid");

                        }
                        if (cOM_MachinaryLC_Master.comid == null || cOM_MachinaryLC_Master.comid == 0)
                        {

                            cOM_MachinaryLC_Master.comid = int.Parse(Session["comid"].ToString());

                        }

                        IQueryable<COM_MachinaryLC_Details> grouplcsub = db.COM_MachinaryLC_Detailss.Where(p => p.MachinaryLCId == cOM_MachinaryLC_Master.MachinaryLCId);

                        foreach (COM_MachinaryLC_Details ss in grouplcsub)
                        {
                            db.COM_MachinaryLC_Detailss.Remove(ss);
                        }

                        if (cOM_MachinaryLC_Master.COM_MachinaryLC_Details != null)
                        {
                            foreach (COM_MachinaryLC_Details ss in cOM_MachinaryLC_Master.COM_MachinaryLC_Details)
                            {
                                //ss.DateAdded = DateTime.Now;
                                //ss.DateUpdated = DateTime.Now;

                                //db.VoucherSubs.Add(ss);
                                db.COM_MachinaryLC_Detailss.Add(ss);
                            }
                        }
                        db.Entry(cOM_MachinaryLC_Master).State = EntityState.Modified;

                    }
                    else
                    {
                        cOM_MachinaryLC_Master.DateAdded = DateTime.Now;
                        cOM_MachinaryLC_Master.DateUpdated = DateTime.Now;
                        cOM_MachinaryLC_Master.comid = int.Parse(Session["comid"].ToString());
                        cOM_MachinaryLC_Master.userid = (HttpContext.Session.GetString("userid"));

                        //cOM_MachinaryLC_Master.userid = HttpContext.Session.GetString("userid");

                        foreach (COM_MachinaryLC_Details ss in cOM_MachinaryLC_Master.COM_MachinaryLC_Details)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            //db.cOM_MachinaryLC_MasterExports.Add(ss);
                        }

                        db.COM_MachinaryLC_Masters.Add(cOM_MachinaryLC_Master);
                    }

                    db.SaveChanges();                    
                    //return RedirectToAction("Index");
                }
                return Json(new { Success = 1, MachinaryLCId = cOM_MachinaryLC_Master.MachinaryLCId, ex = "" });            

                //ViewBag.GroupLCId = new SelectList(db.cOM_MachinaryLC_Masters, "GroupLCId", "GroupLCRefName", cOM_MachinaryLC_Master.GroupLCId);
                //ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_MachinaryLC_Master.CommercialCompanyId);
                //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_MachinaryLC_Master.CurrencyId);
                //ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_MachinaryLC_Master.DestinationID);
                //ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_MachinaryLC_Master.PortOfLoadingId);
                //ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_MachinaryLC_Master.ShipModeId);
                //ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_MachinaryLC_Master.SupplierId);
                //return View(cOM_MachinaryLC_Master);
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

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        // GET: COM_MachinaryLC_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_MachinaryLC_Master cOM_MachinaryLC_Master = db.COM_MachinaryLC_Masters.Find(id);
            if (cOM_MachinaryLC_Master == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            //ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == cOM_MachinaryLC_Master.SupplierId).ToList();

            //ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_MachinaryLC_Master.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_MachinaryLC_Master.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_MachinaryLC_Master.CurrencyId);
            ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_MachinaryLC_Master.DestinationID);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_MachinaryLC_Master.PortOfLoadingId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_MachinaryLC_Master.ShipModeId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_MachinaryLC_Master.SupplierId);

            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_MachinaryLC_Master.OpeningBankId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_MachinaryLC_Master.LienBankId);
            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_MachinaryLC_Master.TradeTermId);

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_MachinaryLC_Master.PaymentTermsId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_MachinaryLC_Master.DayListId);
            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName" , cOM_MachinaryLC_Master.ItemGroupId);


            //ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == SupplierId && (m.ItemGroups.ItemGroupName.ToUpper().Contains("MACHINERY") || m.ItemGroups.ItemGroupName.ToUpper().Contains("YARN") || m.ItemGroups.ItemGroupName.ToUpper().Contains("CHEMICAL"))).ToList();

            ViewBag.PIInformation = (from proformainvoice in db.COM_ProformaInvoices.Where(m => m.comid.ToString() == AppData.intComId && m.ItemGroupId != null)
                                    //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                where (proformainvoice.SupplierId == cOM_MachinaryLC_Master.SupplierId) &&
                                (proformainvoice.ItemGroups.ItemGroupName.ToUpper().Contains("MACHINERY") || proformainvoice.ItemGroups.ItemGroupName.ToUpper().Contains("YARN") || proformainvoice.ItemGroups.ItemGroupName.ToUpper().Contains("CHEMICAL")) &&
                                (!db.COM_MachinaryLC_Detailss.Any(f => f.PIId == proformainvoice.PIId))
                                select proformainvoice).Distinct().ToList();

                       
            //ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == cOM_MachinaryLC_Master.GroupLCId).ToList();

            //List<COM_GroupLC_Main> cOM_MachinaryLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == cOM_MachinaryLC_Master.GroupLCId).ToList();
            //List<GroupLCInfo> data = new List<GroupLCInfo>();
            //foreach (var item in cOM_MachinaryLC_Masters)
            //{
            //    GroupLCInfo asdf = new GroupLCInfo();
            //    asdf.GroupLCId = item.GroupLCId;
            //    asdf.GroupLCRef = item.GroupLCRefName;
            //    asdf.BuyerName = item.BuyerInformation.BuyerName;
            //    asdf.Beneficiary = "Benificary";
            //    asdf.ContactRef = item.GroupLCRefName;
            //    asdf.LCDate = DateTime.Parse(item.FirstShipDate.ToString()).ToString("dd-MMM-yy");
            //    asdf.TotalValue = decimal.Parse(item.TotalGroupLCValueManual.ToString());

            //    data.Add(asdf);
            //}


            //List<COM_GroupLC_Main> cOM_MachinaryLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            //List<GroupLCInfo> data = new List<GroupLCInfo>();
            //foreach (var item in cOM_MachinaryLC_Masters)
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
            //ViewBag.GroupLC = data;

            return View("Create", cOM_MachinaryLC_Master);
        }
        

        // GET: COM_MachinaryLC_Master/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_MachinaryLC_Master cOM_MachinaryLC_Master = db.COM_MachinaryLC_Masters.Find(id);
            if (cOM_MachinaryLC_Master == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
                       
            //ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == cOM_MachinaryLC_Master.SupplierId).ToList();

            //ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_MachinaryLC_Master.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_MachinaryLC_Master.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_MachinaryLC_Master.CurrencyId);
            ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", cOM_MachinaryLC_Master.DestinationID);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", cOM_MachinaryLC_Master.PortOfLoadingId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", cOM_MachinaryLC_Master.ShipModeId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_MachinaryLC_Master.SupplierId);

            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", cOM_MachinaryLC_Master.OpeningBankId);
            ViewBag.LienBankId = new SelectList(db.LienBanks, "LienBankId", "LienBankName", cOM_MachinaryLC_Master.LienBankId);
            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", cOM_MachinaryLC_Master.TradeTermId);

            ViewBag.PaymentTermsId = new SelectList(db.PaymentTerms, "PaymentTermsId", "PaymentTermsName", cOM_MachinaryLC_Master.PaymentTermsId);
            ViewBag.DayListId = new SelectList(db.DayLists, "DayListId", "DayListName", cOM_MachinaryLC_Master.DayListId);

            ViewBag.ItemGroupId = new SelectList(db.ItemGroups, "ItemGroupId", "ItemGroupName", cOM_MachinaryLC_Master.ItemGroupId);


            //ViewBag.PIInformation = db.COM_ProformaInvoices.Where(m => m.SupplierId == SupplierId && (m.ItemGroups.ItemGroupName.ToUpper().Contains("MACHINERY") || m.ItemGroups.ItemGroupName.ToUpper().Contains("YARN") || m.ItemGroups.ItemGroupName.ToUpper().Contains("CHEMICAL"))).ToList();

            ViewBag.PIInformation = (from proformainvoice in db.COM_ProformaInvoices.Where(m => m.comid.ToString() == AppData.intComId && m.ItemGroupId != null)
                                         //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                     where (proformainvoice.SupplierId == cOM_MachinaryLC_Master.SupplierId) &&
                                     (proformainvoice.ItemGroups.ItemGroupName.ToUpper().Contains("MACHINERY") || proformainvoice.ItemGroups.ItemGroupName.ToUpper().Contains("YARN") || proformainvoice.ItemGroups.ItemGroupName.ToUpper().Contains("CHEMICAL")) &&
                                     (!db.COM_MachinaryLC_Detailss.Any(f => f.PIId == proformainvoice.PIId))
                                     select proformainvoice).Distinct().ToList();

            //ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == cOM_MachinaryLC_Master.GroupLCId).ToList();

            List<COM_GroupLC_Main> cOM_MachinaryLC_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            List<GroupLCInfo> data = new List<GroupLCInfo>();
            foreach (var item in cOM_MachinaryLC_Masters)
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
            
            return View("Create", cOM_MachinaryLC_Master);
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        // POST: COM_MachinaryLC_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {


                IQueryable<COM_MachinaryLC_Details> grouplcsub = db.COM_MachinaryLC_Detailss.Where(p => p.MachinaryLCId == id);

                db.COM_MachinaryLC_Detailss.RemoveRange(grouplcsub);

                //foreach (COM_MachinaryLC_Details ss in grouplcsub)
                //{
                //    db.COM_MachinaryLC_Details.Remove(ss);
                //}

                COM_MachinaryLC_Master COM_MachinaryLC_Master = db.COM_MachinaryLC_Masters.Find(id);
                db.COM_MachinaryLC_Masters.Remove(COM_MachinaryLC_Master);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = COM_MachinaryLC_Master.MachinaryLCId, ex = "" });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintMachinaryLCOpen(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptOpenBBLBank.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptMachinaryLCOpen_Import] '" + Session["MultiSelectData"].ToString() + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCOP(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptCollectionOfProceeds.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintFBOE(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptBillOfExchange1ST.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintSBOE(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptBillOfExchange2ND.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
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
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptDocumentsCommercialInvoice.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintPL(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptPackingList.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintBC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptBenificiaryCertificate.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintDC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptDeliveryChallan.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintTC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptTruckChallan.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "','" + AppData.TruckNo + "','" + AppData.DriverName + "','" + AppData.DriverMobileNo + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCOO(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptCertificateOfOrigin.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintPSIC(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptPreShipmentInspecitonCertificate.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptImportDocumentsAutomation] '" + id + "','" + AppData.intComId + "','" + AppData.RefNo + "','" + AppData.PrintDate + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost, ActionName("SetSession")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSession(string reporttype , string action , int? reportid, string refno , string printdate, string truckno, string drivername, string drivermobileno, int? grouplcid, string multipleproformainvoice )
        {
            try
            {
                Session["ReportType"] = reporttype;
                Session["MultiSelectData"] = multipleproformainvoice;
                Session["GroupLCId"] = grouplcid;


                AppData.RefNo = refno;
                AppData.PrintDate = printdate;

                AppData.TruckNo = truckno;
                AppData.DriverName = drivername;
                AppData.DriverMobileNo = drivermobileno;

                //return Json(new { Success = 1, TermsId = param, ex = "" });

                var redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintMachinaryLCOpen")
                {
                    redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_MachinaryLC_Master", new { id = grouplcid }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                }
                else
                {
                    //var vals = reportid.Split(',')[0];
                    redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_MachinaryLC_Master", new { id = reportid }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }

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
