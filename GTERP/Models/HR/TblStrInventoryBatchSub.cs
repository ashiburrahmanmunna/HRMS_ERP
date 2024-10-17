using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrInventoryBatchSub
    {
        public long InvId { get; set; }
        public long PrdDisId { get; set; }
        public decimal Qty { get; set; }
        public decimal Consumption { get; set; }
        public decimal PrdRunDays { get; set; }
        public decimal PipelineOrder { get; set; }
        public decimal TtlStcOrd { get; set; }
        public decimal PrdRunDaysStcOrd { get; set; }
        public DateTime NextMatOrdDate { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public double? ExtraPer { get; set; }
        public string CalcFor { get; set; }
        public string Remarks { get; set; }

        public virtual TblStrInventoryBatchMain Inv { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
