using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempTradingAccount
    {
        public byte LuserId { get; set; }
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
        public decimal TranDebit { get; set; }
        public decimal TranCredit { get; set; }
        public decimal ClDebit { get; set; }
        public decimal ClCredit { get; set; }
        public byte IsSingleDate { get; set; }
        public byte IsFybased { get; set; }
        public byte UserId { get; set; }
        public string Fyname { get; set; }
        public string AccType { get; set; }
        public byte IsOnlySum { get; set; }
        public string Currency { get; set; }
        public string Caption { get; set; }
    }
}
