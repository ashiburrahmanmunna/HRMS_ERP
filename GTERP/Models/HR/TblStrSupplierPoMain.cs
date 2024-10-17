using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSupplierPoMain
    {
        public TblStrSupplierPoMain()
        {
            TblStrSupplierPoSub = new HashSet<TblStrSupplierPoSub>();
            TblStrSupplierPoTerms = new HashSet<TblStrSupplierPoTerms>();
        }

        public byte ComId { get; set; }
        public long SupPoid { get; set; }
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
        public string BasedOn { get; set; }
        public int? BasedId { get; set; }
        public string Comcaption { get; set; }
        public int? CountryId { get; set; }

        public virtual ICollection<TblStrSupplierPoSub> TblStrSupplierPoSub { get; set; }
        public virtual ICollection<TblStrSupplierPoTerms> TblStrSupplierPoTerms { get; set; }
    }
}
