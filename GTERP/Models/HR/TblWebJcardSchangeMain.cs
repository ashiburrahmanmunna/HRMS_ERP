using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebJcardSchangeMain
    {
        public int SchangeId { get; set; }
        public int AppliedId { get; set; }
        public int ApprovedId { get; set; }
        public string Note { get; set; }
        public byte? ApprovedStatus { get; set; }
        public DateTime DtApply { get; set; }
        public DateTime? DtApprove { get; set; }
        public int AId { get; set; }
    }
}
