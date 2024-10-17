using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccBillRcvdInfo
    {
        public TblAccBillRcvdInfo()
        {
            TblAccBillRcvdData = new HashSet<TblAccBillRcvdData>();
        }

        public byte ComId { get; set; }
        public int RcvId { get; set; }
        public DateTime InptDt { get; set; }
        public DateTime RcvDt { get; set; }
        public string RcvMode { get; set; }
        public int? ClientId { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public float ConvRate { get; set; }
        public double Amount { get; set; }
        public double AmountLocal { get; set; }
        public string AmountInWords { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccNo { get; set; }
        public string RefNo { get; set; }
        public DateTime? ClearDt { get; set; }
        public DateTime? NextSubmitDt { get; set; }
        public string Status { get; set; }
        public byte IsComplete { get; set; }
        public byte IsCumulative { get; set; }
        public string Remarks { get; set; }
        public byte AccStatus { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public int LuserId { get; set; }
        public string ChequeNo { get; set; }

        public virtual ICollection<TblAccBillRcvdData> TblAccBillRcvdData { get; set; }
    }
}
