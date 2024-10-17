using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesTruckChallanBillSub
    {
        public long TruckChallanBillId { get; set; }
        public long? ChallanId { get; set; }
        public long PrdId { get; set; }
        public int? Prddisid { get; set; }
        public byte RowNo { get; set; }
        public byte? DorowNo { get; set; }
        public byte? TruckRowNo { get; set; }
        public byte? BillRowNo { get; set; }
        public double Qty { get; set; }
        public double QtyIssue { get; set; }
        public double QtyDel { get; set; }
        public double QtyBal { get; set; }
        public double QtyCancel { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public Guid WId { get; set; }
        public double Discount { get; set; }
        public double CarryingRate { get; set; }
        public double UnLoading { get; set; }
        public double? Billnetrate { get; set; }

        public virtual TblSalesTruckChallanBillMain TruckChallanBill { get; set; }
    }
}
