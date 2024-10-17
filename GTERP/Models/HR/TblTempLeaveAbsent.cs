using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempLeaveAbsent
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime? DtJoin { get; set; }
        public short? DesigId { get; set; }
        public string DesigName { get; set; }
        public string Grade { get; set; }
        public short SectId { get; set; }
        public string SectName { get; set; }
        public string SubSectName { get; set; }
        public string Shift { get; set; }
        public short? Year { get; set; }
        public short? Month { get; set; }
        public short? Day { get; set; }
        public float? TtlAb { get; set; }
        public float? Cma { get; set; }
        public float? Cml { get; set; }
        public float? Cya { get; set; }
        public float Cyl { get; set; }
        public float? Lyal { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Sslno { get; set; }
        public int? Dslno { get; set; }
        public string Absent { get; set; }
    }
}
