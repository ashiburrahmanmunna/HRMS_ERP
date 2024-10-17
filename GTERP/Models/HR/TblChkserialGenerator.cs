using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblChkserialGenerator
    {
        public long? AId { get; set; }
        public int GenId { get; set; }
        public string BankName { get; set; }
        public string BookNo { get; set; }
        public string AccountName { get; set; }
        public int? SrlFrm { get; set; }
        public int? SrlTo { get; set; }
        public string ChkNo { get; set; }
        public int? ComId { get; set; }
        public int? IsGen { get; set; }
        public DateTime? GenDate { get; set; }
        public Guid? Wid { get; set; }
    }
}
