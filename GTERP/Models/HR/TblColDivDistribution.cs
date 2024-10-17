using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblColDivDistribution
    {
        public long PrdId { get; set; }
        public int? RefId { get; set; }
        public DateTime? DateLast { get; set; }
        public double CutQty { get; set; }
        public int? StcStart { get; set; }
        public double QtyLast { get; set; }
        public Guid? WId { get; set; }
        public long AId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
        public byte? Comid { get; set; }
        public int RowNo { get; set; }
        public byte IsBundleSticker { get; set; }
        public byte IsProcessSticker { get; set; }
    }
}
