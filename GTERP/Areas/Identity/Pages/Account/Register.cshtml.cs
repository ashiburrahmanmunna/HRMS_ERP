#region Assembly Refference
using GTERP.Models;
using GTERP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace DAP.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        #region Feilds and Constructor
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        GTRDBContext db;
        public RegisterModel(IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            GTRDBContext _db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
            db = _db;
        }
        #endregion

        #region Properties
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        #endregion

        #region Input Model
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]

            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public Guid AppKey { get; set; } = Guid.Parse("00696C16-1B7D-91B9-783A-B789C9D15B2C");

        }
        #endregion

        #region OnGetAsync
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        #endregion

        #region OnPostAsyncRegister
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            WebHelper webHelper = new WebHelper();

            string request = JsonConvert.SerializeObject(Input);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:Register")));
            string response = webHelper.Post(url, request);

            RegResponse res = new RegResponse(response);
            if (res.Succeeded == true)
            {
                //Handle your reponse here
                //return LocalRedirect("./CheckEmail");
                return RedirectToPage("./CheckEmail");
            }
            else
            {
                if (res.Errors != null)
                {
                    foreach (var item in res.Errors)
                    {
                        if (!string.IsNullOrEmpty(item.Code) && !string.IsNullOrEmpty(item.Description))
                        {
                            HttpContext.Session.SetString("error", $"{item.Description}");
                        }
                    }

                    ViewData["error"] = res.Errors;

                }

                return RedirectToPage("Register");

                //No Response from the server

            }


        }
        #endregion
    }
}
