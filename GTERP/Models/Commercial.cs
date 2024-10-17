using GTERP.Models.Base;
using GTERP.Models.Self;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class StyleInformation
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StyleInformation()
        {
            ExportOrders = new HashSet<ExportOrder>();
            //this.ExportOrders1 = new HashSet<ExportOrder>();
        }
        [Key]
        public int StyleId { get; set; }

        [Display(Name = "Style Name")]
        //[Index("IX_StyleInformationName", 1, IsUnique = true)]
        [StringLength(200)]
        public string StyleName { get; set; }
        [Display(Name = "Style Code")]

        public string StyleCode { get; set; }
        [Display(Name = "Concern")]

        public Nullable<int> CommercialCompanyId { get; set; }
        [Display(Name = "Buyer")]

        public Nullable<int> BuyerId { get; set; }
        [Display(Name = "Order Qty")]

        public Nullable<int> OrderQty { get; set; }
        [Display(Name = "UOM")]

        public string UnitMasterId { get; set; }
        [Display(Name = "FOB")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> FOB { get; set; }
        [Display(Name = "Currency")]

        public Nullable<int> CurrencyId { get; set; }
        [Display(Name = "Sales Cost")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> SalesCost { get; set; }
        //[Display(Name = "Style Status")]

        //public string StyleStatus { get; set; }
        [Display(Name = "Status")]

        public Nullable<int> StyleStatusId { get; set; }

        [Display(Name = "HSCode")]
        public string HSCode { get; set; }

        [Display(Name = "Fabrication")]
        public string Fabrication { get; set; }





        [Display(Name = "First Ship Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> FirstShipDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Ship Date")]

        public Nullable<System.DateTime> LastShipDate { get; set; }
        [Display(Name = "Product Category")]

        public Nullable<long> ProductCategoryId { get; set; }
        [Display(Name = "Brand")]

        public Nullable<int> BrandInfoId { get; set; }
        [Display(Name = "Product Group")]

        public Nullable<int> ProductGroupId { get; set; }


        public string AddByUserId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual BrandInfo BrandInfo { get; set; }
        [ForeignKey("BuyerId")]
        public virtual BuyerInformation BuyerInformation { get; set; }
        public virtual SisterConcernCompany Company { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [Display(Name = "Company Id")]
        //[Index("IX_ComAccnameUniqe", 1, IsUnique = true)]
        [StringLength(128)]
        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }
        public virtual StyleStatus StyleStatus { get; set; }


        public virtual ProductGroup ProductGroup { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportOrder> ExportOrders { get; set; }
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ExportOrder> ExportOrders1 { get; set; }
    }

    public partial class BrandInfo
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BrandInfo()
        {
            StyleInformations = new HashSet<StyleInformation>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int BrandInfoId { get; set; }
        [Display(Name = "Brand")]


        public string BrandName { get; set; }
        public string AddByUserId { get; set; }
        [Display(Name = "Company Id")]
        //[Index("IX_ComAccnameUniqe", 1, IsUnique = true)]
        [StringLength(128)]
        public string comid { get; set; }
        public Boolean isDelete { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public partial class SisterConcernCompany : BaseModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SisterConcernCompany()
        {
            StyleInformations = new HashSet<StyleInformation>();
        }

        public int SisterConcernCompanyId { get; set; }
        [Display(Name = "Concern")]
        [DataType(DataType.MultilineText)]

        public string CompanyName { get; set; }
        [Display(Name = "Concern")]

        public string CompanyShortName { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]

        public string Address { get; set; }

        [Display(Name = "Address2")]
        [DataType(DataType.MultilineText)]

        public string Address2 { get; set; }

        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }
        [Display(Name = "Fax")]

        public string FaxNumber { get; set; }
        [Display(Name = "Email")]

        public string EmailID { get; set; }

        [Display(Name = "Web Address")]

        public string Web { get; set; }

        [Display(Name = "Contact Person")]

        public string ContactPerson { get; set; }
        [Display(Name = "Status")]

        public bool Active { get; set; }
        [Display(Name = "Trade License No")]

        public string TradeLicenceNo { get; set; }
        [Display(Name = "TIN No")]

        public string TINNo { get; set; }
        [Display(Name = "VAT No")]

        //public string VATNo { get; set; }
        //[Display(Name = "IRC No")]

        //public string IRCNo { get; set; }
        //[Display(Name = "BKMEA Reg No")]

        //public string BKMEARegNo { get; set; }
        //[StringLength(128)]
        //public string comid { get; set; }
        //[StringLength(128)]
        //public string userid { get; set; }
        //public Boolean isDelete { get; set; }



        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public partial class ProductGroup
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductGroup()
        {
            StyleInformations = new HashSet<StyleInformation>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProductGroupId { get; set; }
        [Display(Name = "Product Group")]

        public string ProductGroupName { get; set; }
        public string AddByUserId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public partial class Currency : SelfModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Currency()
        {
            StyleInformations = new HashSet<StyleInformation>();
        }

        public int CurrencyId { get; set; }
        public Boolean isDefault { get; set; }

        [Display(Name = "Currency")]

        public string CurCode { get; set; }


        [Display(Name = "Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Rate { get; set; }
        [Display(Name = "Effective Date")]

        public Nullable<System.DateTime> EffectDate { get; set; }
        public string AddByUserId { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleInformation> StyleInformations { get; set; }

        public virtual ICollection<COM_MasterLC> COM_MasterLC { get; set; }
    }

    public partial class ProductCategory
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductCategory()
        {
            StyleInformations = new HashSet<StyleInformation>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AddByUserId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public partial class BuyerInformation : BaseModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuyerInformation()
        {
            StyleInformations = new HashSet<StyleInformation>();
        }
        [Key]
        public int BuyerId { get; set; }


        //[Index("IX_FirstAndSecond", 1, IsUnique = true)]
        [Display(Name = "Buyer")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required]
        ////[Index(IsUnique = true)]
        [DataType(DataType.MultilineText)]
        public string BuyerName { get; set; }

        //[Index("IX_FirstAndSecond", 2, IsUnique = true)]

        [Display(Name = "Country")]
        public int CountryId { get; set; }
        [Display(Name = "Contact Person")]


        public string ContactPerson { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Shipping Marks")]
        public string ShippingMarks { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Local Office")]
        public string LocalOffice { get; set; }


        [Display(Name = "Buyer Search Field")]
        public string BuyerSearchName { get; set; }


        [Display(Name = "LC Margin")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> LCMargin { get; set; }



        [Display(Name = "Discount [%]")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> DiscountPercentage { get; set; }


        [Display(Name = "CMP [%]")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> CMPPercentage { get; set; }



        [Display(Name = "Defferred Payment Days")]
        public Nullable<int> DefferredPaymentDays { get; set; }


        [Display(Name = "Import Person / Employee :")]
        public Nullable<int> EmployeeIdImport { get; set; }

        //[Display(Name = "Category Name")]
        [ForeignKey("EmployeeIdImport")]
        public virtual Employee EmployeeImport { get; set; }


        [Display(Name = "Export Person / Employee :")]
        public Nullable<int> EmployeeIdExport { get; set; }

        //[Display(Name = "Category Name")]
        [ForeignKey("EmployeeIdExport")]
        public virtual Employee EmployeeExport { get; set; }


        public string Addedby { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string Updatedby { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }
        //[StringLength(128)]
        //public string comid { get; set; }
        //[Display(Name = "Country")]

        public virtual Country Country { get; set; }







        [Display(Name = "Buyer Group :")]
        public Nullable<int> BuyerGroupId { get; set; }

        //[Display(Name = "Category Name")]
        [ForeignKey("BuyerGroupId")]
        public virtual BuyerGroup BuyerGroups { get; set; }



        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleInformation> StyleInformations { get; set; }
    }

    public partial class ExportOrder
    {
        //        [Key, Column(Order = 0)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExportOrderID { get; set; }
        [Display(Name = "Buyer PO No.")]
        public string BuyerContactPONo { get; set; }
        [Display(Name = "PO Line No")]

        public string POLineNo { get; set; }
        ////[Key, Column(Order = 1)]
        [Display(Name = "Style No")]

        public int StyleID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "PO Date")]
        public System.DateTime PoDate { get; set; }
        [Display(Name = "Destination")]

        public int DestinationID { get; set; }
        [Display(Name = "Order Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderQty { get; set; }
        public string UnitMasterId { get; set; }
        [Display(Name = "Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }
        [Display(Name = "CM")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> CM { get; set; }
        [Display(Name = "Order Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderValue { get; set; }
        [Display(Name = "Ship Mode")]

        public Nullable<int> ShipModeId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ex Factory Date")]
        public Nullable<System.DateTime> ExFactoryDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ship Date")]
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Added Date")]
        public System.DateTime DateAdded { get; set; }
        public string UpdatedBy { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Updated Date")]
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [Display(Name = "Order Status")]


        public Nullable<int> ExportOrderStatusId { get; set; }
        [Display(Name = "Order Category")]

        public Nullable<int> ExportOrderCategoryId { get; set; }


        [Display(Name = "Remark")]

        public string Remark { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        public string comid { get; set; }
        public Boolean isDelete { get; set; }


        [Display(Name = "Style Information")]

        public virtual StyleInformation StyleInformation { get; set; }
        //public virtual StyleInformation StyleInformation1 { get; set; }
        [Display(Name = "Unit")]

        public virtual UnitMaster UnitMaster { get; set; }
        [Display(Name = "Destination")]


        public virtual Destination Destination { get; set; }
        public virtual ShipMode ShipModes { get; set; }
        public virtual ExportOrderStatus ExportOrderStatus { get; set; }
        public virtual ExportOrderCategory ExportOrderCategorys { get; set; }



    }
    public partial class UnitGroup
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitGroup()
        {
            UnitMaster = new HashSet<UnitMaster>();
        }
        [Display(Name = "Unit Group")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UnitGroupId { get; set; }


        //public string UnitGroupName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitMaster> UnitMaster { get; set; }
    }

    public partial class UnitMaster : SelfModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitMaster()
        {
            ExportOrders = new HashSet<ExportOrder>();
        }

        public string UnitGroupId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Unit")]

        public string UnitMasterId { get; set; }
        //public string UnitShortName { get; set; }
        [Display(Name = "Unit Short Name")]
        public string UnitName { get; set; }
        [Display(Name = "Relative Factor")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> RelativeFactor { get; set; }
        [Display(Name = "isBase UOM")]

        public Nullable<bool> IsBaseUOM { get; set; }
        public string AddedBy { get; set; }
        //public System.DateTime DateAdded { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public string UMId { get; set; }

        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportOrder> ExportOrders { get; set; }
        [ForeignKey("UnitGroupId")]
        public virtual UnitGroup UnitGroup { get; set; }
    }

    public partial class Destination : BaseModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Destination()
        {
            ExportOrders = new HashSet<ExportOrder>();
        }


        [Display(Name = "Destination Search Name")]
        public string DestinationNameSearch { get; set; }

        public int DestinationID { get; set; }
        [Display(Name = "Destination Code")]

        public string DestinationCode { get; set; }
        [Display(Name = "Destination")]
        //[Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required]
        public string DestinationName { get; set; }
        [Display(Name = "Country")]

        public Nullable<int> CountryId { get; set; }
        public virtual Country Countrys { get; set; }
        public string Addedby { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string Updatedby { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportOrder> ExportOrders { get; set; }
    }

    public partial class SupplierInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ContactID")]
        public int ContactID { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required]
        [Display(Name = "Supplier")]

        public string SupplierName { get; set; }
        [Display(Name = "Country")]

        public int CountryId { get; set; }
        [Display(Name = "Contact Person")]

        public string ContactPerson { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Local Office")]
        public string LocalOffice { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        [Display(Name = "Merchandiser")]

        public string Merchandiser { get; set; }

        public string Addedby { get; set; }

        //[Required]
        [StringLength(128)]
        public string userid { get; set; }
        [StringLength(128)]
        public string comid { get; set; }
        public Boolean isDelete { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual Country Country { get; set; }
    }

    public partial class COM_CNFBillExportMaster
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CNFExpenseID")]
        public int CNFExpenseID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "CNF EXpense Bill Date")]
        public System.DateTime CNFEXpenseBillDate { get; set; }
        [Required]
        [Display(Name = "Company")]
        public int CommercialCompanyId { get; set; }
        public Nullable<int> BuyerID { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required(ErrorMessage = "Please Provide JOB No")]
        [Display(Name = "Job No")]
        //[Index(IsUnique = true)]
        public string JobNo { get; set; }
        public string Messers { get; set; }
        public string Subject { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Comission { get; set; }
        [Display(Name = "Total Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalAmount { get; set; }
        [Display(Name = "Invoice Value")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> InvoiceValue { get; set; }
        [Display(Name = "Conversion Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> ConversionRate { get; set; }


        public bool IsLocked { get; set; }
        public string AddedBy { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime DateUpdated { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        public Boolean isDelete { get; set; }

        public virtual SisterConcernCompany Companies { get; set; }
        public virtual BuyerInformation BuyerInformations { get; set; }
        public virtual ICollection<COM_CNFBillExportDetails> COM_CNFBillExportDetails { get; set; }
        [Display(Name = "Export Invoice")]
        public Nullable<int> InvoiceId { get; set; }

        public virtual ExportInvoiceMaster ExportInvoiceMasters { get; set; }
    }
    public class COM_CNFBillImportMaster
    {
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COM_CNFBillImportMaster()
        {
            this.COM_CNFBillImportDetails = new HashSet<COM_CNFBillImportDetails>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CNFExpenseID")]
        public int CNFExpenseID { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "CNF ExpenseBill Date")]
        public System.DateTime CNFEXpenseBillDate { get; set; }

        [Required]
        [Display(Name = "Messers")]
        public int CommercialCompanyId { get; set; }

        //[Required]
        [Display(Name = "Buyer")]
        public Nullable<int> BuyerID { get; set; }


        [Display(Name = "Supplier")]
        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformations { get; set; }



        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required(ErrorMessage = "Please Provide JOB No")]
        [Display(Name = "Job No")]
        //[Index(IsUnique = true)]
        public string JobNo { get; set; }

        [Display(Name = "Consignment")]

        public string Consignment { get; set; }

        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Vassel Name")]
        public string VasselName { get; set; }

        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Rotation No")]
        public string RotationNo { get; set; }
        [Display(Name = "Line")]

        public string Line { get; set; }

        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Bond BENo")]
        public string BondBENo { get; set; }

        [Display(Name = "Bond Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BondDate { get; set; }

        [Display(Name = "BL No")]
        public string BLNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "BL Date")]
        public Nullable<System.DateTime> BLDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "BBLC No")]
        public Nullable<int> BBLCId { get; set; }
        public virtual COM_BBLC_Master COM_BBLC_Masters { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "BBLC Date")]
        public Nullable<System.DateTime> BBLCDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Comission")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Comission { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Total Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalAmount { get; set; }

        [Display(Name = "Conversion Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> ConversionRate { get; set; }
        public bool IsLocked { get; set; }

        [Display(Name = "Added By")]
        public string AddedBy { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        [StringLength(128)]
        public string useridupdate { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        public Boolean isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Added")]
        public System.DateTime DateAdded { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Updated")]
        public System.DateTime DateUpdated { get; set; }

        //public virtual COM_CNFBillImportDetails COM_CNFBillImportDetails { get; set; }
        public virtual SisterConcernCompany Companies { get; set; }
        public virtual BuyerInformation BuyerInformations { get; set; }
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_CNFBillImportDetails> COM_CNFBillImportDetails { get; set; }

        //[Required]
        [Display(Name = "Import Invoice")]
        public Nullable<long> CommercialInvoiceId { get; set; }

        public virtual COM_CommercialInvoice COM_CommercialInvoices { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "CI Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> CIAmount { get; set; }
    }

    public class COM_CNFBillImportDetails
    {
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public COM_CNFBillImportDetails()
        //{
        //    this.COM_CNFBillImportMaster = new HashSet<COM_CNFBillImportMaster>();


        //}

        [Key, Column(Order = 0)]
        public int CNFExpenseID { get; set; }
        //[Key, Column(Order = 1)]
        public int ExpenseHeadID { get; set; }
        [Display(Name = "Is Check")]

        public bool IsCheck { get; set; }
        //public int CNFExpensesNo { get; set; }

        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Amount { get; set; }

        public string AddedBY { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }

        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual COM_CNFBillImportMaster COM_CNFBillImportMaster { get; set; }

        public virtual COM_CNFExpenseType COM_CNFExpenseTypes { get; set; }
    }

    public partial class COM_CNFBillExportDetails
    {
        //public int CNFExpensesNo { get; set; }
        [Key, Column(Order = 0)]
        public int CNFExpenseID { get; set; }
        //[Key, Column(Order = 1)]
        public int ExpenseHeadID { get; set; }
        public string Remarks { get; set; }
        public string CTN { get; set; }
        public string InvoiceNo { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Amount { get; set; }
        public bool IsCheck { get; set; }
        public string AddedBY { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public virtual COM_CNFBillExportMaster COM_CNFBillExportMaster { get; set; }

        public virtual COM_CNFExpenseType COM_CNFExpenseTypes { get; set; }
    }

    public partial class COM_CNFExpenseType
    {
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COM_CNFExpenseType()
        {
            this.COM_CNFBillImportDetails = new HashSet<COM_CNFBillImportDetails>();

        }

        [Key]
        public int ExpenseHeadID { get; set; }
        [Display(Name = "CNF Expense No")]

        public string CNFExpenseNo { get; set; }
        [Display(Name = "CNF Expense Name")]


        public string CNFExpenseName { get; set; }

        [Display(Name = "Percentage [%]")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> AmountPercentage { get; set; }

        [Display(Name = "Default Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> DefaultAmount { get; set; }
        [Display(Name = "Import / Export")]

        public string ImportOrExport { get; set; }
        [Display(Name = "Is Active")]

        public Nullable<bool> IsActive { get; set; }
        public string AddedBy { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdatedBy { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        public Boolean isDelete { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ICollection<COM_CNFBillImportDetails> COM_CNFBillImportDetails { get; set; }
        public virtual ICollection<COM_CNFBillExportDetails> COM_CNFBillExportDetails { get; set; }

    }

    public partial class COM_MasterLC
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COM_MasterLC()
        {
            this.COM_MasterLCExport = new HashSet<COM_MasterLCExport>();
            this.COM_MasterLC_Details = new HashSet<COM_MasterLC_Details>();
            this.ExportInvoiceMasters = new HashSet<ExportInvoiceMaster>();
            this.COM_GroupLC_Subs = new HashSet<COM_GroupLC_Sub>();
        }


        //private ILazyLoader LazyLoader { get; set; }
        //public COM_MasterLC(ILazyLoader lazyLoader)
        //{
        //    LazyLoader = lazyLoader;
        //}

        //private ICollection<COM_MasterLC_Details> _COM_MasterLC_Details;


        //public ICollection<COM_MasterLC_Details> COM_MasterLC_Details
        //{
        //    get => LazyLoader.Load(this, ref _COM_MasterLC_Details);
        //    set => _COM_MasterLC_Details = value;
        //}




        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MasterLCID")]
        public int MasterLCID { get; set; }
        [Display(Name = "Sales Contract No.")]
        [Required]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        //[Index(IsUnique = true)]
        public string LCRefNo { get; set; }
        [Display(Name = "LC Type")]

        public int LCTypeId { get; set; }
        public virtual LCType LCType { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        [Display(Name = "Export LC No.")]
        //[Index(IsUnique = true)]
        public string BuyerLCRef { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Display(Name = "Export LC No. [Import Dept.] Rev. Amd No.")]
        ////[Index(IsUnique = true)]
        public string LCNOforImport { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Export LC Issue Date")]
        public Nullable<System.DateTime> LCOpenDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Sales Contract Issue Date")]
        public Nullable<System.DateTime> SalesContractIssueDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Sales Contract Last Shipment Date")]
        public Nullable<System.DateTime> LCExpirydate { get; set; }


        [Display(Name = "Last Shipment Date")]
        public Nullable<System.DateTime> LastShipmentDate { get; set; }

        [Display(Name = "Export LC/ Contract Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalLCQty { get; set; } = 0;
        [Display(Name = "UOM")]
        public string UnitMasterId { get; set; }
        [Display(Name = "Export LC/ Contract Value")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> LCValue { get; set; } = 0;

        [Display(Name = "Remaining Contract Value")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Balance { get; set; } = 0;


        [Display(Name = "Sales Contract Value")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal MasterLCValueManual { get; set; }
        [Display(Name = "LC Status")]
        [Required(ErrorMessage = "Please select LC Status")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select LC Status")]
        public int LCStatusId { get; set; }
        public virtual LCStatus LCStatus { get; set; }

        [Display(Name = "LC Nature")]
        [Required(ErrorMessage = "Please select LC Nature")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select LC Nature")]
        public int LCNatureId { get; set; }

        public virtual LCNature LCNature { get; set; }

        [Display(Name = "Tenor")]

        public Nullable<int> Tenor { get; set; }
        [Display(Name = "Trade Term")]
        [Required(ErrorMessage = "Please select Trade Term")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Trade Term")]

        public Nullable<int> TradeTermId { get; set; }


        public virtual TradeTerm TradeTerms { get; set; }

        public string Addedby { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        public Boolean isDelete { get; set; }

        [Display(Name = "Ship Mode")]
        [Required(ErrorMessage = "Please select Ship Mode")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Ship Mode")]
        public int ShipModeId { get; set; }
        public virtual ShipMode ShipModes { get; set; }


        [Display(Name = "Payment Terms")]
        public int PaymentTermsId { get; set; }
        public virtual PaymentTerms PaymentTerms { get; set; }

        public int DayListId { get; set; }
        public virtual DayList DayList { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_MasterLCExport> COM_MasterLCExport { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_MasterLC_Details> COM_MasterLC_Details { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportInvoiceMaster> ExportInvoiceMasters { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_GroupLC_Sub> COM_GroupLC_Subs { get; set; }



        [Display(Name = "Buyer")]
        [Required(ErrorMessage = "Please select Buyer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Buyer")]
        public int BuyerID { get; set; }
        [Display(Name = "Buyer")]

        public virtual BuyerInformation BuyerInformations { get; set; }


        [Display(Name = "Group Buyer")]
        //[Required(ErrorMessage = "Please select Buyer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Buyer")]
        public Nullable<int> BuyerGroupID { get; set; }
        [Display(Name = "Group Buyer")]

        public virtual BuyerGroup BuyerGroups { get; set; }




        [Display(Name = "Final Destination")]
        [Required(ErrorMessage = "Please select Destination")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Destination")]
        public int DestinationId { get; set; }
        [Display(Name = "Final Destination")]

        public virtual Destination Destinations { get; set; }


        [Display(Name = "Destination")]
        [Required(ErrorMessage = "Please select Destination")]
        public string DestinationContract { get; set; }


        [Display(Name = "Remarks")]
        public string Remarks { get; set; }


        [Display(Name = "Company / Concern")]
        [Required(ErrorMessage = "Please select Concern")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Concern")]


        public int CommercialCompanyId { get; set; }
        [Display(Name = "Company / Concern")]

        public virtual SisterConcernCompany CommercialCompanies { get; set; }

        [Display(Name = "Unit")]


        public virtual UnitMaster UnitMaster { get; set; }
        [Display(Name = "Currency")]

        [Required(ErrorMessage = "Please select Currency")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "Currency")]

        public virtual Currency Currency { get; set; }
        [Display(Name = "Exporter Bank")]
        [Required(ErrorMessage = "Please select Exporter Bank")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Exporter Bank")]
        public int OpeningBankId { get; set; }
        public virtual OpeningBank OpeningBank { get; set; }

        [Display(Name = "Bank Account No")]
        //[Required(ErrorMessage = "Please select Account No.")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Bank Account No.")]
        public Nullable<int> BankAccountId { get; set; }


        public virtual BankAccountNo BankAccountNos { get; set; }
        [Display(Name = "Opening / Buyer Bank")]
        [Required(ErrorMessage = "Please select Opening / Buyer Bank")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Opening / Buyer Bank")]

        public int LienBankId { get; set; }
        [Display(Name = "Opening / Buyer Bank")]

        public virtual LienBank LienBank { get; set; }
        [Display(Name = "Port Of Loading / Port of Discharge")]
        [Required(ErrorMessage = "Please select Loading Point")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Loading Point")]
        public Nullable<int> PortOfLoadingId { get; set; }
        [Display(Name = "Port Of Loading")]

        public virtual PortOfLoading PortOfLoading { get; set; }


        [Display(Name = "Port Of Discharge / Port of Discharge")]
        [Required(ErrorMessage = "Please select Discharge Point")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select Discharge Point")]
        public Nullable<int> PortOfDischargeId { get; set; }
        [Display(Name = "Port Of Discharge")]

        public virtual PortOfDischarge PortOfDischarge { get; set; }


        [Display(Name = "Tolerance")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> Tolerance { get; set; }

        [Display(Name = "Shipping Agent")]

        public Nullable<int> SupplierId { get; set; }///supplier table

        [ForeignKey("SupplierId")]

        public virtual SupplierInformation ShippingAgent { get; set; }

        ///////////
        /////////
        /////newly added column 5-nov-19
        /////

        public string Insurance { get; set; }

        public string AccountNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FirstShipmentDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> LastShipmentDate { get; set; }

        public string RemarksOne { get; set; }
        public string RemarksTwo { get; set; }
        public string RemarksThree { get; set; }
        public string RemarksFour { get; set; }
        public string RemarksFive { get; set; }

        public string FileNo { get; set; } //need to add later

    }

    public partial class COM_MasterLCExport
    {
        [Key, Column(Order = 0)]
        public int MasterLCID { get; set; }
        //[Key, Column(Order = 1)]

        public int ExportOrderID { get; set; }
        public string ExportPONo { get; set; }
        public string ExportOrderStatus { get; set; }
        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [Display(Name = "Master LC")]


        public virtual COM_MasterLC COM_MasterLC { get; set; }

        [Display(Name = "Export Orders")]

        public virtual ExportOrder ExportOrders { get; set; }


        //////newlyadded column
        ///
    }

    public partial class COM_MasterLC_Details
    {
        //public int COM_MasterLC_DetailsId { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COM_MasterLC_Details()
        {
            this.ExportInvoiceDetails = new HashSet<ExportInvoiceDetails>();
        }



        //private COM_MasterLC _cOM_MasterLC;

        //private COM_MasterLC_Details(ILazyLoader lazyLoader)
        //{
        //    LazyLoader = lazyLoader;
        //}
        //private ILazyLoader LazyLoader { get; set; }

        //public COM_MasterLC COM_MasterLC
        //{
        //    get => LazyLoader.Load(this, ref _cOM_MasterLC);
        //    set => _cOM_MasterLC = value;
        //}

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MasterLCDetailsID { get; set; }

        ////[Key, Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MasterLCID { get; set; }


        [Display(Name = "SL No")]

        public Nullable<int> SL { get; set; }

        public string ExportPONo { get; set; }
        public string StyleName { get; set; }
        public string ItemName { get; set; }
        public string HSCode { get; set; }
        public string Fabrication { get; set; }
        public float OrderQty { get; set; }
        [Display(Name = "Order UOM")]

        public string UnitMasterId { get; set; }
        [Display(Name = "Unit")]
        [ForeignKey("UnitMasterId")]

        public virtual UnitMaster UnitMaster { get; set; }

        public float Factor { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal QtyInPcs { get; set; }
        //[Column(TypeName = "decimal(16,3)")]
        //[DataType("decimal(18,2)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } ///qty in pcs * qtyinpcs
        [Column(TypeName = "decimal(18,2)")]

        public decimal TotalValue { get; set; }
        [Display(Name = "Shipment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")] //, ApplyFormatInEditMode = true
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public string Destination { get; set; }

        [Display(Name = "Contract No")]

        public string ContractNo { get; set; }
        [Display(Name = "Order Type")]

        public string OrderType { get; set; }
        [NotMapped]
        public Nullable<Boolean> isDelete { get; set; }

        [Display(Name = "Cat No")]

        public string CatNo { get; set; }

        [Display(Name = "Delivery No")]

        public string DeliveryNo { get; set; }


        [Display(Name = "Destination PO")]

        public string DestinationPO { get; set; }
        [Display(Name = "Kimball")]

        public string Kimball { get; set; }

        [Display(Name = "Color Code")]

        public string ColorCode { get; set; }


        [Display(Name = "Contract Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")] //, ApplyFormatInEditMode = true
        public Nullable<System.DateTime> ContractDate { get; set; }

        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [Display(Name = "Master LC")]

        [ForeignKey("MasterLCID")]
        public virtual COM_MasterLC COM_MasterLC { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportInvoiceDetails> ExportInvoiceDetails { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }


    }

    public partial class OpeningBank : BaseModel
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OpeningBankId { get; set; }
        [Display(Name = "Concern Bank")]

        public string OpeningBankName { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Display(Name = "Swift Code")]

        public string SwiftCode { get; set; }

        [Display(Name = "Branch Address")]
        [DataType(DataType.MultilineText)]

        public string BranchAddress { get; set; }
        [Display(Name = "Branch Address 2")]
        [DataType(DataType.MultilineText)]

        public string BranchAddress2 { get; set; }
        [Display(Name = "Phone No")]

        public string PhoneNo { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]

        public string Remarks { get; set; }

        public string AddedBy { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string UpdatedBy { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //public virtual ICollection<COM_MasterLC> COM_MasterLC { get; set; }
        public virtual ICollection<BankAccountNo> BankAccountNos { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }
        //[StringLength(128)]

        //public string comid { get; set; }
        //public Boolean isDelete { get; set; }
    }

    public partial class LienBank : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LienBankId { get; set; }
        [Display(Name = "Buyer / Supplier Bank")]

        public string LienBankName { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        [Display(Name = "Swift Code")]

        public string SwiftCode { get; set; }

        [Display(Name = "Lien Bank Account No")]

        public string LienBankAccountNo { get; set; }


        [Display(Name = "Branch Address")]

        [DataType(DataType.MultilineText)]

        public string BranchAddress { get; set; }

        [DataType(DataType.MultilineText)]

        [Display(Name = "Branch Address 2")]

        public string BranchAddress2 { get; set; }
        [Display(Name = "Phone No")]

        public string PhoneNo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]

        public string Remarks { get; set; }
        public string AddedBy { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string UpdatedBy { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ICollection<COM_MasterLC> COM_MasterLC { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }
        //[StringLength(128)]
        //public string comid { get; set; }
        //public Boolean isDelete { get; set; }
    }

    public partial class Consignee
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsigneeId { get; set; }
        [Display(Name = "Consignee Name")]

        public string ConsigneeName { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        [Display(Name = "Consignee Code")]

        public string Code { get; set; }
        [Display(Name = "Consignee Address")]

        public string BranchAddress { get; set; }
        [Display(Name = "Consignee Phone No")]

        public string PhoneNo { get; set; }
        [Display(Name = "Remarks")]

        public string Remarks { get; set; }

        public string AddedBy { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }


        [StringLength(128)]
        public string userid { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        public Boolean isDelete { get; set; }
    }

    public partial class PortOfLoading : BaseModel
    {
        [Key]
        public int PortOfLoadingId { get; set; }

        [Display(Name = "Port Of Loading")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required]
        public string PortOfLoadingName { get; set; }
        [Display(Name = "Port Code")]

        public string PortCode { get; set; }
        [Display(Name = "Country")]

        public Nullable<int> CountryId { get; set; }
        public virtual Country Countrys { get; set; }
        public string AddedBy { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string UpdatedBy { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }
        [Display(Name = "Master LC")]

        public virtual ICollection<COM_MasterLC> COM_MasterLC { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }
        //[StringLength(128)]

        //public string comid { get; set; }
        //public Boolean isDelete { get; set; }
    }
    public partial class PortOfDischarge : BaseModel
    {
        [Key]
        public int PortOfDischargeId { get; set; }
        [Display(Name = "Port Of Discharge")]
        //[Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required]
        public string PortOfDischargeName { get; set; }
        [Display(Name = "Port Code")]

        public string PortCode { get; set; }
        [Display(Name = "Country")]

        public Nullable<int> CountryId { get; set; }
        public virtual Country Countrys { get; set; }
        public string AddedBy { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string UpdatedBy { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }
        [Display(Name = "Master LC")]

        public virtual ICollection<COM_MasterLC> COM_MasterLC { get; set; }

        //[StringLength(128)]
        //public string userid { get; set; }
        //[StringLength(128)]

        //public string comid { get; set; }
        //public Boolean isDelete { get; set; }
    }

    public partial class COM_GroupLC_Main
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COM_GroupLC_Main()
        {
            this.COM_GroupLC_Sub = new HashSet<COM_GroupLC_Sub>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GroupLCId")]
        public int GroupLCId { get; set; }
        public int BuyerId { get; set; }
        public int CommercialCompanyId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual BuyerInformation BuyerInformation { get; set; }
        public virtual SisterConcernCompany Company { get; set; }
        [Display(Name = "Group LC Ref.")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        //[Index(IsUnique = true)]
        [Required]
        public string GroupLCRefName { get; set; }

        [Display(Name = "Group LC Value [Final Export Value]")]


        [Column(TypeName = "decimal(18,2)")]
        public System.Decimal TotalGroupLCValue { get; set; }

        [Display(Name = "Group LC Value [LC Opening Value]")]

        [Column(TypeName = "decimal(18,2)")]
        public System.Decimal TotalGroupLCValueManual { get; set; }


        [Display(Name = "Group LC Qty")]

        [Column(TypeName = "decimal(18,2)")]
        public System.Decimal TotalGroupLCQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "First Shipment Date")]

        public Nullable<System.DateTime> FirstShipDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Shipment Date")]


        public Nullable<System.DateTime> LastShipDate { get; set; }


        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> comid { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public virtual ICollection<COM_GroupLC_Sub> COM_GroupLC_Sub { get; set; }


        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Display(Name = "Group LC Rev. Amd No.")]
        ////[Index(IsUnique = true)]
        public string GroupLCAmdNo { get; set; }
    }

    public partial class COM_GroupLC_Sub
    {
        [Key, Column(Order = 0)]
        public int GroupLCId { get; set; }
        //[Key, Column(Order = 1)]

        public int MasterLCID { get; set; }
        public virtual COM_MasterLC COM_MasterLC { get; set; }
        public virtual COM_GroupLC_Main COM_GroupLC_Mains { get; set; }


    }

    public partial class COM_ProformaInvoice : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PIId { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(200)]
        [Required(ErrorMessage = "Please Provide PO No")]
        [Display(Name = "PI No.")]
        //[Index(IsUnique = true)]
        public string PINo { get; set; }
        [Display(Name = "PI Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PIDate { get; set; }

        [Display(Name = "PI Receiving Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PIReceivingDate { get; set; }


        [Display(Name = "Concern")]

        public Nullable<int> CommercialCompanyId { get; set; }
        [Display(Name = "Currency")]

        public Nullable<int> CurrencyId { get; set; }
        [Display(Name = "Supplier")]


        public Nullable<int> SupplierId { get; set; }
        [Display(Name = "Import PO No.")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]
        ////[Index(IsUnique = true)]
        public string ImportPONo { get; set; }
        [Display(Name = "File No.")]
        [Column(TypeName = "VARCHAR(MAX)")]
        //[StringLength(500)]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]

        public string FileNo { get; set; }
        [Display(Name = "LCAF")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]
        public string LCAF { get; set; }


        [Display(Name = "Item Group")]
        public Nullable<int> ItemGroupId { get; set; }

        [NotMapped]
        ///[DataType(DataType.MultilineText)]
        public string[] ItemDescArray { get; set; }

        public string ItemDescList { get; set; }



        public virtual ItemGroup ItemGroups { get; set; }


        [Display(Name = "Group LC Contract")]
        public Nullable<int> GroupLCId { get; set; }

        public virtual COM_GroupLC_Main COM_GroupLC_Mains { get; set; }



        [Display(Name = "Item Group")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]

        public string ItemGroupName { get; set; }

        [Display(Name = "Item Desc.")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]

        public string ItemDescription { get; set; }


        [Display(Name = "Item Description")]
        public Nullable<int> ItemDescId { get; set; }
        public virtual ItemDesc ItemDescs { get; set; }



        [Display(Name = "Size")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(40)]

        public string Size { get; set; }

        [Display(Name = "Remarks")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]

        public string Remarks { get; set; }

        [Display(Name = "Qty.")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> ImportQty { get; set; }
        [Display(Name = "Unit")]

        public string UnitMasterId { get; set; }
        [Display(Name = "Import Rate")]


        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> ImportRate { get; set; }
        [Display(Name = "Carton/Roll Qty.")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> CartonRollQty { get; set; }
        [Display(Name = "Total Value")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalValue { get; set; }
        [StringLength(20)]
        public string Addedby { get; set; }
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //[StringLength(20)]
        //public string Updatedby { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }
        //public Nullable<int> comid { get; set; }


        [Display(Name = "HSCode")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(50)]

        public string HSCode { get; set; }




        [Display(Name = "Employee")]

        public Nullable<int> EmployeeId { get; set; }


        [Display(Name = "Merchandiser")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(300)]

        public string MerchandiserName { get; set; }

        [Display(Name = "Rev No.")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(300)]

        public string RevNo { get; set; }

        public virtual SisterConcernCompany Company { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }
        [ForeignKey("SupplierId")]

        public virtual SupplierInformation SupplierInformation { get; set; }

        [ForeignKey("EmployeeId")]

        public virtual Employee Employee { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        //[StringLength(128)]
        //public string useridUpdate { get; set; }

        //public Boolean isDelete { get; set; }

        public virtual ICollection<COM_BBLC_Details> COM_BBLC_Details { get; set; }
        public virtual ICollection<COM_MachinaryLC_Details> COM_MachinaryLC_Details { get; set; }

        public virtual ICollection<COM_ProformaInvoice_Sub> COM_ProformaInvoice_Subs { get; set; }


        [Display(Name = "Payment Terms")]
        public Nullable<int> PaymentTermsId { get; set; }
        public virtual PaymentTerms PaymentTerms { get; set; }

        public Nullable<int> DayListId { get; set; }
        public virtual DayList DayList { get; set; }



        public Nullable<int> OpeningBankId { get; set; }
        public virtual OpeningBank OpeningBanks { get; set; }

        public Nullable<int> BankAccountId { get; set; }
        public virtual BankAccountNo BankAccountNos { get; set; }

        public Nullable<int> LienBankAccountId { get; set; }
        public virtual BankAccountNoLienBank BankAccountNoLienBanks { get; set; }

        public Nullable<int> PINatureId { get; set; }
        public virtual PINature PINatures { get; set; }

    }


    //public partial class ExportRealizationBankInfo
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int BankRefId { get; set; }

    //    [Column(TypeName = "VARCHAR(MAX)")]
    //    [StringLength(200)]
    //    [Required(ErrorMessage = "Please Provide Bank Ref No")]
    //    [Display(Name = "Bank Ref No.")]
    //    //[Index(IsUnique = true)]
    //    public string BankRefNo { get; set; }
    //    [Display(Name = "Bank Ref Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public Nullable<System.DateTime> BankRefDate { get; set; }


    //    [Display(Name = "Buyer Group")]

    //    public Nullable<int> BuyerGroupId { get; set; }

    //    [ForeignKey("BuyerGroupId")]

    //    public virtual BuyerGroup BuyerGroups { get; set; }


    //    [Display(Name = "Concern")]

    //    public Nullable<int> CommercialCompanyId { get; set; }
    //    public virtual SisterConcernCompany Company { get; set; }



    //    [Display(Name = "Currency")]

    //    public Nullable<int> CurrencyId { get; set; }

    //    public virtual Currency Currency { get; set; }

    //    [Display(Name = "File No.")]
    //    [Column(TypeName = "VARCHAR(MAX)")]
    //    [StringLength(500)]
    //    public string FileNo { get; set; }




    //    [Display(Name = "Courier No.")]
    //    [Column(TypeName = "VARCHAR(MAX)")]
    //    [StringLength(200)]
    //    public string CourierNo { get; set; }


    //    [Display(Name = "Courier Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public Nullable<System.DateTime> CourierDate { get; set; }

    //    [Display(Name = "Total Value")]

    //    [Column(TypeName = "decimal(18,2)")]
    //    public Nullable<decimal> TotalValue { get; set; }


    //    [Display(Name = "Realization Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public Nullable<System.DateTime> RealizationDate { get; set; }

    //    [Display(Name = "Realization Amount")]

    //    [Column(TypeName = "decimal(18,2)")]
    //    public Nullable<decimal> RealizationAmount { get; set; }

    //    [Display(Name = "BBLC Margin Amount")]
    //    [Column(TypeName = "decimal(18,2)")]
    //    public Nullable<decimal> BBLCMarginAmount { get; set; }

    //    [Display(Name = "CM Amount")]
    //    [Column(TypeName = "decimal(18,2)")]
    //    public Nullable<decimal> CMAmount { get; set; }

    //    [Display(Name = "Other Bank Charge")]
    //    [Column(TypeName = "decimal(18,2)")]
    //    public Nullable<decimal> OtherBankCharge { get; set; }



    //    [StringLength(20)]
    //    public string Addedby { get; set; }
    //    public Nullable<System.DateTime> DateAdded { get; set; }
    //    [StringLength(20)]
    //    public string Updatedby { get; set; }
    //    public Nullable<System.DateTime> DateUpdated { get; set; }
    //    public Nullable<int> comid { get; set; }








    //    [StringLength(128)]
    //    public string userid { get; set; }

    //    [StringLength(128)]
    //    public string useridUpdate { get; set; }

    //    public Boolean isDelete { get; set; }



    //    [Display(Name = "Remarks")]
    //    [Column(TypeName = "VARCHAR(MAX)")]
    //    [StringLength(500)]

    //    public string Remarks { get; set; }

    //}

    public partial class COM_ProformaInvoice_Sub
    {

        [Key, Column(Order = 0)]
        public int PIId { get; set; }

        //[Key, Column(Order = 1)]
        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Item Desc")]
        public int ItemDescId { get; set; }

        [Display(Name = "Item Description")]

        public virtual ItemDesc ItemDescs { get; set; }

        public virtual COM_ProformaInvoice Com_proformaInvoices { get; set; }

    }

    public partial class COM_CommercialInvoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CommercialInvoiceId")]
        public long CommercialInvoiceId { get; set; }

        [StringLength(30, ErrorMessage = "<img src='/img/icon-info.png' /><p>The {0} must be between {2} and {1} characters long.</p>", MinimumLength = 4)]
        [Display(Name = "Com. Inv. No")]
        [Required]
        [Column(TypeName = "VARCHAR(MAX)")]
        //[Index(IsUnique = true)]
        public string CommercialInvoiceNo { get; set; }
        [Display(Name = "Con. Inv. Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> CommercialInvoiceDate { get; set; }
        [Display(Name = "Concern")]

        //[Range(0, int.MaxValue, ErrorMessage = "Please select Concern")]
        [Required(ErrorMessage = "Please select Concern")]
        [Range(0, int.MaxValue, ErrorMessage = "Please Select Valid Data")]
        public int CommercialCompanyID { get; set; }

        public virtual SisterConcernCompany CommercialCompany { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Please Select Valid Data")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Valid Data")]
        [Display(Name = "BBLC No.")]

        public Nullable<int> BBLCId { get; set; }
        [ForeignKey("BBLCId")]
        public virtual COM_BBLC_Master COM_BBLC_Master { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please Select Valid Data")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Valid Data")]
        [Display(Name = "Supplier")]

        public Nullable<int> SupplierID { get; set; }
        [ForeignKey("SupplierID")]
        public virtual SupplierInformation SupplierInformations { get; set; }
        [Display(Name = "Total PI")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalPI { get; set; }
        //public Nullable<decimal> BBLCValue { get; set; }

        [Display(Name = "Document Receipt Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> DocumentReceiptDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Quantity { get; set; }
        [Display(Name = "Total Value")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalValue { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please Select Valid Data")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Valid Data")]
        [Display(Name = "Currency")]

        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        ///[Range(0, int.MaxValue, ErrorMessage = "Please Select Valid Data")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Valid Data")]
        [Display(Name = "Unit Master")]

        public string UnitMasterId { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }
        public Boolean isDelete { get; set; }

        [Display(Name = "Conversion Rate")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> ConversionRate { get; set; }
        //[StringLength(20, ErrorMessage = "The {0} must be between {2} and {1} characters long.</p>", MinimumLength = 4)]
        [Display(Name = "Item Category")]

        public string ItemGroupName { get; set; }

        //[StringLength(20, ErrorMessage = "<img src='~/Content/img/icon-info.png' /><p>The {0} must be between {2} and {1} characters long.</p>", MinimumLength = 4)]
        [Display(Name = "Goods Description")]

        public string ItemDescription { get; set; }


        [Display(Name = "Item Group")]
        public Nullable<int> ItemGroupId { get; set; }

        [NotMapped]
        ///[DataType(DataType.MultilineText)]
        public string[] ItemDescArray { get; set; }

        public string ItemDescList { get; set; }



        public virtual ItemGroup ItemGroups { get; set; }


        [Display(Name = "Item Description")]
        public Nullable<int> ItemDescId { get; set; }
        public virtual ItemDesc ItemDescs { get; set; }


        //[StringLength(20, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 4)]
        [Display(Name = "Bill Of Lading No")]

        public string BLNo { get; set; }

        [Display(Name = "BL Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> BLDate { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please Select Valid Data")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Valid Data")]
        [Display(Name = "Document Status")]

        public int DocumentStatusId { get; set; }
        [ForeignKey("DocumentStatusId")]
        public virtual DocumentStatus DocumentStatus { get; set; }
        [Display(Name = "Doc. Assesment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> DocumentAssesmentDate { get; set; }
        [Display(Name = "Bill Of Entry No")]

        //[StringLength(20, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 4)]
        public string BillOfEntryNo { get; set; }
        [Display(Name = "Bill Of Entry Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> BillOfEntryDate { get; set; }
        [Display(Name = "Custom Assesment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> CustomAssesmentDate { get; set; }
        [Display(Name = "Vassel ETA Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> VasselETADate { get; set; }
        [Display(Name = "ETB Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> ETBDate { get; set; }
        [Display(Name = "Good Inhouse Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> GoodsInhouseDate { get; set; }

        [Display(Name = "Mother Vassel")]

        public string MotherVassel { get; set; }
        [Display(Name = "Fider Vassel")]

        public string FidderVasel { get; set; }
        public string AddedBy { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ICollection<COM_CommercialInvoice_Sub> COM_CommercialInvoice_Subs { get; set; }



        [Display(Name = "LC Type")]

        public string LCType { get; set; }

        [Display(Name = "LC Type")]

        public Nullable<int> CommercialLCTypeId { get; set; }
        [ForeignKey("CommercialLCTypeId")]
        public virtual CommercialLCType CommercialLCTypes { get; set; }


        [Display(Name = "Regular LC No.")]

        public Nullable<int> MachinaryLCId { get; set; }
        [ForeignKey("MachinaryLCId")]
        public virtual COM_MachinaryLC_Master COM_MachinaryLC_Master { get; set; }


        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        ////[Index(IsUnique = true)]
        public string Remarks { get; set; }
    }


    public partial class COM_CommercialInvoice_Sub
    {

        [Key, Column(Order = 0)]
        public long CommercialInvoiceId { get; set; }

        //[Key, Column(Order = 1)]
        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Item Desc")]
        public int ItemDescId { get; set; }

        [Display(Name = "Item Description")]

        public virtual ItemDesc ItemDescs { get; set; }

        public virtual COM_CommercialInvoice COM_CommercialInvoices { get; set; }

    }

    public partial class COM_DocumentAcceptance_Master
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BillsOfExchangeId")]
        public long BillsOfExchangeId { get; set; }

        [Display(Name = "DA No.")]
        [StringLength(30)]
        [Required(ErrorMessage = "Please Provide DA No")]
        //[Index(IsUnique = true)]
        public string BillOfExchangeRef { get; set; }
        [Display(Name = "Bill Date")]

        public Nullable<System.DateTime> BillDate { get; set; }

        [Display(Name = "Bill Maturity Date")]

        public Nullable<System.DateTime> BillMaturityDate { get; set; }

        [Display(Name = "Bill Payment Date")]

        public Nullable<System.DateTime> BillPaymentDate { get; set; }

        [Display(Name = "Concern")]

        public Nullable<int> CommercialCompanyId { get; set; }
        public virtual SisterConcernCompany CommercialCompanys { get; set; }

        public Nullable<int> BuyerId { get; set; }
        public virtual BuyerInformation BuyerInformation { get; set; }
        [Display(Name = "Supplier")]

        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformations { get; set; }
        public Nullable<int> BBLCId { get; set; }
        [ForeignKey("BBLCId")]

        public virtual COM_BBLC_Master COM_BBLC_Master { get; set; }

        public Nullable<int> GroupLCId { get; set; }
        [ForeignKey("GroupLCId")]

        public virtual COM_GroupLC_Main COM_GroupLC_Main { get; set; }

        public string MasterLCRef { get; set; }
        [Display(Name = "BBLC Amount")]

        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalBBLCAmount { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        [Display(Name = "Paid Amount")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> PaidAmount { get; set; }
        [Display(Name = "Payable Amount")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> PayableAmount { get; set; }
        [Display(Name = "Previous Accepted")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> AcceptedAmount { get; set; }
        [Display(Name = "New CI Amount")]

        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> NewCIAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> ConversionRate { get; set; }

        //public string ApprovalPerson { get; set; }
        public Nullable<int> ApprovedById { get; set; }
        public virtual ApprovedBy ApprovedBy { get; set; }

        public Nullable<System.DateTime> DateApproval { get; set; }
        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_DocumentAcceptance_Details> COM_DocumentAcceptance_Details { get; set; }
    }

    public partial class COM_DocumentAcceptance_Details
    {
        [Key, Column(Order = 0)]
        public long BillsOfExchangeId { get; set; }
        //[Key, Column(Order = 1)]


        [Display(Name = "CI No.")]

        public Nullable<long> CommercialInvoiceId { get; set; }
        [ForeignKey("CommercialInvoiceId")]

        public virtual COM_CommercialInvoice COM_CommercialInvoice { get; set; }

        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [Display(Name = "Document Acceptance")]
        public virtual COM_DocumentAcceptance_Master COM_DocumentAcceptance_Master { get; set; }


    }

    public partial class COM_BBLC_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BBLCId { get; set; }
        [Required]
        [Display(Name = "BBLC NO")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        //[Index(IsUnique = true)]
        public string BBLCNo { get; set; }

        [Display(Name = "UD NO")]
        public string UDNo { get; set; }

        [Display(Name = "Amendment NO")]
        public string AmdNo { get; set; }

        [Display(Name = "Concern")]
        public Nullable<int> CommercialCompanyId { get; set; }

        [Display(Name = "ShipMode")]
        public Nullable<int> ShipModeId { get; set; }
        [Display(Name = "Group LC")]
        public Nullable<int> GroupLCId { get; set; }
        [Display(Name = "Total Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }
        [Display(Name = "Tenor")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Tenor { get; set; }
        [Display(Name = "Payment Term")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Payment Term")]
        public Nullable<int> PaymentTermsId { get; set; }
        public virtual PaymentTerms PaymentTermss { get; set; }


        [Display(Name = "Day List Term")]
        public Nullable<int> DayListId { get; set; }
        public virtual DayList daylists { get; set; }


        public string Insurance { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "Port Of Loading")]
        public int PortOfLoadingId { get; set; }
        [Display(Name = "Port Of Discharge")]
        public Nullable<int> PortOfDischargeId { get; set; }


        [Display(Name = "Concern Bank")]
        public Nullable<int> OpeningBankId { get; set; }

        [Display(Name = "Supplier Bank")]
        public Nullable<int> LienBankId { get; set; }

        [Display(Name = "Incoterm")]
        public Nullable<int> TradeTermId { get; set; }
        public virtual TradeTerm TradeTerms { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LC Opening Date")]
        public DateTime LcOpeningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "UD Date")]
        public Nullable<DateTime> UDDate { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "First Shipment Date")]
        public Nullable<DateTime> FirstShipmentDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Shipment Date")]
        public Nullable<DateTime> LastShipmentDate { get; set; }


        [Display(Name = "Final Destination")]
        public int DestinationID { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "ConvertRate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConvertRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "BBLC Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BBLCValue { get; set; }
        [Display(Name = "BBLC Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BBLCQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Margin [%]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GroupLCAverage { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }


        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Increase")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal IncreaseValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Decrease")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        public decimal DecreaseValue { get; set; }
        [Display(Name = "Final Value")]
        [Column(TypeName = "decimal(18,2)")]

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        public decimal NetValue { get; set; }



        [Display(Name = "BBLC Print Doc Ref")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(50)]

        public string BBLCPrintDocRef { get; set; }

        [Display(Name = "BBLC Print Doc Date")]
        public Nullable<System.DateTime> BBLCPrintDocDate { get; set; }

        public virtual SisterConcernCompany Company { get; set; }

        [Display(Name = "Supplier")]
        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformation { get; set; }
        public virtual ShipMode ShipMode { get; set; }
        [ForeignKey("GroupLCId")]
        public virtual COM_GroupLC_Main COM_GroupLC_Main { get; set; }

        //public virtual COM_MasterLC COM_MasterLC { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual PortOfLoading PortOfLoading { get; set; }
        public virtual PortOfDischarge PortOfDischarge { get; set; }

        public virtual Destination Destination { get; set; }

        public virtual OpeningBank OpeningBank { get; set; }
        public virtual LienBank LienBank { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_BBLC_Details> COM_BBLC_Details { get; set; }
        public virtual ICollection<COM_CommercialInvoice> COM_CommercialInvoice { get; set; }

        public string ApprovalPerson { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> DateApproval { get; set; }
        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        [Display(Name = "Remarks")]

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }



        [StringLength(128)]
        [Display(Name = "Update Ref No")]

        public string useridUpdateRefNo { get; set; }

        public string RefNo { get; set; }
        public string PrintDate { get; set; }
        public string Percentage { get; set; }



        [Display(Name = "LC For :")]
        public Nullable<int> ItemGroupId { get; set; }


    }

    public partial class COM_BBLC_Details
    {
        [Key, Column(Order = 0)]
        public int BBLCId { get; set; }
        //[Key, Column(Order = 1)]
        ////[Index(IsUnique = true)]
        public int PIId { get; set; }

        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [Display(Name = "Master BBLC")]


        public virtual COM_BBLC_Master COM_BBLC_Master { get; set; }

        [Display(Name = "Proforma Invoice")]

        public virtual COM_ProformaInvoice COM_ProformaInvoice { get; set; }
        //public virtual ICollection<COM_GroupLC_Main> COM_GroupLC_Main { get; set; }
    }

    public class COM_CommercialInvoice_Single_List
    {
        public COM_CommercialInvoice COM_CommercialInvoice { get; set; }
        public List<COM_CommercialInvoice> COM_CommercialInvoice_List { get; set; }
    }

    public partial class WorkOrderMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkOrderId { get; set; }
        [Display(Name = "Company")]

        public int CommercialCompanyId { get; set; }
        public virtual SisterConcernCompany CommercialCompany { get; set; }
        [Display(Name = "WorkOrder No")]
        public string WorkOrderNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "WO.Date")]

        public System.DateTime WorkOrderDate { get; set; }
        [Display(Name = "To Company")]
        //[Display(Name = "supplier")]
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformation { get; set; }
        [Display(Name = "To Person")]

        public string ToPerson { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Agreement Date")]

        public System.DateTime AgreementDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Delivery Date")]

        public System.DateTime DeliveryDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Service Start")]

        public Nullable<System.DateTime> ServiceContractStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Service End")]

        public Nullable<System.DateTime> ServiceContractEndDate { get; set; }

        [Display(Name = "Currency")]

        public long CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        [Display(Name = "Conv. Rate")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> ConversionRate { get; set; }
        [Display(Name = "Workorder Type")] ////fahad confusion

        public string WorkOrderType { get; set; }



        [Display(Name = "Subject")]

        public string Subject { get; set; }
        public string Body { get; set; }
        [Display(Name = "Pay. Terms")]

        public string PaymentTerms { get; set; }
        [Display(Name = "Others Terms")]

        public string OtherTerms { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "WorkOrder Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal WorkOrderQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "WorkOrder Rate")]
        public decimal WorkOrderRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Total Amount")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Sales Tax")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal SalesTax { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Other Exp")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal OtherExp { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "WorkOrder Amt")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal WorkOrderAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Advance Pay")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> AdvancePayment { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Net Payable")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> NetPayable { get; set; }
        [Display(Name = "Remarks")]

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public bool IsLocked { get; set; }
        [Display(Name = "Service Contract")]

        public string ServiceContract { get; set; }
        [Display(Name = "WO Status")]
        public int WorkOrderStatusId { get; set; }
        public virtual WorkorderStatus WorkorderStatus { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "WO Details")]
        public string WODetails { get; set; }
        [Display(Name = "Ship To")]
        public string ShipTo { get; set; }
        [Display(Name = "Shipping")]
        public string Shipping { get; set; }
        [Display(Name = "Total")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Total { get; set; }
        [Display(Name = "Approved By")]
        public int ApprovedById { get; set; }
        [ForeignKey("ApprovedById")]
        public virtual ApprovedBy ApprovedBy { get; set; }

        [Display(Name = "Recommened By")]
        public int RecommenedById { get; set; }
        [ForeignKey("RecommenedById")]
        public virtual ApprovedBy RecommenedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Approved Date")]
        public Nullable<System.DateTime> DateApproval { get; set; }

        [Display(Name = "Item Group")]
        public Nullable<int> ItemGroupId { get; set; }
        [NotMapped]
        ///[DataType(DataType.MultilineText)]
        public string[] ItemDescArray { get; set; }
        public string ItemDescList { get; set; }
        public virtual ItemGroup ItemGroups { get; set; }

        [Display(Name = "Item Group")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]
        public string ItemGroupName { get; set; }

        [Display(Name = "Item Desc.")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(500)]
        public string ItemDescription { get; set; }


        [Display(Name = "Item Description")]
        public Nullable<int> ItemDescId { get; set; }
        public virtual ItemDesc ItemDescs { get; set; }

        [Display(Name = "Added By")]
        public string AddedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Added")]
        public Nullable<System.DateTime> DateAdded { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Updated")]
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }
        public Boolean isDelete { get; set; }

        public virtual ICollection<COM_MachineryLCDetails> COM_MachineryLCDetailss { get; set; }
    }

    public partial class ExportInvoiceMaster : BaseModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExportInvoiceMaster()
        {
            this.ExportInvoiceDetails = new HashSet<ExportInvoiceDetails>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Display(Name = "Invoice No")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        //[Index(IsUnique = true)]
        [Required]
        public string InvoiceNo { get; set; }

        [Display(Name = "Invoice Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime InvoiceDate { get; set; }

        [Display(Name = "Delivery Term")]
        public string DeliveryTerm { get; set; }
        [Display(Name = "Total Shipped")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal TotalShipped { get; set; }

        [Display(Name = "Balance Shipped")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal BalanceShip { get; set; }

        [Display(Name = "MasterLCId")]
        public Nullable<int> MasterLCId { get; set; }
        public virtual COM_MasterLC COM_MasterLC { get; set; }

        [Display(Name = "Buyer")]
        public Nullable<int> BuyerId { get; set; }
        public virtual BuyerInformation BuyerInformation { get; set; }


        [Display(Name = "Buyer Group")]
        public Nullable<int> BuyerGroupId { get; set; }
        public virtual BuyerGroup BuyerGroups { get; set; }


        [Display(Name = "Manufacture Company")]
        public Nullable<int> ManufactureId { get; set; }
        [ForeignKey("ManufactureId")]
        public virtual SisterConcernCompany ComercialCompanyss { get; set; }

        [Display(Name = "Notify Party 1st")]
        public Nullable<long> FirstNotifyPartyId { get; set; }
        [ForeignKey("FirstNotifyPartyId")]
        public virtual NotifyParty NotifyPartyFirst { get; set; }

        [Display(Name = "Notify Party 2nd")]
        public Nullable<long> SecoundNotifyPartyId { get; set; }
        [ForeignKey("SecoundNotifyPartyId")]
        public virtual NotifyParty NotifyPartySecound { get; set; }


        [Display(Name = "Notify Party 3rd")]
        public Nullable<long> ThirdNotifyPartyId { get; set; }
        [ForeignKey("ThirdNotifyPartyId")]
        public virtual NotifyParty NotifyPartyThird { get; set; }


        [Display(Name = "Exporter")]

        public Nullable<int> CommercialCompanyId { get; set; }
        [ForeignKey("CommercialCompanyId")]
        public virtual SisterConcernCompany CommercialCompany { get; set; }

        [Display(Name = "Port Of Loading")]
        public Nullable<int> PortOfLoadingId { get; set; }
        public virtual PortOfLoading PortOfLoading { get; set; }
        [Display(Name = "Port Of Discharge")]
        public Nullable<int> PortOfDischargeId { get; set; }
        public virtual PortOfDischarge PortOfDischarge { get; set; }

        [Display(Name = "Final Destination")]
        public Nullable<int> DestinationId { get; set; }
        public virtual Destination Destination { get; set; }

        [Display(Name = "ShipMode")]
        public Nullable<int> ShipModeId { get; set; }
        public virtual ShipMode ShipMode { get; set; }

        [Display(Name = "Cond. Of Sales")]
        public Nullable<int> TradeTermId { get; set; }
        public virtual TradeTerm Tradeterms { get; set; }

        [Display(Name = "Forwarder")]
        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformation { get; set; }

        [Display(Name = "Exfactory Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ExfactoryDate { get; set; }

        [Display(Name = "Onboard Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OnboardDate { get; set; }

        [Display(Name = "Exp No")]
        [StringLength(100)]
        public string ExpNo { get; set; }

        [Display(Name = "Exp Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ExpDate { get; set; }

        [Display(Name = "BL No")]
        [StringLength(100)]
        public string BLNo { get; set; }

        [Display(Name = "BL Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BLDate { get; set; }




        [Display(Name = "BOOKING No")]
        public string BookingNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Booking Date")]
        public Nullable<System.DateTime> BookingDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Goods Description")]
        [StringLength(500)]
        public string GoodsDescription { get; set; }

        [Display(Name = "Carton Measurement")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]

        public string CartonMeasurement { get; set; }

        [Display(Name = "Vessel Name")]
        [StringLength(500)]
        public string VesselName { get; set; }

        [Display(Name = "Shipment Authorization")]
        [StringLength(500)]
        public string ShipmentAuthorization { get; set; }

        [Display(Name = "Voyage No")]
        [StringLength(500)]
        public string VoyageNo { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Main Marks")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string MainMark { get; set; }

        [Display(Name = "Net Weight")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> NetWeight { get; set; }

        [Display(Name = "Gross Weight")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> GrossWeight { get; set; }
        [Display(Name = "CBM")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> CBM { get; set; }
        [Display(Name = "Number Of Carton")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalCartonQty { get; set; }
        [Display(Name = "Packing Type")]
        public string PackingType { get; set; }

        [Display(Name = "Payment Terms Manual")]
        public string PaymentTermsManual { get; set; }

        [Display(Name = "Session")]
        public string Session { get; set; }

        [Display(Name = "Total Invoice Qty")]
        [Column(TypeName = "decimal(18,2)")]

        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public Nullable<decimal> TotalInvoiceQty { get; set; }

        [Column(TypeName = "decimal(18,2)")]

        [Display(Name = "Total Invoice Qty [Pcs]")]
        public Nullable<decimal> TotalInvoiceQtyPcs { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> Rate { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        [Display(Name = "Total Value")]
        public Nullable<decimal> TotalValue { get; set; }

        [Display(Name = "Discount")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> Discount { get; set; }

        [Display(Name = "Net Value")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> NetValue { get; set; }


        [Display(Name = "CMP Percentage")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> CMPPercentage { get; set; }

        [Display(Name = "CMP Value")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> CMPValue { get; set; }


        [Display(Name = "Cargo Handover Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> CargoHandoverDate { get; set; }

        public string BankAccNo { get; set; }

        [StringLength(50)]
        public string Addedby { get; set; }
        //[StringLength(128)]

        //public string comid { get; set; }
        //[StringLength(128)]
        //[Display(Name = "Entry User")]

        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }


        //public Nullable<System.DateTime> DateAdded { get; set; }

        //[StringLength(50)]
        //public string Updatedby { get; set; }

        //public Nullable<System.DateTime> DateUpdated { get; set; }

        //public virtual ICollection<ExportInvoiceDetails> ExportInvoiceDetails { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportInvoiceDetails> ExportInvoiceDetails { get; set; }




        [Display(Name = "Exporter Bank")]
        //[Required(ErrorMessage = "Please select Exporter Bank")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please select Exporter Bank")]
        public Nullable<int> OpeningBankId { get; set; }
        public virtual OpeningBank OpeningBank { get; set; }

        [Display(Name = "Bank Account No")]
        //[Required(ErrorMessage = "Please select Account No.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please select Bank Account No.")]
        public Nullable<int> BankAccountId { get; set; }


        public virtual BankAccountNo BankAccountNos { get; set; }

    }

    public partial class ExportInvoiceDetails
    {

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExportInvoiceDetails()
        {
            this.ExportInvoicePackingLists = new HashSet<ExportInvoicePackingList>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key, Column(Order = 0)]
        public int ExportInvoiceDetailsId { get; set; }

        public int InvoiceId { get; set; }



        ////[Key, Column(Order = 1)]
        public int MasterLCDetailsID { get; set; }
        [NotMapped]
        public Boolean isDelete { get; set; }



        ////[Key, Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ExportInvoiceDetailsId { get; set; }






        [Display(Name = "Style No")]
        public string StyleNo { get; set; }

        [Display(Name = "Export PoNo")]
        public string ExportPoNo { get; set; }

        [Display(Name = "Destination")]
        public string Destination { get; set; }

        //public Nullable<int> DestinationId { get; set; }
        //public virtual Destination Destination { get; set; }

        [Display(Name = "LC Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> LCQty { get; set; }
        [Display(Name = "UOM")]
        public string UnitMasterId { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }

        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Invoice Qty")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> InvoiceQty { get; set; }

        [Display(Name = "Invoice Rate")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> InvoiceRate { get; set; }

        [Display(Name = "Invoice Value")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> InvoiceValue { get; set; }
        [Display(Name = "Shipment Date")]

        public Nullable<System.DateTime> ShipmentDate { get; set; }

        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }


        [Display(Name = "Net Weight")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> NetWeightLine { get; set; }

        [Display(Name = "Gross Weight")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> GrossWeightLine { get; set; }
        [Display(Name = "CBM")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> CBMLine { get; set; }
        [Display(Name = "Number of Carton")]
        public Nullable<int> CartonQty { get; set; }


        [Display(Name = "Document Send Date")]

        public Nullable<System.DateTime> DocumentSendDate { get; set; }
        [Display(Name = "Bill Receive Date")]

        public Nullable<System.DateTime> BillReceiveDate { get; set; }


        [Display(Name = "Master BBLC")]


        public virtual ExportInvoiceMaster ExportInvoiceMasters { get; set; }
        //public int MasterLCDetailsID { get; set; }

        public virtual COM_MasterLC_Details COM_MasterLC_Detail { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportInvoicePackingList> ExportInvoicePackingLists { get; set; }



        [Display(Name = "Color Code")]
        public string ColorCode { get; set; }

        [Display(Name = "PO Date")]
        public Nullable<System.DateTime> PODate { get; set; }


        [Display(Name = "Box Length")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> BoxLength { get; set; }

        [Display(Name = "Box Width")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> BoxWidth { get; set; }

        [Display(Name = "Box Height")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> BoxHeight { get; set; }

        [Display(Name = "SL NO")]
        public Nullable<int> SLNO { get; set; }

        [Display(Name = "Factor")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> InvoiceFactor { get; set; }

        [Display(Name = "Invoice Qty [Pcs]")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> InvoiceQtyInPcs { get; set; }



    }

    public partial class ExportInvoicePackingList
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key, Column(Order = 0)]
        public int ExportInvoicePackingListId { get; set; }

        public int ExportInvoiceDetailsId { get; set; }
        [Display(Name = "Export PoNo")]
        public string ExportPoNo { get; set; }

        [Display(Name = "Number of Carton")]
        public Nullable<int> CartonQty { get; set; }


        [Display(Name = "Gross Weight")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> GrossWeightLinePacking { get; set; }

        [Display(Name = "Net Weight")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> NetWeightLinePacking { get; set; }


        [Display(Name = "Box Length")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> BoxLengthLinePacking { get; set; }

        [Display(Name = "Box Width")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> BoxWidthLinePacking { get; set; }

        [Display(Name = "Box Height")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> BoxHeightLinePacking { get; set; }



        [Display(Name = "CBM")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> CBMLinePacking { get; set; }


        [Display(Name = "SL NO")]
        public Nullable<int> SLNOLinePacking { get; set; }

        [NotMapped]
        public Boolean isDelete { get; set; }



        [Display(Name = "Item Number")]
        public string ItemNumber { get; set; }

        [Display(Name = "UPC Number")]
        public string UPCNumber { get; set; }

        [Display(Name = "Qty")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> Qty { get; set; }

        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Display(Name = "Total Value")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalValue { get; set; }


        public virtual ExportInvoiceDetails ExportInvoiceDetailss { get; set; }
        //public int MasterLCDetailsID { get; set; }


    }

    public partial class ExportRealizationMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RealizationId { get; set; }

        [Display(Name = "Realization No")]
        public string RealizationNo { get; set; }

        [Display(Name = "Realization Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RealizationDate { get; set; }


        [Display(Name = "MasterLC")]
        public Nullable<int> MasterLCId { get; set; }
        public virtual COM_MasterLC COM_MasterLC { get; set; }


        [Display(Name = "Exporter")]
        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformation { get; set; }

        [Display(Name = "Buyer")]
        public Nullable<int> BuyerId { get; set; }
        public virtual BuyerInformation BuyerInformation { get; set; }

        [Display(Name = "Total Export Invoice")]
        public Nullable<int> TotalExportInvoice { get; set; }

        [Display(Name = "Total Order Qty")]
        public Nullable<int> TotalOrderQty { get; set; }

        [Display(Name = "Total Value")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalValue { get; set; }

        [Display(Name = "Realized Amount")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> RealizedAmount { get; set; }

        [Display(Name = "Pending Value")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> PendingValue { get; set; }
        public string Addedby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }

        public virtual ICollection<ExportRealizationDetails> ExportRealizationDetails { get; set; }
    }

    public partial class ExportRealizationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] ///, Column(Order = 0)
        public int RealizationDetailsId { get; set; }
        ////[Key, Column(Order = 1)]
        public Nullable<int> RealizationId { get; set; }

        // [Display(Name = "Export Invoice No.")]
        public Nullable<int> InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual ExportInvoiceMaster ExportInvoiceMaster { get; set; }

        [Display(Name = "Total Quantity")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalQuantity { get; set; }

        [Display(Name = "Total Value")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public Nullable<decimal> TotalValue { get; set; }
        public string Addedby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ExportRealizationMaster ExportRealizationMaster { get; set; }

    }

    public partial class NotifyParty
    {
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public NotifyParty()
        //{
        //    ExportInvoiceMasters = new HashSet<ExportInvoiceMaster>();
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NotifyPartyId { get; set; }
        [Display(Name = "Notify Party Name")]
        public string NotifyPartyName { get; set; }
        [Display(Name = "Notify Party Search Name")]
        public string NotifyPartyNameSearch { get; set; }

        [DataType(DataType.MultilineText)]

        public string Address1 { get; set; }
        [DataType(DataType.MultilineText)]

        public string Address2 { get; set; }

        public string PhoneNo { get; set; }
        public string Email { get; set; }

        public Nullable<int> BuyerId { get; set; }



        public virtual BuyerInformation BuyerInformations { get; set; }
        public Nullable<int> CountryId { get; set; }
        public virtual Country Countrys { get; set; }
        public Nullable<int> PortOfDischargeId { get; set; }
        public virtual PortOfDischarge PortOfDischarge { get; set; }
        [Display(Name = "Shop Code")]

        public string ShopCode { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Shipped To")]

        public string ShippedTo { get; set; }

        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }


        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ExportInvoiceMaster> ExportInvoiceMasters { get; set; }
    }


    public partial class COM_MachineryLCMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MachineryLCId { get; set; }

        public string LCNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LC Date")]
        public Nullable<System.DateTime> LCDate { get; set; }

        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformation { get; set; }

        [Display(Name = "Payment Type")]
        public Nullable<int> PaymentTermsId { get; set; }
        public virtual PaymentTerms PaymentTerms { get; set; }
        [Display(Name = "Payment Days")]
        public Nullable<int> DefferredPaymentDays { get; set; } //comes from payment
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ship Date")]
        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<int> InsuranceCompanyId { get; set; }
        public string InsurancePayStatus { get; set; }
        public string ImportBillNo { get; set; }
        public Nullable<System.DateTime> ImportBillDate { get; set; }
        public Nullable<System.DateTime> BillMacturityDate { get; set; }
        public Nullable<System.DateTime> BillPayDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBillAmount { get; set; }
        public string Addedby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }



        public ICollection<COM_MachineryLCDetails> COM_MachineryLCDetailses { get; set; }
    }

    public partial class COM_MachineryLCDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MachineryLCDetailId { get; set; }
        public int MachineryLCId { get; set; }
        [ForeignKey("MachineryLCId")]
        public virtual COM_MachineryLCMaster COM_MachineryLCMaster { get; set; }

        public int WorkOrderId { get; set; }
        [ForeignKey("WorkOrderId")]
        public virtual WorkOrderMaster WorkOrderMaster { get; set; }
        public string WorkOrderRef { get; set; }
        //public string Items { get; set; }
        //public decimal Rate { get; set; }      
        //public string Addedby { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public string Updatedby { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> DateUpdated { get; set; }
        //[StringLength(128)]
        //public string userid { get; set; }
        //public int comid { get; set; }
        //public Boolean isDelete { get; set; }
    }



    public partial class ExportRealization_Master
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExportRealization_Master()
        {
            this.ExportRealization_Details = new HashSet<ExportRealization_Details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RealizationId { get; set; }

        [Display(Name = "Export Form Number")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        public string ExportFormNo { get; set; }

        [Display(Name = "File Number")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        public string FileNumber { get; set; }



        [Display(Name = "MasterLCID")]
        public int MasterLCID { get; set; }
        public virtual COM_MasterLC COM_MasterLC { get; set; }

        [Display(Name = "FBP Number")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        public string FBPNo { get; set; }

        [Display(Name = "FBP Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FBPDate { get; set; }



        //[Display(Name = "FBP Value")]
        //[DisplayFormat(DataFormatString = "{0:#,#.00}")]
        //public decimal FBPValue { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DueDate { get; set; }

        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PaymentDate { get; set; }


        [Display(Name = "Bank Reference")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        public string BankRef { get; set; }

        [Display(Name = "COURIER NO")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        public string CourierNo { get; set; }

        [Display(Name = "Courier Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CourierDate { get; set; }


        [Display(Name = "Realization Amount")]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RealizationAmount { get; set; }

        [Display(Name = "Realization Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RealizationDate { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalInvoiceQty { get; set; }
        //[DataType("decimal(18, 4)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceivingVlaue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BankCharge { get; set; }


        [Display(Name = "Remarks")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        public string Remarks { get; set; }


        [StringLength(50)]
        public string Addedby { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }


        public Nullable<System.DateTime> DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }

        //public virtual ICollection<ExportInvoiceDetails> ExportInvoiceDetails { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportRealization_Details> ExportRealization_Details { get; set; }

    }

    public partial class ExportRealization_Details
    {

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int ExportRealizationDetailsId { get; set; }

        public int RealizationId { get; set; }
        public virtual ExportRealization_Master ExportRealization_Masters { get; set; }

        public int InvoiceId { get; set; }
        public virtual ExportInvoiceMaster ExportInvoiceMaster { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalQty { get; set; }

        //public decimal Rate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceivingValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BankCharge { get; set; }


        //public Nullable<System.DateTime> DocumentSendDate { get; set; }
        //public Nullable<System.DateTime> DocumentReceivedDate { get; set; }

        [NotMapped]
        public Boolean isDelete { get; set; }

        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        // public virtual ICollection<ExportInvoiceMaster> ExportInvoiceMasters { get; set; }
    }



}