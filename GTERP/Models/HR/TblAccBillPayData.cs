using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccBillPayData
    {
        public long? AId { get; set; }
        public int? PayId { get; set; }
        public string PayIdh { get; set; }
        public string CequeNo { get; set; }
        public string BankName { get; set; }
        public string CompName { get; set; }
        public string DocNo { get; set; }
        public string DocTye { get; set; }
        public double? Amount { get; set; }
        public string Remarks { get; set; }

        public virtual TblAccBillPay Pay { get; set; }
    }
}
