using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAppEdu
    {
        public long AppId { get; set; }
        public string ExamName { get; set; }
        public string ExamResult { get; set; }
        public string MajorSub { get; set; }
        public string InstituteName { get; set; }
        public string BoardName { get; set; }
        public string PassingYear { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
