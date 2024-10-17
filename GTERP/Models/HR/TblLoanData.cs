using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoanData
    {
        public byte? ComId { get; set; }
        public int? EmpId { get; set; }
        public int? LoanId { get; set; }
        public decimal? Amount { get; set; }
        public int? InstallmentNo { get; set; }
        public int? LoanAmt { get; set; }
        public int? InstAmt { get; set; }
        public int? Balance { get; set; }
        public int? InterestAmt { get; set; }
        public int? FlatAmt { get; set; }
        public int? DeductAmt { get; set; }
        public DateTime? DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public int? AId { get; set; }
        public Guid? WId { get; set; }
        public string PassYesNo { get; set; }
        public float? Rate { get; set; }
        public decimal LoanBalance { get; set; }
    }
}
