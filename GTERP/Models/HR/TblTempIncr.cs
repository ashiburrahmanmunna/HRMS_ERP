using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempIncr
    {
        public byte ComId { get; set; }
        public string IncType { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtInc { get; set; }
        public decimal? Amount { get; set; }
        public float? Per { get; set; }
        public decimal? PrevSal { get; set; }
        public decimal? NewSal { get; set; }
        public decimal? Bs { get; set; }
        public decimal? Hr { get; set; }
        public decimal? Ma { get; set; }
        public string Remarks { get; set; }
    }
}
