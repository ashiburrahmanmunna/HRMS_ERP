using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLateDeduction
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
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public byte WorkingDays { get; set; }
        public float LateHr { get; set; }
        public float LateDay { get; set; }
        public float LateDedDay { get; set; }
        public decimal LateAmt { get; set; }
    }
}
