using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPuMain
    {
        public TblStrPuMain()
        {
            TblStrOrderPuInstruction = new HashSet<TblStrOrderPuInstruction>();
            TblStrPuArticle = new HashSet<TblStrPuArticle>();
            TblStrPuArticleSize = new HashSet<TblStrPuArticleSize>();
        }

        public byte ComId { get; set; }
        public long BomId { get; set; }
        public string BomNo { get; set; }
        public DateTime DtBom { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public string Remarks { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public string RemarksVerify { get; set; }
        public int LuserIdApprove { get; set; }
        public string RemarksApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public byte? Status { get; set; }

        public virtual ICollection<TblStrOrderPuInstruction> TblStrOrderPuInstruction { get; set; }
        public virtual ICollection<TblStrPuArticle> TblStrPuArticle { get; set; }
        public virtual ICollection<TblStrPuArticleSize> TblStrPuArticleSize { get; set; }
    }
}
