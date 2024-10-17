using DocumentFormat.OpenXml.Drawing;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;


namespace GTERP.Interfaces.HR
{
    public interface IHRRepository
    {
        #region Active User Dashboard

        List<ActiveUser> ActiveEmpList(DateTime date);
        List<ActiveUser> ActiveEmpMale(DateTime date);
        List<ActiveUser> ActiveEmpFemale(DateTime date);
        List<ActiveUser> ActiveEmpOTYes(DateTime date);
        List<ActiveUser> ActiveEmpRelease(DateTime date);
        List<ActiveUser> TotalEmployees(DateTime date);
        #endregion

        #region Daily Attendance Active Dashboard
        List<DailyAttendanceActive> Present(DateTime date);
        List<DailyAttendanceActive> Absent(DateTime date);
        List<DailyAttendanceActive> Late(DateTime date);
        List<DailyAttendanceActive> Leave(DateTime date);
        List<DailyAttendanceActive> WHDay(DateTime date);
        #endregion

        #region Dashboard
        Dashboard LoadData(string dtLoad);
        Dashboard InitializeDashBord(string date = null);
        List<DailyAttendanceSum> PrcGetDailyAttendanceSum(string comid, string fromdate, string toDate);
        DailyAttendance PrcGetDailyAttendance(string date = null);
        MonthlyAttendance PrcGetMonthlyAttendance(string date = null);
        EmployeeDetails PrcGetEmployeeDetails(string comid, string dtPunchDate);
        SalaryDetails PrcGetSalaryDetails(string comid, string dtPunchDate);
        List<DailyAttendanceDepartmentWiseData> GetDepartmentWiseData(DateTime Date, string sectName);
        List<SalarySummeryDetails> PrcGetSalarySummery(string comid);
        List<SalaryCheck> GetSalarySummaryData(string sectname);
        List<DailyCostSummary> GetDailyCost(DateTime Date);
        List<DailyCostDetails> GetDailyCostDetails(DateTime Date, string sectname);
        List<TotalEmpType> GetTotalEmpType(string date);



        #endregion

        #region Employee List
        List<EmpList> EmpListIndex(string criteria,int Offset,int fetch, string EmpCode, string EmpName, DateTime startDate, DateTime endDate);

        List<EmpListCount> EmpListIndexCount(string criteria, int Offset, int fetch, string EmpCode, string EmpName, DateTime startDate, DateTime endDate);
        List<EmpProfile> GetEmpProfile(DateTime dtFrom, DateTime dtTo, int Id);

        List<AttendanceDetails> GetAttendanceDetails(string Period, DateTime dtFrom, DateTime dtTo, int Id);

        List<LeaveDetails> GetLeaveDetails(int year, int Id);

        List<PaymentDetails> GetPaymentDetails(int Id);

        List<ShowCauseDetails> GetShowCauseDetails(int Id);
        List<NomineeDetails> GetNomineeDetails(int Id);
        List<LoanDetails> GetLoanDetails(int Id);
        List<SalaryStructure> GetSalStrucDetails(int Id);
        List<TaxDetails> GetTaxDetails(int Id);
        public String EmpListPrint(int? id, string type = "pdf");

        public void Raw_DataTransfer(DateTime From, DateTime To);

        #endregion

        #region Chart View Model

        List<MonthlyOTRenderChartVM> MonthlyOTChart(DateTime Date);
        List<DailyOTRenderChartVM> DailyOTChart(DateTime Date);
        List<MonthlyJoinReleaseVM> MonthlyJReleasedEmp(DateTime Date);
        List<ManPowerHistoryVM> ManPowerHistoryEmp(DateTime Date);
        List<DailyPresentRenderChartVM> DailyPresentChart(DateTime Date);
        List<DailyPresentRatioChartVM> DailyPresentRatioChart(DateTime Date);
        List<DeptWiseManPowerVM> DeptWiseEmployeeChart(DateTime Date);

        #endregion


    }
}
