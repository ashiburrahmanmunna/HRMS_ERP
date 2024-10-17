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
    public class LoanMotorCycleRepository : BaseRepository<HR_Loan_MotorCycle>, ILoanMotorCycleRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LoanMotorCycleRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public List<HR_Loan_MotorCycle> GetLoanMotorDataList()
        {
            var loanData = _context.HR_Loan_MotorCycle
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Department)
                .Include(x => x.HR_Emp_Info.Cat_Section)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Where(x => x.ComId == _httpContext.HttpContext.Session.GetString("comid")).ToList();
            return loanData;
        }


        public IEnumerable<SelectListItem> GetEmpMotorList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = new SelectList(_context.HR_Emp_Info
                     .Include(x => x.Cat_Department)
                     .Include(x => x.Cat_Designation)
                     .Include(x => x.Cat_Section)
                     .Where(s => s.ComId == comid)
                     .Select(s => new
                     {
                         Text = s.EmpName + " - [ " + s.EmpCode + " ]"
                     + " - [ " + s.Cat_Designation.DesigName + " ]"
                     + " - [ " + s.Cat_Department.DeptName + " ]"
                     + " - [ " + s.Cat_Section.SectName + " ]",
                         Value = s.EmpId
                     })
                     .ToList(), "Value", "Text");
            return empInfo;
        }
        public IEnumerable<SelectListItem> CompoundMotorList()
        {

            return new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "LoanCompound")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public IEnumerable<SelectListItem> PayBackMotorList()
        {
            return new SelectList(_context.Cat_Variable
              .Where(v => v.VarType == "LoanPayBack")
              .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public void SaveLoanMotorCycle(HR_Loan_MotorCycle HR_Loan_MotorCycle, bool newCalculation)
        {
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            HR_Loan_MotorCycle.UpdateByUserId = userid;
            HR_Loan_MotorCycle.ComId = comid;
            HR_Loan_MotorCycle.DateUpdated = DateTime.Now.Date;
            _context.Entry(HR_Loan_MotorCycle).State = EntityState.Modified;
            if (!newCalculation)
            {
                foreach (var item in HR_Loan_MotorCycle.HR_Loan_Data_MotorCycles)
                {
                    //item.LoanHouseId = HR_Loan_MotorCycle.LoanHouseId;
                    _context.Entry(item).State = EntityState.Modified;
                }
            }
            else
            {
                var newData = new List<HR_Loan_Data_MotorCycle>();
                foreach (var item in HR_Loan_MotorCycle.HR_Loan_Data_MotorCycles)
                {
                    item.LoanMotorId = HR_Loan_MotorCycle.LoanMotorId;
                    newData.Add(item);
                }
                var exist = _context.HR_Loan_MotorCycle.Where(l => l.LoanMotorId == HR_Loan_MotorCycle.LoanMotorId).ToList();

                if (exist.Count > 0)
                    _context.RemoveRange(exist);

                if (newData.Count > 0)
                    _context.AddRange(newData);
            }
        }

        public HR_Loan_Data_MotorCycle unPaidMotor(HR_Loan_MotorCycle HR_Loan_MotorCycle)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.HR_Loan_Data_MotorCycle
             .Where(l => l.EmpId == HR_Loan_MotorCycle.EmpId && l.IsPaid == false && l.ComId == comid)
             .FirstOrDefault();
            return data;
        }

        public List<CalculateData> CalcualteLoanMotorPartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
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
            List<CalculateData> loanCal = Helper.ExecProcMapTList<CalculateData>("HR_LoanProcessMotorCycle", sqlParameter);
            return loanCal;
        }

        public List<HR_Loan_Data_HouseBuilding> LoanMotorCalc(int id)
        {
            var loandMaster = _context.HR_Loan_MotorCycle
               .Include(l => l.HR_Emp_Info)
               .Include(l => l.HR_Emp_Info.Cat_Department)
               .Include(l => l.HR_Emp_Info.Cat_Designation)
               .Include(l => l.HR_Emp_Info.HR_Emp_Image)
               .Where(l => l.EmpId == id).FirstOrDefault();

            var data = _context.HR_Loan_Data_HouseBuilding
                   .Where(l => l.LoanHouseId == loandMaster.LoanMotorId)
                   .OrderBy(l => l.DtLoanMonth).ToList();
            return data;
        }

        public HR_Loan_MotorCycle loanMotorMaster(int id)
        {
            var data = _context.HR_Loan_MotorCycle
                 .Include(l => l.HR_Emp_Info)
                 .Include(l => l.HR_Emp_Info.Cat_Department)
                 .Include(l => l.HR_Emp_Info.Cat_Designation)
                 .Include(l => l.HR_Emp_Info.HR_Emp_Image)
                 .Where(l => l.EmpId == id).FirstOrDefault();
            return data;
        }

        public void DeleteMotorCycle(int id)
        {

            var detailsLoan = _context.HR_Loan_Data_MotorCycle.Where(l => l.LoanMotorId == id).ToList();
            _context.HR_Loan_Data_MotorCycle.RemoveRange(detailsLoan);
            var master = _context.HR_Loan_MotorCycle.Where(l => l.LoanMotorId == id).FirstOrDefault();
            _context.HR_Loan_MotorCycle.Remove(master);
            _context.SaveChanges();
        }
    }
}
