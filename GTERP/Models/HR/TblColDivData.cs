using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblColDivData
    {
        public long Aid { get; set; }
        public int? PrdId { get; set; }
        public string Color { get; set; }
        public double? OrdQty { get; set; }
        public double? OrdCutQty { get; set; }
        public int? ReqMacQty { get; set; }
        public double? AchTargetHour { get; set; }
        public double? OptTargetHour { get; set; }
        public double? TtlOpTargetHour { get; set; }
        public double? GroupTargetHour { get; set; }
        public int? RowNo { get; set; }
        public string EntryNo { get; set; }
        public string Remarks { get; set; }
        public int? EndStkNo { get; set; }
        public string BundleSticker { get; set; }
        public int? TtlcuttedQty { get; set; }
        public byte ComId { get; set; }
        public byte? LuserId { get; set; }
        public string PcName { get; set; }
    }
}
