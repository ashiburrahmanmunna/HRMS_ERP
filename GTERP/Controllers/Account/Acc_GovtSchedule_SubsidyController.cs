using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Controllers.Account
{
    [OverridableAuthorize]
    public class Acc_GovtSchedule_SubsidyController : Controller
    {
        private TransactionLogRepository tranlog;

        //Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db;
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; } ///for report service

        public Acc_GovtSchedule_SubsidyController(GTRDBContext context, clsProcedure _clsProc, TransactionLogRepository tran)//for report service
        {

            db = context;
            //Repository = repository; ///for report service
            clsProc = _clsProc;
            tranlog = tran;
        }

        //[Authorize]
        // GET: Categories
        public ViewResult Index(string FromDate, string ToDate, string criteria)
        {

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));


            var Acc_ChartOfAccount = db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-2-10") || c.AccCode.Contains("2-2-11")); //&& c.ComId == (transactioncomid)
            this.ViewBag.AccId = new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");



            List<Acc_GovtSchedule_Subsidy> abcd = new List<Acc_GovtSchedule_Subsidy>();

            if (FromDate == null || FromDate == "")
            {

                abcd = db.Acc_GovtSchedule_Subsidy.Include(x => x.vAcc_ChartOfAccount).Where(x => x.comid == comid).ToList();

            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

                abcd = db.Acc_GovtSchedule_Subsidy.Include(x => x.vAcc_ChartOfAccount).Where(x => x.comid == comid).ToList();



            }
            //if (ToDate == null || ToDate == "")
            //{
            //}
            //else
            //{
            //    dtTo = Convert.ToDateTime(ToDate);

            //}



            //return View(db.Acc_GovtSchedule_Subsidy.Where(c => c.GovtScheduleId > 0).ToList());
            return View(abcd);

        }

        //[Authorize]
        // GET: Categories/Create
        public ActionResult Create()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            Acc_GovtSchedule_Subsidy abc = new Acc_GovtSchedule_Subsidy();


            var x = db.Acc_GovtSchedule_Subsidy.Where(x => x.comid == comid).OrderByDescending(x => x.GovtScheduleId).FirstOrDefault();
            if (x != null)
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");
                var Acc_ChartOfAccount = db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-2-10") || c.AccCode.Contains("2-2-11")); //&& c.ComId == (transactioncomid)
                this.ViewBag.AccId = new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", x.AccId);

                abc.FromDate = x.FromDate.AddYears(1);
                abc.ToDate = x.ToDate.AddYears(1);
                abc.Description = x.Description;
                abc.AccId = x.AccId;




            }
            else
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");
                var Acc_ChartOfAccount = db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-2-10") || c.AccCode.Contains("2-2-11")); //&& c.ComId == (transactioncomid)
                this.ViewBag.AccId = new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                abc.FromDate = DateTime.Now.Date;
                abc.ToDate = DateTime.Now.Date;
            }





            ViewBag.Title = "Create";


            return View(abc);
        }


        public static List<SelectListItem> CriteriaList = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Add", Value="Add"},
            new SelectListItem() { Text="Less", Value="Less"},
        };

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Acc_GovtSchedule_Subsidy Acc_GovtSchedulevar)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            //if (ModelState.IsValid)
            {
                if (Acc_GovtSchedulevar.GovtScheduleId > 0)
                {

                    if (Acc_GovtSchedulevar.comid == null || Acc_GovtSchedulevar.comid == "")
                    {
                        Acc_GovtSchedulevar.comid = comid;

                    }

                    Acc_GovtSchedulevar.DateUpdated = DateTime.Now;
                    Acc_GovtSchedulevar.useridUpdate = userid;
                    //Acc_GovtSchedulevar.isPost = true;


                    db.Entry(Acc_GovtSchedulevar).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {

                    Acc_GovtSchedulevar.DateAdded = DateTime.Now;
                    Acc_GovtSchedulevar.userid = userid;
                    Acc_GovtSchedulevar.comid = comid;
                    //Acc_GovtSchedulevar.isPost = true;


                    db.Acc_GovtSchedule_Subsidy.Add(Acc_GovtSchedulevar);
                    db.SaveChanges();


                    //db.Entry(Acc_GovtSchedulevar).GetDatabaseValues();
                    //int id = Acc_GovtSchedule_Subsidy.GovtScheduleId; // Yes it's here



                    //db.Categories.Add(Acc_GovtSchedule_Subsidy);

                }
            }
            return RedirectToAction("Create");

            //return View(Acc_GovtSchedule_Subsidy);
        }


        //[Authorize]
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            if (id == null)
            {
                return BadRequest();
            }
            Acc_GovtSchedule_Subsidy Acc_GovtSchedule_Subsidy = db.Acc_GovtSchedule_Subsidy.Where(c => c.GovtScheduleId == id).FirstOrDefault();


            if (Acc_GovtSchedule_Subsidy == null)
            {
                return NotFound();
            }


            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", Acc_GovtSchedule_Subsidy.Criteria);
            var Acc_ChartOfAccount = db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-2-10") || c.AccCode.Contains("2-2-11")); //&& c.ComId == (transactioncomid)
            this.ViewBag.AccId = new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

            ViewBag.Title = "Edit";

            return View("Create", Acc_GovtSchedule_Subsidy);

        }




        public JsonResult SetSessionAccountReport(string rptFormat, string FromDate, string ToDate, int? AccId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                string query = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (true)
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptGovtSubsidySchedule";
                    filename = "rptGovtSubsidySchedule_" + DateTime.Now.Date;
                    query = "Exec Acc_rptGovtSubsidy_Schedule '" + comid + "', '" + FromDate + "' ,'" + ToDate + "' ,'" + AccId + "'  ";


                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                }



                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                ////string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;


                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
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



        //[Authorize]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            if (id == null)
            {
                return BadRequest();
            }
            Acc_GovtSchedule_Subsidy Acc_GovtSchedule_Subsidy = db.Acc_GovtSchedule_Subsidy.Where(c => c.GovtScheduleId == id).FirstOrDefault();


            if (Acc_GovtSchedule_Subsidy == null)
            {
                return NotFound();
            }

            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", Acc_GovtSchedule_Subsidy.Criteria);
            var Acc_ChartOfAccount = db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-2-10") || c.AccCode.Contains("2-2-11")); //&& c.ComId == (transactioncomid)
            this.ViewBag.AccId = new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");


            ViewBag.Title = "Delete";

            return View("Create", Acc_GovtSchedule_Subsidy);
        }
        //        //[Authorize]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {

                Acc_GovtSchedule_Subsidy Acc_GovtSchedule_Subsidy = db.Acc_GovtSchedule_Subsidy.Where(c => c.GovtScheduleId == id).FirstOrDefault();

                db.Acc_GovtSchedule_Subsidy.Remove(Acc_GovtSchedule_Subsidy);
                db.SaveChanges();

                return Json(new { Success = 1, GovtScheduleId = Acc_GovtSchedule_Subsidy.GovtScheduleId, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }



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
