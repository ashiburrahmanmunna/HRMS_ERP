using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempServiceIncLog
    {
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public decimal Gs { get; set; }
        public decimal? Bs { get; set; }
        public decimal? Hr { get; set; }
        public string Ma { get; set; }
        public string DesigName { get; set; }
        public string CardNo { get; set; }
        public string SectName { get; set; }
        public string DtInc { get; set; }
        public string Inctype { get; set; }
    }
}
