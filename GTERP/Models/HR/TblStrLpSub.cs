using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLpSub
    {
        public long Lpid { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public double ChallanQty { get; set; }
        public double QtyRcvd { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public string RemarksLp { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public byte? IsComplete { get; set; }
        public double? OtherUnitQtyRcvd { get; set; }

        public virtual TblStrLpMain Lp { get; set; }
    }
}
