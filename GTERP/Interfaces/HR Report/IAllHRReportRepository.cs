using GTERP.Models;
using GTERP.ViewModels;
using System;

namespace GTERP.Interfaces.HR_Report
{
    public interface IAllHRReportRepository
    {
        public String JobCard(JobCardVM jobCard);
        public String JobCardB(JobCardVM jobCardB);
        public String JobCard4h(JobCardVM jobCard4h);
        public String DynamicJobCard(JobCardVM dynamicJobCard);
        public String LeaveReport(LeaveReportVM aLeaveReport);
        public String MonthlyAttendance(MonthlyAttendanceVM aMonthlyAttendance);
        public String ProdReport(ProdVM prod);
        public string LoanReport(LoanReportVM aLoanReport);
        public string IncrementReport(IncrementReportVM aIncrementReport);
        public String DailyAttendance(DailyAttendanceVM aDailyAttendAnce);
        public String EmployeeReport(EmployeeReportVM aEmployeeReport);
    }
}
