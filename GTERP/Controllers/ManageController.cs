using GTCommercial.Models;
using GTERP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    [Authorize]
    public class ManageController : Controller
    {


        private GTRDBContext db = new GTRDBContext();
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
    }

        //
        // GET: /Account/Index
        [HttpGet]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two factor provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "The phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            string userId = User.Identity.GetUserId();
            IndexViewModel model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> IndexMobileVerify(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two factor provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "The phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            string userId = User.Identity.GetUserId();
            IndexViewModel model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // GET: /Account/RemoveLogin
        [HttpGet]
        public ActionResult RemoveLogin()
        {
            System.Collections.Generic.IList<UserLoginInfo> linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            string userId = User.Identity.GetUserId();
            IdentityResult result = await UserManager.RemoveLoginAsync(userId, new //UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Account/AddPhoneNumber
        [HttpGet]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Account/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            string code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                IdentityMessage message = new IdentityMessage
                {
                    Destination = model.Number,
                    //Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);

                smsViaAPI("Your security code is " + code + "", model.Number);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }


        private void smsViaAPI(string Message, string MobileNo)
        {

            try
            {


                WebClient client = new WebClient();
                SmsSetting asdf = new SmsSetting();
                //DataTable dt = new DataTable("smsSettings");

                var x = (db.smssettings.Where(p => p.isInactive == "0").ToList().SingleOrDefault());
                //dt.Rows.Add(x);


                //foreach (DataRow drCurrent in x))
                //{
                    if (x.Company.ToString().ToUpper() == "Dominate".ToUpper())
                    {
                        string Address = x.smsAddress.ToString();
                        string UserID = "?user=" + x.smsUser.ToString();
                        string Password = "&password=" + x.smsPassword.ToString();
                        string SendID = "&sender=" + x.smsSender.ToString();
                        string Message1 = "";
                        string MobileNo1 = "";

                        string BaseUrl = Address + UserID + Password + SendID + Message1 + MobileNo1;

                        Stream data = client.OpenRead(BaseUrl);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                    }
                    else if (x.Company.ToString() == "infobip")
                    {

                        string Address = x.smsAddress.ToString();
                        string UserID = "?username=" + x.smsUser.ToString();
                        string Password = "&password=" + x.smsPassword.ToString();

                        string msisdn = "";


                        string SendID = "&sender=" + x.smsSender.ToString();
                        string Message1 = "";
                        //string MyString = "+8801621657550";

                        Message1 = "&text= " + Message.ToString();
                        msisdn = "&to=88" + MobileNo.Substring(MobileNo.Length - 11);




                        string BaseUrl = Address + UserID + Password + msisdn + Message1;

                        Stream data = client.OpenRead(BaseUrl);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();

                    }
                    else if ((x.Company.ToString() == "sslwireless"))
                    {

                        string Address = x.smsAddress.ToString();


                        string UserID = "?user=" + x.smsUser.ToString();
                        string Password = "&pass=" + x.smsPassword.ToString();


                        string SendID = "&sid=" + x.smsSender.ToString();
                        string csmsid = "&csmsid=123456789";
                        string Message1 = "";


                        string msisdn = "";


                        string MyString = "+8801621657550";

                        Message1 = "&sms= " + "sms will generate here".ToString();
                        msisdn = "&msisdn=88" + MyString.Substring(MyString.Length - 11);



                        string BaseUrl = Address + UserID + Password + SendID + Message1 + msisdn + csmsid;

                        Stream data = client.OpenRead(BaseUrl);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();

                    }
                    else
                    {
                        string Address = x.smsAddress.ToString();
                        string UserID = "?username=" + x.smsUser.ToString();
                        string Password = "&password=" + x.smsPassword.ToString();
                        string apicode = "&apicode=1";
                        string msisdn = "";
                        string countrycode = "&countrycode=880";
                        string cli = "&cli=" + x.smsSender.ToString();
                        string messagetype = "&messagetype=1";

                        string SendID = "&sender=" + x.smsSender.ToString();
                        string Message1 = "";

                        string MyString = "+8801621657550";

                        Message1 = "&message= " + "Sms generate here".ToString();
                        msisdn = "&msisdn=88" + MyString.Substring(MyString.Length - 11);

                        string BaseUrl = Address + UserID + Password + apicode + msisdn + countrycode + cli + messagetype + Message1;

                        Stream data = client.OpenRead(BaseUrl);
                        StreamReader reader = new StreamReader(data);
                        string s = reader.ReadToEnd();
                        data.Close();
                        reader.Close();

                    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // POST: /Manage/RememberBrowser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RememberBrowser()
        {
            System.Security.Claims.ClaimsIdentity rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId());
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, rememberBrowserIdentity);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/ForgetBrowser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetBrowser()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/EnableTFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTFA()
        {
            string userId = User.Identity.GetUserId();
            await UserManager.SetTwoFactorEnabledAsync(userId, true);
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTFA()
        {
            string userId = User.Identity.GetUserId();
            await UserManager.SetTwoFactorEnabledAsync(userId, false);
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Account/VerifyPhoneNumber
        [HttpGet]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            // This code allows you exercise the flow without actually sending codes
            // For production use please register a SMS provider in IdentityConfig and generate a code here.
            string code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            ViewBag.Status = "Verification code is " + code;
            //smsViaAPI("Verification code is " + code + "", phoneNumber);
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });

            
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordMobile()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordMobile(string code , string PhoneNumber)
        {
            ResetPasswordMobileViewModel resetpasswordmobile = new ResetPasswordMobileViewModel();

            resetpasswordmobile.Code = Session["tokencode"].ToString();
            resetpasswordmobile.PhoneNumber = Session["mobileno"].ToString();
            //resetpasswordmobile.VerifyCode = Session["mobileno"].ToString();
            //ViewBag.verifycode = Session["smscode"].ToString();
            ViewBag.error = "";


            //ViewBag.code = code;
            return code == null ? View("Error") : View(resetpasswordmobile);

        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordMobile(ResetPasswordMobileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.VerifyCode.ToString() != Session["smscode"].ToString())
            {
                ViewBag.error = "ভেরিফিকেশন কোডটি সঠিক নয়";
                return View(model);
            }

            ApplicationUser user = await UserManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmationMobile", "Manage");
            }
            model.Code = Session["tokencode"].ToString();
            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmationMobile", "Manage");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmationMobile()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPasswordMobile(ForgotPasswordMobileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var user = await UserManager.FindByNameAsync(model.Mobile);
            //IdentityUser user = UserManager.FindByNameAsync(model.Mobile).Result;

            ApplicationUser auser = await UserManager.FindByNameAsync(model.Mobile);
            //var userid = auser.Id;
            //Generate the token and send it
            string smscode = await UserManager.GenerateChangePhoneNumberTokenAsync(auser.Id, model.Mobile);
            Session["smscode"] = smscode;
            if (UserManager.SmsService != null)
            {
                IdentityMessage message = new IdentityMessage
                {
                    Destination = model.Mobile,
                    //Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);

                smsViaAPI("Your security code is " + smscode + "", model.Mobile);
            }


            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByNameAsync(model.Mobile);
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return View("ForgotPasswordConfirmation");
                //}

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                Session["tokencode"] = code;
                Session["mobileno"] = model.Mobile;

                //perfect//string callbackUrl = Url.Action("ResetPasswordMobile", "Manage", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string callbackUrl = Url.Action("ResetPasswordMobile", "Manage", new { userId = user.Id, code = code ,phoneNumber = model.Mobile }, protocol: Request.Url.Scheme);

                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                //return View("VerifyPhoneNumberMobileUser");
                //return View("ResetPasswordMobile");
                return View("ForgotPasswordConfirmationMobile");

                
            }
            //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //string callbackurl = Url.Action("ResetPassword", "Manage", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //return View("VerifyPhoneNumberMobileUser");
            //string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            //ViewBag.Link = callbackUrl;
            //return RedirectToAction("VerifyPhoneNumberMobileUser", "Manage", new { PhoneNumber = model.Mobile });

            //var resetLink = Url.Action("ResetPassword", "Account", new { token = token },protocol: HttpContext.Request.Scheme);
            //RedirectToAction("Manage", "VerifyPhoneNumberMobileUser"); //new { PhoneNumber = model.Mobile } 

            return View("ResetPassword");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmationMobile()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> VerifyPhoneNumberMobileUser(string phoneNumber)
        {
            // This code allows you exercise the flow without actually sending codes
            // For production use please register a SMS provider in IdentityConfig and generate a code here.
            var x = CheckIsPhoneNumber(phoneNumber.ToString());

            if (x == "NAN")
            {

                ViewBag.Status = "Mobile No not Recognized by System.";
                ViewBag.Code = "Nan";

            }
            else
            {
                string code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), x);
                //ViewBag.Status = "Verification code is " + code;
                smsViaAPI("Verification code is " + code + "", phoneNumber);// fahad need to on after all complete.
                //ViewBag.Code = code;
                //return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });

            }

            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });

        }

        public static string CheckIsPhoneNumber(string str)
        {
            str = Regex.Replace(str, @"\s+", "");
            if (string.IsNullOrWhiteSpace(str)) return "NAN";
            var numberArray = str.Split(',');

            var num = new String(numberArray[0].Where(Char.IsDigit).ToArray());

            var len = num.Length;
            if (len < 10 || len > 13 || len == 12) return "NAN";
            if (len == 13)
            {
                string firstTow = num.Substring(0, 2);
                if (firstTow == "88")
                {
                    string firstFive = num.Substring(0, 5);
                    if (firstFive == "88016" || firstFive == "88015" || firstFive == "88017" || firstFive == "88018" ||
                        firstFive == "88019" || firstFive == "88011" || firstFive == "88013" || firstFive == "88014")
                    {
                        return num.Substring(2, 11);
                    }
                    return "NAN";
                }
                return "NAN";

            }
            if (len == 10)
            {
                var fristTwo = num.Substring(0, 2);
                if (fristTwo == "16" || fristTwo == "15" || fristTwo == "17" || fristTwo == "18" ||
                    fristTwo == "19" || fristTwo == "11" || fristTwo == "13" || fristTwo == "14")
                {
                    return "0" + num;
                }
                return "NAN";
            }
            if (len == 11)
            {
                var firstThree = num.Substring(0, 3);
                if (firstThree == "016" || firstThree == "015" || firstThree == "017" || firstThree == "018" ||
                    firstThree == "019" || firstThree == "011" || firstThree == "013" || firstThree == "014")
                {
                    return num;
                }
                return "NAN";
            }
            return "NAN";
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumberMobileUser(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = User.Identity.GetUserId();
            IdentityResult result = await UserManager.ChangePhoneNumberAsync(userId, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }




                Session["MobileConfirmed"] = "1";
                Session["userid"] = userId;
                Session["userphoneno"] = model.PhoneNumber;

                //return RedirectToAction("IndexMobileVerify", new { Message = ManageMessageId.AddPhoneSuccess });
                return RedirectToAction("Create", "CustomerSerial");
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "সঠিক ভেরিফিকেশান কোড প্রবেশ করুন।"); ///Failed to verify phone
            return View(model);
        }
        //
        // POST: /Account/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = User.Identity.GetUserId();
            IdentityResult result = await UserManager.ChangePhoneNumberAsync(userId, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Account/RemovePhoneNumber
        [HttpGet]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            string userId = User.Identity.GetUserId();
            IdentityResult result = await UserManager.SetPhoneNumberAsync(userId, null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePasswordMobileUser()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = User.Identity.GetUserId();
            IdentityResult result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordMobileUser(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = User.Identity.GetUserId();
            IdentityResult result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("IndexMobileVerify", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }


        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                IdentityResult result = await UserManager.AddPasswordAsync(userId, model.NewPassword);
                if (result.Succeeded)
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            string userId = User.Identity.GetUserId();
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            System.Collections.Generic.IList<//UserLoginInfo> //UserLogins = await UserManager.GetLoginsAsync(userId);
            System.Collections.Generic.List<AuthenticationDescription> otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => //UserLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || //UserLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = //UserLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            string userId = User.Identity.GetUserId();
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, userId);
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(userId, loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}