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
    public class COM_CNFBillImportMasterController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();

        // GET: COM_CNFBillImportMaster
        public ActionResult Index()
        {
            var cOM_CNFBillImportMasters = db.COM_CNFBillImportMasters.Include(c => c.BuyerInformations).Include(c => c.Companies);
            return View(cOM_CNFBillImportMasters.ToList());
        }

        // GET: COM_CNFBillImportMaster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_CNFBillImportMaster cOM_CNFBillImportMaster = db.COM_CNFBillImportMasters.Find(id);
            if (cOM_CNFBillImportMaster == null)
            {
                return NotFound();
            }
            return View(cOM_CNFBillImportMaster);
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        // GET: COM_CNFBillImportMaster/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

                var Import = db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Import" && m.comid.ToString() == AppData.intComId);

                var Importmaster = new COM_CNFBillImportMaster { CNFEXpenseBillDate = DateTime.Now.Date, CommercialCompanyId = 1, BuyerID = 1, JobNo = "" };
                Importmaster.COM_CNFBillImportDetails = new List<COM_CNFBillImportDetails>();
                foreach (var item in Import)
                {
                    COM_CNFBillImportDetails COM_CNFBillImportDetail = new COM_CNFBillImportDetails();
                    COM_CNFBillImportDetail.ExpenseHeadID = item.ExpenseHeadID;
                    COM_CNFBillImportDetail.Amount = 0;
                    COM_CNFBillImportDetail.IsCheck = false;
                    COM_CNFBillImportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName  , AmountPercentage = item.AmountPercentage , DefaultAmount = item.DefaultAmount};
                    Importmaster.COM_CNFBillImportDetails.Add(COM_CNFBillImportDetail);
                }



            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo");
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo");


            return View(Importmaster);
            

        }

        // POST: COM_CNFBillImportMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(COM_CNFBillImportMaster Mastersmain)
        {
            try
            {
                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                //if (ModelState.IsValid)
                //if (errors.Count() < 2)

                {

                    // If sales main has VoucherID then we can understand we have existing sales Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (Mastersmain.CNFExpenseID > 0)
                    {
                        Mastersmain.DateUpdated = DateTime.Now;
                        Mastersmain.DateAdded = DateTime.Now;
                        Mastersmain.comid = int.Parse(AppData.intComId);
                        Mastersmain.useridupdate = HttpContext.Session.GetString("userid");


                        //var CurrentProductSerial = db.ProductSerial.Where(p => p.VoucherId == Vouchermain.VoucherId);
                        var CurrentVoucherSub = db.COM_CNFBillImportDetailss.Where(p => p.CNFExpenseID == Mastersmain.CNFExpenseID);



                        //VoucherSub
                        //foreach (ProductSerial ss in CurrentProductSerial)
                        //db.ProductSerial.Remove(ss);

                        foreach (COM_CNFBillImportDetails ss in CurrentVoucherSub)
                            db.COM_CNFBillImportDetailss.Remove(ss);


                        foreach (COM_CNFBillImportDetails ss in Mastersmain.COM_CNFBillImportDetails)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            db.COM_CNFBillImportDetailss.Add(ss);

                    

                        }
                        



                        db.Entry(Mastersmain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        Mastersmain.DateAdded = DateTime.Now;
                        Mastersmain.DateUpdated = DateTime.Now;
                        Mastersmain.comid = int.Parse(AppData.intComId);
                        Mastersmain.userid = HttpContext.Session.GetString("userid");



                        db.COM_CNFBillImportMasters.Add(Mastersmain);
                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, CNFExpenseID = Mastersmain.CNFExpenseID, ex = "" });
                }
            }
            catch (Exception ex)
            {

                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            //return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Print(int? id, string type)
        {
            try
            {

                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;

                clsReport.rptList = null;

                Session["ReportType"] = "PDF";
                HttpContext.Session.SetString("ReportPath",
                Session["ReportPath"] = "~/Report/CommercialReport/rptImport.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptCNFBillImport] '" + id + "','" + AppData.intComId + "'");
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);


                string filename = db.COM_CNFBillImportMasters.Where(x => x.CNFExpenseID == id).Select(x => x.COM_CommercialInvoices.CommercialInvoiceNo + "_Import_CNF_Bill").Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", ""));



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

        // GET: COM_CNFBillImportMaster/Edit/5

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            //COM_CNFBillImportMaster cOM_CNFBillImportMaster = db.COM_CNFBillImportMasters.Find(id);
            COM_CNFBillImportMaster cOM_CNFBillImportMaster = db.COM_CNFBillImportMasters.Where(m => m.CNFExpenseID.ToString() == id.ToString() && m.comid.ToString() == AppData.intComId).FirstOrDefault(); //Find(id);// 

            if (cOM_CNFBillImportMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";


            //var Import = db.COM_CNFExpenseTypes.Where(m => m.ImportOrImport == "Import");//.Include(d => d.COM_CNFBillImportDetails);

            var Import = (from expensetype in db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Import" && m.comid.ToString() == AppData.intComId)
                          where !db.COM_CNFBillImportDetailss.Where(s => s.CNFExpenseID.ToString() == id.ToString()).Any(f => f.ExpenseHeadID == expensetype.ExpenseHeadID && expensetype.comid.ToString() == AppData.intComId)
                          select expensetype).ToList();
            //List<COM_CNFBillImportDetails> asdf = cOM_CNFBillImportMaster.COM_CNFBillImportDetails.ToList();

            foreach (var item in Import)

            {
                //int x = item.ExpenseHeadID;



                //foreach (var detailsImportitem in asdf)
                //{
                //if (detailsImportitem.ExpenseHeadID == x)
                //{


                //}
                //else
                //{
                    COM_CNFBillImportDetails COM_CNFBillImportDetail = new COM_CNFBillImportDetails();
                    COM_CNFBillImportDetail.ExpenseHeadID = item.ExpenseHeadID;
                    COM_CNFBillImportDetail.Amount = 0;
                    COM_CNFBillImportDetail.IsCheck = false;

                    COM_CNFBillImportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseNo = item.CNFExpenseNo,CNFExpenseName = item.CNFExpenseName, AmountPercentage = item.AmountPercentage, DefaultAmount = item.DefaultAmount };
                    cOM_CNFBillImportMaster.COM_CNFBillImportDetails.Add(COM_CNFBillImportDetail);
                //}


                //}

            }

            //var Importmaster = new COM_CNFBillImportMaster { CNFEXpenseBillDate = DateTime.Now.Date, CommercialCompanyId = 1, BuyerID = 1, JobNo = "", Messers = "message" };

            //cOM_CNFBillImportMaster.COM_CNFBillImportDetails = new List<COM_CNFBillImportDetails>();
            //foreach (var item in Import)
            //{
            //    COM_CNFBillImportDetails COM_CNFBillImportDetail = new COM_CNFBillImportDetails();
            //    COM_CNFBillImportDetail.ExpenseHeadID = item.ExpenseHeadID;
            //    COM_CNFBillImportDetail.Amount = 0;
            //    COM_CNFBillImportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };
            //    cOM_CNFBillImportMaster.COM_CNFBillImportDetails.Add(COM_CNFBillImportDetail);
            //}

            //ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillImportMaster.BuyerID);
            //ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CNFBillImportMaster.CommercialCompanyId);
            //ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_CNFBillImportMaster.CommercialInvoiceId);


            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillImportMaster.BuyerID);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName" , cOM_CNFBillImportMaster.SupplierId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName" , cOM_CNFBillImportMaster.CommercialCompanyId);
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo" , cOM_CNFBillImportMaster.CommercialInvoiceId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo" , cOM_CNFBillImportMaster.BBLCId);

            //return View();
            return View("Create", cOM_CNFBillImportMaster);
        }

        // POST: COM_CNFBillImportMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CNFExpenseID,CNFEXpenseBillDate,CommercialCompanyId,BuyerID,JobNo,Messers,Consignment,VasselName,RotationNo,Line,BondBENo,BondDate,BLNo,BLDate,BBLCNo,BBLCDate,Comission,TotalAmount,IsLocked,AddedBy,DateAdded,UpdatedBy,DateUpdated")] COM_CNFBillImportMaster cOM_CNFBillImportMaster)
        {
            if (ModelState.IsValid)
            {
                cOM_CNFBillImportMaster.comid = int.Parse(AppData.intComId);
                db.Entry(cOM_CNFBillImportMaster).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillImportMaster.BuyerID);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CNFBillImportMaster.CommercialCompanyId);
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_CNFBillImportMaster.CommercialInvoiceId);

            return View(cOM_CNFBillImportMaster);
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        // GET: COM_CNFBillImportMaster/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            //COM_CNFBillImportMaster cOM_CNFBillImportMaster = db.COM_CNFBillImportMasters.Find(id);
            COM_CNFBillImportMaster cOM_CNFBillImportMaster = db.COM_CNFBillImportMasters.Where(m => m.CNFExpenseID.ToString() == id.ToString() && m.comid.ToString() == AppData.intComId).FirstOrDefault(); //Find(id);// 

            if (cOM_CNFBillImportMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";


            //var Import = db.COM_CNFExpenseTypes.Where(m => m.ImportOrImport == "Import");//.Include(d => d.COM_CNFBillImportDetails);

            var Import = (from expensetype in db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Import" && m.comid.ToString() == AppData.intComId)
                          where !db.COM_CNFBillImportDetailss.Where(s => s.CNFExpenseID.ToString() == id.ToString()).Any(f => f.ExpenseHeadID == expensetype.ExpenseHeadID && expensetype.comid.ToString() == AppData.intComId)
                          select expensetype).ToList();
            //List<COM_CNFBillImportDetails> asdf = cOM_CNFBillImportMaster.COM_CNFBillImportDetails.ToList();

            foreach (var item in Import)

            {
                COM_CNFBillImportDetails COM_CNFBillImportDetail = new COM_CNFBillImportDetails();
                COM_CNFBillImportDetail.ExpenseHeadID = item.ExpenseHeadID;
                COM_CNFBillImportDetail.Amount = 0;
                COM_CNFBillImportDetail.IsCheck = false;

                COM_CNFBillImportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseNo = item.CNFExpenseNo, CNFExpenseName = item.CNFExpenseName, AmountPercentage = item.AmountPercentage, DefaultAmount = item.DefaultAmount };
                cOM_CNFBillImportMaster.COM_CNFBillImportDetails.Add(COM_CNFBillImportDetail);


            }
            

            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillImportMaster.BuyerID);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", cOM_CNFBillImportMaster.SupplierId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CNFBillImportMaster.CommercialCompanyId);
            ViewBag.CommercialInvoiceId = new SelectList(db.COM_CommercialInvoices, "CommercialInvoiceId", "CommercialInvoiceNo", cOM_CNFBillImportMaster.CommercialInvoiceId);
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo", cOM_CNFBillImportMaster.BBLCId);

            //return View();
            return View("Create", cOM_CNFBillImportMaster);
        }

        // POST: COM_CNFBillImportMaster/Delete/5
        [HttpPost, ActionName("Delete")]

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                COM_CNFBillImportMaster COM_CNFBillImportMastersss = db.COM_CNFBillImportMasters.Where(m => m.CNFExpenseID == id).FirstOrDefault();
                db.COM_CNFBillImportMasters.Remove(COM_CNFBillImportMastersss);
                db.SaveChanges();
                return Json(new { Success = 1, CNFExpenseID = COM_CNFBillImportMastersss.CNFExpenseID, ex = "" });

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
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult CIInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                COM_CommercialInvoice COM_CommercialInvoices = db.COM_CommercialInvoices.Where(y => y.CommercialInvoiceId == id).SingleOrDefault();
                //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

                return Json(COM_CommercialInvoices, JsonRequestBehavior.AllowGet);
                //return Json("tesst", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new SelectList(product, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
    }
}
