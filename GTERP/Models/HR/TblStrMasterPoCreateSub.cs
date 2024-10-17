using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMasterPoCreateSub
    {
        public long Poid { get; set; }
        public long PrdDisId { get; set; }
        public decimal QtyOrder { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte IsComplete { get; set; }
        public byte Flag { get; set; }
        public int? SupplierId { get; set; }
        public decimal? MasterPoQty { get; set; }
        public decimal? Bdstock { get; set; }
        public decimal? ExpQty { get; set; }
        public double Size { get; set; }
        public byte? IsColour { get; set; }
        public decimal Changes { get; set; }
        public long BasedPoid { get; set; }

        public virtual TblStrMasterPoCreateMain Po { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
