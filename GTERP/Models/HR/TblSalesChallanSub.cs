using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesChallanSub
    {
        public long ChallanId { get; set; }
        public long PrdId { get; set; }
        public int? Prddisid { get; set; }
        public byte RowNo { get; set; }
        public byte? DorowNo { get; set; }
        public byte? VatRowNo { get; set; }
        public byte? TruckRowNo { get; set; }
        public byte? BillRowNo { get; set; }
        public double Qty { get; set; }
        public double QtyDel { get; set; }
        public short UnitId { get; set; }
        public Guid WId { get; set; }
        public double? LoadTrucWeight { get; set; }
        public double? EmptyTruckWeight { get; set; }
        public double? WeightDifference { get; set; }
        public long? Doidtest { get; set; }
        public double? BundleQty { get; set; }
        public double? TotalBundle { get; set; }
        public string TruckRemarks { get; set; }
        public double? RemainingBundleQty { get; set; }
        public double? RemainingBundleContainsQty { get; set; }
        public double? BundleContainsQty { get; set; }
        public double? TotalCumulativeQty { get; set; }
        public string VatChallanRemarks { get; set; }
        public string TruckChallanRemarks { get; set; }

        public virtual TblSalesChallanMain Challan { get; set; }
    }
}
