using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderEvaConsumption
    {
        public long Evaid { get; set; }
        public short ArticleId { get; set; }
        public int? OrdId { get; set; }
        public int PrdDisId { get; set; }
        public decimal QtyBatch { get; set; }
        public decimal QtyBatchKg { get; set; }
        public decimal QtyBatchKgGross { get; set; }
        public float PerWastage { get; set; }
        public decimal QtyWastage { get; set; }
        public decimal QtyBatchKgNet { get; set; }
        public string Flag { get; set; }
        public int? RowNo { get; set; }
        public int? PrdIdeva { get; set; }
        public double? TtlSheet { get; set; }

        public virtual TblStrOrderEvaMain Eva { get; set; }
    }
}
