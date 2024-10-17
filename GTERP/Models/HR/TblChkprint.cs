using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblChkprint
    {
        public int AId { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public double? Amt { get; set; }
        public string InWords { get; set; }
        public Guid? Wid { get; set; }
        public short? ComId { get; set; }
        public string DocType { get; set; }
        public string DocId { get; set; }
    }
}
