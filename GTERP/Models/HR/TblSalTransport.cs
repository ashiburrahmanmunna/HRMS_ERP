using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalTransport
    {
        public byte ComId { get; set; }
        public long TrnId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtInput { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int? AId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
    }
}
