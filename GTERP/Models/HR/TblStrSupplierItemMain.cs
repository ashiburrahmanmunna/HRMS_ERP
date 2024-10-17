using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSupplierItemMain
    {
        public byte ComId { get; set; }
        public long Siid { get; set; }
        public int SupplierId { get; set; }
        public DateTime DtConfig { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
    }
}
