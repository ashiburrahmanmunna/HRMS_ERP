using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrIssueSub
    {
        public long IssueId { get; set; }
        public long PrdDisId { get; set; }
        public double QtyReq { get; set; }
        public double? QtyIssue { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public string Remarks { get; set; }
        public long RowNo { get; set; }
        public Guid WId { get; set; }
        public string RemarksIssue { get; set; }
        public byte? IsComplete { get; set; }
        public double OtherUnitQtyIssue { get; set; }
        public double OtherUnitQtyReq { get; set; }
        public long? OrdId { get; set; }
        public byte? ProcessId { get; set; }

        public virtual TblStrIssueMain Issue { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
