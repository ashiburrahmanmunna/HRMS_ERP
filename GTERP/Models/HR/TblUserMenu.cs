using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblUserMenu
    {
        public int LuserId { get; set; }
        public int MenuId { get; set; }
        public Guid WId { get; set; }
        public int SortNo { get; set; }

        public virtual TblModuleMenu Menu { get; set; }
    }
}
