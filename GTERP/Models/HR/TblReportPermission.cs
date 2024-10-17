using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblReportPermission
    {
        public int Aid { get; set; }
        public int? Id { get; set; }
        public int? LuseridRpt { get; set; }
        public byte IsCheck { get; set; }
        public int? Luserid { get; set; }
        public int? Comid { get; set; }
    }
}
