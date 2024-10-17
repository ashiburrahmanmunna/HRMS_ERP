using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class ProcessLockRepository : BaseRepository<HR_ProcessLock>, IProcessLockRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ProcessLockRepository(GTRDBContext context, IHttpContextAccessor httpconext) : base(context)
        {
            _context = context;
            _httpContext = httpconext;
        }

        public Acc_FiscalMonth FiscalMonth()
        {
            return _context.Acc_FiscalMonths
                .Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> FiscalMonthID()
        {
            var fiscalYear = _context.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            var fiscalMonth = _context.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

            return new SelectList(_context.Acc_FiscalMonths.Where(x => x.FYId == fiscalYear.FYId).OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);
        }

        public IEnumerable<SelectListItem> FiscalMonthID2()
        {
            var fiscalYear = _context.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            var fiscalMonth = _context.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();
            return new SelectList(_context.Acc_FiscalMonths.Where(x => x.FYId == fiscalYear.FiscalYearId).OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");
        }

        public Acc_FiscalYear FiscalYear()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_FiscalYears
                .Where(f => f.isRunning == true && f.isWorking == true && f.ComId == comid && f.IsDelete == false)
                .FirstOrDefault();
        }

        public IEnumerable<SelectListItem> FiscalYearID()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var fiscalYear = _context.Acc_FiscalYears.Where(f =>  f.isRunning == true && f.isWorking == true && f.ComId == comid && f.IsDelete==false).ToList();
            return new SelectList(fiscalYear, "FiscalYearId", "FYName");
        }

        public IEnumerable<Acc_FiscalMonth> GetFiscalMonth(int FiscalYearId)
        {
            return _context.Acc_FiscalMonths.OrderByDescending(y => y.MonthName)
                .Where(m => m.FYId == FiscalYearId).ToList();
        }

        public List<HR_ProcessLock> GetProcessLockData()
        {
            return GetAll()
                .Include(x => x.Acc_FiscalMonths)
                .Include(x => x.Acc_FiscalYears).ToList();
        }

        public IEnumerable<SelectListItem> LockTypeList()
        {
            return new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "ProcessLock")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
        }


    }
}
