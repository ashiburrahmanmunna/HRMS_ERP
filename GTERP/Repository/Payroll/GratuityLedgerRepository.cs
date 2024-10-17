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
    public class GratuityLedgerRepository : BaseRepository<Gratuity_Ledger>, IGratuityLedgerRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IUrlHelper _urlHelper;
        public clsProcedure _clsProc { get; }
        public GratuityLedgerRepository(
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

        public List<Gratuity_Ledger> GratuityLedgerList(string FromDate, string ToDate, string criteria)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            List<Gratuity_Ledger> abcd = new List<Gratuity_Ledger>();

            if (FromDate == null || FromDate == "")
            {

                abcd = _context.Gratuity_Ledgers.Include(x => x.vBankAccountNo).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

                abcd = _context.Gratuity_Ledgers.Include(x => x.vBankAccountNo).Where(x => x.ComId == comid && !x.IsDelete).ToList();
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

        public string GratuitySessionReport(string rptFormat, string FromDate, string ToDate, int? BankAccId)
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

        public void CreateGratuityLedger(Gratuity_Ledger Gratuity_Ledger)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");
            if (Gratuity_Ledger.GratuityLedgerId > 0)
            {
                if (Gratuity_Ledger.ComId == null || Gratuity_Ledger.ComId == "")
                {
                    Gratuity_Ledger.ComId = comid;

                }

                Gratuity_Ledger.DateUpdated = DateTime.Now;
                Gratuity_Ledger.UpdateByUserId = userid;

                _context.Entry(Gratuity_Ledger).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                Gratuity_Ledger.DateAdded = DateTime.Now;
                Gratuity_Ledger.UserId = userid;
                Gratuity_Ledger.ComId = comid;

                _context.Gratuity_Ledgers.Add(Gratuity_Ledger);
                _context.SaveChanges();
            }
        }
    }
}
