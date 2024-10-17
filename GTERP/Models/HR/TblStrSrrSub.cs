using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSrrSub
    {
        public long SrrId { get; set; }
        public long PrdDisId { get; set; }
        public double QtyReq { get; set; }
        public double? QtyRcvd { get; set; }
        public string Remarks { get; set; }
        public long RowNo { get; set; }
        public Guid WId { get; set; }
        public short IsComplete { get; set; }
        public int? MachineId { get; set; }
        public int? IssuedId { get; set; }
        public string RemarksSrr { get; set; }
        public int? BomTypeId { get; set; }
        public double? OtherUnitQtyReq { get; set; }
        public long? OrdId { get; set; }
        public byte? ProcessId { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrSrrMain Srr { get; set; }
    }
}
