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
    public class PFLedgerRepository : BaseRepository<PF_Ledger>, IPFLedgerRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IUrlHelper _urlHelper;
        public clsProcedure _clsProc { get; }
        public PFLedgerRepository(
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

        public void CreatePFLedger(PF_Ledger PF_Ledger)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");
            if (PF_Ledger.PFLedgerId > 0)
            {
                if (PF_Ledger.ComId == null || PF_Ledger.ComId == "")
                {
                    PF_Ledger.ComId = comid;

                }

                PF_Ledger.DateUpdated = DateTime.Now;
                PF_Ledger.UpdateByUserId = userid;

                _context.Entry(PF_Ledger).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                PF_Ledger.DateAdded = DateTime.Now;
                PF_Ledger.UserId = userid;
                PF_Ledger.ComId = comid;

                _context.PF_Ledgers.Add(PF_Ledger);
                _context.SaveChanges();
            }
        }

        public List<PF_Ledger> PFLedgerList(string FromDate, string ToDate, string criteria)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            List<PF_Ledger> abcd = new List<PF_Ledger>();

            if (FromDate == null || FromDate == "")
            {

                abcd = _context.PF_Ledgers.Include(x => x.vBankAccountNo).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

                abcd = _context.PF_Ledgers.Include(x => x.vBankAccountNo).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            }
            return abcd;
        }

        public string PFSessionReport(string rptFormat, string FromDate, string ToDate, int? BankAccId)
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
    }
}
