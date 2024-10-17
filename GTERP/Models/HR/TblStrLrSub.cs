using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLrSub
    {
        public long Lrid { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public double ChallanQty { get; set; }
        public double QtyRcvd { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public string RemarksLr { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public byte? IsComplete { get; set; }
        public double? Lsqty { get; set; }
        public double? Lsprice { get; set; }
        public double? Lsamount { get; set; }
        public int? Lpid { get; set; }
        public string RemarksLp { get; set; }
        public double? OtherUnitQtyRcvd { get; set; }

        public virtual TblStrLrMain Lr { get; set; }
    }
}
