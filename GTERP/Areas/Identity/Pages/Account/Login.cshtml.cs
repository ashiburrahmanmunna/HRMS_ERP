#region Assembly Refference
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Company = GTERP.Models.Company;
//using FreeGeoIPCore.AppCode;
#endregion

namespace GTERP.Areas.Identity.Pages.Account
{
    public class Login1Model : PageModel
    {

        #region Feilds and constructor
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Login1Model> _logger;
        private readonly GTRDBContext db;

        public Login1Model(SignInManager<IdentityUser> signInManager,
            ILogger<Login1Model> logger,
            UserManager<IdentityUser> userManager,
            GTRDBContext context, TransactionLogRepository logRepository, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            db = context;
            LogRepository = logRepository;
            _configuration = configuration;

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
        public TransactionLogRepository LogRepository { get; }
        public IConfiguration Config { get; }
        public IConfiguration Configuration { get; }
        public AuthorizationFilterContext Accessor { get; }
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
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public Guid AppKey { get; set; }// = Guid.Parse("A8301F9A-38CC-460A-916F-FCD559A61732"); // anj // trinity logistics ltd.
                                            //9AFA4FBC-21F7-430B-A00B-480D47CD7F25 -- gsg appeky - GTSoftMangement -- id
                                            //46452A80-134F-4D1A-9F13-C6A13AEAAF5E -- areial properties ltd // appkey             //340EB995-FC99-4974-95CB-2ECA4201AE14 --- aerial properties ltd // comid


            //insert into UserMangement(UserId ,                ComID , Roleid , IsDefault)
            //select '4864add7-0ab2-4c4f-9eb8-6b63a425e665' , '3A5A92E3-AB8F-4CA1-B639-747AC7F49144' , '3c2e2d8e-ead7-43cc-ba33-f1cb9a3e1f63' , 1

        }

        #endregion


