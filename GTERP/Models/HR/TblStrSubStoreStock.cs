using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSubStoreStock
    {
        public short SubStoreId { get; set; }
        public long PrdDisId { get; set; }
        public double Qty { get; set; }
        public double QtyOp { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public int? ComId { get; set; }
        public DateTime? DtDate { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrSubStore SubStore { get; set; }
    }
}
