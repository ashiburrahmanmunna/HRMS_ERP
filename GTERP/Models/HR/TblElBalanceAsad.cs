using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblElBalanceAsad
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtStart { get; set; }
        public DateTime? DtEnd { get; set; }
        public DateTime? DtOpbal { get; set; }
        public int TtlPresent { get; set; }
        public int PrevEl { get; set; }
        public int Ael { get; set; }
        public int El { get; set; }
        public int CashedEl { get; set; }
        public int CurrBel { get; set; }
        public byte BalType { get; set; }
        public long AId { get; set; }
        public string Remarks { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
    }
}
