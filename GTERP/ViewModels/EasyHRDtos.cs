using System.Collections.Generic;

namespace GTERP.ViewModels
{
    public class MenuList
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }

    public class AppInfoRespone
    {
        public int EmployeeId { get; set; }
        public int SectionId { get; set; }
        public string FingerId { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string ProfilePicture { get; set; }
        public string CompanyImage { get; set; }
        public List<MenuList> MenuList { get; set; }
    }

    public class AttendenceHistory
    {
        public string PunchDate { get; set; }
        public string PunchTime { get; set; }
        public string LocationName { get; set; }
    }

    public class JobCardResponse
    {
        public string Present { get; set; }
        public string Absent { get; set; }
        public string LateDay { get; set; }
        public string HoliDay { get; set; }
        public string Weekend { get; set; }
        public string OTHourTotal { get; set; }
        public string Leave { get; set; }
        public List<AttendencList> AttendanceList { get; set; }
    }

    public class AttendencList
    {
        public string PunchDate { get; set; }
        public string Status { get; set; }
        public string Late { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string OTHour { get; set; }
    }

    public class JobCardList
    {
        public string dtPunchDate { get; set; }
        public string Leave { get; set; }
        public string Status { get; set; }
        public string Late { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string Present { get; set; }
        public string Absent { get; set; }
        public string LateDay { get; set; }
        public string HDay { get; set; }
        public string WDay { get; set; }
        public string OTHrsTtl { get; set; }
        public string OTHour { get; set; }
    }

    public class ProcessType
    {
        public string ProssType { get; set; }
    }

    public class PaySlip
    {
        public string GrossSalay { get; set; }          // GS
        public string BasicSalary { get; set; }         // BS
        public string HouseRent { get; set; }           // HR
        public string Stamp { get; set; }               // Stamp
        public string MedicalAllowance { get; set; }    // MA
        public string FoodAllowance { get; set; }       // FoodAllow	
        public string Conveyance { get; set; }          // ConvAllow	
        public string AttendanceBonus { get; set; }     // AB
        public string ProvidentFund { get; set; }       // PF
        public string OthersDeduct { get; set; }        // OthersDeduct
        public string AbsentDeduction { get; set; }     // ADV
        public string TotalDeduct { get; set; }         // TotalDeduct
        public string TotalPayable { get; set; }        // TotalPayable
        public string NetSalary { get; set; }           // NetSalary
        public string NetSalaryPayable { get; set; }    // NetSalaryPayable

    }

    public class GetPaySlip
    {
        public string GS { get; set; }
        public string BS { get; set; }
        public string HR { get; set; }
        public string Stamp { get; set; }
        public string MA { get; set; }
        public string FoodAllow { get; set; }
        public string ConvAllow { get; set; }
        public string AttBonus { get; set; }
        public string PF { get; set; }
        public string OthersDeduct { get; set; }
        public string ADV { get; set; }
        public string TotalDeduct { get; set; }
        public string TotalPayable { get; set; }
        public string NetSalary { get; set; }
        public string NetSalaryPayable { get; set; }

    }

    public class LeaveType
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
    }

    public class LeaveInfoResponse
    {
        public float TotalCL { get; set; }
        public float EnjoyedCL { get; set; }
        public float TotalSL { get; set; }
        public float EnjoyedSL { get; set; }
        public float TotalEL { get; set; }
        public float EnjoyedEL { get; set; }
        public List<LeaveType> LeaveTypesList { get; set; }
    }

    public class LeaveApplications
    {
        public int LeaveId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveInputDate { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string TotalDay { get; set; }
        public string LeaveType { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public bool IsApprove { get; set; }
    }

}

