using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblMasterUserTransLog
    {
        public int LuserId { get; set; }
        public string FormName { get; set; }
        public string TranStatement { get; set; }
        public DateTime TranDate { get; set; }
        public DateTime TranTime { get; set; }
        public string TranType { get; set; }
        public Guid Wid { get; set; }
        public string Pcname { get; set; }
    }
}
