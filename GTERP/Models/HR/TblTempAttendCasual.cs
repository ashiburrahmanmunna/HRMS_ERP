using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempAttendCasual
    {
        public byte ComId { get; set; }
        public short SectId { get; set; }
        public string SectName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string CardNo { get; set; }
        public short DesigId { get; set; }
        public string DesigName { get; set; }
        public short ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public string Status { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? RegHour { get; set; }
        public DateTime? Othour { get; set; }
        public float? Othr { get; set; }
        public string Remarks { get; set; }
        public int Sl { get; set; }
        public int? SslNo { get; set; }
        public int? DslNo { get; set; }
        public int? Slno { get; set; }
        public string Pstatus { get; set; }
        public DateTime? PtimeIn { get; set; }
        public DateTime? PtimeOut { get; set; }
        public int? AbTn { get; set; }
        public DateTime? DtFromDate { get; set; }
        public string EmpType { get; set; }
        public short DeptId { get; set; }
        public string DeptName { get; set; }
        public string Band { get; set; }
        public DateTime? DtJoin { get; set; }
        public string Operation { get; set; }
        public short? CscomId { get; set; }
    }
}
