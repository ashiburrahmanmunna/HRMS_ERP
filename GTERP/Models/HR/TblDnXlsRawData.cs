using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblDnXlsRawData
    {
        public byte ComId { get; set; }
        public string XlsFileName { get; set; }
        public DateTime? DtProcess { get; set; }
        public string EntryNo { get; set; }
        public short BuyerId { get; set; }
        public string Consignee { get; set; }
        public string BookingNo { get; set; }
        public short CarId { get; set; }
        public string Carrier { get; set; }
        public string Fvessel { get; set; }
        public DateTime? Etdcgp { get; set; }
        public string Etahub { get; set; }
        public string Mvessel { get; set; }
        public string Etdhub { get; set; }
        public string Etadestination { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public string Size { get; set; }
        public string SizeFinal { get; set; }
        public string Type { get; set; }
        public string Shipper { get; set; }
        public string Po { get; set; }
        public string ItemNo { get; set; }
        public float? TtlCartoon { get; set; }
        public float? TtlPcs { get; set; }
        public float? TtlCbm { get; set; }
        public float? TtlGrossWt { get; set; }
        public float TtlGrossWtmt { get; set; }
        public string Mode { get; set; }
        public float? TtlLp { get; set; }
        public string DtRcvdCgo { get; set; }
        public string DtDocRcvd { get; set; }
        public string DtStuffing { get; set; }
        public string Desitnation { get; set; }
        public string ShipBillNo { get; set; }
        public string Vat { get; set; }
        public string Hblno { get; set; }
        public string Oblno { get; set; }
        public string Remarks { get; set; }
        public string PayTerms { get; set; }
        public float? NoOfOriginal { get; set; }
    }
}
