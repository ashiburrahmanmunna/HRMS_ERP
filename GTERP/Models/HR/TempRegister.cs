using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempRegister
    {
        public int? PrdId { get; set; }
        public int? Prddisid { get; set; }
        public double? OpQty { get; set; }
        public double? RcvQtyGrr { get; set; }
        public double? RcvLoanIn { get; set; }
        public double? AvailQty { get; set; }
        public double? IssueQtyissue { get; set; }
        public double? IssueQtyLoanOut { get; set; }
        public double? Cqty { get; set; }
        public string Remarks { get; set; }
        public double OpQtyOtherUnit { get; set; }
        public double RcvQtyGrrotherUnit { get; set; }
        public double RcvLoanInOtherUnit { get; set; }
        public double IssueQtyissueOtherUnit { get; set; }
        public double IssueQtyLoanOutOtherUnit { get; set; }
        public double CqtyOtherUnit { get; set; }
        public double AvailQtyOtherUnit { get; set; }
    }
}
