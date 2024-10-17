using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempTran
    {
        public byte? ComId { get; set; }
        public string ComName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public DateTime? DtTransfer { get; set; }
        public string TranStatus { get; set; }
    }
}
