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
    public class WorkOrderMastersController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        // GET: WorkOrderMasters
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Index()
        {
            var workOrderMasters = db.WorkOrderMasters.Include(w => w.ApprovedBy).Include(w => w.CommercialCompany).Include(w => w.RecommenedBy).Include(w => w.WorkorderStatus);
            return View(workOrderMasters.ToList());
        }

        // GET: WorkOrderMasters/Details/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            WorkOrderMaster workOrderMaster = db.WorkOrderMasters.Find(id);
            if (workOrderMaster == null)
            {
                return NotFound();
            }
            return View(workOrderMaster);
        }

        // GET: WorkOrderMasters/Create
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Create()
        {

            ViewBag.Title = "Create";

            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.RecommenedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName");
            ViewBag.WorkOrderStatusId = new SelectList(db.WorkorderStatus, "WorkorderStatusId", "WorkorderStatusName");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            ViewBag.RecommendedId = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName");


            return View();
        }

        // POST: WorkOrderMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkOrderMaster workOrderMaster)
        {
            try
            {
                ViewBag.Title = "Create";

                  if (AppData.intComId == "0" || AppData.intComId == null)
            {
                return NotFound();

            }

                  if (AppData.intComId == "0" || AppData.intComId == null)
            {
                return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

            }

            //Mastersmain.VoucherInputDate = DateTime.Now.Date;

            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });


            if (ModelState.IsValid)
            {

                if (workOrderMaster.WorkOrderId > 0)
                {

                    db.Entry(workOrderMaster).State = EntityState.Modified;
                    workOrderMaster.DateAdded = DateTime.Now;
                    workOrderMaster.DateUpdated = DateTime.Now;
                    workOrderMaster.comid = int.Parse(AppData.intComId);
                    workOrderMaster.userid = HttpContext.Session.GetString("userid");


                    db.SaveChanges();
                }


                //message = "Weekend update succeded";

                else
                {
                    workOrderMaster.DateAdded = DateTime.Now;
                    workOrderMaster.DateUpdated = DateTime.Now;
                    workOrderMaster.comid = int.Parse(AppData.intComId);
                    workOrderMaster.userid = HttpContext.Session.GetString("userid");


                    db.WorkOrderMasters.Add(workOrderMaster);
                    //db.WeekdaySectionWise.Add(item);
                    db.SaveChanges();


                }

                return RedirectToAction("Index");
            }

           
            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.ApprovedById);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", workOrderMaster.CommercialCompanyId);
            ViewBag.RecommenedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.RecommenedById);
            ViewBag.WorkOrderStatusId = new SelectList(db.WorkorderStatus, "WorkorderStatusId", "WorkorderStatusName", workOrderMaster.WorkOrderStatusId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", workOrderMaster.WorkOrderStatusId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", workOrderMaster.WorkOrderStatusId);
            ViewBag.RecommendedId = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.WorkOrderStatusId);


                return View(workOrderMaster);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: WorkOrderMasters/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            WorkOrderMaster workOrderMaster = db.WorkOrderMasters.Find(id);
            if (workOrderMaster == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.ApprovedById);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", workOrderMaster.CommercialCompanyId);
            ViewBag.RecommenedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.RecommenedById);
            ViewBag.WorkOrderStatusId = new SelectList(db.WorkorderStatus, "WorkorderStatusId", "WorkorderStatusName", workOrderMaster.WorkOrderStatusId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", workOrderMaster.WorkOrderStatusId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", workOrderMaster.WorkOrderStatusId);
            ViewBag.RecommendedId = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.WorkOrderStatusId);

            return View("Create",workOrderMaster);
        }

        // POST: WorkOrderMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "WorkOrderId,CommercialCompanyId,WorkOrderNo,WorkOrderDate,SupplierId,ToPerson,AgreementDate,DeliveryDate,ServiceContractStartDate,ServiceContractEndDate,CurrencyId,ConversionRate,WorkOrderType,Subject,Body,PaymentTerms,OtherTerms,WorkOrderQty,WorkOrderRate,SubTotal,SalesTax,OtherExp,WorkOrderAmt,AdvancePayment,NetPayable,Remarks,IsLocked,ServiceContract,WorkOrderStatusId,WODetails,ShipTo,Shipping,Total,ApprovedById,RecommenedById,DateApproval,AddedBy,DateAdded,UpdatedBy,DateUpdated")] WorkOrderMaster workOrderMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workOrderMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.ApprovedById);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", workOrderMaster.CommercialCompanyId);
            ViewBag.RecommenedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.RecommenedById);
            ViewBag.WorkOrderStatusId = new SelectList(db.WorkorderStatus, "WorkorderStatusId", "WorkorderStatusName", workOrderMaster.WorkOrderStatusId);
            return View(workOrderMaster);
        }

        // GET: WorkOrderMasters/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            WorkOrderMaster workOrderMaster = db.WorkOrderMasters.Find(id);
            if (workOrderMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            ViewBag.ApprovedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.ApprovedById);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", workOrderMaster.CommercialCompanyId);
            ViewBag.RecommenedById = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.RecommenedById);
            ViewBag.WorkOrderStatusId = new SelectList(db.WorkorderStatus, "WorkorderStatusId", "WorkorderStatusName", workOrderMaster.WorkOrderStatusId);
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", workOrderMaster.WorkOrderStatusId);
            ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode", workOrderMaster.WorkOrderStatusId);
            ViewBag.RecommendedId = new SelectList(db.ApprovedBys, "ApprovedById", "ApprovedByName", workOrderMaster.WorkOrderStatusId);

            return View("Create", workOrderMaster);
        }

        // POST: COM_DocumentAcceptance/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]c
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                WorkOrderMaster WorkOrderMaster = db.WorkOrderMasters.Find(id);
                db.WorkOrderMasters.Remove(WorkOrderMaster);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = WorkOrderMaster.WorkOrderId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintWO(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/CommercialReport/rptWorkOrder.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptWorkOrder] '" + id + "','" + AppData.intComId + "'");

                string ReportPath = "~/Report/CommercialReport/rptWorkOrder.rdlc";
                string SQLQuery = "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptWorkOrder] '" + id + "','" + AppData.intComId + "'");
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
