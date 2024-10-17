using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrArticleItemMain
    {
        public TblStrArticleItemMain()
        {
            TblStrArticleItemSub = new HashSet<TblStrArticleItemSub>();
        }

        public byte ComId { get; set; }
        public long Aiid { get; set; }
        public long ArticleId { get; set; }
        public DateTime DtConfig { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public int RevisedNo { get; set; }
        public byte IsRevised { get; set; }
        public decimal? ArticleRate { get; set; }
        public decimal? ArticleFob { get; set; }
        public string RevVersion { get; set; }
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
        public short? Status { get; set; }
        public int LuserIdCheckSales { get; set; }
        public string RemarksCheckSales { get; set; }
        public DateTime? DtCheckSales { get; set; }
        public int LuserIdVerifySales { get; set; }
        public string RemarksVerifySales { get; set; }
        public DateTime? DtVerifySales { get; set; }
        public int LuserIdApproveSales { get; set; }
        public string RemarksApproveSales { get; set; }
        public DateTime? DtApproveSales { get; set; }
        public short StatusSales { get; set; }
        public short? LuserIdSales { get; set; }
        public string PcNameSales { get; set; }
        public DateTime? DtConfigSales { get; set; }
        public decimal? MakingCost { get; set; }
        public decimal? Hocost { get; set; }
        public byte? IsSendForApproval { get; set; }
        public byte? IsSendForApprovalBomsales { get; set; }
        public byte? IschkImageShowReport { get; set; }
        public string PatternPic1 { get; set; }
        public string PatternPic2 { get; set; }
        public string PatternPic3 { get; set; }
        public int? BasedAiid { get; set; }
        public int? OldAiid { get; set; }

        public virtual ICollection<TblStrArticleItemSub> TblStrArticleItemSub { get; set; }
    }
}
