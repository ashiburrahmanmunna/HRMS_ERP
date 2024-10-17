using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrTransactionSubInvalid
    {
        public long TranId { get; set; }
        public long PrdDisId { get; set; }
        public short Whid { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double IssueQty { get; set; }
        public double Balance { get; set; }
        public Guid WId { get; set; }
    }
}
