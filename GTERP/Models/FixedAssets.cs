using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{



    public class Items
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Display(Name = "Item  Code")]
        public string ItemCode { get; set; }

        [Display(Name = "HSCode")]
        public string ItemHSCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Column(TypeName = "VARCHAR(MAX)")]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Item Short Name")]
        public string ItemShortName { get; set; }

        [Display(Name = "Default Unit of Measure")]
        public int DOM { get; set; }

        public bool Disabled { get; set; }
        public bool AllowAlternativeItem { get; set; }
        public bool MaintainStock { get; set; }
        public bool IncludeIteminManufacturing { get; set; }
        public bool ValuationRate { get; set; }
        public bool StandardSellingRate { get; set; }
        public bool IsFixedAsset { get; set; }
        public bool AutoCreateAssetsonPurchase { get; set; }
        public bool AllowancePercentage { get; set; }
        public bool UploadImage { get; set; }
        public bool Brand { get; set; }
        public bool Description { get; set; }

        public Nullable<int> ItemGroupId { get; set; }
        public virtual ItemGroup ItemGroups { get; set; }

        [StringLength(128)]
        public string userid { get; set; }

        public Boolean isDelete { get; set; }
        [StringLength(128)]

        public string comid { get; set; }

    }

    public class Asset : BaseModel
    {
        [Key]
        public int AssetId { get; set; }

        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }
        public string SerialNumber { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("ProductCategory")]
        [Display(Name = "Asset Category")]
        public long ProductCategoryId { get; set; }

        //public string ComId { get; set; }
        public string Description { get; set; }
        [Display(Name = "Assigned To")]
        public int? AssignedTo { get; set; }

        [Display(Name = "Department")]
        public int? DeptId { get; set; }
        public string AssignComponent { get; set; }

        [Display(Name = "Current State of Asset")]
        public int? CurrentStateId { get; set; }
        public virtual AssetCurrentState AssetCurrentState { get; set; }
        public float UsefulLife { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Display(Name = "Item")]
        public int? ProductId { get; set; }

        //[ForeignKey("Depreciation")]
        //[Display(Name = "Depteciation Type")]
        //public int DepriciationId { get; set; }

        [Display(Name = "Vendor Name")]
        public int? VendorId { get; set; }



        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Warrenty Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [ForeignKey("PurchaseType")]
        [Display(Name = "Purchase Type")]
        public int PurchaseTypeId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Product Product { get; set; }
        public virtual Company Company { get; set; }
        public virtual Cat_Location Location { get; set; }
        public virtual PurchaseType PurchaseType { get; set; }
        public virtual Depreciation Depreciation { get; set; }
        public virtual Supplier Vendor { get; set; }
    }

    public class AssetCurrentState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrentStateId { get; set; }
        public string CurrentState { get; set; }
        public string Description { get; set; }
    }
    public class ProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductTypeId { get; set; }
        public string TypeName { get; set; }
        public string ComId { get; set; }


    }
    public class PurchaseType
    {
        public int PurchaseTypeId { get; set; }
        public string TypeName { get; set; }
        public string ComId { get; set; }

    }

    //public class Asset
    //{
    //    [Key]
    //    public int AssetId { get; set; }

    //    [Display(Name = "Asset Name")]
    //    public string AssetName { get; set; }
    //    public Items Items { get; set; }
    //    public AdditionalOptions AdditionalOptions { get; set; }

    //    [ForeignKey("AssetCategory")]
    //    [Display(Name = "Asset Category")]
    //    public int AssetCategoryId { get; set; }

    //    [ForeignKey("Company")]
    //    public string ComId { get; set; }

    //    [ForeignKey("Location")]
    //    [Display(Name = "Location")]
    //    public int LocationId { get; set; }

    //    [Display(Name = "Purchase Date")]
    //    public DateTime PurchaseDate { get; set; }

    //    public virtual AssetCategory AssetCategory { get; set; }
    //    public virtual Company Company { get; set; }
    //    public virtual Cat_Location Location { get; set; }
    //}

    public class AdditionalOptions
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }


        [Display(Name = "Custodian")]
        public string Custodian { get; set; }


        [Display(Name = "Department of Custodian")]
        public int DeptId { get; set; }
        public virtual Cat_Department Department { get; set; }


        [Display(Name = "Is Existing Asset")]
        public bool IsExistingAsset { get; set; }


        [Display(Name = "Opening Accumulated Depreciation")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OADepreciation { get; set; }


        [Display(Name = "Number of Depreciations Booked")]
        public int NoDBooked { get; set; }


        [Display(Name = "Current Value(After Depreciation)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CVADepreciation { get; set; }


        [Display(Name = "Next Depreciation Date")]
        public DateTime NDDate { get; set; }


        [Display(Name = "Calculate Depreciation")]
        public bool CalculateDepreciation { get; set; }


        [Display(Name = "Allow Monthly Depreciation")]
        public bool AllowMonthlyDepreciation { get; set; }

    }

    public class AssetCategory
    {
        [Key]
        public int AssetCategoryId { get; set; }
        public string CatName { get; set; }

        public virtual ICollection<FinanceBook> FinanceBooks { get; set; }
        public virtual ICollection<AccountDetails> AccountDetails { get; set; }
    }

    public class FinanceBook
    {
        [Key]
        public int FinanceBookId { get; set; }

        [Display(Name = "Depreciation Method")]
        [ForeignKey("DepreciationMethod")]
        public int DepreciationMethodId { get; set; }

        [Display(Name = "Frequency of Depreciation (Months)")]
        public string FoDepreciation { get; set; }

        [Display(Name = "Total Number of Depreciations")]
        public string DTotal { get; set; }
        [Display(Name = "Rate of Depreciation")]
        public string DRate { get; set; }
        public virtual DepreciationMethod DepreciationMethod { get; set; }


    }

    public class Depreciation
    {
        [Key]
        public int DepreciationId { get; set; }

        [ForeignKey("FA_Details")]
        [Display(Name = "Sub Asset")]
        public int FA_DetailsId { get; set; }
        public virtual FA_Details FA_Details { get; set; }


        [ForeignKey("FA_Master")]
        [Display(Name = "Fixed Asset No")]
        public int FA_MasterId { get; set; }
        public virtual FA_Master FA_Master { get; set; }

        [ForeignKey("DepreciationFrequency")]

        [Display(Name = "Frequency of Depreciation")]
        public int FoDId { get; set; }
        public virtual DepreciationFrequency DepreciationFrequency { get; set; }


        [Display(Name = "Total Number of Depreciations")]
        public int TNoD { get; set; }

        [ForeignKey("DepreciationMethod")]
        [Display(Name = "Depreciation Method")]
        public int DMId { get; set; }
        public virtual DepreciationMethod DepreciationMethod { get; set; }

        [Display(Name = "Depreciation Start Date")]
        public DateTime DSDate { get; set; }

        [Display(Name = "Expected Value After Useful Life")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EVAULife { get; set; }

        [Display(Name = "Rate of Depreciation")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DRate { get; set; }
        public string ComId { get; set; }
    }
    public class DepreciationFrequency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoDId { get; set; }
        public string Title { get; set; }
        public string CompoundingPeriod { get; set; }
        public string FractionOfOneYear { get; set; }
        public decimal CentinalValue { get; set; }
        public int DSAMonth { get; set; }
        public int DSADay { get; set; }
        public string ComId { get; set; }
    }
    public class DepreciationMethod
    {
        [Key]
        public int DMId { get; set; }
        public string DMName { get; set; }
    }

    public class AccountDetails
    {
        [Key]
        public int AccountDetailsId { get; set; }
        public int? ComId { get; set; }
        public int? FixedAssetAccountId { get; set; }
        public int? AccumulatedDepreciationAccountId { get; set; }
        public int? DepreciationExpenseAccountId { get; set; }
        public bool CapitalWorkInProgressAccount { get; set; }

    }

    public class DepreciationScheduleSales
    {
        [Key]
        public int DepreciationScheduleSaleId { get; set; }


        [ForeignKey("FA_SellId")]
        [Display(Name = "Asset Item")]
        public int FA_SellId { get; set; }
        public virtual FA_Sell FA_Sell { get; set; }

        [NotMapped]
        public string AssetItem { get; set; }
        public DateTime ScheduleDate { get; set; }
        public decimal DepAmount { get; set; }
        public decimal AccumulateDepAmount { get; set; }
        public int AccumulateDepBooked { get; set; }
        public int AccumulateDepRemain { get; set; }
        public bool JournalEntry { get; set; }
        [StringLength(100)]
        public string VoucherNo { get; set; }
        public decimal? Rate { get; set; }


    }
    public class DepreciationSchedule
    {
        [Key]
        public int DepreciationScheduleId { get; set; }
        [ForeignKey("FA_Details")]
        [Display(Name = "Asset Item")]
        public int FA_DetailsId { get; set; }
        [NotMapped]
        public string AssetItem { get; set; }
        public DateTime ScheduleDate { get; set; }
        public decimal DepAmount { get; set; }
        public decimal AccumulateDepAmount { get; set; }
        public int AccumulateDepBooked { get; set; }
        public int AccumulateDepRemain { get; set; }
        public bool JournalEntry { get; set; }

        [StringLength(100)]
        public string VoucherNo { get; set; }
        public decimal? Rate { get; set; }
        public virtual FA_Details FA_Details { get; set; }

    }

    public class TemDepSchedule
    {
        [Key]
        public int DepreciationScheduleId { get; set; }

        public int FA_DetailsId { get; set; }
        [NotMapped]
        public string AssetItem { get; set; }
        public DateTime ScheduleDate { get; set; }
        public decimal DepAmount { get; set; }
        public decimal AccumulateDepAmount { get; set; }
        public int AccumulateDepBooked { get; set; }
        public int AccumulateDepRemain { get; set; }
        public bool JournalEntry { get; set; }

    }

    public class TemDepScheduleSale
    {
        [Key]
        public int DepreciationScheduleSaleId { get; set; }

        public int FA_SellId { get; set; }
        [NotMapped]
        public string AssetItem { get; set; }
        public DateTime ScheduleDate { get; set; }
        public decimal DepAmount { get; set; }
        public decimal AccumulateDepAmount { get; set; }
        public int AccumulateDepBooked { get; set; }
        public int AccumulateDepRemain { get; set; }
        public bool JournalEntry { get; set; }

    }


}
