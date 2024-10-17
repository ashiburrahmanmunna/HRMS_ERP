using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrGrrAmtSub
    {
        public long GrrAmtId { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public long Poid { get; set; }
        public double ChallanQty { get; set; }
        public double QtyRcvd { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        public string Remarks { get; set; }
        public byte? RowNo { get; set; }
        public Guid WId { get; set; }
        public int? Whid { get; set; }
        public int? BinId { get; set; }
        public byte IsComplete { get; set; }
        public string RemarksGrr { get; set; }
        public string BasedOn { get; set; }
        public int BasedId { get; set; }

        public virtual TblStrGrrAmtMain GrrAmt { get; set; }
        public virtual TblStrProductDistribution Prddis { get; set; }
    }
}
