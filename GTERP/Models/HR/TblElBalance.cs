using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblElBalance
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtStart { get; set; }
        public DateTime? DtEnd { get; set; }
        public DateTime? DtOpbal { get; set; }
        public float TtlPresent { get; set; }
        public float PrevEl { get; set; }
        public float Ael { get; set; }
        public float El { get; set; }
        public float CashedEl { get; set; }
        public float CurrBel { get; set; }
        public int BalType { get; set; }
        public long AId { get; set; }
        public string Remarks { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
        public DateTime? DtPayment { get; set; }
    }
}
