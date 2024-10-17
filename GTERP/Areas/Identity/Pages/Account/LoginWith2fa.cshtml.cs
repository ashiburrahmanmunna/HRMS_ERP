using GTERP.AppData;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DAP.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly GTRDBContext db;
        public LoginWith2faModel(SignInManager<IdentityUser> signInManager, ILogger<LoginWith2faModel> logger, IConfiguration configuration, GTRDBContext db)
        {
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            this.db = db;
        }
        [TempData]
        public string ErrorMessage { get; set; }
        public TransactionLogRepository LogRepository { get; }
        public IConfiguration Config { get; }
        public IConfiguration Configuration { get; }
        public AuthorizationFilterContext Accessor { get; }
        [BindProperty]
        public InputModel Input { get; set; }
        public string UserName { get; set; }
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public class Login2FAModel
        {

            public string UserName { get; set; }
            public string Code { get; set; }
        }
        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Please Input here")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string userName, string returnUrl = null)
        {

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            UserName = userName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string userName, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            var client = new HttpClient();
            var result = await client.PostAsJsonAsync(_configuration.GetValue<string>("API:Login2FA"), new { UserName = userName, Code = authenticatorCode });
            LoginResponse res = new LoginResponse(null);
            if (result.IsSuccessStatusCode)
            {
                res = new LoginResponse(await result.Content.ReadAsStringAsync());



                if (res.Success == true && res.IsActive)
                {
                    AppData.dbdaperpconstring = _configuration.GetConnectionString("DefaultConnection");




                    HttpContext.Session.SetString("userid", res.UserId);
                    HttpContext.Session.SetString("username", res.UserName);
                    HttpContext.Session.SetString("fullname", res.FullName);

                    if (res.AppKey.Count is not 0)
                    {
                        var appKeyList = new List<Guid>().ToArray();

                        if (res.UserName.Contains("@gtrbd."))
                        {
                            appKeyList = res.AppKey.Select(a => a.ComId).Distinct().ToArray();
                        }
                        else
                        {
                            appKeyList = res.AppKey.Where(a => a.ComId.ToString() != "576b68b3-da3f-4fe5-9656-bc92e4dcdf72").Select(a => a.ComId).Distinct().ToArray(); // Without Genuine Technology ComID
                        }

                        var comList = new List<GTERP.Models.Company>();

                        foreach (var item in appKeyList)
                        {
                            var coms = db.Companys.Where(a => a.CompanyCode == item.ToString() && a.IsInActive == false).ToList();
                            comList.AddRange(coms);
                        }

                        HttpContext.Session.SetObject("ComList", comList);
                        HttpContext.Session.SetObject("ProductList", res.Products);
                        HttpContext.Session.SetObject("ResponseData", res);
                        HttpContext.Session.SetObject("UserRoles", res.UserRoles);
                        HttpContext.Session.SetObject("AppKeys", res.AppKey);

                        return RedirectToAction("ViewCompanies", "Home");
                    }




                    var gotoUrl = HttpContext.Session.GetString("gotourl");
                    return LocalRedirect(returnUrl);

                }

                else
                {

                    ReturnUrl = returnUrl;
                    RememberMe = rememberMe;
                    UserName = userName;
                    ModelState.Clear();
                    ModelState.AddModelError("", "User Is In Active. Please Contact With Administrator");
                    return Page();

                }
            }

            if ((int)result.StatusCode == StatusCodes.Status400BadRequest)
            {
                ReturnUrl = returnUrl;
                RememberMe = rememberMe;
                UserName = userName;
                ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
                return Page();
            }
            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            UserName = userName;
            return Page();
        }
    }
}
