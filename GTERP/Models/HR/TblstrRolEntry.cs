using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblstrRolEntry
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public string PrdName { get; set; }
        public string Spec { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string ColorName { get; set; }
        public string Unit { get; set; }
        public int? Intialiaze { get; set; }
        public float? Qty { get; set; }
        public float? Average { get; set; }
        public float? Average1 { get; set; }
        public int? LtTime { get; set; }
        public float? SftStock { get; set; }
        public float? Rol { get; set; }
        public int? Year { get; set; }
        public byte? ComId { get; set; }
        public byte? LuserId { get; set; }
    }
}
