using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTermsDetails
    {
        public int TermsId { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public byte RowNo { get; set; }
        public byte Flag { get; set; }
        public byte IsSysGenerate { get; set; }
    }
}
