using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblInputPermission
    {
        public int Pid { get; set; }
        public string Ptype { get; set; }
        public string EmpType { get; set; }
        public string ComName { get; set; }
        public byte ComId { get; set; }
        public int FirstAppId { get; set; }
        public int FinalAppId { get; set; }
        public byte AppFirst { get; set; }
        public byte AppFinal { get; set; }
        public byte IsInactive { get; set; }
        public string Remarks { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
    }
}
