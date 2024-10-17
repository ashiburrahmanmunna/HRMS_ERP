using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPendingJobSub
    {
        public int PendingJobId { get; set; }
        public int? Doid { get; set; }
        public string Brand { get; set; }
        public int? Prddisid { get; set; }
        public double? Qty { get; set; }
        public string Remarks { get; set; }
        public byte? DorowNo { get; set; }
        public string DospecManual { get; set; }
        public int? Slno { get; set; }

        public virtual TblStrPendingJobMain PendingJob { get; set; }
    }
}
