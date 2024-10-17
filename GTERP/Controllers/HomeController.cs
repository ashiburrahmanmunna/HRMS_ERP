
using GTERP.Areas.Identity.Pages.Account;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace GTERP.Controllers
{
    public class HomeController : Controller
    {
        private GTRDBContext db;
        private readonly ILogger<Login1Model> _logger;
        private readonly IConfiguration _configuration;
        public TransactionLogRepository LogRepository { get; }

        public IHttpContextAccessor Accessor { get; }

        public HomeController(IConfiguration configuration, GTRDBContext context, IHttpContextAccessor accessor, ILogger<Login1Model> logger, TransactionLogRepository logRepository)
        {
            db = context;
            Accessor = accessor;
            LogRepository = logRepository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            ViewBag.Message = "Access Denied";

            return View();
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpGet]
        public ActionResult Index_Ecommerce()
        {
            //homeSlider
            List<HomeSlider> mainSlider = db.HomeSlider.ToList();
            ViewBag.homeSliders = mainSlider;

            //category
            var category = db.Categories.Where(c => c.CategoryId > 0).ToList();
            ViewBag.Category = category;

            //subcat
            var subcategory = db.SubCategory.Where(c => c.SubCategoryId > 0).ToList();
            ViewBag.SubCategories = subcategory;
            //product
            //var products = db.Products.Include(p => p.SubCategory); ///sahinur work
            //var products = db.Products.Include(p => p.CategoryId);
            return View(db.Products.Where(c => c.ProductId > 1).ToList());
            //return View();
        }
        [HttpGet]
        public ActionResult Profile()
        {
            string userId = HttpContext.Session.GetString("userid");

            var user = db.UserPermission.Include(a => a.HR_Emp_Info).Where(a => a.AppUserId == userId).FirstOrDefault();

            return View(user);
        }

        [HttpGet]

        public ActionResult ResetPassword()
        {

            var comid = Accessor.HttpContext.Session.GetString("comid");
            var userid = Accessor.HttpContext.Session.GetString("userid");
            var appKey = HttpContext.Session.GetString("appkey");
            string username = HttpContext.Session.GetString("username");

            //var currentappkey = db.Companys.Where(x => x.CompanySecretCode == comid).FirstOrDefault().AppKey;

            InputModel abc = new InputModel();
            abc.Email = username;
            abc.AppKey = Guid.Parse(appKey.ToString());

            WebHelper webHelper = new WebHelper();

            string request = JsonConvert.SerializeObject(abc);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:ResetPassword")));


            string response = webHelper.Post(url, request);


            //RegResponse res = new RegResponse(response);
            if (response == "true")
            {
                //Handle your reponse here
                //return LocalRedirect("./CheckEmail");
                //return RedirectToPage("../identity/account/CheckEmail");
                //return View("Identity", null);
                return RedirectToPage("/Account/CheckEmailReset", new { area = "Identity" });
            }


            return View();
        }

        public class InputModel
        {

            public string Email { get; set; }

            public Guid AppKey { get; set; }


        }
        public IActionResult ForgotPassOnClick()
        {

            string username = HttpContext.Session.GetString("username");
            InputModel abc = new InputModel();
            abc.Email = username;

            WebHelper webHelper = new WebHelper();

            string request = JsonConvert.SerializeObject(abc);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:ResetPassword")));


            string response = webHelper.Post(url, request);


            //RegResponse res = new RegResponse(response);
            if (response == "true")
            {
                //Handle your reponse here
                //return LocalRedirect("./CheckEmail");
                //return RedirectToPage("../identity/account/CheckEmail");
                //return View("Identity", null);
                return RedirectToPage("/Account/CheckEmailReset", new { area = "Identity" });
            }


            return View();







        }

        [HttpGet]

        public ActionResult Index()
        {
            HttpContext.Session.SetInt32("ActiveModuleMenuId", 0);
            HttpContext.Session.SetString("activemodulename", "Software");
            ViewBag.error = HttpContext.Session.GetString("error");
            HttpContext.Session.SetString("error", "");
            return View();
            //return View();
        }


        [HttpGet]

        public ActionResult Company()
        {
            HttpContext.Session.SetInt32("ActiveModuleMenuId", 0);
            HttpContext.Session.SetString("activemodulename", "Software");
            ViewBag.error = HttpContext.Session.GetString("error");
            HttpContext.Session.SetString("error", "");
            string comid = HttpContext.Session.GetString("comid");

            SqlParameter p1 = new SqlParameter("@ComId", comid);
            var data = Helper.ExecProcMapTList<CompanyViewModel>("dbo.prcGet_CompanyDetails", new SqlParameter[] { p1 });
            return View(data);
            //return View();
        }

        [HttpGet]
        public ActionResult UserSessionCheck() => Json(HttpContext.Session.GetString("userid"));


        public ActionResult CheckSession()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AppKey")))
            {
                return RedirectToPage("./Login1");
            }
            return null;
        }

        [OverridableAuthorize]

        public ActionResult ModuleDefault()
        {
            return View();
            //return View();
        }

        [HttpPost, ActionName("SetSessionHideMenu")]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionHideMenu(string isHide)
        {
            try
            {
                //var JObject = new JObject();
                //var d = JObject.Parse(isHide);
                //string objct = d["data"].ToString();

                //if (objct != null)
                //{


                var a = Convert.ToInt32(isHide);
                HttpContext.Session.SetInt32("ishidemenu", a);
                return Json(new { Success = 1 });
                //}



            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to Set").Message.ToString() });
        }


        [HttpPost, ActionName("SetSessionActiveModule")]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionActiveModule(int isActive)
        {
            try
            {


                HttpContext.Session.SetInt32("isactivemodule", isActive);
                var abc = db.Modules.Where(x => x.ModuleId == isActive).Select(x => x.ModuleName).FirstOrDefault().ToString();
                HttpContext.Session.SetInt32("activemenuid", isActive);
                HttpContext.Session.SetString("activemodulename", abc);

                var AllMenus = HttpContext.Session.GetObject<List<UserMenuPermission>>("UserMenu");
                if (AllMenus != null)
                {
                    var defaultMenu = AllMenus.Where(m => m.ModuleId == isActive && m.isInactive == 0 && m.isParent == 0 && m.ParentId != 0).OrderBy(m => m.SLNo).FirstOrDefault();

                    return Json(new { Success = 1, Menu = defaultMenu });
                }
                else
                {
                    return Json(new { Success = 1, Menu = "" });
                }


            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        public void SaveUserState(string url)
        {
            var comid = Accessor.HttpContext.Session.GetString("comid");
            var userid = Accessor.HttpContext.Session.GetString("userid");
            var recentVisit = url;
            if (userid == null && comid == null)
            {
                return;
            }
            var user = db.UserStates.Where(x => x.UserId == userid).FirstOrDefault();
            if (user != null)
            {
                user.LastVisited = recentVisit;
                db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var newuserstate = new UserState() { UserId = userid, LastVisited = recentVisit, ComId = comid };
                db.Add(newuserstate);
                db.SaveChanges();
            }
        }


        //[HttpPost, ActionName("SetSessionLocation")]
        ////[ValidateAntiForgeryToken]
        //public JsonResult SetSessionLocation(string Latitude , string Longitude)
        //{
        //    try
        //    {

        //        HttpContext.Session.SetString("Latitude", Latitude);
        //        HttpContext.Session.SetString("Longitude", Longitude);


        //        return Json(new { Success = 1 });

        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }

        //}

        [HttpPost, ActionName("SetSessionLocation")]
        //public JsonResult DeleteConfirmed(int id)
        public IActionResult SetSessionActiveModule(string Latitude, string Longitude)
        {
            try
            {
                if (Latitude == null)
                {
                    Latitude = "";
                    Longitude = "";

                }
                //Acc_VoucherSub Acc_VoucherSub = db.Acc_VoucherSubs.Find(id);
                //db.Acc_VoucherSubs.Remove(Acc_VoucherSub);

                HttpContext.Session.SetString("Latitude", Latitude);
                HttpContext.Session.SetString("Longitude", Longitude);

                return Json(new { Success = 1, VoucherID = 0, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }


            // return RedirectToAction("Index");
        }


        [HttpPost, ActionName("SetSessionModuleMenuId")]
        //public JsonResult DeleteConfirmed(int id)
        public IActionResult SetSessionModuleMenuId(int ModuleMenuId)
        {
            try
            {
                if (ModuleMenuId != null)
                {
                    HttpContext.Session.SetInt32("ActiveModuleMenuId", ModuleMenuId);
                }


                return Json(new { Success = 1, ModuleMenuId = ModuleMenuId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }


            // return RedirectToAction("Index");
        }


        public IActionResult GenerateReport()
        {
            // return Redirect("https://localhost:44383/Home/About");

            var ReportPath = "";
            var SqlCmd = "Exec GTERP.dbo.[rptMasterLCSalesContact] '" + 1 + "','" + 4 + "'";
            var DbName = "GTERP";
            var ReportType = "PDF";
            return Redirect("https://localhost:44375/ReportViewer/GenerateReport?ReportPath=" + ReportPath + "&SqlCmd=" + SqlCmd + "&DbName=" + DbName + "&ReportType=" + ReportType);
        }

        //public ActionResult ECommerce()
        //{

        //}


        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult ViewCompanies()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewProducts(string comId)
        {
            HttpContext.Session.SetString("comid", comId);
            HttpContext.Session.SetString("CompanyName", db.Companys.Where(a => a.CompanyCode == comId).Select(a => a.CompanyName).SingleOrDefault());
            List<Softwares> list = HttpContext.Session.GetObject<List<Softwares>>("ProductList");
            List<UserRoles> userRoles = HttpContext.Session.GetObject<List<UserRoles>>("UserRoles");
            List<Softwares> productsList = new List<Softwares>();
            foreach (var item in list.Where(a => a.Name == "GT ACCOUNTING" || a.Name == "GS HRMS" || a.Name == "GT INVENTORY" || a.Name == "GT SOLUTION" || a.Name == "GT Tax"))  // Only get GT ACCOUNTING, GT HRIS, GT INVENTORY from the purchased software. 
            {
                var prod = new Softwares
                {
                    AppKey = item.AppKey,
                    ComId = item.ComId,
                    Name = item.Name,
                    VersionName = item.VersionName
                };
                productsList.Add(prod);
            }

            var companyRole = userRoles.Where(a => a.ComId == comId.ToLower()).Select(a => a.Role).FirstOrDefault();

            HttpContext.Session.SetString("companyRole", companyRole);

            var products = productsList.Where(a => a.ComId == comId.ToLower()).ToList();

            if (products != null)
            {
                ViewBag.products = products;
            }
            ViewBag.comId = comId;
            return View();
        }

        public ActionResult SetAppKey(string AppKey)
        {
            HttpContext.Session.SetString("appkey", AppKey);
            string returnUrl = null;
            returnUrl = returnUrl ?? Url.Content("~/Home/Company");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");
            List<AppKey> appKeys = HttpContext.Session.GetObject<List<AppKey>>("AppKeys");

            var expireDate = appKeys.Where(a => a.Id.ToString() == AppKey).Select(a => a.Exdate).SingleOrDefault();
            var dateRemaining = (expireDate - DateTime.Now).Days;

            if (dateRemaining <= 60)
            {
                HttpContext.Session.SetString("RemainingDay", dateRemaining.ToString());
                HttpContext.Session.SetString("ExpireDate", expireDate.ToString("dd MMMM, yyyy"));
            }

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            HttpContext.Session.SetString("SoftwareId", softwareId.ToString());
            HttpContext.Session.SetString("VersionId", versionId.ToString());


            if (res.Products != null)
            {
                var comId = HttpContext.Session.GetString("comid");
                var companyRole = HttpContext.Session.GetString("companyRole");
                var userId = res.UserId;
                string username = HttpContext.Session.GetString("username").ToLower();

                List<UserMenuPermission> userMenus = new List<UserMenuPermission>();

                if (username != null)
                {
                    if (companyRole is not null)
                    {
                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter("@comid", comId);
                        sqlParameter[1] = new SqlParameter("@userid", userId);
                        sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
                        sqlParameter[3] = new SqlParameter("@softwareId", softwareId);
                        sqlParameter[4] = new SqlParameter("@versionId", versionId);
                        var query = $"Exec prcgetMenuPermission '{comId}','{userId}','{companyRole}','{softwareId}','{versionId}'";
                        userMenus = Helper.ExecProcMapTList<UserMenuPermission>("prcgetMenuPermission", sqlParameter);
                    }
                    //else
                    //{
                    //    SqlParameter[] sqlParameter = new SqlParameter[2];
                    //    sqlParameter[0] = new SqlParameter("@softwareId", softwareId);
                    //    sqlParameter[1] = new SqlParameter("@versionId", versionId);
                    //    userMenus = Helper.ExecProcMapTList<UserMenuPermission>("prcGetVersionMenuPermission", sqlParameter);
                    //}
                }

                // set session Usermenu
                if (userMenus.Count > 0)
                {
                    var usermenuaction = db.Module_Menu_Action.Include(x => x.ModuleMenu).ToList();

                    HttpContext.Session.SetObject("UserMenuAction", usermenuaction);
                    //foreach (var item in userMenus)
                    //{
                    //    item.ActionList = usermenuaction.Where(x => x.ModuleMenuId == item.ModuleMenuId).ToList();

                    //}


                    HttpContext.Session.SetObject("UserMenu", userMenus.OrderBy(a => a.SLNo));

                    var moduleses = userMenus.Select(a => a.ModuleId).Distinct();
                    var ModuleMenuCaption = userMenus.Where(x => x.Visible == true).Distinct().Select(x => x.ModuleMenuCaption).FirstOrDefault();
                    var activemoduleid = userMenus.Where(x => x.Visible == true).Distinct().Select(x => x.ModuleMenuCaption).FirstOrDefault();

                    HttpContext.Session.SetObject("activemodulename", ModuleMenuCaption);
                    HttpContext.Session.SetObject("activemoduleid", activemoduleid);
                    HttpContext.Session.SetObject("Modules", db.Modules.Where(a => moduleses.Contains(a.ModuleId)).ToList());
                    //    var x = db.ModuleMenus.Where(x => x.isParent == 1).ToList();
                    //HttpContext.Session.SetObject("ModuleMenuPrent",x);
                }

                var userPermission = db.UserPermission.Where(u => u.AppUserId == res.UserId).FirstOrDefault();

                if (userPermission != null)
                    HttpContext.Session.SetObject("userpermission", userPermission);
                else
                    HttpContext.Session.SetObject("userpermission", new UserPermission());


                MenuPermission_Master master = db.MenuPermissionMasters.Where(m => m.useridPermission == userId).FirstOrDefault();
                if (master != null)
                {
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
                         IsReport = m.IsReport,
                         SLNo = m.SLNo

                     }).OrderBy(x => x.SLNo).ToList();//.Where(m => m.MenuPermission_Masters.useridPermission == user.Id).ToList();

                    if (userMenuPermission.Count > 0)
                    {
                        HttpContext.Session.SetObject("menupermission", userMenuPermission);
                        HttpContext.Session.SetObject("modules", db.Modules.ToList());

                        HttpContext.Session.SetInt32("activemenuid", 1);
                    }

                }
                else
                {
                    // If menupermission master not found in MenuPermissionMaster

                    VersionMenuPermission_Master versionMenuMaster = db.VersionMenuPermission_Masters.Where(m => m.SoftwareId == softwareId && m.VersionId == versionId).FirstOrDefault();
                  
                    var userVersionMenuPermission = db.VersionMenuPermission_Details.Where(m => m.MenuPermissionId == versionMenuMaster.MenuPermissionId)
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
                        IsReport = m.IsReport,
                        SLNo = m.SLNo

                    }).OrderBy(x => x.SLNo).ToList();//.Where(m => m.MenuPermission_Masters.useridPermission == user.Id).ToList();

                    if (userVersionMenuPermission.Count > 0)
                    {
                        HttpContext.Session.SetObject("menupermission", userVersionMenuPermission);
                        HttpContext.Session.SetObject("modules", db.Modules.ToList());

                        HttpContext.Session.SetInt32("activemenuid", 1);
                    }
                }

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
                    ParentId = m.ParentId,
                    SLNo = m.SLNo
                }).OrderBy(x => x.SLNo).ToList();

                HttpContext.Session.SetObject("menu", menus);


                //     _logger.LogInformation("User logged in.");


                //Company abcd = db.Companys.Include(x => x.vCountryCompany).Where(x => x.AppKey == "A8301F9A-38CC-460A-916F-FCD559A61732").FirstOrDefault();
                var companyId = db.AppKeys.Where(x => x.AppKey == AppKey).Select(x => x.ComId).FirstOrDefault();
                var abcd = db.Companys.Include(x => x.AppKeys).Include(x => x.vCountryCompany).Where(x => x.ComId == companyId).FirstOrDefault();


                if (abcd != null)
                {

                    HttpContext.Session.SetString("isMultiDebitCredit", abcd.isMultiDebitCredit.ToString());
                    HttpContext.Session.SetString("isMultiCurrency", abcd.isMultiCurrency.ToString());
                    HttpContext.Session.SetString("isVoucherDistributionEntry", abcd.isVoucherDistributionEntry.ToString());
                    HttpContext.Session.SetString("isChequeDetails", abcd.isChequeDetails.ToString());

                    HttpContext.Session.SetInt32("defaultcurrencyid", abcd.CountryId);
                    HttpContext.Session.SetString("defaultcurrencyname", abcd.vCountryCompany.CurrencyShortName.ToString());


                    LogRepository.SuccessLogin(HttpContext.Session.GetString("Latitude"), HttpContext.Session.GetString("Longitude"), "Success");


                    List<SalesSub> addtocart = new List<SalesSub>();

                    HttpContext.Session.SetObject("cartlist", addtocart);
                }


                return LocalRedirect(returnUrl);
                //}
            }

            return View();
        }


        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult UnderConstruction()
        {
            ViewBag.Message = "Under Construction.";

            return View();
        }

        //[HttpGet]
        //public ActionResult AccessDenied()
        //{
        //    ViewBag.Message = "Access Denied";

        //    return View();
        //}

        public ActionResult ChangeSelectedCompany(string comid)
        {
            var com = db.Companys.Where(x => x.CompanyCode == comid).FirstOrDefault();
            if (com != null)
            {
                HttpContext.Session.SetString("comid", "");
                HttpContext.Session.SetString("comid", com.CompanySecretCode);
                //HttpContext.Session.SetString("appkey", "");
                //HttpContext.Session.SetString("appkey", com.AppKey);
                HttpContext.Session.SetObject("UserMenu", null);
                HttpContext.Session.SetObject("activemodulename", null);
                HttpContext.Session.SetObject("activemoduleid", null);
                HttpContext.Session.SetObject("Modules", null);
                HttpContext.Session.SetObject("menupermission", null);
                HttpContext.Session.SetObject("menu", null);
                HttpContext.Session.SetInt32("activemenuid", 0);
                var uid = HttpContext.Session.GetString("userid");


                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@comid", com.CompanySecretCode);
                sqlParameter[1] = new SqlParameter("@userid", uid);
                List<UserMenuPermission> userMenus = Helper.ExecProcMapTList<UserMenuPermission>("prcgetMenuPermission", sqlParameter);


                // set session Usermenu

                if (userMenus.Count > 0)
                {
                    HttpContext.Session.SetObject("UserMenu", userMenus);

                    var ModuleMenuCaption = userMenus.Where(x => x.Visible == true).Distinct().Select(x => x.ModuleMenuCaption).FirstOrDefault();
                    var activemoduleid = userMenus.Where(x => x.Visible == true).Distinct().Select(x => x.ModuleMenuCaption).FirstOrDefault();

                    HttpContext.Session.SetObject("activemodulename", ModuleMenuCaption);
                    HttpContext.Session.SetObject("activemoduleid", activemoduleid);
                    HttpContext.Session.SetObject("Modules", db.Modules.ToList());

                }




                MenuPermission_Master master = db.MenuPermissionMasters.Where(m => m.useridPermission == uid).FirstOrDefault();

                if (master != null)
                {

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

                        }).ToList();
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
                        HttpContext.Session.SetInt32("activemenuid", 1);
                    }
                }

                return Json(new { success = 1 });
                //return View("Index");
            }


            return Json(new { error = 0 });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            var ehf = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ViewData["ErrorMessage"] = ehf.Error.Message;
            ViewData["ErrorInnerException"] = ehf.Error.InnerException;
            //ViewData["ErrorLocation"] = ehf.Path;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }



    public class Softwares
    {
        public string AppKey { get; set; }
        public string Name { get; set; }
        public string ComId { get; set; }
        public string VersionName { get; set; }
    }

    public class ChangeComModel
    {
        public string ComId { get; set; }
    }

    public class UserRoles
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string ComId { get; set; }
    }
    //------------Code for Certificate System----------------



}


