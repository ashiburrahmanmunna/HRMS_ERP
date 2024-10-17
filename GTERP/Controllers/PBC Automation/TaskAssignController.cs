using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace GTERP.Controllers.PBC_Automation
{
    public class TaskAssignController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        private readonly IHostingEnvironment env;
        public TaskAssignController(TransactionLogRepository tran, GTRDBContext db, IHostingEnvironment env)
        {
            this.db = db;
            this.tranlog = tran;
            this.env = env;
        }
        public IActionResult Index()
        {

            return View(db.PBC_TaskAssign
                .Include(x => x.HR_Emp_Info)
                .ToList());
            return View();
        }
        public IActionResult TaskAssignE()
        {

            return View(db.PBC_TaskAssign
                .Include(x => x.HR_Emp_Info)
                .ToList());
            return View();
        }

        public IActionResult Details(int id)
        {
            var data = db.PBC_TaskAssign
                .Include(x => x.HR_Emp_Info)
                .Where(x => x.TaskAssignId == id).FirstOrDefault();
            return View(data);
        }

        public IActionResult Create()
        {

            ViewBag.action = "Create";
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.ComId = comid;

            var empData = db.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .Select(s => new
                {
                    EmpId = s.EmpId,

                    DesigName = s.Cat_Designation.DesigName,
                    DeptName = s.Cat_Department.DeptName,
                    SectName = s.Cat_Section.SectName
                })
                .ToList();
            ViewBag.EmpInfo = empData;


            var empInfo = (from emp in db.HR_Emp_Info
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId

                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] " + " - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");


            return View("Create");
        }
        [HttpPost]
        public IActionResult Create(TaskAssignVM task)
        {

            ViewBag.action = "Create";
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.ComId = comid;

            var empData = db.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .Select(s => new
                {
                    EmpId = s.EmpId,

                    DesigName = s.Cat_Designation.DesigName,
                    DeptName = s.Cat_Department.DeptName,
                    SectName = s.Cat_Section.SectName
                })
                .ToList();
            ViewBag.EmpInfo = empData;


            var empInfo = (from emp in db.HR_Emp_Info
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId

                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] " + " - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");

            if (ModelState.IsValid)
            {
                PBC_TaskAssign ta = new PBC_TaskAssign
                {
                    TaskAssignId = task.TaskAssignId,
                    EmpId = task.EmpId,
                    ComId = comid,
                    Year = task.Year,
                    Duration = task.Duration,
                    Quarter = task.Quarter,
                    Type_BV_Description = task.Type_BV_Description,
                    Type_BV_Task = task.Type_BV_Task,
                    Type_K_Description = task.Type_K_Description,
                    Type_K_Task = task.Type_K_Task,
                    Type_M_Description = task.Type_M_Description,
                    Type_M_Task = task.Type_M_Task,
                    Type_T_Description = task.Type_T_Description,
                    Type_T_Task = task.Type_T_Task,
                    Documents = "",
                    AssignDate = task.AssignDate,
                    SubmiteDate = task.SubmiteDate
                };
                if (task.Documents != null)
                {
                    var imagePath = env.WebRootPath + @"\Images";

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(task.Documents.FileName);

                    FileStream fs = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    task.Documents.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    ta.Documents = fileName;
                }
                db.PBC_TaskAssign.Add(ta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }


        [HttpGet]
        public IActionResult Edit(int ts)
        {


            ViewBag.action = "Edit";
            var comid = HttpContext.Session.GetString("comid");

            var empData = db.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .Select(s => new
                {
                    EmpId = s.EmpId,

                    DesigName = s.Cat_Designation.DesigName,
                    DeptName = s.Cat_Department.DeptName,
                    SectName = s.Cat_Section.SectName
                })
                .ToList();
            ViewBag.EmpInfo = empData;


            var empInfo = (from emp in db.HR_Emp_Info
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId

                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] " + " - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            var task = db.PBC_TaskAssign.FirstOrDefault(x => x.TaskAssignId == ts);
            return View("Create", new TaskAssignVM
            {
                TaskAssignId = task.TaskAssignId,
                EmpId = task.EmpId,
                ComId = comid,
                Year = task.Year,
                Duration = task.Duration,
                Quarter = task.Quarter,
                Type_BV_Description = task.Type_BV_Description,
                Type_BV_Task = task.Type_BV_Task,
                Type_K_Description = task.Type_K_Description,
                Type_K_Task = task.Type_K_Task,
                Type_M_Description = task.Type_M_Description,
                Type_M_Task = task.Type_M_Task,
                Type_T_Description = task.Type_T_Description,
                Type_T_Task = task.Type_T_Task,
                AssignDate = task.AssignDate,
                SubmiteDate = task.SubmiteDate
            });
            return View();
        }

        [HttpPost]
        public IActionResult Edit(TaskAssignVM task)
        {

            ViewBag.action = "Edit";
            var comid = HttpContext.Session.GetString("comid");

            var empData = db.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .Select(s => new
                {
                    EmpId = s.EmpId,

                    DesigName = s.Cat_Designation.DesigName,
                    DeptName = s.Cat_Department.DeptName,
                    SectName = s.Cat_Section.SectName
                })
                .ToList();
            ViewBag.EmpInfo = empData;


            var empInfo = (from emp in db.HR_Emp_Info
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId

                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] " + " - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            PBC_TaskAssign ta = new PBC_TaskAssign
            {
                TaskAssignId = task.TaskAssignId,
                EmpId = task.EmpId,
                ComId = comid,
                Year = task.Year,
                Duration = task.Duration,
                Quarter = task.Quarter,
                Type_BV_Description = task.Type_BV_Description,
                Type_BV_Task = task.Type_BV_Task,
                Type_K_Description = task.Type_K_Description,
                Type_K_Task = task.Type_K_Task,
                Type_M_Description = task.Type_M_Description,
                Type_M_Task = task.Type_M_Task,
                Type_T_Description = task.Type_T_Description,
                Type_T_Task = task.Type_T_Task,
                Documents = "",
                AssignDate = task.AssignDate,
                SubmiteDate = task.SubmiteDate
            };
            if (task.Documents != null)
            {
                var imagePath = env.WebRootPath + @"\Images";

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(task.Documents.FileName);

                FileStream fs = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                task.Documents.CopyTo(fs);
                fs.Flush();
                fs.Close();
                ta.Documents = fileName;
            }
            db.Entry(ta).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int ts)
        {
            ViewBag.action = "Delete";
            var comid = HttpContext.Session.GetString("comid");

            var empData = db.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .Select(s => new
                {
                    EmpId = s.EmpId,

                    DesigName = s.Cat_Designation.DesigName,
                    DeptName = s.Cat_Department.DeptName,
                    SectName = s.Cat_Section.SectName
                })
                .ToList();
            ViewBag.EmpInfo = empData;


            var empInfo = (from emp in db.HR_Emp_Info
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId

                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] " + " - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            var task = db.PBC_TaskAssign.FirstOrDefault(x => x.TaskAssignId == ts);
            return View("Create", new TaskAssignVM
            {
                TaskAssignId = task.TaskAssignId,
                EmpId = task.EmpId,
                ComId = comid,
                Year = task.Year,
                Duration = task.Duration,
                Quarter = task.Quarter,
                Type_BV_Description = task.Type_BV_Description,
                Type_BV_Task = task.Type_BV_Task,
                Type_K_Description = task.Type_K_Description,
                Type_K_Task = task.Type_K_Task,
                Type_M_Description = task.Type_M_Description,
                Type_M_Task = task.Type_M_Task,
                Type_T_Description = task.Type_T_Description,
                Type_T_Task = task.Type_T_Task,
                AssignDate = task.AssignDate,
                SubmiteDate = task.SubmiteDate
            });
            return View();

        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(PBC_TaskAssign ts)
        {
            var data = new PBC_TaskAssign { TaskAssignId = ts.TaskAssignId };
            db.Entry(data).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // DownLoad part

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename is not availble");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip", "application/zip" },
                {".rar" , "application/vnd.rar"}
            };
        }


        public class TaskAssignVM
        {
            public int TaskAssignId { get; set; }
            public string ComId { get; set; }
            public string Year { get; set; }
            public string Quarter { get; set; }
            public DateTime AssignDate { get; set; }
            public DateTime SubmiteDate { get; set; }
            //public int RemainingDays { get; set; }
            public string Duration { get; set; }
            public string Type_T_Task { get; set; }
            public string Type_T_Description { get; set; }
            public string Type_M_Task { get; set; }
            public string Type_M_Description { get; set; }
            public string Type_K_Task { get; set; }
            public string Type_K_Description { get; set; }
            public string Type_BV_Task { get; set; }
            public string Type_BV_Description { get; set; }
            public IFormFile Documents { get; set; }
            public int EmpId { get; set; }
            public int UserId { get; set; }
        }

    }
}
