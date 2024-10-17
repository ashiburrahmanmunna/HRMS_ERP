using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTemplate
    {
        public int AccId { get; set; }
        public string AccName { get; set; }
        public int RefId { get; set; }
        public string RefName { get; set; }
        public string Note1 { get; set; }
        public int Tkdebit { get; set; }
        public int Tkcredit { get; set; }
        public byte Slno { get; set; }
    }
}
