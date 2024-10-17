using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPoSub
    {
        public long Poid { get; set; }
        public long PrdDisId { get; set; }
        public double QtyOrder { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte IsComplete { get; set; }
        public byte Flag { get; set; }

        public virtual TblStrPoMain Po { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
