using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblstrArticaleItemSizeMain
    {
        public byte ComId { get; set; }
        public long Aiid { get; set; }
        public long ArticleId { get; set; }
        public DateTime DtConfig { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
    }
}
