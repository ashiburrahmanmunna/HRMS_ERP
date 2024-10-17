using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrBomArticleSize
    {
        public long BomId { get; set; }
        public long Bmid { get; set; }
        public int Qty { get; set; }
        public int SizeId { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public int? ArticleId { get; set; }

        public virtual TblStrBomMain Bom { get; set; }
    }
}
