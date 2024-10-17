using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccCurrencyTran
    {
        public int Aid { get; set; }
        public DateTime TranDate { get; set; }
        public short CountryIdForeign { get; set; }
        public double AmountForeign { get; set; }
        public short CountryIdLocal { get; set; }
        public double AmountLocalBuy { get; set; }
        public double AmountLocalSale { get; set; }
        public Guid Wid { get; set; }
        public byte ComId { get; set; }
        public byte IsAutoEntry { get; set; }
    }
}
