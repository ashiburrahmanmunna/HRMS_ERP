using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblDbbackup
    {
        public byte Slno { get; set; }
        public string DbName { get; set; }
        public string DbCaption { get; set; }
        public string Location { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtTime { get; set; }
        public short? LuserId { get; set; }
    }
}
