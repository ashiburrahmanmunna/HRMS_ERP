using GTERP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    #region StoreRequisition

    public partial class StoreReQuisitionResult
    {
        public int StoreReqId { get; set; }

        public string SRNo { get; set; }
        [Display(Name = "Reference")]
        public string Reference { get; set; }


        [Display(Name = "Required Date")]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "Requisition Date")]
        public string RequisitionDate { get; set; }

        [Display(Name = "Board Meeting Date")]
        public DateTime BoardMeetingDate { get; set; }


        public string Purpose { get; set; }


        public string Department { get; set; }


        public string ApprovedBy { get; set; }


        public string RecommenedBy { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public string Remarks { get; set; }
        public virtual IssueMain Issue { get; set; }

    }
    #endregion

    #region Purchase Requisition
    public partial class PurchaseReQuisitionResult
    {
        public int PurReqId { get; set; }
        public string PRNo { get; set; }
        [Display(Name = "Product Unit")]
        public string PrdUnitName { get; set; }

        [Display(Name = "Requisition Ref")]
        public string ReqRef { get; set; }

        [Display(Name = "Requisition Date")]
        public string ReqDate { get; set; }

        [Display(Name = "Board Meeting Date")]
        public string BoardMeetingDate { get; set; }

        public string PurposeName { get; set; }
        public string DeptName { get; set; }
        public string ApprovedBy { get; set; }

        public string RecommenedBy { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Required Date")]
        public string RequiredDate { get; set; }

    }
    #endregion

    #region Issues
    public partial class IssueResult
    {
        public int IssueMainId { get; set; }
        public string IssueNo { get; set; }

        public string IssueDate { get; set; }

        public string IssueRef { get; set; }

        public string Department { get; set; }

        public string PrdUnitName { get; set; }
        public string SRNo { get; set; }

        public string SRDate { get; set; }


        public string TypeName { get; set; }
        public string CurCode { get; set; }
        public float ConvertionRate { get; set; }
        public float TotalIssueValue { get; set; }
        public float? Deduction { get; set; }
        public float? NetIssueValue { get; set; }
        public string SectName { get; set; }

        //public DateTime GateInHouseDate { get; set; }
        //public DateTime? ExpectedReciveDate { get; set; }
        public string TermsAndCondition { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string Store { get; set; }

        public string InNo { get; set; }
        public string InDate { get; set; }


        public List<ItemResult> ItemResultList { get; set; }

    }
    public class ItemResult
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string IssueQty { get; set; }

        public string IssueRate { get; set; }

        public string IssueValue { get; set; }

    }
    public class IssueDetailsModel
    {
        public int? IssueMainId { get; set; }
        public int? IssueSubId { get; set; }
        public int? ProductId { get; set; }
        public int? UnitId { get; set; }
        public string SLNo { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal? StoreReqQty { get; set; }
        public decimal? RequisitionQty { get; set; }
        public decimal? RemainingReqQty { get; set; }
        public decimal? StoreQty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? TotalValue { get; set; }
        public string Remarks { get; set; }
        public int? StoreReqId { get; set; }
        public int? StoreReqSubId { get; set; }
    }

    #endregion

    #region GoodsReceive

    public partial class GoodsReceiveResult
    {
        public int GRRMainId { get; set; }

        public string GRRNo { get; set; }

        public string GRRDate { get; set; }

        public string GRRRef { get; set; }

        public string Department { get; set; }

        public string PrdUnitName { get; set; }
        public string PRNo { get; set; }

        public string SupplierName { get; set; }

        public string TypeName { get; set; }
        public string CurCode { get; set; }
        public float ConvertionRate { get; set; }
        public float TotalGRRValue { get; set; }
        public float? Deduction { get; set; }
        public float? NetGRRValue { get; set; }
        public string SectName { get; set; }
        public string PONo { get; set; }
        public DateTime? GateInHouseDate { get; set; }
        public DateTime? ExpectedReciveDate { get; set; }
        public string TermsAndCondition { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }


    }

    public class GoodsReceiveDetailsModel
    {
        public int? PurOrderMainId { get; set; }
        public int? PurOrderSubId { get; set; }
        public int? GRRSubId { get; set; }
        public int? ProductId { get; set; }
        public int? UnitId { get; set; }
        public int? SLNo { get; set; }
        public string PONo { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        //public int? PurReqQty { get; set; }
        public int? RequisitionQty { get; set; }
        public float? RemainingReqQty { get; set; }
        public float? PurchaseQty { get; set; }
        public float? Rate { get; set; }
        public float? TotalValue { get; set; }
        public float? Quality { get; set; }
        public float? Received { get; set; }
        public float? Damage { get; set; }
        public string PORemarks { get; set; }
        public string Remarks { get; set; }
        public int? PurReqId { get; set; }
        public int? PurReqSubId { get; set; }
        public int? WarehouseId { get; set; }

    }


    #endregion

    #region Purchase Order
    public partial class PurchaseOrderResult
    {
        public int PurOrderMainId { get; set; }
        public string PONo { get; set; }
        public string Refference { get; set; }
        public string PODate { get; set; }
        public string Department { get; set; }
        public string SupplierName { get; set; }
        public string TypeName { get; set; }
        public string CurCode { get; set; }
        public float ConvertionRate { get; set; }
        public float TotalPOValue { get; set; }
        public float? Deduction { get; set; }
        public float? NetPOValue { get; set; }
        public string SectName { get; set; }
        public string Status { get; set; }
        public string LastDateOfDelivery { get; set; }
        public string ExpectedRecivedDate { get; set; }
    }

    public class PurchaseOrderDetailsModel
    {
        public int? PurOrderMainId { get; set; }
        public int? PurOrderSubId { get; set; }
        public int? ProductId { get; set; }
        public int? UnitId { get; set; }
        public string SLNo { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal? PurReqQty { get; set; }
        public decimal? RequisitionQty { get; set; }
        public decimal? RemainingReqQty { get; set; }
        public decimal? PurchaseQty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? TotalValue { get; set; }
        public string Remarks { get; set; }
        public int? PurReqId { get; set; }
        public int? PurReqSubId { get; set; }
    }
    #endregion

}
