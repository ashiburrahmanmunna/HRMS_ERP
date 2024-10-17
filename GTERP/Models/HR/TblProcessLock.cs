using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProcessLock
    {
        public int AId { get; set; }
        public byte ComId { get; set; }
        public string LockType { get; set; }
        public DateTime DtDate { get; set; }
        public byte IsLock { get; set; }
        public byte? IsInactive { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
    }
}
