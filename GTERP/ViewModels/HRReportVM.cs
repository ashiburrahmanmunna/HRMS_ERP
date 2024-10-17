using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.ViewModels
{

    #region JobCard, JobCardB, JobCard4h
    public class JobCardVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string LId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string Format { get; set; }

        public JobCardGrid JobCardGrid { get; set; }

    }
    public class JobCardGrid
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string LId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string Format { get; set; }
    }

    #endregion

    #region Leave Report
    public class LeaveReportVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string LeaveTypeId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }

        public LeaveProdGrid LeaveProdGrid { get; set; }
    }
    public class LeaveProdGrid
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpTypeId { get; set; }
        public string SubSectionId { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string LeaveTypeId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }

    }

    #endregion

    #region Production Report
    public class ProdVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string StyleId { get; set; }
        
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        
        public ProdGridVM ProdGridVM { get; set; }
    }

    public class ProdGridVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string StyleId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
    }
    #endregion
    public class LoanReportVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int SectionId { get; set; }
        public int EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public int LId { get; set; }
        public string ReportName { get; set; }
        public string Format { get; set; }
    }

    #region Increment Report
    public class IncrementReportVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }

        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }

        public IncrementProdGrid IncrementProdGrid { get; set; }
    }
    public class IncrementProdGrid
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpTypeId { get; set; }
        public string SubSectionId { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
     
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }

    }
    #endregion

    #region Employee Report
    public class EmployeeReportVM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Criteria { get; set; }
        public string EmpTypeId { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string DesigId { get; set; }
        public string DesigName { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string ShiftName { get; set; }
        public string SectId { get; set; }
        public string SectName { get; set; }
        public string SubSectId { get; set; }
        public string SubSectName { get; set; }
        public string EmpCode { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LineId { get; set; }
        public string UnitId { get; set; }
        public int GenderId { get; set; }
        public string VarId { get; set; }

        public string ReportName { get; set; }
        [Display(Name = @"View As: ")]
        public string ViewReportAs { get; set; }


        public EmployeeReportGrid EmployeeReportPropGrid { get; set; }


    }
    public class EmployeeReportGrid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Criteria { get; set; }
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string EmpTypeId { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string DesigId { get; set; }
        public string DesigName { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string SectId { get; set; }
        public string SectName { get; set; }
        public string SubSectId { get; set; }
        public string SubSectName { get; set; }
        public string EmpCode { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LineId { get; set; }
        public string UnitId { get; set; }
        public int GenderId { get; set; }
        public string VarId { get; set; }
        public string ReportName { get; set; }

    }
    #endregion

    #region Monthly Attendance Report
    public class MonthlyAttendanceVM
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string SubSectionId { get; set; }
        public string EmpId { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpName { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string LId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string Format { get; set; }

        public MonthlyAttendanceProdGrid MonthlyAttendanceProdGrid { get; set; }
    }
    public class MonthlyAttendanceProdGrid
    {
        public string Criteria { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpTypeId { get; set; }
        public string SubSectionId { get; set; }
        public string UnitId { get; set; }
        public string ShiftId { get; set; }
        public string FloorId { get; set; }
        public string LineId { get; set; }
        public string LId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string Format { get; set; }
        public string EmpStatus { get; set; }

    }

    #endregion

    #region Daily Attendance Report
    public class DailyAttendanceVM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Criteria { get; set; }
        public string EmptypeId { get; set; } // for id
        public string DeptId { get; set; } // for id
        public string DeptName { get; set; }
        public string DesigId { get; set; } // for id
        public string DesigName { get; set; }
        public string ShiftId { get; set; } // for id
        public string FloorId { get; set; } // for id
        public string FloorName { get; set; }
        public string ShiftName { get; set; }
        public string SectId { get; set; } // for id
        public string SectName { get; set; }
        public string SubSectId { get; set; } // for id
        public string SubSectName { get; set; }
        public string EmpCode { get; set; }
        public string EmpId { get; set; } // for id
        public string EmpName { get; set; }
        public string ReportType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public double? Beginning { get; set; }
        public double? End { get; set; }
        public string LineId { get; set; } // for id
        public string UnitId { get; set; } // for id
        [Display(Name = @"View As: ")]
        public string ViewReportAs { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = @"{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? From { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = @"{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? To { get; set; }

        public DailyAttendanceGrid DailyAttendancePropGrid { get; set; }


    }
    public class DailyAttendanceGrid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Criteria { get; set; }
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string EmptypeId { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string DesigId { get; set; }
        public string DesigName { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string SectId { get; set; }
        public string SectName { get; set; }
        public string SubSectId { get; set; }
        public string SubSectName { get; set; }
        public string EmpCode { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LineId { get; set; }
        public string UnitId { get; set; }
    }
    #endregion
}
