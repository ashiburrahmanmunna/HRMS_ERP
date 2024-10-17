using DocumentFormat.OpenXml.Bibliography;
using GTERP.BLL;
using GTERP.Interfaces.Letter;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Letter;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static StandaloneSDKDemo.SDKHelper;

namespace GTERP.Controllers.Letter
{
    public class LetterReportController : Controller
    {
        #region Common Property
        private readonly GTRDBContext _context;
        private readonly IAbsentLetterRepository _absentLetterRepository;
        public PayrollRepository PayrollRepository { get; set; }
        #endregion

        #region Constructor
        public LetterReportController(
            GTRDBContext context,
            PayrollRepository payrollRepos,
            IAbsentLetterRepository absentLetterRepository
            )
        {
            _context = context;
            //Repository = repository;
            PayrollRepository = payrollRepos;
            _absentLetterRepository = absentLetterRepository;
        }

        #endregion


        #region All Letter fill
        public void OtsheetFill()
        {


            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 2 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2 || a.EmpTypeId == 3)).ToList();
            }

            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 2)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2)).ToList();

            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 3)).ToList();
            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 2 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 2 || a.EmpTypeId == 3)).ToList();
            }

            else if (approvetype.Contains(1186))
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && x.EmpTypeId == 1).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 1).ToList();
            }
            else if (approvetype.Contains(1187))
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && x.EmpTypeId == 2).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 2).ToList();
            }
            else if (approvetype.Contains(1257))
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && x.EmpTypeId == 3).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 3).ToList();
            }
            else
            {
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 2 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
                ViewBag.allp = 1;
            }

        }
        #endregion


        #region All Letter
        public async Task<IActionResult> AllLetter()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            //OtsheetFill();
            ViewBag.UnitList = _context.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus").ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).OrderBy(o => o.EmpCode).Where(a => a.ComId == comid && !a.IsDelete).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Date = DateTime.Now;

            //ViewBag.ReportList = permission;

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;


            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Letter Report" && x.UserId == userid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Letter Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Letter Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Letter Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Letter Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission != null)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Letter Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }
                }
                ViewBag.ReportList = reports;
            }
            else
            {
                ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Letter Report").OrderBy(x => x.SLNo).ToList();
            }


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AllLetter(AllLetter AllLettermodel)
        {
            var callBackUrl = _absentLetterRepository.AllLetter(AllLettermodel);
            return Redirect(callBackUrl);
        }
        #endregion

        #region Absent Letter
        public IActionResult AbsentLetterList()
        {

            string comid = HttpContext.Session.GetString("comid");
            ViewBag.AbsentLetterVM = new List<AbsentLetterVM>();
            SqlParameter p1 = new SqlParameter("@ComId", comid);
            var data = Helper.ExecProcMapTList<AbsentLetterVM>("dbo.Hr_prcGetAbsLetter", new SqlParameter[] { p1 });
            return View(data);

        }
        public IActionResult GetFilteredEmployee(string keyword)
        {
            string comid = HttpContext.Session.GetString("comid");
            
            
            // Perform case-insensitive filtering based on employee names or other relevant properties
            keyword = keyword.ToLower();

            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                           join sh in _context.Cat_Shift on emp.ShiftId equals sh.ShiftId
                           //join EmpRel in _context.HR_Emp_Released on emp.EmpId equals EmpRel.EmpId

                           where emp.ComId == comid && (emp.EmpName.Contains(keyword) || emp.EmpCode.Contains(keyword))
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ] - [ " + sh.ShiftName + " ] "
                           }).Take(10).ToList();



            return Json(empInfo);
        }
        public IActionResult CreateAbsentLetter()
        {
            ViewBag.Title = "Create";
            string comid = HttpContext.Session.GetString("comid");
            //ViewBag.list = _context.HR_Emp_Info.ToList();
           
            return View();
        }



        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAbsentLetter(Hr_Emp_AbsentLetter absentLetter)
        {
            if (ModelState.IsValid)
            {

                absentLetter.ComId = HttpContext.Session.GetString("comid");
                if (absentLetter.RefId > 0)
                {

                    _context.Entry(absentLetter).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";

                }
                else
                {
                    _context.Add(absentLetter);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";

                }
                return RedirectToAction(nameof(AbsentLetterList));
            }
            return View(absentLetter);
        }

        public async Task<IActionResult> EditAbsentLetter(int? id)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";


            var absentLetter = await _context.Hr_Emp_AbsentLetter.FindAsync(id);
            ViewBag.EmpId = new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == absentLetter.EmpId), "EmpId", "EmpName", absentLetter.EmpId);

            if (absentLetter == null)
            {
                return NotFound();
            }
            return View("CreateAbsentLetter", absentLetter);
        }


        public async Task<IActionResult> DeleteAbsentLetter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absentLetter = await _context.Hr_Emp_AbsentLetter

              .FirstOrDefaultAsync(m => m.RefId == id);
            if (absentLetter == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            return View("CreateAbsentLetter", absentLetter);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteAbsentLetter")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAbsentLetterConfirmed(int id)
        {
            try
            {
                var absentLetter = await _context.Hr_Emp_AbsentLetter.FindAsync(id);
                _context.Hr_Emp_AbsentLetter.Remove(absentLetter);
                _context.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, SectId = absentLetter.RefId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        private bool Cat_SectionExists(int id)
        {
            return _context.Hr_Emp_AbsentLetter.Any(e => e.RefId == id);
        }

        public ActionResult Print(int? id, string letterType, string type = "pdf")
        {
            string callBackUrl = _absentLetterRepository.Print(id, letterType, type);
            return Redirect(callBackUrl);
        }
        #endregion
    }
}
