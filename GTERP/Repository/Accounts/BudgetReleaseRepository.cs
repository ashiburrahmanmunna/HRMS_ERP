using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class BudgetReleaseRepository : BaseRepository<Acc_BudgetRelease>, IBudgetReleaseRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        public BudgetReleaseRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IEnumerable<SelectListItem> AccId()
        {
            var filterAccount = FilterAccount();
            return new SelectList(filterAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public BudgetDetails BudgetDetails(int AccId, int FiscalYearId)
        {
            var main = BudgetMain(FiscalYearId);
            return _context.BudgetDetails.Where(b => b.BudgetMainId == main.BudgetMainId && b.AccId == AccId).FirstOrDefault();
        }

        public BudgetMain BudgetMain(int FiscalYearId)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.BudgetMains.Where(b => b.FiscalYearId == FiscalYearId && b.ComId == comid).FirstOrDefault();
        }

        public double BudgetRelease(int AccId, int FiscalYearId, int BudgetReleaseId)
        {
            return _context.Acc_BudgetRelease.Where(r => r.BudgetReleaseId != BudgetReleaseId && r.AccId == AccId && r.FiscalYearId == FiscalYearId).Sum(r => r.DebitAmount);
        }

        public IQueryable<Acc_ChartOfAccount> FilterAccount()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_ChartOfAccounts.Where(
               acc => _context.Acc_BudgetRelease.Any(a => a.AccId == acc.AccId)
               && acc.ComId == comid).Distinct();
        }

        public Acc_FiscalMonth FiscalMonth()
        {
            return _context.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> FiscalMonthId()
        {
            var fiscalMonth = FiscalMonth();
            var fiscalYear = FiscalYear();
            return new SelectList(_context.Acc_FiscalMonths.Where(x => x.FYId == fiscalYear.FYId).OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);
        }

        public Acc_FiscalYear FiscalYear()
        {
            return _context.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> FiscalYearId()
        {
            return new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
        }

        public List<Acc_BudgetRelease> GetBudgetRelease()
        {
            return _context.Acc_BudgetRelease.Include(b => b.Acc_FiscalYear).Include(b => b.Acc_FiscalMonth).Include(p => p.Acc_ChartOfAccount).Include(p => p.HR_Emp_Info).ToList();
        }

        public List<Acc_FiscalMonth> GetMonthData(int FiscalYearId)
        {
            return _context.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(m => m.FYId == FiscalYearId).ToList();
        }

        public string SetSessionAccountReport(string rptFormat, string FiscalYearId, string AccId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            string query = "";

            if (true)
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptBudgetRelease";
                filename = "Notes_" + DateTime.Now.Date;
                query = "Exec Acc_rptBudgetRelease '" + comid + "', '" + FiscalYearId + "' ,'" + AccId + "'";


                _httpContext.HttpContext.Session.SetString("reportquery", query);
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            }

            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            string DataSourceName = "DataSet1";

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }
    }
}
