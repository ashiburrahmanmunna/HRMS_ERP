using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProcessId
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessFullName { get; set; }
        public int? ProcessForProductId { get; set; }
        public int? Comid { get; set; }
        public int? UserId { get; set; }
        public Guid? Wid { get; set; }

        public virtual TblProcessForProduct ProcessForProduct { get; set; }
    }
}
