using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblstrAvgConsume
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public float? IssueQty { get; set; }
        public float? Avg { get; set; }
    }
}
