using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrMosmanualSub
    {
        public int MosmanualId { get; set; }
        public int? Slno { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? DtDate { get; set; }
        public int? PrdId { get; set; }
        public int? SupplierId { get; set; }
        public int? Prddisid { get; set; }
        public int? CountryId { get; set; }
        public double? InvAmount { get; set; }
        public int BuyerId { get; set; }
        public int? PrimaryUnitId { get; set; }
        public double? PrimaryQty { get; set; }
        public int? SecoundaryUnitId { get; set; }
        public double? SecoundaryQty { get; set; }
        public string ShipMode { get; set; }
        public string CourierNo { get; set; }
        public DateTime? DtOnBoard { get; set; }
        public DateTime? DtEta { get; set; }
        public DateTime? DtTtdate { get; set; }
        public string ContractPo { get; set; }
        public string OrginalDocCourierNo { get; set; }
        public string SupplierIdCourier { get; set; }
        public string DocCourierStatus { get; set; }
        public string Remarks { get; set; }
        public DateTime? DtDelivery { get; set; }
        public string ItemDetails { get; set; }
        public string WarehouseNo { get; set; }
        public byte RowNo { get; set; }
        public string GoodsStatus { get; set; }

        public virtual TblStrMosmanualMain Mosmanual { get; set; }
    }
}
