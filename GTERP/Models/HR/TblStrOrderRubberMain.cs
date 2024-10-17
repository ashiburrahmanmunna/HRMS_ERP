using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderRubberMain
    {
        public TblStrOrderRubberMain()
        {
            TblStrOrderRubberConsumption = new HashSet<TblStrOrderRubberConsumption>();
            TblStrOrderRubberInstruction = new HashSet<TblStrOrderRubberInstruction>();
            TblStrOrderRubberSub = new HashSet<TblStrOrderRubberSub>();
            TblStrOrderRubberWeight = new HashSet<TblStrOrderRubberWeight>();
        }

        public byte ComId { get; set; }
        public long Sppid { get; set; }
        public short ArticleId { get; set; }
        public string Sppno { get; set; }
        public DateTime? DtSpp { get; set; }
        public DateTime? DtIssue { get; set; }
        public float QtyOrder { get; set; }
        public float QtyBatchKg { get; set; }
        public float QtyWeightKg { get; set; }
        public float QtyBatch { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public string Remarks { get; set; }
        public int LuserIdCheck { get; set; }
        public string RemarksCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public string RemarksVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public int LuserIdApprove { get; set; }
        public string RemarksApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public byte? Status { get; set; }

        public virtual ICollection<TblStrOrderRubberConsumption> TblStrOrderRubberConsumption { get; set; }
        public virtual ICollection<TblStrOrderRubberInstruction> TblStrOrderRubberInstruction { get; set; }
        public virtual ICollection<TblStrOrderRubberSub> TblStrOrderRubberSub { get; set; }
        public virtual ICollection<TblStrOrderRubberWeight> TblStrOrderRubberWeight { get; set; }
    }
}
