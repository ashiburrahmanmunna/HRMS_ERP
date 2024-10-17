using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMasterPoCreateMain
    {
        public TblStrMasterPoCreateMain()
        {
            TblStrMasterPoCreateSub = new HashSet<TblStrMasterPoCreateSub>();
            TblStrMasterPoCreateSubInvoice = new HashSet<TblStrMasterPoCreateSubInvoice>();
        }

        public byte ComId { get; set; }
        public long PobasedId { get; set; }
        public long Poid { get; set; }
        public string Pono { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtEdt { get; set; }
        public string Remarks { get; set; }
        public long MasterPoid { get; set; }
        public int? SupplierId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public byte IsComplete { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public short ShipInfoId { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public string Attention { get; set; }
        public int? BasedIdOld { get; set; }
        public int CountryId { get; set; }
        public string PayModeId { get; set; }
        public int? TermsId { get; set; }
        public int? SubSectIdto { get; set; }
        public int? PobuyerId { get; set; }
        public string ComCaption { get; set; }
        public byte IsRevised { get; set; }
        public byte OldMasterPoid { get; set; }
        public byte RevisedNo { get; set; }
        public int? LuseridInvoice { get; set; }

        public virtual TblStrMasterPoMain MasterPo { get; set; }
        public virtual ICollection<TblStrMasterPoCreateSub> TblStrMasterPoCreateSub { get; set; }
        public virtual ICollection<TblStrMasterPoCreateSubInvoice> TblStrMasterPoCreateSubInvoice { get; set; }
    }
}
