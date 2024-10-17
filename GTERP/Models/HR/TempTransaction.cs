using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempTransaction
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public int LuserId { get; set; }
        public string Caption { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public decimal Tkdebit { get; set; }
        public decimal Tkcredit { get; set; }
        public decimal Tkbalance { get; set; }
        public int? ParentId { get; set; }
        public string ParentCode { get; set; }
        public string AccType { get; set; }
        public string Parentname { get; set; }
    }
}
