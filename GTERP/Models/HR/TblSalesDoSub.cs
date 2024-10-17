using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesDoSub
    {
        public long Doid { get; set; }
        public long PrdId { get; set; }
        public double Qty { get; set; }
        public double QtyIssue { get; set; }
        public double QtyDel { get; set; }
        public double QtyBal { get; set; }
        public double QtyCancel { get; set; }
        public short UnitId { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public byte RowNo { get; set; }
        public byte DorowNo { get; set; }
        public double FreeBagCon { get; set; }
        public Guid WId { get; set; }
        public byte IsFreeBag { get; set; }
        public double Freebag { get; set; }
        public double Discount { get; set; }
        public double CarryingRate { get; set; }
        public double UnLoading { get; set; }
        public double? Freebagcal { get; set; }
        public int? VatId { get; set; }
        public double NetRate { get; set; }
        public double? ExportValue { get; set; }
        public int? Prddisid { get; set; }
        public string DospecManual { get; set; }
        public string DosizeManual { get; set; }
        public string DospecBrand { get; set; }
        public string Doremarks { get; set; }
        public int? DoBookId { get; set; }

        public virtual TblSalesDoMain Do { get; set; }
    }
}
