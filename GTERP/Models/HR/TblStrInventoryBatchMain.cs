using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrInventoryBatchMain
    {
        public TblStrInventoryBatchMain()
        {
            TblStrInventoryBatchSub = new HashSet<TblStrInventoryBatchSub>();
        }

        public byte ComId { get; set; }
        public long InvId { get; set; }
        public int DeptId { get; set; }
        public DateTime DtConfig { get; set; }
        public decimal TtlWeight { get; set; }
        public byte Wastage { get; set; }
        public long PrdDisId { get; set; }
        public short ColorId { get; set; }
        public string ProductName { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public string InvNo { get; set; }

        public virtual ICollection<TblStrInventoryBatchSub> TblStrInventoryBatchSub { get; set; }
    }
}
