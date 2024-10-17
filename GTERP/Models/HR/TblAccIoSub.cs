using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccIoSub
    {
        public int? IoId { get; set; }
        public string RefType { get; set; }
        public int? RefId { get; set; }
        public double? Amnt { get; set; }
        public double? AmntLocal { get; set; }
        public int? RowNo { get; set; }
    }
}
