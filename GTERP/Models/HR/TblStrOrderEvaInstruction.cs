using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderEvaInstruction
    {
        public long Evaid { get; set; }
        public string Instruction { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblStrOrderEvaMain Eva { get; set; }
    }
}
