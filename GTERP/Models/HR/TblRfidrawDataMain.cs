using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblRfidrawDataMain
    {
        public long Aid { get; set; }
        public int? DocId { get; set; }
        public string DocNo { get; set; }
        public int? ExpectedQty { get; set; }
        public int? ExpectedTagsRead { get; set; }
        public int? NotExpected { get; set; }
        public DateTime? DtDate { get; set; }
        public int? Comid { get; set; }
        public int? LuserId { get; set; }
        public Guid? Wid { get; set; }
        public string Pcname { get; set; }
        public string Ean { get; set; }
        public int? Status { get; set; }
        public int? OrdId { get; set; }
        public int? SizeId { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public string RemarksVerify { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public short Status1 { get; set; }
    }
}
