using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models.Payroll
{
    public class EarnLeaveSheet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmpStatus { get; set; }
        public string Paymode { get; set; }

        public string UnitId { get; set; } //for id
        public string EmpTypeId { get; set; }
        public string ProssType { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }

        public string SectId { get; set; }
        public string SectName { get; set; }
        public string EmpCode { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string ReportType { get; set; }
        public string LId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string DesigId { get; set; } //for id
        public string SubSectId { get; set; } //for id
        public string FloorId { get; set; } //for id
        public string BankId { get; set; } //for id
        public string LineId { get; set; } //for id
        [Display(Name = @"View As: ")]
        public string ViewReportAs { get; set; }

        public EarnLeaveSheetGrid EarnLeaveSheetPropGrid { get; set; }
    }
    public class EarnLeaveSheetGrid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmpStatus { get; set; }
        public string Paymode { get; set; }

        public string UnitId { get; set; } //for id
        public string EmpTypeId { get; set; }
        public string ProssType { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string SectId { get; set; }
        public string SectName { get; set; }
        public string EmpCode { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }

        public string LId { get; set; }
        public string? BankId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DesigId { get; set; } //for id
        public string SubSectId { get; set; } //for id
        public string FloorId { get; set; } //for id
        public string LineId { get; set; } //for id
    }
}
