using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblModuleMenu
    {
        public TblModuleMenu()
        {
            TblUserMenu = new HashSet<TblUserMenu>();
        }

        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuCaption { get; set; }
        public int MMenuGroupId { get; set; }
        public byte MenuImageExist { get; set; }
        public byte MenuImageSize { get; set; }
        public string MenuImageName { get; set; }
        public string FrmName { get; set; }
        public string FrmLocation { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public byte HasChild { get; set; }
        public byte IsFormBased { get; set; }
        public byte IsDropDown { get; set; }
        public byte IsDropDownParent { get; set; }
        public short DropDownParentId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }

        public virtual TblModuleGroup MMenuGroup { get; set; }
        public virtual ICollection<TblUserMenu> TblUserMenu { get; set; }
    }
}
