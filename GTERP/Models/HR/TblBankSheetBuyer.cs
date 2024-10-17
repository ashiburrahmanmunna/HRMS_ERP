using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBankSheetBuyer
    {
        public int? Aempid { get; set; }
        public string Empid { get; set; }
        public string Empname { get; set; }
        public string AccountNo { get; set; }
        public DateTime? Date { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int? Netpayable { get; set; }
        public int? Suspense { get; set; }
        public int? Allowance { get; set; }
        public int? MobileBill { get; set; }
        public int? BankPayable { get; set; }
        public string Paytype { get; set; }
        public string Vempdesig { get; set; }
        public string Vempsec { get; set; }
        public string Vprosstype { get; set; }
        public int? Desigid { get; set; }
        public int? Sectid { get; set; }
    }
}
