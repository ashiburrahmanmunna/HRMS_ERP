using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSubStrIssueMain
    {
        public TblStrSubStrIssueMain()
        {
            TblStrSubStrIssueSub = new HashSet<TblStrSubStrIssueSub>();
        }

        public byte ComId { get; set; }
        public long SubStrIssueId { get; set; }
        public string SubStrIssueNo { get; set; }
        public DateTime DtDate { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public string Remarks { get; set; }
        public int IsPosted { get; set; }
        public int? SubSectId { get; set; }
        public int? SubSectIdto { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int? SubStoreId { get; set; }
        public int? LuserIdCheck { get; set; }

        public virtual ICollection<TblStrSubStrIssueSub> TblStrSubStrIssueSub { get; set; }
    }
}
