using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderBom
    {
        public long OrdId { get; set; }
        public int BomId { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
    }
}
