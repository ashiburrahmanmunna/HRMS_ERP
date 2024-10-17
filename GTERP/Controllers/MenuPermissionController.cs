using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers
{
    //[OverridableAuthorize]
    public class MenuPermissionController : Controller
    {

        private readonly ILogger<MenuPermissionController> _logger;
        private GTRDBContext db;
        private readonly IConfiguration _configuration;

        public MenuPermissionController(IConfiguration configuration, GTRDBContext context, ILogger<MenuPermissionController> logger)
        {
            db = context;
            _logger = logger;
            _configuration = configuration;
        }
        // GET: MenuPermission
        public ActionResult Index()
        {
            var comId = HttpContext.Session.GetString("comid");
            var appKey = HttpContext.Session.GetString("appkey");
            var userid = HttpContext.Session.GetString("userid");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);


            if (res.MyUsers != null || res.Companies != null)
            {
                SqlParameter[] sqlParameter1 = new SqlParameter[3];
                sqlParameter1[0] = new SqlParameter("@Criteria", "MenuPermission");
                sqlParameter1[1] = new SqlParameter("@ComId", comId);
                sqlParameter1[2] = new SqlParameter("@userId", userid);


                List<MenuPermission> menuPermission = Helper.ExecProcMapTList<MenuPermission>("prcgetAspnetUserList", sqlParameter1).ToList();

                var resultUsers = new List<MenuPermission>();
                for (int i = 0; i < res.MyUsers.Count; i++)
                {
                    var seqUserId = res.MyUsers.ElementAt(i).UserID;
                    var UserName = res.MyUsers.ElementAt(i).UserName;
                    var hs = menuPermission.Where(x => x.UserId == seqUserId).ToList();

                    if (hs != null)
                    {
                        hs.ForEach(m => m.CompanyName = db.Companys?.Where(a => a.CompanyCode == m.comid)?.FirstOrDefault()?.CompanyName);
                    }

                    foreach (var menu in hs.Where(x => x.comid == comId))
                    {
                        var resultUser = new MenuPermission();

                        resultUser.UserId = menu.UserId;
                        resultUser.useridPermission = res.MyUsers.Where(w => w.UserID == menu.UserName).Select(s => s.UserName).FirstOrDefault();
                        resultUser.UserName = UserName;
                        resultUser.CompanyName = menu.CompanyName;
                        resultUser.comid = menu.comid;
                        resultUser.Email = UserName;
                        resultUser.MenuPermissionId = menu.MenuPermissionId;
                        resultUser.ReportPermission_MasterId = menu.ReportPermission_MasterId;
                        resultUsers.Add(resultUser);
                    }
                }


                ViewBag.MenuPermission = resultUsers;

            }


            return View();
        }

        public class MenuPermission
        {
            public int ReportPermission_MasterId { get; set; }
            public int MenuPermissionId { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string comid { get; set; }
            public string CompanyName { get; set; }
            public string useridPermission { get; set; }

        }

        public ActionResult UserTransfer(string menuPermissionId, string userIdPermission, string comId)
        {

            String userid = HttpContext.Session.GetString("userid");


            SqlParameter[] sqlParameter2 = new SqlParameter[4];
            sqlParameter2[0] = new SqlParameter("@menuPermissionId", menuPermissionId);
            sqlParameter2[1] = new SqlParameter("@userIdPermission", userIdPermission);
            sqlParameter2[2] = new SqlParameter("@comId", comId);
            sqlParameter2[3] = new SqlParameter("@AddedByUserId", userid);
            var query = $"Exec prcPermissionTransfer '{menuPermissionId}','{userIdPermission}','{comId}' ,'{userid}'";
            Helper.ExecProc("prcPermissionTransfer", sqlParameter2);

            MenuPermission_Master menuPermission_Master = db.MenuPermissionMasters.Include(m => m.MenuPermission_Details).ThenInclude(abc => abc.ModuleMenus).Where(a => a.useridPermission == userIdPermission && a.comid == comId).FirstOrDefault();

            ViewBag.Title = "Edit";

            if (menuPermission_Master != null)
            {
                ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", menuPermission_Master.DefaultModuleId);
            }
            else
            {
                ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption");
            }

            var CompanyList = db.Companys.Where(x => x.IsInActive == false && x.IsGroup == false);
            ViewBag.comid = new SelectList(CompanyList, "CompanyCode", "CompanyName", comId);


            SqlParameter[] sqlParameter3 = new SqlParameter[1];
            sqlParameter3[0] = new SqlParameter("@Criteria", "CompanyPermission");

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));


            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);


            var list = res.MyUsers.ToList();
            var userlist = list.DistinctBy(x => x.UserName);
            var l = new List<AspnetUserList>();
            foreach (var c in userlist)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }

            ViewBag.useridPermission = new SelectList(l, "UserId", "UserName", userIdPermission);
            ViewBag.newUserPermission = new SelectList(l, "UserId", "UserName");

            int softwareId = Convert.ToInt32(HttpContext.Session.GetString("SoftwareId"));
            int versionId = Convert.ToInt32(HttpContext.Session.GetString("VersionId"));

            SqlParameter[] sqlParameter4 = new SqlParameter[6];
            sqlParameter4[0] = new SqlParameter("@ComId", comId);
            sqlParameter4[1] = new SqlParameter("@UserId", userIdPermission);
            sqlParameter4[2] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter4[3] = new SqlParameter("@VersionId", versionId);
            sqlParameter4[4] = new SqlParameter("@ReportPermission_MasterId", "0");
            sqlParameter4[5] = new SqlParameter("@Type", "Menu Permission");
            //var query = $"Exec PrcGetMenuPermissionUser '{comId}','{userIdPermission}',{softwareId} ,{versionId},0,'Menu Permission'";
            List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("PrcGetMenuPermissionUser", sqlParameter4).ToList();
            ViewBag.MenuList = menuPermissionModels;

            return View("Create", menuPermission_Master);
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        public ActionResult Create(string comid, string UserId, int? MenuPermissionId, int? ReportPermission_MasterId, int? isDelete)
        {
            ViewBag.ReportPermission_MasterId = ReportPermission_MasterId;
            comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            var useridsession = HttpContext.Session.GetString("userid");
            int softwareId = Convert.ToInt32(HttpContext.Session.GetString("SoftwareId"));
            int versionId = Convert.ToInt32(HttpContext.Session.GetString("VersionId"));
            var softwareDefaultModuleId = db.VersionMenuPermission_Masters.Where(a => a.SoftwareId == softwareId && a.VersionId == versionId).Select(a => a.DefaultModuleId).FirstOrDefault();
            string useridpermission = useridsession;
            if (UserId == null)
            {
                UserId = userid;

            }

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);

            // for report permission
            if (ReportPermission_MasterId > 0)
            {

                if (res.MyUsers != null || res.Companies != null)
                {
                    if (isDelete == 1)
                    {

                        ViewBag.Title = "Delete";

                    }
                    else
                    {
                        ViewBag.Title = "Edit";

                    }


                    if (ReportPermission_MasterId == null)
                    {
                        return BadRequest();
                    }
                    var userIdMenuPermitted = db.MenuPermissionMasters.Where(a => a.MenuPermissionId == MenuPermissionId).Select(a => a.useridPermission).FirstOrDefault();
                    // right side report table
                    SqlParameter[] sqlParameter5 = new SqlParameter[6];
                    sqlParameter5[0] = new SqlParameter("@ComId", comid);
                    sqlParameter5[1] = new SqlParameter("@UserId", userIdMenuPermitted);
                    sqlParameter5[2] = new SqlParameter("@SoftwareId", "0");
                    sqlParameter5[3] = new SqlParameter("@VersionId", "0");
                    sqlParameter5[4] = new SqlParameter("@ReportPermission_MasterId", ReportPermission_MasterId);
                    sqlParameter5[5] = new SqlParameter("@Type", "Permitted Report List");

                    //var query = $"Exec PrcGetMenuPermissionUser '{comid}','{userIdMenuPermitted}',0 ,0,{ReportPermission_MasterId},'Permitted Report List'";
                    List<ExistReportPermissionsVM> reportPermission_Master = Helper.ExecProcMapTList<ExistReportPermissionsVM>("PrcGetMenuPermissionUser", sqlParameter5);

                    ViewBag.ReportPermission = reportPermission_Master;

                    if (reportPermission_Master == null)
                    {
                        return NotFound();
                    }

                    // for report permission
                    // left side table
                    SqlParameter[] sqlParameter6 = new SqlParameter[6];
                    sqlParameter6[0] = new SqlParameter("@ComId", comid);
                    sqlParameter6[1] = new SqlParameter("@UserId", userIdMenuPermitted);
                    sqlParameter6[2] = new SqlParameter("@SoftwareId", softwareId);
                    sqlParameter6[3] = new SqlParameter("@VersionId", versionId);
                    sqlParameter6[4] = new SqlParameter("@ReportPermission_MasterId", ReportPermission_MasterId);
                    sqlParameter6[5] = new SqlParameter("@Type", "Available Report List");

                    var Sqlquery = $"Exec PrcGetMenuPermissionUser '{comid}','{userIdMenuPermitted}',{softwareId} ,{versionId},{ReportPermission_MasterId},'Available Report List'";

                    List<ReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<ReportPermissionsVM>("PrcGetMenuPermissionUser", sqlParameter6);
                    ViewBag.ReportList = ListOfReportPermission;


                }

            }

            else
            {

                ViewBag.Title = "Create";

                SqlParameter[] sqlParameter7 = new SqlParameter[6];
                sqlParameter7[0] = new SqlParameter("@ComId", comid);
                sqlParameter7[1] = new SqlParameter("@UserId", UserId);
                sqlParameter7[2] = new SqlParameter("@SoftwareId", softwareId);
                sqlParameter7[3] = new SqlParameter("@VersionId", versionId);
                sqlParameter7[4] = new SqlParameter("@ReportPermission_MasterId", "0");
                sqlParameter7[5] = new SqlParameter("@Type", "All Report");

                var query = $"Exec PrcGetMenuPermissionUser '{comid}','{userid}',{softwareId} ,{versionId},0,'All Report'";
                List<ReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<ReportPermissionsVM>("PrcGetMenuPermissionUser", sqlParameter7);
                ViewBag.ReportList = ListOfReportPermission;


            }

            // for menu permission

            if (MenuPermissionId > 0)
            {

                if (res.MyUsers != null || res.Companies != null)
                {

                    if (isDelete == 1)
                    {

                        ViewBag.Title = "Delete";

                    }
                    else
                    {
                        ViewBag.Title = "Edit";

                    }


                    if (MenuPermissionId == null)
                    {
                        return BadRequest();
                    }

                    MenuPermission_Master menuPermission_Master = db.MenuPermissionMasters.Include(m => m.MenuPermission_Details).ThenInclude(abc => abc.ModuleMenus).Where(m => m.MenuPermissionId.ToString() == MenuPermissionId.ToString()).FirstOrDefault();

                    var userIdMenuPermitted = db.MenuPermissionMasters.Where(a => a.MenuPermissionId == MenuPermissionId).Select(a => a.useridPermission).FirstOrDefault();

                    if (menuPermission_Master == null)
                    {
                        return NotFound();
                    }



                    if (UserId == null)
                    {
                        UserId = HttpContext.Session.GetString("userid");


                    }
                    if (comid == null)
                    {
                        comid = HttpContext.Session.GetString("comid");

                    }

                    var list = res.MyUsers.ToList();
                    var l = new List<AspnetUserList>();
                    foreach (var c in list)
                    {
                        var le = new AspnetUserList();
                        le.Email = c.UserName;
                        le.UserId = c.UserID;
                        le.UserName = c.UserName;

                        l.Add(le);
                    }
                    var mas = db.MenuPermissionMasters.Where(x => x.comid == comid && x.useridPermission == UserId && x.DefaultModuleId != null).FirstOrDefault();

                    if (mas == null)
                    {

                        ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", softwareDefaultModuleId);
                    }
                    else
                    {

                        var defaultmodule = db.Modules.Where(x => x.ModuleId == mas.DefaultModuleId).FirstOrDefault();
                        if (defaultmodule == null)
                        {
                            ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", softwareDefaultModuleId);

                        }
                        else
                        {
                            ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", softwareDefaultModuleId);

                        }

                    }
                    ViewBag.useridPermission = new SelectList(l, "UserId", "UserName", useridpermission);
                    ViewBag.newUserPermission = new SelectList(l, "UserId", "UserName", UserId);
                    //Right table
                    SqlParameter[] sqlParameter8 = new SqlParameter[6];
                    sqlParameter8[0] = new SqlParameter("@ComId", comid);
                    sqlParameter8[1] = new SqlParameter("@UserId", userIdMenuPermitted);
                    sqlParameter8[2] = new SqlParameter("@SoftwareId", softwareId);
                    sqlParameter8[3] = new SqlParameter("@VersionId", versionId);
                    sqlParameter8[4] = new SqlParameter("@ReportPermission_MasterId", "0");
                    sqlParameter8[5] = new SqlParameter("@Type", "Menu Permission");

                    //var query = $"Exec PrcGetMenuPermissionUser '{comid}','{userIdMenuPermitted}',{softwareId} ,{versionId},0,'Menu Permission'";
                    List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("PrcGetMenuPermissionUser", sqlParameter8).ToList();

                    ViewBag.MenuList = menuPermissionModels;


                    string comId = HttpContext.Session.GetString("comid");

                    var CompanyList = db.Companys.Where(x => x.IsInActive == false && x.IsGroup == false && x.CompanyCode == comId).ToList();
                    ViewBag.comid = new SelectList(CompanyList, "CompanyCode", "CompanyName", comid);


                    return View("Create", menuPermission_Master);

                }

            }

            else
            {

                if (UserId == null)
                {
                    UserId = HttpContext.Session.GetString("userid");

                }
                if (comid == null)
                {
                    comid = HttpContext.Session.GetString("comid");
                }

                ViewBag.Title = "Create";
                var list = res.MyUsers.ToList();
                var l = new List<AspnetUserList>();
                foreach (var c in list)
                {
                    var le = new AspnetUserList();
                    le.Email = c.UserName;
                    le.UserId = c.UserID;
                    le.UserName = c.UserName;
                    l.Add(le);
                }

                ViewBag.useridPermission = new SelectList(l, "UserId", "UserName", UserId);
                ViewBag.newUserPermission = new SelectList(l, "UserId", "UserName");
                if (comid != null && UserId != null)
                {

                    MenuPermission_Master menuPermission_Master = db.MenuPermissionMasters.Include(m => m.MenuPermission_Details).ThenInclude(abc => abc.ModuleMenus)
                        .ThenInclude(x => x.ParentModuleMenu).Where(m => m.comid == comid && m.useridPermission == UserId).FirstOrDefault();
                    if (menuPermission_Master != null)
                    {
                        ViewBag.Title = "Edit";
                        ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", softwareDefaultModuleId);
                    }
                    else
                    {
                        ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption");
                    }


                    string comId = HttpContext.Session.GetString("comid");

                    var CompanySelectList = db.Companys.Where(x => x.IsInActive == false && x.IsGroup == false && x.CompanyCode == comId).ToList();
                    ViewBag.comid = new SelectList(CompanySelectList, "CompanySecretCode", "CompanyName", comid);

                    SqlParameter[] sqlParameter9 = new SqlParameter[6];
                    sqlParameter9[0] = new SqlParameter("@ComId", comid);
                    sqlParameter9[1] = new SqlParameter("@UserId", userid);
                    sqlParameter9[2] = new SqlParameter("@SoftwareId", softwareId);
                    sqlParameter9[3] = new SqlParameter("@VersionId", versionId);
                    sqlParameter9[4] = new SqlParameter("@ReportPermission_MasterId", "0");
                    sqlParameter9[5] = new SqlParameter("@Type", "Menu Permission");
                    var query = $"Exec PrcGetMenuPermissionUser '{comid}','{userid}',{softwareId} ,{versionId},0,'Menu Permission'";
                    List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("PrcGetMenuPermissionUser", sqlParameter9).ToList();

                    ViewBag.MenuList = menuPermissionModels;


                    return View(menuPermission_Master);
                }

                ViewBag.MenuList = null;
                List<CompanyList> CompanyList = new List<CompanyList>();
                foreach (var company in res.Companies)
                {
                    var com = new CompanyList();
                    com.ComId = company.ComId;
                    com.CompanyShortName = company.CompanyName;

                    CompanyList.Add(com);
                }


            }


            return View();


        }

        // for menu permission

        #region For Menu Permission Create

        [HttpPost]
        [RequestSizeLimit(73400320)]
        public ActionResult Create(MenuPermission_Master menuPermission_Master)
        {
            try
            {

                //var a =   menuPermission_Master;
                if ((HttpContext.Session.GetString("comid")) == "0" || (HttpContext.Session.GetString("comid")) == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });
                }
                ViewBag.Title = "Create";

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    if (menuPermission_Master.MenuPermissionId > 0)
                    {
                        menuPermission_Master.DateUpdated = DateTime.Now;
                        menuPermission_Master.useridUpdate = HttpContext.Session.GetString("userid");


                        if (menuPermission_Master.userid == null || menuPermission_Master.userid == "0")
                        {
                            menuPermission_Master.userid = HttpContext.Session.GetString("userid");

                        }
                        if (menuPermission_Master.comid == "")
                        {
                            menuPermission_Master.comid = HttpContext.Session.GetString("comid");
                        }

                        IQueryable<MenuPermission_Details> menuPermission_Detailses = db.MenuPermissionDetails.Where(p => p.MenuPermissionId == menuPermission_Master.MenuPermissionId);

                        db.MenuPermissionDetails.RemoveRange(menuPermission_Detailses);

                        if (menuPermission_Master.MenuPermission_Details != null)
                        {
                            foreach (MenuPermission_Details ss in menuPermission_Master.MenuPermission_Details)
                            {
                                ss.DateAdded = DateTime.Now;
                                ss.DateUpdated = DateTime.Now;

                                db.MenuPermissionDetails.Add(ss);
                                //db.SaveChanges();
                            }
                            db.SaveChanges();

                        }
                        db.Entry(menuPermission_Master).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        menuPermission_Master.DateAdded = DateTime.Now;
                        menuPermission_Master.DateUpdated = DateTime.Now;
                        menuPermission_Master.userid = (HttpContext.Session.GetString("userid"));
                        menuPermission_Master.useridPermission = menuPermission_Master.useridPermission;

                        db.MenuPermissionMasters.Add(menuPermission_Master);
                    }

                    db.SaveChanges();
                }
                return Json(new { Success = 1 });


            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    errors = ex.Message
                });
            }
        }

        #endregion

        // for report permission

        #region Report Permission Create

        [HttpPost]
        [RequestSizeLimit(73400320)]
        public IActionResult CreateReport(ReportPermission_Master reportPermission_Master)
        {
            try
            {
                //var a =   menuPermission_Master;
                if ((HttpContext.Session.GetString("comid")) == "0" || (HttpContext.Session.GetString("comid")) == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });
                }
                ViewBag.Title = "Create";

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });


                if (reportPermission_Master.ReportPermission_MasterId > 0)
                {
                    reportPermission_Master.DateUpdated = DateTime.Now;
                    reportPermission_Master.UserId = HttpContext.Session.GetString("userid");
                    reportPermission_Master.Updatedby = HttpContext.Session.GetString("userid");
                    reportPermission_Master.UpdateByUser = HttpContext.Session.GetString("userid");


                    if (reportPermission_Master.UserId == null || reportPermission_Master.UserId == "0")
                    {
                        reportPermission_Master.UserId = HttpContext.Session.GetString("userid");
                    }


                    IQueryable<ReportPermission_Details> reportPermission_Details = db.ReportPermission_Details.Where(p => p.ReportPermission_MasterId == reportPermission_Master.ReportPermission_MasterId);

                    db.ReportPermission_Details.RemoveRange(reportPermission_Details);

                    if (reportPermission_Master.ReportPermission_Details != null)
                    {
                        foreach (ReportPermission_Details ss in reportPermission_Master.ReportPermission_Details)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            db.ReportPermission_Details.Add(ss);
                        }
                        db.SaveChanges();

                    }
                    db.Entry(reportPermission_Master).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    reportPermission_Master.DateAdded = DateTime.Now;
                    reportPermission_Master.DateUpdated = DateTime.Now;
                    reportPermission_Master.UserId = (HttpContext.Session.GetString("userid"));
                    reportPermission_Master.useridPermission = reportPermission_Master.useridPermission;
                    db.ReportPermission_Masters.Add(reportPermission_Master);
                }

                db.SaveChanges();
                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    errors = ex.Message

                });
            }
        }

        #endregion

        #region Version Apps Permission

        public ActionResult VersionMenuList()
        {
            WebHelper webHelper = new WebHelper();

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetSoftwareVersion")));

            string response = webHelper.Get(url);

            GetSoftwareResponse res = new GetSoftwareResponse(response);



            List<SoftwareVersion> SoftwareVersionList = Helper.ExecProcMapTList<SoftwareVersion>("PrcGetVersionPermission").ToList();


            var softwareVersionData = new List<SoftwareVersion>();

            if (res.Softwares != null || res.SoftwareVersions != null)
            {
                foreach (var item in SoftwareVersionList)
                {
                    var versionMenu = new SoftwareVersion();
                    versionMenu.MenuPermissionId = item.MenuPermissionId;
                    versionMenu.VersionReportMasterId = item.VersionReportMasterId;
                    versionMenu.SoftwareId = item.SoftwareId;
                    versionMenu.VersionId = item.VersionId;
                    versionMenu.SoftwareName = res.Softwares.Where(x => x.Id == item.SoftwareId).Select(x => x.Name).FirstOrDefault();
                    versionMenu.VersionName = res.SoftwareVersions.Where(x => x.Id == item.VersionId).Select(x => x.VersionName).FirstOrDefault();


                    softwareVersionData.Add(versionMenu);
                }

            }

            ViewBag.versionList = softwareVersionData;

            return View();
        }

        public ActionResult VersionMenu(int softwareId, int versionId, int? MenuPermissionId, int? VersionReportMasterId, int? isDelete)
        {
            var appKey = HttpContext.Session.GetString("appkey");
            WebHelper webHelper = new WebHelper();

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetSoftwareVersion")));

            string response = webHelper.Get(url);

            GetSoftwareResponse res = new GetSoftwareResponse(response);

            if (VersionReportMasterId > 0)
            {

                if (res.Softwares != null || res.SoftwareVersions != null)
                {
                    if (isDelete == 1)
                    {

                        ViewBag.Title = "Delete";

                    }
                    else
                    {
                        ViewBag.Title = "Edit";

                    }


                    if (VersionReportMasterId == null)
                    {
                        return BadRequest();
                    }


                    SqlParameter[] sqlparameters = new SqlParameter[1];
                    sqlparameters[0] = new SqlParameter("@VersionReportMasterId", VersionReportMasterId);

                    List<ExistReportPermissionsVM> reportPermission_Master = Helper.ExecProcMapTList<ExistReportPermissionsVM>("prcVersionWiseExistReportPermission", sqlparameters);

                    ViewBag.ReportPermission = reportPermission_Master;
                    if (reportPermission_Master == null)
                    {
                        return NotFound();
                    }

                    // for report permission

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@SoftwareId", softwareId);
                    parameters[1] = new SqlParameter("@VersionId", versionId);

                    List<ReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<ReportPermissionsVM>("prcVersionWiseReportPermission", parameters);
                    ViewBag.ReportList = ListOfReportPermission;


                }

            }

            else
            {


                ViewBag.Title = "Create";


                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@SoftwareId", softwareId);
                parameters[1] = new SqlParameter("@VersionId", versionId);

                List<ReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<ReportPermissionsVM>("prcVersionWiseReportPermission", parameters);
                ViewBag.ReportList = ListOfReportPermission;

            }

            if (MenuPermissionId > 0)
            {

                if (res.Softwares != null || res.SoftwareVersions != null)
                {

                    if (isDelete == 1)
                    {

                        ViewBag.Title = "Delete";

                    }
                    else
                    {
                        ViewBag.Title = "Edit";

                    }


                    if (MenuPermissionId == null)
                    {
                        return BadRequest();
                    }




                    VersionMenuPermission_Master menuPermission_Master = db.VersionMenuPermission_Masters.Include(m => m.VersionMenuPermission_Details).ThenInclude(abc => abc.ModuleMenus).Where(m => m.MenuPermissionId.ToString() == MenuPermissionId.ToString()).FirstOrDefault();
                    if (menuPermission_Master == null)
                    {
                        return NotFound();
                    }



                    var list = res.SoftwareVersions.ToList();

                    var menuAccess = db.VersionMenuPermission_Masters.Where(x => x.SoftwareId == softwareId && x.VersionId == versionId /*&& x.DefaultModuleId != null*/).FirstOrDefault();

                    if (menuAccess == null)
                    {

                        ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption");
                    }
                    else
                    {
                        var defaultmodule = db.Modules.Where(x => x.ModuleId == menuAccess.DefaultModuleId).FirstOrDefault();
                        if (defaultmodule == null)
                        {
                            ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption");

                        }
                        else
                        {
                            ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", defaultmodule.ModuleId);

                        }

                    }

                    ViewBag.softwareVersions = new SelectList(list, "Id", "VersionName", versionId);
                    string comId = HttpContext.Session.GetString("comid");
                    string userid = HttpContext.Session.GetString("userid");

                    SqlParameter[] sqlParameterversion = new SqlParameter[4];
                    sqlParameterversion[0] = new SqlParameter("@SoftwareId", softwareId);
                    sqlParameterversion[1] = new SqlParameter("@VersionId", versionId);
                    sqlParameterversion[2] = new SqlParameter("@ComId", comId);
                    sqlParameterversion[3] = new SqlParameter("@UserId", userid);
                    List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("MenuPermissionVersion", sqlParameterversion).ToList();

                    ViewBag.MenuList = menuPermissionModels;




                    /////////////////////////////////////////////////////////////////////
                    var CompanyList = db.Companys.Where(x => x.IsInActive == false && x.IsGroup == false && x.CompanySecretCode == comId).ToList();

                    var softwareList = res.Softwares.ToList();
                    //ViewBag.comid = new SelectList(CompanyList, "ComId", "CompanyShortName", comid);
                    ViewBag.softwares = new SelectList(softwareList, "Id", "Name", softwareId);


                    return View(menuPermission_Master);

                }

            }

            else
            {

                ViewBag.Title = "Create";
                var list = res.SoftwareVersions.ToList();

                if (versionId != 0)
                {
                    ViewBag.softwareVersions = new SelectList(list, "Id", "VersionName", versionId);
                }
                else
                {
                    ViewBag.softwareVersions = new SelectList(list, "Id", "VersionName");
                }


                VersionMenuPermission_Master menuPermission_Master = db.VersionMenuPermission_Masters.Include(m => m.VersionMenuPermission_Details).ThenInclude(abc => abc.ModuleMenus).Where(m => m.SoftwareId == softwareId && m.VersionId == versionId).FirstOrDefault();


                if (menuPermission_Master != null)
                {
                    ViewBag.Title = "Edit";
                    ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption", menuPermission_Master.DefaultModuleId);
                }
                else
                {
                    ViewBag.ModuleList = new SelectList(db.Modules, "ModuleId", "ModuleCaption");

                }


                string comId = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");
                //var CompanySelectList = db.Companys.Where(x => x.IsInActive == false && x.IsGroup == false && x.CompanySecretCode == comId).ToList();

                var softwareList = res.Softwares.ToList();

                if (softwareId != 0)
                {

                    ViewBag.softwares = new SelectList(softwareList, "Id", "Name", softwareId);
                }
                else
                {
                    ViewBag.softwares = new SelectList(softwareList, "Id", "Name");
                }



                SqlParameter[] sqlParameterPermissionVersion = new SqlParameter[4];
                sqlParameterPermissionVersion[0] = new SqlParameter("@SoftwareId", softwareId);
                sqlParameterPermissionVersion[1] = new SqlParameter("@VersionId", versionId);
                sqlParameterPermissionVersion[2] = new SqlParameter("@ComId", comId);
                sqlParameterPermissionVersion[3] = new SqlParameter("@UserId", userid);
                List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("MenuPermissionVersion", sqlParameterPermissionVersion).ToList();

                ViewBag.MenuList = menuPermissionModels;
                ///////////////////////////////////////////////////////////////////////////////////
                return View(menuPermission_Master);


                ViewBag.MenuList = null;

            }

            return View();
        }

        [HttpPost]
        [RequestSizeLimit(73400320)]
        public ActionResult VersionMenu(VersionMenuPermission_Master menuPermission_Master)
        {
            try
            {
                //var a =   menuPermission_Master;
                if ((HttpContext.Session.GetString("comid")) == "0" || (HttpContext.Session.GetString("comid")) == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });
                }
                ViewBag.Title = "Create";
                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                //if (!UserAccess(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString()))
                //{
                //    return NotFound();
                //}

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    if (menuPermission_Master.MenuPermissionId > 0)
                    {
                        menuPermission_Master.DateUpdated = DateTime.Now;
                        //cOM_MachinaryLC_Master.DateAdded = DateTime.Now;

                        //menuPermission_Master.comid = HttpContext.Session.GetString("comid");
                        menuPermission_Master.useridUpdate = HttpContext.Session.GetString("userid");


                        if (menuPermission_Master.userid == null || menuPermission_Master.userid == "0")
                        {
                            menuPermission_Master.userid = HttpContext.Session.GetString("userid");
                        }
                        if (menuPermission_Master.SoftwareId == 0)
                        {
                            menuPermission_Master.SoftwareId = (int)HttpContext.Session.GetInt32("softwareId");
                        }

                        IQueryable<VersionMenuPermission_Details> menuPermission_Detailses = db.VersionMenuPermission_Details.Where(p => p.MenuPermissionId == menuPermission_Master.MenuPermissionId);

                        db.VersionMenuPermission_Details.RemoveRange(menuPermission_Detailses);
                        //foreach (MenuPermission_Details ss in menuPermission_Detailses)
                        //{
                        //    db.MenuPermissionDetails.Remove(ss);
                        //}

                        if (menuPermission_Master.VersionMenuPermission_Details != null)
                        {
                            foreach (VersionMenuPermission_Details ss in menuPermission_Master.VersionMenuPermission_Details)
                            {
                                ss.DateAdded = DateTime.Now;
                                ss.DateUpdated = DateTime.Now;

                                db.VersionMenuPermission_Details.Add(ss);
                                //db.SaveChanges();
                            }
                            db.SaveChanges();

                        }
                        db.Entry(menuPermission_Master).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        menuPermission_Master.DateAdded = DateTime.Now;
                        menuPermission_Master.DateUpdated = DateTime.Now;
                        //menuPermission_Master.comid = HttpContext.Session.GetString("comid");
                        menuPermission_Master.userid = (HttpContext.Session.GetString("userid"));

                        //cOM_MachinaryLC_Master.userid = HttpContext.Session.GetString("userid");

                        //foreach (MenuPermission_Details ss in menuPermission_Master.MenuPermission_Details)
                        //{
                        //    ss.DateAdded = DateTime.Now;
                        //    ss.DateUpdated = DateTime.Now;

                        //    //db.VoucherSubs.Add(ss);
                        //    //db.cOM_MachinaryLC_MasterExports.Add(ss);
                        //}

                        db.VersionMenuPermission_Masters.Add(menuPermission_Master);
                    }

                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                //return Json(new { Success = 1, MenuPermissionId = menuPermission_Master.MenuPermissionId, ex = "" });
                return Json(new { Success = 1 });


            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    errors = ex.Message
                    //ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPost]
        [RequestSizeLimit(73400320)]
        public ActionResult VersionReportMenu(Version_Report_Master reportPermission_Master)
        {
            try
            {
                //var a =   menuPermission_Master;
                if ((HttpContext.Session.GetString("comid")) == "0" || (HttpContext.Session.GetString("comid")) == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });
                }
                ViewBag.Title = "Create";

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });


                if (reportPermission_Master.VersionReportMasterId > 0)
                {
                    reportPermission_Master.DateUpdated = DateTime.Now;
                    //cOM_MachinaryLC_Master.DateAdded = DateTime.Now;

                    //menuPermission_Master.comid = HttpContext.Session.GetString("comid");
                    reportPermission_Master.UpdateByUser = HttpContext.Session.GetString("userid");


                    if (reportPermission_Master.UserId == null || reportPermission_Master.UserId == "0")
                    {
                        reportPermission_Master.UserId = HttpContext.Session.GetString("userid");
                    }
                    if (reportPermission_Master.SoftwareId == 0)
                    {
                        reportPermission_Master.SoftwareId = (int)HttpContext.Session.GetInt32("softwareId");
                    }

                    IQueryable<Version_Report_Details> reportPermission_Details = db.Version_Report_Details.Where(p => p.VersionReportMasterId == reportPermission_Master.VersionReportMasterId);

                    db.Version_Report_Details.RemoveRange(reportPermission_Details);
                    //foreach (MenuPermission_Details ss in menuPermission_Detailses)
                    //{
                    //    db.MenuPermissionDetails.Remove(ss);
                    //}

                    if (reportPermission_Master.Version_Report_Details != null)
                    {
                        foreach (Version_Report_Details ss in reportPermission_Master.Version_Report_Details)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;

                            db.Version_Report_Details.Add(ss);
                            //db.SaveChanges();
                        }
                        db.SaveChanges();

                    }
                    db.Entry(reportPermission_Master).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    reportPermission_Master.DateAdded = DateTime.Now;
                    reportPermission_Master.DateUpdated = DateTime.Now;
                    //menuPermission_Master.comid = HttpContext.Session.GetString("comid");
                    reportPermission_Master.UserId = (HttpContext.Session.GetString("userid"));



                    db.Version_Report_Masters.Add(reportPermission_Master);
                }

                db.SaveChanges();

                return Json(new { Success = 1 });


            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    errors = ex.Message

                });
            }
        }

        [HttpPost, ActionName("DeleteVersionMenu")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteVersionMenuConfirmed(int id)
        {
            try
            {

                IQueryable<VersionMenuPermission_Details> versionMenuDetails = db.VersionMenuPermission_Details.Where(p => p.MenuPermissionId == id);

                db.VersionMenuPermission_Details.RemoveRange(versionMenuDetails);

                //foreach (COM_MachinaryLC_Details ss in grouplcsub)
                //{
                //    db.COM_MachinaryLC_Details.Remove(ss);
                //}

                VersionMenuPermission_Master menuPermission_Master = db.VersionMenuPermission_Masters.Find(id);
                db.VersionMenuPermission_Masters.Remove(menuPermission_Master);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = menuPermission_Master.MenuPermissionId, ex = "" });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            //return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");
        }

        #endregion

        #region For Delete Menu Permission Module and Report

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id, int ReportId)
        {
            try
            {
                // for menu 
                IQueryable<MenuPermission_Details> grouplcsub = db.MenuPermissionDetails.Where(p => p.MenuPermissionId == id);
                db.MenuPermissionDetails.RemoveRange(grouplcsub);

                MenuPermission_Master menuPermission_Master = db.MenuPermissionMasters.Find(id);
                db.MenuPermissionMasters.Remove(menuPermission_Master);
                db.SaveChanges();

                // for report
                IQueryable<ReportPermission_Details> reports = db.ReportPermission_Details.Where(x => x.ReportPermission_MasterId == ReportId);
                db.ReportPermission_Details.RemoveRange(reports);

                ReportPermission_Master reportMaster = db.ReportPermission_Masters.Find(ReportId);
                db.ReportPermission_Masters.Remove(reportMaster);
                db.SaveChanges();

                return Json(new { Success = 1, TermsId = menuPermission_Master.MenuPermissionId, ex = "" });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            //return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");
        }

        #endregion


        public ActionResult Approval()
        {
            return View("Approval");
        }

        #region UserId Auto Selection Change

        [HttpPost]
        [Route("[controller]/GetMenuPermissionId/comId/userId")]
        public int GetMenuPermissionId(string comId, string userId)
        {
            int menuPermissionId = 0;
            if (comId is not null && userId is not null)
            {
                menuPermissionId = db.MenuPermissionMasters.Where(a => a.comid == comId && a.useridPermission == userId).Select(a => a.MenuPermissionId).SingleOrDefault();
            }
            return menuPermissionId;
        }


        [HttpPost]
        [Route("[controller]/GetReportPermissionId/comId/userId")]
        public int GetReportPermissionId(string comId, string userId)
        {
            int reportPermissionId = 0;
            if (comId is not null && userId is not null)
            {
                reportPermissionId = db.ReportPermission_Masters.Where(a => a.ComId == comId && a.useridPermission == userId).Select(a => a.ReportPermission_MasterId).SingleOrDefault();
            }
            return reportPermissionId;
        }

        #endregion

        #region View Models
        public class CompanyList
        {
            public string ComId { get; set; }
            public string CompanyShortName { get; set; }
        }
        public class MenuPermissionModel
        {

            public int ModuleMenuId { get; set; }


            public string ParentMenuName { get; set; }

            public string ModuleMenuName { get; set; }

            public string ModuleMenuCaption { get; set; }

            public string ModuleMenuController { get; set; }

            public int SLNO { get; set; }

            public bool IsDefault { get; set; }



        }

        public class ReportPermissionsVM
        {

            public int ReportPermissionsId { get; set; }

            public string ReportName { get; set; }

            public string ReportType { get; set; }
        }

        public class ExistReportPermissionsVM
        {
            public int VersionReportMasterId { get; set; }
            public int ReportId { get; set; }

            public string ReportName { get; set; }

            public string ReportType { get; set; }

        }

        public class SoftwareVersion
        {
            public int MenuPermissionId { get; set; }
            public int SoftwareId { get; set; }
            public int VersionId { get; set; }
            public string SoftwareName { get; set; }
            public string VersionName { get; set; }
            public string VersionReportMasterId { get; set; }
        }

        public class ExistReportMenuPermissionsVM
        {
            public int ReportPermission_MasterId { get; set; }
            public int ReportId { get; set; }

            public string ReportName { get; set; }

            public string ReportType { get; set; }

            public bool IsCreate { get; set; }
            public bool IsEdit { get; set; }
            public bool IsDelete { get; set; }
            public bool IsView { get; set; }
        }

        #endregion
    }
}