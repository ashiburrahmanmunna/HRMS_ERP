using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TbltempSubStoreReport
    {
        public int? Prddisid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? LuserId { get; set; }
        public byte? ComId { get; set; }
        public byte? SubStoreId { get; set; }
        public double? OpBal { get; set; }
        public double? QtyRec { get; set; }
        public double? QtyIssue { get; set; }
        public double? EndBal { get; set; }
    }
}
