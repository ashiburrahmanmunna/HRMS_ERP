using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblDnXlsMain
    {
        public byte ComId { get; set; }
        public string XlsFileName { get; set; }
        public DateTime DtProcess { get; set; }
        public string EntryNo { get; set; }
        public short? LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
    }
}
