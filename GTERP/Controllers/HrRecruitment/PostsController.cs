using DocumentFormat.OpenXml.Bibliography;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRrecruitment;
using GTERP.Migrations.GTRDB;
using GTERP.Models;
using GTERP.Models.Recruitment;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace GTERP.Controllers.HrRecruitments
{
    public class PostsController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly IPostRepository _ipostrepository;


        public PostsController(IPostRepository postrepository, GTRDBContext context)
        {
            _ipostrepository = postrepository;
            _context = context;
        }




        public IActionResult PostIndex()
        {
            var data= _ipostrepository.GetAll();

            //var dd= _context.HR_Recruitment_Posts.Includ
            return View(data);
        }





        [HttpPost]
        public ActionResult GetTemplete(int pId)
        {
           //var data1 = _ipostrepository.GetTemplete(pId);
           // var dd = Json(data1);


            //failed to return json from repository
          var data= _context.HR_Recruitment_Templetes.Where(w => w.TempId == pId).Select(s => s).FirstOrDefault();
            return Json(data);
        }





        public IActionResult PostCreate()
        {
            ViewData["DepartmentName"] = new SelectList(_context.HR_Recruitment_Department, "DeptId", "Dept_Name");
            ViewData["PostTitle"] = new SelectList(_context.HR_Recruitment_Templetes, "TempId", "Title");
            return View();
        }




        [HttpPost]
        public IActionResult Create(HR_Recruitment_Post post)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            post.ComId = comid;
            post.UserId = userid;
            _ipostrepository.CreatePost(post);
            return RedirectToAction(nameof(PostIndex));
        }



        public async Task<IActionResult> PostEdit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.HR_Recruitment_Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["DepartmentName"] = new SelectList(_context.HR_Recruitment_Department, "DeptId", "Dept_Name", post.Department);
            ViewData["PostTitle"] = new SelectList(_context.HR_Recruitment_Templetes, "TempId", "Title");
            return View(post);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostEdit(int id, HR_Recruitment_Post post)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            if (id != post.PostId)
            {
                return NotFound();
            }

            post.ComId = comid;
            post.UserId = userid;
            _ipostrepository.UpdatePost(post);
                
                
            
            return RedirectToAction(nameof(Index));
        }

        
       

        // GET: Posts/Delete/5
        public IActionResult PostDelete(int id)
        {


            ViewBag.Title = "Delete";

            var post = _ipostrepository.GetPostbyId(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
           
            _ipostrepository.DeleteConfirmed(id);

            return RedirectToAction(nameof(PostIndex));
        }
        public  IActionResult PostDetails(int id)
        {
            
            var post = _ipostrepository.GetPostbyId(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public IActionResult goodluckpage()
        {
            return View(_ipostrepository.GetDepartment());
        }




        public IActionResult DepartmentIndex()
        {
            return View( _ipostrepository.GetDepartment());
        }

        public IActionResult ModuleIndex()
        {
            return View(_context.Exam_Modules.ToList());
        }


        public IActionResult DepartmentDetails(int id)
        {
            

            

            return View(_ipostrepository.DepartmentDetails(id));
        }



        public IActionResult DepartmentCreate()
        {
            return View();
        }





        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult DepartmentCreate( DeptVm department)
        {


            _ipostrepository.DepartmentCreate(department);
            
            return RedirectToAction(nameof(DepartmentIndex)); ;
        }



        public  IActionResult DepartmentEdit(int id)
        {
            

            var department = _context.HR_Recruitment_Department.Find(id); 
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }


        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DepartmentEdit(int id,  HR_Recruitment_Department department)
        {



            _ipostrepository.DepartmentEdit(department);



            return RedirectToAction(nameof(DepartmentIndex)); ;
        }
        public IActionResult DepartmentDelete(int id)
        {




            var department = _context.HR_Recruitment_Department.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Posts/Delete/
        
        
        public IActionResult DepartmentConfirmedDelete(int id)
        {


            _ipostrepository.DepartmentDelete(id);
            return RedirectToAction(nameof(DepartmentIndex));
        }
         


        public IActionResult JobIndex()
        {
            var query = _ipostrepository.GetAll();

            return View(query);
        }



        public IActionResult ApplyForm(int id)
        {
            
            var query = _context.HR_Recruitment_Posts.Where(x => x.PostId == id).Select(x => x.PostTitle).FirstOrDefault();

            ViewBag.id = id;
            ViewBag.PostTitle = query;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForm(SubmittedCVVM submittedCVVM)
        {
            var alreadysubmitted = _context.HR_Recruitment_SubmittedCVs.Where(x => x.SubmittedCV_Email == submittedCVVM.Email && x.PostId == submittedCVVM.PostId).FirstOrDefault();


            if (ModelState.IsValid && alreadysubmitted == null)
            {
                using (var ms = new MemoryStream())
                {
                    submittedCVVM.Attachment.CopyTo(ms);

                    var data = new HR_Recruitment_SubmittedCV()
                    {
                        SubmittedCV_Name = submittedCVVM.Name,
                        SubmittedCV_Email = submittedCVVM.Email,
                        SubmittedCV_Number = submittedCVVM.Number,
                        PostId = submittedCVVM.PostId,
                        IsDelete = false,
                        SubmittedCV_ExpectedSalary = submittedCVVM.ExpectedSalary,
                        SubmittedCV_CoverLetter = submittedCVVM.CoverLetter,
                        SubmittedCV_linkedin_Url = submittedCVVM.linkedin_Url,
                        SubmittedDate = DateTime.Now,


                        Attachment = ms.ToArray(),
                        FileName = Path.GetFileName(submittedCVVM.Attachment.FileName),
                        FileType = submittedCVVM.Attachment.ContentType,

                    };
                    _context.Add(data);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return Json("You Have already applied for this position");
            //return View(submittedCVVM);
        }

       

        public IActionResult CVSort()
        {
            //var list = _context.HR_Recruitment_SubmittedCVs.Where(x => x.IsDelete == false).ToList();
            var q = (from p in _context.HR_Recruitment_Posts
                     join s in _context.HR_Recruitment_SubmittedCVs.Where(x => x.IsDelete == false) on p.PostId equals s.PostId
                     select new cvVM
                     {
                         S_Id = s.S_Id,
                         Name = s.SubmittedCV_Name,
                         Email = s.SubmittedCV_Email,
                         Number = s.SubmittedCV_Number,
                         linkedin_Url = s.SubmittedCV_linkedin_Url,
                         ExpectedSalary = s.SubmittedCV_ExpectedSalary,
                         CoverLetter = s.SubmittedCV_CoverLetter,
                         SubmittedDate = s.SubmittedDate,
                         PostTitle = p.PostTitle,
                     }).ToList();

            return View(q);
        }



        public async Task<FileResult> Download(int id)
        {
            var file = await _context.HR_Recruitment_SubmittedCVs.Where(x => x.S_Id == id).SingleOrDefaultAsync();
            return File(file.Attachment, file.FileType);
        }

        public IActionResult OpeningDetails(int id)
        {

            


            var post = _context.HR_Recruitment_Posts.Include(x => x.Department).Where(x => x.PostId == id).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public async Task<IActionResult> ApplicantDetails(int id)
        {

            
            var q = (from p in _context.HR_Recruitment_Posts
                     join s in _context.HR_Recruitment_SubmittedCVs.Where(x => x.S_Id == id) on p.PostId equals s.PostId
                     select p.PostTitle).FirstOrDefault();

            ViewBag.PostTitle = q;

            var post = await _context.HR_Recruitment_SubmittedCVs.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }


        [HttpGet]
        public async Task<IActionResult> IsSubDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.HR_Recruitment_SubmittedCVs.FindAsync(id);
            post.IsDelete = true;

            _context.Update(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CVSort));
        }

        public async Task<IActionResult> SubcvDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.HR_Recruitment_SubmittedCVs
                
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmittedcvDeleteConfirmed(int id)
        {
            var post = await _context.HR_Recruitment_Posts.FindAsync(id);
            _context.HR_Recruitment_Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RequizitionIndex()
        {
            //var data = _context.Requisition.Include(r => r.Department).AsNoTracking().ToList(); 
            var data2 = _context.HR_Recruitment_Requisition.Include(x => x.Department).AsNoTracking().ToList();
            return View(data2);
        }

        

        // GET: Requisitions/Create
        public IActionResult RequisitionCreate()
        {
            ViewData["DeptId"] = new SelectList(_context.HR_Recruitment_Department, "DeptId", "Dept_Name");
            return View();
        }

        // POST: Requisitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequisitionCreate(HR_Recruitment_Requisition requisition)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            requisition.ComId = comid;
            requisition.UserId = userid;
                _context.Add(requisition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RequizitionIndex));
            
            ViewData["DeptId"] = new SelectList(_context.HR_Recruitment_Department, "DeptId", "Dept_Name", requisition.DeptId);
            return View(requisition);
        }

        // GET: Requisitions/Edit/5
        public async Task<IActionResult> RequisitionEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisition = await _context.HR_Recruitment_Requisition.FindAsync(id);
            if (requisition == null)
            {
                return NotFound();
            }
            ViewData["DeptId"] = new SelectList(_context.HR_Recruitment_Department, "DeptId", "Dept_Name", requisition.DeptId);
            return View(requisition);
        }

        // POST: Requisitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequisitionEdit(int id, HR_Recruitment_Requisition requisition)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            if (id != requisition.RequId)
            {
                return NotFound();
            }

            
                try
                {


                    requisition.ComId= comid;
                    requisition.UserId= userid;
                    _context.Update(requisition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisitionExists(requisition.RequId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
             
            ViewData["DeptId"] = new SelectList(_context.HR_Recruitment_Department, "DeptId", "Dept_Name", requisition.DeptId);
            return View(requisition);
        }

        // GET: Requisitions/Delete/5
        public async Task<IActionResult> RequisitionDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisition = await _context.HR_Recruitment_Requisition
                .Include(r => r.Department)
                .FirstOrDefaultAsync(m => m.RequId == id);
            if (requisition == null)
            {
                return NotFound();
            }

            return View(requisition);
        }

        // POST: Requisitions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequisitionDeleteConfirmed(int id)
        {
            var requisition = await _context.HR_Recruitment_Department.FindAsync(id);
            _context.HR_Recruitment_Department.Remove(requisition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RequizitionIndex));
        }

        private bool RequisitionExists(int id)
        {
            return _context.HR_Recruitment_Requisition.Any(e => e.RequId == id);
        }


        public ActionResult ApplicantList(int id)
        {

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var data = _context.HR_Recruitment_SubmittedCVs.Where(x => x.S_Id == id).FirstOrDefault();
            HR_Recruitment_Applicant ap = new HR_Recruitment_Applicant();

            ap.ComId = comid;
            ap.UserId=userid;
            ap.ExamResult = 0;
            ap.VivaResult = 0;
            ap.Status = 1;
            ap.S_Id = data.S_Id;
            ap.PostId = data.PostId;
            _context.Add(ap);
            _context.SaveChanges();
            return RedirectToAction(nameof(EvaluationIndex));
        }

        public ActionResult EvaluationIndex()
        {
            var data = _context.HR_Recruitment_Applicants.ToList();

            var q = (from p in _context.HR_Recruitment_Posts
                     join s in _context.HR_Recruitment_SubmittedCVs on p.PostId equals s.PostId
                     join a in _context.HR_Recruitment_Applicants on s.S_Id equals a.S_Id
                     select new ListVM
                     {
                         App_Id = a.Applicant_Id,
                         PostTitle = p.PostTitle,
                         App_Name = s.SubmittedCV_Name,
                         ExamResult = a.ExamResult,
                         VivaResult = a.VivaResult,
                         Status = a.Status
                     }).ToList();

            return View(q);
        }

        
        



        

        
        [HttpPost]
        public IActionResult Moduleupdate(List<modlisti> mod)
        {
            int sid = 0;
            int appid = 0;
            string module = "";
            if (mod.Count > 0)
            {

                foreach (var moditem in mod)
                {
                    module = moditem.module;
                    sid = moditem.s_id;
                    appid = moditem.Appid;
                }
                var moduleupdate = _context.Exam_Modules.Where(w => w.module == module).Select(s => s).FirstOrDefault();

                var up = _context.HR_Recruitment_Applicants.Where(w => w.Applicant_Id == appid).Select(s => s).FirstOrDefault();
                up.Moduleid = moduleupdate.ModuleId;
                _context.Update(up);
                _context.SaveChanges();

            }



            return RedirectToAction(nameof(EvaluationIndex));
        }


        // GET: EvaluationController/Details/5
        public ActionResult EvaluationDetails(int id)
        {
            var data = _context.HR_Recruitment_Applicants.ToList();
            var mod = _context.Exam_Modules.Select(s => s).ToList();
            var q = (from p in _context.HR_Recruitment_Posts
                     join s in _context.HR_Recruitment_SubmittedCVs on p.PostId equals s.PostId
                     join a in _context.HR_Recruitment_Applicants on s.S_Id equals a.S_Id
                     where a.Applicant_Id == id
                     select new ListVM
                     {
                         App_Id = a.Applicant_Id,
                         PostTitle = p.PostTitle,
                         App_Name = s.SubmittedCV_Name,
                         ExamResult = a.ExamResult,
                         VivaResult = a.VivaResult,
                         Status = a.Status,
                         Email = s.SubmittedCV_Email,
                         SubmittedDate = s.SubmittedDate,
                         linkedin_Url = s.SubmittedCV_linkedin_Url,
                         Comment = a.Comment,
                         Number = s.SubmittedCV_Number,
                         ModuleId = a.Moduleid,
                         modulelist = mod,
                         ModuleName = "https://localhost:44389/Quizs/QuizExamPage/" + a.Moduleid,
                     }).ToList();


            return View(q);
        }

        





        // GET: EvaluationController/Edit/5
        public ActionResult EvaluationEdit(int id)
        {


            var data = _context.HR_Recruitment_Applicants.Where(x => x.Applicant_Id == id).FirstOrDefault();
            var P_Id = _context.HR_Recruitment_Applicants.Where(x => x.Applicant_Id == id).Select(x => x.PostId).FirstOrDefault();
            var S_Status = _context.HR_Recruitment_Applicants.Where(x => x.Applicant_Id == id).Select(x => x.Status).FirstOrDefault();
            var S_ID = _context.HR_Recruitment_Applicants.Where(x => x.Applicant_Id == id).Select(x => x.S_Id).FirstOrDefault();

            var q = (from p in _context.HR_Recruitment_Posts
                     join s in _context.HR_Recruitment_SubmittedCVs.Where(x => x.IsDelete == false) on p.PostId equals s.PostId
                     join a in _context.HR_Recruitment_Applicants.Where(x => x.Applicant_Id == id) on s.S_Id equals a.S_Id
                     select new ListVM
                     {
                         App_Id = a.Applicant_Id,
                         PostTitle = p.PostTitle,
                         App_Name = s.SubmittedCV_Name,
                         ExamResult = a.ExamResult,
                         VivaResult = a.VivaResult,
                         Status = a.Status
                     }).FirstOrDefault();
            if (q == null)
            {
                return NotFound();
            }
            ViewBag.id = id;
            ViewBag.P_Id = P_Id;
            ViewBag.S_Status = S_Status;
            ViewBag.S_ID = S_ID;
            return View(q);
        }

        // POST: EvaluationController/Edit/5
        [HttpPost]
        public ActionResult EvaluationEdit(int id, HR_Recruitment_Applicant applicant)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            try
                {
                    if (applicant.VivaResult > 0)
                    {
                    applicant.ComId = comid;
                    applicant.UserId = userid;
                    applicant.Applicant_Id = id;
                        applicant.Status = 3;
                    }

                    _context.Update(applicant);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!PostExists(post.PostId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
            
             return RedirectToAction(nameof(EvaluationIndex));
        }

        public IActionResult EvaluationDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var app = _context.HR_Recruitment_Applicants.Where(x => x.Applicant_Id == id).FirstOrDefault();
            var dataPostName = _context.HR_Recruitment_Posts.Where(x => x.PostId == app.PostId).Select(x => x.PostTitle).FirstOrDefault();
            var dataSubmitName = _context.HR_Recruitment_SubmittedCVs.Where(x => x.S_Id == app.S_Id).Select(x => x.SubmittedCV_Name).FirstOrDefault();


            ViewBag.PostName = dataPostName;
            ViewBag.SubmitName = dataSubmitName;
            if (app == null)
            {
                return NotFound();
            }

            return View(app);
        }

       
        [HttpPost]
        public async Task<IActionResult> EvaluationDeleteConfirmed(int id)
        {
            var app = await _context.HR_Recruitment_Applicants.FindAsync(id);
            _context.HR_Recruitment_Applicants.Remove(app);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EvaluationIndex));
        }
        public IActionResult QuizSet()
        {
            //var moduleList = _context.Modules.Select(ss=>ss).ToList();
            ViewData["moduleid"] = new SelectList(_context.Exam_Modules, "ModuleId", "module");




            //ViewBag.moduleList = moduleList;
            return View();
        }
        

       

        


        private int sec(int hour, int min, int sec)
        {

            int x = (hour * 3600) + (min * 60) + sec;


            return x;

        }

        [HttpPost]
        public IActionResult SaveQuiz(List<QuizSave> ques)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            try
            {
                List<Exam_Quiz> Q = new List<Exam_Quiz>();
                foreach (var q in ques)
                {
                    int time = sec(q.hoursdd, q.minutedd, q.secondsdd);
                    var existQid = _context.Exam_Quizzes.Where(w => w.Quiz_Id == q.Id).Select(s => s).FirstOrDefault();
                    if (existQid != null)
                    {

                        existQid.quiz = q.quiz;
                        existQid.passMark = q.passMark;
                        existQid.preparedBy = q.preparedBy;
                        existQid.timer = time;
                        _context.Update(existQid);
                    }
                    else
                    {
                        Exam_Quiz qu = new Exam_Quiz();
                        qu.ComId = comid;
                        qu.UserId = userid;
                        qu.ModuleId = q.ModuleId;
                        qu.quiz = q.quiz;
                        qu.passMark = q.passMark;
                        qu.preparedBy = q.preparedBy;
                        qu.timer = time;
                        Q.Add(qu);
                        _context.Exam_Quizzes.AddRange(Q);
                    }

                    var module_update = _context.Exam_Modules.Where(w => w.ModuleId == q.ModuleId).Select(s => s).FirstOrDefault();
                    if (module_update != null)
                    {

                        module_update.ComId = comid;
                        module_update.UserId = userid;
                        module_update.timer = time;
                        _context.Update(module_update);

                    }

                }
                _context.SaveChanges();


                foreach (var q in ques)
                {
                    var existQid = _context.Exam_Quizzes.Include(i => i.Answer).Where(w => w.Quiz_Id == q.Id).Select(s => s).FirstOrDefault();
                    if (existQid != null)
                    {


                        foreach (var a in existQid.Answer)
                        {
                            var existAid = _context.Exam_Answers.Where(w => w.quizid == q.Id).Select(s => s).ToList();

                            foreach (var item in existAid)
                            {
                                if (item.quizid == a.quizid)
                                {
                                    item.ans = a.ans;
                                    item.isRight = a.isRight;
                                    item.optionType = a.optionType;
                                    _context.Exam_Answers.Update(item);
                                }
                            }



                        }
                    }
                    else
                    {
                        var answer = q.Answer.ToList();
                        List<Exam_Answer> answers = new List<Exam_Answer>();
                        foreach (var a in answer)
                        {
                            Exam_Answer An = new Exam_Answer();
                            An.quizid = _context.Exam_Quizzes.Where(w => w.quiz == q.quiz && w.ModuleId == q.ModuleId).Select(s => s.Quiz_Id).FirstOrDefault();
                            An.ans = a.ans;
                            An.isRight = a.isRight;
                            An.optionType = a.optionType;

                            answers.Add(An);
                        }

                        _context.Exam_Answers.AddRange(answers);
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


        public IActionResult QuizExamPage(int id)

        {


            // ViewBag.msg = msg;
            List<quizAnsVms> qal = new List<quizAnsVms>();
            var quiz = _context.Exam_Quizzes.Where(w => w.ModuleId == id).Include(a => a.Answer).Select(s => s).ToList();
            var module_time = _context.Exam_Modules.Where(w => w.ModuleId == id).Select(s => s).FirstOrDefault();
            int hours = module_time.timer / 3600;
            int mins = (module_time.timer % 3600) / 60;
            int secs = module_time.timer % 60;
            string time = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, mins, secs);
            foreach (var q in quiz)
            {
                quizAnsVms qa = new quizAnsVms();
                qa.ModuleId = q.ModuleId;
                qa.quizId = q.Quiz_Id;
                qa.question = q.quiz;
                qa.passMark = q.passMark;
                qa.timer = module_time.timer;
                qa.time = time;
                qa.answer = (List<string>)q.Answer.Select(s => s.ans).ToList();
                qa.optionType = q.Answer.Select(s => s.optionType).FirstOrDefault();

                qal.Add(qa);
            }






            return View(qal);
        }








        
        public IActionResult EditQuiz(int moduleid)
        {
            ViewBag.ModuleName = _context.Exam_Modules.Where(w => w.ModuleId == moduleid).Select(s => s.module).FirstOrDefault();
            ViewBag.PassMark = _context.Exam_Quizzes.Where(w => w.ModuleId == moduleid).Select(s => s.passMark).FirstOrDefault();
            ViewBag.auth = _context.Exam_Quizzes.Where(w => w.ModuleId == moduleid).Select(s => s.preparedBy).FirstOrDefault();

            List<quizAnsVms> qal = new List<quizAnsVms>();
            var quiz = _context.Exam_Quizzes.Where(w => w.ModuleId == moduleid).Include(a => a.Answer).Select(s => s).ToList();
            foreach (var q in quiz)
            {
                quizAnsVms qa = new quizAnsVms();
                qa.ModuleId = q.ModuleId;
                qa.quizId = q.Quiz_Id;
                qa.question = q.quiz;
                qa.preparedBy = q.preparedBy;
                qa.AnsTable = q.Answer.Select(s => s).ToList();
                qa.optionType = q.Answer.Select(s => s.optionType).FirstOrDefault();

                qal.Add(qa);
            }

            return View(qal);
        }

        public IActionResult DeleteQuiz(int moduleid)
        {

            var quiz = _context.Exam_Quizzes.Where(w => w.ModuleId == moduleid).Include(a => a.Answer).Select(s => s).ToList();

            foreach (var q in quiz)
            {
                foreach (var item in q.Answer)
                {
                    _context.Exam_Answers.Remove(item);
                }
                _context.Exam_Quizzes.Remove(q);
            }
            _context.SaveChanges();
            return RedirectToAction("Quizlist");
        }




        [HttpPost]
        public IActionResult DeleteQues(int quesNo)
        {

            var quiz = _context.Exam_Quizzes.Where(w => w.Quiz_Id == quesNo).Include(a => a.Answer).Select(s => s).ToList();

            foreach (var q in quiz)
            {
                foreach (var item in q.Answer)
                {
                    _context.Exam_Answers.Remove(item);
                }
                _context.Exam_Quizzes.Remove(q);
            }
            _context.SaveChanges();
            return Json(new { res = true });
        }








        






        public void Savemarks(int applicantid, double finalScore)
        {

            // var data = _context.Applicants.Where(x => x.App_Id == applicantid).FirstOrDefault();


            var data = _context.HR_Recruitment_Applicants.Where(w => w.Applicant_Id == applicantid).Select(s => s).FirstOrDefault();

            //Applicant ap = new Applicant();
            //ap.App_Id = data.App_Id;
            //ap.ExamResult = finalScore;
            //ap.VivaResult = 0;
            //ap.Status = 2;
            //ap.S_Id = data.S_Id;
            //ap.PostId = data.PostId;
            data.ExamResult = finalScore;
            data.Status = 2;


            _context.Update(data);
            _context.SaveChanges();



        }

        [HttpPost]
        public IActionResult CheckQuiz(List<quizAnsVms> QuizAns)
        {

            double score = 0;
            int ModuleId = 0;
            int PassMark = 0;
            int applicantid = 0;
            
            foreach (var qa in QuizAns)
            {
                ModuleId = qa.ModuleId;
                PassMark = qa.passMark;
                applicantid = qa.applicantid;
                var result = _context.Exam_Answers.Where(w => w.quizid == qa.quizId && w.isRight == true).Select(s => s.ans).ToList();




                foreach (var ans in result)
                {


                    if (qa.answer != null)
                    {

                        if (ans == qa.answer.FirstOrDefault())
                        {
                            score += 1;
                        }


                    }
                    else
                    {

                        score += 0;


                    }




                }
            }

            double CountQues = QuizAns.Count();
            var finalScore = ((score / CountQues) * 100);
            if (finalScore >= PassMark)
            {

                Savemarks(applicantid, finalScore);

                //var data = _context.Applicants.Where(x => x.App_Id == applicantid).AsNoTracking();

                //Applicant ap = new Applicant();
                //ap.App_Id = applicantid;
                //ap.ExamResult = finalScore;
                //ap.VivaResult = 0;
                //ap.Status = 2;


                //_context.Update(ap);
                //_context.SaveChanges();







                //goodluckpage


                return RedirectToAction(nameof(goodluckpage));


            };
            return RedirectToAction(nameof(goodluckpage));
        }






        public IActionResult quizList()
        {


            List<quizAnsVms> qal = new List<quizAnsVms>();
            var dbconfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
            string sql = $"exec HR_RecruitmentGetQuizList ";
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        quizAnsVms qa = new quizAnsVms();

                        qa.ModuleId = Convert.ToInt32(dataReader["ModuleId"].ToString());
                        qa.ModuleName = dataReader["module"].ToString();
                        qa.totalques = Convert.ToInt32(dataReader["TotalQuiz"].ToString());
                        
                        qa.preparedBy = dataReader["preparedBy"].ToString() == null ? "" : dataReader["preparedBy"].ToString();
                        qa.passMark = Convert.ToInt32(dataReader["passMark"].ToString());
                        qa.timer = Convert.ToInt32(dataReader["Timer"].ToString());
                        qal.Add(qa);

                    }
                }
            }
            return View(qal);


        }

    }
}
