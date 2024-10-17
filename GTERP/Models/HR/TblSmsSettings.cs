using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSmsSettings
    {
        public int? Aid { get; set; }
        public string SmsAddress { get; set; }
        public string SmsUser { get; set; }
        public string SmsPassword { get; set; }
        public string SmsSender { get; set; }
        public string SmsCollection { get; set; }
        public string SmsAbsent { get; set; }
        public string SmsPresent { get; set; }
        public string SmsLate { get; set; }
        public string IsInactive { get; set; }
        public Guid? Wid { get; set; }
        public string Company { get; set; }
    }
}
