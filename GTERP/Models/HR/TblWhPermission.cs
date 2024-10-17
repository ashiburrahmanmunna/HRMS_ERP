using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWhPermission
    {
        public int UserId { get; set; }
        public int WhId { get; set; }
        public int IsPermit { get; set; }
    }
}
