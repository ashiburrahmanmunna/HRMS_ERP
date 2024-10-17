using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempGroupAcc
    {
        public byte Comid { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public int ParentId { get; set; }
        public string ParentCode { get; set; }
        public string AccType { get; set; }
    }
}
