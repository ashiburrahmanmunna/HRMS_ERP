using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginActivityLog
    {
        public int Slno { get; set; }
        public int LuserId { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LoginStartTime { get; set; }
        public DateTime? LoginEndTime { get; set; }
        public string LoginPcname { get; set; }
        public string LoginPcip { get; set; }
        public string LoginPcmac { get; set; }
        public Guid Wid { get; set; }
    }
}
