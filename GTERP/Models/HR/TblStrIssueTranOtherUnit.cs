using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrIssueTranOtherUnit
    {
        public long IssueId { get; set; }
        public long PrdDisId { get; set; }
        public double OtherUnitQty { get; set; }
        public double OtherUnitRate { get; set; }
        public long TranId { get; set; }
        public Guid WId { get; set; }
        public long? OrdId { get; set; }
        public int? Rownotemp { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }

        public virtual TblStrIssueMain Issue { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
