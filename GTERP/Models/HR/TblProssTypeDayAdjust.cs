using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProssTypeDayAdjust
    {
        public int AId { get; set; }
        public byte ComId { get; set; }
        public DateTime? DtInput { get; set; }
        public DateTime? ProssDt { get; set; }
        public DateTime? DtJoin { get; set; }
        public string Remarks { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
