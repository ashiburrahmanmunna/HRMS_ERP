using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderSub
    {
        public long OrdId { get; set; }
        public short SizeId { get; set; }
        public double OrdQty { get; set; }
        public short Pcb { get; set; }
        public short TtlCarton { get; set; }
        public short Ue { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public long? AdjOrd { get; set; }
        public byte? IsComplete { get; set; }

        public virtual TblCatSize Size { get; set; }
    }
}
