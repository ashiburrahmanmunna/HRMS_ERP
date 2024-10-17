using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderImevaConsumption
    {
        public long Sppid { get; set; }
        public int PrdDisId { get; set; }
        public decimal QtyBatch { get; set; }
        public decimal QtyBatchKg { get; set; }
        public decimal QtyBatchKgGross { get; set; }
        public float PerWastage { get; set; }
        public decimal QtyWastage { get; set; }
        public decimal QtyBatchKgNet { get; set; }
        public string Flag { get; set; }
        public int ColorId { get; set; }
        public int? RowNo { get; set; }
        public string Remarks { get; set; }

        public virtual TblStrOrderImevaMain Spp { get; set; }
    }
}
