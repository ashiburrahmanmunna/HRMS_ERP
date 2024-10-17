using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblPoOrderTemp
    {
        public string ProductName { get; set; }
        public string Spec { get; set; }
        public string Colour { get; set; }
        public string Unit { get; set; }
        public decimal? BomQty { get; set; }
        public decimal? QtyOrder { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? AvailableQty { get; set; }
        public decimal? PipelineQty { get; set; }
        public int? Prddisid { get; set; }
        public decimal? Moq { get; set; }
        public decimal? Ppissued { get; set; }
    }
}
