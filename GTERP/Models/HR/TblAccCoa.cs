using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccCoa
    {
        public TblAccCoa()
        {
            TblAccTrialBalance = new HashSet<TblAccTrialBalance>();
        }

        public byte ComId { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string AccType { get; set; }
        public int ParentId { get; set; }
        public string ParentCode { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
        public byte IsItemBs { get; set; }
        public byte IsItemPl { get; set; }
        public byte IsItemTa { get; set; }
        public byte IsItemCs { get; set; }
        public byte IsShowCoa { get; set; }
        public byte IsShowUg { get; set; }
        public byte IsChkRef { get; set; }
        public byte IsEntryDep { get; set; }
        public byte IsEntryBankLiability { get; set; }
        public byte IsSysDefined { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public double Rate { get; set; }
        public double OpDebitLocal { get; set; }
        public double OpCreditLocal { get; set; }
        public string Remarks { get; set; }
        public byte IsInactive { get; set; }
        public byte IsItemConsumed { get; set; }
        public DateTime OpDate { get; set; }
        public short OpFyid { get; set; }
        public decimal Balance { get; set; }
        public decimal Advance { get; set; }
        public byte IsSubsidiaryLedger { get; set; }
        public long RelatedId { get; set; }
        public decimal OpDebitFc { get; set; }
        public decimal OpCreditFc { get; set; }

        public virtual ICollection<TblAccTrialBalance> TblAccTrialBalance { get; set; }
    }
}
