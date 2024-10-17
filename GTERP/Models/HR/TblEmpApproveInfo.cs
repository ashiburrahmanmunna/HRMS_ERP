using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpApproveInfo
    {
        public long AId { get; set; }
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public int FirstAppId { get; set; }
        public int FinalAppId { get; set; }
        public bool AppFirst { get; set; }
        public bool AppFinal { get; set; }
        public bool Approved { get; set; }
        public bool IsInactive { get; set; }
        public string RemarksFirst { get; set; }
        public string RemarksFinal { get; set; }
        public string PcnameFirst { get; set; }
        public int? LuserIdFirst { get; set; }
        public string PcnameFinal { get; set; }
        public int? LuserIdFinal { get; set; }
        public byte IsUpdate { get; set; }
        public bool MailSend { get; set; }
    }
}
