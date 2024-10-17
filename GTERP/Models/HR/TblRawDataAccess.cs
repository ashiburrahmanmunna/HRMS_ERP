using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblRawDataAccess
    {
        public string DeviceNo { get; set; }
        public string CardNo { get; set; }
        public string Fpid { get; set; }
        public int? EmpId { get; set; }
        public string DtPunchDate { get; set; }
        public string DtPunchTime { get; set; }
        public long? StNo { get; set; }
        public string InOut { get; set; }
        public string OvNmark { get; set; }
        public byte? IsNew { get; set; }
        public long AId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
    }
}
