using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderItem
    {
        public long OrdId { get; set; }
        public long PrdDisId { get; set; }
        public decimal Qty { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public string Remarks { get; set; }
        public byte? IsComplete { get; set; }
        public int? BomtypeId { get; set; }
    }
}
