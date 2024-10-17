using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrFrSub
    {
        public long Frid { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public double QtyRcvd { get; set; }
        public string RemarksFr { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public double? OrdQty { get; set; }
        public byte IsCompleteWrong { get; set; }
        public double? OtherUnitQtyRcvd { get; set; }

        public virtual TblStrFrMain Fr { get; set; }
    }
}
