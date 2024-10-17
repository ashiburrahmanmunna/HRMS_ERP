using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderImevaWeight
    {
        public long Sppid { get; set; }
        public int SizeId { get; set; }
        public int QtyPair { get; set; }
        public double WeightPairGm { get; set; }
        public double WeightTotalKg { get; set; }
        public int ColorId { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public double? ToeCapWeightGm { get; set; }

        public virtual TblStrOrderImevaMain Spp { get; set; }
    }
}
