using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblConfiguration
    {
        public byte ModuleId { get; set; }
        public string FlagName { get; set; }
        public string FlagValue { get; set; }
        public string Remarks { get; set; }
    }
}
