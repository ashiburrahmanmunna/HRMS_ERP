using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
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
    public class LoanOtherRepository : BaseRepository<HR_Loan_Other>, ILoanOtherRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LoanOtherRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<CalculateData> CalcualteLoanOtherPartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
        {
            SqlParameter[] sqlParameter = new SqlParameter[8];
            sqlParameter[0] = new SqlParameter("@LoanAmount", lAmount);
            sqlParameter[1] = new SqlParameter("@InterestRate", interest);
            sqlParameter[2] = new SqlParameter("@LoanPeriod", period);
            sqlParameter[3] = new SqlParameter("@StartPaymentDate", startDate);
            sqlParameter[4] = new SqlParameter("@TotalLoanAmount", ttlLoanAmt);
            sqlParameter[5] = new SqlParameter("@TotalInterest", ttlInterest);
            sqlParameter[6] = new SqlParameter("@MonthlyLoan", monthlyLoanAmt);
            sqlParameter[7] = new SqlParameter("@LoanType", loanType);
            List<CalculateData> loanCal = Helper.ExecProcMapTList<CalculateData>("HR_LoanProcessOther", sqlParameter);

            return loanCal;
        }

        public IEnumerable<SelectListItem> CompoundOtherList()
        {

            return new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "LoanCompound")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
        }

        public void DeleteOtherCycle(int id)
        {
            var detailsLoan = _context.HR_Loan_Data_Other.Where(l => l.LoanOtherId == id).ToList();
            _context.HR_Loan_Data_Other.RemoveRange(detailsLoan);
            var master = _context.HR_Loan_Other.Where(l => l.LoanOtherId == id).FirstOrDefault();
            _context.HR_Loan_Other.Remove(master);
            _context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetEmpOtherList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = new SelectList(_context.HR_Emp_Info
                     .Include(x => x.Cat_Department)
                     .Include(x => x.Cat_Designation)
                     .Include(x => x.Cat_Section)
                     .Where(s => s.ComId == comid && s.IsDelete == false)
                     .Select(s => new
                     {
                         Text = "-" + s.EmpCode + " - [ " + s.EmpName + " ]"
                     + " - [ " + s.Cat_Designation.DesigName + " ]"
                     + " - [ " + s.Cat_Department.DeptName + " ]"
                     + " - [ " + s.Cat_Section.SectName + " ]",
                         Value = s.EmpId
                     })
                     .ToList(), "Value", "Text");
            return empInfo;
        }

        public List<HR_Loan_Other> GetLoanOtherDataList()
        {
            var data = _context.HR_Loan_Other
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Department)
                .Include(x => x.HR_Emp_Info.Cat_Section)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Where(x => x.ComId == _httpContext.HttpContext.Session.GetString("comid") && x.IsDelete == false).ToList();
            return data;
        }

        public List<HR_Loan_Data_Other> LoanOtherCalc(int id)
        {
            var loandMaster = _context.HR_Loan_Other
             .Include(l => l.HR_Emp_Info)
             .Include(l => l.HR_Emp_Info.Cat_Department)
             .Include(l => l.HR_Emp_Info.Cat_Designation)
             .Include(l => l.HR_Emp_Info.HR_Emp_Image)
             .Where(l => l.LoanOtherId == id).FirstOrDefault();
            var data = _context.HR_Loan_Data_Other
                     .Where(l => l.LoanOtherId == loandMaster.LoanOtherId).OrderBy(l => l.DtLoanMonth).ToList();
            return data;
        }

        public HR_Loan_Other loanOtherMaster(int id)
        {
            var data = _context.HR_Loan_Other
                 .Include(l => l.HR_Emp_Info)
                 .Include(l => l.HR_Emp_Info.Cat_Department)
                 .Include(l => l.HR_Emp_Info.Cat_Designation)
                 .Include(l => l.HR_Emp_Info.HR_Emp_Image)
                 .Where(l => l.LoanOtherId == id).FirstOrDefault();
            return data;
        }

        public IEnumerable<SelectListItem> LoanTypeList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "LoanType")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
        }

        public IEnumerable<SelectListItem> PayBackOtherList()
        {

            return new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "LoanPayBack")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
        }

        public void SaveLoanOther(HR_Loan_Other hR_Loan_Other, bool newCalculation)
        {
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            hR_Loan_Other.UpdateByUserId = userid;
            hR_Loan_Other.DateUpdated = DateTime.Now.Date;
            _context.Entry(hR_Loan_Other).State = EntityState.Modified;
            if (!newCalculation)
            {
                foreach (var item in hR_Loan_Other.HR_Loan_Data_Others)
                {
                    //item.LoanOtherId = hR_Loan_Other.LoanOtherId;
                    _context.Entry(item).State = EntityState.Modified;
                }
            }
            else
            {
                var newData = new List<HR_Loan_Data_Other>();
                foreach (var item in hR_Loan_Other.HR_Loan_Data_Others)
                {
                    item.LoanOtherId = hR_Loan_Other.LoanOtherId;
                    newData.Add(item);
                }
                var exist = _context.HR_Loan_Data_Other.Where(l => l.LoanOtherId == hR_Loan_Other.LoanOtherId).ToList();

                if (exist.Count > 0)
                    _context.RemoveRange(exist);

                if (newData.Count > 0)
                    _context.AddRange(newData);
            }
        }

        public HR_Loan_Data_Other unPaidOther1(HR_Loan_Other hR_Loan_Other)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var master = _context.HR_Loan_Other.Where(l => l.EmpId == hR_Loan_Other.EmpId && l.LoanType == hR_Loan_Other.LoanType
                                && l.LoanOtherId != hR_Loan_Other.LoanOtherId && l.ComId == comid).FirstOrDefault();
            var data = _context.HR_Loan_Data_Other
                            .Where(l => l.LoanOtherId == master.LoanOtherId && l.IsPaid == false)
                            .FirstOrDefault();
            return data;
        }

        public HR_Loan_Data_Other unPaidOther2(HR_Loan_Other hR_Loan_Other)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var master = _context.HR_Loan_Other.Where(l => l.EmpId == hR_Loan_Other.EmpId
                                     && l.LoanType == hR_Loan_Other.LoanType && l.ComId == comid).FirstOrDefault();
            var data = _context.HR_Loan_Data_Other
                            .Where(l => l.LoanOtherId == master.LoanOtherId && l.IsPaid == false)
                            .FirstOrDefault();
            return data;
        }
    }
}
