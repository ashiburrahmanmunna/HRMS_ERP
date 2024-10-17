using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMasterPoSalesOrder
    {
        public TblStrMasterPoSalesOrder()
        {
            TblStrMasterPoItem = new HashSet<TblStrMasterPoItem>();
        }

        public long MasterPoid { get; set; }
        public long Bmid { get; set; }
        public int OrdId { get; set; }
        public decimal Qty { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public DateTime? PrdStartDate { get; set; }
        public DateTime? PrdFinisDate { get; set; }
        public DateTime? Etdchina { get; set; }
        public DateTime? Etactg { get; set; }
        public int? Prevbmid { get; set; }

        public virtual TblStrMasterPoMain MasterPo { get; set; }
        public virtual ICollection<TblStrMasterPoItem> TblStrMasterPoItem { get; set; }
    }
}
