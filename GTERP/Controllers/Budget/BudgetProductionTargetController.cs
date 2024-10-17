using GTERP.BLL;
using GTERP.Interfaces.Budget;
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

namespace GTERP.Controllers.Budget
{
    [OverridableAuthorize]
    public class BudgetProductionTargetController : Controller
    {
        private GTRDBContext _context;
        private readonly IBudgetProductionTargetRepository _budgetProductionTargetRepository;
        public clsProcedure _clsProc { get; }

        public BudgetProductionTargetController(
            GTRDBContext context,
            clsProcedure clsProc,
            IBudgetProductionTargetRepository budgetProductionTargetRepository
            )
        {
            _context = context;
            _clsProc = clsProc;
            _budgetProductionTargetRepository = budgetProductionTargetRepository;
        }

        //[Authorize]
        // GET: Categories
        public ViewResult Index(string FromDate, string ToDate, string criteria)
        {
            this.ViewBag.AccId = _budgetProductionTargetRepository.AccId();

            var abcd = _budgetProductionTargetRepository.GetBudget_ProductionTargetList(FromDate, ToDate, criteria);
            return View(abcd);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            Budget_ProductionTarget abc = new Budget_ProductionTarget();

            var x = _context.Budget_ProductionTargets.Where(x => x.ComId == comid).OrderByDescending(x => x.ProductionTargetId).FirstOrDefault();
            if (x != null)
            {
                ViewData["Criteria"] = _budgetProductionTargetRepository.Criteria();
                this.ViewBag.AccId = _budgetProductionTargetRepository.AccId();
                abc.FromDate = x.FromDate.AddYears(1);
                abc.ToDate = x.ToDate.AddYears(1);
            }
            else
            {
                ViewData["Criteria"] = _budgetProductionTargetRepository.Criteria();
                this.ViewBag.AccId = _budgetProductionTargetRepository.AccId();
                abc.FromDate = DateTime.Now.Date;
                abc.ToDate = DateTime.Now.Date;
            }
            ViewBag.Title = "Create";

            return View(abc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Budget_ProductionTarget Acc_GovtSchedulevar)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            {
                if (Acc_GovtSchedulevar.ProductionTargetId > 0)
                {
                    if (Acc_GovtSchedulevar.ComId == null || Acc_GovtSchedulevar.ComId == "")
                    {
                        Acc_GovtSchedulevar.ComId = comid;
                    }
                    Acc_GovtSchedulevar.DateUpdated = DateTime.Now;
                    Acc_GovtSchedulevar.UpdateByUserId = userid;
                    _budgetProductionTargetRepository.Update(Acc_GovtSchedulevar);
                }
                else
                {
                    Acc_GovtSchedulevar.DateAdded = DateTime.Now;
                    Acc_GovtSchedulevar.UserId = userid;
                    Acc_GovtSchedulevar.ComId = comid;
                    _budgetProductionTargetRepository.Add(Acc_GovtSchedulevar);
                }
            }
            return RedirectToAction("Create");
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
            Budget_ProductionTarget Budget_ProductionTarget = _budgetProductionTargetRepository.FindById(id);

            if (Budget_ProductionTarget == null)
            {
                return NotFound();
            }
            this.ViewBag.AccId = _budgetProductionTargetRepository.AccId();
            ViewBag.Title = "Edit";
            return View("Create", Budget_ProductionTarget);
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
                if (true)
                {
                    reportname = "rptGovtEquitySchedule";
                    filename = "GovtEquitySchedule_" + DateTime.Now.Date;
                    query = "Exec Acc_rptGovtEquity_Schedule '" + comid + "', '" + FromDate + "' ,'" + ToDate + "' ,'" + AccId + "'  ";

                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                }
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });
                return Json(new { Url = redirectUrl });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
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
            Budget_ProductionTarget Budget_ProductionTarget = _budgetProductionTargetRepository.FindById(id);

            if (Budget_ProductionTarget == null)
            {
                return NotFound();
            }
            this.ViewBag.AccId = _budgetProductionTargetRepository.AccId();
            ViewBag.Title = "Delete";
            return View("Create", Budget_ProductionTarget);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                Budget_ProductionTarget Budget_ProductionTarget = _budgetProductionTargetRepository.FindById(id);

                _budgetProductionTargetRepository.Delete(Budget_ProductionTarget);
                return Json(new { Success = 1, ProductionTargetId = Budget_ProductionTarget.ProductionTargetId, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
    }
}
