using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblApplicantsRef
    {
        public long AppId { get; set; }
        public string RefName { get; set; }
        public string RefAdd { get; set; }
        public string RefPhone { get; set; }
        public string RefFax { get; set; }
        public string RefEmail { get; set; }
        public string RefOthers { get; set; }
        public string RefRemarks { get; set; }
        public string RefOrg { get; set; }
        public string RefDesig { get; set; }
        public int? AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
