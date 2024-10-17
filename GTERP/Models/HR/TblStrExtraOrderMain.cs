using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrExtraOrderMain
    {
        public TblStrExtraOrderMain()
        {
            TblStrExtraOrderSub = new HashSet<TblStrExtraOrderSub>();
        }

        public byte ComId { get; set; }
        public long OrdId { get; set; }
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

        public virtual ICollection<TblStrExtraOrderSub> TblStrExtraOrderSub { get; set; }
    }
}
