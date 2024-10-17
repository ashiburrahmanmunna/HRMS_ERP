using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblUnclaimed
    {
        public byte? ComId { get; set; }
        public long? EmpId { get; set; }
        public DateTime? DtDate { get; set; }
        public string ProssType { get; set; }
        public string AllowType { get; set; }
        public decimal? Gs { get; set; }
        public decimal? Otrate { get; set; }
        public decimal? TotalHrs { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool PaidYn { get; set; }
        public string Remarks { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
    }
}
