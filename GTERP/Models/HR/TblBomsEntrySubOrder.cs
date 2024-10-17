using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomsEntrySubOrder
    {
        public int? OrdId { get; set; }
        public int? Prddisid { get; set; }
        public int? Prdid { get; set; }
        public string Unit { get; set; }
        public double? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? Amount { get; set; }
        public byte? Comid { get; set; }
        public int? LuserId { get; set; }
        public int? Rowno { get; set; }
        public int? BomRefId { get; set; }
    }
}
