using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TbltempEl
    {
        public long EmpId { get; set; }
        public DateTime? DtStart { get; set; }
        public DateTime? DtEnd { get; set; }
        public int TtlPresent { get; set; }
        public int PrevEl { get; set; }
        public int Ael { get; set; }
        public int El { get; set; }
        public int CashedEl { get; set; }
        public int CurrBel { get; set; }
    }
}
