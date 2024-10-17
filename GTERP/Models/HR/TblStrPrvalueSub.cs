using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPrvalueSub
    {
        public int Prid { get; set; }
        public long PrdDisId { get; set; }
        public double QtyReq { get; set; }
        public double? UnitPrice { get; set; }
        public double? Amount { get; set; }
        public byte RowNo { get; set; }
        public Guid WiId { get; set; }
        public string Remarks { get; set; }

        public virtual TblStrPrvalueMain Pr { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
