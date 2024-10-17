using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpTraining
    {
        public byte? ComId { get; set; }
        public long EmpId { get; set; }
        public short? DeptId { get; set; }
        public short? SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DesigId { get; set; }
        public string Band { get; set; }
        public DateTime? DtJoin { get; set; }
        public string EmpType { get; set; }
        public string Tname { get; set; }
        public string InstrName { get; set; }
        public int? Tduration { get; set; }
        public DateTime? DtFirst { get; set; }
        public DateTime? DtLast { get; set; }
        public long AId { get; set; }
        public string Remarks { get; set; }
        public float Cost { get; set; }
    }
}
