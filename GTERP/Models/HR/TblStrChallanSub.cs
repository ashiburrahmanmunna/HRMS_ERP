using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrChallanSub
    {
        public long ChallanId { get; set; }
        public long OrdId { get; set; }
        public long ArticleId { get; set; }
        public double? IssuedQty { get; set; }
        public string InvoiceNo { get; set; }
        public string Size { get; set; }
        public string RemarksChallan { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int? UnitId { get; set; }
    }
}