        #region OnGetAsync
        public async Task OnGetAsync(string returnUrl = null)
        {



            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/Home/Company");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        #endregion


        #region OnPostLogin

        public async Task<IActionResult> OnPostLogin(string returnUrl = null)
        {

            try
            {
                returnUrl = returnUrl ?? Url.Content("~/Home/Company");

                // Not checking permission 

                //var inactiveuser = db.UserPermission?.FirstOrDefault(x => x.UserName == Input.Email)?.IsInActive;
                //if (inactiveuser == null || inactiveuser == true)
                //{
                //    ModelState.AddModelError("", "You are not an active user");
                //    return Page();
                //}

                #region Send request to api and get Response
                //WebHelper webHelper = new WebHelper();

                //string request = JsonConvert.SerializeObject(Input);

                //Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:Login")));

                var httpClientHandler = new HttpClientHandler
                {
                    UseCookies = true, // Enable cookie handling
                    CookieContainer = new CookieContainer(),
                };

                //string response = webHelper.Post(url, request);
                HttpClient client = new HttpClient(httpClientHandler);
                var result = await client.PostAsJsonAsync(_configuration.GetValue<string>("API:Login"), Input);

                if ((int)result.StatusCode == 325)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, userName = Input.Email, RememberMe = Input.RememberMe });
                }
                if (result.IsSuccessStatusCode)
                {
                    LoginResponse res = new LoginResponse(await result.Content.ReadAsStringAsync());

                    #endregion

                    #region Do further processing with response
                    if (res.Success == true && res.IsActive)
                    {
                        AppData.AppData.dbdaperpconstring = _configuration.GetConnectionString("DefaultConnection");



                        // Not checking permission 

                        //inactiveuser = db.UserPermission?.FirstOrDefault(x => x.AppUserId == res.UserId)?.IsInActive;
                        //if (inactiveuser == null || inactiveuser == true)
                        //{
                        //    ModelState.AddModelError("", "You are not an active user");
                        //    return Page();
                        //}

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

                            var comList = new List<Company>();

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
                        //else
                        //{
                        //    HttpContext.Session.SetString("appkey", res.AppKey.FirstOrDefault().ToString());
                        //}

                        #region fahad

                        //HttpContext.Session.SetString("securitystamp", res.SecurityStamp);

                        //HttpContext.Session.SetString("comid", res.Companie.ComId);              

                        //// Commenting Previous Code

                        //if (res.Companies != null)
                        //{

                        //    var companies = new List<CompanyUser>();
                        //    foreach (var item in res.Companies)
                        //    {
                        //        var company = new CompanyUser();
                        //        company.ComId = item.ComId;
                        //        company.CompanyName = item.CompanyName;
                        //        companies.Add(company);
                        //    }

                        //    HttpContext.Session.SetObject("company", companies);
                        //    SqlParameter[] sqlParameter1 = new SqlParameter[1];
                        //    sqlParameter1[0] = new SqlParameter("@userid", res.UserId);

                        //    var companys = Helper.ExecProcMapTList<CompanyUser>("prcgetCompany", sqlParameter1);
                        //    if (companys != null)
                        //    {
                        //        List<CompanyUser> CompanyUserList = new List<CompanyUser>();

                        //        for (int i = 0; i < companies.Count(); i++)
                        //        {
                        //            var apiCompany = companies.ElementAt(i);
                        //            foreach (var abc in companys)
                        //            {

                        //                CompanyUser com = new CompanyUser();
                        //                //if (abc.ComId == abc.ComId)
                        //                //{
                        //                    com.ComId = abc.ComId;
                        //                    com.CompanyName = abc.CompanyName;
                        //                    CompanyUserList.Add(com);
                        //                //}

                        //            }
                        //        }


                        //        if (CompanyUserList.Count > 0)
                        //        {
                        //            HttpContext.Session.SetObject("UserCompanys", CompanyUserList);

                        //        }

                        //        // _signInManager.IsSignedIn(res.UserId);

                        //        //var userId = res.UserId;

                        //        var comId = "";
                        //        var defaultcompnay = companys.Where(x => x.isDefault == true).FirstOrDefault();
                        //        if (defaultcompnay != null)
                        //        {
                        //            comId = defaultcompnay.ComId;
                        //        }
                        //        else if (companys != null)
                        //        {
                        //            comId  = companys.FirstOrDefault().ComId;
                        //        }
                        //        HttpContext.Session.SetString("comid", comId.ToString());


                        //        var userId = res.UserId;
                        //        SqlParameter[] sqlParameter = new SqlParameter[2];
                        //        sqlParameter[0] = new SqlParameter("@comid", comId);
                        //        sqlParameter[1] = new SqlParameter("@userid", userId);
                        //        List<UserMenuPermission> userMenus = Helper.ExecProcMapTList<UserMenuPermission>("prcgetMenuPermission", sqlParameter);


                        //        // set session Usermenu

                        //        if (userMenus.Count > 0)
                        //        {
                        //            HttpContext.Session.SetObject("UserMenu", userMenus.OrderBy(a => a.SLNo));

                        //            var moduleses = userMenus.Select(a => a.ModuleId).Distinct();
                        //            var ModuleMenuCaption = userMenus.Where(x => x.Visible == true).Distinct().Select(x => x.ModuleMenuCaption).FirstOrDefault();
                        //            var activemoduleid = userMenus.Where(x => x.Visible == true).Distinct().Select(x => x.ModuleMenuCaption).FirstOrDefault();

                        //            HttpContext.Session.SetObject("activemodulename", ModuleMenuCaption);
                        //            HttpContext.Session.SetObject("activemoduleid", activemoduleid);
                        //            HttpContext.Session.SetObject("Modules", db.Modules.Where(a => moduleses.Contains(a.ModuleId)).ToList());
                        //            //    var x = db.ModuleMenus.Where(x => x.isParent == 1).ToList();
                        //            //HttpContext.Session.SetObject("ModuleMenuPrent",x);
                        //        }

                        //        var userPermission = db.UserPermission.Where(u => u.AppUserId == res.UserId).FirstOrDefault();

                        //        if (userPermission != null)
                        //            HttpContext.Session.SetObject("userpermission", userPermission);
                        //        else
                        //            HttpContext.Session.SetObject("userpermission", new UserPermission());


                        //        MenuPermission_Master master = db.MenuPermissionMasters.Where(m => m.useridPermission == userId).FirstOrDefault();

                        //        if (master == null)
                        //        {
                        //            return LocalRedirect(returnUrl);
                        //        }
                        //        var userMenuPermission = db.MenuPermissionDetails.Where(m => m.MenuPermissionId == master.MenuPermissionId)
                        //            .Select(m => new
                        //            {
                        //                MenuPermissionDetailsId = m.MenuPermissionDetailsId,
                        //                ModuleMenuName = m.ModuleMenus.ModuleMenuName,
                        //                ModuleMenuCaption = m.ModuleMenus.ModuleMenuCaption,
                        //                ModuleMenuLink = m.ModuleMenus.ModuleMenuLink,
                        //                IsCreate = m.IsCreate,
                        //                IsView = m.IsView,
                        //                IsEdit = m.IsEdit,
                        //                IsDelete = m.IsDelete,
                        //                IsReport = m.IsReport,
                        //                SLNo = m.SLNo

                        //            }).OrderBy(x => x.SLNo).ToList();//.Where(m => m.MenuPermission_Masters.useridPermission == user.Id).ToList();

                        //        //var menupermissions = db.MenuPermissionDetails.Where(m => m.MenuPermissionId == menuMaster.MenuPermissionId).ToList();
                        //        var menus = db.ModuleMenus.Select(m => new
                        //        {
                        //            ModuleMenuId = m.ModuleMenuId,
                        //            ModuleMenuName = m.ModuleMenuName,
                        //            ModuleMenuCaption = m.ModuleMenuCaption,
                        //            ModuleMenuLink = m.ModuleMenuLink,
                        //            isInactive = m.isInactive,
                        //            isParent = m.isParent,
                        //            Active = m.Active,
                        //            ParentId = m.ParentId,
                        //            SLNo = m.SLNo
                        //        }).OrderBy(x => x.SLNo).ToList();

                        //        if (userMenuPermission.Count > 0)
                        //        {
                        //            HttpContext.Session.SetObject("menupermission", userMenuPermission);
                        //            HttpContext.Session.SetObject("modules", db.Modules.ToList());
                        //            HttpContext.Session.SetObject("menu", menus);


                        //            HttpContext.Session.SetInt32("activemenuid", 1);
                        //        }


                        //        _logger.LogInformation("User logged in.");

                        //        Company abcd = db.Companys.Include(x => x.vCountryCompany).Where(x => x.AppKey == Input.AppKey.ToString()).FirstOrDefault();




                        //        HttpContext.Session.SetString("isMultiDebitCredit", abcd.isMultiDebitCredit.ToString());
                        //        HttpContext.Session.SetString("isMultiCurrency", abcd.isMultiCurrency.ToString());
                        //        HttpContext.Session.SetString("isVoucherDistributionEntry", abcd.isVoucherDistributionEntry.ToString());
                        //        HttpContext.Session.SetString("isChequeDetails", abcd.isChequeDetails.ToString());

                        //        HttpContext.Session.SetInt32("defaultcurrencyid", abcd.CountryId);
                        //        HttpContext.Session.SetString("defaultcurrencyname", abcd.vCountryCompany.CurrencyShortName.ToString());


                        //        LogRepository.SuccessLogin(HttpContext.Session.GetString("Latitude"), HttpContext.Session.GetString("Longitude"), "Success");


                        //        List<SalesSub> addtocart = new List<SalesSub>();

                        //        HttpContext.Session.SetObject("cartlist", addtocart);
                        //    }
                        //}
                        // }
                        //}

                        //var u = db.UserStates.Where(x => x.UserId == res.UserId).Select(x => x.LastVisited).FirstOrDefault();
                        //if (u!=null)
                        //{
                        //    return Redirect(u);
                        //}
                        //else
                        //{
                        //    return LocalRedirect(returnUrl);
                        //}

                        #endregion

                        var gotoUrl = HttpContext.Session.GetString("gotourl");
                        //if (!gotoUrl.IsNullOrWhitespace())
                        //{
                        //    // return Response.Redirect(gotoUrl);
                        //    //var a = HttpContext.Request;
                        //    //return LocalRedirect($"{a.Scheme}://{a.Host}{a.Path}{a.QueryString}");
                        //    return Redirect(gotoUrl);
                        //}
                        //else
                        {
                            return LocalRedirect(returnUrl);
                        }
                    }

                    else
                    {
                        HttpContext.Session.SetString("userid", Input.Email + " " + Input.Password);
                        LogRepository.SuccessLogin(HttpContext.Session.GetString("Latitude"), HttpContext.Session.GetString("Longitude"), "Failure");

                        if (res.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (res.IsLockedOut)
                        {

                            ModelState.AddModelError("", "The account is locked out");
                            return RedirectToPage("./Lockout");
                        }
                        if (res.IsNotAllowed)
                        {
                            ModelState.Remove("Input.ConfirmPassword");
                            _logger.LogWarning("User email is not confirmed.");
                            ModelState.AddModelError("", "Email is not confirmed.");


                            // var user = await _userManager.FindByEmailAsync(Input.Email);
                            UserId = res.UserId;
                            ShowResend = true;
                            return Page();
                        }

                        if (!res.IsActive)
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("", "User Is In Active. Please Contact With Administrator");
                            return Page();
                        }

                        else
                        {
                            //ModelState.ClearValidationState("ConfirmPassword");
                            //if (ModelState.ContainsKey("{Input.ConfirmPassword}"))
                            //    ModelState["{Input.ConfirmPassword}"].Errors.Clear();


                            //var modelStateErrors = ModelState.Where(model =>
                            //{
                            //    // ignore only if required error is present
                            //    if (model.Value.Errors.Count == 1)
                            //    {
                            //        // assuming required validation error message contains word "required"
                            //        return model.Value.Errors.FirstOrDefault().ErrorMessage.Contains("Invalid");
                            //    }
                            //    return false;
                            //});

                            //foreach (var errorModel in modelStateErrors)
                            //{
                            //    ModelState.Remove(errorModel.Key.ToString());
                            //}
                            ModelState.Remove("Input.ConfirmPassword");
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            ModelState.AddModelError(string.Empty, "User Not Register or Email Not Confirmed");

                            return Page();
                        }



                    }
                }

                if ((int)result.StatusCode == StatusCodes.Status400BadRequest)
                {
                    ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
                    return Page();
                }
                return Page();
            }
            catch (Exception ex)
            {

                throw;
            }
           
            #endregion
        }
        #endregion

        #region OnPostRegister
        public IActionResult OnPostRegister(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            WebHelper webHelper = new WebHelper();

            string request = JsonConvert.SerializeObject(Input);

            //Login
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:Register")));
            //Uri url = new Uri(string.Format("http://gtrbd.net:93/api/User/Register"));


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

                return LocalRedirect(returnUrl);

                //No Response from the server

            }


        }
        #endregion

        public IActionResult OnGetForgotPassword()
        {
            //chittra panel
            return Redirect(_configuration.GetValue<string>("API:ForgotPassword"));
            //return Redirect("http://gtrbd.net:93/Identity/Account/ForgotPassword");

        }
    }
}
