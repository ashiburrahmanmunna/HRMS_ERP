using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccMaterialConsumed
    {
        public byte ComId { get; set; }
        public short Fyid { get; set; }
        public short? HyearId { get; set; }
        public short QtrId { get; set; }
        public short MonthId { get; set; }
        public long AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public byte IsInnerData { get; set; }
        public byte IsBold { get; set; }
        public decimal InnerAmount { get; set; }
        public decimal Amount { get; set; }
        public float SortNo { get; set; }
        public short Status { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }
    }
}
