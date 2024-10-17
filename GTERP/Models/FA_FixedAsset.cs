using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class FA_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_MasterId { get; set; }


        [ForeignKey("ProductId")]
        [Display(Name = "Item")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "Asset Code")]
        public string AssetCode { get; set; }


        [ForeignKey("DepreciationMethod")]
        [Display(Name = "Dep. Method")]
        public int DMId { get; set; }

        [Display(Name = "Dep. Per.")]
        public decimal Percentage { get; set; }

        //[Display(Name = "Salvage Value Parcentage")]
        //public decimal SalvageParcentage { get; set; }

        [ForeignKey("DepreciationHead")]
        [Display(Name = "Dep. Acc.")]
        public int? AccId_DepreciationExpense { get; set; }

        [ForeignKey("AccumulatedDepreciationHead")]

        [Display(Name = "Accu. Dep. Acc.")]
        public int? AccId_AccumulatedDepreciation { get; set; }


        [ForeignKey("DepreciationFrequency")]
        [Display(Name = "Frequency of Dep.")]
        public int FOD { get; set; }

        public string ComId { get; set; }
        public bool IsInProcess { get; set; }

        /// <summary>
        ///  himu add model
        /// </summary>

        [Display(Name = "Identification Code")]
        [StringLength(100)]
        public string IdentificationCode { get; set; }

        [Display(Name = "Location")]
        [StringLength(100)]
        public string Location { get; set; }

        [Display(Name = "Cost Center")]
        [StringLength(100)]
        public string CostCenter { get; set; }

        [Display(Name = "Mark")]
        [StringLength(100)]
        public string Mark { get; set; }

        [Display(Name = "Foreign Exchange Cost")]
        [StringLength(100)]
        public string ForeignExchangeCost { get; set; }

        [Display(Name = "County Of Origin")]
        [StringLength(100)]
        public string CountyOfOrigin { get; set; }

        [Display(Name = "Maker")]
        [StringLength(200)]
        public string Maker { get; set; }

        [Display(Name = "Item No")]
        [StringLength(500)]
        public string ItemNo { get; set; }

        [Display(Name = "Name Of Supplier")]
        [StringLength(200)]
        public string SupplierName { get; set; }

        [Display(Name = "P.O No & Date")]
        [StringLength(200)]
        public string PONoAndDate { get; set; }

        [Display(Name = "Date of Delivery")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Date of Inst.")]
        public DateTime? InstallationDate { get; set; }

        [Display(Name = "Name of Erection Cont.")]
        [StringLength(100)]
        public string ErectionContractor { get; set; }

        [Display(Name = "FOLIO")]
        [StringLength(100)]
        public string Folio { get; set; }

        [Display(Name = "Control Code")]
        [StringLength(100)]
        public string ControlCodeNo { get; set; }

        [Display(Name = "Sub Code")]
        [StringLength(100)]
        public string SubCodeNo { get; set; }

        [Display(Name = "Depr. Rate (Acc)")]
        [StringLength(5)]
        public string DepreciationRate { get; set; }

        [Display(Name = "Anticipated Scrap")]
        [StringLength(20)]
        public string AnticipatedScrapValue { get; set; }

        [Display(Name = "Disposal Date")]
        public DateTime? DisposalDate { get; set; }


        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string CreatedByUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }
        public virtual DepreciationMethod DepreciationMethod { get; set; }

        public virtual DepreciationFrequency DepreciationFrequency { get; set; }
        public virtual Acc_ChartOfAccount DepreciationHead { get; set; }
        public virtual Acc_ChartOfAccount AccumulatedDepreciationHead { get; set; }

        public virtual ICollection<FA_Details> FA_Details { get; set; }




    }
    public class FA_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_DetailsId { get; set; }


        [ForeignKey("FA_Master")]
        [Display(Name = "Fixed Asset Code")]
        public int FA_MasterId { get; set; }

        [Display(Name = "Sub Asset Code")]
        public string AssetItem { get; set; }


        [Display(Name = "Issue No")]
        public string IssuNo { get; set; }

        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Purchase Value")]
        public decimal PurchaseValue { get; set; }

        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }

        public DateTime DepCalFromDate { get; set; }

        [ForeignKey("Cat_Section")]
        [Display(Name = "Assign To Sect.")]
        public int? AssignToSection { get; set; }
        [ForeignKey("Cat_Department")]
        [Display(Name = "Assign To Dept.")]
        public int? AssignToDept { get; set; }

        [Display(Name = "Life Span [Year]")]
        public int UsefullLife { get; set; }

        [Display(Name = "Rem. Year [Round]")]
        public int RemainingYear { get; set; }

        [Display(Name = "Rem. Month")]
        public int RemainingMonth { get; set; }


        [ForeignKey("FA_Dep_Status")]
        [Display(Name = "Current Status")]
        public int FA_Dep_StatusId { get; set; }
        public virtual FA_Dep_Status FA_Dep_Status { get; set; }

        public bool IsInActive { get; set; }


        [Display(Name = "Salvage Value")]
        public decimal EVAULife { get; set; }///Expected / Estimated Value After userful life.

        //[StringLength(1200)]
        public string Description { get; set; }


        public decimal? Qty { get; set; }

        [StringLength(100)]
        [Display(Name = "MRR No")]
        public string MRRNo { get; set; }


        [Display(Name = "MRR Date")]
        public DateTime? MRRDate { get; set; }


        [StringLength(100)]
        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }


        [Display(Name = "Voucher Date")]
        public DateTime? VoucherDate { get; set; }

        public decimal? Rate { get; set; }




        public string ComId { get; set; }
        [Display(Name = "Dep. Status")]

        public bool IsDepRunning { get; set; }
        [NotMapped]
        public bool IsDelete { get; set; }
        [NotMapped]
        public bool CalculateDepreciation { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string CreatedByUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }


        public virtual FA_Master FA_Master { get; set; }
        public virtual HR_Emp_Info Emp_Info { get; set; }
        public virtual Cat_Section Cat_Section { get; set; }
        public virtual Cat_Department Cat_Department { get; set; }
        public virtual ICollection<DepreciationSchedule> DepreciationSchedules { get; set; }
        [Display(Name = "Accu. Dep. Value")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccumulatedDepreciatedValue { get; set; }
        [Display(Name = "Writtendown Value")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal WrittenDownValue { get; set; }


    }

    public class FA_Dep_Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_Dep_StatusId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class Tem_FA_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_DetailsId { get; set; }


        [Display(Name = "Fixed Asset Code")]
        public int FA_MasterId { get; set; }

        [Display(Name = "Sub Asset Code")]
        public string AssetItem { get; set; }


        [Display(Name = "Issue No")]
        public string IssuNo { get; set; }

        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Purchase Value")]
        public decimal PurchaseValue { get; set; }

        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }

        public DateTime DepCalFromDate { get; set; }

        [Display(Name = "Assign To Section")]
        public int? AssignToSection { get; set; }
        [Display(Name = "Assign To Dept.")]
        public int? AssignToDept { get; set; }

        [Display(Name = "Life Span")]
        public int UsefullLife { get; set; }

        [Display(Name = "Rem. Year")]
        public int RemainingYear { get; set; }

        [Display(Name = "Rem. Month")]
        public int RemainingMonth { get; set; }

        [Display(Name = "Current Status")]
        public int FA_Dep_StatusId { get; set; }
        public bool IsInActive { get; set; }


        [Display(Name = "Salvage Value")]
        public decimal EVAULife { get; set; }///Expected / Estimated Value After userful life.
        public string ComId { get; set; }
        [Display(Name = "Dep. Status")]

        public bool IsDepRunning { get; set; }
        [NotMapped]
        public bool IsDelete { get; set; }
        [NotMapped]
        public bool CalculateDepreciation { get; set; }
        [Display(Name = "Accu. Dep. Value")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccumulatedDepreciatedValue { get; set; }
        [Display(Name = "Writtendown Value")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal WrittenDownValue { get; set; }

    }

    public class FA_Sell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_SellId { get; set; }


        [ForeignKey("FA_Details")]
        [Display(Name = "Sub Asset")]
        public int FA_DetailsId { get; set; }

        [ForeignKey("FA_Master")]
        [Display(Name = "Fixed Asset No")]
        public int FA_MasterId { get; set; }

        [ForeignKey("Sells Price")]
        public decimal SellsPrice { get; set; }

        [Display(Name = "Cost Price")]
        public decimal? CostPrice { get; set; }

        public DateTime DepCalFromDate { get; set; }


        public decimal? Qty { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        [Display(Name = "MRR No")]
        public string MRRNo { get; set; }


        [Display(Name = "MRR Date")]
        public DateTime? MRRDate { get; set; }


        [StringLength(100)]
        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }


        [Display(Name = "Voucher Date")]
        public DateTime? VoucherDate { get; set; }

        public decimal? Rate { get; set; }


        [Display(Name = "Sales Date")]
        public DateTime SalesDate { get; set; }

        public bool IsInActive { get; set; }

        public bool IsDepRunning { get; set; }

        [NotMapped]
        public bool CalculateDepreciation { get; set; }



        [NotMapped]
        public bool IsDelete { get; set; }



        [Display(Name = "Current Status")]
        public int FA_Dep_StatusId { get; set; }
        [ForeignKey("FA_Dep_StatusId")]
        public virtual FA_Dep_Status FA_Dep_Status { get; set; }

        public virtual FA_Details FA_Details { get; set; }
        public virtual FA_Master FA_Master { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string CreatedByUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }

    }
    public class Tem_FA_Sell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_SellId { get; set; }


        [ForeignKey("FA_Details")]
        [Display(Name = "Sub Asset")]
        public int FA_DetailsId { get; set; }

        [ForeignKey("FA_Master")]
        [Display(Name = "Fixed Asset No")]
        public int FA_MasterId { get; set; }

        [ForeignKey("Sells Price")]
        public decimal SellsPrice { get; set; }

        [Display(Name = "Cost Price")]
        public decimal? CostPrice { get; set; }


        public decimal? Qty { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        [Display(Name = "MRR No")]
        public string MRRNo { get; set; }


        [Display(Name = "MRR Date")]
        public DateTime? MRRDate { get; set; }


        [StringLength(100)]
        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }


        [Display(Name = "Voucher Date")]
        public DateTime? VoucherDate { get; set; }

        public decimal? Rate { get; set; }


        [Display(Name = "Sales Date")]
        public DateTime SalesDate { get; set; }

        public DateTime DepCalFromDate { get; set; }

        public bool IsInActive { get; set; }

        public bool IsDepRunning { get; set; }

        [NotMapped]
        public bool CalculateDepreciation { get; set; }



        [NotMapped]
        public bool IsDelete { get; set; }



        [Display(Name = "Current Status")]
        public int FA_Dep_StatusId { get; set; }
    }

    public class DepViewModel
    {
        public int FA_DetailsId { get; set; }

        public string AssetItem { get; set; }


        public int FA_MasterId { get; set; }
        public string ProductName { get; set; }


        public int FoDId { get; set; }
        public string Title { get; set; }


        public int TNoD { get; set; }

        public int DMId { get; set; }
        public string DMName { get; set; }

        public DateTime DSDate { get; set; }


        public decimal EVAULife { get; set; }

        public decimal DepRate { get; set; }
    }

    public class FA_Calculation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FA_CalculationId { get; set; }

        [ForeignKey("FA_Details")]
        [Display(Name = "Sub Asset")]
        public int FA_DetailsId { get; set; }


        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }


        [ForeignKey("FA_Master")]
        [Display(Name = "Fixed Asset No")]
        public int FA_MasterId { get; set; }


        public string Year { get; set; }
        public string Month { get; set; }
        public decimal CalculatedValue { get; set; }
        [NotMapped]
        public bool IsDelete { get; set; }
        public virtual FA_Details FA_Details { get; set; }
        public virtual FA_Master FA_Master { get; set; }


    }

    public class FA_ProcessRecord
    {
        [Key]
        public int ProcessRecordId { get; set; }
        [ForeignKey("FA_Details")]
        [Display(Name = "Asset Item")]
        public int FA_DetailsId { get; set; }

        [Display(Name = "Depreciation Start Date")]
        public DateTime DSDate { get; set; }

        [Display(Name = "Depreciation Entry Date")]
        public DateTime DEntryDate { get; set; }

        [Display(Name = "Next Depreciation Date")]
        public DateTime NextDepDate { get; set; }

        [Display(Name = "Depreciation Amount")]
        public decimal DepAmount { get; set; }
        public virtual FA_Details FA_Details { get; set; }

    }

}
