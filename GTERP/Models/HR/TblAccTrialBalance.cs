using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccTrialBalance
    {
        public byte ComId { get; set; }
        public short Fyid { get; set; }
        public int AccId { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
        public decimal TranDebit { get; set; }
        public decimal TranCredit { get; set; }
        public decimal ClDebit { get; set; }
        public decimal ClCredit { get; set; }
        public short HyearId { get; set; }
        public short QtrId { get; set; }
        public short MonthId { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }
        public byte IsInventory { get; set; }

        public virtual TblAccCoa TblAccCoa { get; set; }
    }
}
