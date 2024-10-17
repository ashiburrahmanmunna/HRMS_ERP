using GTERP.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{


    public class CompanyDetails : BaseModel
    {



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int ComDetailsId { get; set; }



        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = "";


        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        public string MD_Name { get; set; }


        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "MD Phone No")]

        public string MD_Phone { get; set; }

        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "MD Email Address")]
        public string MD_Email { get; set; }


        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        public string HR_Name { get; set; }


        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "HR Phone No")]

        public string HR_Phone { get; set; }

        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "HR Email Address")]
        public string Hr_Email { get; set; }


        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        public string AcountantName { get; set; }


        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Acountant Phone No")]

        public string AcountantPhone { get; set; }

        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Acountant Email Address")]
        public string AcountantEmail { get; set; }

        [StringLength(4, ErrorMessage = "The {0} must  be a year.", MinimumLength = 4)]

        [Display(Name = "Year of Establishment")]
        public string YearOfEst { get; set; }


        [Display(Name = "Number of Employee")]
        public int? NoE { get; set; } = 0;
        [Display(Name = "Company")]
        public int CompanyCode { get; set; }
        [ForeignKey("CompanyCode")]
        public virtual Company Companys { get; set; }

    }

}
