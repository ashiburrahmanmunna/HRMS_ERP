using GTERP.Models.Base;
using GTERP.Models.Self;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class Acc_AccProcessViewModel
    {
        [Display(Name = "Currency ")]
        public int? CountryId { get; set; }
        public List<Acc_FiscalYear> ProcessFYs { get; set; }
        public List<Acc_FiscalMonth> ProcessMonths { get; set; }

        //public class ProcessFY
        //{
        //    public Boolean isCheck { get; set; }
        //    public int FYId { get; set; }
        //    public string FYName { get; set; }
        //    public string dtOpen { get; set; }
        //    public string dtClose { get; set; }
        //    public Boolean isWorking { get; set; }
        //    public Boolean isRunning { get; set; }
        //}
        //public class ProcessMonth
        //{
        //    public Boolean isCheck { get; set; }
        //    public int MonthId { get; set; }
        //    public string MonthName { get; set; }
        //    public string dtFrom { get; set; }
        //    public string dtTo { get; set; }
        //}

    }


    public class Acc_ChartOfAccount_Initial
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InitialAccId { get; set; }



        [Display(Name = "Company Id")]
        //[Index("IX_ComAccnameUniqe", 1, IsUnique = true)]
        [StringLength(128)]
        public string comid { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Account Head")]
        //[Index("IX_ComAccnameUniqe", 2, IsUnique = true)]
        public string AccName { get; set; }


        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Account Code")]
        public string AccCode { get; set; }




        [StringLength(1, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Account Type")]
        public string AccType { get; set; }


        [NotMapped]
        [Display(Name = "Account Type")]
        public string PrevAccType { get; set; }



        [ForeignKey("ParentChartOfAccountInitial")]
        [Display(Name = "Base / Parent Head")]
        //public int? ParentAccId { get; set; }
        public int? ParentID { get; set; }
        [NotMapped]
        public int? PrevParentID { get; set; }


        public virtual Acc_ChartOfAccount_Initial ParentChartOfAccountInitial { get; set; }



        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Parent Code")]
        public string ParentCode { get; set; }




        //[Display(Name = "Group")]
        //public virtual Acc_ChartOfAccount vAccountGroup { get; set; } = null;


        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }



        public int IsItemBS { get; set; } = 0;
        public int IsItemPL { get; set; } = 0;
        public int IsItemTA { get; set; } = 0;
        public int IsItemCS { get; set; } = 0;
        public int IsShowCOA { get; set; } = 0;
        public int IsChkRef { get; set; } = 0;
        public int IsEntryDep { get; set; } = 0;
        public int IsEntryBankLiability { get; set; } = 0;
        public int IsSysDefined { get; set; } = 0;
        [Display(Name = "Currency")]

        public int CountryID { get; set; }
        public int CountryIdLocal { get; set; } = 0;
        [Display(Name = "Currency Rate")]

        public int Rate { get; set; } = 0;
        [Display(Name = "Opening Debit [Local]")]

        public float OpDebitLocal { get; set; } = 0;
        [Display(Name = "Opening Credit [Local]")]

        public float OpCreditLocal { get; set; } = 0;
        public int isInactive { get; set; } = 0;
        public int isItemConsumed { get; set; } = 0;
        public int isItemInventory { get; set; } = 0;

        public int isShowUg { get; set; } = 0;





        public int? Level { get; set; } = 0;


        public int? AccSubId { get; set; } = 0;



        public int IsCashItem { get; set; } = 0;

        public int IsBankItem { get; set; } = 0;

        //public string ComId { get; set; }
        //public virtual ICollection<SalesSub> vAccountSalesSubs { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Acc_VoucherSub> VoucherSubs { get; set; }


    }


    public class Acc_ChartOfAccount:BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccId { get; set; }
        //[Display(Name = "Company Id")]
        //[Index("IX_ComAccnameUniqe", 1, IsUnique = true)]
        //[StringLength(128)]
        //public string comid { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Account Head")]
        //[Index("IX_ComAccnameUniqe", 2, IsUnique = true)]
        public string AccName { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Account Code")]
        public string AccCode { get; set; }

        public string AccCode_Old { get; set; }


        [StringLength(1, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Account Type")]
        public string AccType { get; set; }

        [NotMapped]
        [Display(Name = "Account Type")]
        public string PrevAccType { get; set; }

        // [Display(Name = "Base / Parent Head")]
        //public int? ParentAccId { get; set; }
        // public int? ParentIDTest { get; set; }

        public virtual Acc_ChartOfAccount ParentChartOfAccount { get; set; }

        [ForeignKey("ParentChartOfAccount")]
        [Display(Name = "Base / Parent Head")]
        //public int? ParentAccId { get; set; }
        public int? ParentID { get; set; }


        public virtual Acc_ChartOfAccount AccumulatedDepChartOfAccount { get; set; }


        [ForeignKey("AccumulatedDepChartOfAccount")]
        [Display(Name = "Accumulated Depreciation Head - Only for Asset Item.")]
        //public int? ParentAccId { get; set; }
        public int? AccumulatedDepId { get; set; }

        //[ForeignKey("DepExpenseChartOfAccount")]
        [Display(Name = "Depreciation Head - Only for Asset Item.")]
        //public int? ParentAccId { get; set; }
        public int? DepExpenseId { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "Depreciation Rate :")]
        public string DepreciationRate { get; set; }


        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        //[ForeignKey("DepExpenseId")]
        //public virtual Acc_ChartOfAccount DepExpenseChartOfAccount { get; set; }


        [NotMapped]
        public int? PrevParentID { get; set; }


        //public int? ParentBucketGroupId { get; set; }

        ///public virtual ICollection<Acc_ChartOfAccount> ChartOfAccountChildren { get; set; }


        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Parent Code")]
        public string ParentCode { get; set; }

        [Display(Name = "Opening Debit")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpDebit { get; set; } = 0;

        [Display(Name = "Opening Credit")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpCredit { get; set; } = 0;


        //[Display(Name = "Group")]
        //public virtual Acc_ChartOfAccount vAccountGroup { get; set; } = null;


        //[StringLength(128)]
        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }



        public int IsItemBS { get; set; } = 0;
        public int IsItemPL { get; set; } = 0;
        public int IsItemTA { get; set; } = 0;
        public int IsItemCS { get; set; } = 0;
        public int IsShowCOA { get; set; } = 0;
        [Display(Name = "Check / Bank Details Info")]

        public bool IsChkRef { get; set; }

        [Display(Name = "Is Item Depreciation Exp.")]
        public bool IsItemDepExp { get; set; }

        [Display(Name = "Is Item Accumulated Depreciation")]
        public bool IsItemAccmulateddDep { get; set; }

        public int IsEntryBankLiability { get; set; } = 0;
        public int IsSysDefined { get; set; } = 0;
        [Display(Name = "Currency")]

        public int CountryID { get; set; }
        public int CountryIdLocal { get; set; } = 0;


        [Display(Name = "Currency Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; } = 0;



        [Display(Name = "Opening Debit [Local]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpDebitLocal { get; set; } = 0;


        [Display(Name = "Opening Credit [Local]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpCreditLocal { get; set; } = 0;



        [Display(Name = "Is InActive")]

        public bool isInactive { get; set; }

        [Display(Name = "Is Item Consumed")]

        public bool isItemConsumed { get; set; }// = false;
        [Display(Name = "Is Item Inventory")]

        public bool isItemInventory { get; set; }// = false;
        [Display(Name = "Is Item Under Group")]


        public bool isShowUg { get; set; }



        public DateTime OpDate { get; set; }
        [Display(Name = "Fiscal Year")]

        public int opFYId { get; set; }

        //        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Balance { get; set; } = 0;

        public int RelatedId { get; set; } = 0;

        public int? Level { get; set; } = 0;

        public int? AccSubId { get; set; } = 0;
        [Display(Name = "Is Cash Item")]


        public bool IsCashItem { get; set; }
        [Display(Name = "Is Bank Item")]


        public bool IsBankItem { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<SalesSub> vAccountSalesSubs { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Acc_VoucherSub> VoucherSubs { get; set; }


    }

    public partial class Acc_FiscalYear : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FiscalYearId { get; set; }

        public int FYId { get; set; }

        //        [Required]
        [Display(Name = "Fiscal Year Period")]
        public string FYName { get; set; }

        [Display(Name = "Fiscal Year Period Bangla")]
        public string FYNameBangla { get; set; }


        [Display(Name = "Opening Date")]
        public string OpDate { get; set; }

        [Display(Name = "Closing Date")]
        public string ClDate { get; set; }


        [Display(Name = "Opening Date")]
        public DateTime OpeningDate { get; set; }

        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }



        public bool isWorking { get; set; }
        public bool isRunning { get; set; }
        [NotMapped]
        public bool isCheck { get; set; }

        public int? RowNo { get; set; }
        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }


        [Display(Name = "Locked Yes / No")]

        public bool isLocked { get; set; }

        //[Display(Name = "State")]
        //public virtual State vStateFiscalYear { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<Acc_FiscalYear> vProducts { get; set; }
    }


    public class Acc_FiscalMonth
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FiscalMonthId { get; set; }
        public int MonthId { get; set; }
        [NotMapped]
        public bool isCheck { get; set; }


        //        [Required]
        [Display(Name = "Month")]
        public string MonthName { get; set; }


        [Display(Name = "Month Bangla")]
        public string MonthNameBangla { get; set; }

        [Display(Name = "Opening Date")]
        public string dtFrom { get; set; }

        [Display(Name = "Closing Date")]
        public string dtTo { get; set; }


        [Display(Name = "Opening Date")]
        public DateTime OpeningdtFrom { get; set; }

        [Display(Name = "Closing Date")]
        public DateTime ClosingdtTo { get; set; }


        public int aid { get; set; }


        //[Display(Name = "State")]
        //public virtual State vStateFiscalMonth { get; set; }
        public int FYId { get; set; }
        //[ForeignKey("FiscalYearId")]
        //public virtual Acc_FiscalYear YearNames { get; set; }

        public int HYearId { get; set; }
        public int QtrId { get; set; }


        [StringLength(128)]
        public string ComId { get; set; }

        [Display(Name = "Locked Yes / No")]

        public bool isLocked { get; set; }


        [Display(Name = "Store Locked Yes / No")]

        public bool isLockedStore { get; set; }
        [Display(Name = "Accounts Locked Yes / No")]


        public bool isLockedAccounts { get; set; }
        [Display(Name = "Attendance Locked Yes / No")]


        public bool isLockedAttendance { get; set; }
        [Display(Name = "Salary Locked Yes / No")]


        public bool isLockedSalary { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<Acc_FiscalMonth> vProducts { get; set; }
    }


    public class Acc_FiscalHalfYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FiscalHalfYearId { get; set; }

        public int HYearId { get; set; }


        //        [Required]
        [Display(Name = "Fiscal Half Year Period")]
        public string HyearName { get; set; }

        [Display(Name = "Fiscal Half Year Period Bangla")]
        public string HyearNameBangla { get; set; }


        [Display(Name = "Opening Date")]
        public string dtFrom { get; set; }

        [Display(Name = "Closing Date")]
        public string dtTo { get; set; }

        public int aid { get; set; }


        //[Display(Name = "State")]
        //public virtual State vStateFiscalHalfYear { get; set; }
        public int FYId { get; set; }
        [StringLength(128)]
        public string ComId { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<FiscalHalfYear> vProducts { get; set; }
    }

    public class Acc_FiscalQtr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FiscalQtrId { get; set; }
        public int QtrId { get; set; }


        //        [Required]
        [Display(Name = "Quarter Period")]
        public string QtrName { get; set; }


        [Display(Name = "Quarter Period Bangla")]
        public string QtrNameBangla { get; set; }


        [Display(Name = "Opening Date")]
        public string dtFrom { get; set; }

        [Display(Name = "Closing Date")]
        public string dtTo { get; set; }

        public int aid { get; set; }


        //[Display(Name = "State")]
        //public virtual State vStateFiscalQtr { get; set; }
        public int FYId { get; set; }
        public int HYearId { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<FiscalQtr> vProducts { get; set; }
    }

    public partial class PrdUnit: BaseModel
    {
        [Key]
        public int PrdUnitId { get; set; }


        [StringLength(10)]
        [Display(Name = "PrdUnit Code")]
        public string PrdUnitCode { get; set; }


        [Display(Name = "Operating Unit")]
        public string PrdUnitName { get; set; }

        [StringLength(10)]
        [Display(Name = "Short name")]
        public string PrdUnitShortName { get; set; }


        [Display(Name = "is Production Unit")]
        public bool isPrdUnit { get; set; }


        [Display(Name = "Bangla name")]
        public string PrdUnitBanglaName { get; set; }

        [Display(Name = "SLNo")]
        public int SLNo { get; set; }


        //[Required]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Entry User")]

        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //[StringLength(128)]
        //public string comid { get; set; }


    }

    public partial class ShareHolding:BaseModel
    {
        [Key]
        public int ShareHoldingId { get; set; }


        [Display(Name = "Note No")]
        public string NoteNo { get; set; }


        [Display(Name = "Share Holder Name")]
        public string ShareHolderName { get; set; }
        [Display(Name = "Designation")]

        public string ShareHolderDesignation { get; set; }


        [Display(Name = "SLNo")]
        public int SLNo { get; set; }

        public int NoOfShareHoldings { get; set; }

        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }

        //[Required]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string Updatedby { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Entry User")]

        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //[StringLength(128)]
        //public string comid { get; set; }


    }

    public partial class NoteDescription:BaseModel
    {
        [Key]
        public int NoteDescriptionId { get; set; }

        [Display(Name = "Note No")]
        public string NoteNo { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string NoteDetails { get; set; }
        [DataType(DataType.MultilineText)]

        [Display(Name = "Remarks")]

        public string NoteRemarks { get; set; }

        [Display(Name = "SLNo")]
        public int SLNo { get; set; }


        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }

        //[Required]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string Updatedby { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Entry User")]

        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //[StringLength(128)]
        //public string comid { get; set; }


    }

    public partial class ShowVoucherViewModel
    {
        [Display(Name = "From Date ")]
        public string dtFrom { get; set; }

        [Display(Name = "To Date ")]
        public string dtTo { get; set; }
        [Display(Name = "Currency ")]
        public int? CountryId { get; set; }
        public List<Acc_FiscalYear> FiscalYs { get; set; }
        public List<Acc_FiscalMonth> ProcessMonths { get; set; }
        public List<Acc_FiscalHalfYear> ProcessHalfYear { get; set; }
        public List<Acc_FiscalQtr> ProcessQtr { get; set; }


        [Display(Name = "Voucher Type")]
        public int VoucherTypeId { get; set; }

        [Display(Name = "Voucher ID")]
        public string VoucherId { get; set; }

        [Display(Name = "Voucher From No")]
        public string VoucherFrom { get; set; }
        [Display(Name = "Voucher To No")]
        public string VoucherTo { get; set; }



        [Display(Name = "Accounts Name")]
        public int AccId { get; set; }


        [Display(Name = "Accounts Name ")]
        public int AccIdGroup { get; set; }

        [Display(Name = "Note One [CT] ")]
        public int AccIdNoteOneCT { get; set; }


        [Display(Name = "Accounts Name")]
        public int AccIdLedger { get; set; }




        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }
        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }




        [Display(Name = "Accounts Name")]
        public int AccIdRecPay { get; set; }



        public Boolean isLocalCurr { get; set; }

        public Boolean isDetailsReport { get; set; }


        public Boolean isOther { get; set; }
        public Boolean isPosted { get; set; }
        public Boolean isMaterial { get; set; }


        public Boolean isCompare { get; set; }
        public Boolean isCumulative { get; set; }


        public Boolean isGroup { get; set; }

        public Boolean isShowZero { get; set; }
        [Display(Name = "Operating / Prd. Unit ")]

        public Nullable<int> PrdUnitId { get; set; }



    }

    public partial class DocumentList
    {
        [Key]
        [Display(Name = "Document Id")]
        public int DocumentId { get; set; }

        [Display(Name = "Document No"), StringLength(20, ErrorMessage = "Max length 30 char.")]
        public string DocumentNo { get; set; }

        [Display(Name = "Date")]
        public string DocumentDate { get; set; }

        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }


        [Display(Name = "Document Status")]
        public string DocumentStatus { get; set; }

        [Display(Name = "Document Status")]
        public string DeptId { get; set; }

        [Display(Name = "Leave Balance")]
        public float? LeaveBalance { get; set; }
        [NotMapped]
        public string Remark { get; set; }
        [NotMapped]
        public string? LTypeName { get; set; }
        public string? Attachment { get; set; }
    }


    public partial class Acc_VoucherMain:BaseModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Acc_VoucherMain()
        //{
        //    this.VoucherSubs = new HashSet<Acc_VoucherSub>();
        //}
        [Key]
        public int VoucherId { get; set; }

        [NotMapped]
        public Nullable<int> AccId { get; set; }
        public int VoucherSerialId { get; set; }
        public int? YearlyVoucherTypeWiseSerial { get; set; }

        //public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }

        [NotMapped]
        [Display(Name = "Last No")]
        public string LastVoucherNo { get; set; }

        [Display(Name = "Voucher No"),
         StringLength(40, ErrorMessage = "Max length 40 char.")]
        public string VoucherNo { get; set; }
        [Display(Name = "Voucher Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        //[Column(TypeName = "date")]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Voucher Input Date")]
        public DateTime VoucherInputDate { get; set; }


        [Display(Name = "Voucher Type")]
        public int VoucherTypeId { get; set; }

        [Display(Name = "Unit")]
        public Nullable<int> PrdUnitId { get; set; }

        public virtual PrdUnit vPrdUnit { get; set; }


        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string VoucherDesc { get; set; }


        //[Required]
        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }


        public Boolean isAutoEntry { get; set; }
        public Boolean isPosted { get; set; }

        [Display(Name = "Currency"), Range(1, int.MaxValue, ErrorMessage = "Select Currency.")]
        public int CountryId { get; set; }

        [Display(Name = "Currency"), Range(1, int.MaxValue, ErrorMessage = "Select Currency.")]
        public int CountryIdLocal { get; set; }



        [Display(Name = "Amount")]

        public double VAmount { get; set; }
        [Display(Name = "In Words")]
        public string vAmountInWords { get; set; }


        public string Source { get; set; }
        public int SourceId { get; set; }
        [Display(Name = "Convert Rate"), Range(0, double.MaxValue, ErrorMessage = "Conversion rate must grater than zero.")]
        public double ConvRate { get; set; }

        [Display(Name = "Amount [Local]")]

        public double VAmountLocal { get; set; }

        [Display(Name = "Local In Words")]
        public string vAmountLocalInWords { get; set; }

        [Display(Name = "Ref. One")]

        public string Referance { get; set; }
        [Display(Name = "Ref. Two")]

        public string ReferanceTwo { get; set; }

        [Display(Name = "Amount / Ref. Three")]

        public string ReferanceThree { get; set; }



        //[Display(Name = "Local Currency")]
        //public int CurrIdLocal { get; set; }

        //[Display(Name = "Acc Name (Cash)"),
        // Range(1, int.MaxValue, ErrorMessage = "Select Account Name.")]
        //public int AccId { get; set; }





        //[Display(Name = "Voucher Date")]
        //public string DateFrom { get; set; }
        //[Display(Name = "Voucher Date")]
        //public string DateTo { get; set; }





        public Boolean IsCash { get; set; }

        //[Display(Name = "Currency Rate")]

        //public double CurrencyRate { get; set; }


        //[Display(Name = "Amount [Local]")]
        //public double AmountLocal { get; set; }



        //public string InWord { get; set; }
        [ForeignKey("VoucherTypeId")]
        public virtual Acc_VoucherType Acc_VoucherType { get; set; }


        [ForeignKey("CountryId")]
        public virtual Country Acc_Currency { get; set; }

        [ForeignKey("CountryIdLocal")]
        public virtual Country Acc_CurrencyLocal { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acc_VoucherSub> VoucherSubs { get; set; }

        //public virtual Acc_ChartOfAccount vChartofAccountsMain { get; set; }
        //public virtual Acc_VoucherType vVoucherType { get; set; }

        //public virtual Acc_VoucherSub vVouchersubMain { get; set; }
        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }


        [NotMapped]
        public string[] VoucherTranGroupArray { get; set; }

        public string VoucherTranGroupList { get; set; }


        [Display(Name = "Tran Group")]
        public Nullable<int> VoucherTranGroupId { get; set; }
        public virtual VoucherTranGroup VoucherTranGroups { get; set; }

        public virtual ICollection<Acc_VoucherTranGroup> Acc_VoucherTranGroups { get; set; }

    }

    //[Keyless]
    public partial class Acc_VoucherTranGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherTranId { get; set; }

        //[Key, Column(Order = 0)]
        public int VoucherId { get; set; }
        public int VoucherTranGroupId { get; set; }

        //[Key, Column(Order = 1)]
        //[Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        //[DataType(DataType.Text)]
        //[Display(Name = "Transaction Group")]

        [Display(Name = "Transaction Group")]
        [ForeignKey("VoucherTranGroupId")]
        public virtual VoucherTranGroup VoucherTranGroups { get; set; }
        [ForeignKey("VoucherId")]

        public virtual Acc_VoucherMain acc_vouchermains { get; set; }

    }


    public partial class Acc_VoucherSub
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Acc_VoucherSub()
        {
            this.VoucherSubChecnoes = new HashSet<Acc_VoucherSubCheckno>();
            this.VoucherSubSections = new HashSet<Acc_VoucherSubSection>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int VoucherSubId { get; set; }

        public int VoucherId { get; set; }
        public int AccId { get; set; }
        [ForeignKey("AccId")]

        public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }

        public int SRowNo { get; set; }

        public int ccId { get; set; }


        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Country Country { get; set; }


        public int CurrencyForeignId { get; set; }
        [ForeignKey("CurrencyForeignId")]
        public virtual Country CountryForeign { get; set; }

        public double TKDebit { get; set; }
        public double TKCredit { get; set; }

        //public double TKDebitForeign { get; set; }
        //public double TKCreditForeign { get; set; }


        //public double DebitForeign { get; set; }
        //public double CreditForeign { get; set; }

        public double TKDebitLocal { get; set; }
        public double TKCreditLocal { get; set; }



        public double CurrencyRate { get; set; }



        public string Note1 { get; set; }
        public string Note2 { get; set; }

        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public int? RowNo { get; set; }
        public int? RefId { get; set; }
        public int? SLNo { get; set; }


        public int? EmpId { get; set; }

        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Infos { get; set; }


        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customers { get; set; }

        public int? SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Suppliers { get; set; }



        //public virtual Acc_VoucherMain Acc_VoucherMain { get; set; }
        //[InverseProperty("Acc_VoucherSub")]
        [ForeignKey("VoucherId")]

        public virtual Acc_VoucherMain Acc_VoucherMain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Acc_VoucherSubCheckno> VoucherSubChecnoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acc_VoucherSubSection> VoucherSubSections { get; set; }

        public Nullable<int> VoucherTranGroupIdRow { get; set; }

        [Display(Name = "Transaction Group")]
        [ForeignKey("VoucherTranGroupIdRow")]
        public virtual VoucherTranGroup VoucherTranGroups { get; set; }
        //public virtual Country Country { get; set; }
        //public virtual Country CountryForeign { get; set; }
        //public int IsDel { get; set; }
        //public int RowNoDet { get; set; }
    }

    public class Acc_VoucherSubSection
    {
        [Key]
        public int VoucherSubSectionId { get; set; }
        public int VoucherSubId { get; set; }

        public int RowNoSSec { get; set; }
        public int VoucherId { get; set; }
        public int AccId { get; set; }
        public int SRowNo { get; set; }
        public int SubSectId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public double Amount { get; set; }
        //public virtual SubSection SubSection { get; set; }
        // Himu change
        public virtual Cat_SubSection SubSection { get; set; }
        [ForeignKey("VoucherSubId")]

        public virtual Acc_VoucherSub Acc_VoucherSub { get; set; }
    }

    public class Acc_VoucherSubCheckno : BaseModel
    {
        [Key]
        public int VoucherSubCheckId { get; set; }
        public int? VoucherSubId { get; set; }
        public int RowNoChk { get; set; }
        public int? VoucherId { get; set; }
        public int AccId { get; set; }
        public int? SRowNo { get; set; }
        public string ChkNo { get; set; }
        public DateTime? dtChk { get; set; }
        public string dtChkTo { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "Interest Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestRate { get; set; }

        public bool isClear { get; set; }
        public string dtChkClear { get; set; }
        public string Criteria { get; set; }



        [ForeignKey("VoucherSubId")]
        public virtual Acc_VoucherSub Acc_VoucherSub { get; set; }



        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }




        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }


        public Boolean isManualEntry { get; set; }

        //public virtual ICollection<Acc_VoucherSubCheckno_Clearing> Acc_VoucherSubCheckno_Clearings { get; set; }
    }

    //public class Acc_VoucherSubCheckno_Clearing
    //{
    //    [Key]
    //    public int VoucherSubCheckNoClearingId { get; set; }
    //    public int VoucherSubCheckId { get; set; }
    //    [ForeignKey("VoucherSubCheckId")]
    //    public virtual Acc_VoucherSubCheckno Acc_VoucherSubChecknos { get; set; }
    //    [Display(Name = "Voucher Date")]
    //    [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
    //    [DataType(DataType.Date)]
    //    public DateTime dtChkClear { get; set; }
    //    public bool isClear { get; set; }

    //    public string Remarks { get; set; }


    //    [StringLength(128)]
    //    public string userid { get; set; }


    //    [StringLength(128)]
    //    public string comid { get; set; }

    //    [StringLength(128)]
    //    [Display(Name = "Update By")]

    //    public string useridUpdate { get; set; }

    //    public Nullable<System.DateTime> DateAdded { get; set; }
    //    public Nullable<System.DateTime> DateUpdated { get; set; }

    //}



    public class Acc_BudgetMain:BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Acc_BudgetMain()
        {
            this.BudgetSubs = new HashSet<Acc_BudgetSub>();
        }
        [Key]
        public int BudgetId { get; set; }


        public int BudgetSerialId { get; set; }
        //public virtual ChartOfAccount ChartOfAccount { get; set; }



        [Display(Name = "Budget No"),
         StringLength(100, ErrorMessage = "Max length 100 char.")]
        public string BudgetNo { get; set; }
        [Display(Name = "Budget Date")]
        public DateTime BudgetDate { get; set; }


        [Display(Name = "Fiscal Year")]
        public Nullable<int> FiscalYearId { get; set; }

        public virtual Acc_FiscalYear vFiscalYear { get; set; }


        [Display(Name = "Fiscal Year")]
        public Nullable<int> FiscalMonthId { get; set; }

        public virtual Acc_FiscalMonth vFiscalMonth { get; set; }

        [Display(Name = "Operation / Production Unit")]
        public Nullable<int> PrdUnitId { get; set; }

        public virtual PrdUnit vPrdUnit { get; set; }


        [Display(Name = "Description")]
        [DataType(DataType.Text)]
        public string BudgetDesc { get; set; }

        //[StringLength(128)]

        ////[Required]
        //[Display(Name = "Company Id")]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }


        public Boolean isAutoEntry { get; set; }
        public Boolean isPosted { get; set; }

        [Display(Name = "Currency"), Range(1, int.MaxValue, ErrorMessage = "Select Currency.")]
        public int CountryId { get; set; }

        [Display(Name = "Currency"), Range(1, int.MaxValue, ErrorMessage = "Select Currency.")]
        public int CountryIdLocal { get; set; }



        [Display(Name = "Amount")]

        public double VAmount { get; set; }
        [Display(Name = "In Words")]
        public string vAmountInWords { get; set; }



        [Display(Name = "Convert Rate"), Range(0, double.MaxValue, ErrorMessage = "Conversion rate must grater than zero.")]
        public double ConvRate { get; set; }

        [Display(Name = "Amount [Local]")]

        public double VAmountLocal { get; set; }

        [Display(Name = "Local In Words")]
        public string vAmountLocalInWords { get; set; }
        public string Referance { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acc_BudgetSub> BudgetSubs { get; set; }





    }

    public class Acc_BudgetSub
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Acc_BudgetSub()
        {

            this.BudgetSubSections = new HashSet<Acc_BudgetSubSection>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BudgetSubId { get; set; }

        public int BudgetId { get; set; }
        public int AccId { get; set; }
        public virtual Acc_ChartOfAccount ChartOfAccount { get; set; }

        public int SRowNo { get; set; }




        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Country Country { get; set; }


        public int CurrencyForeignId { get; set; }
        [ForeignKey("CurrencyForeignId")]
        public virtual Country CountryForeign { get; set; }

        public double TKDebit { get; set; }
        public double TKCredit { get; set; }

        //public double TKDebitForeign { get; set; }
        //public double TKCreditForeign { get; set; }


        //public double DebitForeign { get; set; }
        //public double CreditForeign { get; set; }

        public double TKDebitLocal { get; set; }
        public double TKCreditLocal { get; set; }



        public double CurrencyRate { get; set; }



        public string Note1 { get; set; }
        public string Note2 { get; set; }


        public int? RowNo { get; set; }


        public virtual Acc_BudgetMain Acc_BudgetMain { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acc_BudgetSubSection> BudgetSubSections { get; set; }


    }


    public class Acc_BudgetSubSection
    {
        [Key]
        public int BudgetSubSectionId { get; set; }
        public int BudgetSubId { get; set; }

        public int RowNoSSec { get; set; }
        public int BudgetId { get; set; }
        public int AccId { get; set; }
        public int SRowNo { get; set; }
        public int SubSectId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public double Amount { get; set; }
        public virtual Cat_SubSection SubSection { get; set; }
        [ForeignKey("BudgetSubId")]
        public virtual Acc_BudgetSub BudgetSub { get; set; }
    }



    public partial class Acc_VoucherNoPrefix : BaseModel
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int VoucherNoPrefixId { get; set; }

        [Display(Name = "Voucher Type")]
        //[Key, Column(Order = 0)]
        public int VoucherTypeId { get; set; }
        [ForeignKey("VoucherTypeId")]
        public virtual Acc_VoucherType vVoucherTypes { get; set; }

        [Display(Name = "Voucher Prefix")]

        public string VoucherShortPrefix { get; set; }
        //[Key, Column(Order = 1)]

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }
        [Display(Name = "No Length")]
        public int Length { get; set; } = 0;


        [Display(Name = "Visible For Entry")]
        public bool isVisible { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }


        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }

    public partial class Acc_VoucherType : SelfModel
    {
        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        //
        //public Acc_VoucherType()
        //{
        //    this.VoucherMains = new HashSet<Acc_VoucherMain>();
        //}
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int VoucherTypeId { get; set; }

        [Display(Name = "Voucher Type")]
        public string VoucherTypeName { get; set; }
        [Display(Name = "Short Name")]

        public string VoucherTypeNameShort { get; set; }
        [Display(Name = "Voucher Type Class [HTML Design / Panel Color]")]

        public string VoucherTypeClass { get; set; }

        [Display(Name = "Button Class [HTML Design / Button Color]")]

        public string VoucherTypeButtonClass { get; set; }
        [Display(Name = "Visible For Entry")]
        public bool isSystem { get; set; }

        public virtual ICollection<Acc_VoucherNoPrefix> VoucherNoPrefixs { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acc_VoucherMain> VoucherMains { get; set; }


    }



    public class Acc_GovtSchedule_StoreInTransit
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }

        public string Criteria { get; set; }
        [Display(Name = "CT-NO.")]

        public string Description { get; set; }
        public DateTime FromDate { get; set; }


        [Display(Name = "LC-NO")]
        public string LPNo { get; set; }
        [Display(Name = "LC-DATE")]

        public DateTime LPDate { get; set; }

        public string Remarks { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Amount Taka")]
        public decimal Amount { get; set; }



        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }


        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }


    public class Acc_GovtSchedule_Equity : BaseModel
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }

        public string Criteria { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Loan")]
        public decimal Loan { get; set; }

        [Display(Name = "Development")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Development { get; set; }


        [Display(Name = "CD & VAT")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CDVAT { get; set; }

        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }

    public class Acc_GovtSchedule_Loan
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }

        public string Criteria { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Principle")]
        public decimal Principle { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Interest { get; set; }


        [Display(Name = "Total Loan")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLoan { get; set; }

        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }


        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }

    public class PF_Ledger : BaseModel
    {
        [Key]
        public int PFLedgerId { get; set; }

        public int BankAccountId { get; set; }
        public string VoucherNo { get; set; }
        public string ChequeNo { get; set; }
        public string Criteria { get; set; }

        public string AmountType { get; set; }
        public string Description { get; set; }
        public DateTime TranDate { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Received(TK)")]
        public decimal ReceivedTK { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Payment(TK)")]
        public decimal PaymentTK { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Balance(TK)")]
        public decimal Balance { get; set; }

        public bool isPrinciple { get; set; }


        [ForeignKey("BankAccountId")]
        public virtual BankAccountNo vBankAccountNo { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }

    public class WF_Ledger : BaseModel
    {
        [Key]
        public int WFLedgerId { get; set; }
        public int BankAccountId { get; set; }
        public string VoucherNo { get; set; }
        public string ChequeNo { get; set; }
        public string Criteria { get; set; }
        public string AmountType { get; set; }
        public string Description { get; set; }
        public DateTime TranDate { get; set; }
        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Received(TK)")]
        public decimal ReceivedTK { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Payment(TK)")]
        public decimal PaymentTK { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Balance(TK)")]
        public decimal Balance { get; set; }

        public bool isPrinciple { get; set; }


        [ForeignKey("BankAccountId")]
        public virtual BankAccountNo vBankAccountNo { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }

    public class Gratuity_Ledger : BaseModel
    {
        [Key]
        public int GratuityLedgerId { get; set; }

        public int BankAccountId { get; set; }
        public string VoucherNo { get; set; }
        public string ChequeNo { get; set; }
        public string Criteria { get; set; }

        public string AmountType { get; set; }
        public string Description { get; set; }
        public DateTime TranDate { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Received(TK)")]
        public decimal ReceivedTK { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Payment(TK)")]
        public decimal PaymentTK { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Balance(TK)")]
        public decimal Balance { get; set; }

        public bool isPrinciple { get; set; }


        [ForeignKey("BankAccountId")]
        public virtual BankAccountNo vBankAccountNo { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }

    public class Cat_ITInvestmentItem
    {
        [Key]
        public int ITInvestId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FYID { get; set; }
        [ForeignKey("FYID")]
        public virtual Acc_FiscalYear Acc_FiscalYear { get; set; }

        [Display(Name = "Date")]
        public DateTime DtInput { get; set; }



        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Received(TK)")]
        public decimal ReceivedTK { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "NSCB Amt")]
        public decimal NSCBAmt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "DPS Amt")]
        public decimal DPSAmt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "DSSM Fund Amt")]
        public decimal DSSMFundAmt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "LIP Amt")]
        public decimal LIPAmt { get; set; }

        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }


    public class Acc_GovtSchedule_ForeignLoan : BaseModel
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }
        [Display(Name = "Group By Name")]
        public string GroupByName { get; set; }

        public string Criteria { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? DateOfPayment { get; set; }


        public string Remarks { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Milestone Portion / Principle (RMB Yuan)")]
        public decimal MilestonePortionPrincipleRMB { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal ExchangeRate { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Milestone Portion / Principle (In Taka)")]
        public decimal MilestonePortionPrincipleTaka { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Interest (In Taka)")]
        public decimal Interest { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Exchange Fluc.  Loss / (Gain) (In Taka)")]
        public decimal ExchangeFlucLossGain { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total In Taka")]
        public decimal Total { get; set; }

        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }




    public class Acc_GovtSchedule_JapanLoan : BaseModel
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }
        [Display(Name = "Group By Name")]
        public string GroupByName { get; set; }

        [Display(Name = "Portion Type")]
        public string PortionType { get; set; }

        public string Criteria { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FromDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ToDate { get; set; }
        public DateTime? DateOfPayment { get; set; }

        public string Remarks { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Milestone Portion (Japanse Yen)")]
        public decimal MilestonePortionYen { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Supply Portion (Japanse Yen)")]
        public decimal SupplyPortionYen { get; set; }


        [Display(Name = "Interest Portion (Japanse Yen)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestPortionYen { get; set; }


        [Display(Name = "Payment Portion (Japanse Yen)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentPortionYen { get; set; }


        [Display(Name = "Total Amount (In Yen)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmountYen { get; set; }


        [Display(Name = "Exchange Rate")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal ExchangeRate { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Milestone Portion (In Taka)")]
        public decimal MilestonePortionTaka { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Supply Portion (In Taka)")]
        public decimal SupplyPortionTaka { get; set; }


        [Display(Name = "Interest Portion (In Taka)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestPortionTaka { get; set; }

        [Display(Name = "Payment Portion (In Taka)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentPortionTaka { get; set; }


        [Display(Name = "Interest (In Taka)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestAmountTaka { get; set; }


        [Display(Name = "Exchange Fluc. Loss/ (Gain) In Taka")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExchangeLossGainTaka { get; set; }


        [Display(Name = "Total Amount (In Taka)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmountTaka { get; set; }

        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }


    public class Acc_GovtSchedule_Subsidy
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }

        public string Criteria { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remarks { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Plant - 1")]
        public decimal ReceivablePlantOne { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Plant - 2")]
        public decimal ReceivablePlantTwo { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total")]
        public decimal ReceivableTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cumulative")]
        public decimal ReceivableCumulative { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Plant - 1")]
        public decimal ReceivedPlantOne { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Plant - 2")]
        public decimal ReceivedPlantTwo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total")]
        public decimal ReceivedTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cumulative")]
        public decimal ReceivedCumulative { get; set; }





        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Plant - 1")]
        public decimal BalancePlantOne { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Plant - 2")]
        public decimal BalancePlantTwo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total")]
        public decimal BalanceTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cumulative")]
        public decimal BalanceCumulative { get; set; }


        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }


        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }



    public class Acc_AdvanceIncomeTax_Schedule
    {
        [Key]
        public int GovtScheduleId { get; set; }

        public int AccId { get; set; }
        [Display(Name = "Group By Name")]
        public string GroupByName { get; set; }

        public string Criteria { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string ContractNo { get; set; }

        public DateTime? ContractDate { get; set; }


        public string LCNo { get; set; }

        public DateTime? LCDate { get; set; }

        public string Description { get; set; }

        public string BENo { get; set; }

        public DateTime? BEDate { get; set; }


        public string Remarks { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Assesed Amount For VAT & Duty")]
        public decimal TotalAssesedAmountVat { get; set; }

        [Display(Name = "Amount of AIT")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AITAmount { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total AIT")]
        public decimal TotalAIT { get; set; }

        public bool isPost { get; set; }


        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vAcc_ChartOfAccount { get; set; }

        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }


        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }





    public partial class SalesMain
    {


        [Key]
        public int SalesId { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Reference")]
        public string ReferenceNo { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "Sales No")]
        public string SalesNo { get; set; }


        [Required]
        [Display(Name = "Sales Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SalesDate { get; set; }


        //[Required]
        [Display(Name = "Sales Person")]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]


        public string SalesPerson { get; set; } = "=N/A=";


        [Required]
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; } = 1;


        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }


        // [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string PostalCode { get; set; }

        //[Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailId { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }




        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }


        [Display(Name = "No. Of Items[Sum]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumQty { get; set; }

        [Display(Name = "No. Of Items [Count]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlCountQty { get; set; }

        [Display(Name = "Unit Price")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlUnitPrice { get; set; }
        [Display(Name = "Individual Vat")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlIndVat { get; set; }

        [Display(Name = "Individual Discount")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlIndDisAmt { get; set; }

        [Display(Name = "Individual Price")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlIndPrice { get; set; }




        [Display(Name = "Sum of Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumAmt { get; set; }





        [Display(Name = "% Discount")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal DisPer { get; set; }
        [Display(Name = "Dis. Amount")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal DisAmt { get; set; }

        [Display(Name = "Service Charge")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal ServiceCharge { get; set; }

        [Display(Name = "Shipping")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal Shipping { get; set; }

        [Display(Name = "Total Vat ")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVat { get; set; }


        [Display(Name = "Net Amount")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }
        [Display(Name = "Paid Amount")]


        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmt { get; set; }
        [Display(Name = "Due Amount")]


        [Column(TypeName = "decimal(18,2)")]
        public decimal DueAmt { get; set; }

        [Display(Name = "Currency")]


        public int CountryId { get; set; }
        [Display(Name = "Currency Rate")]


        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrencyRate { get; set; }
        [Display(Name = "Net Amount BDT")]


        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountBDT { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "PaidInWords")]
        [DataType(DataType.Text)]
        public string PaidInWords { get; set; }

        [Display(Name = "NetInWords")]
        [DataType(DataType.Text)]
        public string NetInWords { get; set; }

        public Boolean ChkPer { get; set; }
        [StringLength(128)]

        [Display(Name = "Company Id")]
        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public int AccountId { get; set; }


        public int CustomerContactId { get; set; }

        [Display(Name = "Posted")]

        public Boolean isPost { get; set; }

        public virtual ICollection<SalesSub> SalesSubs { get; set; }

        public virtual ICollection<SalesTermsSub> SalesTermsSubs { get; set; }
        public virtual ICollection<SalesPaymentSub> SalesPaymentSubs { get; set; }

        public virtual Customer vCustomertName { get; set; }
        public virtual Country vCurrencySalesMain { get; set; }

    }

    public class SalesSub
    {

        [Key, Column(Order = 0)]
        public int SalesId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Product / Item")]
        public int ProductId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [Display(Name = "Type")]
        public int SalesTypeId { get; set; }



        [Required]
        [Display(Name = "Store Location")]
        public int WarehouseId { get; set; }
        //public int ProductId { get; set; }


        //[DisplayFormat(NullDisplayText = "=N/A=", ApplyFormatInEditMode = true)]
        [Display(Name = "Product Serial / IMEI")]
        [Key, Column(Order = 4)]

        public int? ProductSerialId { get; set; } = 0;
        //public int ProductId { get; set; }
        //[Required]
        [Key, Column(Order = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        //public int ProductId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Size")]
        public string Size { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Carton")]
        public string Carton { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "PCTN")]
        public string PCTN { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }



        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }





        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVatPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVat { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisAmt { get; set; }

        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndPrice { get; set; }

        public int IndChkPer { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }


        public int RowNo { get; set; }

        [ForeignKey("SalesId")]
        public virtual SalesMain SalesMain { get; set; }
        [ForeignKey("ProductId")]

        public virtual Product vProductName { get; set; }
        [ForeignKey("WarehouseId")]

        public virtual Warehouse vWarehouse { get; set; }
        [ForeignKey("ProductSerialId")]


        public virtual ProductSerial vProductSerial { get; set; }
        [ForeignKey("SalesTypeId")]


        public virtual SalesType vSalesTypes { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }


    public class SalesTermsSub
    {

        [Key, Column(Order = 0)]
        public int SalesId { get; set; }

        //[Key, Column(Order = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms")]
        public string Terms { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }




        public int RowNo { get; set; }

        //public string ComId { get; set; }




        [ForeignKey("SalesId")]
        public virtual SalesMain SalesMain { get; set; }



        //public virtual ICollection<Product> vProducts { get; set; }




    }


    public class SalesPaymentSub
    {

        [Key, Column(Order = 0)]
        //public int SalesPaymentSubId { get; set; }

        public int SalesId { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]

        [Display(Name = "Payment Type")]
        public int PaymentTypeId { get; set; }
        public string PaymentCardNo { get; set; }


        public Boolean isPosted { get; set; }

        public int? AccId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Amount { get; set; }



        public int? RowNo { get; set; }

        //public string ComId { get; set; }


        [Required]
        public string ComId { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }


        [ForeignKey("SalesId")]

        public virtual SalesMain SalesMain { get; set; }

        [ForeignKey("PaymentTypeId")]

        public virtual PaymentType vPaymentType { get; set; }
        [ForeignKey("AccId")]


        public virtual Acc_ChartOfAccount vChartofAccounts { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }

    public class PurchaseMain
    {
        public PurchaseMain()
        {
            this.PurchaseSubs = new HashSet<PurchaseSub>();
            this.PurchaseTermsSubs = new HashSet<PurchaseTermsSub>();
            this.PurchasePaymentSubs = new HashSet<PurchasePaymentSub>();
            this.PurchaseReturnMains = new HashSet<PurchaseReturnMain>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PurchaseId { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Reference")]
        public string ReferenceNo { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "Purchase No")]
        public string PurchaseNo { get; set; }


        [Required]
        [Display(Name = "Purchase Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }


        //[Required]
        [Display(Name = "Purchase Person")]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]


        public string PurchasePerson { get; set; } = "=N/A=";


        [Required]
        [Display(Name = "Supplier Name")]
        public int SupplierId { get; set; } = 1;


        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }


        // [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string PostalCode { get; set; }

        //[Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailId { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }




        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }


        [Display(Name = "No. Of Items[Sum]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumQty { get; set; }

        [Display(Name = "No. Of Items [Count]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlCountQty { get; set; }

        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlUnitPrice { get; set; }
        [Display(Name = "Individual Vat")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlIndVat { get; set; }

        [Display(Name = "Individual Discount")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlIndDisAmt { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Individual Price")]
        public decimal ttlIndPrice { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Sum of Amount")]
        public decimal ttlSumAmt { get; set; }





        [Display(Name = "% Discount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisPer { get; set; }
        [Display(Name = "Dis. Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisAmt { get; set; }

        [Display(Name = "Service Charge")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServiceCharge { get; set; }

        [Display(Name = "Shipping")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Shipping { get; set; }

        [Display(Name = "Total Vat ")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVat { get; set; }


        [Display(Name = "Net Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public float NetAmount { get; set; }
        [Display(Name = "Paid Amount")]


        public float PaidAmt { get; set; }
        [Display(Name = "Due Amount")]


        public float DueAmt { get; set; }

        [Display(Name = "Currency")]


        public int CountryId { get; set; }
        [Display(Name = "Currency Rate")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrencyRate { get; set; }
        [Display(Name = "Net Amount BDT")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountBDT { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "PaidInWords")]
        [DataType(DataType.Text)]
        public string PaidInWords { get; set; }

        [Display(Name = "NetInWords")]
        [DataType(DataType.Text)]
        public string NetInWords { get; set; }

        public Boolean ChkPer { get; set; }

        //[Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]

        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        public int AccountId { get; set; }


        public int SupplierContactId { get; set; }


        public Boolean isPost { get; set; }
        [Display(Name = "Posted")]

        public virtual ICollection<PurchaseSub> PurchaseSubs { get; set; }
        public virtual ICollection<PurchaseReturnMain> PurchaseReturnMains { get; set; }

        public virtual ICollection<PurchasePaymentSub> PurchasePaymentSubs { get; set; }

        public virtual ICollection<PurchaseTermsSub> PurchaseTermsSubs { get; set; }


        public virtual Supplier vSuppliertName { get; set; }
        public virtual Country vCurrencyPurchaseMain { get; set; }

    }



    public class PurchaseSub
    {
        public PurchaseSub()
        {
            this.ProductSerials = new HashSet<ProductSerial>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseSubId { get; set; }

        //[Key, Column(Order = 0)]
        //[ForeignKey("purchasemain")]
        public int PurchaseId { get; set; }
        //[Key, Column(Order = 1)]
        //[Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Product / Item")]
        public int ProductId { get; set; }


        [Display(Name = "Type")]
        public int? SalesTypeId { get; set; }


        [Required]
        [Display(Name = "Store Location")]
        public int WarehouseId { get; set; }
        //public int ProductId { get; set; }


        [DisplayFormat(NullDisplayText = "=N/A=", ApplyFormatInEditMode = true)]
        [Display(Name = "Product Serial / IMEI")]
        //[Key, Column(Order = 4)]

        public int? ProductSerialId { get; set; } = 0;

        //public int ProductId { get; set; }
        //[Required]
        //[Key, Column(Order = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        //public int ProductId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Size")]
        public string Size { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Carton")]
        public string Carton { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "PCTN")]
        public string PCTN { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }



        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }





        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVatPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVat { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisAmt { get; set; }

        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndPrice { get; set; }

        public int IndChkPer { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }



        public virtual Product vProductName { get; set; }
        public virtual Warehouse vWarehouse { get; set; }


        public virtual ICollection<ProductSerial> ProductSerials { get; set; }



        //public virtual ProductSerial vProductSerial { get; set; }




        //public virtual ICollection<Product> vProducts { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual PurchaseMain PurchaseMain { get; set; }

        public int RowNo { get; set; }


    }

    public class ProductSerial
    {


        [Key]
        //[Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductSerialId { get; set; }


        public int PurchaseSubId { get; set; }

        public int PurchaseId { get; set; }


        //[Key, Column(Order = 1)]
        //[ForeignKey("PurchaseSub")]


        //[Key, Column(Order = 2)]
        //[ForeignKey("PurchaseSub")]
        public int ProductId { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Product Serial / IMEI")]
        [DisplayFormat(NullDisplayText = "=N/A=")]
        public string ProductSerialNo { get; set; }

        public string Remarks { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }



        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }


        [Required]
        [Display(Name = "Warrenty")]
        public int WarrentyId { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        //public virtual ICollection<SalesSub> vProductSerialSalesSubs { get; set; }

        //public virtual Product vProductForSerial { get; set; }

        public virtual ICollection<SalesSub> vSalesSubSerial { get; set; }


        public virtual PurchaseSub PurchaseSub { get; set; }

        public virtual Warrenty vWarrenty { get; set; }

    }

    public class PurchasePaymentSub
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchasePaymentSubId { get; set; }

        public int PurchaseId { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]

        [Display(Name = "Payment Type")]
        public int PaymentTypeId { get; set; }
        public string PaymentCardNo { get; set; }


        public Boolean isPosted { get; set; }

        public int? AccId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Amount { get; set; }



        public int? RowNo { get; set; }

        //public string ComId { get; set; }


        [StringLength(128)]
        public string ComId { get; set; }


        [StringLength(128)]
        public string userid { get; set; }


        [ForeignKey("PurchaseId")]
        public virtual PurchaseMain PurchaseMain { get; set; }

        [ForeignKey("PaymentTypeId")]

        public virtual PaymentType vPaymentType { get; set; }
        [ForeignKey("AccId")]

        public virtual Acc_ChartOfAccount vChartofAccounts { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }


    public class PurchaseTermsSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseTermsSubId { get; set; }
        //[Key, Column(Order = 0)]
        public int PurchaseId { get; set; }

        //[Key, Column(Order = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms")]
        public string Terms { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }




        public int RowNo { get; set; }

        public string ComId { get; set; }




        [ForeignKey("PurchaseId")]
        public virtual PurchaseMain PurchaseMain { get; set; }



        //public virtual ICollection<Product> vProducts { get; set; }




    }
    ///////////////////// Purchase Retrun ///////////////////////
    public class PurchaseReturnMain
    {
        public PurchaseReturnMain()
        {
            this.PurchaseReturnSubs = new HashSet<PurchaseReturnSub>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseReturnId { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Reference")]
        public string ReferenceNo { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "PurchaseReturn No")]
        public string PurchaseReturnNo { get; set; }


        [Required]
        [Display(Name = "PurchaseReturn Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseReturnDate { get; set; }


        //[Required]
        [Display(Name = "PurchaseReturn Person")]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]


        public string PurchaseReturnPerson { get; set; } = "=N/A=";


        [Required]
        [Display(Name = "Supplier Name")]
        public int SupplierId { get; set; } = 1;


        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }


        // [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string PostalCode { get; set; }

        //[Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailId { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }




        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }


        [Display(Name = "No. Of Items[Sum]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumQty { get; set; }

        [Display(Name = "No. Of Items [Count]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlCountQty { get; set; }

        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlUnitPrice { get; set; }
        [Display(Name = "Individual Vat")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlIndVat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Individual Discount")]

        public decimal ttlIndDisAmt { get; set; }

        [Display(Name = "Individual Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlIndPrice { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Sum of Amount")]
        public decimal ttlSumAmt { get; set; }





        [Display(Name = "% Discount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisPer { get; set; }
        [Display(Name = "Dis. Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisAmt { get; set; }

        [Display(Name = "Service Charge")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServiceCharge { get; set; }

        [Display(Name = "Shipping")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Shipping { get; set; }

        [Display(Name = "Total Vat ")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVat { get; set; }


        [Display(Name = "Net Amount")]

        public float NetAmount { get; set; }
        [Display(Name = "Paid Amount")]


        public float PaidAmt { get; set; }
        [Display(Name = "Due Amount")]


        public float DueAmt { get; set; }

        [Display(Name = "Currency")]


        public int CountryId { get; set; }
        [Display(Name = "Currency Rate")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrencyRate { get; set; }
        [Display(Name = "Net Amount BDT")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountBDT { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "PaidInWords")]
        [DataType(DataType.Text)]
        public string PaidInWords { get; set; }

        [Display(Name = "NetInWords")]
        [DataType(DataType.Text)]
        public string NetInWords { get; set; }

        public Boolean ChkPer { get; set; }


        //[Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]

        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }


        public int AccountId { get; set; }


        public int SupplierContactId { get; set; }

        [Display(Name = "Posted")]

        public Boolean isPost { get; set; }

        public virtual ICollection<PurchaseReturnSub> PurchaseReturnSubs { get; set; }

        public virtual ICollection<PurchaseReturnTermsSub> PurchaseReturnTermsSubs { get; set; }
        public virtual ICollection<PurchaseReturnPaymentSub> PurchaseReturnPaymentSubs { get; set; }

        public virtual Supplier vSuppliertName { get; set; }
        public virtual Country vCurrencyPurchaseReturnMain { get; set; }



        public bool isDirectReturn { get; set; }


        //[Required]
        [Display(Name = "Purchase Invoice No")]
        //[ForeignKey("vPurchaseMains")]
        public int? PurchaseId { get; set; }

        public virtual PurchaseMain vPurchaseMains { get; set; }


    }


    public class PurchaseReturnSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseReturnSubId { get; set; }

        //[Key, Column(Order = 0)]
        public int PurchaseReturnId { get; set; }

        //[Key, Column(Order = 1)]
        //[Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Product / Item")]
        public int ProductId { get; set; }

        [Display(Name = "Type")]
        public int? SalesTypeId { get; set; }


        [Required]
        [Display(Name = "Store Location")]
        public int WarehouseId { get; set; }
        //public int ProductId { get; set; }


        //[DisplayFormat(NullDisplayText = "=N/A=", ApplyFormatInEditMode = true)]
        [Display(Name = "Product Serial / IMEI")]
        //[Key, Column(Order = 3)]

        public int? ProductSerialId { get; set; } //= 0;
        //public int ProductId { get; set; }
        //[Required]
        //[Key, Column(Order = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        //public int ProductId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Size")]
        public string Size { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Carton")]
        public string Carton { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "PCTN")]
        public string PCTN { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }



        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }





        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVatPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVat { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisAmt { get; set; }

        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndPrice { get; set; }

        public int IndChkPer { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }


        public virtual PurchaseReturnMain PurchaseReturnMain { get; set; }

        public virtual Product vProductName { get; set; }
        public virtual Warehouse vWarehouse { get; set; }

        public virtual ProductSerial vProductSerial { get; set; }



        //public virtual ICollection<Product> vProducts { get; set; }



        public int RowNo { get; set; }

        [NotMapped]
        public bool isChecked { get; set; }

    }



    public class PurchaseReturnTermsSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseReturnTermsSubId { get; set; }

        //[Key, Column(Order = 0)]
        public int PurchaseReturnId { get; set; }

        //[Key, Column(Order = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms")]
        public string Terms { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }




        public int RowNo { get; set; }

        //public string comid { get; set; }





        public virtual PurchaseReturnMain PurchaseReturnMain { get; set; }



        //public virtual ICollection<Product> vProducts { get; set; }




    }



    public class PurchaseReturnPaymentSub
    {

        [Key]
        public int PurchaseReturnPaymentSubId { get; set; }

        public int PurchaseReturnId { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]

        [Display(Name = "Payment Type")]
        public int PaymentTypeId { get; set; }
        public string PaymentCardNo { get; set; }


        public Boolean isPosted { get; set; }

        public int? AccId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }



        public int? RowNo { get; set; }

        //public string comid { get; set; }


        [Required]
        [StringLength(128)]
        public string comid { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }



        public virtual PurchaseReturnMain PurchaseReturnMain { get; set; }

        public virtual PaymentType vPaymentType { get; set; }

        public virtual Acc_ChartOfAccount vChartofAccounts { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }



    //////////////////////// Sales Return /////////////////////////


    public class SalesReturnMain
    {


        [Key]
        public int SalesReturnId { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Reference")]
        public string ReferenceNo { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "SalesReturn No")]
        public string SalesReturnNo { get; set; }


        [Required]
        [Display(Name = "SalesReturn Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SalesReturnDate { get; set; }


        //[Required]
        [Display(Name = "SalesReturn Person")]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]


        public string SalesReturnPerson { get; set; } = "=N/A=";


        [Required]
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; } = 1;


        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }


        // [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string PostalCode { get; set; }

        //[Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailId { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }




        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }


        [Display(Name = "No. Of Items[Sum]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumQty { get; set; }

        [Display(Name = "No. Of Items [Count]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlCountQty { get; set; }

        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlUnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Individual Vat")]

        public decimal ttlIndVat { get; set; }

        [Display(Name = "Individual Discount")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlIndDisAmt { get; set; }

        [Display(Name = "Individual Price")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal ttlIndPrice { get; set; }




        [Display(Name = "Sum of Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumAmt { get; set; }





        [Display(Name = "% Discount")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal DisPer { get; set; }
        [Display(Name = "Dis. Amount")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal DisAmt { get; set; }

        [Display(Name = "Service Charge")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]

        public decimal ServiceCharge { get; set; }

        [Display(Name = "Shipping")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Shipping { get; set; }

        [Display(Name = "Total Vat ")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal TotalVat { get; set; }


        [Display(Name = "Net Amount")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal NetAmount { get; set; }
        [Display(Name = "Paid Amount")]
        [Column(TypeName = "decimal(18,2)")]


        public decimal PaidAmt { get; set; }
        [Display(Name = "Due Amount")]
        [Column(TypeName = "decimal(18,2)")]


        public decimal DueAmt { get; set; }

        [Display(Name = "Currency")]


        public int CountryId { get; set; }
        [Display(Name = "Currency Rate")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrencyRate { get; set; }
        [Display(Name = "Net Amount BDT")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountBDT { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "PaidInWords")]
        [DataType(DataType.Text)]
        public string PaidInWords { get; set; }

        [Display(Name = "NetInWords")]
        [DataType(DataType.Text)]
        public string NetInWords { get; set; }

        public Boolean ChkPer { get; set; }


        //[Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]

        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }


        public int AccountId { get; set; }


        public int CustomerContactId { get; set; }

        [Display(Name = "Posted")]

        public Boolean isPost { get; set; }

        public virtual ICollection<SalesReturnSub> SalesReturnSubs { get; set; }

        public virtual ICollection<SalesReturnTermsSub> SalesReturnTermsSubs { get; set; }
        public virtual ICollection<SalesReturnPaymentSub> SalesReturnPaymentSubs { get; set; }

        public virtual Customer vCustomertName { get; set; }
        public virtual Country vCurrencySalesReturnMain { get; set; }



        public bool isDirectReturn { get; set; }


        //[Required]
        [Display(Name = "Sales Invoice No")]
        [ForeignKey("vSalesMain")]
        public int? SalesId { get; set; }

        public virtual SalesMain vSalesMain { get; set; }


    }


    public class SalesReturnSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesReturnSubId { get; set; }
        //[Key, Column(Order = 0)]
        public int SalesReturnId { get; set; }

        //[Key, Column(Order = 2)]
        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Product / Item")]
        public int ProductId { get; set; }

        //[Key, Column(Order = 1)]
        [Required]
        [Display(Name = "Type")]
        public int SalesTypeId { get; set; }



        [Required]
        [Display(Name = "Store Location")]
        public int WarehouseId { get; set; }
        //public int ProductId { get; set; }


        //[DisplayFormat(NullDisplayText = "=N/A=", ApplyFormatInEditMode = true)]
        [Display(Name = "Product Serial / IMEI")]
        //[Key, Column(Order = 4)]

        public int? ProductSerialId { get; set; } //= 0;
        //public int ProductId { get; set; }
        //[Required]
        //[Key, Column(Order = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        //public int ProductId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Size")]
        public string Size { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Carton")]
        public string Carton { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "PCTN")]
        public string PCTN { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }



        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }





        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVatPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Vat")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndVat { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Per.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisPer { get; set; }



        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndDisAmt { get; set; }

        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Individual Discount Amt.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IndPrice { get; set; }

        public int IndChkPer { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }


        public virtual SalesReturnMain SalesReturnMain { get; set; }

        public virtual Product vProductName { get; set; }
        public virtual Warehouse vWarehouse { get; set; }

        public virtual ProductSerial vProductSerial { get; set; }

        public virtual SalesType vSalesTypes { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }



        public int RowNo { get; set; }

        [NotMapped]
        public bool isChecked { get; set; }

    }



    public class SalesReturnTermsSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesReturnTermsSubId { get; set; }
        //[Key, Column(Order = 0)]
        public int SalesReturnId { get; set; }

        //[Key, Column(Order = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms")]
        public string Terms { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }




        public int RowNo { get; set; }

        //public string comid { get; set; }





        public virtual SalesReturnMain SalesReturnMain { get; set; }



        //public virtual ICollection<Product> vProducts { get; set; }




    }



    public class SalesReturnPaymentSub
    {

        [Key]
        public int SalesReturnPaymentSubId { get; set; }

        public int SalesReturnId { get; set; }


        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]

        [Display(Name = "Payment Type")]
        public int PaymentTypeId { get; set; }
        public string PaymentCardNo { get; set; }


        public Boolean isPosted { get; set; }

        public int? AccId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Amount { get; set; }



        public int? RowNo { get; set; }

        //public string comid { get; set; }


        [Required]
        [StringLength(128)]
        public string comid { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }



        public virtual SalesReturnMain SalesReturnMain { get; set; }

        public virtual PaymentType vPaymentType { get; set; }

        public virtual Acc_ChartOfAccount vChartofAccounts { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }

    public class Acc_VoucherNoCreatedType
    {
        [Key]
        //[Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherNoCreatedTypeId { get; set; }

        [Required]
        [Display(Name = "Voucher No Created Types Code")]
        public string VoucherNoCreatedTypeCode { get; set; }

        [Required]
        [Display(Name = "Voucher No Created Types Name")]
        public string VoucherNoCreatedTypeName { get; set; }

        //[Display(Name = "BusinessType Name")]

        //public virtual ICollection<Company> vBusinessTypesCompany { get; set; }
    }



    public partial class CartOrderMain : BaseModel
    {
        [Key]
        public int CartOrderId { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Reference")]
        public string CartOrderNo { get; set; }


        [Required]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Display(Name = "No. Of Items[Sum]")]
        public float ttlSumQty { get; set; }

        [Display(Name = "No. Of Items [Count]")]
        public float ttlCountQty { get; set; }
        [Display(Name = "Net Amount")]
        public float NetAmount { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Company Id")]
        //public string comid { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ICollection<CartOrderDetails> CartorderDetails { get; set; }

    }

    public class CartOrderDetails
    {
        [Key, Column(Order = 0)]
        public int CartOrderId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Product / Item")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public float Qty { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public float UnitPrice { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public float Amount { get; set; }

        public int RowNo { get; set; }

        [ForeignKey("CartOrderId")]
        public virtual CartOrderMain vCartOrderMain { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProductName { get; set; }

    }

    public class CostAllocation_Main: BaseModel
    {
        [Key]
        public int CostAlloMainId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Fical Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYear { get; set; }

        [DisplayName("Fical Month")]
        public int FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonth { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }



        public virtual List<CostAllocation_Details> CostAllocation_Detailses { get; set; }
        public virtual List<CostAllocation_Distribute> CostAllocation_Distributes { get; set; }

        //[StringLength(128)]
        //public string ComId { get; set; }

        //[StringLength(128)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }

        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

    }


    public class CostAllocation_Details
    {
        [Key]
        public int CostAlloSubId { get; set; }

        public int CostAlloMainId { get; set; }
        [ForeignKey("CostAlloMainId")]
        public virtual CostAllocation_Main CostAllocation_Main { get; set; }

        public int Details_AccId { get; set; }
        [ForeignKey("Details_AccId")]
        public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }

        [StringLength(50)]
        [Display(Name = "Caption")]
        public string DetailsTableCaption { get; set; }

        [Display(Name = "Percentage")]
        public float? DetailsTablePercentage { get; set; }



        [NotMapped]
        public bool IsDelete { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }

    }

    public class CostAllocation_Distribute
    {
        [Key]
        public int CostAlloDistributeId { get; set; }


        [StringLength(50)]
        public string Caption { get; set; }


        public float? Percentage { get; set; }

        public int CostAlloMainId { get; set; }
        [ForeignKey("CostAlloMainId")]
        public virtual CostAllocation_Main CostAllocation_Main { get; set; }


        public int? CostAlloSubId { get; set; }
        [ForeignKey("CostAlloSubId")]
        public virtual CostAllocation_Details CostAllocation_Details { get; set; }


        public int? Details_AccId { get; set; }


        public int? Distribute_AccId { get; set; }
        [ForeignKey("Distribute_AccId")]
        public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }



    }

    public class Acc_BudgetRelease : BaseModel
    {
        [Key]
        public int BudgetReleaseId { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYear { get; set; }

        [Display(Name = "Fiscal Month")]
        public int FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonth { get; set; }

        [Display(Name = "Chart of Account")]
        public int AccId { get; set; }
        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }

        [Display(Name = "Balance")]
        public double Balance { get; set; }
        [Display(Name = "Debit Amount")]
        public double DebitAmount { get; set; }
        [Display(Name = "Credit Amount")]
        public double CreditAmount { get; set; }

        [Display(Name = "Issue To Emp.")]
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        //[StringLength(128)]
        //public string ComId { get; set; }
        //[StringLength(128)]
        //public string UserId { get; set; }
        //[StringLength(128)]
        //public string UserIdUpdated { get; set; }


        //public DateTime? DateAdded { get; set; }

        //public DateTime? DateUpdated { get; set; }



    }

    public class Bill_Main : BaseModel
    {
        [Key]
        public int BillMainId { get; set; }

        [Display(Name = "Bill No")]
        [StringLength(50)]
        public string BillNo { get; set; }

        [Display(Name = "Date")]
        public DateTime BillDate { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        [Display(Name = "Supplier Name")]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Display(Name = "Bank Acc.")]
        public int? AccId { get; set; }
        [ForeignKey("AccId")]
        public Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }


        [Display(Name = "Total PO Qty")]
        public double TotalPOQty { get; set; }
        [Display(Name = "Total Rec. Qty")]
        public double TotalReceiveQty { get; set; }
        [Display(Name = "Gross Amt.")]
        public double GrossAmount { get; set; }
        [Display(Name = "Total SD Amt.")]
        public double TotalSDAmount { get; set; }
        [Display(Name = "Total VAT Amt.")]
        public double TotalVATAmount { get; set; }
        [Display(Name = "Total AIT Amt.")]
        public double TotalAITAmount { get; set; }

        [Display(Name = "Advance")]
        public double Advance { get; set; }

        [Display(Name = "Total WF Amt.")]
        public double TotalWFAmount { get; set; }
        [Display(Name = "Total LD Amt.")]
        public double TotalLDAmount { get; set; }
        [Display(Name = "Total Conf. Amt.")]
        public double TotalConfessionAmount { get; set; }
        [Display(Name = "Total Elect. Amt.")]
        public double TotalElectricityAmount { get; set; }
        [Display(Name = "Total Return Amt.")]
        public double TotalReturnAmount { get; set; }

        [Display(Name = "Net Payable Bill")]
        public double NetPayableBill { get; set; }

        [Display(Name = "Total Others Amt.")]
        public double TotalOthersAmount { get; set; }
        [Display(Name = "Total Forfeiture Amt.")]
        public double TotalForfeitureAmount { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        public virtual List<Bill_Sub> Bill_Subs { get; set; }

        //[StringLength(128)]
        //public string ComId { get; set; }

        //[StringLength(128)]
        //public string UserId { get; set; }

        //[StringLength(128)]
        //public string UserIdUpdated { get; set; }

        //public DateTime? DateAdded { get; set; }

        //public DateTime? DateUpdated { get; set; }
    }

    public class Bill_Sub
    {
        [Key]
        public int BillSubId { get; set; }

        [Display(Name = "Bill Main")]
        public int BillMainId { get; set; }
        [ForeignKey("BillMainId")]
        public Bill_Main Bill_Main { get; set; }

        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Display(Name = "Product Name")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Display(Name = "PO Qty")]
        public double POQty { get; set; }

        [Display(Name = "Receive Qty")]
        public double ReceiveQty { get; set; }

        [Display(Name = "Rate")]
        public double Rate { get; set; }

        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Display(Name = "SD %")]
        public float SDPercentage { get; set; }
        [Display(Name = "SD Amt.")]
        public double SDAmount { get; set; }

        [Display(Name = "VAT %")]
        public float VATPercentage { get; set; }
        [Display(Name = "VAT Amt.")]
        public double VATAmount { get; set; }

        [Display(Name = "AIT %")]
        public float AITPercentage { get; set; }
        [Display(Name = "AIT Amt.")]
        public double AITAmount { get; set; }

        [Display(Name = "Walfare %")]
        public float WFPercentage { get; set; }
        [Display(Name = "Walfare Amt.")]
        public double WFAmount { get; set; }

        [Display(Name = "LD %")]
        public float LDPercentage { get; set; }
        [Display(Name = "LD Amt.")]
        public double LDAmount { get; set; }

        [Display(Name = "Confession %")]
        public float ConfessionPercentage { get; set; }
        [Display(Name = "Confession Amt.")]
        public double ConfessionAmount { get; set; }

        [Display(Name = "Elect. Amt")]
        public double ElectricityAmount { get; set; }
        [Display(Name = "Return Amt.")]
        public double ReturnAmount { get; set; }

        [Display(Name = "Forfeiture Amt.")]
        public double ForfeitureAmount { get; set; }

        [Display(Name = "Others Amt.")]
        public double OthersAmount { get; set; }


        [Display(Name = "Net Amt.")]
        public double NetAmount { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    public class BillEmp_Main
    {
        [Key]
        public int BillEmpMainId { get; set; }

        [Display(Name = "Bill No")]
        [StringLength(50)]
        public string BillNo { get; set; }

        [Display(Name = "Date")]
        public DateTime BillDate { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        [Display(Name = "Supplier Name")]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Display(Name = "Bank Acc.")]
        public int? AccId { get; set; }
        [ForeignKey("AccId")]
        public Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }


        [Display(Name = "Total PO Qty")]
        public double TotalPOQty { get; set; }
        [Display(Name = "Total Rec. Qty")]
        public double TotalReceiveQty { get; set; }
        [Display(Name = "Gross Amt.")]
        public double GrossAmount { get; set; }
        [Display(Name = "Total SD Amt.")]
        public double TotalSDAmount { get; set; }
        [Display(Name = "Total VAT Amt.")]
        public double TotalVATAmount { get; set; }
        [Display(Name = "Total AIT Amt.")]
        public double TotalAITAmount { get; set; }

        [Display(Name = "Net Payable Bill")]
        public double NetPayableBill { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        public virtual List<BillEmp_Sub> BillEmp_Subs { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        public string UserIdUpdated { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    public class BillEmp_Sub
    {
        [Key]
        public int BillEmpSubId { get; set; }

        [Display(Name = "Bill Main")]
        public int BillEmpMainId { get; set; }
        [ForeignKey("BillMainId")]
        public BillEmp_Main BillEmp_Main { get; set; }

        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Display(Name = "Product Name")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Display(Name = "PO Qty")]
        public double POQty { get; set; }

        [Display(Name = "Receive Qty")]
        public double ReceiveQty { get; set; }

        [Display(Name = "Rate")]
        public double Rate { get; set; }

        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Display(Name = "SD %")]
        public float SDPercentage { get; set; }
        [Display(Name = "SD Amt.")]
        public double SDAmount { get; set; }

        [Display(Name = "VAT %")]
        public float VATPercentage { get; set; }
        [Display(Name = "VAT Amt.")]
        public double VATAmount { get; set; }

        [Display(Name = "AIT %")]
        public float AITPercentage { get; set; }
        [Display(Name = "AIT Amt.")]
        public double AITAmount { get; set; }

        [Display(Name = "Walfare %")]
        public float WFPercentage { get; set; }
        [Display(Name = "Walfare Amt.")]
        public double WFAmount { get; set; }

        [Display(Name = "LD %")]
        public float LDPercentage { get; set; }
        [Display(Name = "LD Amt.")]
        public double LDAmount { get; set; }

        [Display(Name = "Confession %")]
        public float ConfessionPercentage { get; set; }
        [Display(Name = "Confession Amt.")]
        public double ConfessionAmount { get; set; }

        [Display(Name = "Elect. Amt")]
        public double ElectricityAmount { get; set; }
        [Display(Name = "Return Amt.")]
        public double ReturnAmount { get; set; }


        [Display(Name = "Net Amt.")]
        public double NetAmount { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        [StringLength(400)]
        public string Remarks { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }
    }





}
