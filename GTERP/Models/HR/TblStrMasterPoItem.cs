using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMasterPoItem
    {
        public long Bmid { get; set; }
        public long PrdDisId { get; set; }
        public int SupplierId { get; set; }
        public decimal Qty { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public string Remarks { get; set; }
        public int? Articleid { get; set; }

        public virtual TblStrMasterPoSalesOrder Bm { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
