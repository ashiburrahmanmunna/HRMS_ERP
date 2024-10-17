using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductSupplier
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public int SupId { get; set; }
        public double MinOrdQty { get; set; }
        public double RateLast { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public int Scomid { get; set; }
        public DateTime? DtLastPurDate { get; set; }

        public virtual TblStrProduct Prd { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblCatSupplier Sup { get; set; }
    }
}
