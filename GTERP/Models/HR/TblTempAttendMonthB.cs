using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempAttendMonthB
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public short? DesigId { get; set; }
        public string DesigName { get; set; }
        public string Grade { get; set; }
        public short SectId { get; set; }
        public string SectName { get; set; }
        public short? DeptId { get; set; }
        public string DeptName { get; set; }
        public string Band { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public byte DayMonth { get; set; }
        public float DayTtl { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float LateDay { get; set; }
        public float Leave { get; set; }
        public float Hday { get; set; }
        public float Wday { get; set; }
        public float Cl { get; set; }
        public float El { get; set; }
        public float Sl { get; set; }
        public float Ml { get; set; }
        public float AccL { get; set; }
        public float Lwp { get; set; }
        public float LateHrs { get; set; }
        public float EarlyLvHrs { get; set; }
        public float ShortLvHrs { get; set; }
        public string OthrsTtl { get; set; }
        public byte Lunch { get; set; }
        public byte Night { get; set; }
        public float Othr { get; set; }
        public string LateHrTtl { get; set; }
        public float OthrDed { get; set; }
        public float? Rot { get; set; }
        public float? Eot { get; set; }
        public int? Sslno { get; set; }
        public int? Dslno { get; set; }
        public int? ShiftId { get; set; }
        public decimal? Gs { get; set; }
        public decimal? Bs { get; set; }
        public float? Otrate { get; set; }
        public decimal? Ot { get; set; }
        public string SubSectName { get; set; }
        public string DaysCnt { get; set; }
        public short SubSectId { get; set; }
    }
}
