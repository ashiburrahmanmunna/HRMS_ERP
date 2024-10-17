using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class VerSoft
    {
        public string Ver { get; set; }
        public string Dis { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? DateEx { get; set; }
        public string ActiveYesNo { get; set; }
        public byte? IsActive { get; set; }
    }
}
