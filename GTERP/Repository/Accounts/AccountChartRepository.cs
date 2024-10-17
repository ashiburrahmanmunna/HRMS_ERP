using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class AccountChartRepository :BaseRepository<Acc_ChartOfAccount>, IAccountChartRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<AccountChartRepository> _logger;

        public AccountChartRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<AccountChartRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public List<SelectListItem> AccumulatedDepId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var lastacccoa = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.AccCode.Contains("1-3-1") && x.AccType == "G"), "AccId", "AccName", lastacccoa.AccumulatedDepId).ToList();
        }

        public IEnumerable<SelectListItem> AccumulatedDepId2(int? Id)
        {
            Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Find(Id);
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.AccCode.Contains("1-3-1") && x.AccType == "G"), "AccId", "AccName", chartofaccount.AccumulatedDepId).ToList();
        }

        public IEnumerable<SelectListItem> AccumulatedDepIdElse()
        {
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.AccCode.Contains("1-3-1") && x.AccType == "G"), "AccId", "AccName").ToList();
        }

        public void AddAccountChart(Acc_ChartOfAccount model)
        {
            _context.Acc_ChartOfAccounts.Add(model);
            _context.SaveChanges();
        }

        public List<Acc_ChartOfAccount> All()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var data = _context.Acc_ChartOfAccounts.Where(a => a.ComId == comid).OrderBy(a => a.ParentID).ToList();
            return data;
        }

        public List<Acc_ChartOfAccount> ChartOfAccountList()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var data = _context.Acc_ChartOfAccounts.Take(1).Include(x => x.ParentChartOfAccount).Where(c => c.AccId > 0 && c.ComId == comid && c.IsSysDefined == 0 && !c.IsDelete).ToList();
            return data;
        }

        public List<COAtemp> COAParent1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);
            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("Acc_prcgetCOAParent", sqlParameter);
            return COAParent;
        }

        public IEnumerable<SelectListItem> CountryId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var defaultcurrency = _context.Companys.Where(x => x.CompanyCode == comid).Select(x => x.CountryId).FirstOrDefault();
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", defaultcurrency).ToList();
        }

        public IEnumerable<SelectListItem> CountryId1(Acc_ChartOfAccount model)
        {
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", model.ComId);
        }

        public IEnumerable<SelectListItem> CountryId2(int? Id)
        {
            Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Find(Id);
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", chartofaccount.CountryID).ToList();
        }

        public int DefaultCurrency()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var data = _context.Companys.Where(x => x.CompanyCode == comid).Select(x => x.CountryId).FirstOrDefault();
            return data;
        }

        public IEnumerable<SelectListItem> DepExpenseId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var lastacccoa = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.AccCode.Contains("4-2-2") && x.AccType == "G"), "AccId", "AccName", lastacccoa.DepExpenseId).ToList();
        }

        public IEnumerable<SelectListItem> DepExpenseId2(int? Id)
        {
            Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Find(Id);
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.AccCode.Contains("4-2-2") && x.AccType == "G"), "AccId", "AccName", chartofaccount.DepExpenseId).ToList();
        }

        public IEnumerable<SelectListItem> DepExpenseIdElse()
        {
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.AccCode.Contains("4-2-2") && x.AccType == "G"), "AccId", "AccName").ToList();
        }

        public Acc_ChartOfAccount LastAcccoa()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var data = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();
            return data;
        }

        public IEnumerable<SelectListItem> OpFYId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalYears.OrderByDescending(c => c.FYId).Where(c => c.FYId > 0 && c.ComId == comid), "FYId", "FYName");
        }

        public IEnumerable<SelectListItem> OpFYId1(Acc_ChartOfAccount model)
        {
            return new SelectList(_context.Acc_FiscalYears.Where(c => c.FYId > 0 && c.ComId == (_httpcontext.HttpContext.Session.GetString("comid"))), "FYId", "FYName", model.opFYId);
        }

        public IEnumerable<SelectListItem> OpFYId2(int? Id)
        {
            Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Find(Id);
            return new SelectList(_context.Acc_FiscalYears.OrderByDescending(c => c.FYId).Where(c => c.FYId > 0 && c.ComId == (_httpcontext.HttpContext.Session.GetString("comid"))), "FYId", "FYName", chartofaccount.opFYId);
        }

        public IEnumerable<SelectListItem> ParentId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var lastacccoa = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);

            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("Acc_prcgetCOAParent", sqlParameter);

            return new SelectList(COAParent, "AccId", "AccName", lastacccoa.ParentID).ToList();
        }

        public IEnumerable<SelectListItem> ParentId1(Acc_ChartOfAccount model)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var lastacccoa = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);

            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("Acc_prcgetCOAParent", sqlParameter);
            return new SelectList(COAParent, "AccId", "AccName", model.ParentID);
        }

        public IEnumerable<SelectListItem> ParentId2(int? Id)
        {
            Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Find(Id);

            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var lastacccoa = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);

            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("Acc_prcgetCOAParent", sqlParameter);

            return new SelectList(COAParent, "AccId", "AccName", chartofaccount.ParentID).ToList(); 
        }

        public IEnumerable<SelectListItem> ParentIdElse()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var lastacccoa = _context.Acc_ChartOfAccounts.Where(x => x.ComId == comid).OrderByDescending(x => x.AccId).FirstOrDefault();

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);

            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("Acc_prcgetCOAParent", sqlParameter);
            return new SelectList(COAParent, "AccId", "AccName").ToList();
        }

        public void UpdateAccountChart(Acc_ChartOfAccount model)
        {
            var userid = _httpcontext.HttpContext.Session.GetString("userid");
            model.UpdateByUserId = userid;
            model.DateUpdated = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
