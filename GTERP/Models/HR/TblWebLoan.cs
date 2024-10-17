using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebLoan
    {
        public int LnId { get; set; }
        public long IdApplicant { get; set; }
        public DateTime DtApply { get; set; }
        public string ApplyType { get; set; }
        public int AmountApply { get; set; }
        public string ResonApply { get; set; }
        public byte Status { get; set; }
        public DateTime? DtCheck { get; set; }
        public long? IdApproved { get; set; }
        public int? AmountApproved { get; set; }
        public string ResonDeny { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
    }
}
