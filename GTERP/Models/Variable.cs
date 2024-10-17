using GTERP.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class StyleStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StyleStatusId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string StyleStatusName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string StyleStatusShortName { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }



    }



    public class ShipMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipModeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "ShipMode")]
        public string ShipModeName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "ShipMode")]
        public string ShipModeShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }



    }


    public class ExportOrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExportOrderStatusId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ExportOrderStatusName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ExportOrderStatusShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }



    }


    public class ExportOrderCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExportOrderCategoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ExportOrderCategoryName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ExportOrderCategoryShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }



    }

    public class LCType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LCTypeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string LCTypeName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string LCTypeShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }



    }


    public class TradeTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TradeTermId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Trade Term")]
        public string TradeTermName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Trade Term")]
        public string TradeTermShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public class BankAccountNo : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankAccountId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Bank Account No")]
        public string BankAccountNumber { get; set; }

        public Nullable<int> CommercialCompanyId { get; set; }
        public virtual SisterConcernCompany CommercialCompanys { get; set; }

        public Nullable<int> OpeningBankId { get; set; }
        public virtual OpeningBank OpeningBanks { get; set; }

        //[StringLength(128)]
        //public string comid { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }

        //[StringLength(128)]
        //public string useridupdate { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public class BankAccountNoLienBank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LienBankAccountId { get; set; }


        //[Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR(MAX)")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Bank Account No")]
        public string BankAccountNumber { get; set; }
        public string SwiftCodeBankAccountNoLienBank { get; set; }

        public Nullable<int> CommercialCompanyId { get; set; }
        public virtual SisterConcernCompany CommercialCompanys { get; set; }

        public Nullable<int> LienBankId { get; set; }
        public virtual LienBank LienBanks { get; set; }


        public Nullable<int> comid { get; set; }


        public Nullable<int> CountryId { get; set; }
        public virtual Country Countrys { get; set; }




        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformations { get; set; }


        public Nullable<int> BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual BuyerInformation BuyerInformations { get; set; }
        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }

        [StringLength(128)]
        public string userid { get; set; }


        [StringLength(128)]
        public string useridupdate { get; set; }
    }


    public class LCNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LCNatureId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Nature")]
        public string LCNatureName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Nature")]
        public string LCNatureShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public class PINature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PINatureId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "PI Nature")]
        public string PINatureName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Nature")]
        public string PINatureShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public class LCStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LCStatusId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string LCStatusName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string LCStatusShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }
    public class Representative
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RepresentativeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Representative Name")]
        public string RepresentativeName { get; set; }


        [Display(Name = "Representative Address")]

        public string RepresentativeAddress { get; set; }

        [Display(Name = "Mobile No")]

        public string RepresentativeMobile { get; set; }






        [StringLength(128)]
        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        [StringLength(128)]
        public string useridUpdate { get; set; }
    }

    public class PaymentTerms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentTermsId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Payment Terms")]
        public string PaymentTermsName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Payment Terms")]
        public string PaymentTermsShortName { get; set; }

        public int Days { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }
    public class DayList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DayListId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Days")]
        public string DayListName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Days")]
        public string DayListShortName { get; set; }
        public string DayListGroup { get; set; }


        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public class DocumentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentStatusId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string DocumentStatusName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string DocumentStatusShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }
    public class WorkorderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkorderStatusId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string WorkorderStatusName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string WorkorderStatusShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public class ApprovedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovedById { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ApprovedByName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ApprovedByShortName { get; set; }

        public Nullable<int> comid { get; set; }
        public Nullable<int> userid { get; set; }


        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }


    public class BuyerGroup : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuyerGroupId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        //[Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR(MAX)")]
        //[StringLength(200)]
        [Display(Name = "Buyer Group")]
        public string BuyerGroupName { get; set; }

        [Display(Name = "Buyer Group Code")]
        public string BuyerGroupCode { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Buyer Group Short Name")]
        public string BuyerGroupShortName { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }

        //public Boolean isDelete { get; set; }

        //public string comid { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }

    }

    public class ItemGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        //[Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR(MAX)")]
        //[StringLength(200)]
        [Display(Name = "Item Group")]
        public string ItemGroupName { get; set; }

        [Display(Name = "Item Group Code")]
        public string ItemGroupCode { get; set; }

        [Display(Name = "HSCode")]
        public string ItemGroupHSCode { get; set; }

        [Display(Name = "Item Margin [Bill Discounting]")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> ItemMargin { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Item Group Short Name")]
        public string ItemGroupShortName { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        public Boolean isDelete { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }

    }


    public class VoucherTranGroup:BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherTranGroupId { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Transaction Group Name")]
        public string VoucherTranGroupName { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }

        //public Boolean isDelete { get; set; }
        //[StringLength(128)]

        //public string comid { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }

    }


    public class Acc_BankStatementBalance : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankStatementBalanceId { get; set; }

        [Display(Name = "Bank Account Head")]
        public int AccId { get; set; }

        [ForeignKey("AccId")]
        [Display(Name = "Bank Account Head")]

        public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }

        [Display(Name = "Cash Book Statement")]
        public double CashBookStatementAmount { get; set; }

        [Display(Name = "Bank Book Statement")]
        public double BankStatementAmount { get; set; }

        [Display(Name = "Add Amount")]

        public double AddAmount { get; set; }

        [Display(Name = "Less Amount")]


        public double LessAmount { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }


        //[StringLength(128)]
        //public string useridUpdate { get; set; }

        //public Boolean isDelete { get; set; }
        //[StringLength(128)]

        //public string comid { get; set; }

        [Display(Name = "Balance Date")]

        public DateTime BalanceDate { get; set; }
        public string Remarks { get; set; }


        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }

    }



    public class ItemDesc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemDescId { get; set; }

        [Display(Name = "Item Desc Code")]
        public string ItemDescCode { get; set; }

        [Display(Name = "HSCode")]
        public string ItemDescHSCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        //[Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR(MAX)")]
        //[StringLength(200)]
        [Display(Name = "Item Desc")]
        public string ItemDescName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Item Desc. Short Name")]
        public string ItemDescShortName { get; set; }

        public Nullable<int> ItemGroupId { get; set; }
        public virtual ItemGroup ItemGroups { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        public Boolean isDelete { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }

    }

    public class ApprovedByHimu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovedByHimuId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string ApprovedByHimuName { get; set; }


        public Nullable<int> comid { get; set; }
        public Nullable<int> userid { get; set; }


        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }



    public class CommercialLCType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommercialLCTypeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string CommercialLCTypeName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string CommercialLCTypeShortName { get; set; }

        public Nullable<int> comid { get; set; }

        //public string ComId { get; set; }
        //public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }



}