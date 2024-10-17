using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccIoMain
    {
        public int IoId { get; set; }
        public DateTime? TramDate { get; set; }
        public int? CountryId { get; set; }
        public short? CountryIdLocal { get; set; }
        public string Rmrks { get; set; }
        public int? ComId { get; set; }
        public int? Aid { get; set; }
        public Guid? Wid { get; set; }
    }
}
