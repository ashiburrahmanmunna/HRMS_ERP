using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblGradeEntry
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtDate { get; set; }
        public string ProssType { get; set; }
        public short? SectId { get; set; }
        public short? DesigId { get; set; }
        public short? DeptId { get; set; }
        public string Band { get; set; }
        public string Grade { get; set; }
        public decimal? Amount { get; set; }
    }
}
