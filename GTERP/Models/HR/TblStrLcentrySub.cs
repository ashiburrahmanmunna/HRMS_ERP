using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLcentrySub
    {
        public int LcId { get; set; }
        public long PrdDisId { get; set; }
        public double QtyReq { get; set; }
        public decimal? Rate { get; set; }
        public decimal? LocalRate { get; set; }
        public decimal? Amount { get; set; }
        public long RowNo { get; set; }
        public string Remarks { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrLcentryMain Lc { get; set; }
    }
}
