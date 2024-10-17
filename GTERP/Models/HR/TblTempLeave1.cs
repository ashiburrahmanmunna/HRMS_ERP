using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempLeave1
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public byte? AId { get; set; }
        public short? DtYear { get; set; }
        public string Status { get; set; }
        public float LeaveBalance { get; set; }
        public float LeaveUsed { get; set; }
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }
    }
}
