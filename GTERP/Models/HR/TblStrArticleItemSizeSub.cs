using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrArticleItemSizeSub
    {
        public long Aiid { get; set; }
        public long PrdDisId { get; set; }
        public double Qty { get; set; }
        public double QtyFloor { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public int? SlNo { get; set; }
        public string Remarks { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
