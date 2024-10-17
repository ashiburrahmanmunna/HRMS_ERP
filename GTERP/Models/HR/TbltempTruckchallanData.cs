using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TbltempTruckchallanData
    {
        public int? Challanid { get; set; }
        public int? Prdid { get; set; }
        public int? Prddisid { get; set; }
        public double? QtyDel { get; set; }
        public int? Unitid { get; set; }
        public int? Comid { get; set; }
        public int? Userid { get; set; }
        public int? Vatid { get; set; }
        public int? Dorowno { get; set; }
        public int? Rowno { get; set; }
        public int? Truckrowno { get; set; }
        public int Luserid { get; set; }
        public double? BundleQty { get; set; }
        public double? TotalBundle { get; set; }
        public string TruckRemarks { get; set; }
        public string BundleDescription { get; set; }
        public string BundleQtyMerge { get; set; }
    }
}
