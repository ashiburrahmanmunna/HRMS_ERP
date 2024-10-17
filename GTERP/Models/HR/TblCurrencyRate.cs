using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblCurrencyRate
    {
        public int Aid { get; set; }
        public byte CurrencyId { get; set; }
        public DateTime DtDate { get; set; }
        public double Rate { get; set; }
        public Guid Wid { get; set; }
        public byte Comid { get; set; }
        public int LuserId { get; set; }
    }
}
