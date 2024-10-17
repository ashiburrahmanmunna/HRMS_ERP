using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPoTerms
    {
        public long Poid { get; set; }
        public string TermsDetails { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte? Flag { get; set; }

        public virtual TblStrPoMain Po { get; set; }
    }
}
