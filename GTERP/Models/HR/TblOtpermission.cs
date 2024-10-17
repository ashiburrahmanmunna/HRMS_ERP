using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblOtpermission
    {
        public int ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public float Othour { get; set; }
        public byte IsInactive { get; set; }
        public string Remarks { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public string Approval { get; set; }
        public float PrevOthour { get; set; }
        public int FirstAppId { get; set; }
        public int FinalAppId { get; set; }
        public bool AppFirst { get; set; }
        public bool AppFinal { get; set; }
        public bool Approved { get; set; }
        public byte IsUpdate { get; set; }
        public bool MailSend { get; set; }

        public virtual TblCatCompany Com { get; set; }
    }
}
