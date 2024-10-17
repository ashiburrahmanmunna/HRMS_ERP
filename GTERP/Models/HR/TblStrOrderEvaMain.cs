using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderEvaMain
    {
        public TblStrOrderEvaMain()
        {
            TblStrOrderEvaConsumption = new HashSet<TblStrOrderEvaConsumption>();
            TblStrOrderEvaInstruction = new HashSet<TblStrOrderEvaInstruction>();
            TblStrOrderEvaSub = new HashSet<TblStrOrderEvaSub>();
        }

        public byte ComId { get; set; }
        public long Evaid { get; set; }
        public string Evano { get; set; }
        public DateTime? DtEva { get; set; }
        public DateTime? DtComplete { get; set; }
        public DateTime? DtIssue { get; set; }
        public int LuserId { get; set; }
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

        public virtual ICollection<TblStrOrderEvaConsumption> TblStrOrderEvaConsumption { get; set; }
        public virtual ICollection<TblStrOrderEvaInstruction> TblStrOrderEvaInstruction { get; set; }
        public virtual ICollection<TblStrOrderEvaSub> TblStrOrderEvaSub { get; set; }
    }
}
