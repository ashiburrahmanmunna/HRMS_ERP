using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempVoucher
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string VtypeName { get; set; }
        public string VoucherDesc { get; set; }
        public byte IsAutoEntry { get; set; }
        public byte IsPosted { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int? RefId { get; set; }
        public string RefCode { get; set; }
        public string RefName { get; set; }
        public int? Ccid { get; set; }
        public string Cccode { get; set; }
        public string Ccname { get; set; }
        public string Cctype { get; set; }
        public string BizType { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public double Tkdebit { get; set; }
        public double Tkcredit { get; set; }
        public byte RowNo { get; set; }
        public string Currency { get; set; }
        public string Caption { get; set; }
        public decimal Vamount { get; set; }
        public string VamountInWords { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public decimal TkdebitLocal { get; set; }
        public decimal TkcreditLocal { get; set; }
        public string CurrencyLocal { get; set; }
        public float Rate { get; set; }
        public decimal VamountLocal { get; set; }
        public string Vtype { get; set; }
        public int? LuserId { get; set; }
        public int? LuserIdApprove { get; set; }
        public int? LuserIdCheck { get; set; }
        public string ForCheck { get; set; }
        public string ForeignCurrency { get; set; }
        public double? ForeignAmount { get; set; }
        public double? CurrencyRate { get; set; }
        public string ImgName { get; set; }
        public string Referance { get; set; }
        public string ReferanceTwo { get; set; }
        public string ReferanceThree { get; set; }
    }
}
