using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempReference
    {
        public int? LuserId { get; set; }
        public byte? ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int RefId { get; set; }
        public string RefCode { get; set; }
        public string RefName { get; set; }
        public decimal OpenDebit { get; set; }
        public decimal OpenCredit { get; set; }
        public decimal TranDebit { get; set; }
        public decimal TranCredit { get; set; }
        public decimal CloseDebit { get; set; }
        public decimal CloseCredit { get; set; }
        public decimal TempBalance { get; set; }
        public string Currency { get; set; }
        public string Caption { get; set; }
        public byte IsShowZero { get; set; }
    }
}
