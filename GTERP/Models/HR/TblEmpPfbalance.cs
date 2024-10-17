using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpPfbalance
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtPf { get; set; }
        public string Pfstatus { get; set; }
        public string RegPage { get; set; }
        public decimal OpOwn { get; set; }
        public decimal OpCom { get; set; }
        public decimal OpProfit { get; set; }
        public decimal OpTotal { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Dyown { get; set; }
        public decimal Dycom { get; set; }
        public decimal Dytotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Profit { get; set; }
        public decimal Clown { get; set; }
        public decimal Clcom { get; set; }
        public decimal Clprofit { get; set; }
        public decimal Cltotal { get; set; }
        public decimal PayOwn { get; set; }
        public decimal PayCom { get; set; }
        public decimal PayTotal { get; set; }
    }
}
