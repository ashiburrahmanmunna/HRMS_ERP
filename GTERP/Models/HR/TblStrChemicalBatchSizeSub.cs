using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrChemicalBatchSizeSub
    {
        public long Cbid { get; set; }
        public int SizeId { get; set; }
        public string Flag { get; set; }
        public decimal SoleWeightGm { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public double? ExtraPer { get; set; }
        public decimal? ToeCapWeightGm { get; set; }

        public virtual TblStrChemicalBatchMain Cb { get; set; }
    }
}
