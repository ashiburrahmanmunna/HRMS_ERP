using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccBillPay
    {
        public TblAccBillPay()
        {
            TblAccBillPayData = new HashSet<TblAccBillPayData>();
        }

        public int? ComId { get; set; }
        public int PayId { get; set; }
        public DateTime? InptDt { get; set; }
        public int? ClientId { get; set; }
        public int? PayAmt { get; set; }
        public string PayAmtInWrd { get; set; }
        public DateTime? PayDt { get; set; }
        public string AccNo { get; set; }
        public string PayMode { get; set; }
        public string BankName { get; set; }
        public string BanchName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ClrDt { get; set; }
        public string Sts { get; set; }
        public DateTime? NxtClrDt { get; set; }
        public string Remarks { get; set; }
        public string IsCumulative { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }

        public virtual ICollection<TblAccBillPayData> TblAccBillPayData { get; set; }
    }
}
