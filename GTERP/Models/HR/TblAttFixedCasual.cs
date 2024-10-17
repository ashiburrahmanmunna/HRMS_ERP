using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAttFixedCasual
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Othour { get; set; }
        public string Status { get; set; }
        public byte IsInactive { get; set; }
        public string Remarks { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public float Othr { get; set; }
    }
}
