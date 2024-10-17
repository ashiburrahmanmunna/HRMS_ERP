using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSubStoreProduct
    {
        public int Aid { get; set; }
        public int? PrdDisId { get; set; }
        public double? OpBal { get; set; }
        public double? RecQty { get; set; }
        public double? IssueQty { get; set; }
        public Guid? Wid { get; set; }
        public int? Comid { get; set; }
        public int? SubStoreId { get; set; }
        public int? LuserId { get; set; }
        public double? EndBal { get; set; }
    }
}
