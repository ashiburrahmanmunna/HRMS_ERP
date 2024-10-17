using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginUserMenuSub
    {
        public int? UserMenuId { get; set; }
        public int? MenuId { get; set; }
        public string AId { get; set; }

        public virtual TblLoginMenuWeb Menu { get; set; }
        public virtual TblLoginUserMenuMain UserMenu { get; set; }
    }
}
