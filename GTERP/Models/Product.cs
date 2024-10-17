using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string ProductName { get; set; }


        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Name Bangla")]
        public string ProductNameBangla { get; set; }

        //[Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Material Code")]
        public string ProductCode { get; set; }



        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Location Details")]
        public string ProductBarcode { get; set; }


        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Brand")]
        public string ProductBrand { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Model")]
        public string ProductModel { get; set; }

        [Required]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }




        [Required]
        [Display(Name = "Currency")]

        public int CountryId { get; set; }


        //[Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Cost Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Sales Price")]
        public decimal SalePrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [DataType(DataType.Currency)]
        [Display(Name = "Retailer Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailerPrice { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "VAT Percentage")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal vatPercentage { get; set; }

        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "VAT Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal vatAmount { get; set; }


        [Display(Name = "Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ProductImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]

        [Display(Name = "Image [Folder]")]

        public string ImagePath { get; set; }

        [Display(Name = "Files Extension")]
        public string FileExtension { get; set; }


        [Required]
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



        //[Display(Name = "Sales Details")]
        //public virtual ICollection<SalesSub> vSalesSub { get; set; }

        [Display(Name = "Purchase Details")]
        public virtual ICollection<PurchaseSub> vPurchaseSub { get; set; }

        //[Display(Name = "Category Name")]
        public virtual Category vPrimaryCategory { get; set; }


        //[Required]
        [Display(Name = "Sub Category")]
        public Nullable<int> SubCategoryId { get; set; }
        public virtual SubCategory vSubCategory { get; set; }

        [Display(Name = "Product Main Group")]
        public Nullable<int> ProductMainGroupId { get; set; }
        [ForeignKey("ProductMainGroupId")]
        public virtual ProductMainGroup vProductMainGroup { get; set; }



        public virtual Unit vProductUnit { get; set; }


        [Display(Name = "Currency")]
        public virtual Country vProductCountry { get; set; }



        public virtual ICollection<Inventory> InventorySubs { get; set; }

        public virtual ICollection<CostCalculated> CostCalculated { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Reorder Level")]
        public float ReorderLevelOne { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Minimum Order Qty")]
        public float MinimumOrderQty { get; set; }




        [Display(Name = "Inventory Code :")]
        public Nullable<int> AccIdInventory { get; set; }
        [ForeignKey("AccIdInventory")]
        public virtual Acc_ChartOfAccount ChartOfAccountsInventory { get; set; }


        [Display(Name = "Consumption Code :")]
        public Nullable<int> AccIdConsumption { get; set; }
        [ForeignKey("AccIdConsumption")]
        public virtual Acc_ChartOfAccount ChartOfAccountsConsumption { get; set; }


    }

    public class SessionProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int CategoryId { get; set; }
        public string ComId { get; set; }
    }

    public class ProductPrie
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductPrieId { get; set; }

        public int ProductId { get; set; }


        [Display(Name = "Product Name")]
        public virtual Product Products { get; set; }

        public Nullable<System.DateTime> PriceDate { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Costing Price")]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal cPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Sales Price")]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal sPrice { get; set; }
        public int RowNo { get; set; }

        public int? SourceId { get; set; }
        public string Source { get; set; }



        [Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry By")]
        public string userid { get; set; }


        //[StringLength(128)]
        //[Display(Name = "Update By")]
        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }
    public class Inventory
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int WareHouseId { get; set; }



        [Display(Name = "Product Name")]
        public virtual Product Products { get; set; }


        //[Required]
        [Display(Name = "Warehouse Name")]
        public virtual Warehouse Warehouses { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Opening Stock")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpStock { get; set; }




        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Purchase Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Purchase Return Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurRetQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Purchase Exchange Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurExcQty { get; set; }


        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Sales Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Sales Return Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesRetQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Sales Exchange Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesExcQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Challan Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChallanQty { get; set; }



        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Issue Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IssueQty { get; set; } = 0;


        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Issue Return Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IssueRtnQty { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Goods Receive Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GoodsReceiveQty { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Goods Rcv Rtn Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GoodsRcvRtnQty { get; set; } = 0;



        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Ending Stock")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EnStock { get; set; }

        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Current Stock")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; }


        [Required]
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

        public Nullable<System.DateTime> OpeningStockDate { get; set; }



        //[Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }




    }

    public class CostCalculated
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CostCalculatedId { get; set; }

        public int? GRRMainId { get; set; }
        [Display(Name = "Goods Receive")]
        [ForeignKey("GRRMainId")]
        public virtual GoodsReceiveMain vGoodsReceiveMain { get; set; }
        public int? IssueMainId { get; set; }
        [ForeignKey("IssueMainId")]
        [Display(Name = "Issue")]
        public virtual IssueMain vIssueMain { get; set; }


        public int? StoreReqId { get; set; }
        [Display(Name = "Store Requisition Id")]
        [ForeignKey("StoreReqId")]
        public virtual StoreRequisitionMain vStoreRequsitionMain { get; set; }



        public int? ProductId { get; set; }
        [Display(Name = "Product Name")]
        public virtual Product Products { get; set; }


        public int? WarehouseId { get; set; }
        [Display(Name = "Warehouse")]
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouses { get; set; }

        [Display(Name = "isSubStore")]
        public bool isSubStore { get; set; }

        [Display(Name = "isDelete")]
        public bool isDelete { get; set; }

        [Display(Name = "isManualProcess")]
        public bool isManualProcess { get; set; }

        //[DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Current Qty")]
        [Column(TypeName = "decimal(18,6)")]

        public decimal CurrQty { get; set; } = 0;

        [Display(Name = "Current Price")]
        [Column(TypeName = "decimal(18,5)")]

        public decimal CurrPrice { get; set; } = 0;
        [Display(Name = "Total Current Price")]
        [Column(TypeName = "decimal(18,5)")]

        public decimal TotalCurrPrice { get; set; } = 0;

        [Display(Name = "Previous Qty")]
        [Column(TypeName = "decimal(18,6)")]

        public decimal PrevQty { get; set; } = 0;
        [Display(Name = "Previous Price")]
        [Column(TypeName = "decimal(18,5)")]

        public decimal PrevPrice { get; set; } = 0;
        [Display(Name = "Total Previous Price")]
        [Column(TypeName = "decimal(18,5)")]

        public decimal TotalPrevPrice { get; set; } = 0;

        [Display(Name = "Calculated Price")]
        [Column(TypeName = "decimal(18,5)")]

        public decimal CalculatedPrice { get; set; } = 0;

        [Display(Name = "Calculated Date")]
        public DateTime CalculatedDate { get; set; }


        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }


        [Display(Name = "User Id")]
        [StringLength(128)]
        public string userid { get; set; }

    }

    public partial class PurchaseRequisitionMain : BaseModel
    {
        public PurchaseRequisitionMain()
        {
            this.PurchaseRequisitionSub = new HashSet<PurchaseRequisitionSub>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurReqId { get; set; }


        [Display(Name = "Plant/Operation")]
        public int PrdUnitId { get; set; }

        [Required]
        [Display(Name = "PR No")]
        [StringLength(50)]
        public string PRNo { get; set; }


        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }


        [Display(Name = "Requisition Reference")]
        public string ReqRef { get; set; }


        [Display(Name = "Requisition Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]

        //[DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:dd/MM/yy}")]
        [DataType(DataType.Date)]
        public DateTime ReqDate { get; set; }


        [Display(Name = "Board Meeting Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]

        //[DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]

        public DateTime BoardMeetingDate { get; set; }





        [Display(Name = "Require Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime RequiredDate { get; set; }




        [Display(Name = "Purpose")]
        public int PurposeId { get; set; }

        [ForeignKey("PurposeId")]
        public virtual Purpose Purpose { get; set; }
        [Display(Name = "From Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Cat_Department Department { get; set; }




        [Display(Name = "From Section")]
        public int? SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Cat_Sections { get; set; }



        [Display(Name = "Approved By")]
        public int? ApprovedByEmpId { get; set; }
        [ForeignKey("ApprovedByEmpId")]
        public virtual HR_Emp_Info ApprovedBy { get; set; }

        [Display(Name = "Recommened By")]
        public int? RecommenedByEmpId { get; set; }
        [ForeignKey("RecommenedByEmpId")]
        public virtual HR_Emp_Info RecommenedBy { get; set; }
        public int Status { get; set; } = 0;
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [StringLength(60)]
        public string PcName { get; set; }
        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }
        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }


        public virtual ICollection<PurchaseRequisitionSub> PurchaseRequisitionSub { get; set; }
    }
    public partial class PurchaseRequisitionSub : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurReqSubId { get; set; }
        public int SLNo { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }

        //[MinValue(1, "Value must be at least 1")]
        [Display(Name = "Pur. Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurReqQty { get; set; }

        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RemainingReqQty { get; set; }


        [Display(Name = "Last Purchase Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LastPurchasePrice { get; set; }


        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }
        public int PurReqId { get; set; }
        [ForeignKey("PurReqId")]
        public virtual PurchaseRequisitionMain PurchaseRequisitionMain { get; set; }
        [StringLength(60)]
        public string PcName { get; set; }
    }

    public class PurchaseOrderValidMain : BaseModel
    {
        public PurchaseOrderValidMain()
        {
            this.PurchaseOrderValidSub = new HashSet<PurchaseOrderValidSub>();
        }
        [Key]
        public int PurOrderValidMainId { get; set; }
        [Display(Name = "PO Validation No")]
        [Required]
        public string POValidNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "PO Valid Date")]
        public DateTime PODate { get; set; }
        [Display(Name = "PO Valid Ref")]
        public string PORef { get; set; }
        [Display(Name = "Department")]
        public string Department { get; set; }
        [Display(Name = "Unit")]
        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }
        [Display(Name = "Purchase Req")]
        public int? PurReqId { get; set; }
        [ForeignKey("PurReqId")]
        public virtual PurchaseRequisitionMain PurchaseRequisitionMain { get; set; }
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        [Display(Name = "Payment Type")]
        public int? PaymentTypeId { get; set; }
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [Display(Name = "Convert Rate")]
        public float ConvertionRate { get; set; }
        [Display(Name = "Total PO Amount")]
        public float TotalPOValue { get; set; }
        public float? Deduction { get; set; }
        [Display(Name = "Net PO")]
        public float? NetPOValue { get; set; }
        [Display(Name = "Section")]
        public int? SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Section { get; set; }
        [Display(Name = "In House Date")]
        public DateTime? GateInHouseDate { get; set; }
        [Display(Name = "Recive Date")]
        public DateTime? ExpectedReciveDate { get; set; }


        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "Last Date Of Delivery")]
        public DateTime LastDateOfDelivery { get; set; }


        public int Status { get; set; } = 0;

        [Display(Name = "Terms & Condition")]
        public string TermsAndCondition { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [StringLength(60)]
        public string PcName { get; set; }
        public virtual ICollection<PurchaseOrderValidSub> PurchaseOrderValidSub { get; set; }

        public string GateInHouseDatestring { get; set; }
        public string ExpectedReciveDatestring { get; set; }
    }
    public class PurchaseOrderValidSub : BaseModel
    {
        [Key]
        public int PurOrderValidSubId { get; set; }
        [Display(Name = "SL No")]
        public string SLNo { get; set; }
        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }
        [Display(Name = "Requisition Qty")]
        public int? RequisitionQty { get; set; }
        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RemainingReqQty { get; set; }

        [Display(Name = "Purchase Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchaseQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Rate { get; set; }
        [Display(Name = "Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalValue { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        [StringLength(60)]
        public string PcName { get; set; }

        public int? PurReqSubId { get; set; }
        [ForeignKey("PurReqSubId")]
        public virtual PurchaseRequisitionSub PurchaseRequisitionSub { get; set; }
        public int? PurOrderValidMainId { get; set; }
        [ForeignKey("PurOrderValidMainId")]
        public virtual PurchaseOrderValidMain PurchaseOrderValidMain { get; set; }


        public virtual ICollection<PurchaseOrderValidSubSupplier> PurchaseOrderValidSubSupplier { get; set; }
    }
    public class PurchaseOrderValidSubSupplier
    {
        [Key]
        public int POValidSubSupplierId { get; set; }

        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier vSupplier { get; set; }

        public int ProductId { get; set; }
        public int SRowNo { get; set; }

        [Display(Name = "PO Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal POValidQty { get; set; }

        public int PurOrderValidSubId { get; set; }
        [ForeignKey("PurOrderValidSubId")]
        public virtual PurchaseOrderValidSub PurchaseOrderValidSub { get; set; }
    }

    public partial class StoreRequisitionMain : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreReqId { get; set; }


        [Display(Name = "Plant/Operation")]
        public int PrdUnitId { get; set; }

        [Required]
        [Display(Name = "Store Requisition No")]
        [StringLength(50)]
        public string SRNo { get; set; }


        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }


        [Display(Name = "Requisition Reference")]
        public string ReqRef { get; set; }


        [Display(Name = "Requisition Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime ReqDate { get; set; }


        [Display(Name = "Board Meeting Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime BoardMeetingDate { get; set; }

        [Display(Name = "Purpose")]
        public int PurposeId { get; set; }

        [ForeignKey("PurposeId")]
        public virtual Purpose Purpose { get; set; }
        [Display(Name = "From Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Cat_Department Department { get; set; }

        public int? SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Section { get; set; }

        [Display(Name = "Approved By Employee")]
        public int? ApprovedByEmpId { get; set; }
        [ForeignKey("ApprovedByEmpId")]
        public virtual HR_Emp_Info ApprovedBy { get; set; }

        [Display(Name = "Recommened By Employee")]
        public int? RecommenedByEmpId { get; set; }
        [ForeignKey("RecommenedByEmpId")]
        public virtual HR_Emp_Info RecommenedBy { get; set; }
        public int Status { get; set; } = 0;
        public int Complete { get; set; } = 0;
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        //[DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Require Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "Require Date")]
        public bool IsSubStore { get; set; }

        [Display(Name = "Sub Warehouse")]
        public int? SubWarehouseId { get; set; }
        [ForeignKey("SubWarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        [Display(Name = "IN No")]
        [StringLength(25)]
        public string INNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "IN Date")]
        public DateTime? INDate { get; set; }




        [StringLength(60)]
        public string PcName { get; set; }



        public virtual ICollection<StoreRequisitionSub> StoreRequisitionSub { get; set; }


        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }

    }
    public partial class StoreRequisitionSub : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreReqSubId { get; set; }
        public int SLNo { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }

        //[MinValue(1, "Value must be at least 1")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal StoreReqQty { get; set; }

        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RemainingReqQty { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }

        public bool Complete { get; set; }

        public int StoreReqId { get; set; }
        [ForeignKey("StoreReqId")]
        public virtual StoreRequisitionMain StoreRequisitionMain { get; set; }



        [StringLength(60)]
        public string PcName { get; set; }

    }
    public class Purpose
    {
        [Key]
        public int PurposeId { get; set; }
        public string PurposeName { get; set; }
        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
    public class PurchaseOrderMain : BaseModel
    {
        public PurchaseOrderMain()
        {
            this.PurchaseOrderSub = new HashSet<PurchaseOrderSub>();
        }
        [Key]
        public int PurOrderMainId { get; set; }
        [Display(Name = "PO No")]
        [Required]
        public string PONo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "PO Date")]
        public DateTime PODate { get; set; }
        [Display(Name = "PO Ref")]
        public string PORef { get; set; }
        [Display(Name = "User Department")]
        public int? DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department Department { get; set; }

        [Display(Name = "Unit")]
        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }
        [Display(Name = "Purchase Req")]
        public int? PurReqId { get; set; }
        [ForeignKey("PurReqId")]
        public virtual PurchaseRequisitionMain PurchaseRequisitionMain { get; set; }
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        [Display(Name = "Payment Type")]
        public int? PaymentTypeId { get; set; }
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [Display(Name = "Convert Rate")]
        public float ConvertionRate { get; set; }
        [Display(Name = "Total PO Amount")]
        public float TotalPOValue { get; set; }
        public float? Deduction { get; set; }
        [Display(Name = "Net Amount")]
        public float? NetPOValue { get; set; }
        [Display(Name = "Section")]
        public int? SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Section { get; set; }
        //[Display(Name = "In House Date")]
        //public DateTime? GateInHouseDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "Received Date")]
        public DateTime? ExpectedRecivedDate { get; set; }


        //[Display(Name = "Fiscal Year")]
        //public int? FiscalYearId { get; set; }
        //[ForeignKey("FiscalYearId")]
        //public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "Last Date Of Delivery")]
        public DateTime LastDateOfDelivery { get; set; }





        public int Status { get; set; } = 0;

        [Display(Name = "Terms & Condition")]
        [DataType(DataType.MultilineText)]
        public string TermsAndCondition { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        [StringLength(60)]
        public string PcName { get; set; }

        public virtual ICollection<PurchaseOrderSub> PurchaseOrderSub { get; set; }


        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }

    }
    public class PurchaseOrderSub : BaseModel
    {
        [Key]
        public int PurOrderSubId { get; set; }
        [Display(Name = "SL No")]
        public string SLNo { get; set; }
        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }
        [Display(Name = "Requisition Qty")]
        public int? RequisitionQty { get; set; }
        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RemainingReqQty { get; set; }

        [Display(Name = "Purchase Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchaseQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Rate { get; set; }
        [Display(Name = "Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalValue { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        [StringLength(60)]
        public string PcName { get; set; }

        public int? PurReqSubId { get; set; }
        [ForeignKey("PurReqSubId")]
        public virtual PurchaseRequisitionSub PurchaseRequisitionSub { get; set; }
        public int? PurOrderMainId { get; set; }
        [ForeignKey("PurOrderMainId")]
        public virtual PurchaseOrderMain PurchaseOrderMain { get; set; }
    }
    public class IssueMain : BaseModel
    {

        [Key]
        public int IssueMainId { get; set; }
        [Display(Name = "Issue No / IN No.")]
        [Required]
        public string IssueNo { get; set; }

        [Display(Name = "Manual SRR No.")]
        //[Required]
        public string ManualSRRNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        //[DataType(DataType.Date)]
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Manual SR Date")]
        public DateTime? ManualSRRDate { get; set; }


        [Display(Name = "IN No")]
        [StringLength(25)]
        public string INNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        //[DataType(DataType.Date)]
        [Display(Name = "IN Date")]
        public DateTime? INDate { get; set; }



        [Display(Name = "Issue/SR Ref")]
        public string IssueRef { get; set; }

        [Display(Name = "Department")]

        public int? DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department Department { get; set; }


        [Display(Name = "Unit")]
        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }

        public int Status { get; set; } = 0;

        [Display(Name = "Store Req")]
        public int? StoreReqId { get; set; }

        [ForeignKey("StoreReqId")]
        public virtual StoreRequisitionMain StoreRequisitionMain { get; set; }

        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [Display(Name = "Convert Rate")]
        public float ConvertionRate { get; set; }
        [Display(Name = "Total Issue Value")]
        public float TotalIssueValue { get; set; }
        public float? Deduction { get; set; }

        public float? Addition { get; set; }

        [Display(Name = "Net Issue Value")]

        public float? NetIssueValue { get; set; }

        [Display(Name = "Section")]
        public int? SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Section { get; set; }
        //[Display(Name = "In House Date")]
        //public DateTime? GateInHouseDate { get; set; }
        //[Display(Name = "Recive Date")]
        //public DateTime? ExpectedReciveDate { get; set; }
        //[Display(Name = "Terms & Condition")]
        //public string TermsAndCondition { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Rec. From KAFCO")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string AmmoniaReceivingCapco { get; set; }


        [Display(Name = "Rec. From CUFL")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string AmmoniaReceivingCufl { get; set; }



        [Display(Name = "Acid Rec.")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string PhosphoricReceiving { get; set; }



        public bool IsSubStore { get; set; }

        public bool IsDirectIssue { get; set; }






        [StringLength(60)]
        public string PcName { get; set; }

        public virtual ICollection<IssueSub> IssueSub { get; set; }

        public string GateInHouseDatestring { get; set; }
        public string ExpectedReciveDatestring { get; set; }


        ///////medical column and data 
        ///        
        [Display(Name = "Employee")]
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Doctor")]
        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual HR_Emp_Info Doctor { get; set; }

        //public float? Weight { get; set; }
        //public float? Pulse { get; set; }
        //[StringLength(10)]
        //public string BP { get; set; }

        //[StringLength(300)]
        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Advice")]
        //public string Advice { get; set; }


        /////production column and data
        ///
        [Display(Name = "BOM")]
        public int? BOMMainId { get; set; }
        [ForeignKey("BOMMainId")]
        public virtual BOMMain BOMMain { get; set; }



        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }

    }
    public class IssueSub
    {
        [Key]
        public int IssueSubId { get; set; }
        [Display(Name = "SL No")]
        public string SLNo { get; set; }
        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }

        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse vWarehouse { get; set; }
        [Display(Name = "Requisition Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal? RequisitionQty { get; set; }
        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal? RemainingReqQty { get; set; }

        [Display(Name = "Issue Qty")]
        [Column(TypeName = "decimal(18,6)")]
        public decimal IssueQty { get; set; }
        [Column(TypeName = "decimal(18,5)")]
        public decimal Rate { get; set; }

        [Display(Name = "Value")]
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalValue { get; set; }

        [Display(Name = "Balance")]
        [Column(TypeName = "decimal(18,3)")]
        public decimal? BalanceQty { get; set; }




        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        //[StringLength(80)]
        //public string ComId { get; set; }

        //[StringLength(60)]
        //public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
        public int? StoreReqSubId { get; set; }
        [ForeignKey("StoreReqSubId")]
        public virtual StoreRequisitionSub StoreRequisitionSub { get; set; }
        public int? IssueMainId { get; set; }
        [ForeignKey("IssueMainId")]
        public virtual IssueMain IssueMain { get; set; }
        public virtual ICollection<IssueSubWarehouse> IssueSubWarehouse { get; set; }

        ///////medical column and data 
        /// 
        [Display(Name = "Patient")]
        [StringLength(30)]
        public string Patient { get; set; }

        [Display(Name = "Age")]
        public float? Age { get; set; }

        //// Production Column
        [Display(Name = "Production Seed")]
        public float? ProductionSeedQty { get; set; }

        [Display(Name = "Production Bag")]
        public float? ProductionBagQty { get; set; }

        [Display(Name = "Sales Seed")]
        public float? SalesSeedQty { get; set; }

        [Display(Name = "Sales Bag")]
        public float? SalesBagQty { get; set; }



    }

    public class IssueSubWarehouse
    {
        [Key]
        public int IssueSubWarehouseId { get; set; }


        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse vWarehouse { get; set; }

        public int ProductId { get; set; }
        public int SRowNo { get; set; }


        [Display(Name = "Issue Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal IssueQty { get; set; }

        public int IssueSubId { get; set; }
        [ForeignKey("IssueSubId")]
        public virtual IssueSub IssueSub { get; set; }
    }

    public class GoodsReceiveMain : BaseModel
    {
        public GoodsReceiveMain()
        {
            this.GoodsReceiveSub = new HashSet<GoodsReceiveSub>();
            this.GoodsReceiveProvision = new HashSet<GoodsReceiveProvision>();

        }
        [Key]
        public int GRRMainId { get; set; }

        public bool IsDirectGRR { get; set; }
        [Display(Name = "GRR/MR No")]
        [Required]
        public string GRRNo { get; set; }
        [Display(Name = "GRR/MR Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime GRRDate { get; set; }
        [Display(Name = "GRR/MR Ref")]
        public string GRRRef { get; set; }
        [Display(Name = "Department")]
        public int? DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department vDepartment { get; set; }
        [Display(Name = "Unit")]
        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }
        [Display(Name = "Purchase Req")]
        public int? PurReqId { get; set; }
        [ForeignKey("PurReqId")]
        public virtual PurchaseRequisitionMain PurchaseRequisitionMain { get; set; }
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("Supplier Name")]
        public string ManualSupplierName { get; set; }


        [ForeignKey("LP No.")]
        public string LPNo { get; set; }
        [Display(Name = "LP Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime? LPDate { get; set; }


        [Display(Name = "Payment Type")]
        public int? PaymentTypeId { get; set; }
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [Display(Name = "Convert Rate")]
        public float ConvertionRate { get; set; }
        [Display(Name = "Total GRR")]
        public float TotalGRRValue { get; set; }

        [Display(Name = "Challan/Cash Memo No")]
        [StringLength(50)]
        public string ChallanNo { get; set; }

        [Display(Name = "Challan/Cash Memo Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime? ChallanDate { get; set; }


        [Display(Name = "Certificate No")]
        [StringLength(50)]
        public string CertificateNo { get; set; }

        [Display(Name = "Certificate / Received Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime? CertificateDate { get; set; }




        public int Status { get; set; } = 0;

        [Display(Name = "Net GRR/MR")]
        public float? NetGRRValue { get; set; }
        //[Display(Name = "Sub Section")]
        //public int? SubSectId { get; set; }
        //[ForeignKey("SubSectId")]
        //public virtual Cat_SubSection Cat_SubSection { get; set; }
        public int? PurOrderMainId { get; set; }
        [ForeignKey("PurOrderMainId")]
        public virtual PurchaseOrderMain PurchaseOrderMain { get; set; }

        [Display(Name = "In House Date")]
        public DateTime? GateInHouseDate { get; set; }
        [Display(Name = "Receive Date")]
        public DateTime? ExpectedReciveDate { get; set; }
        //[Display(Name = "Terms & Condition")]
        //public string TermsAndCondition { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }



        [StringLength(60)]
        public string PcName { get; set; }


        public virtual ICollection<GoodsReceiveSub> GoodsReceiveSub { get; set; }
        public virtual ICollection<GoodsReceiveProvision> GoodsReceiveProvision { get; set; }




        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }

    }

    public class GoodsReceiveSub
    {
        [Key]
        public int GRRSubId { get; set; }
        [Display(Name = "SL No")]
        public int? SLNo { get; set; }
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }
        [Display(Name = "Requisition Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RequisitionQty { get; set; }
        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RemainingReqQty { get; set; }

        [Display(Name = "Purchase Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchaseQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VatParcent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VatAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }

        [NotMapped]
        public bool Isdelete { get; set; }
        [Column(TypeName = "decimal(18,5)")]
        public decimal Rate { get; set; }
        [Display(Name = "Total Value")]
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalValue { get; set; }


        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse vWarehouse { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal Quality { get; set; } = 0;
        [Column(TypeName = "decimal(18,6)")]
        public decimal Received { get; set; } = 0;
        [Column(TypeName = "decimal(18,6)")]
        public decimal? Damage { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]

        public decimal? Deduction { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]

        public decimal? Addition { get; set; } = 0;

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        public int? PurOrderSubId { get; set; }
        [ForeignKey("PurOrderSubId")]
        public virtual PurchaseOrderSub PurchaseOrderSub { get; set; }
        public int GRRMainId { get; set; }
        [ForeignKey("GRRMainId")]
        public virtual GoodsReceiveMain GoodsReceiveMain { get; set; }
        public virtual ICollection<GoodsReceiveSubWarehouse> GoodsReceiveSubWarehouse { get; set; }
    }


    public class GoodsReceiveProvision
    {
        [Key]
        public int GRRProvisionId { get; set; }
        [Display(Name = "SL No")]
        public int? SLNo { get; set; }
        [Display(Name = "Product")]
        public int AccId { get; set; }
        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount vChartOfAccounts { get; set; }

        [Column(TypeName = "decimal(18,2)")]

        public decimal? DebitAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]

        public decimal? CreditAmount { get; set; } = 0;


        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public int GRRMainId { get; set; }
        [ForeignKey("GRRMainId")]
        public virtual GoodsReceiveMain GoodsReceiveMain { get; set; }

        [NotMapped]
        public bool Isdelete { get; set; }
    }


    public class GoodsReceiveSubWarehouse
    {
        [Key]
        public int GRRSubWarehouseId { get; set; }

        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse vWarehouse { get; set; }

        public int ProductId { get; set; }
        public int SRowNo { get; set; }

        [Display(Name = "GRR/MR Qty")]
        [Column(TypeName = "decimal(18,6)")]

        public decimal GRRQty { get; set; }

        public int GRRSubId { get; set; }
        [ForeignKey("GRRSubId")]
        public virtual GoodsReceiveSub GoodsReceiveSub { get; set; }
    }


    public partial class DistrictWiseBooking : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DistWiseBookingId { get; set; }
        [Display(Name = "Fiscal Year")]

        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }
        [Display(Name = "Fiscal Month")]

        public int FiscalMonthId { get; set; }

        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }
        [Display(Name = "District")]

        public int DistId { get; set; }
        [ForeignKey("DistId")]
        public virtual Cat_District Cat_District { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }
        public float Qty { get; set; }
        [NotMapped]
        [Column(TypeName = "decimal(18,2)")]

        public double PrevAllotmentQty { get; set; }
        public string AllotmentType { get; set; }
        public string PcName { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
    }
    public class Booking : BaseModel
    {
        public int BookingId { get; set; }
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }
        public int FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public int PStationId { get; set; }
        [ForeignKey("PStationId")]
        public virtual Cat_PoliceStation Cat_PoliceStation { get; set; }
        public int DistId { get; set; }
        [ForeignKey("DistId")]
        public virtual Cat_District Cat_District { get; set; }
        public float AllotmentQty { get; set; }
        public string AllotmentType { get; set; }
        [StringLength(60)]
        public string PcName { get; set; }
        public int? DistWiseBookingId { get; set; }
        [ForeignKey("DistWiseBookingId")]
        public virtual DistrictWiseBooking DistrictWiseBooking { get; set; }

        public virtual ICollection<DeliveryOrder> vDeliveryOrder { get; set; }


    }





    public class DeliveryOrder : BaseModel
    {
        public DeliveryOrder()
        {

        }
        [Key]
        public int DOId { get; set; }
        public int DONo { get; set; } = 1;

        [NotMapped]
        public int OldDONo { get; set; }


        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime DODate { get; set; }

        public int AccId { get; set; }
        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount Acc_ChartOfAccount { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        public int? RepresentativeId { get; set; }
        [ForeignKey("RepresentativeId")]
        public virtual Representative vRepresentative { get; set; }


        public int PayInSlipNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime PayInSlipDate { get; set; }
        public float Qty { get; set; }
        public float RemainingQty { get; set; }
        [NotMapped]
        public float CurrentRemainingQty { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public string Remarks { get; set; }
        public string QtyInWordsEng { get; set; }
        public string QtyInWordsBng { get; set; }
        public string TotalPriceInWordsEng { get; set; }
        public string TotalPriceInWordsBng { get; set; }
        [StringLength(60)]
        public string PcName { get; set; }
        public virtual ICollection<DeliveryChallan> vDeliveryChallan { get; set; }
    }
    public class DeliveryChallan : BaseModel
    {

        public DeliveryChallan()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryChallanId { get; set; }
        public int ChallanNo { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
        public float DeliveryQty { get; set; }
        public int DOId { get; set; }
        [ForeignKey("DOId")]
        public virtual DeliveryOrder DeliveryOrder { get; set; }

        public virtual ICollection<GatePassSub> GatePassChallans { get; set; }

        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public string DODate { get; set; }
        [NotMapped]
        public string PayInSlipDate { get; set; }
    }

    public class GatePass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GatePassId { get; set; }
        public int GatePassNo { get; set; }
        public string GatePassFrom { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime GatePassDate { get; set; }

        [Required]
        public string TruckNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverMobile { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverMobile { get; set; }


        public int FiscalYearId { get; set; }
        public int FiscalMonthId { get; set; }
        public int? DistrictId { get; set; }
        public int? PoliceStationId { get; set; }
        public float TotalQty { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        [StringLength(128)]
        public string UpdateByUserId { get; set; }

        public int Status { get; set; } = 0;

        public virtual ICollection<GatePassSub> Challans { get; set; }
    }

    public class GatePassSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GatePassSubId { get; set; }


        public int GatePassId { get; set; }
        [ForeignKey("GatePassId")]
        public virtual GatePass GatePass { get; set; }

        public int DeliveryChallanId { get; set; }
        [ForeignKey("DeliveryChallanId")]

        public virtual DeliveryChallan DeliveryChallans { get; set; }


        public float TruckLoadQty { get; set; }
        public float BalanceQty { get; set; }




        [NotMapped]
        public string DistName { get; set; }
        [NotMapped]
        public int PStationName { get; set; }
        [NotMapped]
        public string FyName { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }


    }


    public class DirectGoodsReceive
    {

        [Key]
        public int GRRMainId { get; set; }
        [Display(Name = "GRR No")]
        [Required]
        public string GRRNo { get; set; }
        [Display(Name = "GRR Date")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        public DateTime GRRDate { get; set; }
        [Display(Name = "GRR Ref")]
        public string GRRRef { get; set; }
        [Display(Name = "Department")]
        public string Department { get; set; }
        [Display(Name = "Unit")]
        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }
        [Display(Name = "Purchase Req")]
        public int? PurReqId { get; set; }
        [ForeignKey("PurReqId")]
        public virtual PurchaseRequisitionMain PurchaseRequisitionMain { get; set; }
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        [Display(Name = "Payment Type")]
        public int? PaymentTypeId { get; set; }
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [Display(Name = "Convert Rate")]
        public float ConvertionRate { get; set; }
        [Display(Name = "Total GRR")]
        public float TotalGRRValue { get; set; }
        public float? Deduction { get; set; }
        [Display(Name = "Net GRR")]
        public float? NetGRRValue { get; set; }
        [Display(Name = "Sub Section")]
        public int? SubSectId { get; set; }
        [ForeignKey("SubSectId")]
        public virtual Cat_SubSection Cat_SubSection { get; set; }
        public int? PurOrderMainId { get; set; }
        [ForeignKey("PurOrderMainId")]
        public virtual PurchaseOrderMain PurchaseOrderMain { get; set; }

        [Display(Name = "In House Date")]
        public DateTime? GateInHouseDate { get; set; }
        [Display(Name = "Recive Date")]
        public DateTime? ExpectedReciveDate { get; set; }
        [Display(Name = "Terms & Condition")]
        public string TermsAndCondition { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public int PurReqSubId { get; set; }
        public int SLNo { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product vProduct { get; set; }

        //[MinValue(1, "Value must be at least 1")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurReqQty { get; set; }

        [Display(Name = "Remaining Req Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RemainingReqQty { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [NotMapped]
        public bool IsDelete { get; set; }
        //public int PurReqId { get; set; }


        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<GoodsReceiveSub> GoodsReceiveSub { get; set; }
    }



    public class BudgetMain
    {
        [Key]
        public int BudgetMainId { get; set; }
        /// public int BookingNo { get; set; }
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }
        public int FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBudgetDebit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBudgetCredit { get; set; }


        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }


        public virtual ICollection<BudgetDetails> BudgetDetails { get; set; }


    }


    public class BudgetDetails
    {

        [Key]
        public int BudgetDetailsId { get; set; }

        public int BudgetMainId { get; set; }
        [ForeignKey("BudgetMainId")]
        public virtual BudgetMain BudgetMain { get; set; }

        /// public int BookingNo { get; set; }
        //public int FiscalYearId { get; set; }
        //[ForeignKey("FiscalYearId")]
        //public virtual Acc_FiscalYear YearName { get; set; }
        //public int FiscalMonthId { get; set; }
        //[ForeignKey("FiscalMonthId")]
        //public virtual Acc_FiscalMonth MonthName { get; set; }
        public int AccId { get; set; }
        [ForeignKey("AccId")]
        public virtual Acc_ChartOfAccount Acc_ChartOfAccounts { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BudgetDebit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BudgetCredit { get; set; }



        //[Display(Name = "Currency")]

        //public int CountryId { get; set; }
        //[ForeignKey("CountryId")]
        //public virtual Country Currenncy { get; set; }

        //[Display(Name = "Currency Local")]

        //public int CountryIdLocal { get; set; } = 0;

        //[ForeignKey("CountryIdLocal")]
        //public virtual Country CurrenncyLocal { get; set; }




        //[Display(Name = "Currency Rate")]
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal Rate { get; set; } = 0;



        //[Display(Name = "Budget Debit [Local]")]
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal BudgetDebitLocal { get; set; } = 0;


        //[Display(Name = "Budget Credit [Local]")]
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal BudgetCreditLocal { get; set; } = 0;




        //[StringLength(80)]
        //public string ComId { get; set; }

        //[StringLength(60)]
        //public string PcName { get; set; }
        //[StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }


    }

}