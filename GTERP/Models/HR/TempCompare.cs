using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempCompare
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string Caption { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string DateName1 { get; set; }
        public decimal? Tkdebit1 { get; set; }
        public decimal? Tkcredit1 { get; set; }
        public string DateName2 { get; set; }
        public decimal? Tkdebit2 { get; set; }
        public decimal? Tkcredit2 { get; set; }
        public byte TranStatus { get; set; }
    }
}
