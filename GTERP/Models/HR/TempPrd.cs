using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempPrd
    {
        public int? Prdid { get; set; }
        public double? PurQty { get; set; }
        public double? SalesQty { get; set; }
        public string Ttype { get; set; }
        public double? CostSft { get; set; }
        public int? PrdDisId { get; set; }
    }
}
