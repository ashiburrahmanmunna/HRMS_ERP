using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempChemicalWeight
    {
        public long Sppid { get; set; }
        public long ArticleId { get; set; }
        public short SizeId { get; set; }
        public short QtyPair { get; set; }
        public double WeightPairGm { get; set; }
        public double WeightTotalGm { get; set; }
        public short WeightTotalKg { get; set; }
    }
}
