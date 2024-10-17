
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GTERP.Models.CustomerSerial;

namespace GTERP.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        //public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordViewModelBangla
    {
        [Required(ErrorMessage = "বর্তমান পাসওয়ার্ড ইনপুট দিতে হবে")]
        [DataType(DataType.Password)]
        [Display(Name = "বর্তমান পাসওয়ার্ড")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "নতুন পাসওয়ার্ড ইনপুট দিতে হবে")]
        [StringLength(100, ErrorMessage = "নতুন পাসওয়ার্ড নূনতম ৬ অক্ষরের হতে হবে।", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "নতুন পাসওয়ার্ড")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "কনফার্ম নতুন পাসওয়ার্ড")]
        [Compare("NewPassword", ErrorMessage = "নতুন পাসওয়ার্ড এবং কনফার্ম পাসওয়ার্ড মিলেনি।")]
        public string ConfirmPassword { get; set; }
    }


    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
    }

}