using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpPf
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public short DeptId { get; set; }
        public short SectId { get; set; }
        public short SubSectId { get; set; }
        public short DesigId { get; set; }
        public string Band { get; set; }
        public DateTime? DtJoin { get; set; }
        public DateTime? DtPf { get; set; }
        public DateTime? DtReleased { get; set; }
        public byte IsReleased { get; set; }
        public short Year { get; set; }
        public string Month { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
        public string FinancialYear { get; set; }
        public string Pfstatus { get; set; }
        public string RegPage { get; set; }
        public string EmpType { get; set; }
        public short SerYear { get; set; }
        public short SerMonth { get; set; }
        public short PrevTtlMonth { get; set; }
        public short FinTtlMonth { get; set; }
        public short TtlMonth { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public decimal Pf { get; set; }
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
        public decimal PayProfit { get; set; }
        public decimal PayTotal { get; set; }
        public string Remarks { get; set; }
        public byte Cf { get; set; }
        public int TtlAmt { get; set; }
        public short Tk1000 { get; set; }
        public short Tk500 { get; set; }
        public short Tk100 { get; set; }
        public short Tk50 { get; set; }
        public short Tk20 { get; set; }
        public short Tk10 { get; set; }
        public short Tk5 { get; set; }
        public bool FirstPf { get; set; }
        public bool YearEnd { get; set; }
        public float Rate { get; set; }
    }
}
