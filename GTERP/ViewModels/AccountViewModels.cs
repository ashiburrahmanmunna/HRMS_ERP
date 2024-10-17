using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    #region Account Process
    public class fymonthclass
    {
        //public int FiscalMonthId { get; set; }
        public int MonthId { get; set; }
        public int isCheck { get; set; }

        public string MonthName { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
        //public int FYId { get; set; }

    }
    #endregion

    #region Chart of Account
    public class ChartOfAccountsModel
    {
        public int MasterLCDetailsID { get; set; }
        public int ExportInvoiceDetailsId { get; set; }
        public string StyleName { get; set; }
        public string ExportPONo { get; set; }
        public string ShipmentDate { get; set; }
        public string Destination { get; set; }
        public float OrderQty { get; set; }
        public string UnitMasterId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public string PODate { get; set; }
        public string ColorCode { get; set; }
        public string DestinationPO { get; set; }

    }

    public class COAtemp
    {
        public int AccId { get; set; }
        public string AccName { get; set; }

    }

    public class ChartOfAccountsabc
    {
        public int AccId { get; set; }
        public string AccName { get; set; }
        public string AccCode { get; set; }
        public string AccType { get; set; }
        public string ParentName { get; set; }
        public string Remarks { get; set; }
        public decimal OpDebit { get; set; }
        public decimal OpCredit { get; set; }
    }
    #endregion

    #region Budget
    public class BudgetMainsList
    {
        public int BudgetMainId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string TotalBudgetDebit { get; set; }
        public string TotalBudgetCredit { get; set; }
    }
    public class Acc_BudgetData
    {
        public int? BudgetMainId { get; set; }
        public int? BudgetDetailsId { get; set; }
        public int? FiscalYearId { get; set; }
        public int? FiscalMonthId { get; set; }

        public string Year { get; set; }
        public string Month { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string BudgetDebit { get; set; }
        public string BudgetCredit { get; set; }


    }
    #endregion

    #region Account Dashboard
    public class HMShipmentStatusModel
    {
        public string YearNo { get; set; }

        public string MonthName { get; set; }

        public decimal PCS { get; set; }

        public decimal DOZEN { get; set; }

        public decimal USD { get; set; }

        public decimal MILLIONS { get; set; }

    }

    public class Dashboard1
    {
        public string Caption { get; set; }
        public string Count { get; set; }
        public string ReportLinkCaption { get; set; }
    }


    public class Dashboard2
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public decimal? TotalDocument { get; set; }
    }

    public class Dashboard3
    {
        public string username { get; set; }
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public int TotalDocument { get; set; }
    }

    public class Dashboard4
    {
        public int DayId { get; set; }
        public string Caption { get; set; }
        public string DateNameString { get; set; }
        public int TotalDocument { get; set; }
    }

    public class UserCountDocumentLastTransaction
    {
        public string UserName { get; set; }
        public string YearInput { get; set; }
        public string TotalCount { get; set; }
        public string LastTransactionDate { get; set; }
    }

    public class UserLogingInfoes
    {
        public string UserName { get; set; }
        public string DeviceType { get; set; }
        public DateTime? LoginTime { get; set; }
        public string IPAddress { get; set; }
    }

    public class TopTransaction
    {
        public string UserName { get; set; }
        public string Caption { get; set; }
        public DateTime? WorkDate { get; set; }
    }

    public class LedgerBalance
    {
        public string AccName { get; set; }
        public string AccCode { get; set; }
        public decimal? Balance { get; set; }
        public string Caption { get; set; }
    }



    public class LastTransactions
    {
        public string Caption { get; set; }
        public int Total { get; set; }
        public decimal? Value { get; set; }

        public string EntryDate { get; set; }

    }
    public class GatePassChartSummary
    {

        public string SupplierName { get; set; }
        public decimal TotalValue { get; set; }

    }


    public class GatePassDetails
    {
        public string GatePassNo { get; set; }
        public string CompanyName { get; set; }
        public string SupplierName { get; set; }
        public string LcOpeningDate { get; set; }
        public string ExpiryDate { get; set; }
        public string UDNo { get; set; }
        public string UDDate { get; set; }
        public string PortOfLoadingName { get; set; }
        public string PINo { get; set; }
        public string FileNo { get; set; }
        public string ImportPONo { get; set; }
        public string HSCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemGroupName { get; set; }
        public decimal ImportRate { get; set; }
        public decimal TotalValue { get; set; }

    }

    public class GroupLC
    {
        public string CompanyName { get; set; }
        public string BuyerName { get; set; }
        public string GroupLCRefName { get; set; }
        public string TotalGroupLCValue { get; set; }
        public string LCRefNo { get; set; }
        public string ExportPONo { get; set; }
        public string HSCode { get; set; }
        public string ItemName { get; set; }
        public string Fabrication { get; set; }
        public string TotalValue { get; set; }
        public string QtyInPcs { get; set; }
        public string OrderQty { get; set; }
    }
    public class GroupLCchart
    {
        public string BuyerName { get; set; }
        public decimal TotalMLCValue { get; set; }
    }
    public class MarginAlert
    {
        public string GatePassNo { get; set; }
        public string CompanyName { get; set; }
        public string SupplierName { get; set; }
        public string BuyerName { get; set; }
        public string LcOpeningDate { get; set; }
        public string ExpiryDate { get; set; }
        public int? TotalPI { get; set; }

        public string PortOfLoadingName { get; set; }
        public decimal TotalValue { get; set; }
        public decimal GatePassValue { get; set; }
        public decimal TotalMLCValue { get; set; }
        public decimal SalesBookingAmount { get; set; }
        public decimal MarginUsed { get; set; }
    }
    public class SupplierBillMaturityOverdue
    {
        public string CompanyName { get; set; }
        public string SalesBooking { get; set; }
        public string B2BNo { get; set; }
        public string Supplier { get; set; }
        public string BillRef { get; set; }
        public string DeliveryOrderNo { get; set; }// as Total PI
        public string CommercialDeliveryOrderDate { get; set; }
        public string BillDate { get; set; }
        public string MaturityDate { get; set; }

    }

    public class BillCreatePending
    {
        public string CompanyName { get; set; }
        public string SupplierName { get; set; }
        public string GatePassNo { get; set; }
        public string CommercialDeliveryOrderNo { get; set; }
        public string CommercialDeliveryOrderDate { get; set; }
        public string ItemGroupName { get; set; }
        public string ItemDescription { get; set; }
        public string BLNo { get; set; }
        public string BLDate { get; set; }
        public string DocumentReceiptDate { get; set; }
        public string DocumentAssesmentDate { get; set; }
        public decimal TotalValue { get; set; }

    }

    public class ExportDeliveryOrderStatusModel
    {
        public string RowNo { get; set; }

        public string BuyerLCRef { get; set; }
        public string DeliveryOrderNo { get; set; }
        public decimal TotalDeliveryOrderQty { get; set; }
        public decimal DeliveryOrderValue { get; set; }
        public string CargoHandOverDate { get; set; }

        public string BLNO { get; set; }

        public string BankRef { get; set; }

        public decimal RealizationAmount { get; set; } ///TotalValue	ReceivingDate	PIStatus

        public decimal ShortPayment { get; set; }
        public string DestinationName { get; set; }

        public string ExpNo { get; set; }

        public string BuyerGroupName { get; set; }
        public string BuyerName { get; set; }

        public string FirstNotifyParty { get; set; }

        public decimal FBPAmount { get; set; }

        public string DeliveryOrderDate { get; set; }
        public string VesselSailingDate { get; set; }

        public string PaymentMaturityDate { get; set; }

        public string PaymentReceiveDate { get; set; }

        public string DeliveryOrderPaymentStatus { get; set; }

        public string StatusColor { get; set; }

    }
    #endregion

    #region Post Voucher
    public class VoucherView
    {
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherTypeName { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherDesc { get; set; }
        public string Status { get; set; }
        public bool isPosted { get; set; }
        public string Comid { get; set; }

        public double VAmount { get; set; }

        public string VoucherTypeNameShort { get; set; }
    }

    public class SubReport
    {
        public int Id { get; set; }
        public string strRptPathSub { get; set; } // Sub Report Path name
        public string strRFNSub { get; set; }   // Relational Field Name 
        public string strDSNSub { get; set; }   // DSN Name Sub Report
        public string strQuerySub { get; set; } // Query string Sub Report
    }
    #endregion

    #region Summary Report
    public partial class TrialBalanceModel
    {
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string Caption { get; set; }
        public string opBalance { get; set; }
        public string tranBalance { get; set; }
        public string clBalance { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string opDebit { get; set; }
        public string opCredit { get; set; }
        public string TranDebit { get; set; }
        public string TranCredit { get; set; }
        public string clDebit { get; set; }
        public string clCredit { get; set; }
        public string AccType { get; set; }

    }
    #endregion

    #region General Report
    public partial class LedgerDetailsModel
    {
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string ComLogo { get; set; }
        public string ComImgPath { get; set; }
        public string Caption { get; set; }
        public string Caption2 { get; set; }
        public string CaptionCashBook { get; set; }

        public string VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string dtVoucher { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherDesc { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string AccId1 { get; set; }
        public string AccCode1 { get; set; }
        public string AccName1 { get; set; }
        public string TKDebit { get; set; }
        public string TKCredit { get; set; }
        public string TKDebit1 { get; set; }
        public string TKCredit1 { get; set; }
        public string TKDebit2 { get; set; }
        public string TKCredit2 { get; set; }
        public string RowNo { get; set; }
        public string RowDr { get; set; }
        public string RowCr { get; set; }
        public string Amount { get; set; }
        public string intFlag { get; set; }
        public string IsBatch { get; set; }
        public string OpBalance { get; set; }
        public string ClBalance { get; set; }
        public string ttlDebit { get; set; }
        public string ttlCredit { get; set; }
        public string referance { get; set; }
        public string ReferanceTwo { get; set; }
        public string ReferanceThree { get; set; }
        public string Currency { get; set; }
        public string AccNameOrg { get; set; }
        public string ParentNameOne { get; set; }
        public string ParentNameTwo { get; set; }
        public string ParentNameThree { get; set; }
        public string ParentNameFour { get; set; }
        public string ParentNameFive { get; set; }
        public string ChkNo { get; set; }


    }
    #endregion

    #region Bank Clearing
    public class BankClearing
    {
        public int Checked { get; set; }
        public int VoucherSubCheckId { get; set; }
        public int VoucherSubCheckNoClearingId { get; set; }
        public string VoucherNo { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string ChkNo { get; set; }
        public string VoucherDate { get; set; }
        public string DtChk { get; set; }
        public string DtChkTo { get; set; }
        public string DtChkClear { get; set; }
        public bool IsClear { get; set; }
        public string Amount { get; set; }
        public int VoucherId { get; set; }
    }
    #endregion

    #region Note Description
    public class NotesView
    {
        public string FYName { get; set; }
        public int SLNo { get; set; }
        public string NoteNo { get; set; }
        public string NoteDetails { get; set; }
        public string NoteRemarks { get; set; }
        public int NoteDescriptionId { get; set; }
    }
    #endregion

    #region Acc_Budget
    public class ChartOfAccountsModel1
    {
        public int MasterLCDetailsID { get; set; }
        public int ExportInvoiceDetailsId { get; set; }
        public string StyleName { get; set; }
        public string ExportPONo { get; set; }
        public string ShipmentDate { get; set; }
        public string Destination { get; set; }
        public float OrderQty { get; set; }
        public string UnitMasterId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public string PODate { get; set; }
        public string ColorCode { get; set; }
        public string DestinationPO { get; set; }
    }
    #endregion

    #region Acc Voucher
    public class VoucherView1
    {
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherDesc { get; set; }
        public string Status { get; set; }
        public bool isPosted { get; set; }
        public double VAmount { get; set; }
        public string Currency { get; set; }
        public string VoucherTypeNameShort { get; set; }
    }

    public class VoucherChartOfAccount
    {
        public int AccId { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string ParentName { get; set; }
        public string Parentcode { get; set; }
        public decimal Balance { get; set; }
        public int IsChkBalance { get; set; }
        public int CountryId { get; set; }
        public int CountryIdLocal { get; set; }
        public decimal AmountLocalBuy { get; set; }
        public decimal AmountLocalSale { get; set; }
        public int isCredit { get; set; }
    }

    public class Acc_ChartOfAccount_view
    {
        public int AccId { get; set; }
        public int? ParentId { get; set; }
        public int IsBankItem { get; set; }
        public int IsCashItem { get; set; }
        public int IsChkRef { get; set; }
        public string AccountParent { get; set; }
        public int CurrencyId { get; set; }
    }
    #endregion
}
