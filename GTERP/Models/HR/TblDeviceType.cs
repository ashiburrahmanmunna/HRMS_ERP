using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblDeviceType
    {
        public int Aid { get; set; }
        public int? DeviceTypeId { get; set; }
        public string DeviceTypeNo { get; set; }
        public string DeviceTypeName { get; set; }
        public Guid? Wid { get; set; }
        public byte? Comid { get; set; }
        public int? Userid { get; set; }
        public int? Capacity { get; set; }
        public string PrdimageActive { get; set; }
        public string PrdimageInActive { get; set; }
        public string PrdimageAlert { get; set; }
    }
}
