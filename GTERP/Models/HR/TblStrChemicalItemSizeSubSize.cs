using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrChemicalItemSizeSubSize
    {
        public long Aiid { get; set; }
        public long? ArticleId { get; set; }
        public long PrdDisId { get; set; }
        public int? SizeId { get; set; }
        public int? RowNo { get; set; }
        public Guid Wid { get; set; }
        public byte? Comid { get; set; }
        public double? SizeQty { get; set; }
        public long? Cbid { get; set; }
        public long? Colorid { get; set; }
    }
}
