using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempLeaveInput
    {
        public byte ComId { get; set; }
        public long AId { get; set; }
        public int LvId { get; set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public DateTime? DtInput { get; set; }
        public string LvType { get; set; }
        public DateTime? DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public int LvDay { get; set; }
        public string Remarks { get; set; }
        public int? Sl { get; set; }
        public byte IsApprove { get; set; }
    }
}
