using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;



namespace GTERP.Repository.HR
{
    public class HRRepository : IHRRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        public HRRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }



        #region Active User Dashboard
        public List<ActiveUser> ActiveEmpList(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Active");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','Active','1'";

            var data = Helper.ExecProcMapTList<ActiveUser>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<ActiveUser> ActiveEmpMale(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Male");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','Male','1'";

            var data = Helper.ExecProcMapTList<ActiveUser>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<ActiveUser> ActiveEmpFemale(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Female");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','Female','1'";

            var data = Helper.ExecProcMapTList<ActiveUser>("HR_prcGetDashboard", parameter);
            return data;
        }
        public List<ActiveUser> TotalEmployees(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "TotalEmp");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','TotalEmp','1'";

            var data = Helper.ExecProcMapTList<ActiveUser>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<ActiveUser> ActiveEmpOTYes(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "OTYes");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','OTYes','1'";

            var data = Helper.ExecProcMapTList<ActiveUser>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<ActiveUser> ActiveEmpRelease(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Released");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','Released','1'";

            var data = Helper.ExecProcMapTList<ActiveUser>("HR_prcGetDashboard", parameter);
            return data;
        }

        #endregion

        #region Daily Attendance Active Dashboard
        public List<DailyAttendanceActive> Present(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@dtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Present");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string queryJ = $"Exec HR_prcGetDashboard '{comid}', '','{1}', '{date}','Present','1'";

            var data = Helper.ExecProcMapTList<DailyAttendanceActive>("dbo.HR_prcGetDashboard", parameter);

            return data;
        }

        public List<DailyAttendanceActive> Absent(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@dtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Absent");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string queryJ = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','Absent','1'";

            var data = Helper.ExecProcMapTList<DailyAttendanceActive>("dbo.HR_prcGetDashboard", parameter);

            return data;
        }

        public List<DailyAttendanceActive> Late(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@dtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Late");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string queryJ = $"Exec HR_prcGetDashboard '{comid}', '{1}', '{date}','Late','1'";

            var data = Helper.ExecProcMapTList<DailyAttendanceActive>("dbo.HR_prcGetDashboard", parameter);

            return data;
        }

        public List<DailyAttendanceActive> Leave(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@dtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "Leave");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string queryJ = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','Leave','1'";

            var data = Helper.ExecProcMapTList<DailyAttendanceActive>("dbo.HR_prcGetDashboard", parameter);
            return data;
        }
        public List<DailyAttendanceActive> WHDay(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@dtPunchDate", date);
            parameter[4] = new SqlParameter("@Type", "WHDay");
            parameter[5] = new SqlParameter("@Sectname", 1);

            string queryJ = $"Exec HR_prcGetDashboard '{comid}','' ,'{1}', '{date}','WHDay','1'";

            var data = Helper.ExecProcMapTList<DailyAttendanceActive>("dbo.HR_prcGetDashboard", parameter);
            return data;
        }
        #endregion

        #region Dashboard
        public Dashboard LoadData(string dtLoad)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            if (dtLoad == "")
            {
                var dashboard = InitializeDashBord();
                return dashboard;
            }
            else
            {
                //HttpContext.Session.SetString("DashboardDate", dtLoad);
                var dashboard = InitializeDashBord(dtLoad);
                return dashboard;
            }
        }

        public Dashboard InitializeDashBord(string date = null)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var dashBord = new Dashboard();

            if (date != null)
            {
                dashBord.DailyAttendance = PrcGetDailyAttendance(date);
                dashBord.MonthlyAttendance = PrcGetMonthlyAttendance(date);
            }
            else
            {
                dashBord.DailyAttendance = PrcGetDailyAttendance();
                dashBord.MonthlyAttendance = PrcGetMonthlyAttendance();
            }

            dashBord.DailyAttendanceSum = PrcGetDailyAttendanceSum(comid, date, date);
            dashBord.EmployeeDetails = PrcGetEmployeeDetails(comid, date);
            dashBord.SalaryDetails = PrcGetSalaryDetails(comid, date);
            dashBord.TotalEmpType = GetTotalEmpType(date);

            return dashBord;
        }

        public List<DailyAttendanceSum> PrcGetDailyAttendanceSum(string comid, string fromdate, string toDate)
        {
            var date = DateTime.Now;

            SqlParameter[] sqlParameter1 = new SqlParameter[14];
            sqlParameter1[0] = new SqlParameter("@ComId", comid);
            sqlParameter1[1] = new SqlParameter("@dtDate", Convert.ToDateTime(fromdate));
            sqlParameter1[2] = new SqlParameter("@dtTO", toDate);
            sqlParameter1[3] = new SqlParameter("@EmpId", "0");
            sqlParameter1[4] = new SqlParameter("@ShiftId", "0");
            sqlParameter1[5] = new SqlParameter("@DesigId", "0");
            sqlParameter1[6] = new SqlParameter("@DeptId", "0");
            sqlParameter1[7] = new SqlParameter("@SectId", "0");
            sqlParameter1[8] = new SqlParameter("@SubSectId", "0");
            sqlParameter1[9] = new SqlParameter("@EmpTypeId", "0");
            sqlParameter1[10] = new SqlParameter("@LineId", "0");
            sqlParameter1[11] = new SqlParameter("@UnitId", "0");
            sqlParameter1[12] = new SqlParameter("@FloorId", "0");
            sqlParameter1[13] = new SqlParameter("@Flag", "Department");
            string query = $"Exec HR_RptAttendSum '{comid}', '{fromdate}', '{toDate}', '{'0'}','{'0'}', '{'0'}','{'0'}','{'0'}','{'0'}', '{'0'}','{'0'}', '{'0'}','{'0'}','{"Department"}'";
            List<DailyAttendanceSum> listofDailyAttendance = Helper.ExecProcMapTList<DailyAttendanceSum>("HR_RptAttendSum", sqlParameter1);
            return listofDailyAttendance;
        }

        public DailyAttendance PrcGetDailyAttendance(string date = null)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            if (comid != null && date == null)
            {
                SqlParameter[] sqlParemeter = new SqlParameter[6];
                sqlParemeter[0] = new SqlParameter("@ComID", comid);
                sqlParemeter[1] = new SqlParameter("@userid", "");
                sqlParemeter[2] = new SqlParameter("@Id", 1);
                sqlParemeter[3] = new SqlParameter("@dtPunchDate", date);
                sqlParemeter[4] = new SqlParameter("@Type", "DailyAttendance");
                sqlParemeter[5] = new SqlParameter("@Sectname", 1);
                DailyAttendance aDailyAttendance = Helper.ExecProcMapT<DailyAttendance>("HR_prcGetDashboard", sqlParemeter);
                return aDailyAttendance;
            }
            if (comid != null && date != null)
            {
                SqlParameter[] sqlParemeter = new SqlParameter[6];
                sqlParemeter[0] = new SqlParameter("@ComID", comid);
                sqlParemeter[1] = new SqlParameter("@userid", "");
                sqlParemeter[2] = new SqlParameter("@Id", 1);
                sqlParemeter[3] = new SqlParameter("@dtPunchDate", date);
                sqlParemeter[4] = new SqlParameter("@Type", "DailyAttendance");
                sqlParemeter[5] = new SqlParameter("@Sectname", 1);

                string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{date}','DailyAttendance','1'";

                DailyAttendance aDailyAttendance = Helper.ExecProcMapT<DailyAttendance>("HR_prcGetDashboard", sqlParemeter);
                return aDailyAttendance;
            }
            return null;
        }

        public MonthlyAttendance PrcGetMonthlyAttendance(string date = null)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] sqlParemeter = new SqlParameter[6];
            sqlParemeter[0] = new SqlParameter("@ComID", comid);
            sqlParemeter[1] = new SqlParameter("@userid", "");
            sqlParemeter[2] = new SqlParameter("@Id", 1);
            sqlParemeter[3] = new SqlParameter("@dtPunchDate", date);
            sqlParemeter[4] = new SqlParameter("@Type", "OTInformation");
            sqlParemeter[5] = new SqlParameter("@Sectname", 1);
            string query = $"Exec HR_prcGetDashboard '{comid}', '','{1}', '{date}','OTInformation','1'";
            MonthlyAttendance aMonthlyAttendance = Helper.ExecProcMapT<MonthlyAttendance>("HR_prcGetDashboard", sqlParemeter);
            return aMonthlyAttendance;
        }

        public EmployeeDetails PrcGetEmployeeDetails(string comid, string dtPunchDate)
        {
            comid = _httpContext.HttpContext.Session.GetString("comid");
            var date = DateTime.Now;

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@userid", "");
            sqlParameter[2] = new SqlParameter("@Id", 1);
            sqlParameter[3] = new SqlParameter("@dtPunchDate", Convert.ToDateTime(dtPunchDate));
            sqlParameter[4] = new SqlParameter("@Type", "TotalEmployee");
            sqlParameter[5] = new SqlParameter("@Sectname", 1);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{'1'}','{dtPunchDate}', 'TotalEmployee','1'";

            EmployeeDetails aEmployeeDetails = Helper.ExecProcMapT<EmployeeDetails>("HR_prcGetDashboard", sqlParameter);


            return aEmployeeDetails;
        }

        //empType grap


        public List<TotalEmpType> GetTotalEmpType(string date)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");


            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@userid", "");
            sqlParameter[2] = new SqlParameter("@Id", 1);
            sqlParameter[3] = new SqlParameter("@dtPunchDate", Convert.ToDateTime(date));
            sqlParameter[4] = new SqlParameter("@Type", "TotalEmployeeType");
            sqlParameter[5] = new SqlParameter("@Sectname", "");

            //string query = $"Exec HR_RptAttendSum '{comid}','', '{'1'}','{dtPunchDate}', 'TotalEmployee','1'";

            var aTotalEmpType = Helper.ExecProcMapTList<TotalEmpType>("HR_prcGetDashboard", sqlParameter);



            return aTotalEmpType;
        }

        public SalaryDetails PrcGetSalaryDetails(string comid, string dtPunchDate)
        {
            comid = _httpContext.HttpContext.Session.GetString("comid");
            var date = DateTime.Now;

            SqlParameter[] sqlParemeter = new SqlParameter[2];
            sqlParemeter[0] = new SqlParameter("@ComID", comid);
            sqlParemeter[1] = new SqlParameter("@dtPunchDate", Convert.ToDateTime(dtPunchDate));
            string query = $"Exec PrcGetSalaryDetails '{comid}', '{dtPunchDate}'";
            SalaryDetails aSalaryDetails = Helper.ExecProcMapT<SalaryDetails>("PrcGetSalaryDetails", sqlParemeter);
            return aSalaryDetails;
        }

        public List<DailyAttendanceDepartmentWiseData> GetDepartmentWiseData(DateTime Date, string sectName)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "Daily Attendance Summary");
            parameter[5] = new SqlParameter("@Sectname", sectName);

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','Daily Attendance Summary','{sectName}'";

            var data = Helper.ExecProcMapTList<DailyAttendanceDepartmentWiseData>("HR_prcGetDashboard", parameter);
            return data;
        }


        public List<SalarySummeryDetails> PrcGetSalarySummery(string comid)
        {
            comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");
            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", userid);
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", "");
            parameter[4] = new SqlParameter("@Type", "Salary Summary");
            parameter[5] = new SqlParameter("@Sectname", "1");

            string query = $"Exec HR_prcGetDashboard '{comid}','{userid}', '{1}', '','Salary Summary','1'";

            var data = Helper.ExecProcMapTList<SalarySummeryDetails>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<SalaryCheck> GetSalarySummaryData(string sectname)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", userid);
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", "");
            parameter[4] = new SqlParameter("@Type", "Summery Salary Data");
            parameter[5] = new SqlParameter("@Sectname", sectname);

            string query = $"Exec HR_prcGetDashboard '{comid}', '{1}', '','Summery Salary Data','{sectname}'";

            var data = Helper.ExecProcMapTList<SalaryCheck>("HR_prcGetDashboard", parameter);
            return data;
        }
        /// <summary>
        /// Daily Cost

        public List<DailyCostSummary> GetDailyCost(DateTime Date)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var date = DateTime.Now.ToString("yyyy-MM-d");
            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", userid);
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "Daily costing summary");
            parameter[5] = new SqlParameter("@Sectname", "");

            string query = $"Exec HR_prcGetDashboard '{comid}','{userid}', '{1}', '{Date}','Daily costing summary',''";

            var data = Helper.ExecProcMapTList<DailyCostSummary>("HR_prcGetDashboard", parameter);
            return data;
        }
        /// <summary>
        /// 
        /// Daily Cost Summary
        /// </summary>
        public List<DailyCostDetails> GetDailyCostDetails(DateTime Date, string sectname)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var date = Date.Date.ToString("dd-MM-YYYY");
            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", userid);
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "Daily costing");
            parameter[5] = new SqlParameter("@Sectname", sectname);

            string query = $"Exec HR_prcGetDashboard '{comid}', '{1}', '','Summery Salary Data','{sectname}'";

            var data = Helper.ExecProcMapTList<DailyCostDetails>("HR_prcGetDashboard", parameter);
            return data;
        }
        #endregion

        #region Employee List


        public string EmpListPrint(int? id, string type = "pdf")
        {
            try
            {
                string SqlCmd = "";
                string ReportPath = "";
                var ConstrName = "ApplicationServices";
                var ReportType = "PDF";
                var comid = _httpContext.HttpContext.Session.GetString("comid");

                var reportname = "rptEmployeeDetails";
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/HR/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [HR_rptEmployeeDetails] '" + comid + "','" + id + "'");

                string filename = _context.HR_Emp_Info.Where(x => x.EmpId == id).Select(x => x.EmpName).Single().ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                _httpContext.HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                SqlCmd = clsReport.strQueryMain;
                ReportPath = clsReport.strReportPathMain;

                string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<EmpList> EmpListIndex(string criteria, int Offset, int fetch, string EmpCode, string EmpName, DateTime startDate, DateTime endDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[9];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@Criteria", criteria);
                parameter[2] = new SqlParameter("@Offset", Offset);
                parameter[3] = new SqlParameter("@fetch", fetch);
                parameter[4] = new SqlParameter("@SearchKeywords", EmpCode);
                parameter[5] = new SqlParameter("@SearchColumns", EmpName);
                parameter[6] = new SqlParameter("@startDate", startDate);
                parameter[7] = new SqlParameter("@endDate", endDate);
                parameter[8] = new SqlParameter("@UserId", userid);

                //var query = $"Exec HR_PrcGetEmpInfoListTest '{comid}', '{criteria}', '{Offset}','{fetch}','{EmpCode}','{EmpName}','{startDate}','{endDate}'";
                List<EmpList> EmployeeListData = Helper.ExecProcMapTList<EmpList>("HR_PrcGetEmpInfoListWithAprovalSetting", parameter);
                return EmployeeListData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<EmpListCount> EmpListIndexCount(string criteria, int Offset, int fetch, string EmpCode, string EmpName, DateTime startDate, DateTime endDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[8];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@Criteria", criteria);
                parameter[2] = new SqlParameter("@Offset", Offset);
                parameter[3] = new SqlParameter("@fetch", fetch);
                parameter[4] = new SqlParameter("@SearchKeywords", EmpCode);
                parameter[5] = new SqlParameter("@SearchColumns", EmpName);
                parameter[6] = new SqlParameter("@startDate", startDate);
                parameter[7] = new SqlParameter("@endDate", endDate);

                var EmployeeListDataCount = Helper.ExecProcMapTList<EmpListCount>("HR_PrcGetEmpInfoList", parameter);
                return EmployeeListDataCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<EmpProfile> GetEmpProfile(DateTime dtFrom, DateTime dtTo, int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[4];
                parameter[0] = new SqlParameter("@dtFrom", dtFrom);
                parameter[1] = new SqlParameter("@dtTo", dtTo);
                parameter[2] = new SqlParameter("@ComId", comid);
                parameter[3] = new SqlParameter("@EmpId", Id);

                List<EmpProfile> EmployeeProfile = Helper.ExecProcMapTList<EmpProfile>("Hr_prcEmpProfile", parameter);
                return EmployeeProfile;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<AttendanceDetails> GetAttendanceDetails(string Period, DateTime dtFrom, DateTime dtTo, int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[5];
                parameter[0] = new SqlParameter("@Period", Period);
                parameter[1] = new SqlParameter("@dtFrom", dtFrom);
                parameter[2] = new SqlParameter("@dtTo", dtTo);
                parameter[3] = new SqlParameter("@ComId", comid);
                parameter[4] = new SqlParameter("@EmpId", Id);

                List<AttendanceDetails> AttendanceDetails = Helper.ExecProcMapTList<AttendanceDetails>("Hr_prcAttDetails", parameter);
                return AttendanceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MonthlyOTRenderChartVM> MonthlyOTChart(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "MonthlyOT");
            parameter[5] = new SqlParameter("@Sectname", "1");

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','MonthlyOT','1'";

            var data = Helper.ExecProcMapTList<MonthlyOTRenderChartVM>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<DailyOTRenderChartVM> DailyOTChart(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "DailyOT");
            parameter[5] = new SqlParameter("@Sectname", "1");

            string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','DailyOT','1'";

            var data = Helper.ExecProcMapTList<DailyOTRenderChartVM>("HR_prcGetDashboard", parameter);
            return data;
        }

        public List<DailyPresentRenderChartVM> DailyPresentChart(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "DailyPresentList");
            parameter[5] = new SqlParameter("@Sectname", "1");

            //string query = $"Exec HR_prcGetDashboard '{comid}', '{1}', '{Date}','DailyOT','1'";

            var data = Helper.ExecProcMapTList<DailyPresentRenderChartVM>("HR_prcGetDashboard", parameter);
            return data;
        }

        ///Daily Present Ratio
        public List<DailyPresentRatioChartVM> DailyPresentRatioChart(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[6];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@userid", "");
            parameter[2] = new SqlParameter("@Id", 1);
            parameter[3] = new SqlParameter("@DtPunchDate", Date);
            parameter[4] = new SqlParameter("@Type", "Daily Present Ratio");
            parameter[5] = new SqlParameter("@Sectname", "1");

            //string query = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','DailyOT','1'";

            var data = Helper.ExecProcMapTList<DailyPresentRatioChartVM>("HR_prcGetDashboard", parameter);
            return data;
        }


        public List<MonthlyJoinReleaseVM> MonthlyJReleasedEmp(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameterJ = new SqlParameter[6];
            parameterJ[0] = new SqlParameter("@ComId", comid);
            parameterJ[1] = new SqlParameter("@userid", "");
            parameterJ[2] = new SqlParameter("@Id", 1);
            parameterJ[3] = new SqlParameter("@DtPunchDate", Date);
            parameterJ[4] = new SqlParameter("@Type", "MonthlyJoin");
            parameterJ[5] = new SqlParameter("@Sectname", "1");

            string queryJ = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','MonthlyJoin','1'";

            var dataJoin = Helper.ExecProcMapTList<MonthlyJoinVM>("HR_prcGetDashboard", parameterJ);

            SqlParameter[] parameterR = new SqlParameter[6];
            parameterR[0] = new SqlParameter("@ComId", comid);
            parameterR[1] = new SqlParameter("@userid", "");
            parameterR[2] = new SqlParameter("@Id", 1);
            parameterR[3] = new SqlParameter("@DtPunchDate", Date);
            parameterR[4] = new SqlParameter("@Type", "MonthlyReleased");
            parameterR[5] = new SqlParameter("@Sectname", "1");

            string queryR = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','MonthlyReleased','1'";

            var dataRelease = Helper.ExecProcMapTList<MonthlyReleaseVM>("HR_prcGetDashboard", parameterR);

            var dataJoinRelease = (from j in dataJoin
                                   join r in dataRelease
                                   on j.DtJoin equals r.DtReleased
                                   select new MonthlyJoinReleaseVM
                                   {
                                       DtJoin = j.DtJoin,
                                       JoinCount = j.EmpId,
                                       ReleaseCount = r.EmpId
                                   }).ToList();
            return dataJoinRelease;
        }

        public List<ManPowerHistoryVM> ManPowerHistoryEmp(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameterJ = new SqlParameter[6];
            parameterJ[0] = new SqlParameter("@ComId", comid);
            parameterJ[1] = new SqlParameter("@userid", "");
            parameterJ[2] = new SqlParameter("@Id", 1);
            parameterJ[3] = new SqlParameter("@DtPunchDate", Date);
            parameterJ[4] = new SqlParameter("@Type", "Line Wise Manpower");
            parameterJ[5] = new SqlParameter("@Sectname", "1");

            string queryJ = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','Line Wise Manpower','1'";

            var dataJoin = Helper.ExecProcMapTList<LineVM>("HR_prcGetDashboard", parameterJ);

            //SqlParameter[] parameterR = new SqlParameter[5];
            //parameterR[0] = new SqlParameter("@ComId", comid);
            //parameterR[1] = new SqlParameter("@Id", 1);
            //parameterR[2] = new SqlParameter("@DtPunchDate", Date);
            //parameterR[3] = new SqlParameter("@Type", "Dept. Wise Manpower");
            //parameterR[4] = new SqlParameter("@Sectname", "1");

            //string queryR = $"Exec HR_prcGetDashboard '{comid}', '{1}', '{Date}','Dept. Wise Manpower','1'";

            //var dataRelease = Helper.ExecProcMapTList<DeptWiseVM>("HR_prcGetDashboard", parameterR);

            var dataJoinRelease = (from j in dataJoin
                                   select new ManPowerHistoryVM
                                   {
                                       linename = j.linename,
                                       LineEmpCount = j.EmpId,

                                   }).ToList();


            return dataJoinRelease;
        }


        public List<DeptWiseManPowerVM> DeptWiseEmployeeChart(DateTime Date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameterR = new SqlParameter[6];
            parameterR[0] = new SqlParameter("@ComId", comid);
            parameterR[1] = new SqlParameter("@userid", "");
            parameterR[2] = new SqlParameter("@Id", 1);
            parameterR[3] = new SqlParameter("@DtPunchDate", Date);
            parameterR[4] = new SqlParameter("@Type", "Dept. Wise Manpower");
            parameterR[5] = new SqlParameter("@Sectname", "1");

            string queryR = $"Exec HR_prcGetDashboard '{comid}','', '{1}', '{Date}','Dept. Wise Manpower','1'";

            var deptData = Helper.ExecProcMapTList<DeptWiseVM>("HR_prcGetDashboard", parameterR);
            var deptWiseEmployee = (from j in deptData
                                    select new DeptWiseManPowerVM
                                    {
                                        DeptName = j.DeptName,
                                        DeptEmpCount = j.EmpId,

                                    }).ToList();
            return deptWiseEmployee;
        }


        public void Raw_DataTransfer(DateTime From, DateTime To)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[3];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@dtFrom", From);
                parameter[2] = new SqlParameter("@dtTo", To);

                var query = $"Exec HR_RawDataTransfer '{comid}', '{From}', '{To}'";
                List<EmpList> EmployeeListData = Helper.ExecProcMapTList<EmpList>("HR_RawDataTransfer", parameter);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<LeaveDetails> IHRRepository.GetLeaveDetails(int year, int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[3];
                parameter[0] = new SqlParameter("@year", year);
                parameter[1] = new SqlParameter("@ComId", comid);
                parameter[2] = new SqlParameter("@EmpId", Id);

                var query = $"Exec Hr_prcLeaveDetails '{year}', '{comid}', '{Id}'";
                List<LeaveDetails> leaveDetails = Helper.ExecProcMapTList<LeaveDetails>("Hr_prcLeaveDetails", parameter);
                if (leaveDetails.Count() == 0)
                {
                    var data = new LeaveDetails();
                    data.CL = "0";
                    data.ACL = "0";
                    data.EL = "0";
                    data.AEL = "0";
                    data.ML = "0";
                    data.AML = "0";
                    data.SL = "0";
                    data.ASL = "0";
                    leaveDetails.Add(data);
                }
                return leaveDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PaymentDetails> GetPaymentDetails(int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", Id);

                List<PaymentDetails> paymentDetails = Helper.ExecProcMapTList<PaymentDetails>("Hr_prcPaymentDetails", parameter);
                if (paymentDetails.Count() == 0)
                {
                    var data = new PaymentDetails();
                    data.PersonalPay = "0";
                    data.BasicSalary = "0";
                    data.HouseRent = "0";
                    data.MadicalAllow = "0";
                    data.CanteenAllow = "0";
                    data.MiscAdd = "0";
                    data.ConveyanceAllow = "0";
                    paymentDetails.Add(data);
                }
                return paymentDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<NomineeDetails> GetNomineeDetails(int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", Id);


                List<NomineeDetails> paymentDetails = Helper.ExecProcMapTList<NomineeDetails>("Hr_prcNomineeDetails", parameter);
                if (paymentDetails.Count() == 0)
                {
                    var data = new NomineeDetails();
                    data.EmpNomineeName1 = " ";
                    data.EmpNomineeName2 = " ";
                    data.EmpNomineeMobile1 = " ";
                    data.EmpNomineeMobile2 = " ";
                    paymentDetails.Add(data);
                }
                return paymentDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ShowCauseDetails> GetShowCauseDetails(int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", Id);


                List<ShowCauseDetails> causeDetails = Helper.ExecProcMapTList<ShowCauseDetails>("Hr_prcShowCauseDetails", parameter);
                return causeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<LoanDetails> GetLoanDetails(int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", Id);


                List<LoanDetails> loanDetails = Helper.ExecProcMapTList<LoanDetails>("Hr_prcLoanDetailsData", parameter);
                return loanDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalaryStructure> GetSalStrucDetails(int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", Id);


                List<SalaryStructure> strucDetails = Helper.ExecProcMapTList<SalaryStructure>("Hr_prcSalStructureDetails", parameter);
                foreach (var item in strucDetails)
                {
                    item.BS += "%";
                    item.HR += "%";
                    item.MA += "%";
                    item.CA += "%";
                    item.FA += "%";
                }
                if (strucDetails.Count() == 0)
                {
                    var data = new SalaryStructure();
                    data.BS = "50%";
                    data.HR = "30%";
                    data.MA = "15%";
                    data.CA = "5%";
                    data.FA = "5%";
                    strucDetails.Add(data);
                }
                return strucDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TaxDetails> GetTaxDetails(int Id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", Id);


                List<TaxDetails> taxDetails = Helper.ExecProcMapTList<TaxDetails>("Hr_prcTaxDetails", parameter);
                foreach (var item in taxDetails)
                {
                    int temp = (int)decimal.Parse(item.IncomeTax);
                    item.IncomeTax = temp.ToString();
                    item.yearlypay = temp * 12;
                }
                if (taxDetails.Count() == 0)
                {
                    var data = new TaxDetails();
                    data.IncomeTax = "0";
                    data.yearlypay = 0;
                    taxDetails.Add(data);

                }
                return taxDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
