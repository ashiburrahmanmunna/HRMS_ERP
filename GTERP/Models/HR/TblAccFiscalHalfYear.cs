using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccFiscalHalfYear
    {
        public byte ComId { get; set; }
        public short Fyid { get; set; }
        public short HyearId { get; set; }
        public string HyearName { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
        public Guid WId { get; set; }
        public short AId { get; set; }

        public virtual TblAccFiscalYear Fy { get; set; }
    }
}
