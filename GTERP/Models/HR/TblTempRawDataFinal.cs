using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempRawDataFinal
    {
        public byte? ComId { get; set; }
        public string EmpId { get; set; }
        public DateTime? PunchDate { get; set; }
        public TimeSpan? PunchTime { get; set; }
        public int? RowNo { get; set; }
    }
}
