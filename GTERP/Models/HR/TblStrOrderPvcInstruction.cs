using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderPvcInstruction
    {
        public long Sppid { get; set; }
        public string Instruction { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrOrderPvcMain Spp { get; set; }
    }
}
