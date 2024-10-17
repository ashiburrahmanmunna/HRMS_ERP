using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.Models;

namespace MasterDetail.Controllers
{
    public class COM_DocumentAcceptanceController : Controller
    {
        private MasterDetailContext db = new MasterDetailContext();

        // GET: COM_DocumentAcceptance
        public ActionResult Index()
        {
            var cOM_DocumentAcceptances = db.COM_DocumentAcceptances.ToList(); //.Include(c => c.BuyerInformation).Include(c => c.COM_BBLC_Master).Include(c => c.COM_CommercialInvoice).Include(c => c.COM_GroupLC_Main).Include(c => c.CommercialCompanys).Include(c => c.Currency).Include(c => c.SupplierInformations);
            return View(cOM_DocumentAcceptances.ToList());
        }

        // GET: COM_DocumentAcceptance/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_DocumentAcceptance cOM_DocumentAcceptance = db.COM_DocumentAcceptances.Find(id);
            if (cOM_DocumentAcceptance == null)
            {
                return NotFound();
            }
            return View(cOM_DocumentAcceptance);
        }

        // GET: COM_DocumentAcceptance/Create
        public ActionResult Create()
        {
            try
            {

  

            ViewBag.Title = "Create";

            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo");
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo");
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName");


     
            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName");


            return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // POST: COM_DocumentAcceptance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "BillsOfExchangeId,BillOfExchangeRef,BillDate,BillMaturityDate , BillPaymentDate,CommercialInvoiceId,CommercialCompanyId,BuyerId,SupplierId,BBLCId,GroupLCId,MasterLCRef,TotalBBLCAmount,CurrencyId,ConversionRate,PaidAmount,PayableAmount,AcceptedAmount,NewCIAmount,ApprovalPerson,ApprovedById,DateApproval,Addedby,Dateadded,Updatedby,Dateupdated,comid,userid")] COM_DocumentAcceptance cOM_DocumentAcceptance)
        {


            try
            {
                ViewBag.Title = "Create";

                if (AppData.intComId == "0")
                {
                    return NotFound();

                }

                if (AppData.intComId == "0")
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }

                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });





                if (ModelState.IsValid)
                {

                    if (cOM_DocumentAcceptance.BillsOfExchangeId > 0)
                    {

                        db.Entry(cOM_DocumentAcceptance).State = EntityState.Modified;
                        cOM_DocumentAcceptance.Dateadded = DateTime.Now.Date;
                        cOM_DocumentAcceptance.Dateupdated = DateTime.Now.Date;
                        cOM_DocumentAcceptance.comid = int.Parse(AppData.intComId);
                        cOM_DocumentAcceptance.userid = HttpContext.Session.GetString("userid");

                        db.SaveChanges();
                    }


                    //message = "Weekend update succeded";

                    else
                    {
                        cOM_DocumentAcceptance.Dateadded = DateTime.Now.Date;
                        cOM_DocumentAcceptance.Dateupdated = DateTime.Now.Date;
                        cOM_DocumentAcceptance.comid = int.Parse(AppData.intComId);
                        cOM_DocumentAcceptance.userid = HttpContext.Session.GetString("userid");


                        db.COM_DocumentAcceptances.Add(cOM_DocumentAcceptance);
                        //db.WeekdaySectionWise.Add(item);
                        db.SaveChanges();


                    }

                    return RedirectToAction("Index");
                }

                ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_DocumentAcceptance.BuyerId);
                ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_DocumentAcceptance.BBLCId);
                ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_DocumentAcceptance.CommercialInvoiceId);
                ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_DocumentAcceptance.GroupLCId);
                ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_DocumentAcceptance.CommercialCompanyId);
                ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_DocumentAcceptance.CurrencyId);
                ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_DocumentAcceptance.SupplierId);

                ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", cOM_DocumentAcceptance.ApprovedById);


                return View(cOM_DocumentAcceptance);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: COM_DocumentAcceptance/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_DocumentAcceptance cOM_DocumentAcceptance = db.COM_DocumentAcceptances.Find(id);
            if (cOM_DocumentAcceptance == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_DocumentAcceptance.BuyerId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_DocumentAcceptance.BBLCId);
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_DocumentAcceptance.CommercialInvoiceId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_DocumentAcceptance.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_DocumentAcceptance.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_DocumentAcceptance.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_DocumentAcceptance.SupplierId);
            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", cOM_DocumentAcceptance.ApprovedById);
            return View("Create",cOM_DocumentAcceptance);
        }

        // POST: COM_DocumentAcceptance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "BillsOfExchangeId,BillOfExchangeRef,BillDate,BillMaturityDate , BillPaymentDate,CommercialInvoiceId,CommercialCompanyId,BuyerId,SupplierId,BBLCId,GroupLCId,MasterLCRef,TotalBBLCAmount,CurrencyId,ConversionRate,PaidAmount,PayableAmount,AcceptedAmount,NewCIAmount,ApprovalPerson,ApprovedById,DateApproval,Addedby,Dateadded,Updatedby,Dateupdated,comid,userid")] COM_DocumentAcceptance cOM_DocumentAcceptance)
        {
            try
            {

           
            if (ModelState.IsValid)
            {
                db.Entry(cOM_DocumentAcceptance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_DocumentAcceptance.BuyerId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_DocumentAcceptance.BBLCId);
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_DocumentAcceptance.CommercialInvoiceId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_DocumentAcceptance.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_DocumentAcceptance.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_DocumentAcceptance.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_DocumentAcceptance.SupplierId);
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_DocumentAcceptance.BuyerId);
            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", cOM_DocumentAcceptance.ApprovedById);
            return View(cOM_DocumentAcceptance);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: COM_DocumentAcceptance/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            COM_DocumentAcceptance cOM_DocumentAcceptance = db.COM_DocumentAcceptances.Find(id);

            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_DocumentAcceptance.BuyerId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_DocumentAcceptance.BBLCId);
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_DocumentAcceptance.CommercialInvoiceId);
            ViewBag.GroupLCId = new SelectList(db.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName", cOM_DocumentAcceptance.GroupLCId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_DocumentAcceptance.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", cOM_DocumentAcceptance.CurrencyId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", cOM_DocumentAcceptance.SupplierId);
            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", cOM_DocumentAcceptance.ApprovedById);

            if (cOM_DocumentAcceptance == null)
            {
                return NotFound();
            }
            return View("Create",cOM_DocumentAcceptance);
        }

        // POST: COM_DocumentAcceptance/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]c
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                COM_DocumentAcceptance COM_DocumentAcceptances = db.COM_DocumentAcceptances.Find(id);
                db.COM_DocumentAcceptances.Remove(COM_DocumentAcceptances);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = COM_DocumentAcceptances.BillsOfExchangeId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }


        }


        public JsonResult CIInfo(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.GroupLCId = id;
            //List<cOM_BBLC_Master> asdf = db.cOM_BBLC_Masters.Where(p => (p.GroupLCId.ToString() == id.ToString())).ToList();
            COM_CommercialInvoice COM_CommercialInvoices = db.COM_CommercialInvoices.Where(m => m.CommercialInvoiceId == id).FirstOrDefault();

            var result = db.COM_DocumentAcceptances.Where(xyz => xyz.CommercialCompanyId == id).GroupBy(o => o.CommercialInvoiceId).Select(g => new { membername = g.Key, total = g.Sum(i => i.PayableAmount) }); ;


            //var result = db.pruchasemasters.GroupBy(o => o.membername)
            //       .Select(g => new { membername = g.Key, total = g.Sum(i => i.cost) });

            //foreach (var group in result)
            //{
            //    Console.WriteLine("Membername = {0} Totalcost={1}", group.membername, group.total);
            //}

            CommercialInvoiceInfo data = new CommercialInvoiceInfo();

            data.CommercialCompanyId = COM_CommercialInvoices.CommercialCompanyID;
            data.SupplierId = COM_CommercialInvoices.SupplierID;
            data.BBLCId = COM_CommercialInvoices.BBLCId;
            data.GroupLCId = COM_CommercialInvoices.COM_BBLC_Master.GroupLCId;
            data.CurrencyId = COM_CommercialInvoices.CurrencyId;
            data.ConversionRate = COM_CommercialInvoices.ConversionRate.ToString();
            data.ItemGroup = COM_CommercialInvoices.ItemGroupName;
            data.ItemDescription = COM_CommercialInvoices.ItemDescription;

            var acceptedamount = 200;


            data.TotalBBLCAmount = COM_CommercialInvoices.COM_BBLC_Master.TotalValue;
            data.AcceptedAmount = acceptedamount;
            data.PayableAmount = COM_CommercialInvoices.COM_BBLC_Master.TotalValue - acceptedamount;
            data.NewCIAmount = 400;



            var x = db.COM_GroupLC_Subs.Where(y => y.GroupLCId == COM_CommercialInvoices.COM_BBLC_Master.GroupLCId).ToList();

            string jointext="";
            foreach (var item in x)
            {
                jointext = jointext.ToString()  + item.COM_MasterLC.BuyerLCRef + ",";
                data.BuyerId = item.COM_MasterLC.BuyerID;
            }
            jointext = jointext.Substring(0, jointext.Length - 1);

            data.MasterLCRef = jointext;


            //foreach (var item in cOM_BBLC_Masters)
            //{
            //    CommercialInvoiceInfo asdf = new CommercialInvoiceInfo();
            //    asdf.GroupLCId = item.GroupLCId;
            //    asdf.GroupLCRef = item.GroupLCRefName;
            //    asdf.BuyerName = item.BuyerInformation.BuyerName;
            //    asdf.Beneficiary = "Benificary";
            //    asdf.ContactRef = item.GroupLCRefName;
            //    asdf.LCDate = (item.FirstShipDate.ToString());
            //    asdf.TotalValue = decimal.Parse(item.TotalGroupLCValue.ToString());

            //    data.Add(asdf);
            //}

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
