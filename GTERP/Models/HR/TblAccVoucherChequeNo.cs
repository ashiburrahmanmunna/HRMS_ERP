using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccVoucherChequeNo
    {
        public int Aid { get; set; }
        public int VoucherId { get; set; }
        public int Accid { get; set; }
        public string ChequeNo { get; set; }
        public byte Rowno { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Remarks { get; set; }
        public double? Amount { get; set; }
        public Guid? Wid { get; set; }
        public int? CrowNo { get; set; }

        public virtual TblAccVoucherMain Voucher { get; set; }
    }
}
