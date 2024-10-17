using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrGrrSub
    {
        public long GrrId { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public long Poid { get; set; }
        public double ChallanQty { get; set; }
        public double QtyRcvd { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public string Remarks { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public byte IsComplete { get; set; }
        public string RemarksGrr { get; set; }
        public string BasedOn { get; set; }
        public int BasedId { get; set; }
        public double? OverQty { get; set; }
        public double? Shortage { get; set; }
        public double OtherUnitQtyRcvd { get; set; }
        public double OtherUnitAmount { get; set; }
        public double OtherUnitUnitPrice { get; set; }
        public double? Reject { get; set; }
        public long? OrdId { get; set; }
        public byte? ProcessId { get; set; }
        public DateTime? DtExpire { get; set; }
        public double? Poqty { get; set; }

        public virtual TblStrGrrMain Grr { get; set; }
        public virtual TblStrProductDistribution Prddis { get; set; }
    }
}
