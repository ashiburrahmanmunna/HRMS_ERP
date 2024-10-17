using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTiffinAllow
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public short DeptId { get; set; }
        public short SectId { get; set; }
        public short SubSectId { get; set; }
        public short DesigId { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
        public string DtSalary { get; set; }
        public string EmpType { get; set; }
        public byte Type { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public byte WorkingDays { get; set; }
        public float Present { get; set; }
        public float PresentShift { get; set; }
        public float PresentGen { get; set; }
        public float Absent { get; set; }
        public decimal Amount { get; set; }
    }
}
