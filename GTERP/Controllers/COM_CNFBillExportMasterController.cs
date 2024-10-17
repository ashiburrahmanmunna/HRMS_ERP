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
using GTERP.AppData;
using GTERP.Controllers.Common;
using GTERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class COM_CNFBillExportMasterController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();

        // GET: COM_CNFBillExportMaster
        public ActionResult Index()
        {
            var cOM_CNFBillExportMasters = db.COM_CNFBillExportMasters.Include(c => c.BuyerInformations).Include(c => c.Companies);
            return View(cOM_CNFBillExportMasters.ToList());
        }

        // GET: COM_CNFBillExportMaster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_CNFBillExportMaster cOM_CNFBillExportMaster = db.COM_CNFBillExportMasters.Find(id);
            if (cOM_CNFBillExportMaster == null)
            {
                return NotFound();
            }
            return View(cOM_CNFBillExportMaster);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult EIInfo(int id)
        {
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                ExportInvoiceMaster ExportInvoiceMasters = db.ExportInvoiceMasters.Where(y => y.InvoiceId == id).SingleOrDefault();
                //return Json(new SelectList(licitiesa, "Value", "Text", JsonRequestBehavior.AllowGet));

                return Json(ExportInvoiceMasters, JsonRequestBehavior.AllowGet);
                //return Json("tesst", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new SelectList(product, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        // GET: COM_CNFBillExportMaster/Create
        public ActionResult Create()
        {
            try
            {

                ////comid problem plz check

                if (AppData.intComId.ToString().Length == 0)
                {
                    ViewBag.error("Session Expired");
//                    throw Exception;//.Equals("Session Expired");
                }

            ViewBag.Title = "Create";

            var Export = db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Export" && m.comid.ToString() == AppData.intComId);

            var Exportmaster = new COM_CNFBillExportMaster { CNFEXpenseBillDate = DateTime.Now.Date, CommercialCompanyId = 1, BuyerID = 1, JobNo = "", Messers = "Messers" };
            Exportmaster.COM_CNFBillExportDetails = new List<COM_CNFBillExportDetails>();
            foreach (var item in Export)
            {
                COM_CNFBillExportDetails COM_CNFBillExportDetail = new COM_CNFBillExportDetails();
                COM_CNFBillExportDetail.ExpenseHeadID = item.ExpenseHeadID;
                COM_CNFBillExportDetail.Amount = 0;
                COM_CNFBillExportDetail.IsCheck = false;
                COM_CNFBillExportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName, AmountPercentage = item.AmountPercentage, DefaultAmount = item.DefaultAmount };
                Exportmaster.COM_CNFBillExportDetails.Add(COM_CNFBillExportDetail);
            }



            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
                ViewBag.InvoiceId = new SelectList(db.ExportInvoiceMasters, "InvoiceId", "InvoiceNo");

                return View(Exportmaster);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // POST: COM_CNFBillExportMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(COM_CNFBillExportMaster Mastersmain)
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
                        Mastersmain.userid = HttpContext.Session.GetString("userid");



                        //var CurrentProductSerial = db.ProductSerial.Where(p => p.VoucherId == Vouchermain.VoucherId);
                        var CurrentVoucherSub = db.COM_CNFBillExportDetails.Where(p => p.CNFExpenseID == Mastersmain.CNFExpenseID);



                        //VoucherSub
                        //foreach (ProductSerial ss in CurrentProductSerial)
                        //db.ProductSerial.Remove(ss);

                        foreach (COM_CNFBillExportDetails ss in CurrentVoucherSub)
                            db.COM_CNFBillExportDetails.Remove(ss);


                        foreach (COM_CNFBillExportDetails ss in Mastersmain.COM_CNFBillExportDetails)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            db.COM_CNFBillExportDetails.Add(ss);



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



                        db.COM_CNFBillExportMasters.Add(Mastersmain);
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
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Print(int? id, string type)
        {
            AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

            //Session["rptList"] = null;
            clsReport.rptList = null;

            Session["ReportType"] = "PDF";

            HttpContext.Session.SetString("ReportPath",

            HttpContext.Session.SetString("ReportPath","~/Report/CommercialReport/rptExport.rdlc");
            HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptCNFBillExport] '" + id + "','" + AppData.intComId + "'");
            HttpContext.Session.SetString("DataSourceName", "DataSet1");

            HttpContext.Session.SetObject("rptList", postData);

            


            string filename = db.COM_CNFBillExportMasters.Where(x => x.CNFExpenseID == id).Select(x => x.ExportInvoiceMasters.InvoiceNo + "_Export_CNF_Bill").Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", ""));


            clsReport.strReportPathMain = Session["ReportPath"].ToString();
            clsReport.strQueryMain = Session["reportquery"].ToString();
            clsReport.strDSNMain = Session["DataSourceName"].ToString();


            return RedirectToAction("Index", "ReportViewer");


        }

        // GET: COM_CNFBillExportMaster/Edit/5

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            //COM_CNFBillExportMaster cOM_CNFBillExportMaster = db.COM_CNFBillExportMasters.Find(id);
            COM_CNFBillExportMaster cOM_CNFBillExportMaster = db.COM_CNFBillExportMasters.Where(m => m.CNFExpenseID.ToString() == id.ToString() && m.comid.ToString() == AppData.intComId).FirstOrDefault(); //Find(id);// 

            if (cOM_CNFBillExportMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";


            //var Export = db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Export");//.Include(d => d.COM_CNFBillExportDetails);

            var Export = (from expensetype in db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Export" && m.comid.ToString() == AppData.intComId)
                          where !db.COM_CNFBillExportDetails.Where(s => s.CNFExpenseID.ToString() == id.ToString()).Any(f => f.ExpenseHeadID == expensetype.ExpenseHeadID && expensetype.comid.ToString() == AppData.intComId)
                          select expensetype).ToList();
            //List<COM_CNFBillExportDetails> asdf = cOM_CNFBillExportMaster.COM_CNFBillExportDetails.ToList();

            foreach (var item in Export)

            {
                //int x = item.ExpenseHeadID;



                //foreach (var detailsExportitem in asdf)
                //{
                //if (detailsExportitem.ExpenseHeadID == x)
                //{


                //}
                //else
                //{
                COM_CNFBillExportDetails COM_CNFBillExportDetail = new COM_CNFBillExportDetails();
                COM_CNFBillExportDetail.ExpenseHeadID = item.ExpenseHeadID;
                COM_CNFBillExportDetail.Amount = 0;
                COM_CNFBillExportDetail.IsCheck = false;

                COM_CNFBillExportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseNo = item.CNFExpenseNo, CNFExpenseName = item.CNFExpenseName ,AmountPercentage = item.AmountPercentage, DefaultAmount = item.DefaultAmount };
                cOM_CNFBillExportMaster.COM_CNFBillExportDetails.Add(COM_CNFBillExportDetail);
                //}


                //}

            }

            //var Exportmaster = new COM_CNFBillExportMaster { CNFEXpenseBillDate = DateTime.Now.Date, CommercialCompanyId = 1, BuyerID = 1, JobNo = "", Messers = "message" };

            //cOM_CNFBillExportMaster.COM_CNFBillExportDetails = new List<COM_CNFBillExportDetails>();
            //foreach (var item in Export)
            //{
            //    COM_CNFBillExportDetails COM_CNFBillExportDetail = new COM_CNFBillExportDetails();
            //    COM_CNFBillExportDetail.ExpenseHeadID = item.ExpenseHeadID;
            //    COM_CNFBillExportDetail.Amount = 0;
            //    COM_CNFBillExportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseName = item.CNFExpenseName };
            //    cOM_CNFBillExportMaster.COM_CNFBillExportDetails.Add(COM_CNFBillExportDetail);
            //}

            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillExportMaster.BuyerID);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CNFBillExportMaster.CommercialCompanyId);
            ViewBag.InvoiceId = new SelectList(db.ExportInvoiceMasters, "InvoiceId", "InvoiceNo", cOM_CNFBillExportMaster.InvoiceId);

            //return View();
            return View("Create", cOM_CNFBillExportMaster);
        }

        // POST: COM_CNFBillExportMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CNFExpenseID,CNFEXpenseBillDate,CommercialCompanyId,BuyerID,JobNo,Messers,Consignment,VasselName,RotationNo,Line,BondBENo,BondDate,BLNo,BLDate,BBLCNo,BBLCDate,Comission,TotalAmount,IsLocked,AddedBy,DateAdded,UpdatedBy,DateUpdated")] COM_CNFBillExportMaster cOM_CNFBillExportMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOM_CNFBillExportMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillExportMaster.BuyerID);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CNFBillExportMaster.CommercialCompanyId);
            ViewBag.InvoiceId = new SelectList(db.ExportInvoiceMasters, "InvoiceId", "InvoiceNo", cOM_CNFBillExportMaster.InvoiceId);
            return View(cOM_CNFBillExportMaster);
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        // GET: COM_CNFBillExportMaster/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            //COM_CNFBillExportMaster cOM_CNFBillExportMaster = db.COM_CNFBillExportMasters.Find(id);
            COM_CNFBillExportMaster cOM_CNFBillExportMaster = db.COM_CNFBillExportMasters.Where(m => m.CNFExpenseID.ToString() == id.ToString() && m.comid.ToString() == AppData.intComId).FirstOrDefault(); //Find(id);// 

            if (cOM_CNFBillExportMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            var Export = (from expensetype in db.COM_CNFExpenseTypes.Where(m => m.ImportOrExport == "Export" && m.comid.ToString() == AppData.intComId)
                          where !db.COM_CNFBillExportDetails.Where(s => s.CNFExpenseID.ToString() == id.ToString()).Any(f => f.ExpenseHeadID == expensetype.ExpenseHeadID && expensetype.comid.ToString() == AppData.intComId)
                          select expensetype).ToList();

            foreach (var item in Export)

            {

                COM_CNFBillExportDetails COM_CNFBillExportDetail = new COM_CNFBillExportDetails();
                COM_CNFBillExportDetail.ExpenseHeadID = item.ExpenseHeadID;
                COM_CNFBillExportDetail.Amount = 0;
                COM_CNFBillExportDetail.IsCheck = false;

                COM_CNFBillExportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseNo = item.CNFExpenseNo, CNFExpenseName = item.CNFExpenseName, AmountPercentage = item.AmountPercentage, DefaultAmount = item.DefaultAmount };
                cOM_CNFBillExportMaster.COM_CNFBillExportDetails.Add(COM_CNFBillExportDetail);


            }
            cOM_CNFBillExportMaster.COM_CNFBillExportDetails.OrderBy(s => s.ExpenseHeadID);

            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName", cOM_CNFBillExportMaster.BuyerID);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_CNFBillExportMaster.CommercialCompanyId);
            ViewBag.InvoiceId = new SelectList(db.ExportInvoiceMasters, "InvoiceId", "InvoiceNo", cOM_CNFBillExportMaster.InvoiceId);
            //return View();
            return View("Create", cOM_CNFBillExportMaster);
        }

        // POST: COM_CNFBillExportMaster/Delete/5
        [HttpPost, ActionName("Delete")]

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                COM_CNFBillExportMaster COM_CNFBillExportMastersss = db.COM_CNFBillExportMasters.Where(m => m.CNFExpenseID == id).FirstOrDefault();
                db.COM_CNFBillExportMasters.Remove(COM_CNFBillExportMastersss);
                db.SaveChanges();
                return Json(new { Success = 1, CNFExpenseID = COM_CNFBillExportMastersss.CNFExpenseID, ex = "" });

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
    }
}
