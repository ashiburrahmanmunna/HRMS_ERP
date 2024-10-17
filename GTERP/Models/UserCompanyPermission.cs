using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class UserCompanyPermission
    {

        public int UserCompanyPermissionId { get; set; }
        //[Key, Column(Order = 0)]
        //[Display(Name = "User")]
        //[ForeignKey("ApplicationUser")]
        //public string UserId { get; set; }

        //public virtual List<ApplicationUser> ApplicationUser { get; set; }


        ////[Key, Column(Order = 1)]
        ////public string ItemName { get; set; }
        ////public int CategoryId { get; set; }

        //[Display(Name = "Company")]
        public int CompanyId { get; set; }
        //public int ProductId { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Default")]
        public Boolean isDefault { get; set; }



        [Display(Name = "Sort No")]
        public int SortNo { get; set; }



        //public virtual IdentitySample.Models.ApplicationUser vUserList { get; set; }

        //public virtual Company vCompanyList { get; set; }
        //public virtual ICollection<Company> vCompanyList { get; set; }




    }
}