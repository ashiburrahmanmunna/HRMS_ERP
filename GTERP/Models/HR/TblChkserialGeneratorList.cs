using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblChkserialGeneratorList
    {
        public long AId { get; set; }
        public int GenId { get; set; }
        public string BankName { get; set; }
        public string BookNo { get; set; }
        public string AccountName { get; set; }
        public string Srl { get; set; }
        public string Sts { get; set; }
        public string DocId { get; set; }
        public string DocType { get; set; }
        public DateTime? StsDate { get; set; }
        public Guid? WId { get; set; }
    }
}
