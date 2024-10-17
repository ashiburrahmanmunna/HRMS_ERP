using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpDataUpdate
    {
        public long AId { get; set; }
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtInput { get; set; }
        public DateTime DtTran { get; set; }
        public string EmpCodePrev { get; set; }
        public string EmpCodeNew { get; set; }
        public short DeptIdPrev { get; set; }
        public short DeptIdNew { get; set; }
        public short SectIdPrev { get; set; }
        public short SectIdNew { get; set; }
        public short DesigIdPrev { get; set; }
        public short DesigIdNew { get; set; }
        public short SubSectIdPrev { get; set; }
        public short SubSectIdNew { get; set; }
        public byte UpdateType { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }
        public string Remarks { get; set; }
    }
}
