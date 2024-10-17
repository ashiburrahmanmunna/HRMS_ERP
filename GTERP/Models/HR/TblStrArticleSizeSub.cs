using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrArticleSizeSub
    {
        public long Asid { get; set; }
        public short SizeId { get; set; }
        public double Pcb { get; set; }
        public double Ue { get; set; }
        public decimal WeightInGm { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public double? SoleWeightGm { get; set; }
        public int? PrdDisId { get; set; }
        public string ItemCode { get; set; }
        public string Rfid { get; set; }
        public string Ean { get; set; }

        public virtual TblStrArticleSizeMain As { get; set; }
        public virtual TblCatSize Size { get; set; }
    }
}
