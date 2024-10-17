using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrOrderMain
    {
        public byte ComId { get; set; }
        public int OrdId { get; set; }
        public int? OrdBasedId { get; set; }
        public string OrdNo { get; set; }
        public int ArticleId { get; set; }
        public DateTime? DtOrderValidate { get; set; }
        public DateTime? DtOrder { get; set; }
        public DateTime? DtShip { get; set; }
        public string Ppno { get; set; }
        public DateTime? DtPp { get; set; }
        public string PpnoSole { get; set; }
        public DateTime? DtPpsole { get; set; }
        public string StsRfid { get; set; }
        public string Remarks { get; set; }
        public int TtlOrdQty { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public byte IsGenOrdSheet { get; set; }
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
        public int? CountryId { get; set; }
        public byte? Wk { get; set; }
        public int? IsComplete { get; set; }
        public string Ordtype { get; set; }
        public int? OrdidExtra { get; set; }
        public byte? IsRevised { get; set; }
        public int? PrevOrdId { get; set; }
        public byte? RevisedNo { get; set; }
        public int? Aiid { get; set; }
        public byte? IsIndentCancel { get; set; }
        public byte? CountReset { get; set; }
        public int? LuserIdReset { get; set; }
        public DateTime? DtReset { get; set; }
        public byte IsManualProcess { get; set; }
        public decimal? ArticleFob { get; set; }
        public decimal? MakingCost { get; set; }
        public decimal? Hocost { get; set; }
        public int? LuseridCancel { get; set; }
    }
}
