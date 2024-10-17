using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using GTERP.Interfaces.HR_Report;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GTERP.Controllers.HR.HrReport
{
   
    public class HRReportController : Controller
    {
        private readonly GTRDBContext db;
        private readonly IAllHRReportRepository 
            _allHRReportRepository;

        public HRReportController(
            GTRDBContext context, 
            IAllHRReportRepository allHRReportRepository
            )
        {
            db = context;
            _allHRReportRepository = allHRReportRepository;
        }
        #region All Letter fill
        public void OtsheetFill()
        {


            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = db.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 2 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2 || a.EmpTypeId == 3)).ToList();
            }

            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 2)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2)).ToList();

            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 1 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 3)).ToList();
            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && (x.EmpTypeId == 2 || x.EmpTypeId == 3)).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 2 || a.EmpTypeId == 3)).ToList();
            }

            else if (approvetype.Contains(1186))
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && x.EmpTypeId == 1).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 1).ToList();
            }
            else if (approvetype.Contains(1187))
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && x.EmpTypeId == 2).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 2).ToList();
            }
            else if (approvetype.Contains(1257))
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && x.EmpTypeId == 3).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 3).ToList();
            }
            else
            {
                ViewBag.Employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete ).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpType = db.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
                ViewBag.allp = 1;
            }

        }
        #endregion
        #region Daily Attendance Report
        public ActionResult DailyAttendanceReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");
            

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.Date = DateTime.Now;
            ViewBag.ShiftList = db.Cat_Shift.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            

            //new report permission code
            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

            //var permission = db.ReportPermissions
            //    .Include(x => x.hr_reporttype)
            //    .Where(x => x.hr_reporttype.ReportType == "Daily Attendance Report"
            //    && x.UserId == userid
            //    ).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Daily Attendance Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Daily Attendance Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);



            //var permission2 = db.Version_Report_Details.Include(x => x.ReportTypes).Where(x => x.ReportTypes.ReportType == "Daily Attendance Report" && x.VersionReportPermissionMasters.VersionId==2).OrderBy(x => x.ReportTypes.ReportName).ToList();

            //if (permission2 != null)
            //{
            //    var reports = new List<HR_ReportType>();
            //    foreach (var item in permission2)
            //    {
            //        var report = db.HR_ReportType.Where(r => r.ReportType == "Daily Attendance Report" && r.ReportId == item.ReportId).FirstOrDefault();
            //        if (report != null)
            //        {
            //            reports.Add(report);
            //        }
            //    }
            //    ViewBag.ReportList = reports;
            //}
            //else
            //{
            //    ViewBag.ReportList = db.HR_ReportType.Where(x => x.ReportType == "Daily Attendance Report").OrderBy(x => x.SLNo).ToList();
            //}

            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Daily Attendance Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Daily Attendance Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            //var permission = db.Version_Report_Masters.Include(x => x.Version_Report_Details).ThenInclude(x=>x.ReportTypes).Where(x => x.re == "Daily Attendance Report" && x.VersionReportMasterId == 17).OrderBy(x => x.ReportTypes.ReportId).ToList();

            if (permission != null)
                {
                    var reports = new List<HR_ReportType>();
                    foreach (var item in permission)
                    {
                        var report = db.HR_ReportType.Where(r => r.ReportType == "Daily Attendance Report" && r.ReportId == item.ReportId).FirstOrDefault();
                        if (report != null)
                        {
                            reports.Add(report);
                        }
                    }
                    ViewBag.ReportList = reports;
                }
                else
                {
                    ViewBag.ReportList = db.HR_ReportType.Where(x => x.ReportType == "Daily Attendance Report").OrderBy(x => x.SLNo).ToList();
                }


            return View();
        }

        [HttpPost]

        public ActionResult DailyAttendanceReport(DailyAttendanceVM aDailyAttendance)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            var callBackUrl = _allHRReportRepository.DailyAttendance(aDailyAttendance);
            return Redirect(callBackUrl);

        }
        #endregion

        #region Employee Report
        public ActionResult EmployeeReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            List<Cat_Department> Cat_Departments = new List<Cat_Department>();
            Cat_Departments = db.Cat_Department.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Departments = Cat_Departments;

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Include(x => x.Dept).Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_Designation> Cat_Designations = new List<Cat_Designation>();
            Cat_Designations = db.Cat_Designation.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Designations = Cat_Designations;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info.Include(d => d.Cat_Designation).Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Employee = employee;

            //List<Cat_Location> Locations = new List<Cat_Location>();
            //Locations = db.Cat_Location.Where(a => a.ComId == comid).ToList();
            //ViewBag.Location = Locations;

            List<Cat_Gender> Genders = new List<Cat_Gender>();
            Genders = db.Cat_Gender.Where(a => a.GenderId > 1).ToList();
            ViewBag.Genders = Genders;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            List<Cat_Shift> Shifts = new List<Cat_Shift>();
            Shifts = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Shifts = Shifts;

            ViewBag.LineList = db.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.FloorList = db.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSectList = db.Cat_SubSection.Include(x => x.Sect).Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Date = DateTime.Now;

            ViewBag.ReleaseType = db.Cat_Variable.Where(x => x.VarType == "ReleasedType" && !x.IsDelete).ToList();

            ViewBag.UnitList = db.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //var permission = db.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Employee Report" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
                //SqlParameter[] parameter = new SqlParameter[3];
                //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
                //parameter[1] = new SqlParameter("@VersionId", versionId);
                //parameter[2] = new SqlParameter("@ReportType", "Employee Report");


                //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Employee Report'";

                //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Employee Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Employee Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = db.HR_ReportType.Where(r => r.ReportType == "Employee Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = db.HR_ReportType.Where(a => a.ReportType == "Employee Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = PFreport;
            }

            return View();
        }

        [HttpPost]

        public ActionResult EmployeeReport(EmployeeReportVM aEmployeeReport)
        {
            string comid = HttpContext.Session.GetString("comid");

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            string callBackUrl = _allHRReportRepository.EmployeeReport(aEmployeeReport);
            return Redirect(callBackUrl);

        }
        #endregion

        #region Increment Report 
        public ActionResult IncrementReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            //List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            //employee = db.HR_Emp_Info
            //    .Include(d => d.Cat_Designation)
            //    .Where(x => x.ComId == comid && !x.IsDelete)
            //    .OrderBy(o => o.EmpCode)
            //    .ToList();
            //ViewBag.Employee = employee;
            OtsheetFill();
            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            //List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            //EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            //ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            //var permission = db.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Monthly Attendance" && x.UserId == userid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

                //SqlParameter[] parameter = new SqlParameter[3];
                //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
                //parameter[1] = new SqlParameter("@VersionId", versionId);
                //parameter[2] = new SqlParameter("@ReportType", "Increment Report");


                //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Increment Report'";

                //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);

            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Increment Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Increment Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);


            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = db.HR_ReportType.Where(r => r.ReportType == "Increment Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = db.HR_ReportType.Where(a => a.ReportType == "Increment Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = PFreport;
            }
           
            ViewBag.DateFrom = DateTime.Now;
            ViewBag.DateTo = DateTime.Now;

            return View();
        }

        [HttpPost]

        public ActionResult IncrementReport(IncrementReportVM aIncrementReport)
        {

            string comid = HttpContext.Session.GetString("comid");

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            string callBackUrl = _allHRReportRepository.IncrementReport(aIncrementReport);
            return Redirect(callBackUrl);

        }
        #endregion

        #region Job Card
        public IActionResult JobCard()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            // for default 26/25 date  

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var salaryMonth = db.Cat_SalaryMonths.Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();
            if (salaryMonth != null)
            {
                var month = DateTime.Now.AddMonths(-1);
                var day = salaryMonth.DtFrom;
                var year = DateTime.Now.Year;
                var fromdate = new DateTime(year, month.Month, day);
                ViewBag.Start = fromdate;

                var monthto = DateTime.Now.Month;
                var dayto = salaryMonth.DtTo;
                var todate = new DateTime(year, monthto, dayto);
                ViewBag.End = todate;

                ViewBag.dayfrom = day;
                ViewBag.dayto = dayto;
            }

            ViewBag.salarymonth = salaryMonth;

            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var endDate = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.DateFrom = firstDayOfMonth;
            ViewBag.DateTo = endDate;

            return View();
        }

        [HttpPost]
        public ActionResult JobCard(JobCardVM jobCard)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }
            string callBackUrl = _allHRReportRepository.JobCard(jobCard);
            return Redirect(callBackUrl);

        }

        #endregion

        #region Job Card Buyer
        public IActionResult JobCardB()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var salaryMonth = db.Cat_SalaryMonths.Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();
            if (salaryMonth != null)
            {
                var month = DateTime.Now.AddMonths(-1);
                var day = salaryMonth.DtFrom;
                var year = DateTime.Now.Year;
                var fromdate = new DateTime(year, month.Month, day);
                ViewBag.Start = fromdate;

                var monthto = DateTime.Now.Month;
                var dayto = salaryMonth.DtTo;
                var todate = new DateTime(year, monthto, dayto);
                ViewBag.End = todate;

                ViewBag.dayfrom = day;
                ViewBag.dayto = dayto;
            }

            ViewBag.salarymonth = salaryMonth;

            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var endDate = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.DateFrom = firstDayOfMonth;
            ViewBag.DateTo = endDate;

            return View();
        }

        [HttpPost]
        public ActionResult JobCardB(JobCardVM JobCardB)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            var callBackUrl = _allHRReportRepository.JobCardB(JobCardB);
            return Redirect(callBackUrl);
        }
        #endregion

        #region Job Card 4 hour
        public IActionResult JobCard4h()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var salaryMonth = db.Cat_SalaryMonths.Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();
            if (salaryMonth != null)
            {
                var month = DateTime.Now.AddMonths(-1);
                var day = salaryMonth.DtFrom;
                var year = DateTime.Now.Year;
                var fromdate = new DateTime(year, month.Month, day);
                ViewBag.Start = fromdate;

                var monthto = DateTime.Now.Month;
                var dayto = salaryMonth.DtTo;
                var todate = new DateTime(year, monthto, dayto);
                ViewBag.End = todate;

                ViewBag.dayfrom = day;
                ViewBag.dayto = dayto;
            }

            ViewBag.salarymonth = salaryMonth;

            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var endDate = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.DateFrom = firstDayOfMonth;
            ViewBag.DateTo = endDate;

            return View();
        }

        [HttpPost]
        public ActionResult JobCard4h(JobCardVM job4h)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            var callBackUrl = _allHRReportRepository.JobCard4h(job4h);
            return Redirect(callBackUrl);
        }

        #endregion

        #region Dynamic Job Card

        public IActionResult DynamicJobCard()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var salaryMonth = db.Cat_SalaryMonths.Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();
            if (salaryMonth != null)
            {
                var month = DateTime.Now.AddMonths(-1);
                var day = salaryMonth.DtFrom;
                var year = DateTime.Now.Year;
                var fromdate = new DateTime(year, month.Month, day);
                ViewBag.Start = fromdate;

                var monthto = DateTime.Now.Month;
                var dayto = salaryMonth.DtTo;
                var todate = new DateTime(year, monthto, dayto);
                ViewBag.End = todate;

                ViewBag.dayfrom = day;
                ViewBag.dayto = dayto;
            }

            ViewBag.salarymonth = salaryMonth;

            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var endDate = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.DateFrom = firstDayOfMonth;
            ViewBag.DateTo = endDate;

            return View();
        }

        [HttpPost]
        public ActionResult DynamicJobCard(JobCardVM dyjobcard)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            var callBackUrl = _allHRReportRepository.DynamicJobCard(dyjobcard);
            return Redirect(callBackUrl);
        }

        #endregion

        #region Leave Report
        public ActionResult LeaveReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LeaveTypeId = db.Cat_Leave_Type.ToList();

            #region Commented Code

            //List<HR_ReportType> reports = db.HR_ReportType.Where(a => a.ComId == comid && a.ReportType=="Leave Report").OrderBy(a=>a.SLNo).ToList();
            //ViewBag.ReportTypes = reports;

            //List<string> Reporttype = PayrollRepository.ReportName();
            //var permission = db.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Leave Report" && x.UserId == userid).OrderBy(x => x.hr_reporttype.SLNo).ToList();
            //var permission = db.ReportPermissions
            //    .Include(x => x.hr_reporttype)
            //    .Where(x => x.hr_reporttype.ReportType == "Leave Report"
            //    && x.UserId == userid
            //    ).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Leave Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Leave Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);


            #endregion

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Leave Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Leave Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);
            if (permission != null)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = db.HR_ReportType.Where(r => r.ReportType == "Leave Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }
                }
                ViewBag.ReportList = reports;
            }
            else
            {
                ViewBag.ReportList = db.HR_ReportType.Where(x => x.ReportType == "Leave Report").OrderBy(x => x.SLNo).ToList();
            }
            ViewBag.DateFrom = DateTime.Now;
            ViewBag.DateTo = DateTime.Now;
            return View();
        }

        [HttpPost]

        public ActionResult LeaveReport(LeaveReportVM aLeaveReport)
        {

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            var callBackUrl = _allHRReportRepository.LeaveReport(aLeaveReport);
            return Redirect(callBackUrl);
        }

        #endregion

        #region Loan Report
        public ActionResult LoanReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Employee = employee;

            List<Cat_Location> Locations = new List<Cat_Location>();
            Locations = db.Cat_Location.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Location = Locations;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            List<Cat_Shift> Shifts = new List<Cat_Shift>();
            Shifts = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.Shifts = Shifts;

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Loan Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Loan Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);

            //List<HR_ReportType> reports = new List<HR_ReportType>();
            //reports = db.HR_ReportType.Where(a => a.ReportType == "Loan Report") //a.ComId == comid && 
            //    .OrderBy(v => v.SLNo).ToList();
            //ViewBag.ReportTypes = reports;

            //List<HR_ReportType> reports = db.HR_ReportType.Where(a => a.ComId == comid && a.ReportType=="Employee Report").OrderBy(a=>a.SLNo).ToList();
            //ViewBag.ReportTypes = reports;


            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Loan Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Loan Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = db.HR_ReportType.Where(r => r.ReportType == "Loan Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }
                }
                ViewBag.ReportList = reports;
            }
            else
            {
                List<HR_ReportType> PFreport = db.HR_ReportType.Where(a => a.ReportType == "Loan Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = PFreport;
            }
           
            return View();
        }

        [HttpPost]

        public ActionResult LoanReport(LoanReportVM aLoanReport)
        {
            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }

            var callBackUrl = _allHRReportRepository.LoanReport(aLoanReport);
            return Redirect(callBackUrl);
        }
        #endregion

        #region Monthly Attendance Report
        public ActionResult MonthlyAttendanceReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }
            List<Cat_Variable> EmpStatus = new List<Cat_Variable>();
            EmpStatus = db.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();
            ViewBag.EmpStatusList = EmpStatus;

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();

            //var permission = db.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Monthly Attendance" && x.UserId == userid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            //for period wise date range
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

                //SqlParameter[] parameter = new SqlParameter[3];
                //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
                //parameter[1] = new SqlParameter("@VersionId", versionId);
                //parameter[2] = new SqlParameter("@ReportType", "Monthly Attendance");


                //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Monthly Attendance'";

                //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);

            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Monthly Attendance");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Monthly Attendance'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = db.HR_ReportType.Where(r => r.ReportType == "Monthly Attendance" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = db.HR_ReportType.Where(a => a.ReportType == "Monthly Attendance").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = PFreport;
            }
            var salaryMonth = db.Cat_SalaryMonths.Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();
            if (salaryMonth != null)
            {
                var month = DateTime.Now.AddMonths(-1);
                var day = salaryMonth.DtFrom;
                var year = DateTime.Now.Year;
                var fromdate = new DateTime(year, month.Month, day);
                ViewBag.Start = fromdate;

                var monthto = DateTime.Now.Month;
                var dayto = salaryMonth.DtTo;
                var todate = new DateTime(year, monthto, dayto);
                ViewBag.End = todate;

                ViewBag.dayfrom = day;
                ViewBag.dayto = dayto;
            }

            ViewBag.salarymonth = salaryMonth;

            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var endDate = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.DateFrom = firstDayOfMonth;
            ViewBag.DateTo = endDate;

            return View();
        }

        [HttpPost]

        public ActionResult MonthlyAttendanceReport(MonthlyAttendanceVM aMonthlyAttendance)
        {
            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("default");
            }
            string callBackUrl = _allHRReportRepository.MonthlyAttendance(aMonthlyAttendance);
            return Redirect(callBackUrl);

        }


        #endregion

        #region Prod Report
        public IActionResult ProdReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            if (userid == null)
            {
                return RedirectToRoute("GTR");
            }

            List<Cat_Section> Cat_Sections = new List<Cat_Section>();
            Cat_Sections = db.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Sections = Cat_Sections;

            List<Cat_SubSection> SubSection = new List<Cat_SubSection>();
            SubSection = db.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSection = SubSection;

            List<Cat_Designation> Designation = new List<Cat_Designation>();
            Designation = db.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Designaiton = Designation;

            List<Cat_Department> DepartmentList = new List<Cat_Department>();
            DepartmentList = db.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.Department = DepartmentList;

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
            employee = db.HR_Emp_Info
                .Include(d => d.Cat_Designation)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .OrderBy(o => o.EmpCode)
                .ToList();
            ViewBag.Employee = employee;

            List<Cat_Floor> Floors = new List<Cat_Floor>();
            Floors = db.Cat_Floor.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.FloorList = Floors;

            List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            ViewBag.EmpType = EmpTypes;

            ViewBag.UnitId = db.Cat_Unit.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.LineId = db.Cat_Line.Where(a => a.ComId == comid && !a.IsDelete).ToList();


            ViewBag.ShiftList = db.Cat_Shift.Where(a => a.ComId == comid && !a.IsDelete).ToList();
            ViewBag.StyleId = db.Cat_Style.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            //var permission = db.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Production Report" && x.UserId == userid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

                //SqlParameter[] parameter = new SqlParameter[3];
                //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
                //parameter[1] = new SqlParameter("@VersionId", versionId);
                //parameter[2] = new SqlParameter("@ReportType", "Production Report");


                //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Production Report'";

                //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);

            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Production Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Production Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = db.HR_ReportType.Where(r => r.ReportType == "Production Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports;
            }
            else
            {

                List<HR_ReportType> PFreport = db.HR_ReportType.Where(a => a.ReportType == "Production Report").OrderBy(a => a.SLNo).ToList();

                ViewBag.ReportList = PFreport;
            }
            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var endDate = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.DateFrom = firstDayOfMonth;
            ViewBag.DateTo = endDate;
            return View();
        }
        [HttpPost]
        public ActionResult ProdReport(ProdVM prod)
        {
            string comid = HttpContext.Session.GetString("comid");

            if (HttpContext.Session.GetString("userid") == null)
            {
                return RedirectToRoute("GTR");
            }
            string callBackUrl = _allHRReportRepository.ProdReport(prod);
            return Redirect(callBackUrl);


        }
        #endregion

        #region JobCardProcess

        public IActionResult JobCardProcessList()
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = db.Cat_JobCardDynamic.Where(x => x.ComId == comid && x.IsDelete == false).ToList();
            return View(data);
        }
        public IActionResult CreateJobCardProcess()
        {
            ViewBag.Title = "Create";
            List<SelectListItem> options = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Actual Job Card", Text = "Actual Job Card" },
                    new SelectListItem { Value = "Dynamic Job Card", Text = "Dynamic Job Card" },
                    new SelectListItem { Value = "Compliance Job Card", Text = "Compliance Job Card" }
                };
             ViewBag.Type = options;
             return View();
        }
        [HttpPost]
        public IActionResult CreateJobCardProcess(Cat_JobCardDynamic model)
        {
            ViewBag.Title = "Create"; 
            var comid = HttpContext.Session.GetString("comid");
            model.ComId = comid;
            List<SelectListItem> options = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Actual Job Card", Text = "Actual Job Card" },
                    new SelectListItem { Value = "Dynamic Job Card", Text = "Dynamic Job Card" },
                    new SelectListItem { Value = "Compliance Job Card", Text = "Compliance Job Card" }
                };
            ViewBag.Type = options;
            if (ModelState.IsValid)
            {
                if (model.JcdId > 0)
                {
                    model.Dateupdated = DateTime.Now;
                    model.UpdateByUserId = HttpContext.Session.GetString("userid");
                    db.Cat_JobCardDynamic.Update(model);
                }
                else
                {
                    model.DateAdded = DateTime.Now;
                    db.Cat_JobCardDynamic.Add(model);
                }

                db.SaveChanges();
                return RedirectToAction("JobCardProcessList", "HRReport");
            }
            return View(model);
            
           
            
            
        }
        public IActionResult EditJobCardProcess(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            var data = db.Cat_JobCardDynamic.Where(x=>x.JcdId == id && x.ComId == comid).FirstOrDefault();

            List<SelectListItem> options = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Actual Job Card", Text = "Actual Job Card" },
                    new SelectListItem { Value = "Dynamic Job Card", Text = "Dynamic Job Card" },
                    new SelectListItem { Value = "Compliance Job Card", Text = "Compliance Job Card" }
                };
            ViewBag.Type = options;
            if (data == null)
            {
                return NotFound();
            }


            return View("CreateJobCardProcess", data);

        }
        public IActionResult DeleteJobCardProcess(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            List<SelectListItem> options = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Actual Job Card", Text = "Actual Job Card" },
                    new SelectListItem { Value = "Dynamic Job Card", Text = "Dynamic Job Card" },
                    new SelectListItem { Value = "Compliance Job Card", Text = "Compliance Job Card" }
                };
            ViewBag.Type = options;
            var comid = HttpContext.Session.GetString("comid");
            var data = db.Cat_JobCardDynamic.Where(x => x.JcdId == id && x.ComId == comid).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";


            return View("CreateJobCardProcess", data);

        }

        // POST: ProcessLock/Delete/5
        [HttpPost, ActionName("DeleteJobCardProcess")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteJobCardProcessConfirmed(int id)
        {

            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var data = db.Cat_JobCardDynamic.Where(x => x.JcdId == id && x.ComId == comid).FirstOrDefault();
                data.IsDelete = true;
                db.Cat_JobCardDynamic.Update(data);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = data.JcdId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        //jobcard tabulator report
        public IActionResult JobCardReport()
        {
            List<string> myList = new List<string> { "This Month", "This Week", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            return View();
        }


        [HttpGet]
        public JsonResult JobCardReportData(int? EmpId, DateTime dtFrom, DateTime dtTo)
        {

            try

            {
                string comid = HttpContext.Session.GetString("comid");
                string FloorId = "";
                string Type = "";
                if (EmpId == null)
                {
                    EmpId = 0;
                }
                //else
                //{
                //    EmpId = EmpId.ToString();
                //}

                SqlParameter[] parameters = new SqlParameter[16];
                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@dtFrom", dtFrom);
                parameters[2] = new SqlParameter("@dtTo", dtTo);
                parameters[3] = new SqlParameter("@EmpId", EmpId.ToString());
                parameters[4] = new SqlParameter("@ShiftId", 0);
                parameters[5] = new SqlParameter("@DesigId", 0);
                parameters[6] = new SqlParameter("@DeptId", 0);
                parameters[7] = new SqlParameter("@SectId", 0);
                parameters[8] = new SqlParameter("@SubSectId", 0);
                parameters[9] = new SqlParameter("@EmpTypeId", 0);
                parameters[10] = new SqlParameter("@LineId", 0);
                parameters[11] = new SqlParameter("@UnitId", 0);

                parameters[12] = new SqlParameter("@FloorId", FloorId);
                parameters[13] = new SqlParameter("@Type", Type);
                parameters[14] = new SqlParameter("@optCriteria", "=All=");
                parameters[15] = new SqlParameter("@EmpStatus", "=ALL=");

                var datasetabc = Helper.ExecProcMapTList<EmployeeReportModel>("HR_rptAttendanceView", parameters);
                string query = $"Exec HR_rptAttendanceView '{comid}','{dtFrom}','{dtTo}','{EmpId}','{0}','{0}','{0}','{0}','{0}','{0}','{0}','{0},'{FloorId}','{Type}','{"=All="}','{"=All="}'";


                //return Json(new {data= datasetabc });
                return Json(new { Success = 1, error = false, data = datasetabc });
                // return Json(new { Success = 1, data = datasetabc, message = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message });
                // throw ex;
            }
        }

        
      public JsonResult SearchJobCardReport(string term)
      {
            string comid = HttpContext.Session.GetString("comid");
            var datasetabcs = db.HR_Emp_Info
                .Where(e => e.EmpName.Contains(term) || e.EmpCode.Contains(term) && e.ComId == comid)
                .Select(e => new { label = e.EmpCode + " " + e.EmpName, value = e.EmpId })
                .Take(10)
                .ToList();

            return new JsonResult(datasetabcs);
       }


        #endregion
    }
}
