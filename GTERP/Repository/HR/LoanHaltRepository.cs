using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class LoanHaltRepository : ILoanHaltRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LoanHaltRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<EmpViewModel> GetEmployee()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var employees = _context.HR_Emp_Info
                 .Include(h => h.Cat_Department)
                 .Include(h => h.Cat_Designation)
                 .Include(h => h.Cat_Section)
                 .Select(e => new EmpViewModel
                 {
                     EmpId = e.EmpId,
                     ComId = e.ComId,
                     EmpName = e.EmpName,
                     EmpCode = e.EmpCode,
                     DesigName = e.Cat_Designation.DesigName,
                     SectName = e.Cat_Section.SectName,
                     DeptName = e.Cat_Department.DeptName
                 }).ToList();

            return employees;
        }

        public void LoanCreate(LoanHalt loanHalt)
        {

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            if (loanHalt.OtherLoanType == null)
                loanHalt.OtherLoanType = "";


            if (loanHalt.Criteria == "All")
            {
                var lInc = new HR_Loan_IncreaseInfo();
                lInc.LoanType = loanHalt.LoanType;
                lInc.InputType = loanHalt.Criteria;
                lInc.OtherLoanType = loanHalt.OtherLoanType;
                lInc.TtlIncreaseMonth = loanHalt.IncreaseMonth;
                lInc.DtEffectiveMonth = loanHalt.EffectiveMonth;
                lInc.Remarks = loanHalt.Remarks;
                //lInc.EmpId = loanHalt.Employess[i];
                lInc.UserId = userid;
                lInc.ComId = comid;
                lInc.DateAdded = DateTime.Now;
                lInc.EmpId = 0;
                _context.HR_Loan_IncreaseInfo.Add(lInc);
                _context.SaveChanges();

                SqlParameter[] sqlParameters = new SqlParameter[8];
                sqlParameters[0] = new SqlParameter("@comid", comid);
                sqlParameters[1] = new SqlParameter("@id", 0);
                sqlParameters[2] = new SqlParameter("@LoanType", loanHalt.LoanType);
                sqlParameters[3] = new SqlParameter("@TtlLoanIncreaseMonth", loanHalt.IncreaseMonth);
                sqlParameters[4] = new SqlParameter("@EffectiveMonth", loanHalt.EffectiveMonth);
                sqlParameters[5] = new SqlParameter("@Remarks", loanHalt.Remarks);

                //sqlParameters[1] = new SqlParameter("@id", 0);
                sqlParameters[6] = new SqlParameter("@EmpId", 0);
                sqlParameters[7] = new SqlParameter("@LoanOtherType", loanHalt.OtherLoanType);
                Helper.ExecProc("prcProcessLoanTermIncrease", sqlParameters);

            }
            else if (loanHalt.Criteria == "Employee")
            {
                for (int i = 0; i < loanHalt.Employess.Count; i++)
                {
                    var lInc = new HR_Loan_IncreaseInfo();
                    lInc.LoanType = loanHalt.LoanType;
                    lInc.InputType = loanHalt.Criteria;
                    lInc.OtherLoanType = loanHalt.OtherLoanType;
                    lInc.TtlIncreaseMonth = loanHalt.IncreaseMonth;
                    lInc.DtEffectiveMonth = loanHalt.EffectiveMonth;
                    lInc.Remarks = loanHalt.Remarks;
                    //lInc.EmpId = loanHalt.Employess[i];
                    lInc.UserId = userid;
                    lInc.ComId = comid;
                    lInc.DateAdded = DateTime.Now;
                    lInc.EmpId = loanHalt.Employess[i];

                    _context.HR_Loan_IncreaseInfo.Add(lInc);
                    _context.SaveChanges();

                    SqlParameter[] sqlParameters = new SqlParameter[8];
                    sqlParameters[0] = new SqlParameter("@comid", comid);
                    sqlParameters[1] = new SqlParameter("@id", 1);
                    sqlParameters[2] = new SqlParameter("@LoanType", loanHalt.LoanType);
                    sqlParameters[3] = new SqlParameter("@TtlLoanIncreaseMonth", loanHalt.IncreaseMonth);
                    sqlParameters[4] = new SqlParameter("@EffectiveMonth", loanHalt.EffectiveMonth);
                    sqlParameters[5] = new SqlParameter("@Remarks", loanHalt.Remarks);
                    sqlParameters[6] = new SqlParameter("@EmpId", loanHalt.Employess[i]);
                    sqlParameters[7] = new SqlParameter("@LoanOtherType", loanHalt.OtherLoanType);

                    Helper.ExecProc("prcProcessLoanTermIncrease", sqlParameters);
                }
            }


        }
        public IEnumerable<SelectListItem> LoanTypeCat_Variable()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var items = new SelectList(_context.Cat_Variable
                .Where(v => v.ComId == comid && v.VarType == "LoanHalt")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
            return items;
        }
        public IEnumerable<SelectListItem> OtherTypeCat_Variable()
        {

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var OtherLoanType = new SelectList(_context.Cat_Variable
                     .Where(v => v.ComId == comid && v.VarType == "LoanType")
                     .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
            return OtherLoanType;
        }
    }

}
