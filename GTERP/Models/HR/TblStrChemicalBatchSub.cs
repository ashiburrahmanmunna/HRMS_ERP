using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrChemicalBatchSub
    {
        public long Cbid { get; set; }
        public long PrdDisId { get; set; }
        public decimal Qty { get; set; }
        public string Flag { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public double? ExtraPer { get; set; }
        public string CalcFor { get; set; }
        public string Remarks { get; set; }

        public virtual TblStrChemicalBatchMain Cb { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
