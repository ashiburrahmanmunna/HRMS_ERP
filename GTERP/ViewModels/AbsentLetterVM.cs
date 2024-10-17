using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.ViewModels
{
    public class AbsentLetterVM
    {
        public int RefId { get; set; }
        public string ComId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }

        public string EmpName { get; set; }
        public DateTime DtJoin { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string DeptName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "From"),
            DataType(DataType.Date), DisplayFormat(DataFormatString = "0:dd-MMM-yyyy")]
        public DateTime? DtFrom { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "To"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:dd-MMM-yyyy")]
        public DateTime? DtTo { get; set; }
        [Required, StringLength(30)]
        public string ShowCauseLetterRef { get; set; }

        [StringLength(30)]
        public string SelfDefenceRef { get; set; }
        [StringLength(30)]
        public string TerminationLetterRef { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Date"),
        DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? DtFirst { get; set; }

        [Column(TypeName = "date"), Display(Name = "Date"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:dd-MMM-yyyy")]
        public DateTime? DtSecond { get; set; }
        [Column(TypeName = "date"), Display(Name = "Date"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:dd-MMM-yyyy")]
        public DateTime? DtThird { get; set; }

    }
}
