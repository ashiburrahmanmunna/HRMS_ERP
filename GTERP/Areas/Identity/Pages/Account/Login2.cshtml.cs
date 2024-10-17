#region Assembly Refference
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using GTERP.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using GTERP.Services;
using System.IO;

#endregion

namespace DAP.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {

        #region Feilds and constructor
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly GTRDBContext db;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            GTRDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            db = context;

        }
        #endregion

        #region Property

        [BindProperty]
        public InputModel Input { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public bool ShowResend { get; set; }
        public string UserId { get; set; }


        [TempData]
        public string ErrorMessage { get; set; }
        #endregion

        //To Do : Input model should be extend to capture request property

        #region InputModel
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            public int AppKey { get; set; } = 4035;


        }

        #endregion


        #region OnGetAsync
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        #endregion


        #region OnPostAsync

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {


            returnUrl = returnUrl ?? Url.Content("~/");



            #region Send request to api and get Response
            WebHelper webHelper = new WebHelper();

            string request = JsonConvert.SerializeObject(Input);

            //Login
            // Uri url = new Uri(string.Format("https://localhost:44336/api/User/Login"));
            Uri url = new Uri(string.Format("http://101.2.165.187:82/api/User/Login"));

            string response = webHelper.Post(url, request);


            LoginResponse res = new LoginResponse(response);
            #endregion

            #region Do further processing with response
            if (res.Success == true)
            {
                HttpContext.Session.SetString("userid", res.UserId);
                HttpContext.Session.SetString("username", res.UserName);
                //HttpContext.Session.SetString("comid", res.Companie.ComId);
                HttpContext.Session.SetInt32("appkey", Input.AppKey);

                var companies = new List<CompanyUser>();

                var company = new CompanyUser();
                company.ComId = res.Companie.ComId;
                company.CompanyName = res.Companie.CompanyName;
                //companies.Add()
                HttpContext.Session.SetObject("company", company);

                // _signInManager.IsSignedIn(res.UserId);

                //var userId = res.UserId;

                var comId = res.Companie.ComId;

                var userId = res.UserId;
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@comid", comId);
                sqlParameter[1] = new SqlParameter("@userid", userId);
                List<UserMenuPermission> userMenus = Helper.ExecProcMapTList<UserMenuPermission>("prcgetMenuPermission", sqlParameter).ToList();


                // set session Usermenu

                if (userMenus.Count > 0)
                {
                    HttpContext.Session.SetObject("UserMenu", userMenus);
                }


                SqlParameter[] sqlParameter1 = new SqlParameter[1];
                sqlParameter1[0] = new SqlParameter("@userid", userId);

                var companys = Helper.ExecProcMapTList<CompanyUser>("prcgetCompany", sqlParameter1).ToList();

                List<CompanyUser> CompanyUserList = new List<CompanyUser>();
                foreach (var abc in companys)
                {
                    CompanyUser com = new CompanyUser();
                    if (abc.ComId == company.ComId)
                    {
                        com.ComId = company.ComId;
                        com.CompanyName = company.CompanyName;
                    }

                    CompanyUserList.Add(com);
                }



                if (companys.Count > 0)
                {
                    HttpContext.Session.SetObject("UserCompanys", CompanyUserList);
                }



                MenuPermission_Master master = db.MenuPermissionMasters.Where(m => m.useridPermission == userId).FirstOrDefault();

                if (master == null)
                {
                    return LocalRedirect(returnUrl);
                }
                var userMenuPermission = db.MenuPermissionDetails.Where(m => m.MenuPermissionId == master.MenuPermissionId)
                    .Select(m => new
                    {
                        MenuPermissionDetailsId = m.MenuPermissionDetailsId,
                        ModuleMenuName = m.ModuleMenus.ModuleMenuName,
                        ModuleMenuCaption = m.ModuleMenus.ModuleMenuCaption,
                        ModuleMenuLink = m.ModuleMenus.ModuleMenuLink,
                        IsCreate = m.IsCreate,
                        IsView = m.IsView,
                        IsEdit = m.IsEdit,
                        IsDelete = m.IsDelete,
                        IsReport = m.IsReport
                    }).ToList();//.Where(m => m.MenuPermission_Masters.useridPermission == user.Id).ToList();

                //var menupermissions = db.MenuPermissionDetails.Where(m => m.MenuPermissionId == menuMaster.MenuPermissionId).ToList();
                var menus = db.ModuleMenus.Select(m => new
                {
                    ModuleMenuId = m.ModuleMenuId,
                    ModuleMenuName = m.ModuleMenuName,
                    ModuleMenuCaption = m.ModuleMenuCaption,
                    ModuleMenuLink = m.ModuleMenuLink,
                    isInactive = m.isInactive,
                    isParent = m.isParent,
                    Active = m.Active,
                    ParentId = m.ParentId
                }).ToList();

                if (userMenuPermission.Count > 0)
                {
                    HttpContext.Session.SetObject("menupermission", userMenuPermission);
                    HttpContext.Session.SetObject("menu", menus);
                }


                _logger.LogInformation("User logged in.");

                // }
                //}
                return LocalRedirect(returnUrl);
            }


            else
            {
                if (res.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (res.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                if (res.IsNotAllowed)
                {

                    _logger.LogWarning("User email is not confirmed.");
                    ModelState.AddModelError("", "Email is not confirmed.");


                    // var user = await _userManager.FindByEmailAsync(Input.Email);
                    UserId = res.UserId;
                    ShowResend = true;
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

            } 
            #endregion
        } 
        #endregion
    }
}