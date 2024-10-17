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


namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ExportRealization_MasterController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;

        public ExportRealization_MasterController()
        {
            //UserLog = new //UserLog();
        }

        // GET: ExportRealization_Masters
        public ActionResult Index()
        {
            var exportRealizationMasters = db.ExportRealization_Masters.ToList();//db.ExportRealization_Masters.Include(e => e.BuyerInformation).Include(e => e.COM_MasterLC).Include(e => e.SupplierInformation);
            return View(exportRealizationMasters.ToList());
        }

        // GET: ExportRealization_Masters/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //ExportRealization_Master exportRealizationMaster = db.ExportRealization_Masters.Find(id);
            //if (exportRealizationMaster == null)
            //{
            //    return NotFound();
            //}
            //return View(exportRealizationMaster);
            return View();
        }

        // GET: ExportRealization_Masters/Create
        public ActionResult Create(int? MasterLCId)
        {
            ViewBag.Title = "Create";
            ViewBag.MasterLCIDNo = MasterLCId;
            if (MasterLCId == null)
            {
                MasterLCId = 0;
            }




            if (MasterLCId > 0)
            {
                COM_MasterLC testMasterLC = new COM_MasterLC();
                testMasterLC = db.COM_MasterLCs.Where(m => m.MasterLCID == MasterLCId).FirstOrDefault();

                ExportRealization_Master ExportRealization_Masterss = new ExportRealization_Master();


                ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", testMasterLC.BuyerGroupID);
                ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", testMasterLC.MasterLCID);

                List<ExportOrderDetailsModel> ExportRealizationDetailsInformationss = (db.Database.SqlQuery<ExportOrderDetailsModel>("[ExportRelaizationForInvoice]  @comid, @userid,@MasterLCId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("MasterLCId", MasterLCId))).ToList();
                //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                ViewBag.ExportInvoiceDetailsInformation = ExportRealizationDetailsInformationss;

                return View(ExportRealization_Masterss);
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");

            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo");
            List<ExportOrderDetailsModel> ExportRealizationDetailsInformations = (db.Database.SqlQuery<ExportOrderDetailsModel>("[ExportRelaizationForInvoice]  @comid, @userid,@MasterLCId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("MasterLCId", MasterLCId))).ToList();
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ExportInvoiceDetailsInformation = ExportRealizationDetailsInformations;

            return View();
        }

        public class ExportOrderDetailsModel
        {
            //public int MasterLCDetailsID { get; set; }

            public int InvoiceId { get; set; }
            public int ExportInvoiceDetailsId { get; set; }

            public string BuyerLCRef { get; set; }

            public string InvoiceNo { get; set; }

            public Nullable<System.DateTime> InvoiceDate { get; set; }
            public string ExpNo { get; set; }

            public string BuyerBank { get; set; }
            public string CommercialCompany { get; set; }
            public string BuyerName { get; set; }
            public string BLNo { get; set; }

            public Nullable<System.DateTime> BLDate { get; set; }

            public string DestinationName { get; set; }
            public string TradeTermName { get; set; }
            public string PaymentTermsName { get; set; }

            public string DayListName { get; set; }
            public int TotalInvoiceQty { get; set; }

            public decimal TotalValue { get; set; }

        }

        // POST: ExportRealization_Masters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(ExportRealization_Master ExportRealizationMaster)
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

                if (ModelState.IsValid)
                {
                    if (ExportRealizationMaster.RealizationId > 0)
                    {
                        if (ExportRealizationMaster.DateAdded == null)
                        {
                            ExportRealizationMaster.DateAdded = DateTime.Now;
                        }

                        ExportRealizationMaster.DateUpdated = DateTime.Now;
                        ExportRealizationMaster.comid = int.Parse(AppData.intComId);

                        if (ExportRealizationMaster.userid == null)
                        {
                            ExportRealizationMaster.userid = HttpContext.Session.GetString("userid");
                        }

                        foreach (var item in ExportRealizationMaster.ExportRealization_Details)
                        {
                            if (item.ExportRealizationDetailsId > 0)
                            {
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.ExportRealization_Detailss.Add(item);
                                db.SaveChanges();
                            }
                        }

                        List<ExportRealization_Details> dbExportRealizationDdetails = db.ExportRealization_Detailss.Where(x => x.RealizationId == ExportRealizationMaster.RealizationId).ToList();
                        //List<ExportInvoiceDetails> newlyaddedlist = new List<ExportInvoiceDetails>();
                        foreach (var item1 in ExportRealizationMaster.ExportRealization_Details)
                        {
                            foreach (var item2 in dbExportRealizationDdetails)
                            {
                                if (item1 == item2)
                                {
                                    item2.isDelete = true;
                                }
                            }

                        }
                        db.ExportRealization_Detailss.RemoveRange(dbExportRealizationDdetails.Where(x => x.isDelete != true));
                        db.Entry(ExportRealizationMaster).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ExportRealizationMaster.DateAdded = DateTime.Now;
                        ExportRealizationMaster.DateUpdated = DateTime.Now;
                        ExportRealizationMaster.comid = int.Parse(AppData.intComId);
                        ExportRealizationMaster.userid = HttpContext.Session.GetString("userid");

                        //db.ExportRealization_Master .Add(ExportRealizationMaster);
                        db.ExportRealization_Masters.Add(ExportRealizationMaster);
                        db.SaveChanges();
                    }
                    return Json(new { Success = 1, RealizationId = ExportRealizationMaster.RealizationId, ex = "Dave Save Successfully" });

                    // return RedirectToAction("Index");
                }
                else
                {
                    return Json(new { Success = 0, RealizationId = ExportRealizationMaster.RealizationId, ex = "Unable to Save / Update" });

                }





                //return View(exportInvoiceMaster);

            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message
                    //ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        // GET: ExportRealization_Masters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportRealization_Master exportRealizationMaster = db.ExportRealization_Masters.Find(id);
            if (exportRealizationMaster == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", exportRealizationMaster.COM_MasterLC.BuyerID);
            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", exportRealizationMaster.MasterLCID);

            List<ExportOrderDetailsModel> ExportRealizationDetailsInformationss = (db.Database.SqlQuery<ExportOrderDetailsModel>("[ExportRelaizationForInvoice]  @comid, @userid,@MasterLCId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("MasterLCId", exportRealizationMaster.MasterLCID))).ToList();
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ExportInvoiceDetailsInformation = ExportRealizationDetailsInformationss;
            return View("Create", exportRealizationMaster);
        }

        // POST: ExportRealization_Masters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "RealizationId,RealizationNo,RealizationDate,MasterLCId,SupplierId,BuyerId,TotalExportInvoice,TotalOrderQty,TotalValue,RealizedAmount,PendingValue,Addedby,Dateadded,Updatedby,Dateupdated,comid,userid")] ExportRealization_Master exportRealizationMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exportRealizationMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", exportRealizationMaster.BuyerId);
            //ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", exportRealizationMaster.MasterLCId);
            //ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactID", "SupplierName", exportRealizationMaster.SupplierId);
            return View(exportRealizationMaster);
        }

        // GET: ExportRealization_Masters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ExportRealization_Master exportRealizationMaster = db.ExportRealization_Masters.Find(id);
            if (exportRealizationMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            //ViewBag.ExportDetails = db.ExportInvoiceDetailss.Where(m => m.ExportInvoiceMasters.MasterLCId == 0).ToList();

            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", exportRealizationMaster.COM_MasterLC.BuyerID);
            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo", exportRealizationMaster.MasterLCID);
            return View("Create", exportRealizationMaster);
        }

        // POST: ExportRealization_Masters/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {


                ExportRealization_Master exportRealizationMaster = db.ExportRealization_Masters.Where(z => z.RealizationId == id).FirstOrDefault();
                List<ExportRealization_Details> dbExportRealizationDdetails = db.ExportRealization_Detailss.Where(x => x.RealizationId == exportRealizationMaster.RealizationId).ToList();

                db.ExportRealization_Detailss.RemoveRange(dbExportRealizationDdetails);
                db.ExportRealization_Masters.Remove(exportRealizationMaster);

                TempData["Message"] = "Data Delete Successfully";

                
                db.SaveChanges();
                TempData["Status"] = "3";

                //return RedirectToAction("Index");
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), exportRealizationMaster.RealizationId.ToString(), "Delete");


                return Json(new { Success = 1, TermsId = exportRealizationMaster.RealizationId, ex = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message.ToString()
                });
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
