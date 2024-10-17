using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMasterPoMain
    {
        public TblStrMasterPoMain()
        {
            TblStrMasterPoCreateMain = new HashSet<TblStrMasterPoCreateMain>();
            TblStrMasterPoSalesOrder = new HashSet<TblStrMasterPoSalesOrder>();
        }

        public byte ComId { get; set; }
        public long MasterPoid { get; set; }
        public string MasterPono { get; set; }
        public DateTime DtMasterPo { get; set; }
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
        public int? BasedMasterPoid { get; set; }
        public byte? PotypeId { get; set; }
        public int? OldMasterPoid { get; set; }
        public byte? RevisedNo { get; set; }
        public long? PrevMasterPoid { get; set; }
        public byte? IsRevised { get; set; }

        public virtual ICollection<TblStrMasterPoCreateMain> TblStrMasterPoCreateMain { get; set; }
        public virtual ICollection<TblStrMasterPoSalesOrder> TblStrMasterPoSalesOrder { get; set; }
    }
}
