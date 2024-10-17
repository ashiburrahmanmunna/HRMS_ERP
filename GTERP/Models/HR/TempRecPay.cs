using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempRecPay
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string Caption { get; set; }
        public int? ParentAccId { get; set; }
        public string ParentAccCode { get; set; }
        public string ParentAccName { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
        public decimal Tkdebit { get; set; }
        public decimal Tkcredit { get; set; }
        public decimal Tkbalance { get; set; }
        public short? LuserId { get; set; }
        public byte IsCash { get; set; }
        public byte IsBank { get; set; }
        public byte PayType { get; set; }
        public byte TranStatus { get; set; }
        public string TranCaption { get; set; }
        public decimal Tkcash { get; set; }
        public decimal Tkbank { get; set; }
        public decimal Tktotal { get; set; }
        public byte IsFinal { get; set; }
        public long VoucherId { get; set; }
        public byte? IsCreditBalance { get; set; }
    }
}
