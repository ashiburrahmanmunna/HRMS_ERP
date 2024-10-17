using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesVatChallanSub
    {
        public long VatChallanId { get; set; }
        public long PrdId { get; set; }
        public double Qty { get; set; }
        public double QtyDel { get; set; }
        public short UnitId { get; set; }
        public byte VatRowNo { get; set; }
        public Guid WId { get; set; }
        public long DoIdtest { get; set; }
        public byte DorowNo { get; set; }
        public int? PrdDisId { get; set; }
        public byte? RowNo { get; set; }
        public string VatChallanRemarks { get; set; }

        public virtual TblSalesVatChallanMain VatChallan { get; set; }
    }
}
