using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblIncenManualEntry
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public string ProssType { get; set; }
        public decimal? IncenBns { get; set; }
        public decimal AttBonus { get; set; }
        public decimal? GradeAmt { get; set; }
        public short? NightAmt { get; set; }
    }
}
