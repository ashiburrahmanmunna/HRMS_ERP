using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class Category : BaseModel
    {

        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category Code")]
        public string CategoryCode { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Name { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Category Description")]
        public string CategoryDescription { get; set; }

        [Display(Name = "Category Link")]
        public string LinkAdd { get; set; }


        [Display(Name = "Category Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] CategoryImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]

        [Display(Name = "Category Image [Folder]")]

        public string CategoryImagePath { get; set; }

        [Display(Name = "Category Files Extension")]
        public string CategoryFileExtension { get; set; }


        //[Required]
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


        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }


        [Display(Name = "Category")]

        public virtual ICollection<Product> vProducts { get; set; }
    }


    public class SubCategory
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubCategory()
        {
            this.Products = new HashSet<Product>();
            //this.ProductTypes = new HashSet<ProductType>();
        }

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }
        [Display(Name = "Sub Category Code")]

        public string SubCategoryCode { get; set; }
        [Display(Name = "Category")]

        public Nullable<int> CategoryId { get; set; }
        public virtual Category Category { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]



        [Display(Name = "Sub Category Name")]

        public string SubCategoryName { get; set; }

        [Display(Name = "Sub Category Link")]

        public string LinkAdd { get; set; }


        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Sub Category Description")]
        public string SubCategoryDescription { get; set; }



        [Display(Name = "Sub Category Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] SubCategoryImage { get; set; }
        [Display(Name = "Sub Category Image [Folder]")]

        public string SubCategoryImagePath { get; set; }

        [Display(Name = "Category Files Extension")]
        public string SubCategoryFileExtension { get; set; }


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


        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }


        public virtual ICollection<Product> Products { get; set; }
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        ///public virtual ICollection<ProductType> ProductTypes { get; set; }

    }


    public class ProductMainGroup
    {

        public int ProductMainGroupId { get; set; }

        [Required]
        [Display(Name = "Product Main Group Code")]
        public string ProductMainGroupCode { get; set; }

        [Required]
        [Display(Name = "Product Main Group")]
        public string Name { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "ProductGroup Description")]
        public string ProductMainGroupDescription { get; set; }

        [Display(Name = "Serial No")]
        public int SLNo { get; set; }

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


        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }


        [Display(Name = "Product Group")]

        public virtual ICollection<Product> vProducts { get; set; }
    }






}