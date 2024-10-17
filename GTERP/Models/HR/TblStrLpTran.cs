using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLpTran
    {
        public long Lpid { get; set; }
        public long PrdDisId { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public long TranId { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrLpMain Lp { get; set; }
    }
}
