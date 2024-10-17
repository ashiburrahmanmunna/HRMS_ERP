using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPrSub
    {
        public long Prid { get; set; }
        public long PrdDisId { get; set; }
        public double QtyReq { get; set; }
        public byte RowNo { get; set; }
        public Guid WiId { get; set; }
        public string Remarks { get; set; }
        public byte IsComplete { get; set; }
        public string ShipMode { get; set; }
        public double QtyReqOtherUnit { get; set; }
        public int ProcessId { get; set; }
        public long OrdId { get; set; }
        public string GroupByName { get; set; }
        public string Density { get; set; }
        public string PicName { get; set; }

        public virtual TblStrPrMain Pr { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
