using GTERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DAP.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public ForgotPasswordModel(IConfiguration configuration, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Microsoft.Extensions.Primitives.StringValues originValues;
                Request.Headers.TryGetValue("Origin", out originValues);



                #region Send request to api and get Response
                WebHelper webHelper = new WebHelper();

                string request = JsonConvert.SerializeObject(Input);

                //Login
                // Uri url =
                // ("https://localhost:44336/api/User/Login"));
                //Uri url = new Uri("http://101.2.165.187:82/api/User/Login");
                //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/Login")); ///enable ssl certificate for secure connection
                //Uri url = new Uri(string.Format("https://www.gtrbd.net/Support/api/AccountAPI/Login"));
                Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:ForgotPassword")));

                string response = webHelper.Post(url, request);


                //LoginResponse res = new LoginResponse(response);
                #endregion

                if (response == "true")
                {
                    //Handle your reponse here
                    //return LocalRedirect("./CheckEmail");
                    //return RedirectToPage("../identity/account/CheckEmail");
                    //return View("Identity", null);
                    return RedirectToPage("/Account/CheckEmail", new { area = "Identity" });
                }




                //var user = await _userManager.FindByEmailAsync(Input.Email).ConfigureAwait(false);
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return RedirectToPage("./ForgotPasswordConfirmation");
                //}

                //// For more information on how to enable account confirmation and password reset please 
                //// visit https://go.microsoft.com/fwlink/?LinkID=532713
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var callbackUrl = Url.Page(
                //    "/Account/ResetPassword",
                //    pageHandler: null,
                //    values: new { area = "Identity", code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

                //return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
