using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GTERP.Models.CustomerSerial;

namespace GTERP.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ForgotViewModelMobile
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [StringLength(11, ErrorMessage = "১১ সংখ্যার মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        [Display(Name = "Mobile No")]
        public string PhoneNumber { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        //public bool IsEnabled { get; set; }
    }
    public class LoginViewModelMobile
    {
        [Required(ErrorMessage = "মোবাইল নাম্বার প্রবেশ করুন |")]
        [DataType(DataType.PhoneNumber)]

        [StringLength(11, ErrorMessage = "১১ সংখ্যার মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        //[StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long. Ex: 01621657550", MinimumLength = 11)]


        [Display(Name = "Mobile No")]
        [Phone]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "পাসওয়ার্ড প্রবেশ করুন |")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        //public bool IsEnabled { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterViewModelMobile
    {
        [Required(ErrorMessage = "মোবাইল নাম্বার প্রবেশ করুন |")]
        [Phone]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile")]
        [StringLength(11, ErrorMessage = "১১ সংখ্যার মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        //[StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long. Ex: 01621657550", MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "পাসওয়ার্ড প্রবেশ করুন |")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        //        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [StringLength(11, ErrorMessage = "নূনতম ৬ অক্ষরের পাসওয়ার্ড দিন |", MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])).+$", ErrorMessage = "Password doesn't meet the requirements")]

        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\d)).+$", ErrorMessage = "Password doesn't meet the requirements")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "কনফার্ম পাসওয়ার্ড উপরের পাসওয়ার্ড এর সাথে মিলেনি ।")]
        public string ConfirmPassword { get; set; }

        ///public bool? IsEnabled { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    public class ResetPasswordMobileViewModel
    {
        [Required(ErrorMessage = "মোবাইল নাম্বার প্রবেশ করুন")]
        [Phone]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        [StringLength(11, ErrorMessage = "১১ সংখ্যার মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        [Display(Name = "মোবাইল নাম্বার")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "পাসওয়ার্ড প্রবেশ করুন")]
        [StringLength(100, ErrorMessage = "নূনতম ৬ অক্ষরের পাসওয়ার্ড দিন।", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "পাসওয়ার্ড")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "কনফার্ম পাসওয়ার্ড")]
        [Compare("Password", ErrorMessage = "কনফার্ম পাসওয়ার্ড উপরের পাসওয়ার্ড এর সাথে মিলেনি।")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        [Display(Name = "ভেরিফিকেশন কোড")]
        [Required(ErrorMessage = "ভেরিফিকেশন কোড দিন।")]
        [StringLength(6, ErrorMessage = "নূনতম ৬ অক্ষরের ভেরিফিকেশন কোড দিন।", MinimumLength = 6)]
        public string VerifyCode { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ForgotPasswordMobileViewModel
    {
        [Required(ErrorMessage = "মোবাইল নাম্বার প্রবেশ করুন")]
        [Phone]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয়।")]
        [StringLength(11, ErrorMessage = "১১ সংখ্যার মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        [Display(Name = "মোবাইল")]
        public string Mobile { get; set; }
    }

    public class LoginRegisterViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }
}