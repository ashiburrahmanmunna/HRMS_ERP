using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblFestAdvSalary
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public short DeptId { get; set; }
        public short SectId { get; set; }
        public short SubSectId { get; set; }
        public short DesigId { get; set; }
        public string Band { get; set; }
        public DateTime? DtJoin { get; set; }
        public short Year { get; set; }
        public string Month { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
        public string FestivalType { get; set; }
        public string SalaryType { get; set; }
        public string EmpType { get; set; }
        public string Religion { get; set; }
        public short SerYear { get; set; }
        public short SerMonth { get; set; }
        public short SerDay { get; set; }
        public int SerTtlDay { get; set; }
        public byte IsWithSalary { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public decimal Amount { get; set; }
        public byte Stamp { get; set; }
        public decimal NetPayable { get; set; }
        public byte Cf { get; set; }
        public int TtlAmt { get; set; }
        public short Tk1000 { get; set; }
        public short Tk500 { get; set; }
        public short Tk100 { get; set; }
        public short Tk50 { get; set; }
        public short Tk20 { get; set; }
        public short Tk10 { get; set; }
        public short Tk5 { get; set; }
        public short Tk2 { get; set; }
        public short Tk1 { get; set; }
        public bool EditYn { get; set; }
        public string Remarks { get; set; }
        public float Per { get; set; }
    }
}
