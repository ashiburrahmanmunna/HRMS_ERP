using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class COM_DocumentAcceptance_MasterController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: COM_DocumentAcceptance_Master
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Index()
        {
            var COM_DocumentAcceptance_Master = db.COM_DocumentAcceptance_Masters;
            return View(COM_DocumentAcceptance_Master.ToList());
        }

        // GET: COM_DocumentAcceptance_Master/Details/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_DocumentAcceptance_Master COM_DocumentAcceptance_Master = db.COM_DocumentAcceptance_Masters.Find(id);
            if (COM_DocumentAcceptance_Master == null)
            {
                return NotFound();
            }
            return View(COM_DocumentAcceptance_Master);
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult GetCIByBBLCId(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.GroupLCId = id;
            //List<COM_DocumentAcceptance_Master> asdf = db.COM_DocumentAcceptance_Masters.Where(p => (p.GroupLCId.ToString() == id.ToString())).ToList();
            List<COM_CommercialInvoice> COM_CommercialInvoice = db.COM_CommercialInvoices.Where(m => m.BBLCId == id).ToList();

            List<CIInfoModeltest> data = new List<CIInfoModeltest>();
            foreach (var item in COM_CommercialInvoice)
            {
                CIInfoModeltest ciinfomodel = new CIInfoModeltest();
                ciinfomodel.CommercialInvoiceId = item.CommercialInvoiceId;
                ciinfomodel.CommercialInvoiceNo = item.CommercialInvoiceNo;
                ciinfomodel.ConversionRate = item.ConversionRate.ToString();
                ciinfomodel.CurCode = item.Currency.CurCode;
                ciinfomodel.ItemGroupName = item.ItemGroupName;
                ciinfomodel.ItemDescription = item.ItemDescription;
                ciinfomodel.UnitMasterId = item.UnitMasterId;
                ciinfomodel.Quantity = item.Quantity;
                ciinfomodel.CommercialInvoiceDate = DateTime.Parse(item.CommercialInvoiceDate.ToString()).ToString("dd-MMM-yy");
                ciinfomodel.TotalValue = decimal.Parse(item.TotalValue.ToString());

                data.Add(ciinfomodel);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class CIInfoModeltest
        {
            /// </summary>
            public long CommercialInvoiceId { get; set; }
            public string CommercialInvoiceNo { get; set; }
            public string ConversionRate { get; set; }
            public string CurCode { get; set; }
            public string ItemGroupName { get; set; }
            public string ItemDescription { get; set; }
            public string UnitMasterId { get; set; }
            public decimal? Quantity { get; set; }
            public string CommercialInvoiceDate { get; set; }
            public decimal TotalValue { get; set; }
        }

        // GET: COM_DocumentAcceptance_Master/Create
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Create(int? SupplierId)
        {

            ViewBag.Title = "Create";
            ViewBag.SupplierId = SupplierId;
            if (SupplierId == null)
            {
                SupplierId = 0;
            }


            ViewBag.CIInformation = db.COM_CommercialInvoices.Where(m=>m.SupplierID == SupplierId).ToList();


            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo");
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo");
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");

            return View();
        }

        // POST: COM_DocumentAcceptance_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(COM_DocumentAcceptance_Master COM_DocumentAcceptance_Master)
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

                    if (COM_DocumentAcceptance_Master.BillsOfExchangeId > 0)
                    {
                        COM_DocumentAcceptance_Master.DateUpdated = DateTime.Now;
                        COM_DocumentAcceptance_Master.DateAdded = DateTime.Now;
                        COM_DocumentAcceptance_Master.comid = int.Parse(Session["comid"].ToString());
                        COM_DocumentAcceptance_Master.userid = (HttpContext.Session.GetString("userid"));

                        IQueryable<COM_DocumentAcceptance_Details> grouplcsub = db.COM_DocumentAcceptance_Details.Where(p => p.BillsOfExchangeId == COM_DocumentAcceptance_Master.BillsOfExchangeId);

                        foreach (COM_DocumentAcceptance_Details ss in grouplcsub)
                        {
                            db.COM_DocumentAcceptance_Details.Remove(ss);
                        }
                        db.SaveChanges();

                        //List<COM_DocumentAcceptance_Details> COM_DocumentAcceptance_DetailsS = COM_DocumentAcceptance_Master.COM_DocumentAcceptance_Details.ToList();
                        //db.COM_DocumentAcceptance_Details.RemoveRange(COM_DocumentAcceptance_DetailsS);


                        if (COM_DocumentAcceptance_Master.COM_DocumentAcceptance_Details != null)
                        {
                            foreach (COM_DocumentAcceptance_Details ss in COM_DocumentAcceptance_Master.COM_DocumentAcceptance_Details)
                            {


                                ss.DateAdded = DateTime.Now;
                                ss.DateUpdated = DateTime.Now;

                                //if (ss.CommercialInvoiceId > 0)
                                //{
                                //    db.Entry(COM_DocumentAcceptance_Master.COM_DocumentAcceptance_Details).State =  EntityState.Modified;
                                //}
                                //else
                                //{
                                db.COM_DocumentAcceptance_Details.Add(ss);

                                //}


                                //db.VoucherSubs.Add(ss);
                  



                            }
                            db.SaveChanges();
                        }
                        db.Entry(COM_DocumentAcceptance_Master).State = EntityState.Modified;

                    }
                    else
                    {
                        COM_DocumentAcceptance_Master.DateAdded = DateTime.Now;
                        COM_DocumentAcceptance_Master.DateUpdated = DateTime.Now;
                        COM_DocumentAcceptance_Master.comid = int.Parse(Session["comid"].ToString());
                        COM_DocumentAcceptance_Master.userid = (HttpContext.Session.GetString("userid"));

                        //COM_DocumentAcceptance_Master.userid = HttpContext.Session.GetString("userid");

                        foreach (COM_DocumentAcceptance_Details ss in COM_DocumentAcceptance_Master.COM_DocumentAcceptance_Details)
                        {

                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            //db.COM_DocumentAcceptance_MasterExports.Add(ss);



                        }

                        db.COM_DocumentAcceptance_Masters.Add(COM_DocumentAcceptance_Master);
                    }

                    db.SaveChanges();


                    //return RedirectToAction("Index");
                }
                return Json(new { Success = 1, BillsOfExchangeId = COM_DocumentAcceptance_Master.BillsOfExchangeId, ex = "" });
            

                //ViewBag.GroupLCId = new SelectList(db.COM_DocumentAcceptance_Masters, "GroupLCId", "GroupLCRefName", COM_DocumentAcceptance_Master.GroupLCId);
                //ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", COM_DocumentAcceptance_Master.CommercialCompanyId);
                //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", COM_DocumentAcceptance_Master.CurrencyId);
                //ViewBag.DestinationID = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", COM_DocumentAcceptance_Master.DestinationID);
                //ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", COM_DocumentAcceptance_Master.PortOfLoadingId);
                //ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", COM_DocumentAcceptance_Master.ShipModeId);
                //ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", COM_DocumentAcceptance_Master.SupplierId);
                //return View(COM_DocumentAcceptance_Master);
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


        // GET: COM_DocumentAcceptance_Master/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_DocumentAcceptance_Master COM_DocumentAcceptance_Master = db.COM_DocumentAcceptance_Masters.Find(id);
            if (COM_DocumentAcceptance_Master == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";



            //ViewBag.PIInformation = db.COM_commercialinvoices.Where(m => m.SupplierId == COM_DocumentAcceptance_Master.SupplierId).ToList();

            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", COM_DocumentAcceptance_Master.GroupLCId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName" , COM_DocumentAcceptance_Master.BuyerId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo" , COM_DocumentAcceptance_Master.BBLCId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName",COM_DocumentAcceptance_Master.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", COM_DocumentAcceptance_Master.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", COM_DocumentAcceptance_Master.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", COM_DocumentAcceptance_Master.SupplierId);


            ViewBag.CIInformation = (from commercialinvoice in db.COM_CommercialInvoices.Where(m => m.comid.ToString() == AppData.intComId)
                                    //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                where commercialinvoice.SupplierID == COM_DocumentAcceptance_Master.SupplierId && commercialinvoice.BBLCId == COM_DocumentAcceptance_Master.BBLCId &&
                                !db.COM_DocumentAcceptance_Details.Any(f => f.CommercialInvoiceId == commercialinvoice.CommercialInvoiceId)
                                select commercialinvoice).Distinct().ToList();




            //ViewBag.GroupLC = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == COM_DocumentAcceptance_Master.GroupLCId).ToList();

            //List<COM_GroupLC_Main> COM_DocumentAcceptance_Masters = db.COM_GroupLC_Mains.Where(m => m.GroupLCId == id).ToList();
            //List<CIInfoModeltest> data = new List<CIInfoModeltest>();
            //foreach (var item in COM_DocumentAcceptance_Masters)
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


            return View("Create", COM_DocumentAcceptance_Master);

        }



        // GET: COM_DocumentAcceptance_Master/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_DocumentAcceptance_Master COM_DocumentAcceptance_Master = db.COM_DocumentAcceptance_Masters.Find(id);
            if (COM_DocumentAcceptance_Master == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";



            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", COM_DocumentAcceptance_Master.GroupLCId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", COM_DocumentAcceptance_Master.BuyerId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", COM_DocumentAcceptance_Master.BBLCId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", COM_DocumentAcceptance_Master.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", COM_DocumentAcceptance_Master.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", COM_DocumentAcceptance_Master.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", COM_DocumentAcceptance_Master.SupplierId);


            ViewBag.CIInformation = (from commercialinvoice in db.COM_CommercialInvoices.Where(m => m.comid.ToString() == AppData.intComId)
                                         //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                     where commercialinvoice.SupplierID == COM_DocumentAcceptance_Master.SupplierId && commercialinvoice.BBLCId == COM_DocumentAcceptance_Master.BBLCId &&
                                     !db.COM_DocumentAcceptance_Details.Any(f => f.CommercialInvoiceId == commercialinvoice.CommercialInvoiceId)
                                     select commercialinvoice).Distinct().ToList();




            return View("Create", COM_DocumentAcceptance_Master);
        }

        // POST: COM_DocumentAcceptance_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                COM_DocumentAcceptance_Master COM_DocumentAcceptance_Master = db.COM_DocumentAcceptance_Masters.Find(id);

                List<COM_DocumentAcceptance_Details> COM_DocumentAcceptance_DetailsS = COM_DocumentAcceptance_Master.COM_DocumentAcceptance_Details.Where(m=>m.BillsOfExchangeId == id).ToList();
                db.COM_DocumentAcceptance_Details.RemoveRange(COM_DocumentAcceptance_DetailsS);
                db.COM_DocumentAcceptance_Masters.Remove(COM_DocumentAcceptance_Master);
                db.SaveChanges();

                return Json(new { Success = 1, BillsOfExchangeId = COM_DocumentAcceptance_Master.BillsOfExchangeId, ex = "" });

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
        public JsonResult CIInfo(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.GroupLCId = id;
            //List<cOM_BBLC_Master> asdf = db.cOM_BBLC_Masters.Where(p => (p.GroupLCId.ToString() == id.ToString())).ToList();
            //COM_CommercialInvoice COM_CommercialInvoices = db.COM_CommercialInvoices.Where(m => m.CommercialInvoiceId == id).FirstOrDefault();

            COM_BBLC_Master COM_BBLC_Master = db.COM_BBLC_Master.Where(m => m.BBLCId == id).FirstOrDefault();
            //var result = db.COM_DocumentAcceptance_Masters.Include(COM_DocumentAcceptance_Details).Where(xyz => xyz.CommercialCompanyId == id).GroupBy(o => o.CommercialInvoiceId).Select(g => new { membername = g.Key, total = g.Sum(i => i.PayableAmount) }); ;


            //var result = db.pruchasemasters.GroupBy(o => o.membername)
            //       .Select(g => new { membername = g.Key, total = g.Sum(i => i.cost) });

            //foreach (var group in result)
            //{
            //    Console.WriteLine("Membername = {0} Totalcost={1}", group.membername, group.total);
            //}

            CommercialInvoiceInfo data = new CommercialInvoiceInfo();

            data.CommercialCompanyId = int.Parse(COM_BBLC_Master.CommercialCompanyId.ToString());
            data.SupplierId = COM_BBLC_Master.SupplierId;
            data.BBLCId = COM_BBLC_Master.BBLCId;
            data.GroupLCId = COM_BBLC_Master.GroupLCId;
            data.CurrencyId = COM_BBLC_Master.CurrencyId;
            data.ConversionRate = COM_BBLC_Master.ConvertRate.ToString();
          

            var acceptedamount = 200;


            data.TotalBBLCAmount = COM_BBLC_Master.TotalValue;
            data.AcceptedAmount = acceptedamount;
            data.PayableAmount = COM_BBLC_Master.TotalValue - acceptedamount;
            data.NewCIAmount = 400;



            var x = db.COM_GroupLC_Subs.Where(y => y.GroupLCId == COM_BBLC_Master.GroupLCId).ToList();

            string jointext = "";
            foreach (var item in x)
            {
                jointext = jointext.ToString() + item.COM_MasterLC.BuyerLCRef + ",";
                data.BuyerId = item.COM_MasterLC.BuyerID;
            }
            jointext = jointext.Substring(0, jointext.Length - 1);

            data.MasterLCRef = jointext;


            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class CommercialInvoiceInfo
        {

            /// </summary>
            public int CommercialCompanyId { get; set; }
            public int? SupplierId { get; set; }
            public int? BBLCId { get; set; }
            public int? GroupLCId { get; set; }
            public int? BuyerId { get; set; }

            public int CurrencyId { get; set; }
            public string ConversionRate { get; set; }
            public string MasterLCRef { get; set; }
            public string ItemGroup { get; set; }
            public string ItemDescription { get; set; }

            public decimal TotalBBLCAmount { get; set; }
            public decimal AcceptedAmount { get; set; }
            public decimal PayableAmount { get; set; }
            public decimal NewCIAmount { get; set; }

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
