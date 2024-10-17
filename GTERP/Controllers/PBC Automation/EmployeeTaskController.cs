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
    public class EmployeeTaskController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        private readonly IHostingEnvironment env;
        public EmployeeTaskController(TransactionLogRepository tran, GTRDBContext db, IHostingEnvironment env)
        {
            this.db = db;
            this.tranlog = tran;
            this.env = env;
        }
        public IActionResult Index(int id)
        {
            return View(db.PBC_EmployeeTask
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.PBC_Status).ToList());
        }
        public IActionResult Details(int id)
        {
            var data = db.PBC_TaskAssign
                .Include(x => x.HR_Emp_Info)
                .Where(x => x.TaskAssignId == id).FirstOrDefault();
            return View(data);
        }

        public IActionResult Create(int id)
        {
            ViewBag.action = "Create";

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.ComId = comid;

            var data1 = db.PBC_TaskAssign
                .Include(x => x.HR_Emp_Info)
                .Where(x => x.TaskAssignId == id).FirstOrDefault();
            ViewBag.Emp = db.PBC_TaskAssign.Where(x => x.EmpId == data1.EmpId).Select(x => x.HR_Emp_Info.EmpName).FirstOrDefault();
            ViewBag.EmpId = db.PBC_TaskAssign.Where(x => x.EmpId == data1.EmpId).Select(x => x.HR_Emp_Info.EmpId).FirstOrDefault();
            ViewBag.TaskAssignId = db.PBC_TaskAssign.Where(x => x.EmpId == data1.EmpId).Select(x => x.TaskAssignId).FirstOrDefault();

            var data = db.PBC_EmployeeTask.Include(x => x.PBC_TaskAssign).Include(y => y.HR_Emp_Info).Where(c => c.ComId == comid).FirstOrDefault();
            return View(new EmployeeTaskVM
            {
                EmployeeTaskId = data.EmployeeTaskId,
                ComId = data.ComId,
                IsComplete = data.IsComplete,
                EmpId = data.EmpId,
                TaskAssignId = data.TaskAssignId,
                User_Comments = data.User_Comments,
                SubmitDate = data.SubmitDate,
                StatusDate = data.StatusDate,
                PBCStatusId = data.PBCStatusId
            });

        }

        [HttpPost]
        public IActionResult Create(EmployeeTaskVM empTask)
        {
            ViewBag.action = "Create";

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.ComId = comid;


            if (ModelState.IsValid)
            {
                PBC_EmployeeTask e = new PBC_EmployeeTask
                {
                    User_Comments = empTask.User_Comments,
                    IsComplete = empTask.IsComplete,
                    ComId = empTask.ComId,
                    EmpId = empTask.EmpId,
                    TaskAssignId = empTask.TaskAssignId,
                    SubmitDate = empTask.SubmitDate,
                    StatusDate = empTask.StatusDate,
                    PBCStatusId = empTask.PBCStatusId,
                    Files = ""
                };
                if (empTask.Files != null)
                {
                    var imagePath = env.WebRootPath + @"\Images";

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(empTask.Files.FileName);
                    FileStream fs = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    empTask.Files.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    e.Files = fileName;
                }
                db.PBC_EmployeeTask.Add(e);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empTask);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.action = "Edit";

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.ComId = comid;

            var data1 = db.PBC_EmployeeTask
                .Include(x => x.PBC_TaskAssign)
                .Include(x => x.HR_Emp_Info)
                .Where(x => x.EmployeeTaskId == id).FirstOrDefault();
            ViewBag.Emp = db.PBC_TaskAssign.Where(x => x.EmpId == data1.EmpId).Select(x => x.HR_Emp_Info.EmpName).FirstOrDefault();
            ViewBag.EmpId = db.PBC_TaskAssign.Where(x => x.EmpId == data1.EmpId).Select(x => x.HR_Emp_Info.EmpId).FirstOrDefault();
            ViewBag.TaskAssignId = db.PBC_TaskAssign.Where(x => x.EmpId == data1.EmpId).Select(x => x.TaskAssignId).FirstOrDefault();


            var e = db.PBC_EmployeeTask.First(x => x.EmployeeTaskId == id);
            ViewBag.Files = e.Files;
            return View("Create", new EmployeeTaskVM
            {
                EmployeeTaskId = e.EmployeeTaskId,
                ComId = e.ComId,
                IsComplete = e.IsComplete,
                EmpId = e.EmpId,
                TaskAssignId = e.TaskAssignId,
                User_Comments = e.User_Comments,
                SubmitDate = e.SubmitDate,
                StatusDate = e.StatusDate,
                HODRemarks = e.HODRemarks,
                MNGRemarks = e.MNGRemarks,
                CheckedBy = e.CheckedBy,
                ApprovedBy = e.ApprovedBy,
                PBCStatusId = e.PBCStatusId
            });
        }
        [HttpPost]
        public IActionResult Edit(EmployeeTaskVM empTask)
        {
            ViewBag.action = "Edit";

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.ComId = comid;


            if (ModelState.IsValid)
            {

                var et = db.PBC_EmployeeTask.Where(x => x.EmployeeTaskId == empTask.EmployeeTaskId).FirstOrDefault();

                if (empTask.User_Comments != null)
                {
                    et.User_Comments = empTask.User_Comments;
                }

                et.IsComplete = empTask.IsComplete;
                et.ComId = empTask.ComId;
                et.EmpId = empTask.EmpId;
                et.TaskAssignId = empTask.TaskAssignId;
                et.SubmitDate = empTask.SubmitDate;
                et.StatusDate = empTask.StatusDate;
                et.HODRemarks = empTask.HODRemarks;
                et.MNGRemarks = empTask.MNGRemarks;
                et.CheckedBy = empTask.CheckedBy;
                et.ApprovedBy = empTask.ApprovedBy;
                et.PBCStatusId = empTask.PBCStatusId;
                if (empTask.Files != null)
                {
                    var imagePath = env.WebRootPath + @"\Images";

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(empTask.Files.FileName);
                    FileStream fs = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    empTask.Files.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    et.Files = fileName;
                }
                db.Entry(et).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empTask);
        }

        [HttpPost]
        public IActionResult UpdateStatus(EmployeeTaskVM empTask)
        {
            if (ModelState.IsValid)
            {

                var et = db.PBC_EmployeeTask.Where(x => x.EmployeeTaskId == empTask.EmployeeTaskId).FirstOrDefault();

                if (empTask.HODRemarks != null && empTask.CheckedBy != null)
                {
                    et.CheckedBy = empTask.CheckedBy;
                    et.HODRemarks = empTask.HODRemarks;
                    et.StatusDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
                    et.PBCStatusId = empTask.PBCStatusId;
                    db.Entry(et).State = EntityState.Modified;
                    db.SaveChanges();

                }

                if (empTask.MNGRemarks != null && empTask.ApprovedBy != null)
                {
                    et.ApprovedBy = empTask.ApprovedBy;
                    et.MNGRemarks = empTask.MNGRemarks;
                    et.StatusDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
                    et.PBCStatusId = empTask.PBCStatusId;
                    db.Entry(et).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(new { Success = 1, message = "Data Updated Successfully" });
        }

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

        public IActionResult HODCheck()
        {
            ViewBag.Status = new SelectList(db.PBC_Status, "PBCStatusId", "Status");

            return View(db.PBC_EmployeeTask
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.PBC_Status).Where(x => x.PBCStatusId == 1).ToList());
        }
        public IActionResult ManagementApprove()
        {
            ViewBag.Status = new SelectList(db.PBC_Status, "PBCStatusId", "Status");

            return View(db.PBC_EmployeeTask
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.PBC_Status).Where(x => x.PBCStatusId == 2).ToList());
        }

        public class EmployeeTaskVM
        {
            public int EmployeeTaskId { get; set; }
            public IFormFile Files { get; set; }
            public string User_Comments { get; set; }
            public bool IsComplete { get; set; }
            public string ComId { get; set; }
            public DateTime SubmitDate { get; set; }
            public DateTime StatusDate { get; set; }
            public string HODRemarks { get; set; }
            public string MNGRemarks { get; set; }
            public string CheckedBy { get; set; }
            public string ApprovedBy { get; set; }
            public int EmpId { get; set; }
            public int UserId { get; set; }
            public int TaskAssignId { get; set; }
            public int PBCStatusId { get; set; }
            public string EmpName { get; set; }
        }
    }
}
