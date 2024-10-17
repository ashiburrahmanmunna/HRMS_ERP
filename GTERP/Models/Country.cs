using GTERP.Models.Self;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Country : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Dial Code")]
        public string DialCode { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Country Short Name")]
        public string CountryShortName { get; set; }


        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Culture Info")]
        public string CultureInfo { get; set; }

        //[Required]
        //public int TimeZone { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }


        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Currency Symbol")]
        public string CurrencySymbol { get; set; }


        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Currency Short Name")]
        public string CurrencyShortName { get; set; }


        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Flag Class")]
        public string FlagClass { get; set; }

        [Display(Name = "Customer")]
        public virtual ICollection<Customer> vCustomer { get; set; }
        public virtual ICollection<Supplier> vSupplier { get; set; }

        public bool isActive { get; set; }


        [Display(Name = "State")]
        public virtual ICollection<State> vStateCountry { get; set; }

        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Acc_VoucherSub> VoucherSubs { get; set; }
        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Acc_VoucherSub> VoucherSubs1 { get; set; }
        ////public virtual ICollection<Product> vProductCountry { get; set; }

    }
}