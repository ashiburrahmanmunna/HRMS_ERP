using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nancy.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.COM_ProformaInvoiceController;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;

namespace GTERP.Controllers
{
    public class SubMenuPermissionController : Controller
    {
        private readonly GTRDBContext db;
        private readonly IConfiguration _configuration;
        public SubMenuPermissionController(GTRDBContext context, IConfiguration configuration)
        {
            db = context;
            _configuration = configuration;
        }
        // GET: MenuPermission
        public ActionResult Index()
        {

            var comid = HttpContext.Session.GetString("comid");
            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            //Login
            //Uri url = new Uri(string.Format("https://localhost:44336/api/User/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("http://101.2.165.187:82/api/user/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            //Uri url = new Uri(string.Format("https://gtrbd.net/chitraapi/api/User/GetUsersCompanies"));
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);

            if (res.MyUsers != null || res.Companies != null)
            {
                SqlParameter[] sqlParameter1 = new SqlParameter[2];
                sqlParameter1[0] = new SqlParameter("@Criteria", "MenuPermission");
                sqlParameter1[1] = new SqlParameter("@comId", comid);

                List<SubMenuPermission> menuPermission = Helper.ExecProcMapTList<SubMenuPermission>("prcgetSubMenuPermission", sqlParameter1).ToList();

                var resultUsers = new List<SubMenuPermission>();
                for (int i = 0; i < res.MyUsers.Count; i++)
                {
                    var seqUserId = res.MyUsers.ElementAt(i).UserID;
                    var UserName = res.MyUsers.ElementAt(i).UserName;
                    var hs = menuPermission.Where(x => x.UserIdPermission == seqUserId).ToList();

                    if (hs != null)
                    {
                        hs.ForEach(m => m.CompanyName = db.Companys?.Where(a => a.CompanySecretCode == m.ComId)?.FirstOrDefault()?.CompanyName);
                    }

                    foreach (var menu in hs.Where(x => x.ComId == comid))
                    {
                        var resultUser = new SubMenuPermission();

                        resultUser.UserId = menu.UserId;
                        resultUser.UserIdPermission = menu.UserIdPermission;
                        resultUser.UserName = UserName;
                        resultUser.CompanyName = menu.CompanyName;
                        resultUser.ComId = menu.ComId;
                        resultUser.Email = UserName;
                        resultUser.SubMenuPermissionId = menu.SubMenuPermissionId;
                        resultUser.SubMenuPermissionReportMasterId = menu.SubMenuPermissionReportMasterId;

                        resultUsers.Add(resultUser);
                    }
                }

                ViewBag.SubMenuPermission = resultUsers.DistinctBy(x => x.UserName);
            }
            return View();
        }

