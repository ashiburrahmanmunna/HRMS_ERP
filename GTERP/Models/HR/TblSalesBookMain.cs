using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesBookMain
    {
        public TblSalesBookMain()
        {
            TblSalesBookSub = new HashSet<TblSalesBookSub>();
            TblSalesDoMain = new HashSet<TblSalesDoMain>();
        }

        public byte ComId { get; set; }
        public long BookId { get; set; }
        public string BookNo { get; set; }
        public DateTime? DtBook { get; set; }
        public int? CustType { get; set; }
        public int CustId { get; set; }
        public int? RcustId { get; set; }
        public int? DcustId { get; set; }
        public short SrefId { get; set; }
        public string BookType { get; set; }
        public string PbookNo { get; set; }
        public short DsiteId { get; set; }
        public string DsiteAdd { get; set; }
        public byte? IsAllowLoadCharge { get; set; }
        public double? LoadChargeAmount { get; set; }
        public byte? IsAllowCarryCharge { get; set; }
        public double? CarryChargeAmount { get; set; }
        public string DiscountType { get; set; }
        public double? DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
        public string InWords { get; set; }
        public string SalesNote { get; set; }
        public string Remarks { get; set; }
        public byte TotalBooking { get; set; }
        public string BookSts { get; set; }
        public DateTime? DtCancel { get; set; }
        public string DeliverySts { get; set; }
        public string TransportType { get; set; }
        public DateTime? DtDelivery { get; set; }
        public string CollectDetails { get; set; }
        public byte IsPaid { get; set; }
        public byte IsPosted { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public int? SalesSubid { get; set; }
        public byte? IsApproved { get; set; }
        public int? FbCustId { get; set; }
        public byte IsAuthorised { get; set; }
        public string Status { get; set; }
        public byte IsWeb { get; set; }
        public int? RetailerId { get; set; }
        public int? LocationId { get; set; }
        public DateTime RddtDate { get; set; }
        public int? ScratchCardId { get; set; }
        public string OrderedBy { get; set; }
        public string BookingBrand { get; set; }
        public string BuyerPono { get; set; }
        public string OrdRcvdBy { get; set; }
        public string DesgignApprBy { get; set; }
        public string Bdapprvdno { get; set; }
        public string UsFdaregNo { get; set; }
        public string LotNo { get; set; }
        public DateTime? DdateofPrd { get; set; }
        public DateTime? DdateBestBefore { get; set; }
        public string Wax { get; set; }
        public byte? IsSeenDateofProduction { get; set; }
        public byte? IsSeenBestBeforeDate { get; set; }
        public byte? IsSeenBestBeforeMonth { get; set; }
        public byte? IsSeenBestBeforeYear { get; set; }
        public int? LuserIdApprove { get; set; }
        public string OldRefNo { get; set; }

        public virtual ICollection<TblSalesBookSub> TblSalesBookSub { get; set; }
        public virtual ICollection<TblSalesDoMain> TblSalesDoMain { get; set; }
    }
}
