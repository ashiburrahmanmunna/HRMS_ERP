using GTERP.BLL;
using GTERP.Interfaces.HRVariables;
using GTERP.Interfaces.Payroll_Report;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Payroll;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.Payroll.PayrollReport
{
    public class PayrollReportController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly IPayrollReportRepository _PayrollReportRepository;
        private readonly IBankRepository _bankRepository;
        public PayrollReportController(
            GTRDBContext context,
            IPayrollReportRepository payrollReportRepository,
             PayrollRepository payrollRepos,
             IBankRepository bankRepository
            )
        {
            _context = context;
            _PayrollReportRepository = payrollReportRepository;
            PayrollRepository = payrollRepos;
            _bankRepository = bankRepository; ;
        }
        public PayrollRepository PayrollRepository { get; set; }

        #region Salary Report
        public async Task<IActionResult> SalaryReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            reportempFill();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Include(x => x.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete && !x.IsInactive == true).ToList();
            //Where(x => x.ComId == comid && x.IsDelete == false && x.IsInactive == false && x.IsApprove == true).ToList();
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();




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
            sqlParameter[5] = new SqlParameter("@ReportType", "Salary Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Salary Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Salary Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports;
            }
            else
            {
                List<HR_ReportType> Salaryreport = _context.HR_ReportType.Where(a => a.ReportType == "Salary Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = Salaryreport;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalaryReport(SalarySheet SalarySheetmodel)
        {

            var callBackUrl = _PayrollReportRepository.SalarySheet(SalarySheetmodel);
            return Redirect(callBackUrl);

        }
        #endregion

        #region PF Report
        public async Task<IActionResult> PFReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            reportempFill();

            var empsalary = await _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true).ToListAsync();

            ViewBag.AllEmpApprovetype = empsalary;
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetPFProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete).Include(d => d.Cat_Designation).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Salary Report" && x.ReportPath == null).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Salary Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Salary Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion 

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "PF Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'PF Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "PF Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports;
            }
            else
            {
                List<HR_ReportType> PFreport = _context.HR_ReportType.Where(a => a.ReportType == "PF Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = PFreport;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PFReport(PFSheet PFSheetmodel)
        {

            var callBackUrl = _PayrollReportRepository.PFSheet(PFSheetmodel);
            return Redirect(callBackUrl);

        }









        #endregion

        #region Casual Salary Report
        public async Task<IActionResult> CasualSalaryReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.CasualProcessTypeList = PayrollRepository.GetCasualProssTypes();
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();



            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete).Include(d => d.Cat_Designation).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Salary Report" && x.ReportPath == null).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Salary Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Salary Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion 

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Casual Salary Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Salary Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Casual Salary Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports;
            }
            else
            {
                List<HR_ReportType> Salaryreport = _context.HR_ReportType.Where(a => a.ReportType == "Casual Salary Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = Salaryreport;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CasualSalaryReport(SalarySheet CasualSalarySheetmodel)
        {

            var callBackUrl = _PayrollReportRepository.CasualSalarySheet(CasualSalarySheetmodel);
            return Redirect(callBackUrl);

        }



        #endregion

        #region Salary employ table fill
        public void reportempFill()
        {


            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                var empsalaryByAT = _context.HR_Emp_Salary
                  .Include(x => x.HR_Emp_Info)
                  .Include(x => x.HR_Emp_Info.Cat_Designation)
                  .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                  .Where(x => x.ComId == comid && !x.IsDelete ).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete ).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2 || a.EmpTypeId == 3)).ToList();

            }

            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {

                var empsalaryByAT = _context.HR_Emp_Salary
                 .Include(x => x.HR_Emp_Info)
                 .Include(x => x.HR_Emp_Info.Cat_Designation)
                 .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                 .Where(x => x.ComId == comid && !x.IsDelete  && (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 2)).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 2)).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2)).ToList();

            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {

                var empsalaryByAT = _context.HR_Emp_Salary
                        .Include(x => x.HR_Emp_Info)
                        .Include(x => x.HR_Emp_Info.Cat_Designation)
                        .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                        .Where(x => x.ComId == comid && !x.IsDelete && (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 3)).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 3)).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;

                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 3)).ToList();
            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                var empsalaryByAT = _context.HR_Emp_Salary
                 .Include(x => x.HR_Emp_Info)
                 .Include(x => x.HR_Emp_Info.Cat_Designation)
                 .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                 .Where(x => x.ComId == comid && !x.IsDelete && (x.HR_Emp_Info.EmpTypeId == 2 || x.HR_Emp_Info.EmpTypeId == 3)).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && (x.HR_Emp_Info.EmpTypeId == 2 || x.HR_Emp_Info.EmpTypeId == 3)).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;

                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 2 || a.EmpTypeId == 3)).ToList();
            }

            else if (approvetype.Contains(1186))
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                 .Include(x => x.HR_Emp_Info)
                 .Include(x => x.HR_Emp_Info.Cat_Designation)
                 .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                 .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId == 1).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && x.HR_Emp_Info.EmpTypeId == 1).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 1).ToList();
            }
            else if (approvetype.Contains(1187))
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                    .Include(x => x.HR_Emp_Info)
                    .Include(x => x.HR_Emp_Info.Cat_Designation)
                    .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                    .Where(x => x.ComId == comid && !x.IsDelete && x.HR_Emp_Info.EmpTypeId == 2).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && x.HR_Emp_Info.EmpTypeId == 2).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 2).ToList();
            }
            else if (approvetype.Contains(1257))
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                 .Include(x => x.HR_Emp_Info)
                 .Include(x => x.HR_Emp_Info.Cat_Designation)
                 .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                 .Where(x => x.ComId == comid && !x.IsDelete && x.HR_Emp_Info.EmpTypeId == 1).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && x.HR_Emp_Info.EmpTypeId == 1).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 3).ToList();
            }
            else
            {

                var empsalaryByAT = _context.HR_Emp_Salary
                  .Include(x => x.HR_Emp_Info)
                  .Include(x => x.HR_Emp_Info.Cat_Designation)
                  .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                  .Where(x => x.ComId == comid && !x.IsDelete).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
                ViewBag.allp = 1;

            }

        }
        public void IsApproveFill()         
        {
            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            //Check approvetype for Worker
            var isApprove_1186 = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false && w.ApprovalType == 1186).Select(s => s.IsApprove).ToList();
            //Check approvetype for Staff
            var isApprove_1187 = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false && w.ApprovalType == 1187).Select(s => s.IsApprove).ToList();

            //If Approve for both Worker & Staff
            if (isApprove_1186.Contains(true) && isApprove_1187.Contains(true))
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                    .Include(x => x.HR_Emp_Info)
                    .Include(x => x.HR_Emp_Info.Cat_Designation)
                    .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                    .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 2)).FirstOrDefault();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && (a.EmpTypeId == 1 || a.EmpTypeId == 2)).ToList();
                ViewBag.EmpApprovetype = empsalaryByAT;
            }
            //If Approve for only Worker 
            else if (isApprove_1186.Contains(true))
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId == 1).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId == 1).FirstOrDefault();

                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 1).ToList();
                ViewBag.EmpApprovetype = empsalaryByAT;
            }
            //If Approve for only Staff
            else if (isApprove_1187.Contains(true))
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId != 1).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId != 1).FirstOrDefault();

                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete && a.EmpTypeId == 2).ToList();
                ViewBag.EmpApprovetype = empsalaryByAT;
            }
            //if Aprove==null
            else
            {
                var empsalaryByAT = _context.HR_Emp_Salary
                  .Include(x => x.HR_Emp_Info)
                  .Include(x => x.HR_Emp_Info.Cat_Designation)
                  .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                  .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true).ToList();

                ViewBag.empType = _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true).FirstOrDefault();

                ViewBag.EmpApprovetype = empsalaryByAT;
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
                ViewBag.allp = 1;
            }
        }
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
                ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.EmpCode).ToList();
                ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
                ViewBag.allp = 1;
            }

        }
        #endregion

        #region Salary Report Buyer
        public async Task<IActionResult> SalaryReportB()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.EmpCode).ToList();
            //permission wise Employee Type Data
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

            reportempFill();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LocationList = _context.Cat_Location.ToList();
            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Salary Report Buyer" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Salary Sheet Buyer");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Salary Sheet Buyer'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);

            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Salary Sheet Buyer");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Salary Sheet Buyer'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Salary Sheet Buyer" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportName = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = _context.HR_ReportType.Where(a => a.ReportType == "Salary Sheet Buyer").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportName = PFreport;
            }



            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalaryReportB(SalarySheet SalarySheetmodel)
        {
            var data = _PayrollReportRepository.SalarySheetB(SalarySheetmodel);
            return Redirect(data);
        }
        #endregion


        //Kamrul Dynamic Salary Report

        #region Dynamic Salary Report
        public async Task<IActionResult> DynamicSalaryReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.ToList();
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();


            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.EmpCode).ToList();
            //permission wise Employee Type Data
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

            reportempFill();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LocationList = _context.Cat_Location.ToList();
            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Salary Report Buyer" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code

            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Salary Sheet Buyer");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Salary Sheet Buyer'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);

            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Dynamic Salary Sheet");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Dynamic Salary Sheet'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Dynamic Salary Sheet" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportName = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = _context.HR_ReportType.Where(a => a.ReportType == "Dynamic Salary Sheet").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportName = PFreport;
            }



            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DynamicSalaryReport(SalarySheet SalarySheetmodel)
        {
            var data = _PayrollReportRepository.DynamicSalarySheet(SalarySheetmodel);
            return Redirect(data);
        }
        #endregion



        #region Extra OT Sheet

        public async Task<IActionResult> ExtraOTSheet()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            // ViewBag.EmpTypeList = await _context.Cat_Emp_Type.ToListAsync();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.EmpCode).ToList();
            //for rollwise
            OtsheetFill();
            //end
            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LocationList = _context.Cat_Location.ToList();
            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();

            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Salary Report Buyer" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Extra OT Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Extra OT Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Extra OT Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Extra OT Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Extra OT Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportName = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = _context.HR_ReportType.Where(a => a.ReportType == "Extra OT Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportName = PFreport;
            }



            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExtraOTSheet(SalarySheet SalarySheetmodel)
        {
            var data = _PayrollReportRepository.ExtraOTSheet(SalarySheetmodel);
            return Redirect(data);
        }

        #endregion

        #region Festival Bonus Report
        public async Task<IActionResult> FestBonusReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();

            //ViewBag.EmpTypeList = _context.Cat_Emp_Type.ToList();
            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus" && !x.IsDelete).ToList();
            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.FestivalTypeList= await _context.HR_Emp_Info.Where(x=>x.s).ToListAsync();

            ViewBag.FestivalTypeList = _context.Cat_Variable.Where(x => x.VarType == "FestivalType").OrderBy(x => x.SLNo).ToList();
            OtsheetFill();
            ViewBag.ProcessTypeList = PayrollRepository.GetFestBonusProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Include(x => x.Dept).Where(x => x.ComId == comid && !x.IsDelete).ToList();
            // ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.ToList();

            //ViewBag.ReportList = permission;
            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Festival Report" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Festival Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Festival Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Festival Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Festival Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Festival Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportName = reports; // db.HR_ReportType.Where(r=>r.ReportType == "Sales Report" && r.IsActive == true).OrderBy(x=>x.SLNo).ToList();
            }
            else
            {
                List<HR_ReportType> PFreport = _context.HR_ReportType.Where(a => a.ReportType == "Monthly Attendance").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportName = PFreport;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FestBonusReport(FestivalBonus FestivalBonusmodel)
        {
            string callBackUrl = _PayrollReportRepository.FestBonus(FestivalBonusmodel);
            return Redirect(callBackUrl);
        }

        #endregion

        #region Loan Report
        public async Task<IActionResult> LoanReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus").ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete).Include(d => d.Cat_Designation).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.ReportList = permission;
            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Loan Report" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

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

            if (permission != null)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Loan Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }
                }
                ViewBag.ReportList = reports;
            }
            else
            {
                ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Loan Report").OrderBy(x => x.SLNo).ToList();
            }


            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoanReport(LoanReport LoanReportmodel)
        //{

        //    string callBackUrl = _PayrollReportRepository.Loan(LoanReportmodel);
        //    return Redirect(callBackUrl);
        //}

        #endregion

        #region Earn Leave Report
        public async Task<IActionResult> EarnLeaveReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            //ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();
            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus").ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetElProssTypes();
            OtsheetFill();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete).Include(d => d.Cat_Designation).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Salary Report" && x.UserId == userid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "EarnLvSheet Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'EarnLvSheet Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "EarnLvSheet Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'EarnLvSheet Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "EarnLvSheet Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports;
            }
            else
            {
                List<HR_ReportType> Salaryreport = _context.HR_ReportType.Where(a => a.ReportType == "EarnLvSheet Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = Salaryreport;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EarnLeaveReport(Models.Payroll.EarnLeaveSheet EarnLeaveSheet)
        {

            var callBackUrl = _PayrollReportRepository.EarnLeaveSheet(EarnLeaveSheet);
            return Redirect(callBackUrl);

        }
        #endregion

        //#region Advance Salary Report
        //public ActionResult AdvSalaryReport()
        //{
        //    string comid = HttpContext.Session.GetString("comid");

        //    if (HttpContext.Session.GetString("userid") == null)
        //    {
        //        return RedirectToRoute("GTR");
        //    }

        //    List<Cat_Section> Cat_Sections = new List<Cat_Section>();
        //    Cat_Sections = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();
        //    ViewBag.Sections = Cat_Sections;

        //    List<HR_Emp_Info> employee = new List<HR_Emp_Info>();
        //    employee = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete).ToList();
        //    ViewBag.Employee = employee;

        //    List<Cat_Line> Lines = new List<Cat_Line>();
        //    Lines = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();
        //    ViewBag.EmpLine = Lines;

        //    return View();
        //}

        //[HttpPost]

        //public ActionResult AdvSalaryReport(AdvSalaryReport aAdvSalaryReport)
        //{


        //    if (HttpContext.Session.GetString("userid") == null)
        //    {
        //        return RedirectToRoute("GTR");
        //    }
        //    var callBackUrl = _PayrollReportRepository.AdvSalary(aAdvSalaryReport);
        //    return Redirect(callBackUrl);

        //}

        //#endregion

        #region Board Paper Report
        public async Task<IActionResult> BoardPaperReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus").ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Include(d => d.Cat_Designation).Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.ReportList = permission;
            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Board Paper Report" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();

            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Board Paper Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Board Paper Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Board Paper Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Board Paper Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission != null)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Board Paper Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }
                }
                ViewBag.ReportList = reports;
            }
            else
            {
                ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Board Paper Report").OrderBy(x => x.SLNo).ToList();
            }


            return View();
        }


        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult BoardPaperReport(BoardPaper BoardPapermodel)
        //{
        //    var callBackUrl = _PayrollReportRepository.BoardPaper(BoardPapermodel);
        //    return Redirect(callBackUrl);
        //}

        #endregion

        #region MGT Salary Report
        public async Task<IActionResult> MGTSalaryReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            ViewBag.EmpTypeList = _context.Cat_Emp_Type.Where(a => !a.IsDelete).ToList();

            ViewBag.UnitList = _context.Cat_Unit.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.PayModeList = _context.Cat_PayMode.ToList();

            ViewBag.EmpStatusList = _context.Cat_Variable.Where(x => x.VarType == "EmpStatus").ToList();

            ViewBag.ProcessTypeList = PayrollRepository.GetProssTypes();

            ViewBag.DepartmentList = _context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SectionList = _context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.EmpList = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete).Include(d => d.Cat_Designation).OrderBy(o => o.EmpCode).ToList();

            ViewBag.LocationList = _context.Cat_Location.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            //ViewBag.ReportList = permission;

            ViewBag.LineList = _context.Cat_Line.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.FloorList = _context.Cat_Floor.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.SubSectList = _context.Cat_SubSection.Include(x => x.Sect).Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.BankList = _context.Cat_Bank.Where(x => !x.IsDelete).ToList();
            //var permission = _context.ReportPermissions.Include(x => x.hr_reporttype).Where(x => x.hr_reporttype.ReportType == "Salary Report" && x.UserId == userid && x.ComId == comid).OrderBy(x => x.hr_reporttype.SLNo).ToList();
            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();

            #region Commented Code
            //SqlParameter[] parameter = new SqlParameter[3];
            //parameter[0] = new SqlParameter("@SoftwareId", softwareId);
            //parameter[1] = new SqlParameter("@VersionId", versionId);
            //parameter[2] = new SqlParameter("@ReportType", "Salary Report");


            //string query = $"Exec prc_VersionReportPermission '{softwareId}', '{versionId}', 'Salary Report'";

            //var permission = Helper.ExecProcMapTList<VersionReport>("dbo.prc_VersionReportPermission", parameter);
            #endregion

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Salary Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                $"'{softwareId}', '{versionId}', 'Salary Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Salary Report_Mng").OrderBy(x => x.SLNo).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MGTSalaryReport(SalarySheet SalarySheetmodel)
        {
            string callBackUrl = _PayrollReportRepository.MGTSalarySheet(SalarySheetmodel);
            return Redirect(callBackUrl);
        }

        #endregion

        #region ML Report
        public IActionResult MLReport()

        {
            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[3];
            parameter[0] = new SqlParameter("@Id", -1);
            parameter[1] = new SqlParameter("@ComID", comid);
            parameter[2] = new SqlParameter("@CHKML", '0');
            var getML = Helper.ExecProcMapTList<HRGetMLViewModel>("HR_prcGetMLPross", parameter);
            ViewBag.MLList = getML;
            return View();
        }

        [HttpGet]
        public IActionResult GetLeavData(int? lvId)
        {
            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[3];
            parameter[0] = new SqlParameter("@Id", lvId);
            parameter[1] = new SqlParameter("@ComID", comid);
            parameter[2] = new SqlParameter("@CHKML", 1);
            //var query = $"Exec HR_prcGetMLPross '{lvId}', '{comid}','{1}'";
            var getLeave = Helper.ExecProcMapTList<HR_Emp_ML_View>("HR_prcGetMLPross", parameter);
            return Json(getLeave);
        }

        [HttpPost]
        public IActionResult MLProcess(int? lvId, int MLPross, HR_Emp_ML hR_Emp_ML, bool isEdit)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                if (isEdit)
                {
                    var exist = _context.HR_Emp_ML.Where(a => a.LvId == hR_Emp_ML.LvId).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.FirstAmt = hR_Emp_ML.FirstAmt;
                        exist.SecondAmt = hR_Emp_ML.SecondAmt;
                        exist.ThirdAmt = hR_Emp_ML.ThirdAmt;
                        exist.FirstDays = hR_Emp_ML.FirstDays;
                        exist.SecondDays = hR_Emp_ML.SecondDays;
                        exist.ThirdDays = hR_Emp_ML.ThirdDays;
                        exist.TtlAmount = hR_Emp_ML.TtlAmount;
                        exist.TtlDays = hR_Emp_ML.TtlDays;
                        exist.OtherBonus = hR_Emp_ML.OtherBonus;
                        exist.OtherDeduct = hR_Emp_ML.OtherDeduct;
                        exist.FirstPayment = hR_Emp_ML.FirstPayment;
                        exist.LastPayment = hR_Emp_ML.LastPayment;
                        exist.Remarks = hR_Emp_ML.Remarks;
                        exist.NetPayable = hR_Emp_ML.NetPayable;


                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }


                SqlParameter[] parameter = new SqlParameter[3];
                parameter[0] = new SqlParameter("@ComID", comid);
                parameter[1] = new SqlParameter("@Id", lvId);
                parameter[2] = new SqlParameter("@MLPross", MLPross);
                Helper.ExecProc("Hr_prcProcessML", parameter);
                return Json(new { success = 1, message = "Process Successfully Completed." });
            }
            catch (Exception ex)
            {
                return Json(new { success = 0, message = ex.Message });
            }

        }


        [HttpGet]
        public IActionResult GetReport(int? lvId, string rptFormat/*, string reportType,int productId,  int empId, DateTime fromDate, DateTime toDate, string rptFormat, string product, int? WarehouseId*/)
        {
            try
            {
                #region Commented Code
                //string comid = HttpContext.Session.GetString("comid");
                //var reportname = "";
                //var filename = "";

                //reportname = "rptML";
                //filename = "rptML" + DateTime.Now.Date.ToString();
                //var query = "Exec HR_rptML '" + comid + "', '" + lvId + "'";

                //HttpContext.Session.SetString("reportquery", "Exec HR_rptML '" + comid + "', '" + lvId + "'");

                //HttpContext.Session.SetString("ReportPath", "~/ReportViewer/ML/" + reportname + ".rdlc");
                //HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //string DataSourceName = "DataSet1";
                //GTERP.Models.Common.clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                //GTERP.Models.Common.clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //GTERP.Models.Common.clsReport.strDSNMain = DataSourceName;

                ////var ConstrName = "ApplicationServices";
                ////string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat }); //Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });
                #endregion

                string redirectUrl = "";
                string callBackUrl = _PayrollReportRepository.MLGetReport(lvId, rptFormat);
                redirectUrl = callBackUrl;
                return Json(new { Url = redirectUrl });
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
