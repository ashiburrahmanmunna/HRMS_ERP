using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblNightAllow
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtDate { get; set; }
        public string ProssType { get; set; }
        public short? SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DeptId { get; set; }
        public short? DesigId { get; set; }
        public string Band { get; set; }
        public short Present { get; set; }
        public short Absent { get; set; }
        public short NightAbsent { get; set; }
        public decimal NightPay { get; set; }
        public decimal FoodPay { get; set; }
        public decimal NetPay { get; set; }
        public int? Cf { get; set; }
        public string ManualEntry { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
    }
}
