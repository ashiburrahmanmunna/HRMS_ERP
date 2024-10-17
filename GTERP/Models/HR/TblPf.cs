using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblPf
    {
        public double? Sl { get; set; }
        public double? NewEmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? DtPf { get; set; }
        public string Wrs { get; set; }
        public double? RegNo { get; set; }
        public string UnitName { get; set; }
        public double? OwnCon { get; set; }
        public double? ComCon { get; set; }
        public double? Interest { get; set; }
        public double? Total { get; set; }
        public byte? ComId { get; set; }
    }
}
