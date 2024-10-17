using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempIndLedger1
    {
        public byte ComId { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int? ContraAccId { get; set; }
        public string ContraAccCode { get; set; }
        public string ContraAccName { get; set; }
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Description { get; set; }
        public int Ccid { get; set; }
        public string Cccode { get; set; }
        public string Ccname { get; set; }
        public string Cctype { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public int RefId { get; set; }
        public string RefCode { get; set; }
        public string AreaName { get; set; }
        public decimal Tkdebit { get; set; }
        public decimal Tkcredit { get; set; }
        public byte? TranStatus { get; set; }
        public byte? AccStatus { get; set; }
        public byte ShowStatus { get; set; }
        public byte TotRow { get; set; }
        public string TranAccCode { get; set; }
        public string TranAccName { get; set; }
        public string Currency { get; set; }
        public byte ShowInLocalCurr { get; set; }
        public string Source { get; set; }
    }
}
