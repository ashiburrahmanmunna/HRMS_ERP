using GTERP.BLL;
using GTERP.Interfaces.HR;
//using FreeGeoIPCore.AppCode;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Controllers.HR
{
    [OverridableAuthorize]
    public class DashboardController : Controller
    {
        private TransactionLogRepository tranlog;
        private GTRDBContext db;
        private readonly IHRRepository _hrRepository;
        //public clsConnectionNew clsCon { get; set; }
        public IHttpContextAccessor _httpContext { get; }
        public Dashboard _Dashboard { get; }
        public DailyAttendanceSum DailyAttendanceSum1 { get; }

        public DashboardController(
            GTRDBContext _db,
            clsConnectionNew _clsCon,
            IHttpContextAccessor httpContext,
            Dashboard _dashboard,
            DailyAttendanceSum dailyAttendanceSum,
            TransactionLogRepository tran,
            IHRRepository hrRepository
            )
        {
            tranlog = tran;
            //clsCon = _clsCon;
            _httpContext = httpContext;
            _Dashboard = _dashboard;
            DailyAttendanceSum1 = dailyAttendanceSum;
            this.db = _db;
            _hrRepository = hrRepository;
        }
        public ActionResult Index(string dtLoad)
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            var comid = HttpContext.Session.GetString("comid");

            //if (dtLoad == "")
            if (dtLoad == null)
            {

                var dashboard = _hrRepository.InitializeDashBord(DateTime.Now.ToString());
                //HttpContext.Session.SetString("DashboardDate", DateTime.Now.Date.ToString());
                ViewBag.ListOfAtt = _hrRepository.PrcGetDailyAttendanceSum(comid, DateTime.Now.ToString(), DateTime.Now.ToString());
                ViewBag.SalarySum = _hrRepository.PrcGetSalarySummery(comid);
                ViewBag.DtLoad = DateTime.Now.ToString("dd-MMM-yyyy");
                return RedirectToAction("LoadData", "Dashboard", new { dtLoad = ViewBag.DtLoad });
            }
            else
            {
                //HttpContext.Session.SetString("DashboardDate", dtLoad);
                var dashboard = dtLoad;
                ViewBag.DtLoad = dtLoad;
                ViewBag.ListOfAtt = _hrRepository.PrcGetDailyAttendanceSum(comid, DateTime.Now.ToString(), DateTime.Now.ToString());
                ViewBag.SalarySum = _hrRepository.PrcGetSalarySummery(comid);
                return View(dashboard);
            }

        }

        [HttpGet]
        public ActionResult LoadData(string dtLoad)
        {
            var comid = HttpContext.Session.GetString("comid");

            if (dtLoad == "")
            {
                var dashboard = _hrRepository.InitializeDashBord();
                //HttpContext.Session.SetString("DashboardDate", DateTime.Now.Date.ToString());
                ViewBag.ListOfAtt = _hrRepository.PrcGetDailyAttendanceSum(comid, DateTime.Now.ToString(), DateTime.Now.ToString());
                ViewBag.SalarySum = _hrRepository.PrcGetSalarySummery(comid);
                ViewBag.DtLoad = DateTime.Now.ToString("dd-MMMM-yyyy");
                return View(dashboard);
            }
            else
            {
                //HttpContext.Session.SetString("DashboardDate", dtLoad);
                var dashboard = _hrRepository.InitializeDashBord(dtLoad);
                ViewBag.DtLoad = dtLoad;
                ViewBag.ListOfAtt = _hrRepository.PrcGetDailyAttendanceSum(comid, dtLoad, dtLoad);
                ViewBag.SalarySum = _hrRepository.PrcGetSalarySummery(comid);
                return View("Index", dashboard);
            }

        }

        public ActionResult GetDailyAttSum()
        {
            var comid = HttpContext.Session.GetString("comid");
            List<DailyAttendanceSum> dailyAttendanceSum = PrcGetDailyAttendanceSum(comid, DateTime.Now.ToString(), DateTime.Now.ToString());
            return Json(dailyAttendanceSum);
        }

        public Dashboard InitializeDashBord(string date = null)
        {
            var comid = HttpContext.Session.GetString("comid");
            var dashBord = new Dashboard();

            if (date != null)
            {
                dashBord.DailyAttendance = _hrRepository.PrcGetDailyAttendance(date);
                dashBord.MonthlyAttendance = _hrRepository.PrcGetMonthlyAttendance(date);
            }
            else
            {
                dashBord.DailyAttendance = _hrRepository.PrcGetDailyAttendance();
                dashBord.MonthlyAttendance = _hrRepository.PrcGetMonthlyAttendance();
            }

            dashBord.DailyAttendanceSum = _hrRepository.PrcGetDailyAttendanceSum(comid, date, date);
            dashBord.EmployeeDetails = _hrRepository.PrcGetEmployeeDetails(comid, date);
            dashBord.SalaryDetails = _hrRepository.PrcGetSalaryDetails(comid, date);
            dashBord.SalarySummeryDetails = _hrRepository.PrcGetSalarySummery(comid);

            return dashBord;
        }

        public MonthlyAttendance PrcGetMonthlyAttendance(string date = null)
        {
            var comid = HttpContext.Session.GetString("comid");

            try
            {

                MonthlyAttendance aMonthlyAttendance = _hrRepository.PrcGetMonthlyAttendance(date);
                return aMonthlyAttendance;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public EmployeeDetails PrcGetEmployeeDetails(string comid, string dtPunchDate)

        {
            EmployeeDetails aEmployeeDetails = _hrRepository.PrcGetEmployeeDetails(comid, dtPunchDate);
            return aEmployeeDetails;
        }

        public SalaryDetails PrcGetSalaryDetails(string comid, string dtPunchDate)
        {
            SalaryDetails aSalaryDetails = _hrRepository.PrcGetSalaryDetails(comid, dtPunchDate);
            return aSalaryDetails;
        }

        public List<DailyAttendanceSum> PrcGetDailyAttendanceSum(string comid, string fromdate, string toDate)
        {
            try
            {
                List<DailyAttendanceSum> listofDailyAttendance = _hrRepository.PrcGetDailyAttendanceSum(comid, fromdate, toDate);
                return listofDailyAttendance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DailyAttendance PrcGetDailyAttendance(string date = null)
        {
            var comid = HttpContext.Session.GetString("comid");

            try
            {
                if (comid != null && date == null)
                {
                    DailyAttendance aDailyAttendance = _hrRepository.PrcGetDailyAttendance(date);
                    return aDailyAttendance;
                }
                if (comid != null && date != null)
                {
                    DailyAttendance aDailyAttendance = _hrRepository.PrcGetDailyAttendance(date);
                    return aDailyAttendance;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return null;
        }

        // for daily atttendance sum data
        public IActionResult GetDepartmentWiseData(DateTime Date, string sectName)
        {
            var data = _hrRepository.GetDepartmentWiseData(Date, sectName);
            return View(data);
        }

        // for 
        public IActionResult SalarySummaryData(string sectName)
        {

            var data = _hrRepository.GetSalarySummaryData(sectName);
            return View(data);
        }

        // for salary summary

        public IActionResult SalarySummaryDetails()
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _hrRepository.PrcGetSalarySummery(comid);
            return View(data);
        }

        //montly overtime
        public IActionResult MonthlyOTChart(DateTime Date)
        {
            var data = _hrRepository.MonthlyOTChart(Date);
            return Json(data);
        }

        // daily overtime
        public IActionResult DailyOTChart(DateTime Date)
        {

            var data = _hrRepository.DailyOTChart(Date);
            return Json(data);
        }

        //Daily Present
        public IActionResult DailyPresentChart(DateTime Date)
        {

            var data = _hrRepository.DailyPresentChart(Date);
            return Json(data);
        }

        // Monthly Join & Released Employee
        public IActionResult MonthlyJReleasedEmp(DateTime Date)
        {
            var dataJoinRelease = _hrRepository.MonthlyJReleasedEmp(Date);
            return Json(dataJoinRelease);
        }
        // Monthly Join & Released Employee
        public IActionResult ManPowerHistory(DateTime Date)
        {
            var dataJoinRelease = _hrRepository.ManPowerHistoryEmp(Date);
            return Json(dataJoinRelease);
        }

        //Line Wise
        public IActionResult DeptWiseDashBoard(DateTime Date)
        {
            var deptWiseTotalEmp = _hrRepository.DeptWiseEmployeeChart(Date);
            return Json(deptWiseTotalEmp);
        }




        /// Daily Cost Summary
        public IActionResult DailyCostSummary(DateTime Date)
       {
            var data = _hrRepository.GetDailyCost(Date);
            var dt = Date.ToString("yyyy-MM-dd");
            ViewBag.Dt = dt;
            return View(data);
        }

        public IActionResult DailyCostDetails(DateTime Date, string sectname)
        {
            DateTime dt = Convert.ToDateTime(Date);
            var data = _hrRepository.GetDailyCostDetails(Date, sectname);
            return View(data);
        }

        public IActionResult EmpTypeGrap(string date)
        {

            var data = _hrRepository.GetTotalEmpType(date);
            return Json(data);
        }

        public IActionResult DailyPresentRatio(DateTime Date)
        {
            List<DailyPresentRatioChartVM> dailyPresentRatioChartVMs = new List<DailyPresentRatioChartVM>();

            var data = _hrRepository.DailyPresentRatioChart(Date);
            var distinctDate = data.Select(o => o.date).Distinct();


            foreach (var date in distinctDate)
            {
                DailyPresentRatioChartVM dailyPresentRatioChartVM = new DailyPresentRatioChartVM();
                dailyPresentRatioChartVM.date = date;
                dailyPresentRatioChartVM.ttlemp = data.Where(w => w.ttlemp > 0 && w.date == date).Select(o => o.ttlemp).FirstOrDefault();
                dailyPresentRatioChartVM.ttlpresent = data.Where(w => w.date == date).Max(o => o.ttlpresent);
                dailyPresentRatioChartVM.ttlAbsent = data.Where(w => w.date == date).Max(o => o.ttlAbsent);

                dailyPresentRatioChartVMs.Add(dailyPresentRatioChartVM);
            }


            return Json(dailyPresentRatioChartVMs);
        }

    }

}