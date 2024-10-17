using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrBomArticle
    {
        public TblStrBomArticle()
        {
            TblStrBomItem = new HashSet<TblStrBomItem>();
        }

        public long BomId { get; set; }
        public long Bmid { get; set; }
        public int ArticleId { get; set; }
        public decimal Qty { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }

        public virtual TblCatArticle Article { get; set; }
        public virtual TblStrBomMain Bom { get; set; }
        public virtual ICollection<TblStrBomItem> TblStrBomItem { get; set; }
    }
}
