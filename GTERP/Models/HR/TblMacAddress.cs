using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblMacAddress
    {
        public int? Aid { get; set; }
        public string Macaddress { get; set; }
        public int? Comid { get; set; }
        public string Pcname { get; set; }
        public DateTime Inputdate { get; set; }
        public int? ValidDays { get; set; }
        public DateTime? DateEx { get; set; }
        public string ActiveYesNo { get; set; }
    }
}
