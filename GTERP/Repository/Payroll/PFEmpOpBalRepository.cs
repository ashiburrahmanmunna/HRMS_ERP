using GTERP.Interfaces.Payroll;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Payroll
{
    public class PFEmpOpBalRepository : BaseRepository<HR_Emp_PF_OPBal>, IPFEmpOpBalRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public PFEmpOpBalRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IEnumerable<SelectListItem> CreateCreditAccId()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PF_ChartOfAccounts
                .Where(x => x.comid == ComId && x.AccType == "L"), "AccId", "AccName");
        }

        public IEnumerable<SelectListItem> CreateDebitAccId()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PF_ChartOfAccounts
                .Where(x => x.comid == ComId && x.AccType == "L"), "AccId", "AccName");
        }

        public IEnumerable<SelectListItem> CreatelEmpInfoList()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join ep in _context.HR_Emp_PersonalInfo on emp.EmpId equals ep.EmpId
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == ComId & ep.IsAllowPF == true
                           select new
                           {
                               Value = emp.EmpId,
                               Text = ep.PFFileNo + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");
        }

        public IEnumerable<SelectListItem> CreatePFOPBalYID()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalYears
                .Where(x => x.ComId == ComId), "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> CreditAccId(int? id)
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            var PFOPBal = _context.HR_Emp_PF_OPBal
                  .Include(c => c.HR_Emp_Info)
                .FirstOrDefault(m => m.PFOPBalId == id);

            return new SelectList(_context.PF_ChartOfAccounts
                .Where(x => x.comid == ComId && x.AccType == "L")
                , "AccId", "AccName", PFOPBal.CreditAccId);
        }

        public IEnumerable<SelectListItem> DebitAccId(int? id)
        {
            var PFOPBal = _context.HR_Emp_PF_OPBal
                  .Include(c => c.HR_Emp_Info)
                .FirstOrDefault(m => m.PFOPBalId == id);

            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PF_ChartOfAccounts
                .Where(x => x.comid == ComId && x.AccType == "L"),
                "AccId", "AccName", PFOPBal.DebitAccId);
        }

        public IEnumerable<SelectListItem> DelEmpInfoList(int? id)
        {
            var PFOPBal = _context.HR_Emp_PF_OPBal
                  .Include(c => c.HR_Emp_Info)
                .FirstOrDefault(m => m.PFOPBalId == id);

            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join ep in _context.HR_Emp_PersonalInfo on emp.EmpId equals ep.EmpId
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid & ep.IsAllowPF == true
                           select new
                           {
                               Value = emp.EmpId,
                               Text = ep.PFFileNo + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            return new SelectList(empInfo, "Value", "Text", PFOPBal.EmpId);
        }

        public HR_Emp_PF_OPBal GetEmpPFOpBal(int? id)
        {
            var data = _context.HR_Emp_PF_OPBal
                  .Include(c => c.HR_Emp_Info)
                .FirstOrDefault(m => m.PFOPBalId == id);
            return data;
        }

        public List<HR_Emp_PF_OPBal> GetIndexInfo()
        {
            var data = _context.HR_Emp_PF_OPBal
                .Include(s => s.HR_Emp_Info)
                    .Include(x => x.PFChartOfAccountDebit)
                    .Include(x => x.PFChartOfAccountCredit)
                  .ToList();
            return data;
        }

        public IEnumerable<SelectListItem> PFOPBalYID(int? id)
        {
            var PFOPBal = _context.HR_Emp_PF_OPBal
                  .Include(c => c.HR_Emp_Info)
                .FirstOrDefault(m => m.PFOPBalId == id);

            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalYears
                .Where(x => x.ComId == ComId), "FiscalYearId", "FYName",
                PFOPBal.PFOPBalYID);
        }
    }
}
