using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempSum
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public DateTime DtDate { get; set; }
        public short DeptId { get; set; }
        public short SectId { get; set; }
        public short SubSectId { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string SubSectName { get; set; }
        public string EmpType { get; set; }
        public short ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? ShiftIn { get; set; }
        public float Strength { get; set; }
        public float TtlEmp { get; set; }
        public float NewJoin { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float AbsentPer { get; set; }
        public float Leave { get; set; }
        public float Total { get; set; }
    }
}
