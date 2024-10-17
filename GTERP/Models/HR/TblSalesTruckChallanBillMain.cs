using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesTruckChallanBillMain
    {
        public TblSalesTruckChallanBillMain()
        {
            TblSalesTruckChallanBillSub = new HashSet<TblSalesTruckChallanBillSub>();
        }

        public byte ComId { get; set; }
        public long TruckChallanBillId { get; set; }
        public string TruckChallanBillNo { get; set; }
        public DateTime? DtTruckChallanBill { get; set; }
        public int CustId { get; set; }
        public double NetAmount { get; set; }
        public string InWords { get; set; }
        public string DiscountType { get; set; }
        public double? DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public string Remarks { get; set; }
        public DateTime? DtCancel { get; set; }
        public string DeliverySts { get; set; }
        public byte IsPosted { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public byte IsPrinted { get; set; }
        public short PrintedQty { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public string Subject { get; set; }

        public virtual ICollection<TblSalesTruckChallanBillSub> TblSalesTruckChallanBillSub { get; set; }
    }
}
