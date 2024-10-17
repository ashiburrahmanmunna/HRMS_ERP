using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Accounts
{
    public class Acc_BudgetRepository:BaseRepository<Acc_BudgetMain>, IAcc_BudgetRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<Acc_BudgetRepository> _logger;

        public Acc_BudgetRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<Acc_BudgetRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> Account()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Acc_ChartOfAccounts = _context.Acc_ChartOfAccounts.Where(c => c.ComId.ToString() == comid && c.AccId > 0 && c.AccType == "L");
            return new SelectList(Acc_ChartOfAccounts.Where(x => x.IsBankItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> Account1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Acc_ChartOfAccounts = _context.Acc_ChartOfAccounts.Where(c => c.ComId.ToString() == comid && c.AccId > 0 && c.AccType == "L");
            return new SelectList(Acc_ChartOfAccounts.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccountMain(int? FiscalYearId, int? FiscalMonthId)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            Acc_BudgetMain budgetMain = _context.Acc_BudgetMains.Where(m => m.FiscalYearId == FiscalYearId && m.FiscalMonthId == FiscalMonthId).FirstOrDefault();
            int AccId = budgetMain.BudgetSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
            return  new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId.ToString() == comid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);
        }

        public IEnumerable<SelectListItem> AccountMain1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId.ToString() == comid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccountParent()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var ChartOfAccountParent = _context.Acc_ChartOfAccounts
                                        .Where(p => p.ComId.ToString() == comid)
                                        .Where(c => c.AccId > 0 && c.AccType == "G" && c.ComId.ToString() == comid)
                                        .Select(s => new { Text = s.AccName, Value = s.AccId }).ToList();
            return  new SelectList(ChartOfAccountParent, "Value", "Text");
        }

        public IEnumerable<SelectListItem> Country(int? FiscalYearId, int? FiscalMonthId)
        {
            Acc_BudgetMain budgetMain = _context.Acc_BudgetMains.Where(m => m.FiscalYearId == FiscalYearId && m.FiscalMonthId == FiscalMonthId).FirstOrDefault();
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", budgetMain.CountryId);
        }

        public IEnumerable<SelectListItem> Country1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var transactioncompany = _context.Companys.Where(c => c.ComId.ToString() == comid).FirstOrDefault();
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", transactioncompany.CountryId);
        }

        public IEnumerable<SelectListItem> FiscalMonthId(int? FiscalYearId, int? FiscalMonthId)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            Acc_BudgetMain budgetMain = _context.Acc_BudgetMains.Where(m => m.FiscalYearId == FiscalYearId && m.FiscalMonthId == FiscalMonthId).FirstOrDefault();
            return new SelectList(_context.Acc_FiscalMonths.Where(p => p.ComId.ToString() == comid && p.FiscalMonthId == budgetMain.FiscalMonthId), "FiscalMonthId", "MonthName", budgetMain.FiscalMonthId).ToList();
        }

        public IEnumerable<SelectListItem> FiscalMonthId1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalMonths.Where(p => p.ComId.ToString() == comid && p.FiscalMonthId > 0), "FiscalMonthId", "MonthName").Take(0).ToList();
        }

        public IEnumerable<SelectListItem> FiscalYearId(int? FiscalYearId, int? FiscalMonthId)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            Acc_BudgetMain budgetMain = _context.Acc_BudgetMains.Where(m => m.FiscalYearId == FiscalYearId && m.FiscalMonthId == FiscalMonthId).FirstOrDefault();
            return new SelectList(_context.Acc_FiscalYears.Where(p => p.ComId.ToString() == comid && p.FiscalYearId > 0), "FiscalYearId", "FYName", budgetMain.FiscalYearId).ToList();
        }

        public IEnumerable<SelectListItem> FiscalYearId1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalYears.Where(p => p.ComId.ToString() == comid && p.FiscalYearId > 0), "FiscalYearId", "FYName").ToList();
        }

        public IEnumerable<SelectListItem> PrdUnitId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PrdUnits.Where(c => c.ComId.ToString() == comid), "PrdUnitId", "PrdUnitName");
        }
    }
}
