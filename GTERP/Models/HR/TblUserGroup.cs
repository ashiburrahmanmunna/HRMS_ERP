using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblUserGroup
    {
        public int LuserId { get; set; }
        public int MMenuGroupId { get; set; }
        public Guid Wid { get; set; }
        public int SortNo { get; set; }

        public virtual TblModuleGroup MMenuGroup { get; set; }
    }
}
