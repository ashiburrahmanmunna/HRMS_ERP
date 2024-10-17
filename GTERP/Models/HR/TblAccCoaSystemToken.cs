using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccCoaSystemToken
    {
        public byte AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string AccType { get; set; }
        public int ParentId { get; set; }
        public string ParentCode { get; set; }
        public byte IsShowUg { get; set; }
    }
}
