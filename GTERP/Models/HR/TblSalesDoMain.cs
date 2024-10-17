using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesDoMain
    {
        public TblSalesDoMain()
        {
            TblSalesDoSub = new HashSet<TblSalesDoSub>();
        }

        public byte ComId { get; set; }
        public long Doid { get; set; }
        public string Dono { get; set; }
        public DateTime? DtDo { get; set; }
        public string OldDono { get; set; }
        public long BookId { get; set; }
        public int? TrackId { get; set; }
        public int CustId { get; set; }
        public short? SrefId { get; set; }
        public string Dotype { get; set; }
        public string Pdono { get; set; }
        public short DsiteId { get; set; }
        public string DsiteAdd { get; set; }
        public string DsiteAddBangla { get; set; }
        public double NetAmount { get; set; }
        public string InWords { get; set; }
        public byte IsAllowLoadCharge { get; set; }
        public double LoadChargeAmount { get; set; }
        public byte IsAllowCarryCharge { get; set; }
        public double CarryChargeAmount { get; set; }
        public string DiscountType { get; set; }
        public double? DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public string SalesNote { get; set; }
        public string Remarks { get; set; }
        public byte TotalDoing { get; set; }
        public int? DcustId { get; set; }
        public string Dosts { get; set; }
        public DateTime? DtCancel { get; set; }
        public string DeliverySts { get; set; }
        public string TransportType { get; set; }
        public DateTime? DtDelivery { get; set; }
        public string CollectDetails { get; set; }
        public byte IsPaid { get; set; }
        public byte IsPosted { get; set; }
        public int RcustId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public int? FbCustId { get; set; }
        public byte IsPrinted { get; set; }
        public short PrintedQty { get; set; }
        public double? Fbqty { get; set; }
        public int? SresfsubId { get; set; }
        public int? IsBill { get; set; }
        public string VatChallan { get; set; }
        public byte Smsflag { get; set; }
        public int? VCustId { get; set; }
        public short Status { get; set; }
        public int LuserIdCancel { get; set; }
        public DateTime? DtCancelDoc { get; set; }
        public string RemarksCancel { get; set; }
        public int? Retailerid { get; set; }
        public int? LocationId { get; set; }
        public DateTime? RddtDate { get; set; }
        public int? ScratchCardId { get; set; }
        public string OldRefNo { get; set; }

        public virtual TblSalesBookMain Book { get; set; }
        public virtual ICollection<TblSalesDoSub> TblSalesDoSub { get; set; }
    }
}
