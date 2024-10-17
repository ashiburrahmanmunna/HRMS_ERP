using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAttFixedMonthly
    {
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpDesig { get; set; }
        public string EmpSec { get; set; }
        public DateTime SalMonth { get; set; }
        public double Present { get; set; }
        public double Absent { get; set; }
        public double Late { get; set; }
        public double Cl { get; set; }
        public double El { get; set; }
        public double Sl { get; set; }
        public double Whday { get; set; }
        public string OverTime { get; set; }
        public double ProdBns { get; set; }
        public double FestBns { get; set; }
        public double Arer { get; set; }
        public double OtherBns { get; set; }
        public double AdvDeduct { get; set; }
        public double OtherDeduct { get; set; }
        public int? OtMin { get; set; }
    }
}
