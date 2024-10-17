using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccVoucherSub
    {
        public int VoucherId { get; set; }
        public int AccId { get; set; }
        public int? RefId { get; set; }
        public int? CcId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public double Tkdebit { get; set; }
        public double Tkcredit { get; set; }
        public double TkdebitLocal { get; set; }
        public double TkcreditLocal { get; set; }
        public string ChkNo { get; set; }
        public DateTime? ChkDate { get; set; }
        public byte RowNo { get; set; }
        public int? AccId1 { get; set; }
        public int Ddd { get; set; }
        public string Vtype { get; set; }
        public double? Tcr { get; set; }
        public string Color { get; set; }
        public byte? ForeignCurrency { get; set; }
        public double? CurrencyRate { get; set; }
        public double? DebitForeign { get; set; }
        public double? CreditForeign { get; set; }
        public byte? Scomid { get; set; }
        public long? Basedid { get; set; }

        public virtual TblAccVoucherMain Voucher { get; set; }
    }
}
