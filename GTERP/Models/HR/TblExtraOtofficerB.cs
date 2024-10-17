using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblExtraOtofficerB
    {
        public int? AEmpId { get; set; }
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public string Month { get; set; }
        public short Year { get; set; }
        public string EmpCode { get; set; }
        public string EmpType { get; set; }
        public DateTime? DtInput { get; set; }
        public short? DesigId { get; set; }
        public short SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DeptId { get; set; }
        public string Band { get; set; }
        public string ProssType { get; set; }
        public string Grade { get; set; }
        public string DtSalary { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public string AllowType { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public decimal OtrateFirst { get; set; }
        public decimal OtrateLast { get; set; }
        public float OthrTtl { get; set; }
        public decimal Ot { get; set; }
        public float ExOthr { get; set; }
        public decimal ExOtamount { get; set; }
        public decimal Trn { get; set; }
        public decimal LunchAmt { get; set; }
        public decimal TiffinAllow { get; set; }
        public decimal NetPayable { get; set; }
        public byte IsReleased { get; set; }
        public int Amount { get; set; }
        public int Tk1000 { get; set; }
        public int Tk500 { get; set; }
        public int Tk100 { get; set; }
        public int Tk50 { get; set; }
        public int Tk20 { get; set; }
        public int Tk10 { get; set; }
        public int Tk5 { get; set; }
        public int Tk2 { get; set; }
        public int Tk1 { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
    }
}
