using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuLink { get; set; }
        public int ParentId { get; set; }
        public byte IsInactive { get; set; }
        public byte IsDefault { get; set; }
        public int Aid { get; set; }
    }
}
