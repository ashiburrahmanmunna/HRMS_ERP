using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrIssueMain
    {
        public TblStrIssueMain()
        {
            TblStrIssueSub = new HashSet<TblStrIssueSub>();
            TblStrIssueTran = new HashSet<TblStrIssueTran>();
            TblStrIssueTranOtherUnit = new HashSet<TblStrIssueTranOtherUnit>();
        }

        public byte ComId { get; set; }
        public long IssueId { get; set; }
        public string IssueNo { get; set; }
        public DateTime DtDate { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public string Remarks { get; set; }
        public string BasedOn { get; set; }
        public long BasedId { get; set; }
        public int IsPosted { get; set; }
        public int? SubSectId { get; set; }
        public int? SubSectIdto { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int? SupplierId { get; set; }
        public byte? IsExternalParty { get; set; }
        public DateTime? DtTime { get; set; }

        public virtual TblStrSrrMain Based { get; set; }
        public virtual ICollection<TblStrIssueSub> TblStrIssueSub { get; set; }
        public virtual ICollection<TblStrIssueTran> TblStrIssueTran { get; set; }
        public virtual ICollection<TblStrIssueTranOtherUnit> TblStrIssueTranOtherUnit { get; set; }
    }
}
