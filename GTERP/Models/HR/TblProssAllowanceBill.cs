using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProssAllowanceBill
    {
        public byte? ComId { get; set; }
        public long? EmpId { get; set; }
        public string EmpName { get; set; }
        public short? SectId { get; set; }
        public short? DesigId { get; set; }
        public short? DeptId { get; set; }
        public string Band { get; set; }
        public DateTime? DtDate { get; set; }
        public string ProssType { get; set; }
        public decimal? Gs { get; set; }
        public decimal? Bs { get; set; }
        public short? Present { get; set; }
        public short? Rate { get; set; }
        public short? Times { get; set; }
        public decimal? TotalAmount { get; set; }
        public string AllowType { get; set; }
        public decimal? TotalHrs { get; set; }
        public DateTime? Othour { get; set; }
        public decimal? Otrate { get; set; }
        public decimal? FirstOt { get; set; }
        public short? FirstOtday { get; set; }
        public decimal? FirstOtamt { get; set; }
        public decimal? RestOt { get; set; }
        public decimal? RestOtamt { get; set; }
        public byte? CheckYn { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public short? Cf { get; set; }
        public short Amount { get; set; }
        public short Tk1000 { get; set; }
        public short Tk500 { get; set; }
        public short Tk100 { get; set; }
        public short Tk50 { get; set; }
        public short Tk20 { get; set; }
        public short Tk10 { get; set; }
        public short Tk5 { get; set; }
        public float Covering { get; set; }
    }
}
