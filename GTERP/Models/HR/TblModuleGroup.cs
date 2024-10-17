using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblModuleGroup
    {
        public TblModuleGroup()
        {
            TblModuleMenu = new HashSet<TblModuleMenu>();
            TblUserGroup = new HashSet<TblUserGroup>();
        }

        public int MMenuGroupId { get; set; }
        public string MMenuGroupName { get; set; }
        public string MMenuGroupCaption { get; set; }
        public byte ModuleId { get; set; }
        public string ContainerType { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }

        public virtual TblModule Module { get; set; }
        public virtual ICollection<TblModuleMenu> TblModuleMenu { get; set; }
        public virtual ICollection<TblUserGroup> TblUserGroup { get; set; }
    }
}
