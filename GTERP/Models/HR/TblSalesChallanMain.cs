using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesChallanMain
    {
        public TblSalesChallanMain()
        {
            TblSalesChallanSub = new HashSet<TblSalesChallanSub>();
        }

        public byte ComId { get; set; }
        public long ChallanId { get; set; }
        public string ChallanNo { get; set; }
        public DateTime? DtChallan { get; set; }
        public long Doid { get; set; }
        public string TruckNo { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string DriverName { get; set; }
        public string DriverContNo { get; set; }
        public string VatchallanNo { get; set; }
        public string Remarks { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
        public byte IsPosted { get; set; }
        public int? TruckId { get; set; }
        public int? VatChallanId { get; set; }
        public string LoadingSlipNo { get; set; }
        public int Custid { get; set; }
        public int? Retailerid { get; set; }
        public string SalesNote { get; set; }
        public int Salessubid { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int? LuserIdCancel { get; set; }
        public DateTime? DtCancel { get; set; }
        public string RemarksCancel { get; set; }
        public string GatePassNo { get; set; }
        public int? Trucktypeid { get; set; }
        public long? SerialNo { get; set; }
        public string RemarksOther { get; set; }
        public byte? Isbill { get; set; }
        public decimal? TruckFair { get; set; }

        public virtual ICollection<TblSalesChallanSub> TblSalesChallanSub { get; set; }
    }
}
