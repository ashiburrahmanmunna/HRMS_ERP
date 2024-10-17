using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderEvaSub
    {
        public long Evaid { get; set; }
        public long OrdId { get; set; }
        public short ArticleId { get; set; }
        public float QtyOrder { get; set; }
        public float QtyBatchKg { get; set; }
        public float QtyWeightKg { get; set; }
        public float QtyBatch { get; set; }
        public int PrdId { get; set; }
        public string PrdCodeEva { get; set; }
        public short ColorId { get; set; }
        public string Hardness { get; set; }
        public string Thickness { get; set; }
        public short SheetWidth { get; set; }
        public short SheetHight { get; set; }
        public double Wastage { get; set; }
        public float SheetTtl { get; set; }
        public double LoadingWeight { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public double? TtlPcs { get; set; }
        public double? PcsPerSheet { get; set; }
        public string Remarks { get; set; }
        public double? TtlSheet { get; set; }

        public virtual TblStrOrderEvaMain Eva { get; set; }
    }
}
