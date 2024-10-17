using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrTransactionSub
    {
        public long TranId { get; set; }
        public long PrdDisId { get; set; }
        public short Whid { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double IssueQty { get; set; }
        public double Balance { get; set; }
        public Guid WId { get; set; }
        public int? PrdId { get; set; }
        public int? OrdId { get; set; }
        public int? ComId { get; set; }
        public double OtherUnitIssueQty { get; set; }
        public double OtherUnitQty { get; set; }
        public double OtherUnitBalance { get; set; }
        public double OtherUnitRate { get; set; }
        public int BinId { get; set; }
        public double? OtherBalance { get; set; }
        public double? OtherIssueQty { get; set; }
        public DateTime? DtGrr { get; set; }
        public DateTime? Dtexpire { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrTransactionMain Tran { get; set; }
    }
}
