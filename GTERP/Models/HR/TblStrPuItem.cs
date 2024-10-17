using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPuItem
    {
        public int BomId { get; set; }
        public long Bmid { get; set; }
        public int ArticleId { get; set; }
        public long PrdDisId { get; set; }
        public int SizeId { get; set; }
        public int SizeQty { get; set; }
        public double SoleWeightGm { get; set; }
        public double CalcGrm { get; set; }
        public double ExtraPer { get; set; }
        public double ExtraQty { get; set; }
        public double PerPairGm { get; set; }
        public double TtlGram { get; set; }
        public double NetGram { get; set; }
        public double NetGmKg { get; set; }
        public int SupplierId { get; set; }
        public decimal Qty { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }

        public virtual TblStrPuArticle Bm { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
