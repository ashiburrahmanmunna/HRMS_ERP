#region Using Directive
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace GTERP
{
    public class OverridableAuthorize : Attribute, IAuthorizationFilter
    {
        #region commented code
        //private GTRDBContext db = new GTRDBContext();
        //private readonly GTRDBContext _dbContext = new GTRDBContext();
        ////private LCType abc = db.LCTypes.FirstOrDefault();
        //MenuPermission_Details filterpermission = new MenuPermission_Details(); 
        #endregion
        private readonly IServiceProvider _serviceProvider;

        #region Commented Code
        //public OverridableAuthorize(IServiceProvider serviceProvider)
        //{
        //    _serviceProvider = serviceProvider;
        //    var x = db.MenuPermissionDetails.ToList();
        //}

        //public OverridableAuthorize(GTRDBContext context)
        //{
        //    db = context;

        //} 
        #endregion
        private GTRDBContext db { get; set; }
        public OverridableAuthorize() { }
        public OverridableAuthorize(GTRDBContext _db)
        {
            db = _db;

        }
        //private static IServiceProvider _provider;


        //private MenuPermission_Details filterpermission { get; set; }
        private UserMenuPermission filterpermission { get; set; }

        //private bool IsAccess()
        //{
        //    var abc =  db.HR_AttFixed.ToList();

        //    return true;
        //}
        public void OnAuthorization(AuthorizationFilterContext filterContext)//, IServiceProvider serviceProvider
        {
            int clickedMenuId = int.Parse(filterContext.HttpContext.Session.GetInt32("ActiveModuleMenuId").ToString());
            //var clickedMeduleMenuId = filterContext.HttpContext.Session.GetString("MenuPermissionId");
            filterContext.HttpContext.Session.SetString("Status", "");
            filterContext.HttpContext.Session.SetString("Message", "");


            // GTRDBContext context = new GTRDBContext();

            #region commented Code
            //_dbContext =new GTRDBContext();
            //var dbContext = _serviceProvider.GetRequiredService<GTRDBContext>();
            //var x = dbContext.MenuPermissionDetails.FirstOrDefault();

            //using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{

            //}
            //using (var abccontext = _serviceProvider.GetService<GTRDBContext>())
            //{
            //    var test =  abccontext.MenuPermissionDetails.ToList();

            //    // do stuff
            //}

            //var options = _serviceProvider.GetService<DbContextOptions<GTRDBContext>>();

            ///need to check the comid
            //string userId = HttpContext.Session.GetString("userid"); 
            #endregion

            ////////// for db access
            //db = filterContext.HttpContext.RequestServices.GetRequiredService<GTRDBContext>();

            string userId = filterContext.HttpContext.Session.GetString("userid");
            var comid = filterContext.HttpContext.Session.GetString("comid");



            //string securitystamp = filterContext.HttpContext.Session.GetString("securitystamp");
            #region securitycheck
            //WebHelper webHelper = new WebHelper();

            ////string request = JsonConvert.SerializeObject(Input);

            ////Login
            //Uri url = new Uri(($"https://localhost:5001/api/User/CheckUserActivity?UserId={userId}&UserValue={securitystamp}"));

            ////Uri url = new Uri(string.Format("http://101.2.165.187:82/api/User/securitycheck"));
            //// Uri url = new Uri(string.Format("https://localhost:5000/api/User/securitycheck"));
            ////Uri url = new Uri(string.Format("http://pqstec.com:82/Api/User/securitycheck")); /// without ssl certificate
            ////Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/securitycheck")); ///enable ssl certificate for secure connection
            ////okcode///Uri url = new Uri(string.Format("https://www.gtrbd.net/Support/api/AccountAPI/securitycheck"));
            ////Uri url = new Uri(string.Format("http://gtrbd.net:93/api/User/securitycheck"));


            //var res = "0";

            //using (var client=new HttpClient())
            //{
            //  res= client.GetStringAsync(url).Result;

            //}

            //if (res == "0")
            //{
            //    //filterContext.Result = new RedirectResult("~/Identity/Account/Logout");

            //    //filterContext.Result = new RedirectToRouteResult(
            //    //new RouteValueDictionary{
            //    //        { "Areas","Identity"},{ "controller", "Account" },
            //    //                      { "action", "Logout" }});

            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary{{ "controller", "Home" },
            //                          { "action", "AccessDenied" }, {"null","null" } });

            //}


            #endregion

            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;

            string controller = controllerActionDescriptor?.ControllerName;
            //var action = filterContext.ActionDescriptor;
            var action = controllerActionDescriptor?.ActionName;

            if (userId == null || userId == "")
            {
                filterContext.HttpContext.Session.SetString("Status", "4");
                filterContext.HttpContext.Session.SetString("Message", "Access Denied");

                filterContext.HttpContext.Session.SetString("gotourl", filterContext.HttpContext.Request.GetDisplayUrl());

                //filterContext.Result = new RedirectToRouteResult(
                //   new RouteValueDictionary{{ "controller", "Home" },
                //                      { "action", "LoginPage" },
                //       {"null","null" }
                //                    });
                filterContext.Result = new RedirectResult("~/Identity/Account/Login");

                //filterContext.Result = new RedirectToRouteResult("/Account/Login1", new { area = "Identity" });

                //base.OnAuthorization(filterContext);
                //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Headers["Referer"].ToString());

            }
            else
            {

                // string isactivemodule = filterContext.HttpContext.Session.GetInt32("isactivemodule").ToString();

                //var abc = IsAccess();

                //if (action.IsDefined(typeof(IgnoreAuthorization), true)) return;

                //var controller = action.ControllerDescriptor;

                //var AllMenus = HttpContext.Current.Session["Menus"] as List<GTERP.Models.UserMenuPermission>;
                var AllMenus = filterContext.HttpContext.Session.GetObject<List<UserMenuPermission>>("UserMenu");
                var AllMenusAction = filterContext.HttpContext.Session.GetObject<List<Module_Menu_Action>>("UserMenuAction");
                var AllMenus2 = AllMenus.Where(x => x.ComId == comid && x.ModuleMenuId==clickedMenuId && x.UserIdPermission == userId ).ToList();


                var menucountforpermission = AllMenusAction.Where(x => x.ActionName == action).ToList().FirstOrDefault();
                int? permissioncount = null; 

                if (menucountforpermission != null)
                {
                     AllMenus2 = AllMenus.Where(x => x.ModuleMenuId == menucountforpermission.ModuleMenuId && x.ComId == comid ).ToList();


                    if (menucountforpermission.ActionName.ToUpper().Contains("Create".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsCreate == true && x.ComId == comid).Count();
                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Edit".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsEdit == true && x.ComId == comid).Count();
                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Save".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsCreate == true && x.ComId == comid).Count();
                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Save".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsEdit == true && x.ComId == comid).Count();
                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Update".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsEdit == true && x.ComId == comid).Count();
                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Delete".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsDelete == true && x.ComId == comid).Count();
                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("List".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsView == true && x.ComId == comid).Count();

                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Index".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsView == true && x.ComId == comid).Count();

                    }
                    else if (menucountforpermission.ActionName.ToUpper().Contains("Get".ToUpper()))
                    {
                        permissioncount = AllMenus2.Where(x => x.IsView == true && x.ComId == comid).Count();

                    }                 

                    //AllMenus.OrderBy(x => x.ModuleMenuId);

                    //var isView = AllMenus.Where(x => x.ModuleMenuId == clickedMenuId && x.IsView==true).FirstOrDefault();
                    //var isEdit = AllMenus.Where(x => x.ModuleMenuId == clickedMenuId && x.MenuPermissionId==152 && x.IsEdit == true).FirstOrDefault();

                    ///var isdefaultmodule = AllMenus.Where(x => x.IsDefault == true).Select(x => x.ModuleId).FirstOrDefault().ToString(); /// by fahad and shoriot

                    //if (isactivemodule == null || isactivemodule == "")
                    //{
                    //    //isactivemodule = AllMenus.Where(x => x.Visible == true).Select(x => x.ModuleId).FirstOrDefault().ToString();

                    //    var moduleId = AllMenus.Where(m => m.ModuleMenuController == controller).Select(m => m.ModuleId).FirstOrDefault();

                    //    filterContext.HttpContext.Session.SetInt32("isactivemodule", moduleId);
                    //    var abc = db.Modules.Where(x => x.ModuleId == moduleId).Select(x => x.ModuleName).FirstOrDefault().ToString();
                    //    filterContext.HttpContext.Session.SetInt32("activemenuid", moduleId);
                    //    filterContext.HttpContext.Session.SetString("activemodulename", abc);

                    //}

                    var isactivemodule = filterContext.HttpContext.Session.GetInt32("isactivemodule");

                    if (isactivemodule == null)
                    {
                        isactivemodule = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower()).FirstOrDefault().ModuleId;

                    }

                    var modules = filterContext.HttpContext.Session.GetObject<List<Module>>("modules");

                    if (isactivemodule != 0)
                    {
                        filterContext.HttpContext.Session.SetInt32("isactivemodule", (int)isactivemodule);

                        if (modules != null)
                        {
                            var abc = modules.Where(x => x.ModuleId == isactivemodule).FirstOrDefault();
                            filterContext.HttpContext.Session.SetInt32("activemenuid", (int)isactivemodule);
                            filterContext.HttpContext.Session.SetString("activemodulename", abc.ModuleName);
                        }
                    }

                    if (AllMenus != null)
                    {
                        AllMenus.ToList().ForEach
                                                (c =>
                                                {
                                                    c.Active = false; c.Visible = false;
                                                });

                        AllMenus.Where(x => x.ModuleId == isactivemodule).ToList().ForEach(c => c.Visible = true);

                        int parentId = 0;
                        if (clickedMenuId > 0)
                        {
                            parentId = AllMenus.Where(m => m.ModuleMenuId == clickedMenuId).FirstOrDefault().ParentId;

                        }
                        else
                        {
                            parentId = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower()).FirstOrDefault().ParentId;

                        }

                        if (action.ToLower() == "BBLCReport".ToLower())
                        {
                            AllMenus.Where(m => m.ModuleMenuController.ToLower() == "BBLCReport".ToLower()).FirstOrDefault().Active = true;

                        }
                        else if (action.ToLower() == "ExportShippingReport".ToLower())
                        {
                            AllMenus.Where(m => m.ModuleMenuController.ToLower() == "ExportShippingReport".ToLower()).FirstOrDefault().Active = true;
                        }
                        else
                        {
                            if (clickedMenuId > 0)
                            {
                                AllMenus.Where(m => m.ModuleMenuId == clickedMenuId).FirstOrDefault().Active = true;

                            }
                            else
                            {
                                AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower()).FirstOrDefault().Active = true;

                            }
                        }
                        AllMenus.Where(m => m.ModuleMenuId == parentId).FirstOrDefault().Active = true;
                        //AllMenus.Where(m => m.ModuleMenuId == clickedMenuId).FirstOrDefault().Active = true;
                    }
                    
                }

                if (action.ToUpper() == "Dashboard".ToUpper())
                {
                    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToUpper() == controller.ToUpper() && m.ModuleMenuLink.ToUpper().Contains("DASHBOARD".ToUpper())).FirstOrDefault();
                    filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                }
                
                else
                {
                    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower()).FirstOrDefault();
                    filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                }
                //using (var scope = _serviceProvider.GetService())
                //{
                //    var dbContext = scope.ServiceProvider.GetRequiredService<SomeDbContext>();

                //    //...
                //}

                //var activemenuname = AllMenus.Where(x => x.Visible == true).Select(x => x.ModuleMenuCaption).FirstOrDefault().ToString();

                //var isView = db.MenuPermissionDetails
                //                .Include(x => x.MenuPermissionMasters)
                //                .Include(x => x.ModuleMenus)
                //                .Where(x => x.MenuPermissionMasters.comid == comid &&
                //                x.MenuPermissionMasters.useridPermission == userId &&
                //                x.ModuleMenus.ModuleMenuId == clickedMenuId &&
                //                x.IsView == true)
                //                .Select(x => x.IsDelete)
                //                .ToList();

                //if (isView.IsView==true)
                //{
                //    if (clickedMenuId > 0)
                //    {
                //        filterpermission = AllMenus.Where(m => m.ModuleMenuId == clickedMenuId).FirstOrDefault();
                //    }
                //    else
                //    {
                //        filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsView == true).OrderByDescending(x => x.ModuleId).FirstOrDefault();

                //    }
                //    filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                //}
                //else if (action == "Create")
                //{
                //    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsCreate == true).FirstOrDefault();

                //    if (filterpermission != null)
                //    {
                //        //filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsCreate == true).FirstOrDefault();
                //        filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);
                //    }
                //}

                ////var isEdit = db.MenuPermissionDetails
                ////                .Include(x => x.MenuPermissionMasters)
                ////                .Include(x => x.ModuleMenus)
                ////                .Where(x => x.MenuPermissionMasters.comid == comid &&
                ////                x.MenuPermissionMasters.useridPermission == userId &&
                ////                x.ModuleMenus.ModuleMenuId == clickedMenuId &&
                ////                x.IsEdit == true)
                ////                .Select(x => x.IsDelete)
                ////                .FirstOrDefault();

                //if (isEdit!=null && isEdit.IsEdit!=false)
                //{
                //    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsEdit == true).FirstOrDefault();
                //    if (filterpermission != null)
                //    {
                //        filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                //    }
                //}
                //else
                //{
                //    Console.WriteLine("isEdit is False");
                //}
                //if (action == "Delete")
                //{
                //    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsDelete == true).FirstOrDefault();
                //    if (filterpermission != null)
                //    {
                //        filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                //    }
                //    //filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                //}
                //else if (action == "Report")
                //{
                //    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsReport == true).FirstOrDefault();
                //    filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                //}
                //else if (action.ToUpper() == "Dashboard".ToUpper())
                //{
                //    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToUpper() == controller.ToUpper() && m.ModuleMenuLink.ToUpper().Contains("DASHBOARD".ToUpper())).FirstOrDefault(); //
                //    filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);
                //}
                //else
                //{
                //    filterpermission = AllMenus.Where(m => m.ModuleMenuController.ToLower() == controller.ToLower() && m.IsCreate == true).FirstOrDefault();
                //    filterContext.HttpContext.Session.SetString("activemodulename", filterpermission.ModuleMenuCaption);

                //}

                if (filterpermission == null)
                //if (permissioncount == 0)

                {
                    //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Headers["Path"].ToString());

                    filterContext.HttpContext.Session.SetString("Status", "4");
                    filterContext.HttpContext.Session.SetString("Message", "Access Denied");

                    if (filterContext.HttpContext.Request != null)
                    {
                        //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Headers["Referer"].ToString());

                        //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Host + filterContext.HttpContext.Request.Path);
                        string referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
                        //filterContext.Result = new RedirectResult(referer);//perfect code also /

                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary{{ "controller", "Home" },
                                      { "action", "AccessDenied" }, {"null","null" } });

                        //var z = filterContext.HttpContext.Request.;
                        //filterContext.Result = new RedirectToRouteResult(
                        //new RouteValueDictionary{{ "controller", "Account" },
                        //              { "action", "LoginEnglish" }, {"null","null" } });

                    }
                }
                else if (AllMenus2.Count == 0)
                {
                    filterContext.HttpContext.Session.SetObject("UserMenu", AllMenus);
                    filterContext.Result = null;
                }
                else if (permissioncount == 0)
                {
                    filterContext.HttpContext.Session.SetString("Status", "4");
                    filterContext.HttpContext.Session.SetString("Message", "Access Denied");

                    if (filterContext.HttpContext.Request != null)
                    {
                        string referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();

                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary{{ "controller", "Home" },
                                      { "action", "AccessDenied" }, {"null","null" } });
                    }
                }
                
                

                else
                {
                    filterContext.HttpContext.Session.SetObject("UserMenu", AllMenus);

                    //var abc = AllMenus.Where(x => x.Active == true);
                    //filterContext.HttpContext.Session.SetInt32("ActiveModuleMenuId", 0);
                    filterContext.Result = null;
                }
            }
        }
    }
}

