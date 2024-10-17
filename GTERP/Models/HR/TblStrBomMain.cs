using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrBomMain
    {
        public TblStrBomMain()
        {
            TblStrBomArticle = new HashSet<TblStrBomArticle>();
            TblStrBomArticleSize = new HashSet<TblStrBomArticleSize>();
        }

        public byte ComId { get; set; }
        public long BomId { get; set; }
        public string BomNo { get; set; }
        public DateTime DtBom { get; set; }
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

        public virtual ICollection<TblStrBomArticle> TblStrBomArticle { get; set; }
        public virtual ICollection<TblStrBomArticleSize> TblStrBomArticleSize { get; set; }
    }
}
