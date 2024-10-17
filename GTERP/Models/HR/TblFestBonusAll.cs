using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblFestBonusAll
    {
        public byte ComId { get; set; }
        public short Year { get; set; }
        public string Month { get; set; }
        public long? BonusId { get; set; }
        public DateTime? DtProcess { get; set; }
        public DateTime? DtBonus { get; set; }
        public string BonusType { get; set; }
        public string EmpType { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public short DeptId { get; set; }
        public short SectId { get; set; }
        public int? SubSectId { get; set; }
        public short DesigId { get; set; }
        public string Grade { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public short SectIdSal { get; set; }
        public string InputType { get; set; }
        public decimal Amount { get; set; }
        public byte Stamp { get; set; }
        public decimal NetPayable { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public float Tolarance { get; set; }
        public float Workdays { get; set; }
        public string Fyear { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public float Days { get; set; }
        public string Religion { get; set; }
        public byte IsWithSalary { get; set; }
        public string Description1 { get; set; }
        public string Remarks { get; set; }
        public string Criteria { get; set; }
        public byte Tk1000 { get; set; }
        public byte Tk500 { get; set; }
        public byte Tk100 { get; set; }
        public byte Tk50 { get; set; }
        public byte Tk20 { get; set; }
        public byte Tk10 { get; set; }
        public byte Tk5 { get; set; }
        public byte Tk2 { get; set; }
        public byte Tk1 { get; set; }
        public short BankId { get; set; }
        public string BankAcNo { get; set; }
        public decimal ExchRate { get; set; }
        public int? IsWithSal { get; set; }
        public string SubSec { get; set; }
        public float Percen { get; set; }
    }
}
