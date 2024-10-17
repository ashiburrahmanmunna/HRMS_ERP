using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrFaSub
    {
        public long Fpid { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public double QtyAdj { get; set; }
        public string RemarksFa { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }

        public virtual TblStrFaMain Fp { get; set; }
    }
}
