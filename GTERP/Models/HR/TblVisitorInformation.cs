using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblVisitorInformation
    {
        public int? ComId { get; set; }
        public string Vname { get; set; }
        public string Designation { get; set; }
        public string Purpose { get; set; }
        public DateTime? Date { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public DateTime? Intime { get; set; }
        public DateTime? Outtime { get; set; }
        public string VpicLocation { get; set; }
    }
}
