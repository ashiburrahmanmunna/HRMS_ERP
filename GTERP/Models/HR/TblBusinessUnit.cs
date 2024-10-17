using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBusinessUnit
    {
        public int? BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public Guid? Wid { get; set; }
        public int? Comid { get; set; }
        public int? Luserid { get; set; }
    }
}
