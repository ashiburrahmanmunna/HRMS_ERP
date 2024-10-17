using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderPvcSub
    {
        public long Sppid { get; set; }
        public long OrdId { get; set; }
        public DateTime? DtComplete { get; set; }
        public string PpsoleNo { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrOrderPvcMain Spp { get; set; }
    }
}
