using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAttFixedBuyer
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime Othour { get; set; }
        public string Status { get; set; }
        public byte IsInactive { get; set; }
        public string Remarks { get; set; }
        public long AId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public short? ShiftId { get; set; }
        public float? Ot { get; set; }
        public int? FirstAppId { get; set; }
        public int? FinalAppId { get; set; }
        public bool? AppFirst { get; set; }
        public bool AppFinal { get; set; }
        public bool Approved { get; set; }
        public byte IsUpdate { get; set; }
        public bool MailSend { get; set; }
    }
}
