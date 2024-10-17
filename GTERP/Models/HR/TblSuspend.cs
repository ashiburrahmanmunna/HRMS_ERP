using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSuspend
    {
        public byte ComId { get; set; }
        public int Sid { get; set; }
        public long EmpId { get; set; }
        public short DeptId { get; set; }
        public short SectId { get; set; }
        public short SubSectId { get; set; }
        public short DesigId { get; set; }
        public DateTime? DtInput { get; set; }
        public DateTime? DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public float WorkingDays { get; set; }
        public float SusDay { get; set; }
        public float PayDay { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }
    }
}
