using GTERP.Interfaces.Payroll;
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

namespace GTERP.Repository.Payroll
{
    public class WFLedgerRepository : BaseRepository<WF_Ledger>, IWFLedgerRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IUrlHelper _urlHelper;
        public clsProcedure _clsProc { get; }
        public WFLedgerRepository(
            GTRDBContext context,
            clsProcedure clsProc,
             IUrlHelperFactory urlHelperFactory,
             IActionContextAccessor actionContextAccessor
            ) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
            _clsProc = clsProc;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }


        public List<WF_Ledger> WFLedgerList(string FromDate, string ToDate, string criteria)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            List<WF_Ledger> abcd = new List<WF_Ledger>();

            if (FromDate == null || FromDate == "")
            {

                abcd = _context.WF_Ledgers.Include(x => x.vBankAccountNo).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

                abcd = _context.WF_Ledgers.Include(x => x.vBankAccountNo).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            }
            return abcd;
        }
        public IEnumerable<SelectListItem> BankAccountList()
        {
            var BankAccountNo = _context.BankAccountNos
                .Include(x => x.OpeningBanks)
                .Where(c => c.BankAccountId > 0);

            return new SelectList(BankAccountNo
                .Select(s => new
                {
                    Text = s.BankAccountNumber + " - [ " + s.OpeningBanks.OpeningBankName + " ]",
                    Value = s.BankAccountId
                })
                .ToList(), "Value", "Text");

        }

        public string SessionReport(string rptFormat, string FromDate, string ToDate, int? BankAccId)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");
            string userid = _httpcontext.HttpContext.Session.GetString("userid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            string query = "";
            if (true)
            {
                reportname = "rptPFBankAccountLedger";
                filename = "rptWFBankAccountLedger_" + DateTime.Now.Date;
                query = "Exec Payroll_rptBankAccountLedger '" + comid + "', '" + FromDate + "' ,'" + ToDate + "' ,'" + BankAccId + "' , 'WF Ledger'  ";
                _httpcontext.HttpContext.Session.SetString("reportquery", query);
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            }
            _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            clsReport.strReportPathMain = _httpcontext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpcontext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            return redirectUrl = _urlHelper.Action("WFLedgerList", "ReportViewer", new { reporttype = rptFormat });
        }

        public void CreateWFLedger(WF_Ledger WF_Ledger)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");
            if (WF_Ledger.WFLedgerId > 0)
            {
                if (WF_Ledger.ComId == null || WF_Ledger.ComId == "")
                {
                    WF_Ledger.ComId = comid;

                }

                WF_Ledger.DateUpdated = DateTime.Now;
                WF_Ledger.UpdateByUserId = userid;

                _context.Entry(WF_Ledger).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                WF_Ledger.DateAdded = DateTime.Now;
                WF_Ledger.UserId = userid;
                WF_Ledger.ComId = comid;

                _context.WF_Ledgers.Add(WF_Ledger);
                _context.SaveChanges();
            }
        }
    }
}
