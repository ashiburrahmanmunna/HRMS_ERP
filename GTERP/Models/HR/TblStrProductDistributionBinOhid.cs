using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductDistributionBinOhid
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public short WhId { get; set; }
        public short BinId { get; set; }
        public double Qty { get; set; }
        public double QtyOp { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
    }
}
