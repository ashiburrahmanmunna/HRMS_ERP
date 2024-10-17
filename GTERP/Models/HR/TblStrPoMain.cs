using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPoMain
    {
        public TblStrPoMain()
        {
            TblStrPoSub = new HashSet<TblStrPoSub>();
            TblStrPoTerms = new HashSet<TblStrPoTerms>();
        }

        public byte ComId { get; set; }
        public long Poid { get; set; }
        public string Pono { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtEdt { get; set; }
        public string Remarks { get; set; }
        public long Bomid { get; set; }
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

        public virtual ICollection<TblStrPoSub> TblStrPoSub { get; set; }
        public virtual ICollection<TblStrPoTerms> TblStrPoTerms { get; set; }
    }
}
