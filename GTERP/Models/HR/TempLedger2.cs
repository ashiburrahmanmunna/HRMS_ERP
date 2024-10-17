using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempLedger2
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public string Caption2 { get; set; }
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? DtVoucher { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherDesc { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int AccId1 { get; set; }
        public string AccCode1 { get; set; }
        public string AccName1 { get; set; }
        public decimal Tkdebit { get; set; }
        public decimal Tkcredit { get; set; }
        public decimal Tkdebit1 { get; set; }
        public decimal Tkcredit1 { get; set; }
        public decimal Tkdebit2 { get; set; }
        public decimal Tkcredit2 { get; set; }
        public int RowNo { get; set; }
        public int RowDr { get; set; }
        public int RowCr { get; set; }
        public decimal Amount { get; set; }
        public int IntFlag { get; set; }
        public int IsBatch { get; set; }
        public string ImgName { get; set; }
        public string Referance { get; set; }
        public decimal? TkdebitFc { get; set; }
        public decimal? TkcreditFc { get; set; }
        public decimal? Tkdebit1Fc { get; set; }
        public decimal? Tkcredit1Fc { get; set; }
        public decimal? Tkdebit2Fc { get; set; }
        public decimal? Tkcredit2Fc { get; set; }
        public decimal? AmountFc { get; set; }
        public string ReferanceTwo { get; set; }
        public string ReferanceThree { get; set; }
    }
}
