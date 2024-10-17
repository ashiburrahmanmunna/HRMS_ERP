using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebJcardSchangeSub
    {
        public long SchangeSId { get; set; }
        public int SchangeId { get; set; }
        public DateTime DtDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Late { get; set; }
        public string OverTime { get; set; }
        public string StatusOld { get; set; }
        public string StatusNew { get; set; }
        public string Remarks { get; set; }
        public byte IsAllowed { get; set; }
        public string DenyFor { get; set; }
    }
}
