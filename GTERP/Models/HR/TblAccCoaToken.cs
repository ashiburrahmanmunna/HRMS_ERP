using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccCoaToken
    {
        public byte ComId { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public int? EmpId { get; set; }
        public string AccName { get; set; }
        public string AccType { get; set; }
        public int ParentId { get; set; }
        public string ParentCode { get; set; }
        public byte IsShowUg { get; set; }
        public string RefType { get; set; }
        public short? RefId { get; set; }
        public double OpDebit { get; set; }
        public double OpCredit { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public double Rate { get; set; }
        public double OpDebitLocal { get; set; }
        public double OpCreditLocal { get; set; }
        public byte IsSysDefined { get; set; }
        public short AreaId { get; set; }
    }
}
