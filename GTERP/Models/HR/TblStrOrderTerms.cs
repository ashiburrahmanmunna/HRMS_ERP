using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderTerms
    {
        public long OrdId { get; set; }
        public string TermsType { get; set; }
        public string TermsData { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
    }
}
