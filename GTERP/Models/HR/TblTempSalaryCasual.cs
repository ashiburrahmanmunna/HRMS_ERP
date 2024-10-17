using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempSalaryCasual
    {
        public byte? ComId { get; set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime? DtJoin { get; set; }
        public short? DesigId { get; set; }
        public string DesigName { get; set; }
        public short? SectId { get; set; }
        public string SectName { get; set; }
        public string EmpType { get; set; }
        public DateTime? DtDate { get; set; }
        public string ProssType { get; set; }
        public short? WorkingDays { get; set; }
        public short? Present { get; set; }
        public short? TotalHrs { get; set; }
        public decimal? PerHrAmt { get; set; }
        public decimal? Gs { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public string DtDays { get; set; }
        public DateTime? RegHour { get; set; }
        public int? TtlSum { get; set; }
        public string Inwords { get; set; }
        public short Tk1000 { get; set; }
        public short Tk500 { get; set; }
        public short Tk100 { get; set; }
        public short Tk50 { get; set; }
        public short Tk20 { get; set; }
        public short Tk10 { get; set; }
        public short Tk5 { get; set; }
        public byte? CscomId { get; set; }
        public string CscomName { get; set; }
    }
}
