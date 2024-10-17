using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempIncomeCompare
    {
        public byte? LuserId { get; set; }
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public decimal Amount { get; set; }
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
        public string DateName1 { get; set; }
        public string DateName2 { get; set; }
        public decimal Amount1 { get; set; }
    }
}
