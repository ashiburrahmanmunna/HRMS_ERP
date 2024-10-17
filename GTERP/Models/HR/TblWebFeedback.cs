using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebFeedback
    {
        public long Fbid { get; set; }
        public string CompName { get; set; }
        public string ContPerson { get; set; }
        public string ContDesig { get; set; }
        public string ContPhone { get; set; }
        public string ContEmail { get; set; }
        public string ProdUse { get; set; }
        public string Satisfy { get; set; }
        public string Quality { get; set; }
        public string Price { get; set; }
        public string Purchase { get; set; }
        public string Installation { get; set; }
        public string Usage { get; set; }
        public string Service { get; set; }
        public string PurAgain { get; set; }
        public string Recomend { get; set; }
        public string Comment { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
