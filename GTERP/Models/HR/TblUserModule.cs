using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblUserModule
    {
        public int LuserId { get; set; }
        public byte ModuleId { get; set; }
        public byte IsDefault { get; set; }
        public byte SortNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblModule Module { get; set; }
    }
}
