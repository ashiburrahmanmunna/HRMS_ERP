using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempPrdLedger
    {
        public long? PrdId { get; set; }
        public long? PrdDisid { get; set; }
        public string Type { get; set; }
        public double? Qty { get; set; }
        public DateTime? DtDate { get; set; }
        public string DocNo { get; set; }
        public double? Qtytran { get; set; }
        public double? QtytranOtherUnit { get; set; }
        public double? QtyOtherUnit { get; set; }
    }
}
