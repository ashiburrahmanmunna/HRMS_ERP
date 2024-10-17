using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempPrdItem1
    {
        public byte ComId { get; set; }
        public long? PrdDisId { get; set; }
        public string PrdName { get; set; }
        public string Spec { get; set; }
        public string ColorName { get; set; }
        public string UnitName { get; set; }
        public string SupplierName { get; set; }
        public double? Qty { get; set; }
    }
}
