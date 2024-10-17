using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblDeviceTable
    {
        public int Aid { get; set; }
        public int? TableId { get; set; }
        public string TableNo { get; set; }
        public string TableName { get; set; }
        public Guid? Wid { get; set; }
        public byte? Comid { get; set; }
        public int? Userid { get; set; }
        public int? SeatQty { get; set; }
        public string Prdimage { get; set; }
        public byte? Locationid { get; set; }
        public byte? Floorid { get; set; }
    }
}
