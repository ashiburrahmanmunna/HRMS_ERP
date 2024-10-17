using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models.Payroll
{
    public class LoanReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmpStatus { get; set; }
        public string Paymode { get; set; }

        public string Unit { get; set; }
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

        [Display(Name = @"View As: ")]
        public string ViewReportAs { get; set; }



        public LoanReportGrid LoanReportPropGrid { get; set; }


    }
    public class LoanReportGrid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmpStatus { get; set; }
        public string Paymode { get; set; }

        public string Unit { get; set; }
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
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
