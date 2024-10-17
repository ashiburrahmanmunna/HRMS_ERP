using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempNotesCompare
    {
        public int? UserId { get; set; }
        public byte? ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string Period { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string AccType { get; set; }
        public int? ParentId { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public byte IsItemBs { get; set; }
        public byte IsItemPl { get; set; }
        public byte IsItemTa { get; set; }
        public byte IsItemCs { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
        public decimal TranDebit { get; set; }
        public decimal TranCredit { get; set; }
        public string Currency { get; set; }
        public string DateName1 { get; set; }
        public decimal? Tkdebit1 { get; set; }
        public decimal? Tkcredit1 { get; set; }
        public string DateName2 { get; set; }
        public decimal? Tkdebit2 { get; set; }
        public decimal? Tkcredit2 { get; set; }
        public decimal OpDebit2 { get; set; }
        public decimal OpCredit2 { get; set; }
    }
}
