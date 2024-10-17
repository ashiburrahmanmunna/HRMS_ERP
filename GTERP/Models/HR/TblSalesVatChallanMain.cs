using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesVatChallanMain
    {
        public TblSalesVatChallanMain()
        {
            TblSalesVatChallanSub = new HashSet<TblSalesVatChallanSub>();
        }

        public byte ComId { get; set; }
        public long VatChallanId { get; set; }
        public string VatChallanNo { get; set; }
        public DateTime? DtChallan { get; set; }
        public long Doid { get; set; }
        public string TruckNo { get; set; }
        public int? Custid { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
        public byte IsPosted { get; set; }
        public int? TruckId { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int? LuserIdCancel { get; set; }
        public DateTime? DtCancel { get; set; }
        public string RemarksCancel { get; set; }
        public long? Serialno { get; set; }

        public virtual ICollection<TblSalesVatChallanSub> TblSalesVatChallanSub { get; set; }
    }
}
