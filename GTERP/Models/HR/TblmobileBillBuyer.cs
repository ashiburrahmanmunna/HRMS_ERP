using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblmobileBillBuyer
    {
        public int? Aempid { get; set; }
        public string Empid { get; set; }
        public string Empname { get; set; }
        public string Vempdesig { get; set; }
        public string Vempsec { get; set; }
        public string Mobileno { get; set; }
        public int? Allowance { get; set; }
        public int? Prevbill { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public DateTime? Date { get; set; }
        public string Vprosstype { get; set; }
    }
}
