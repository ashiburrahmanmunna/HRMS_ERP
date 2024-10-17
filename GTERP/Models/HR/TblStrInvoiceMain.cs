using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrInvoiceMain
    {
        public TblStrInvoiceMain()
        {
            TblStrInvoiceSub = new HashSet<TblStrInvoiceSub>();
        }

        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int Lcid { get; set; }
        public byte ComId { get; set; }
        public DateTime DtDate { get; set; }
        public DateTime DtBl { get; set; }
        public string CoverNotes { get; set; }
        public decimal? Lcqty { get; set; }
        public decimal? Lcamount { get; set; }
        public int? CurrencyId { get; set; }
        public byte? IsCancelLc { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public int? LuserId { get; set; }
        public decimal? CurrencyRate { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public byte IsComplete { get; set; }

        public virtual TblStrLcentryMain Lc { get; set; }
        public virtual ICollection<TblStrInvoiceSub> TblStrInvoiceSub { get; set; }
    }
}
