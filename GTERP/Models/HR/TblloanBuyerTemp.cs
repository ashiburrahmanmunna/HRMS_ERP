using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblloanBuyerTemp
    {
        public string Empid { get; set; }
        public string Empname { get; set; }
        public int? Loan { get; set; }
        public int? ClosingBalance { get; set; }
        public int? Opbalance { get; set; }
        public string Pass { get; set; }
        public DateTime? FirstDate { get; set; }
        public DateTime? Lastdate { get; set; }
        public int? Debit { get; set; }
        public int? Credit { get; set; }
        public int? Cash { get; set; }
        public DateTime? Inputdate { get; set; }
        public int? Gs { get; set; }
        public DateTime? Date { get; set; }
    }
}
