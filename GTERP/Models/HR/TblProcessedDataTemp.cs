using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProcessedDataTemp
    {
        public int ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string EmpCode { get; set; }
        public short? ShiftId { get; set; }
        public short? DeptId { get; set; }
        public short? SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DesigId { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? LunchIn { get; set; }
        public DateTime? LunchOut { get; set; }
        public DateTime? LunchLate { get; set; }
        public byte NoPunch { get; set; }
        public string Status { get; set; }
        public DateTime RegHour { get; set; }
        public DateTime Othour { get; set; }
        public float Ot { get; set; }
        public float PrevOt { get; set; }
        public float OthourMin { get; set; }
        public DateTime OthourDed { get; set; }
        public float OthourDedMin { get; set; }
        public float RegMin { get; set; }
        public float AbTn { get; set; }
        public float AdJusted { get; set; }
        public DateTime? Rot { get; set; }
        public DateTime? Eot { get; set; }
        public string Remarks { get; set; }
        public byte IsLunchDay { get; set; }
        public byte IsNightShift { get; set; }
        public string NshiftName { get; set; }
        public string Pstatus { get; set; }
        public DateTime? PtimeIn { get; set; }
        public DateTime? PtimeOut { get; set; }
        public string Band { get; set; }
        public float Whot { get; set; }
        public string ShiftType { get; set; }

        public virtual TblCatCompany Com { get; set; }
    }
}
