using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccClosingStock
    {
        public byte ComId { get; set; }
        public DateTime DtClose { get; set; }
        public int AccId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountLocal { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public decimal ConvRate { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public long Slno { get; set; }
    }
}
