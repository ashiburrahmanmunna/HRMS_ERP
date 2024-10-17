using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPuArticle
    {
        public TblStrPuArticle()
        {
            TblStrPuItem = new HashSet<TblStrPuItem>();
        }

        public long BomId { get; set; }
        public long Bmid { get; set; }
        public int ArticleId { get; set; }
        public decimal Qty { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }

        public virtual TblCatArticle Article { get; set; }
        public virtual TblStrPuMain Bom { get; set; }
        public virtual ICollection<TblStrPuItem> TblStrPuItem { get; set; }
    }
}
