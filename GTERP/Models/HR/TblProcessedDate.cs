using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProcessedDate
    {
        public byte ComId { get; set; }
        public DateTime DtProcess { get; set; }
        public string DaySts { get; set; }
        public string DayStsB { get; set; }
        public byte IsLock { get; set; }
        public byte IsExecProc { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
    }
}
