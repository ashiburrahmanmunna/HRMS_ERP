using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblMachineSerial
    {
        public byte ComId { get; set; }
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string DeviceSerial { get; set; }
    }
}
