using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBankSheet
    {
        public int? Aempid { get; set; }
        public string Empid { get; set; }
        public string Empcode { get; set; }
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
        public string DesigId { get; set; }
        public string SectId { get; set; }
        public string Vprosstype { get; set; }
        public double? Bnetpayable { get; set; }
        public double? Bbankpayable { get; set; }
        public int? UserId { get; set; }
        public byte? ComId { get; set; }
    }
}
