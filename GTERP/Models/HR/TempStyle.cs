using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempStyle
    {
        public int? PrdId { get; set; }
        public int? PrdDisId { get; set; }
        public string Spec { get; set; }
        public double RcvQty { get; set; }
        public int? Whid { get; set; }
        public int? BinId { get; set; }
        public string Type { get; set; }
        public double IssueQty { get; set; }
        public double IssueQtyA { get; set; }
        public double TotalBalance { get; set; }
    }
}
