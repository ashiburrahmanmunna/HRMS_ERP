using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesBookSub
    {
        public long BookId { get; set; }
        public int IsFreeBag { get; set; }
        public long PrdId { get; set; }
        public double Qty { get; set; }
        public double Freebag { get; set; }
        public double QtyTotal { get; set; }
        public double QtyIssue { get; set; }
        public double QtyDel { get; set; }
        public double QtyBal { get; set; }
        public double QtyCancel { get; set; }
        public short UnitId { get; set; }
        public int? VatId { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double CarryingRate { get; set; }
        public double Unloading { get; set; }
        public double Discount { get; set; }
        public double NetRate { get; set; }
        public double NetAmount { get; set; }
        public string Status { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public double? ExportValue { get; set; }
        public string SpecBrand { get; set; }
        public string Remarks { get; set; }
        public int? Prddisid { get; set; }
        public string SpecManual { get; set; }
        public string SizeManual { get; set; }

        public virtual TblSalesBookMain Book { get; set; }
    }
}
