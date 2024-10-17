using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrReplaceSub1
    {
        public long RepId { get; set; }
        public long Poid { get; set; }
        public short BuyerId { get; set; }
        public string StyleNo { get; set; }
        public double Qty { get; set; }
        public string Sd { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }

        public virtual TblCatBuyer Buyer { get; set; }
        public virtual TblStrReplaceMain Rep { get; set; }
    }
}
