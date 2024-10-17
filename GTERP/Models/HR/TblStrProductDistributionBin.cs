using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductDistributionBin
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public short WhId { get; set; }
        public short BinId { get; set; }
        public double Qty { get; set; }
        public double QtyOp { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public int Bcomid { get; set; }
        public double OtherUnitQty { get; set; }
        public double OtherUnitQtyOp { get; set; }

        public virtual TblStrProduct Prd { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblCatWarehouse Wh { get; set; }
    }
}
