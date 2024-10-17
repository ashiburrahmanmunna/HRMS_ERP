using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrArticleItemSub
    {
        public long Aiid { get; set; }
        public long PrdDisId { get; set; }
        public double Qty { get; set; }
        public double QtyFloor { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public int? SlNo { get; set; }
        public string Remarks { get; set; }
        public string RemarksOther { get; set; }
        public int? BomtypeId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public string RowColour { get; set; }
        public int? RowColourid { get; set; }
        public double? Size { get; set; }
        public int? IsSalesDept { get; set; }

        public virtual TblStrArticleItemMain Ai { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