        public ActionResult UserTransfer(string menuPermissionId, string userIdPermission, string comId)
        {
            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            //Login
            //Uri url = new Uri(string.Format("https://localhost:44336/api/User/GetUsersCompanies"));
            // Uri url = new Uri(string.Format("http://101.2.165.187:82/api/user/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            Uri url = new Uri(string.Format("https://gtrbd.net/chitraapi/api/User/GetUsersCompanies"));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);

            String userid = HttpContext.Session.GetString("userid");
            SqlParameter[] sqlParameter = new SqlParameter[4];
            sqlParameter[0] = new SqlParameter("@menuPermissionId", menuPermissionId);
            sqlParameter[1] = new SqlParameter("@userIdPermission", userIdPermission);
            sqlParameter[2] = new SqlParameter("@comId", comId);
            sqlParameter[3] = new SqlParameter("@AddedByUserId", userid);

            Helper.ExecProc("prcPermissionTransfer", sqlParameter);

            MenuPermission_Master menuPermission_Master = db.MenuPermissionMasters.Include(m => m.MenuPermission_Details).ThenInclude(abc => abc.ModuleMenus).Where(a => a.useridPermission == userIdPermission && a.comid == comId).FirstOrDefault();

            ViewBag.Title = "Edit";

            SqlParameter[] sqlParameter1 = new SqlParameter[2];
            sqlParameter1[0] = new SqlParameter("@Criteria", "CompanyPermission");
            sqlParameter1[1] = new SqlParameter("@userid", HttpContext.Session.GetString("userid"));

            List<CompanyList> CompanyList = Helper.ExecProcMapTList<CompanyList>("prcgetPermitCompanyList", sqlParameter1).ToList();

            ViewBag.comid = new SelectList(CompanyList, "ComId", "CompanyShortName", comId);

            SqlParameter[] sqlParameter2 = new SqlParameter[1];
            sqlParameter2[0] = new SqlParameter("@Criteria", "CompanyPermission");

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

            ViewBag.useridPermission = new SelectList(l, "UserId", "UserName", userIdPermission);
            ViewBag.newUserPermission = new SelectList(l, "UserId", "UserName");

            SqlParameter[] sqlParameter3 = new SqlParameter[2];
            sqlParameter3[0] = new SqlParameter("@comid", comId);
            sqlParameter3[1] = new SqlParameter("@userid", userid);
            List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("SubMenuPermissionInformation", sqlParameter3).ToList();
            ViewBag.MenuList = menuPermissionModels;

            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        public IActionResult Create(string comid, string UserId, int? SubMenuPermissionId, int? SubMenuPermissionReportMasterId, int? isDelete)
        {
            ViewBag.SubMenuPermissionReportId = SubMenuPermissionReportMasterId;
            comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            var useridsession = HttpContext.Session.GetString("userid");
            var softwareId = Convert.ToInt32(HttpContext.Session.GetString("SoftwareId"));
            var versionId = Convert.ToInt32(HttpContext.Session.GetString("VersionId"));

            var softwareDefaultModuleId = db.SubMenuPermissionMasters.Where(a => a.SoftwareId == softwareId && a.VersionId == versionId).Select(a => a.DefaultModuleId).FirstOrDefault();
            string useridpermission = useridsession;
            if (UserId != null)
            {
                useridpermission = UserId.ToString();

            }

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            Uri urlSoft = new Uri(string.Format(_configuration.GetValue<string>("API:GetSoftwareVersion")));


            string responseSoft = webHelper.Get(urlSoft);
            string response = webHelper.GetUserCompany(url, req);


            GetResponse res = new GetResponse(response);

            // for user list

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
            ViewBag.useridPermission = new SelectList(l, "UserId", "UserName", userid);

            // for company list

            var CompanySelectList = db.Companys.ToList();
            List<CompanyList> CompanyList = new List<CompanyList>();
            foreach (var company in CompanySelectList)
            {
                var com = new CompanyList();
                com.ComId = new Guid(company.CompanyCode);
                com.CompanyShortName = company.CompanyName;

                CompanyList.Add(com);
            }

            ViewBag.comid = new SelectList(CompanyList, "ComId", "CompanyShortName", comid);

            //for software list

            GetSoftwareResponse resSoft = new GetSoftwareResponse(responseSoft);
            var softwareList = resSoft.Softwares.ToList();
            ViewBag.softwares = new SelectList(softwareList, "Id", "Name", softwareId);

            // for version list

            var versionList = resSoft.SoftwareVersions.ToList();
            ViewBag.VersionList = new SelectList(versionList, "Id", "VersionName", versionId);

            if (SubMenuPermissionReportMasterId > 0)
            {

                if (resSoft.Softwares != null || resSoft.SoftwareVersions != null)
                {
                    if (isDelete == 1)
                    {

                        ViewBag.Title = "Delete";

                    }
                    else
                    {
                        ViewBag.Title = "Edit";

                    }


                    if (SubMenuPermissionReportMasterId == null)
                    {
                        return BadRequest();
                    }


                    SqlParameter[] sqlparameters = new SqlParameter[2];
                    sqlparameters[0] = new SqlParameter("@SubMenuPermissionReportId", SubMenuPermissionReportMasterId);
                    sqlparameters[1] = new SqlParameter("@ComId", comid);

                    List<ExistReportPermissionsVM> reportPermission_Master = Helper.ExecProcMapTList<ExistReportPermissionsVM>("prcExistSubMenuReportPermission", sqlparameters);

                    ViewBag.ReportPermission = reportPermission_Master;
                    if (reportPermission_Master == null)
                    {
                        return NotFound();
                    }

                    // for report permission

                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@SoftwareId", softwareId);
                    parameters[1] = new SqlParameter("@VersionId", versionId);
                    parameters[2] = new SqlParameter("@ComId", comid);

                    List<SubMenuReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<SubMenuReportPermissionsVM>("prcSubMenuReportPermission", parameters);
                    ViewBag.ReportList = ListOfReportPermission;


                }

            }

            else
            {

                ViewBag.Title = "Create";

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@SoftwareId", softwareId);
                parameters[1] = new SqlParameter("@VersionId", versionId);
                parameters[2] = new SqlParameter("@ComId", comid);
                List<SubMenuReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<SubMenuReportPermissionsVM>("prcSubMenuReportPermission", parameters);
                ViewBag.ReportList = ListOfReportPermission;

            }

            // for sub menu permission master

            if (SubMenuPermissionId > 0)
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


                    if (SubMenuPermissionId == null)
                    {
                        return BadRequest();
                    }

                    SubMenuPermissionMasters SubMenuPermissionMasters = db.SubMenuPermissionMasters.Include(m => m.SubMenuPermissionDetails).ThenInclude(abc => abc.ModuleMenus).Where(m => m.SubMenuPermissionId.ToString() == SubMenuPermissionId.ToString()).FirstOrDefault();

                    var userIdMenuPermitted = db.SubMenuPermissionMasters.Where(a => a.SubMenuPermissionId == SubMenuPermissionId).Select(a => a.UserIdPermission).FirstOrDefault();

                    if (SubMenuPermissionMasters == null)
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

                    SqlParameter[] sqlParameter2 = new SqlParameter[4];
                    sqlParameter2[0] = new SqlParameter("@SoftwareId", softwareId);
                    sqlParameter2[1] = new SqlParameter("@VersionId", versionId);
                    sqlParameter2[2] = new SqlParameter("@ComId", comid);
                    sqlParameter2[3] = new SqlParameter("@UserId", userIdMenuPermitted);
                    List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("SubMenuPermissionInformation", sqlParameter2).ToList();
                    var query = $"Exec SubMenuPermissionInformation {softwareId},{versionId},'{comid}','{userIdMenuPermitted}'";

                    ViewBag.MenuList = menuPermissionModels;

                    return View("Create", SubMenuPermissionMasters);

                }

            }
            else
            {

                ViewBag.Title = "Create";

                SqlParameter[] sqlParameter2 = new SqlParameter[4];
                sqlParameter2[0] = new SqlParameter("@SoftwareId", softwareId);
                sqlParameter2[1] = new SqlParameter("@VersionId", versionId);
                sqlParameter2[2] = new SqlParameter("@ComId", "");
                sqlParameter2[3] = new SqlParameter("@UserId", "");
                List<MenuPermissionModel> menuPermissionModels = Helper.ExecProcMapTList<MenuPermissionModel>("SubMenuPermissionInformation", sqlParameter2).ToList();

                ViewBag.MenuList = menuPermissionModels;
            }


