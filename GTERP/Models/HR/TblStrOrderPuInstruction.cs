using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderPuInstruction
    {
        public long BomId { get; set; }
        public string Instruction { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrPuMain Bom { get; set; }
    }
}
