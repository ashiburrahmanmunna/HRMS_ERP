using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblDnXls
    {
        public byte ComId { get; set; }
        public string XlsFileName { get; set; }
        public DateTime? DtProcess { get; set; }
        public string EntryNo { get; set; }
        public short CarId { get; set; }
        public string Remarks { get; set; }
        public int? EmpId { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? OtHour { get; set; }
        public string Status { get; set; }
        public string PcName { get; set; }
        public int? LuserId { get; set; }
    }
}
