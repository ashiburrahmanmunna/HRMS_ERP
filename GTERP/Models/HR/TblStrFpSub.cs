using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrFpSub
    {
        public long Fpid { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public double? QtyRcvd { get; set; }
        public double? IssuedQty { get; set; }
        public string RemarksFp { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public double? OtherUnitIssuedQty { get; set; }
    }
}
