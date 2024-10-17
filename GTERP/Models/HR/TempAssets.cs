using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempAssets
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int? ParentId { get; set; }
        public string ParentCode { get; set; }
        public string Currency { get; set; }
        public string Caption { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
        public decimal TranDebit { get; set; }
        public decimal TranCredit { get; set; }
        public decimal ClDebit { get; set; }
        public decimal ClCredit { get; set; }
        public decimal SumDebit { get; set; }
        public decimal SumCredit { get; set; }
        public byte SortNo { get; set; }
        public byte Flag { get; set; }
        public string Remarks { get; set; }
    }
}
