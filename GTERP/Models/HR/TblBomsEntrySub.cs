using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomsEntrySub
    {
        public int? Bomid { get; set; }
        public string SlNo { get; set; }
        public string PartNo { get; set; }
        public string PrdName { get; set; }
        public string Spec { get; set; }
        public int? Prddisid { get; set; }
        public int? Prdid { get; set; }
        public string Unit { get; set; }
        public double? Quantity { get; set; }
        public string PositionNo { get; set; }
        public string DrawingNo { get; set; }
        public string Remarks { get; set; }
        public double? UnitPrice { get; set; }
        public double? Amount { get; set; }
        public byte? Comid { get; set; }
        public int? LuserId { get; set; }
        public int? Rowno { get; set; }
    }
}
