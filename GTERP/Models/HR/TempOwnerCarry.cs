using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempOwnerCarry
    {
        public byte? LuserId { get; set; }
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string Headings { get; set; }
        public decimal AmountSc { get; set; }
        public decimal AmountSp { get; set; }
        public decimal AmountSm { get; set; }
        public decimal AmountRs { get; set; }
        public decimal AmountRe { get; set; }
        public decimal AmountTotal { get; set; }
        public byte DispType { get; set; }
        public byte TranStatus { get; set; }
        public byte TranType { get; set; }
        public byte SortNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public byte IsBold { get; set; }
        public byte IsTopLine { get; set; }
        public byte IsBottomLine { get; set; }
        public byte IsLeftLine { get; set; }
        public byte IsRightLine { get; set; }
        public string Currency { get; set; }
        public string Caption { get; set; }
    }
}
