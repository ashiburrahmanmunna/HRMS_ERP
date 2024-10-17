using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempOpeningBalance
    {
        public int? PrdId { get; set; }
        public double? Qtypcs { get; set; }
        public string Type { get; set; }
        public DateTime? DtDate { get; set; }
        public int? PrdDisId { get; set; }
        public double? QtypcsotherUnit { get; set; }
    }
}
