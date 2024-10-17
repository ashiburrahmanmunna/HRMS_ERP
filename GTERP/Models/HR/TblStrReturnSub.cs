using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrReturnSub
    {
        public int Rtnid { get; set; }
        public long PrdDisId { get; set; }
        public double RtnQty { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public int RowNo { get; set; }
        public string Remarks { get; set; }
        public decimal? UnitPrice { get; set; }
        public double OtherUnitrtnQty { get; set; }
        public double IssueQty { get; set; }
        public double OtherUnitIssueQty { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrReturnMain Rtn { get; set; }
    }
}
