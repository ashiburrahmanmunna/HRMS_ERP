using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempEmail
    {
        public long EmpId { get; set; }
        public long Cnt { get; set; }
        public int Otcnt { get; set; }
        public float Ot { get; set; }
        public int Incr { get; set; }
        public int FixAtt { get; set; }
        public string Type { get; set; }
        public long AId { get; set; }
    }
}
