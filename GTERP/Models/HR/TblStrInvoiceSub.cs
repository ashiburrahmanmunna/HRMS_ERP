using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrInvoiceSub
    {
        public int InvoiceId { get; set; }
        public int? TransId { get; set; }
        public DateTime? DtStock { get; set; }
        public long PrdDisId { get; set; }
        public double QtyReq { get; set; }
        public decimal? Rate { get; set; }
        public decimal? LocalRate { get; set; }
        public decimal? Amount { get; set; }
        public long RowNo { get; set; }
        public string Remarks { get; set; }
        public Guid WId { get; set; }
        public int? Iscomplete { get; set; }
        public decimal? ForeignRate { get; set; }
        public decimal? ForeignAmount { get; set; }

        public virtual TblStrInvoiceMain Invoice { get; set; }
    }
}
