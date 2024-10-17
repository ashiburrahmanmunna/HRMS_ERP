using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccCoaSystem
    {
        public string BizType { get; set; }
        public byte AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string AccType { get; set; }
        public int ParentId { get; set; }
        public string ParentCode { get; set; }
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
        public byte IsItemConsumed { get; set; }
    }
}
