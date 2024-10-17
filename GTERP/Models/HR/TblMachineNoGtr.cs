using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblMachineNoGtr
    {
        public byte ComId { get; set; }
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public byte IsActive { get; set; }
        public string Status { get; set; }
    }
}
