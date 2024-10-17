using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductSupplierTest
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public int SupId { get; set; }
        public double MinOrdQty { get; set; }
        public double RateLast { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public int Scomid { get; set; }
        public DateTime DtLastPurDate { get; set; }
    }
}
