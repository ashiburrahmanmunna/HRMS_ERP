using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Supplier : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }


        //[Required]
        //[StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]

        [Display(Name = "Code")]
        public string SupplierCode { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string SupplierName { get; set; }



        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Name Bangla")]
        public string SupplierNameB { get; set; }



        [Display(Name = "Tin No")]
        [StringLength(20)]
        public string TinNo { get; set; }



        //[Required(ErrorMessage = "Field can't be empty")]

        //[Required]
        //[StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public string EmailId { get; set; }

        //[Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }


        //[Required]
        //[StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        //[StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        //[StringLength(5, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        //[Required]
        //[StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]

        public string PhoneNo { get; set; }

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


        [Display(Name = "Opening Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal OpBalance { get; set; }

        [Display(Name = "Country")]
        public virtual ICollection<PurchaseMain> vCountryPurchase { get; set; }

        [Display(Name = "Country")]
        public virtual Country vSupplierCountry { get; set; }


        [Display(Name = "Police Station")]
        public int? PStationId { get; set; }
        [Display(Name = "Police Station")]
        [ForeignKey("PStationId")]

        public virtual Cat_PoliceStation vCat_PoliceStation { get; set; }



        //[Required]
        [Display(Name = "District")]
        public int? DistId { get; set; }
        [Display(Name = "District")]
        [ForeignKey("DistId")]

        public virtual Cat_District vCat_District { get; set; }


        public int? SLNo { get; set; }

    }

    public class SupplierContact
    {

        [Key, Column(Order = 0)]
        public int SupplierContactId { get; set; }

        //[Key, Column(Order = 1)]
        //public string ItemName { get; set; }
        //public int CategoryId { get; set; }


        public int SupplierId { get; set; }
        //public int ProductId { get; set; }


        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string ContactEmailNo { get; set; }



        public virtual Supplier vSupplier { get; set; }

        //public virtual Product vProductName { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }
    public class TransferVariable : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TVId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string SINo { get; set; }
        //public string ComId { get; set; }
        public string? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string? VandorName { get; set; }
        public string PresentDepartment { get; set; }
        public string? ProposedDepartment { get; set; }
        public string PresentDesignation { get; set; }
        public string? ProposedDesignation { get; set; }
        public string PresentRole { get; set; }
        public string? ProposedRole { get; set; }
        public string PresentCostHead { get; set; }
        public string? ProposedCostHead { get; set; }
        public string PresentAltitudeCode { get; set; }
        public string? ProposedAltitudeCode { get; set; }
        public string PresentWorkerClassification { get; set; }
        public string? ProposedClassification { get; set; }
    }
}