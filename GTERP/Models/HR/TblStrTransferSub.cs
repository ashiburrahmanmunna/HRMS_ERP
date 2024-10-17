using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrTransferSub
    {
        public long TransferId { get; set; }
        public int FwhId { get; set; }
        public int FbinId { get; set; }
        public int PrdId { get; set; }
        public long PrdDisId { get; set; }
        public int ColorId { get; set; }
        public int Unit { get; set; }
        public double Qty { get; set; }
        public int TwhId { get; set; }
        public int TbinId { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrTransferMain Transfer { get; set; }
    }
}
