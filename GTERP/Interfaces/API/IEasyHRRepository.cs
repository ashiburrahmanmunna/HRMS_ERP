using GTERP.Models;
using GTERP.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTERP.Interfaces.API
{
    public interface IEasyHRRepository
    {
        public AppInfoRespone GetAppInfos(string email, int softwareId, int versionId);
        public Task<bool> SetAttendance(HR_RawData_App attendanceData);
        public List<AttendenceHistory> GetAttendenceHistories(int employeeId, string date);
        public JobCardResponse GetJobCard(string comId, int employeeId, int sectionId, string fromDate, string toDate);
        public List<ProcessType> GetProcessType(string comId);
        public PaySlip GetPaySlip(int employeeId, string processType);
        public LeaveInfoResponse GetLeaveInfo(int employeeId);
        public Task<bool> ApplyLeave(HR_Leave_Avail leaveApplication);
        public List<LeaveApplications> GetPendingLeaveApplications(string comId, int userId);
        public List<LeaveApplications> GetPendingLeaveApplications(string comId);
        public Task<bool> LeaveApproval(int leaveId, string approvalType);
        public Task<bool> FinalLeaveApproval(int leaveId, string approvalType);
        public List<LeaveApplications> GetFinalLeaveApplications(string comId, int userId);
        public List<LeaveApplications> GetFinalLeaveApplications(string comId);
        public List<LeaveApplications> GetEmployeeLeaveApplications(int employeeId);
        public string GetAttendanceTypeByEmpId(int empId);
    }
}
