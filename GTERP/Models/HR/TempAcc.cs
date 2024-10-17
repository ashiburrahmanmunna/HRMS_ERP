using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempAcc
    {
        public byte? ComId { get; set; }
        public int? AccId { get; set; }
        public string AccCode { get; set; }
        public decimal? OpDebit { get; set; }
        public decimal? OpCredit { get; set; }
        public decimal? TranDebit { get; set; }
        public decimal? TranCredit { get; set; }
        public decimal? ClDebit { get; set; }
        public decimal? ClCredit { get; set; }
        public int? UserId { get; set; }
        public string Currency { get; set; }
    }
}
