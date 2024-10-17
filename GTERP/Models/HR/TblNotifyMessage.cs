using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblNotifyMessage
    {
        public int? Luserid { get; set; }
        public string Message { get; set; }
        public byte? IsSeen { get; set; }
    }
}
