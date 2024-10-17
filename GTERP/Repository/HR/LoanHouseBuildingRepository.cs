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
    public class LoanHouseBuildingRepository : BaseRepository<HR_Loan_HouseBuilding>, ILoanHouseBuilding
    {
        private readonly GTRDBContext db;
        private readonly IHttpContextAccessor _httpContext;
        public LoanHouseBuildingRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            db = context;
            _httpContext = httpContext;
        }
        public List<HR_Loan_HouseBuilding> GetLoanHouseDataList()
        {
            var loanData = db.HR_Loan_HouseBuilding
               .Include(x => x.HR_Emp_Info)
               .Include(x => x.HR_Emp_Info.Cat_Department)
               .Include(x => x.HR_Emp_Info.Cat_Section)
               .Include(x => x.HR_Emp_Info.Cat_Designation)
               .Where(x => x.ComId == _httpContext.HttpContext.Session.GetString("comid")).ToList();
            return loanData;
        }


        public IEnumerable<SelectListItem> GetEmpHouseList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = new SelectList(db.HR_Emp_Info
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
        public IEnumerable<SelectListItem> CompoundHouseList()
        {
            return new SelectList(db.Cat_Variable
              .Where(v => v.VarType == "LoanCompound")
              .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public IEnumerable<SelectListItem> PayBackHouseList()
        {
            return new SelectList(db.Cat_Variable
              .Where(v => v.VarType == "LoanPayBack")
              .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public void SaveLoanHouseBuilding(HR_Loan_HouseBuilding hR_Loan_HouseBuilding, bool newCalculation)
        {
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            hR_Loan_HouseBuilding.UpdateByUserId = userid;
            hR_Loan_HouseBuilding.ComId = comid;
            hR_Loan_HouseBuilding.DateUpdated = DateTime.Now.Date;
            db.Entry(hR_Loan_HouseBuilding).State = EntityState.Modified;
            if (!newCalculation)
            {
                foreach (var item in hR_Loan_HouseBuilding.HR_Loan_Data_HouseBuildings)
                {
                    //item.LoanHouseId = hR_Loan_HouseBuilding.LoanHouseId;
                    db.Entry(item).State = EntityState.Modified;
                }
            }
            else
            {
                var newData = new List<HR_Loan_Data_HouseBuilding>();
                foreach (var item in hR_Loan_HouseBuilding.HR_Loan_Data_HouseBuildings)
                {
                    item.LoanHouseId = hR_Loan_HouseBuilding.LoanHouseId;
                    newData.Add(item);
                }
                var exist = db.HR_Loan_Data_HouseBuilding.Where(l => l.LoanHouseId == hR_Loan_HouseBuilding.LoanHouseId).ToList();

                if (exist.Count > 0)
                    db.RemoveRange(exist);

                if (newData.Count > 0)
                    db.AddRange(newData);
            }
        }

        public HR_Loan_Data_HouseBuilding unPaidHouse(HR_Loan_HouseBuilding hR_Loan_HouseBuilding)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = db.HR_Loan_Data_HouseBuilding
             .Where(l => l.EmpId == hR_Loan_HouseBuilding.EmpId && l.IsPaid == false && l.ComId == comid)
             .FirstOrDefault();
            return data;
        }

        public List<CalculateData> CalcualteLoanHousePartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
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
            List<CalculateData> loanCal = Helper.ExecProcMapTList<CalculateData>("HR_LoanProcessHouseBuilding", sqlParameter);
            return loanCal;
        }

        public List<HR_Loan_Data_HouseBuilding> LoanHouseCalc(int id)
        {
            var loandMaster = db.HR_Loan_HouseBuilding
               .Include(l => l.HR_Emp_Info)
               .Include(l => l.HR_Emp_Info.Cat_Department)
               .Include(l => l.HR_Emp_Info.Cat_Designation)
               .Include(l => l.HR_Emp_Info.HR_Emp_Image)
               .Where(l => l.EmpId == id).FirstOrDefault();

            var data = db.HR_Loan_Data_HouseBuilding
                   .Where(l => l.LoanHouseId == loandMaster.LoanHouseId)
                   .OrderBy(l => l.DtLoanMonth).ToList();
            return data;
        }

        public HR_Loan_HouseBuilding loanHouseMaster(int id)
        {
            var data = db.HR_Loan_HouseBuilding
                 .Include(l => l.HR_Emp_Info)
                 .Include(l => l.HR_Emp_Info.Cat_Department)
                 .Include(l => l.HR_Emp_Info.Cat_Designation)
                 .Include(l => l.HR_Emp_Info.HR_Emp_Image)
                 .Where(l => l.EmpId == id).FirstOrDefault();
            return data;
        }

        public void DeleteHouseBuilding(int id)
        {

            var detailsLoan = db.HR_Loan_Data_HouseBuilding.Where(l => l.LoanHouseId == id).ToList();
            db.HR_Loan_Data_HouseBuilding.RemoveRange(detailsLoan);
            var master = db.HR_Loan_HouseBuilding.Where(l => l.LoanHouseId == id).FirstOrDefault();
            db.HR_Loan_HouseBuilding.Remove(master);
            db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetEmpList(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.HR_Emp_Info
                    .Include(x => x.Cat_Designation)
                    .Include(x => x.Cat_Department)
                    .Include(x => x.Cat_Section)
                    .Where(s => s.ComId == comid)
                    .Select(s => new
                    {
                        Text = "-" + s.EmpCode + " - [ " + s.EmpName + " ]"
                        + " - [ " + s.Cat_Designation.DesigName + " ]"
                        + " - [ " + s.Cat_Department.DeptName + " ]"
                        + " - [ " + s.Cat_Section.SectName + " ]",
                        Value = s.EmpId
                    })
                    .ToList(), "Value", "Text", id);
        }
    }
}
