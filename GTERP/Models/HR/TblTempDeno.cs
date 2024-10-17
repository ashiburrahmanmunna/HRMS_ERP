using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempDeno
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
        public string Grade { get; set; }
        public string GradeSal { get; set; }
        public string PaySlipNo { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public string ProssType { get; set; }
        public decimal Gs { get; set; }
        public decimal? Gsfinal { get; set; }
        public decimal? NetSalaryPayable { get; set; }
        public long? Amount { get; set; }
        public int Tk1000 { get; set; }
        public int Tk500 { get; set; }
        public int Tk100 { get; set; }
        public int Tk50 { get; set; }
        public int Tk20 { get; set; }
        public int Tk10 { get; set; }
        public int Tk5 { get; set; }
    }
}
