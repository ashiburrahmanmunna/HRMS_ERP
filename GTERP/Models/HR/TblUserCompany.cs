using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblUserCompany
    {
        public int LuserId { get; set; }
        public int ComId { get; set; }
        public byte IsDefault { get; set; }
        public byte SortNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblCatCompany Com { get; set; }
    }
}
