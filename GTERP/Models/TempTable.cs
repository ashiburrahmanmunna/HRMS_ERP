using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class StyleInformations_Temp
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StyleId { get; set; }

        [Display(Name = "Style Name")]
        [StringLength(200)]
        public string StyleName { get; set; }
        [Display(Name = "Style Code")]

        public string StyleCode { get; set; }
        [Display(Name = "Concern")]

        public Nullable<int> CommercialCompanyId { get; set; }
        [Display(Name = "Concern")]

        public string CompanyName { get; set; }


        [Display(Name = "Buyer")]

        public Nullable<int> BuyerId { get; set; }
        [Display(Name = "Buyer")]

        public string BuyerName { get; set; }

        [Display(Name = "Order Qty")]

        public Nullable<int> OrderQty { get; set; }
        [Display(Name = "UOM")]

        public string UnitMasterId { get; set; }
        [Display(Name = "UOM")]

        public string UnitName { get; set; }
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
        [Display(Name = "Status")]

        public string StyleStatusName { get; set; }
        [Display(Name = "First Ship Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> FirstShipDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Ship Date")]

        public Nullable<System.DateTime> LastShipDate { get; set; }
        [Display(Name = "Product Category")]

        public Nullable<long> ProductCategoryId { get; set; }

        [Display(Name = "Product Category")]

        public string CategoryName { get; set; }

        [Display(Name = "Brand")]


        public Nullable<int> BrandInfoId { get; set; }
        [Display(Name = "Brand")]

        public string BrandName { get; set; }
        [Display(Name = "Product Group")]

        public Nullable<int> ProductGroupId { get; set; }
        [Display(Name = "Product Group")]

        public string ProductGroupName { get; set; }
        //public virtual BrandInfo BrandInfo { get; set; }
        //public virtual BuyerInformation BuyerInformation { get; set; }
        //public virtual SisterConcernCompany Company { get; set; }
        //public virtual Currency Currency { get; set; }
        //public virtual ProductCategory ProductCategory { get; set; }
        //public virtual UnitMaster UnitMaster { get; set; }
        //public virtual StyleStatus StyleStatus { get; set; }
        //public virtual ProductGroup ProductGroup { get; set; }
        [StringLength(128)]

        public string comid { get; set; }

    }

}