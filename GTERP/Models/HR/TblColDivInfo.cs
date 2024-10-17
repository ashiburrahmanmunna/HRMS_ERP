using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblColDivInfo
    {
        public long Aid { get; set; }
        public int? Prdid { get; set; }
        public byte ComId { get; set; }
        public DateTime? DtProcess { get; set; }
        public DateTime? DtRevisedDate { get; set; }
        public string Style { get; set; }
        public string RepNo { get; set; }
        public double? OrdQty { get; set; }
        public double? CutQty { get; set; }
        public int? PcsBundle { get; set; }
        public int? ExStc { get; set; }
        public int? StartStkNo { get; set; }
        public double? TtlCuttedQty { get; set; }
        public DateTime? DtDelivery { get; set; }
        public string PcName { get; set; }
        public int LuserId { get; set; }
        public Guid? Wid { get; set; }
        public string Colour { get; set; }
    }
}
