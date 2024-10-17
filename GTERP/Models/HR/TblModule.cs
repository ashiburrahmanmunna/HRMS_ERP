using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblModule
    {
        public TblModule()
        {
            TblModuleGroup = new HashSet<TblModuleGroup>();
            TblUserModule = new HashSet<TblUserModule>();
        }

        public byte ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCaption { get; set; }
        public int? DayEndYn { get; set; }
        public byte IsInactive { get; set; }
        public string ReportPath { get; set; }
        public byte IsTranRelated { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
        public int AId { get; set; }
        public Guid Wid { get; set; }

        public virtual ICollection<TblModuleGroup> TblModuleGroup { get; set; }
        public virtual ICollection<TblUserModule> TblUserModule { get; set; }
    }
}
