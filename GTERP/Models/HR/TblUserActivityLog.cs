using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblUserActivityLog
    {
        public int Slno { get; set; }
        public int LuserId { get; set; }
        public DateTime TranDate { get; set; }
        public DateTime TranStartTime { get; set; }
        public DateTime? TranEndTime { get; set; }
        public string LoginPcname { get; set; }
        public string LoginPcip { get; set; }
        public string LoginPcmac { get; set; }
        public string TranFormName { get; set; }
        public string TranType { get; set; }
        public int TranDataId { get; set; }
        public byte TranStatus { get; set; }
        public Guid Wid { get; set; }
        public int ComId { get; set; }
    }
}
