using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempRmBase
    {
        public byte ComId { get; set; }
        public long AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string FlagType { get; set; }
        public byte FlagValue { get; set; }
        public short SortNo { get; set; }
    }
}
