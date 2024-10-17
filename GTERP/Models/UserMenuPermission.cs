using System.Collections.Generic;

namespace GTERP.Models
{
    public class UserMenuPermission
    {
        public int MenuPermissionId { get; set; }
        public int MenuPermissionDetailsId { get; set; }
        public int ModuleMenuId { get; set; }
        public string ModuleMenuLink { get; set; }
        public string ModuleMenuCaption { get; set; }
        public string ModuleMenuController { get; set; }
        public string ModuleMenuClass { get; set; }
        public string ModuleMenuClassi { get; set; }
        public int isInactive { get; set; }
        public int isParent { get; set; }
        public int ParentId { get; set; }
        public bool Active { get; set; }

        public bool Visible { get; set; }
        public int ModuleId { get; set; }


        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
        public bool IsReport { get; set; }

        public bool IsDefault { get; set; }


        public int SLNo { get; set; }

        public List<Module_Menu_Action> ActionList { get; set; }
        public string ComId { get; set; }
        public string UserId { get; set; }
        public string UserIdPermission { get; set; }
    }
}