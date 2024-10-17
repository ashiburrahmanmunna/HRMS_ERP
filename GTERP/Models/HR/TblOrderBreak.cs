using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblOrderBreak
    {
        public long OrderId { get; set; }
        public string StyleNo { get; set; }
        public short ColorId { get; set; }
        public short? SizeId { get; set; }
        public decimal? Rate { get; set; }
        public int QtyOrder { get; set; }
        public int? QtyShip { get; set; }
        public byte RowNo { get; set; }
        public string RowNo1 { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }

        public virtual TblOrderMain Order { get; set; }
    }
}
