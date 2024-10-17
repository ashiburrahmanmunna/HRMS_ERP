using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPpMain
    {
        public TblStrPpMain()
        {
            TblStrPpSub = new HashSet<TblStrPpSub>();
        }

        public byte ComId { get; set; }
        public long Ppid { get; set; }
        public DateTime DtInput { get; set; }
        public int OrdId { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public short Status { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public string RemarksVerify { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public byte? RevisedNo { get; set; }
        public long? PrevPpid { get; set; }
        public byte IsRevised { get; set; }
        public int? OldPpid { get; set; }
        public string DocNo { get; set; }

        public virtual ICollection<TblStrPpSub> TblStrPpSub { get; set; }
    }
}
