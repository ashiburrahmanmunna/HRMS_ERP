using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpFingerData
    {
        public long AId { get; set; }
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public string FingerData { get; set; }
        public byte Fpindex { get; set; }
        public byte Privilege { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
    }
}
