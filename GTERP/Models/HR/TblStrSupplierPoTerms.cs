using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSupplierPoTerms
    {
        public long SupPoid { get; set; }
        public string TermsDetails { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte? Flag { get; set; }
        public byte? IsBold { get; set; }

        public virtual TblStrSupplierPoMain SupPo { get; set; }
    }
}
