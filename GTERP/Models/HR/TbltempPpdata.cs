using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TbltempPpdata
    {
        public long OrdId { get; set; }
        public int PrdDisId { get; set; }
        public decimal Qty { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public string Remarks { get; set; }
        public byte? IsComplete { get; set; }
        public string DataType { get; set; }
    }
}
