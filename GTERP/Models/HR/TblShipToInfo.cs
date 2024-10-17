using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblShipToInfo
    {
        public byte ShipInfoId { get; set; }
        public string ShpName { get; set; }
        public string ShpCmpName { get; set; }
        public string ShpAddress { get; set; }
        public string ShpZip { get; set; }
        public string ShpPhone { get; set; }
        public string ShpFax { get; set; }
        public string ShpEmail { get; set; }
        public string ApplicationBegin { get; set; }
        public int Aid { get; set; }
        public Guid? Wid { get; set; }
        public byte Comid { get; set; }
        public byte? IsInactive { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
    }
}
