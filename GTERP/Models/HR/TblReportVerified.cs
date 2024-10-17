using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblReportVerified
    {
        public int? ReportId { get; set; }
        public int? ComId { get; set; }
        public int? VerfiedbyLuserId { get; set; }
        public string ReportName { get; set; }
        public DateTime? DtVerified { get; set; }
        public byte IsNotShow { get; set; }
        public string ReportCaption { get; set; }
        public byte ReportVersion { get; set; }
        public Guid? Wid { get; set; }
        public DateTime? DtReportIssue { get; set; }
    }
}
