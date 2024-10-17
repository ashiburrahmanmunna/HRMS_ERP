using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrChemicalBatchMain
    {
        public TblStrChemicalBatchMain()
        {
            TblStrChemicalBatchSizeSub = new HashSet<TblStrChemicalBatchSizeSub>();
            TblStrChemicalBatchSub = new HashSet<TblStrChemicalBatchSub>();
        }

        public byte ComId { get; set; }
        public long Cbid { get; set; }
        public int ArticleId { get; set; }
        public DateTime DtConfig { get; set; }
        public decimal TtlWeight { get; set; }
        public byte Wastage { get; set; }
        public long PrdDisId { get; set; }
        public string Flag { get; set; }
        public short ColorId { get; set; }
        public string ProductName { get; set; }
        public string Hardness { get; set; }
        public string Thickness { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }

        public virtual TblCatArticle Article { get; set; }
        public virtual ICollection<TblStrChemicalBatchSizeSub> TblStrChemicalBatchSizeSub { get; set; }
        public virtual ICollection<TblStrChemicalBatchSub> TblStrChemicalBatchSub { get; set; }
    }
}
