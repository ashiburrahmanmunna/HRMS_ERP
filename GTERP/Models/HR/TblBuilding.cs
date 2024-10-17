using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBuilding
    {
        public int? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int? Projectid { get; set; }
        public Guid? Wid { get; set; }
        public int? Userid { get; set; }
        public int? Comid { get; set; }
    }
}