            return View();
        }

        [HttpPost]
        [RequestSizeLimit(73400320)]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[ValidateAntiForgeryToken]
        //[RequestSizeLimit(1074790400)]
        public ActionResult Create(SubMenuPermissionMasters subMenuPermissionMaster)
        {
            try
            {

                if ((HttpContext.Session.GetString("comid")) == "0" || (HttpContext.Session.GetString("comid")) == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });
                }
                ViewBag.Title = "Create";


                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    if (subMenuPermissionMaster.SubMenuPermissionId > 0)
                    {
                        subMenuPermissionMaster.DateUpdated = DateTime.Now;

                        subMenuPermissionMaster.ComId = HttpContext.Session.GetString("comid");
                        subMenuPermissionMaster.UserIdUpdate = HttpContext.Session.GetString("userid");


                        if (subMenuPermissionMaster.UserId == null || subMenuPermissionMaster.UserId == "0")
                        {
                            subMenuPermissionMaster.UserId = HttpContext.Session.GetString("userid");

                        }
                        if (subMenuPermissionMaster.ComId == "")
                        {
                            subMenuPermissionMaster.ComId = HttpContext.Session.GetString("comid");
                        }

                        IQueryable<SubMenuPermissionDetails> subMenuPermissionDetails = db.SubMenuPermissionDetails.Where(p => p.SubMenuPermissionId == subMenuPermissionMaster.SubMenuPermissionId);

                        db.SubMenuPermissionDetails.RemoveRange(subMenuPermissionDetails);

                        if (subMenuPermissionMaster.SubMenuPermissionDetails != null)
                        {
                            foreach (SubMenuPermissionDetails ss in subMenuPermissionMaster.SubMenuPermissionDetails)
                            {
                                ss.DateAdded = DateTime.Now;
                                ss.DateUpdated = DateTime.Now;
                                ss.ComId = HttpContext.Session.GetString("comid");
                                db.SubMenuPermissionDetails.Add(ss);

                            }
                            db.SaveChanges();

                        }
                        db.Entry(subMenuPermissionMaster).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {

                        subMenuPermissionMaster.DateAdded = DateTime.Now;
                        subMenuPermissionMaster.DateUpdated = DateTime.Now;
                        subMenuPermissionMaster.ComId = HttpContext.Session.GetString("comid");
                        subMenuPermissionMaster.UserId = (HttpContext.Session.GetString("userid"));
                        var modifed=new List<SubMenuPermissionDetails>();
                        foreach (SubMenuPermissionDetails ss in subMenuPermissionMaster.SubMenuPermissionDetails)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;
                            ss.ComId = HttpContext.Session.GetString("comid");
                            modifed.Add(ss);

                        }
                        subMenuPermissionMaster.SubMenuPermissionDetails = modifed;
                        db.SubMenuPermissionMasters.Add(subMenuPermissionMaster);
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

        [HttpPost]
        [RequestSizeLimit(73400320)]
        public ActionResult CreateReport(SubMenuPermissionReportMaster subMenuPermissionReportMaster)
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


                if (subMenuPermissionReportMaster.SubMenuPermissionReportMasterId > 0)
                {
                    subMenuPermissionReportMaster.DateUpdated = DateTime.Now;

                    subMenuPermissionReportMaster.UpdateByUser = HttpContext.Session.GetString("userid");
                    subMenuPermissionReportMaster.ComId = HttpContext.Session.GetString("comid");

                    if (subMenuPermissionReportMaster.UserId == null || subMenuPermissionReportMaster.UserId == "0")
                    {
                        subMenuPermissionReportMaster.UserId = HttpContext.Session.GetString("userid");
                    }
                    if (subMenuPermissionReportMaster.SoftwareId == 0)
                    {
                        subMenuPermissionReportMaster.SoftwareId = (int)HttpContext.Session.GetInt32("softwareId");
                    }

                    IQueryable<SubMenuPermissionReportDetails> subMenuPermissionReportDetails = db.SubMenuPermissionReportDetails.Where(p => p.SubMenuPermissionReportMasterId == subMenuPermissionReportMaster.SubMenuPermissionReportMasterId);

                    db.SubMenuPermissionReportDetails.RemoveRange(subMenuPermissionReportDetails);

                    if (subMenuPermissionReportMaster.SubMenuPermissionReportDetails != null)
                    {
                        foreach (SubMenuPermissionReportDetails ss in subMenuPermissionReportMaster.SubMenuPermissionReportDetails)
                        {
                            ss.DateAdded = DateTime.Now;
                            ss.DateUpdated = DateTime.Now;
                            ss.ComId = HttpContext.Session.GetString("comid");
                            db.SubMenuPermissionReportDetails.Add(ss);
                        }
                        db.SaveChanges();

                    }
                    db.Entry(subMenuPermissionReportMaster).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    subMenuPermissionReportMaster.DateAdded = DateTime.Now;
                    subMenuPermissionReportMaster.DateUpdated = DateTime.Now;
                    subMenuPermissionReportMaster.UpdateByUser = HttpContext.Session.GetString("userid");
                    subMenuPermissionReportMaster.ComId = HttpContext.Session.GetString("comid");
                    subMenuPermissionReportMaster.UserId = (HttpContext.Session.GetString("userid"));
                    var modifiedList = new List<SubMenuPermissionReportDetails>();
                    foreach (SubMenuPermissionReportDetails ss in subMenuPermissionReportMaster.SubMenuPermissionReportDetails)
                    {
                        ss.DateAdded = DateTime.Now;
                        ss.DateUpdated = DateTime.Now;
                        ss.ComId = HttpContext.Session.GetString("comid");
                        modifiedList.Add(ss);
                    }
                    subMenuPermissionReportMaster.SubMenuPermissionReportDetails= modifiedList;
                    db.SubMenuPermissionReportMasters.Add(subMenuPermissionReportMaster);

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

        #region For Delete 
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteSubMenu(int id, int ReportId)
        {
            try
            {
                // for sub menu permission details
                IQueryable<SubMenuPermissionDetails> subMenuPermissionDetails = db.SubMenuPermissionDetails.Where(p => p.SubMenuPermissionId == id);
                db.SubMenuPermissionDetails.RemoveRange(subMenuPermissionDetails);

                // for sub menu permission report details

                IQueryable<SubMenuPermissionReportDetails> subMenuPermissionReportDetails = db.SubMenuPermissionReportDetails.Where(p => p.SubMenuPermissionReportMasterId == ReportId);
                db.SubMenuPermissionReportDetails.RemoveRange(subMenuPermissionReportDetails);

                // for sub menu permission details report

                SubMenuPermissionMasters subMenuPermissionMasters = db.SubMenuPermissionMasters.Find(id);
                db.SubMenuPermissionMasters.Remove(subMenuPermissionMasters);

                // for sub menu permission master report
                SubMenuPermissionReportMaster subMenuPermissionReportMaster = db.SubMenuPermissionReportMasters.Find(ReportId);
                db.SubMenuPermissionReportMasters.Remove(subMenuPermissionReportMaster);
                db.SaveChanges();

                return Json(new { Success = 1, TermsId = subMenuPermissionMasters.SubMenuPermissionId, TermsReportId = subMenuPermissionReportMaster.SubMenuPermissionReportMasterId, ex = "" });
            }

            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        #endregion

        public ActionResult Approval()
        {

            return View("Approval");
        }


        // view model

        #region View Models
        public class SoftwareVersion
        {
            public int SubMenuPermissionId { get; set; }
            public int SoftwareId { get; set; }
            public int VersionId { get; set; }
            public string SoftwareName { get; set; }
            public string VersionName { get; set; }
            public string VersionReportMasterId { get; set; }
        }

        public class CompanyList
        {
            public Guid ComId { get; set; }
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

        public class SubMenuPermission
        {
            public int SubMenuPermissionId { get; set; }
            public int SubMenuPermissionReportMasterId { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string ComId { get; set; }
            public string CompanyName { get; set; }
            public string UserIdPermission { get; set; }

        }

        public class SubMenuReportPermissionsVM
        {

            public int ReportPermissionsId { get; set; }

            public string ReportName { get; set; }

            public string ReportType { get; set; }
            public int isChecked { get; set; }
        }

        public class ExistReportPermissionsVM
        {
            public int SubMenuPermissionReportMasterId { get; set; }
            public int ReportId { get; set; }

            public string ReportName { get; set; }

            public string ReportType { get; set; }

            public bool IsCreate { get; set; }
            public bool IsEdit { get; set; }
            public bool IsDelete { get; set; }
            public bool IsView { get; set; }
        }

        public class MenuPermission
        {
            public int MenuPermissionId { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string comid { get; set; }
            public string CompanyName { get; set; }
            public string useridPermission { get; set; }

        }
        #endregion
    }
}
