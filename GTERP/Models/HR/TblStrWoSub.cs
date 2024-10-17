using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrWoSub
    {
        public long Woid { get; set; }
        public long PrdDisId { get; set; }
        public double QtyOrder { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte IsComplete { get; set; }
        public string RemarksWo { get; set; }
        public int Prid { get; set; }
        public int? InvoiceId { get; set; }
        public double QtyOrderOtherUnit { get; set; }
        public double OtherUnitQtyOrder { get; set; }
        public long? OrdId { get; set; }
        public byte? ProcessId { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrWoMain Wo { get; set; }
    }
}
