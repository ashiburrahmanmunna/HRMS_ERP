using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblwebLeaveBalance
    {
        public int LvBalId { get; set; }
        public int EmpId { get; set; }
        public double? TtlCl { get; set; }
        public double? TtlSl { get; set; }
        public double? TtlEl { get; set; }
        public double? EnCl { get; set; }
        public double? EnSl { get; set; }
        public double? EnEl { get; set; }
        public double? BalCl { get; set; }
        public double? BalSl { get; set; }
        public double? BalEl { get; set; }
        public DateTime? DtOpenBalance { get; set; }
    }
}
