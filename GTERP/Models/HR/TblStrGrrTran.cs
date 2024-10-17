using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrGrrTran
    {
        public long Grrid { get; set; }
        public long PrdDisId { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public long TranId { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrGrrMain Grr { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
