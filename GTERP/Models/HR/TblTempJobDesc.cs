using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempJobDesc
    {
        public string DesigId { get; set; }
        public string VEmpDesig { get; set; }
        public string VEmpSec { get; set; }
        public DateTime? PostDate { get; set; }
        public string AuthorizedBy { get; set; }
        public string JobDesc { get; set; }
    }
}
