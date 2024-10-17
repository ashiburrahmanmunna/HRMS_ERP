using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempCadre
    {
        public byte? ComId { get; set; }
        public string UnitName { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public float? Actual { get; set; }
        public float? Absent { get; set; }
        public float? St { get; set; }
        public float Abper { get; set; }
        public float Stper { get; set; }
    }
}
