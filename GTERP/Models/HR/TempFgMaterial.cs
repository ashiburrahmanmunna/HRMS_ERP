using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempFgMaterial
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string DateCaption { get; set; }
        public long AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public byte IsInnerData { get; set; }
        public byte IsBold { get; set; }
        public decimal InnerAmount { get; set; }
        public decimal Amount { get; set; }
        public float SortNo { get; set; }
        public byte Status { get; set; }
    }
}
