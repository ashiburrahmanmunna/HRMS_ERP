using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GTERP.Models.CustomerSerial;

namespace GTERP.Models
{
    public class XCompany
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Company Short Name")]
        public string CompanyShortName { get; set; }


        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string comPrimaryAddress { get; set; }


        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string comSecoundaryAddress { get; set; }


        [Required]
        //        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long. Ex: 01621657550", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]

        public string comPhone { get; set; }


        [Required]
        //[StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long. Ex: 01621657550", MinimumLength = 11)]
        [Display(Name = "Phone No 2")]

        public string comPhone2 { get; set; }


        //[Required]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long. Ex: 01621657550", MinimumLength = 11)]
        //[StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax")]

        public string comFax { get; set; }



        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public string comEmail { get; set; }



        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Web Site")]
        public string comWeb { get; set; }




        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Person")]
        public string ContPerson { get; set; }

        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Designation")]
        public string ContDesig { get; set; }




        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }


        [Display(Name = "Is POS Print")]
        public bool isPosPrint { get; set; }


        [Display(Name = "Company Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ComImageHeader { get; set; }

        [Display(Name = "Company Logo [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ComLogo { get; set; }


        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Addvertise")]
        public string Addvertise { get; set; }



        //[Display(Name = "Base Company")]
        //public virtual Company vBasedCompany { get; set; }



        // [Display(Name = "Company Name")]


        //public virtual ICollection<Product> vProducts { get; set; }
    }
}