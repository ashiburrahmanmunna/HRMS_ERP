using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using GTERP.Services;
using static GTERP.ViewModels.CommercialVM;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
//using System.Web.Mvc;

namespace GTERP.Controllers.Tutorial_Certificate
{
    public class TutorialCertificateController : Controller
    {

        //public string userId = "";
        private readonly GTRDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public TutorialCertificateController(GTRDBContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAllModuleName(int moduleId, int moduleMenuChildId)
        {
            string comid = HttpContext.Session.GetString("comid");
            var userId = HttpContext.Session.GetString("userid");
            var userEmail = HttpContext.Session.GetString("username");
            ViewBag.UserId = userEmail;

            if (userId != null)
            {
                var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
                //---------------Enable to get Certificate----------------
                bool isCompleteAll = false;
                if (moduleId != 0)
                {


                    try
                    {
                        var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                        string sql = $"exec prcGetCertificateData '{comid}','4','{userId}',{moduleId}";
                        using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                        {
                            SqlCommand command = new SqlCommand(sql, connection);
                            connection.Open();
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {

                                    var completedCourses = Convert.ToInt32(dataReader["CompletedCourse"].ToString());
                                    var TotalCourses = Convert.ToInt32(dataReader["TotalCourses"].ToString());

                                    if (completedCourses >= TotalCourses && TotalCourses != 0)
                                    {
                                        isCompleteAll = true;
                                        ViewBag.isCompleteAll = true;

                                    }
                                    else
                                    {
                                        isCompleteAll = false;
                                        ViewBag.isCompleteAll = false;
                                    }
                                }
                            }
                        }
                        var isPassed = _context.CertificateInfos.Where(x => x.ModuleId == moduleId && x.UserId == userId).FirstOrDefault();

                        var data = "";
                        bool ispa = false;
                        if(isPassed != null)
                        {
                            ispa = true;
                            data = isPassed.CertificateId.ToString();
                        }
                       // ViewBag.CertificateId = data;
                        return Json(new { Course = isCompleteAll ,IsPassed = ispa,Certificateid = data});
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    };



                }
                //-------------------------End---------------------------


                if (moduleMenuChildId != 0)
                {
                    List<FileCollection> fileCollection = new List<FileCollection>();
                    var moduleCourseId = _context.ModuleCourses.Where(a => a.ModuleMenuChildId == moduleMenuChildId).Select(a => a.ModuleCourseId).SingleOrDefault();
                    var fileCollectionList = _context.ModuleCourses.Where(a => a.ModuleCourseId == moduleCourseId).Select(b => b.FilePath).ToList();

                    char[] seperator = { '!' };

                    if (fileCollectionList.Count == 1 && fileCollectionList[0] == null)
                    {
                        fileCollectionList[0] = "";
                        ViewBag.fileCollectionList = fileCollectionList;
                        return Json(fileCollectionList);
                    }

                    else
                    {
                        if (fileCollectionList.Count > 0 && fileCollectionList[0] != null)
                        {
                            String[] strlist = fileCollectionList[0].Split(seperator);
                            strlist = strlist.SkipLast(1).ToArray();
                            List<string> ls = new();
                            foreach (var nam in strlist)
                            {
                                string path = Path.Combine(this._webHostEnvironment.WebRootPath, "FilesForCertificates/") + nam;
                                FileInfo file = new FileInfo(path);
                                if (file.Exists)//check file exsit or not  
                                {
                                    ls.Add(nam);
                                }
                            }

                            fileCollectionList = ls;
                        }
                    }


                    ViewBag.fileCollectionList = fileCollectionList;
                    return Json(fileCollectionList);

                }
                //var userId = HttpContext.Session.GetString("userid");
                Console.WriteLine(HttpContext.Session);
                List<Module> m = new List<Module>();
                List<FileCollection> fileCollections = new List<FileCollection>();
                #region Execute procedure to get ModuleName & ModuleMenuCaption(Now-Commended)
                //----------------Exec Store Procedure-----------------------

                List<ModuleViewModel> viewModels = new List<ModuleViewModel>();///List<string>ls  =new List<string>()

                try
                {
                    var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                    string sql = $"exec prcGetCertificateData '{comid}','1','{userId}',{moduleId}";
                    using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                    {
                        SqlCommand command = new SqlCommand(sql, connection);
                        connection.Open();
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ModuleViewModel vm = new ModuleViewModel();
                                //vm.ModuleId = Int32.Parse(dataReader["ModuleId"].ToString());
                                vm.ModuleId = Convert.ToInt32(dataReader["ModuleId"].ToString());
                                vm.ModuleName = dataReader["ModuleName"].ToString();
                                viewModels.Add(vm);
                                //viewModels.Add(vm);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                };
                var moduleList = viewModels;
                ViewBag.ModulList = moduleList;

                //----------------------(Procedure for ModuleParentMenuId)----------------------
                List<ModuleMenuViewModel> menuViewModels = new List<ModuleMenuViewModel>();
                try
                {
                    var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                    string sql = $"exec prcGetCertificateData '{comid}','2','{userId}',{moduleId}";
                    using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                    {
                        SqlCommand command = new SqlCommand(sql, connection);
                        connection.Open();
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ModuleMenuViewModel mvm = new ModuleMenuViewModel();

                                mvm.ModuleMenuId = Convert.ToInt32(dataReader["ModuleMenuId"].ToString());
                                mvm.ModuleId = Convert.ToInt32(dataReader["ModuleId"].ToString());
                                mvm.ParentId = dataReader["ParentId"].ToString();
                                mvm.ModuleMenuCaption = dataReader["ModuleMenuCaption"].ToString();

                                menuViewModels.Add(mvm);
                                //viewModels.Add(vm);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                };
                var moduleMenuList = menuViewModels;
                ViewBag.ModuleMenuList = moduleMenuList;

                //----------------------(Procedure for ModuleChildMenuId)----------------------
                List<ModuleChildMenuViewModel> childMenuViewModels = new List<ModuleChildMenuViewModel>();
                try
                {
                    var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                    string sql = $"exec prcGetCertificateData '{comid}','3','{userId}',{moduleId}";
                    using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                    {
                        SqlCommand command = new SqlCommand(sql, connection);
                        connection.Open();
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ModuleChildMenuViewModel cmvm = new ModuleChildMenuViewModel();
                                //vm.ModuleId = Int32.Parse(dataReader["ModuleId"].ToString());
                                cmvm.ModuleMenuId = Convert.ToInt32(dataReader["ModuleMenuId"].ToString());
                                cmvm.ModuleMenuCaption = dataReader["ModuleMenuCaption"].ToString();
                                cmvm.ParentId = dataReader["ParentId"].ToString();
                                cmvm.IsComplete = dataReader["IsComplete"].ToString();
                                childMenuViewModels.Add(cmvm);
                                //viewModels.Add(vm);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                };
                var moduleChildMenuList = childMenuViewModels;
                ViewBag.ModuleChildMenuList = moduleChildMenuList;
                

                #endregion

            }
            else
            {
                ViewBag.msg = "Sorry, you have no permission. Please register.";
            }


            return View();

        }

        public class ModuleViewModel
        {
            public int ModuleId { get; set; }
            public string ModuleName { get; set; }
        }

        public class ModuleMenuViewModel
        {
            public int ModuleMenuId { get; set; }
            public int ModuleId { get; set; }
            public string ModuleMenuCaption { get; set; }
            public string ParentId { get; set; }
        }
        public class ModuleChildMenuViewModel
        {
            public int ModuleMenuId { get; set; }
            //public int ModuleId { get; set; }
            public string ModuleMenuCaption { get; set; }
            public string ParentId { get; set; }
            public string IsComplete { get; set; }
        }
        public class CourseCompleteViewModel
        {
            public string ModuleMenuChildName { get; set; }
            public string IsComplete { get; set; }
        }

        [HttpGet]
        public IActionResult ModuleRecordView()//For insert data into ModuleCourses table
        {
            var data = _context.ModuleCourses.ToList();
            char[] spearator = { '!' };

            List<ModuleCoursesViewModel> person = new List<ModuleCoursesViewModel>();
            foreach (var a in data)
            {
                ModuleCoursesViewModel index = new ModuleCoursesViewModel();
                if (a.FilePath == null)
                {
                    a.FilePath = "";
                }
                String[] strlist = a.FilePath.Split(spearator);
                strlist = strlist.SkipLast(1).ToArray();
                List<string> ls = new();

                foreach (var nam in strlist)
                {
                    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "FilesForCertificates/") + nam;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exsit or not  
                    {
                        ls.Add(nam);
                    }

                }
                index.ModuleCourseId = a.ModuleCourseId;
                index.ModuleName = a.ModuleName;
                index.ModuleMenuParentName = a.ModuleMenuParentName;
                index.ModuleMenuChildName = a.ModuleMenuChildName;
                index.VideoLink = a.VideoLink;
                index.Description = a.Description;
                index.FilePath = ls;
                person.Add(index);
            }

            ViewBag.filename = person;
            return View(person);
        }
        //public ActionResult Print(int? id, string type = "pdf")
        //{
        //    var callBackUrl = _context.ModuleCourses(id, type);
        //    return Redirect(callBackUrl);

        //}
        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(this._webHostEnvironment.WebRootPath, "FilesForCertificates/") + filename;
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public IActionResult btnDelete_Click(string fileName)
        {
            string path = Path.Combine(this._webHostEnvironment.WebRootPath, "FilesForCertificates/") + fileName;
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
            return RedirectToAction("ModuleRecordView");
        }
        public IActionResult Create()
        {
            List<Module> moduleList2 = new List<Module>();
            moduleList2 = (from module in _context.Modules select module).ToList();
            moduleList2.Insert(0, new Module { ModuleId = 0, ModuleCaption = "--Select a product--" });
            ViewBag.ListOfModule = moduleList2;

            var moduleList = (from module in _context.Modules
                              select new SelectListItem()
                              {
                                  Text = module.ModuleName,
                                  Value = module.ModuleId.ToString(),
                              }).ToList();

            moduleList.Insert(0, new SelectListItem()
            {
                Text = "----At first select any product----",
                Value = string.Empty
            });
            var moduleMenuParentList = _context.ModuleMenus.ToList();


            //var moduleMenuChildList = (from module in _context.ModuleMenus.Where(a => a.isParent == 0 && a.isInactive == 0)
            var moduleMenuChildList = (from module in _context.ModuleMenus.Where(a => a.isParent == 0)
                                       select new SelectListItem()
                                       {
                                           Text = module.ModuleMenuCaption,
                                           Value = module.ModuleMenuId.ToString(),
                                       }).ToList();

            moduleMenuChildList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            ViewBag.ListofModulesParentMenu = moduleMenuParentList;


            ViewBag.ListofModulesChildMenu = moduleMenuChildList;
            ViewBag.ListofModules = moduleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ModuleCoursesViewModel model)//For insert data into ModuleCourses table
        {

            //var userId = HttpContext.Session.GetString("userid");
            //Console.WriteLine(HttpContext.Session);
            if (ModelState.IsValid && !_context.ModuleCourses.Any(a => a.ModuleMenuChildId == model.ModuleMenuChildId))
            {
                var moduleName = _context.Modules.Where(a => a.ModuleId == model.ModuleId).Select(b => b.ModuleName).FirstOrDefault();
                var moduleMenuPName = _context.ModuleMenus.Where(a => a.ModuleMenuId == model.ModuleMenuId && a.isParent == 1).Select(b => b.ModuleMenuCaption).FirstOrDefault();
                var moduleMenuCName = _context.ModuleMenus.Where(a => a.ModuleMenuId == model.ModuleMenuChildId).Select(b => b.ModuleMenuCaption).FirstOrDefault();
                ModuleCourses moduleCourses = new ModuleCourses
                {

                    ModuleId = model.ModuleId,
                    ModuleMenuId = model.ModuleMenuId,
                    ModuleMenuChildId = model.ModuleMenuChildId,
                    Description = model.Description,
                    VideoLink = model.VideoLink,
                    ModuleName = moduleName,
                    ModuleMenuParentName = moduleMenuPName,
                    ModuleMenuChildName = moduleMenuCName,
                };
                moduleCourses.FilePath = "";
                if(model.File != null)
                {
                    foreach (var item in model.File)
                    {
                        string FileNameUrl = UploadFile(item, moduleMenuCName);
                        moduleCourses.FilePath += FileNameUrl + "!";
                    };
                }
                

                _context.Add(moduleCourses);

                List<FileCollection> fileCollections = new List<FileCollection>();

                string uniqueFileName = null;

                await _context.SaveChangesAsync();


                return RedirectToAction("ModuleRecordView");


            }
            else
            {


                //_context.Entry(model).State = EntityState.Modified;
                //await _context.SaveChangesAsync();
                ModelState.AddModelError(string.Empty, "This tutorial already created  !!!");

            }


            List<Module> moduleList2 = new List<Module>();
            moduleList2 = (from module in _context.Modules select module).ToList();
            moduleList2.Insert(0, new Module { ModuleId = 0, ModuleCaption = "--Select a product--" });
            ViewBag.ListOfModule = moduleList2;

            var moduleList = (from module in _context.Modules
                              select new SelectListItem()
                              {
                                  Text = module.ModuleName,
                                  Value = module.ModuleId.ToString(),
                              }).ToList();

            moduleList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            var moduleMenuParentList = _context.ModuleMenus.ToList();


            //var moduleMenuChildList = (from module in _context.ModuleMenus.Where(a => a.isParent == 0)
            var moduleMenuChildList = (from module in _context.ModuleMenus.Where(a => a.isInactive == 0)
                                       select new SelectListItem()
                                       {
                                           Text = module.ModuleMenuCaption,
                                           Value = module.ModuleMenuId.ToString(),
                                       }).ToList();

            moduleMenuChildList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            ViewBag.EditId = 0;
            ViewBag.ListofModules = moduleList;
            ViewBag.ListofModulesChildMenu = moduleMenuChildList;
            ViewBag.ListofModulesParentMenu = moduleMenuParentList;
            return View(model);
        }
        private string UploadFile(IFormFile file, string title)
        {
            string FileName = null;
            if (file != null)
            {
                //string folder = "book/Gallery/";
                string serverFolder = _webHostEnvironment.WebRootPath + "\\FilesForCertificates\\";
                FileName = title.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(serverFolder, FileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

            }
            return FileName;
        }
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var module = await _context.ModuleCourses.FindAsync(id);
                if (module != null)
                {
                    _context.ModuleCourses.Remove(module);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("ModuleRecordView");
            }
            catch
            {
                return RedirectToAction("ModuleRecordView");
            }


        }

        [HttpGet]
        public async Task<IActionResult> Edit2(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var moduleCoursesById = await _context.ModuleCourses.FindAsync(id);
            List<Module> moduleList2 = new List<Module>();
            moduleList2 = (from module in _context.Modules select module).ToList();
            moduleList2.Insert(0, new Module { ModuleId = 0, ModuleCaption = "--Select a product--" });
            ViewBag.ListOfModule = moduleList2;
            ViewBag.moduleCoursesById = moduleCoursesById;

            ViewBag.EditId = id;



            return View(moduleCoursesById);
        }
        [HttpPost]
        public async Task<IActionResult> Edit2(ModuleCourses model)
        {
            if (ModelState.IsValid && _context.ModuleCourses.Any(a => a.ModuleMenuChildId == model.ModuleMenuChildId))
            {
                var moduleName = _context.Modules.Where(a => a.ModuleId == model.ModuleId).Select(b => b.ModuleName).FirstOrDefault();
                var moduleMenuPName = _context.ModuleMenus.Where(a => a.ModuleMenuId == model.ModuleMenuId && a.isParent == 1).Select(b => b.ModuleMenuCaption).FirstOrDefault();
                var moduleMenuCName = _context.ModuleMenus.Where(a => a.ModuleMenuId == model.ModuleMenuChildId).Select(b => b.ModuleMenuCaption).FirstOrDefault();

                var moduleCoursesById = await _context.ModuleCourses.FindAsync(model.ModuleCourseId);
                moduleCoursesById.ModuleId = model.ModuleId;
                moduleCoursesById.ModuleMenuId = model.ModuleMenuId;
                moduleCoursesById.ModuleMenuChildId = model.ModuleMenuChildId;
                moduleCoursesById.Description = model.Description;
                moduleCoursesById.VideoLink = model.VideoLink;
                moduleCoursesById.ModuleName = moduleName;
                moduleCoursesById.ModuleMenuParentName = moduleMenuPName;
                moduleCoursesById.ModuleMenuChildName = moduleMenuCName;
                if (model.File != null)
                {
                    foreach (var item in model.File)
                    {
                        string FileNameUrl = UploadFile(item, moduleMenuCName);
                        moduleCoursesById.FilePath += FileNameUrl + "!";
                    };
                }


                _context.Update(moduleCoursesById);
                await _context.SaveChangesAsync();
                //_context.Entry(moduleCourses).State = EntityState.Modified;
                //await _context.SaveChangesAsync();



                return RedirectToAction("ModuleRecordView");


            }


            return View();
        }
        [HttpPost]
        public IActionResult InsertLogInTrack(int moduleCourseId, bool checkValue)
        {
            var userId = HttpContext?.Session?.GetString("userid") ?? "";
            //var checkedValue = checkValue;
            if (ModelState.IsValid && !_context.UserLoggingTracks.Any(a => a.ModuleCourseId == moduleCourseId && a.UserId == userId))
            {
                UserLoggingTrack a = new UserLoggingTrack();
                a.ModuleCourseId = moduleCourseId;
                a.UserId = userId;
                a.IsComplete = checkValue;
                _context.UserLoggingTracks.Add(a);
                _context.SaveChanges();
            }
            if (_context.UserLoggingTracks.Any(a => a.ModuleCourseId == moduleCourseId && a.UserId == userId))
            {
                //var x = _context.UserLoggingTracks.FirstOrDefault(item => item.ModuleCourseId == moduleCourseId);
                var x = _context.UserLoggingTracks.Where(y => y.ModuleCourseId == moduleCourseId && y.UserId == userId).FirstOrDefault();
                if (x != null && checkValue == false)
                {
                    x.IsComplete = checkValue;
                    _context.UserLoggingTracks.Update(x);
                    _context.SaveChanges();
                }
                if (x != null && checkValue == true)
                {
                    x.IsComplete = checkValue;
                    _context.UserLoggingTracks.Update(x);
                    _context.SaveChanges();
                }

            }
            //if (checkValue == false && _context.UserLoggingTracks.Any(a => a.ModuleCourseId == moduleCourseId))
            //{
            //    var x = _context.UserLoggingTracks.FirstOrDefault(item => item.ModuleCourseId == moduleCourseId);
            //    if (x != null)
            //    {
            //        x.IsComplete = checkValue;
            //        _context.UserLoggingTracks.Update(x);
            //        _context.SaveChanges();
            //    }
            //};
            //else
            //{

            //    ModelState.AddModelError(string.Empty, "This tutorial already watched !!!");

            //}

            return Json(null);
        }
        public IActionResult GetModuleMenu(string id)
        {

            return View();
        }
        [HttpPost]
        public IActionResult GetAllMenu()
        {
            var moduleMenu = _context.ModuleMenus.Select(a => new
            {
                isParent = a.isParent,
                ModuleId = a.ModuleId,
                ModuleMenuId = a.ModuleMenuId,
                ModuleMenuCaption = a.ModuleMenuCaption,
                ParentId = a.ParentId
            }).ToList();
            return Json(new { data = moduleMenu });
        }
        public IActionResult GetParentMenu(int moduleId)
        {
            //var parentbManu = _context.ModuleMenus.Where(a => a.ModuleId == moduleId && a.isParent == 1).Select(a => new SelectListItem
            var parentbManu = _context.ModuleMenus.Where(a => a.ModuleId == moduleId && a.isParent == 1 && a.isInactive == 0).Select(a => new SelectListItem
            {
                Value = a.ModuleMenuId.ToString(),
                Text = a.ModuleMenuCaption
            }).ToList();
            //parentbManu.Insert(0, new ModuleMenu { ModuleMenuId = 0, ModuleMenuCaption = "Select" });

            return Json(parentbManu);
        }
        [HttpPost]
        public IActionResult GetChildMenu(int moduleId)
        {
            //var ChildMenu = _context.ModuleMenus.Where(a => a.ParentId == moduleId && a.isParent == 0).Select(a => new SelectListItem

            var ChildMenu = _context.ModuleMenus.Where(a => a.ParentId == moduleId && a.isParent == 0 && a.isInactive == 0).Select(a => new SelectListItem
            {
                Value = a.ModuleMenuId.ToString(),
                Text = a.ModuleMenuCaption
            }).ToList();
            //parentbManu.Insert(0, new ModuleMenu { ModuleMenuId = 0, ModuleMenuCaption = "Select" });

            return Json(ChildMenu);
        }
        [HttpPost]
        public IActionResult GetLink(int menuId, int moduleMenuId)
        {
            var userId = HttpContext?.Session?.GetString("userid") ?? "";
            var menuName = _context.ModuleMenus.Where(a => a.ModuleMenuId == menuId).Select(b => b.ModuleMenuCaption).SingleOrDefault();
            var isTrueOrFalse = (from T in _context.UserLoggingTracks
                                 join M in _context.ModuleCourses
                                 on T.ModuleCourseId equals M.ModuleCourseId
                                 where M.ModuleMenuChildId == menuId && T.UserId == userId
                                 select T.IsComplete).FirstOrDefault();
            var listOfLastChild = (from ULT in _context.UserLoggingTracks
                                   join MC in _context.ModuleCourses
                                   on ULT.ModuleCourseId equals MC.ModuleCourseId
                                   where MC.ModuleMenuId == moduleMenuId
                                   select new
                                   {
                                       ModuleMenuChildId = MC.ModuleMenuChildId,
                                       IsComplete = ULT.IsComplete
                                   }).ToList();
            var CompletedLastChildren = (from ULT in _context.UserLoggingTracks
                                         join MC in _context.ModuleCourses
                                         on ULT.ModuleCourseId equals MC.ModuleCourseId
                                         where MC.ModuleMenuId == moduleMenuId && ULT.IsComplete == true && ULT.UserId == userId
                                         select ULT.ModuleCourseId).ToList();
            var ReadyForQuiz = false;
            if (listOfLastChild.Count() <= CompletedLastChildren.Count())
            {
                ReadyForQuiz = true;
            }

            var menuLink = _context.ModuleCourses.Where(a => a.ModuleMenuChildId == menuId).Select(a => new
            {
                VideoLink = a.VideoLink,
                FilePath = a.FilePath,
                Description = a.Description,
                ModuleCourseId = a.ModuleCourseId

            }).SingleOrDefault();

            return Json(new { menuLink = menuLink, isTrueOrFalse = isTrueOrFalse, listOfTrue = listOfLastChild, menuName = menuName, IsReadyForQuiz = ReadyForQuiz });

            //return Json(menuLink);
        }

        [HttpPost]
        public IActionResult GetVideoWatchedOrNot(int moduleCourseId)
        {
            var menuLink = _context.UserLoggingTracks.Where(a => a.ModuleCourseId == moduleCourseId && a.UserId == HttpContext.Session.GetString("userid")).Select(a => new
            {

                LoggingTrackId = a.LoggingTrackId,
                ModuleCourseId = a.ModuleCourseId,
                UserId = a.UserId,
                IsComplete = a.IsComplete

            }).SingleOrDefault();
            return Json(menuLink);
        }

        public IActionResult InsertUserLoggingTrack()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CertificateInfo(int moduleId)
        {
            int moduleID;
            moduleID = _context.Modules.Where(a => a.ModuleId == moduleId).Select(b => b.ModuleId).SingleOrDefault();
            var moduleName = _context.Modules.Where(a => a.ModuleId == moduleId).Select(b => b.ModuleName).SingleOrDefault();
            var userEmail = HttpContext.Session.GetString("username");

            //CertificateDemo(userEmail, moduleName, moduleID);


            //var userFullName = HttpContext.Session.GetString("fullname");
            //CertificateInfo c = new CertificateInfo();

            //var certificateToken;
            //if ()
            //{
            //    TrcaeCert model = new 
            //}
            return Ok(new { moduleName = moduleName, userEmail = userEmail, moduleID = moduleID });
        }

        public IActionResult CertificateDemo(string moduleName, string userEmail, int moduleID)
        {


            var userId = HttpContext.Session.GetString("userid");
            var fullName = HttpContext.Session.GetString("fullname");
            var CertificateID = _context.CertificateInfos.Where(a => a.ModuleId == moduleID && a.UserEmail == userEmail).Select(x => x.CertificateId).FirstOrDefault();
            var AuthorizedBy = _context.Quiz.Where(a => a.ModuleId == moduleID).Select(x => x.preparedBy).FirstOrDefault() == null ? "GTR" : _context.Quiz.Where(a => a.ModuleId == moduleID).Select(x => x.preparedBy).FirstOrDefault();
            var Designation = _context.Quiz.Where(a => a.ModuleId == moduleID).Select(x => x.authDesig).FirstOrDefault() == null ? " " : _context.Quiz.Where(a => a.ModuleId == moduleID).Select(x => x.authDesig).FirstOrDefault();
            var authSign = _context.Quiz.Where(a => a.ModuleId == moduleID).Select(x => x.authSign).FirstOrDefault() == null ? " " : _context.Quiz.Where(a => a.ModuleId == moduleID).Select(x => x.authSign).FirstOrDefault();
            string path1 = Path.Combine(this._webHostEnvironment.WebRootPath, "EmpDocument/") + authSign;
            FileInfo file = new FileInfo(path1);
            string pathfile = "~/EmpDocument/" + authSign;
            if (file.Exists)//check file exsit or not  
            {
                ViewBag.imgUrl = pathfile;
            }
            else
            {
                ViewBag.imgUrl = "~/filesforcertificates/images/sign.png";
            }
            if (!_context.CertificateInfos.Any(a => a.ModuleId == moduleID && a.UserEmail == userEmail))
            {
                try
                {

                    CertificateInfo ce = new CertificateInfo();

                    ce.CertificateId = Guid.NewGuid().ToString();


                    var QRCodeText = "https://gtrbd.net/ERP/TutorialCertificate/CheckCertificate/?id=" + ce.CertificateId;

                    var writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new QrCodeEncodingOptions
                        {
                            Width = 200,
                            Height = 200
                        }
                    };
                    var result = writer.Write(QRCodeText);

                    // Convert the QR code to a base64-encoded string
                    var bitmap = new Bitmap(result);
                    var stream = new MemoryStream();
                    bitmap.Save(stream, ImageFormat.Png);
                    var base64String = Convert.ToBase64String(stream.ToArray());

                    // Set the base64-encoded string as a property on ViewBag
                    ViewBag.QRCode = $"data:image/png;base64,{base64String}";


                    string path = @".\wwwroot\GeneratedQRCode\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string filePath = @".\wwwroot\GeneratedQRCode\" + ce.CertificateId + ".png";
                    bitmap.Save(filePath, ImageFormat.Png);
                    //byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                    //string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    ViewBag.certificateID = QRCodeText;
                    ViewBag.barcode = base64String; 


                    ce.UserId = userId;
                    ce.ModuleId = moduleID;
                    ce.UserName = fullName;
                    //ce.QrCodeUrl = imageUrl;
                    ce.UserEmail = userEmail;

                    _context.Entry(ce).State = EntityState.Added;
                    _context.SaveChanges();
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                var QRCodeText = "https://gtrbd.net/ERP/TutorialCertificate/CheckCertificate/?id=" + CertificateID;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = 200,
                        Height = 200
                    }
                };
                var result = writer.Write(QRCodeText);

                var bitmap = new Bitmap(result);
                var stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                var base64String = Convert.ToBase64String(stream.ToArray());

                // Set the base64-encoded string as a property on ViewBag
                ViewBag.QRCode = $"data:image/png;base64,{base64String}";

                string path = @".\wwwroot\GeneratedQRCode\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode/" + CertificateID + ".png");
                string filePath = @".\wwwroot\GeneratedQRCode\" + CertificateID + ".png";
                bitmap.Save(filePath, ImageFormat.Png);

                ViewBag.certificateID = CertificateID.Substring(CertificateID.Length - 6);
                ViewBag.barcode = base64String;
                //ViewBag.certificateID = "https://gtrbd.net/ERP/TutorialCertificate/CheckCertificate/?id=" + CertificateID;
                //ViewBag.barcode = base64ImageRepresentation;
            }


            ViewBag.moduleName = moduleName;
            ViewBag.userEmail = userEmail;
            ViewBag.fullName = fullName;
            ViewBag.Authorized = AuthorizedBy;
            ViewBag.Designation = Designation;

            return View();

        }
        public IActionResult ShowCertificate(string id)
        {
            var certificateInfo = _context.CertificateInfos?.Where(a => a.CertificateId == id)
                 ?.Select(x => new { userId = x.UserName, moduleId = x.ModuleId, x.QrCodeUrl, x.UserEmail })
                 ?.FirstOrDefault();
            var moduleName = _context?.Modules?.Where(a => a.ModuleId == certificateInfo.moduleId)?.Select(x => x.ModuleName)?.FirstOrDefault();
            var AuthorizedBy = _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.preparedBy).FirstOrDefault() == null ? "GTR" : _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.preparedBy).FirstOrDefault();
            var Designation = _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authDesig).FirstOrDefault() == null ? " " : _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authDesig).FirstOrDefault();
            var authSign = _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authSign).FirstOrDefault() == null ? " " : _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authSign).FirstOrDefault();
            string path1 = Path.Combine(this._webHostEnvironment.WebRootPath, "EmpDocument/") + authSign;
            FileInfo file = new FileInfo(path1);
            string pathfile = "~/EmpDocument/" + authSign;
            if (file.Exists)//check file exsit or not  
            {
                ViewBag.imgUrl = pathfile;
            }
            else
            {
                ViewBag.imgUrl = "~/filesforcertificates/images/sign.png";
            }
            var QRCodeText = "https://gtrbd.net/ERP/TutorialCertificate/CheckCertificate/?id=" + id;
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 200,
                    Height = 200
                }
            };
            var result = writer.Write(QRCodeText);
            var bitmap = new Bitmap(result);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            var base64String = Convert.ToBase64String(stream.ToArray());

            // Set the base64-encoded string as a property on ViewBag
            //string path = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode");
            string path = @".\wwwroot\GeneratedQRCode\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = @".\wwwroot\GeneratedQRCode\" + id + ".png";
            bitmap.Save(filePath, ImageFormat.Png);
            //byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            ViewBag.barcode = base64String;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            //var fullName = HttpContext.Session.GetString("FullName");
            ViewBag.Authorized = AuthorizedBy;
            ViewBag.moduleName = moduleName;
            ViewBag.QrCodeUri = certificateInfo.QrCodeUrl;
            ViewBag.fullName = certificateInfo.userId;
            ViewBag.userEmail = certificateInfo.UserEmail;
            ViewBag.Designation = Designation;
            ViewBag.certificateID = id.Substring(id.Length - 6);
            return View();
        }

        [AllowAnonymous]
        public IActionResult CheckCertificate(string id)
        {
            var certificateInfo = _context.CertificateInfos?.Where(a => a.CertificateId == id)
                 ?.Select(x => new { userId = x.UserName, moduleId = x.ModuleId, x.QrCodeUrl, x.UserEmail })
                 ?.FirstOrDefault();
            var moduleName = _context?.Modules?.Where(a => a.ModuleId == certificateInfo.moduleId)?.Select(x => x.ModuleName)?.FirstOrDefault();
            var AuthorizedBy = _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.preparedBy).FirstOrDefault() == null ? "GTR" : _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.preparedBy).FirstOrDefault();
            var Designation =  _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authDesig).FirstOrDefault() == null ? " " : _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authDesig).FirstOrDefault();
            var authSign = _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authSign).FirstOrDefault() == null ? " " : _context.Quiz.Where(a => a.ModuleId == certificateInfo.moduleId).Select(x => x.authSign).FirstOrDefault();
            string path1 = Path.Combine(this._webHostEnvironment.WebRootPath, "EmpDocument/") + authSign;
            FileInfo file = new FileInfo(path1);
            string pathfile = "~/EmpDocument/" + authSign;
            if (file.Exists)//check file exsit or not  
            {
                ViewBag.imgUrl = pathfile;
            }
            else
            {
                ViewBag.imgUrl = "~/filesforcertificates/images/sign.png";
            }
            var QRCodeText = "https://gtrbd.net/ERP/TutorialCertificate/CheckCertificate/?id=" + id;
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 200,
                    Height = 200
                }
            };
            var result = writer.Write(QRCodeText);
            var bitmap = new Bitmap(result);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            var base64String = Convert.ToBase64String(stream.ToArray());

            // Set the base64-encoded string as a property on ViewBag
            //string path = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode");
            string path = @".\wwwroot\GeneratedQRCode\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = @".\wwwroot\GeneratedQRCode\" + id + ".png";
            bitmap.Save(filePath, ImageFormat.Png);
            //byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            ViewBag.barcode = base64String;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            //var fullName = HttpContext.Session.GetString("FullName");
            ViewBag.Authorized = AuthorizedBy;
            ViewBag.moduleName = moduleName;
            ViewBag.QrCodeUri = certificateInfo.QrCodeUrl;
            ViewBag.fullName = certificateInfo.userId;
            ViewBag.userEmail = certificateInfo.UserEmail;
            ViewBag.Designation = Designation;
            ViewBag.certificateID = id.Substring(id.Length - 6); 
            return View();
        }
        //Quiz

        public IActionResult Quiz()
        {
            var moduleList = (from module in _context.Modules
                              select new SelectListItem()
                              {
                                  Text = module.ModuleName,
                                  Value = module.ModuleId.ToString(),
                              }).ToList();
            ViewBag.moduleList = moduleList;
            return View();
        }
        [HttpPost]
        public IActionResult SaveQuiz(string ques, IFormFile file)
        {
            try
            {
                List<Quiz> quesAns = JsonConvert.DeserializeObject<List<Quiz>>(ques);
                List<Quiz> Q = new List<Quiz>();
                
                string FileNameUrl = UploadFiles(file, quesAns[0].ModuleId.ToString());
                foreach (var q in quesAns)
                {
                    var isfileExists = _context.Quiz.Where(x=>x.ModuleId == q.ModuleId).Select(s=>s.authSign).FirstOrDefault();
                    var existQid = _context.Quiz.Where(w => w.Id == q.Id).Select(s => s).FirstOrDefault();
                    if (existQid != null)
                    {
                        existQid.quiz = q.quiz;
                        existQid.passMark = q.passMark;
                        existQid.preparedBy = q.preparedBy;
                        existQid.authDesig = q.authDesig;
                        if(FileNameUrl != null)
                        {
                            existQid.authSign = FileNameUrl;
                        }
                        else
                        {
                            if (existQid.authSign != null)
                            {
                                isfileExists = existQid.authSign;
                                existQid.authSign = existQid.authSign;
                            }
                            else
                            {
                                existQid.authSign = "";
                            }
                        }
                        _context.Update(existQid);
                    }
                    else
                    {
                        Quiz qu = new Quiz();
                        qu.ModuleId = q.ModuleId;
                        qu.quiz = q.quiz;
                        qu.passMark = q.passMark;
                        qu.preparedBy = q.preparedBy;
                        qu.authDesig = q.authDesig;
                        if(FileNameUrl !=null)
                        {
                            qu.authSign = FileNameUrl;
                        }
                        else
                        {
                            qu.authSign = "";
                            if (/*isfileExists.Length > 1 ||*/ isfileExists != null)
                            {
                                qu.authSign = isfileExists;
                            }
                            else
                            {
                                qu.authSign = "";
                            }

                        }

                        Q.Add(qu);
                        _context.Quiz.AddRange(Q);
                    }

                }
                _context.SaveChanges();


                foreach (var q in quesAns)
                {
                    var existQid = _context.Quiz.Include(i => i.Answer).Where(w => w.Id == q.Id).Select(s => s).FirstOrDefault();
                    if (existQid != null)
                    {


                        foreach (var a in q.Answer)
                        {
                            var existAid = _context.Answer.Where(w => w.quizid == q.Id).Select(s => s).ToList();

                            foreach (var item in existAid)
                            {
                                if (item.Id == a.Id)
                                {
                                    item.ans = a.ans;
                                    item.isRight = a.isRight;
                                    item.optionType = a.optionType;
                                    _context.Answer.Update(item);
                                }
                            }



                        }
                    }
                    else
                    {
                        var answer = q.Answer.ToList();
                        List<Answer> answers = new List<Answer>();
                        foreach (var a in answer)
                        {
                            Answer An = new Answer();
                            An.quizid = _context.Quiz.Where(w => w.quiz == q.quiz && w.ModuleId == q.ModuleId).Select(s => s.Id).FirstOrDefault();
                            An.ans = a.ans;
                            An.isRight = a.isRight;
                            An.optionType = a.optionType;

                            answers.Add(An);
                        }

                        _context.Answer.AddRange(answers);
                    }
                    _context.SaveChanges();
                }


                return Json(new { res = true });

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        private string UploadFiles(IFormFile file, string title)
        {
            string FileName = null;
            if (file != null)
            {
                //string folder = "book/Gallery/";
                string serverFolder = _webHostEnvironment.WebRootPath + "\\EmpDocument\\";
                FileName = title.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(serverFolder, FileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

            }
            return FileName;
        }
        public IActionResult quizList()
        {


            List<quizAnsVm> qal = new List<quizAnsVm>();
            var dbconfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
            string sql = $"exec prcGetQuizList";
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        quizAnsVm qa = new quizAnsVm();

                        qa.ModuleId = Convert.ToInt32(dataReader["ModuleId"].ToString());
                        qa.ModuleName = dataReader["ModuleName"].ToString();
                        qa.totalques = Convert.ToInt32(dataReader["totalquiz"].ToString());
                        qa.preparedBy = dataReader["preparedBy"].ToString() == null ? "" : dataReader["preparedBy"].ToString();
                        qa.passMark = Convert.ToInt32(dataReader["passMark"].ToString());
                        qa.authSign = dataReader["authSign"].ToString();

                        string path1 = Path.Combine(this._webHostEnvironment.WebRootPath, "EmpDocument/") + qa.authSign;
                        FileInfo file = new FileInfo(path1);
                        string pathfile = "~/EmpDocument/" + qa.authSign;
                        if (file.Exists)//check file exsit or not  
                        {
                            qa.authSign = pathfile;
                        }
                        else
                        {
                            qa.authSign = null;
                        }
                        qal.Add(qa);

                    }
                }
            }
            return View(qal);


        }

        public IActionResult EditQuiz(int moduleid)
        {
            ViewBag.ModuleName = _context.Modules.Where(w => w.ModuleId == moduleid).Select(s => s.ModuleName).FirstOrDefault();
            ViewBag.PassMark = _context.Quiz.Where(w => w.ModuleId == moduleid).Select(s => s.passMark).FirstOrDefault();
            ViewBag.auth = _context.Quiz.Where(w => w.ModuleId == moduleid).Select(s => s.preparedBy).FirstOrDefault();
            ViewBag.desig = _context.Quiz.Where(w => w.ModuleId == moduleid).Select(s => s.authDesig).FirstOrDefault();
            ViewBag.Edit = "Edit";
            List<quizAnsVm> qal = new List<quizAnsVm>();
            var quiz = _context.Quiz.Where(w => w.ModuleId == moduleid).Include(a => a.Answer).Select(s => s).ToList();
            foreach (var q in quiz)
            {
                quizAnsVm qa = new quizAnsVm();
                qa.ModuleId = q.ModuleId;
                qa.quizId = q.Id;
                qa.question = q.quiz;
                qa.preparedBy = q.preparedBy;
                qa.authDesig = q.authDesig;
                qa.AnsTable = q.Answer.Select(s => s).ToList();
                qa.optionType = q.Answer.Select(s => s.optionType).FirstOrDefault();

                qal.Add(qa);
            }

            return View(qal);
        }

        public IActionResult DeleteQuiz(int moduleid)
        {

            var quiz = _context.Quiz.Where(w => w.ModuleId == moduleid).Include(a => a.Answer).Select(s => s).ToList();

            foreach (var q in quiz)
            {
                foreach (var item in q.Answer)
                {
                    _context.Answer.Remove(item);
                }
                _context.Quiz.Remove(q);
            }
            _context.SaveChanges();
            return RedirectToAction("quizList");
        }
        [HttpPost]
        public IActionResult DeleteQues(int quesNo)
        {

            var quiz = _context.Quiz.Where(w => w.Id == quesNo).Include(a => a.Answer).Select(s => s).ToList();

            foreach (var q in quiz)
            {
                foreach (var item in q.Answer)
                {
                    _context.Answer.Remove(item);
                }
                _context.Quiz.Remove(q);
            }
            _context.SaveChanges();
            return Json(new { res = true });
        }



        public IActionResult QuizExamPage(int moduleId, string msg)
        {
            var userEmail = HttpContext.Session.GetString("username");
            ViewBag.UserId = userEmail;
            ViewBag.msg = msg;
            List<quizAnsVm> qal = new List<quizAnsVm>();
            var quiz = _context.Quiz.Where(w => w.ModuleId == moduleId).Include(a => a.Answer).Select(s => s).ToList();
            foreach (var q in quiz)
            {
                quizAnsVm qa = new quizAnsVm();
                qa.ModuleId = q.ModuleId;
                qa.quizId = q.Id;
                qa.question = q.quiz;
                qa.passMark = q.passMark;
                qa.answer = (List<string>)q.Answer.Select(s => s.ans).ToList();
                qa.optionType = q.Answer.Select(s => s.optionType).FirstOrDefault();

                qal.Add(qa);
            }


            return View(qal);
        }

        [HttpPost]
        public IActionResult CheckQuiz(List<quizAnsVm> QuizAns)
        {

            double score = 0;
            int ModuleId = 0;
            int PassMark = _context.Quiz.Where(x => x.ModuleId == QuizAns[0].ModuleId).Select(z => z.passMark).FirstOrDefault();

            foreach (var qa in QuizAns)
            {
                ModuleId = qa.ModuleId;
                //PassMark = qa.passMark;
                //var result = _context.Answer.Where(w => w.quizid == qa.quizId && w.isRight == true).Select(s => s.ans).ToList();
                var result = _context.Answer.Where(w => w.quizid == qa.quizId && w.isRight == true).ToList();
                foreach (var ans in result)
                {
                    if(ans.optionType == "text" && ans.ans.Length>=5)
                    {
                        score += 1;
                    }
                    else
                    if (ans.ans == qa.answer.FirstOrDefault())
                    {
                        score += 1;
                    }
                }
                //foreach (var ans in result)
                //{

                //    if (ans == qa.answer.FirstOrDefault())
                //    {
                //        score += 1;
                //    }
                //}
            }
            double CountQues = QuizAns.Count();
            var finalScore = ((score / CountQues) * 100);
            if (finalScore >= PassMark)
            {
                var moduleName = _context.Modules.Where(a => a.ModuleId == ModuleId).Select(b => b.ModuleName).SingleOrDefault();
                var userEmail = HttpContext.Session.GetString("username");
                return Json(new { moduleName = moduleName, userEmail = userEmail, moduleID = ModuleId, result = "pass" });


            };
            return Json(new { moduleID = ModuleId, msg = "Not up to Mark! Please try again", result = "fail" });
        }




    }

}
