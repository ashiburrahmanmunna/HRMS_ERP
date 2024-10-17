using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempCogs
    {
        public int LuserId { get; set; }
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public decimal Amount { get; set; }
        public byte TranStatus { get; set; }
        public short SortNo { get; set; }
        public byte TranType { get; set; }
        public byte IsOnlySum { get; set; }
        public string Currency { get; set; }
        public string Caption { get; set; }
    }
}
