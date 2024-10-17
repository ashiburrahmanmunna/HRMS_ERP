using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPendingJobMain
    {
        public TblStrPendingJobMain()
        {
            TblStrPendingJobSub = new HashSet<TblStrPendingJobSub>();
        }

        public int PendingJobId { get; set; }
        public byte ComId { get; set; }
        public string DocNo { get; set; }
        public DateTime DtDate { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public DateTime? DtIssue { get; set; }
        public string RevNo { get; set; }

        public virtual ICollection<TblStrPendingJobSub> TblStrPendingJobSub { get; set; }
    }
}
