using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class LoanReturnRepository : BaseRepository<HR_Loan_Return>, ILoanReturnRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LoanReturnRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public IEnumerable<SelectListItem> CatVariableList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Variable
               .Where(v => v.ComId == comid && v.VarType == "LoanReturnType")
               .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public HR_Loan_Return CheckData(HR_Loan_Return LoanReturn)
        {
            return _context.HR_Loan_Return
                    .Where(s => s.EmpId == LoanReturn.EmpId && s.DtInput.Date.Month == LoanReturn.DtInput.Date.Month
                    && s.LoanType == LoanReturn.LoanType && s.LoanReturnId != LoanReturn.LoanReturnId).FirstOrDefault();
        }

        public List<HR_Emp_Info> EmpData()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .ToList();

        }

        public List<LoanReturn> LoadLoanReturnPartial(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var LoanReturns = _context.HR_Loan_Return
                .Include(s => s.HR_Emp_Info)
                .Include(s => s.HR_Emp_Info.Cat_Section)
                .Where(s => s.DtInput.Date.Month == date.Month && s.DtInput.Date.Year == date.Year && s.ComId == comid)
                .Select(s => new LoanReturn
                {
                    LoanReturnId = s.LoanReturnId,
                    EmpId = s.EmpId,
                    EmpCode = s.HR_Emp_Info.EmpCode,
                    EmpName = s.HR_Emp_Info.EmpName,
                    Section = s.HR_Emp_Info.Cat_Section.SectName,
                    Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                    DtJoin = s.DtJoin.ToString("dd-MMM-yyyy"),
                    DtInput = s.DtInput.ToString("dd-MMM-yyyy"),
                    Amount = s.Amount,
                    LoanType = s.LoanType
                }).ToList();
            return LoanReturns;
        }
    }
}
