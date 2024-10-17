using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAppTraining
    {
        public long AppId { get; set; }
        public string CourseName { get; set; }
        public string Result { get; set; }
        public string MajorSub { get; set; }
        public string InstituteName { get; set; }
        public DateTime? DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public string Remarks { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
