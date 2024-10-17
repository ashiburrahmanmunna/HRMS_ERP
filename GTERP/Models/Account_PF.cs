using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{

    public partial class PF_PFProcessViewModel
    {
        [Display(Name = "Currency ")]
        public int? CountryId { get; set; }
        public List<PF_FiscalYear> ProcessFYs { get; set; }
        public List<PF_FiscalMonth> ProcessMonths { get; set; }

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

    public partial class PF_FiscalYear : BaseModel
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


    public class PF_FiscalMonth
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





    public class PF_ChartOfAccount
    {

        //[Required]
        //[Key, Column(Order = 1)]


        //[Key, Column(Order = 2)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccId { get; set; }



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


        [ForeignKey("ParentChartOfAccount")]
        [Display(Name = "Base / Parent Head")]
        //public int? ParentAccId { get; set; }
        public int? ParentID { get; set; }
        [NotMapped]
        public int? PrevParentID { get; set; }


        //public int? ParentBucketGroupId { get; set; }

        public virtual PF_ChartOfAccount ParentChartOfAccount { get; set; }

        ///public virtual ICollection<PF_ChartOfAccount> ChartOfAccountChildren { get; set; }


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
        //public virtual PF_ChartOfAccount vAccountGroup { get; set; } = null;


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
        //public virtual ICollection<PF_VoucherSub> VoucherSubs { get; set; }


    }


    public partial class PF_VoucherMain
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public PF_VoucherMain()
        //{
        //    this.VoucherSubs = new HashSet<PF_VoucherSub>();
        //}
        [Key]
        public int VoucherId { get; set; }

        [NotMapped]
        public Nullable<int> AccId { get; set; }
        public int VoucherSerialId { get; set; }
        public int? YearlyVoucherTypeWiseSerial { get; set; }

        //public virtual PF_ChartOfAccount PF_ChartOfAccount { get; set; }

        [NotMapped]
        [Display(Name = "Last Voucher No")]
        public string LastVoucherNo { get; set; }

        [Display(Name = "Voucher No"),
         StringLength(20, ErrorMessage = "Max length 20 char.")]
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
        public virtual ICollection<PF_VoucherSub> VoucherSubs { get; set; }

        //public virtual PF_ChartOfAccount vChartofAccountsMain { get; set; }
        //public virtual Acc_VoucherType vVoucherType { get; set; }

        //public virtual PF_VoucherSub vVouchersubMain { get; set; }
        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual PF_FiscalYear PF_FiscalYear { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }

    }

    public partial class PF_VoucherSub
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PF_VoucherSub()
        {
            this.VoucherSubChecnoes = new HashSet<PF_VoucherSubCheckno>();
            this.VoucherSubSections = new HashSet<PF_VoucherSubSection>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int VoucherSubId { get; set; }

        public int VoucherId { get; set; }
        public int AccId { get; set; }
        [ForeignKey("AccId")]

        public virtual PF_ChartOfAccount PF_ChartOfAccount { get; set; }

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



        //public virtual PF_VoucherMain PF_VoucherMain { get; set; }
        //[InverseProperty("PF_VoucherSub")]
        [ForeignKey("VoucherId")]

        public virtual PF_VoucherMain PF_VoucherMain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<PF_VoucherSubCheckno> VoucherSubChecnoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PF_VoucherSubSection> VoucherSubSections { get; set; }

        //public virtual Country Country { get; set; }
        //public virtual Country CountryForeign { get; set; }
        //public int IsDel { get; set; }
        //public int RowNoDet { get; set; }
    }

    public class PF_VoucherSubSection
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

        public virtual PF_VoucherSub PF_VoucherSub { get; set; }
    }

    public class PF_VoucherSubCheckno
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
        public virtual PF_VoucherSub PF_VoucherSub { get; set; }



        [ForeignKey("AccId")]
        public virtual PF_ChartOfAccount vPF_ChartOfAccount { get; set; }




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


        public Boolean isManualEntry { get; set; }

        //public virtual ICollection<PF_VoucherSubCheckno_Clearing> PF_VoucherSubCheckno_Clearings { get; set; }
    }

    public partial class PFShowVoucherViewModel
    {
        [Display(Name = "From Date ")]
        public string dtFrom { get; set; }

        [Display(Name = "To Date ")]
        public string dtTo { get; set; }
        [Display(Name = "Currency ")]
        public int? CountryId { get; set; }
        public List<PF_FiscalYear> FiscalYs { get; set; }
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


}
