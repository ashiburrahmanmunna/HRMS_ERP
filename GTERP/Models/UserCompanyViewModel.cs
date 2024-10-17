using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IdentitySample.Models
{
    public class CompanyViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Company")]
        public string Name { get; set; }
    }

    public class EditUserCompanyViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "User")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}