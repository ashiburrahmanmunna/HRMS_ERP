using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMasterPoCreateSubInvoice
    {
        public long Poid { get; set; }
        public long PrdDisId { get; set; }
        public string InvNo { get; set; }
        public DateTime DtInvoice { get; set; }
        public decimal InvQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? MtrUnitPrice { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public int? ComId { get; set; }
        public string Remarks { get; set; }
        public decimal? Amount { get; set; }
        public int? SupplierId { get; set; }
        public int OrdId { get; set; }
        public DateTime? DtEta { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public short ShipInfoId { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserId { get; set; }
        public short Status { get; set; }
        public double Size { get; set; }

        public virtual TblStrMasterPoCreateMain Po { get; set; }
        public virtual TblStrProductDistribution PrdDis { get; set; }
    }
}
