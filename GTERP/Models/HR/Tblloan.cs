using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class Tblloan
    {
        public int? Empid { get; set; }
        public string EmpCode { get; set; }
        public string Empname { get; set; }
        public int? LoanId { get; set; }
        public int? ClosingBalance { get; set; }
        public int? Opbalance { get; set; }
        public string Pass { get; set; }
        public DateTime? DtLoanFrom { get; set; }
        public DateTime? DtLoanTo { get; set; }
        public int? Debit { get; set; }
        public int? Credit { get; set; }
        public int? Cash { get; set; }
        public DateTime? Inputdate { get; set; }
        public int? Gs { get; set; }
        public decimal? PayId { get; set; }
        public byte? ComId { get; set; }
        public decimal? Amount { get; set; }
        public int? InstNo { get; set; }
        public decimal? InsAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string Remarks { get; set; }
        public int? Wid { get; set; }
        public int? LuserId { get; set; }
        public float? Rate { get; set; }
        public string Pcname { get; set; }
        public int? AId { get; set; }
    }
}
