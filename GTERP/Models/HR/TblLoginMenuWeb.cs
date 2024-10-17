using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginMenuWeb
    {
        public TblLoginMenuWeb()
        {
            TblLoginUserMenuSub = new HashSet<TblLoginUserMenuSub>();
        }

        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuController { get; set; }
        public string MenuLink { get; set; }
        public int ParentId { get; set; }
        public byte IsParent { get; set; }
        public byte IsInactive { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public byte IsSecurityModule { get; set; }
        public short AId { get; set; }
        public string MenuIcon { get; set; }
        public byte? IsBaseMenu { get; set; }
        public byte? IsLabel { get; set; }
        public byte? IsLabelParent { get; set; }
        public byte? IsHeader { get; set; }
        public string MenuLiId { get; set; }

        public virtual ICollection<TblLoginUserMenuSub> TblLoginUserMenuSub { get; set; }
    }
}
