using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrWoTerms
    {
        public long Woid { get; set; }
        public string TermsDetails { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte? Flag { get; set; }

        public virtual TblStrWoMain Wo { get; set; }
    }
}
