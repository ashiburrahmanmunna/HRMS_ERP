using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.ViewModels;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using static GTERP.Controllers.HR.HRController;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GTERP.Repository.HR
{
    public class RawDataViewRepository : IRawDataViewRepository
    {
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GSRawDataDBContext rDB;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private readonly IUrlHelper _urlHelper;


        public RawDataViewRepository(GTRDBContext context, IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,GSRawDataDBContext RDB)
        {
            _context = context;
            _httpContext = httpContext;
            rDB = RDB;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IEnumerable<SelectListItem> GetEmpList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");
        }

        public IQueryable<RawDataModernVM> GetRawData(PunchDataParamVM punchData)
        {
           
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string dateString = "1950-01-01 00:00:00.0000000";
            DateTime startDate = DateTime.Parse(dateString);
            DateTime endDate = DateTime.Parse(dateString);
            DateTime dateValue1;
            DateTime dateValue2;
            if (DateTime.TryParseExact(punchData.From, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue1))
            {
                startDate = DateTime.ParseExact(dateValue1.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            }
            if (DateTime.TryParseExact(punchData.To, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue2))
            {
                endDate = DateTime.ParseExact(dateValue2.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            }
            if(punchData.EmpId == 0) {
               // var query = from r in rDB.Hr_RawData
                var rawData = rDB.Hr_RawData.Where(r => r.ComId == comid && !r.IsDelete && r.DtPunchDate >= startDate && r.DtPunchDate <= endDate).ToList();
                var empInfo = _context.HR_Emp_Info.Where(i => i.ComId == comid && !i.IsDelete).ToList();
                var designations = _context.Cat_Designation.ToList();
                var sections = _context.Cat_Section.ToList();
                var departments = _context.Cat_Department.ToList();

                var query = from r in rawData
                            join i in empInfo on r.EmpId equals i.EmpId
                            join d in designations on new { i.ComId, i.DesigId } equals new { d.ComId, d.DesigId }
                            join s in sections on new { i.ComId, i.SectId } equals new { s.ComId, s.SectId }
                            join dp in departments on new { i.ComId, i.DeptId } equals new { dp.ComId, dp.DeptId }
                            select new
                            {
                                r,
                                i,
                                d,
                                s,
                                dp
                            };

                if (punchData.DeptName != "undefined")
                {
                    query = query.Where(x => x.dp.DeptName.ToLower().Contains(punchData.DeptName));
                }
                if (punchData.DesigName != "undefined")
                {
                    query = query.Where(x => x.d.DesigName.ToLower().Contains(punchData.DesigName));
                }
                if (punchData.SectName != "undefined")
                {
                    query = query.Where(x => x.s.SectName.ToLower().Contains(punchData.SectName));
                }
                if (punchData.EmpName != "undefined")
                {
                    query = query.Where(x => x.i.EmpName.ToLower().Contains(punchData.EmpName));
                }
                if (punchData.EmpCode != "undefined")
                {
                    query = query.Where(x => x.i.EmpCode.ToLower().Contains(punchData.EmpCode));
                }
                DateTime dtPunchDate;
                bool isValidFormat = DateTime.TryParseExact(punchData.DtPunchDate, new string[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPunchDate);
                if (isValidFormat)
                {
                    query = query.Where(x => x.r.DtPunchDate == dtPunchDate);
                }
                
                DateTime fromdate, todate;
                TimeSpan dtPunchTime;
                bool _isValidFormat = DateTime.TryParseExact(punchData.From, new string[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromdate);
                _isValidFormat = DateTime.TryParseExact(punchData.To, new string[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out todate);
                bool isTimeValid = TimeSpan.TryParseExact(punchData.DtPunchTime, new string[] { "h\\:mm", "hh\\:mm" }, CultureInfo.InvariantCulture, out dtPunchTime);

                if (isTimeValid)
                {
                    // Assuming DtPunchTime in database is of type DateTime
                    TimeSpan lowerBound = dtPunchTime.Subtract(TimeSpan.FromMinutes(1)); // 1 minute before the specified time
                    TimeSpan upperBound = dtPunchTime.Add(TimeSpan.FromMinutes(1)); // 1 minute after the specified time

                    query = query.Where(x => x.r.DtPunchTime >= fromdate.Date.Add(lowerBound) && x.r.DtPunchTime <= todate.Date.Add(upperBound));
                }

                var SelectQuery = query.Select(x => new RawDataModernVM
                {
                    AId = x.r.aId,
                    EmpId = x.r.EmpId,
                    EmpCode = x.i.EmpCode,
                    EmpName = x.i.EmpName,
                    DtJoin = x.i.DtJoin,
                    DesigName = x.d.DesigName,
                    SectName = x.s.SectName,
                    DeptName = x.dp.DeptName,
                    DtPunchDate = x.r.DtPunchDate,
                    DtPunchTime = x.r.DtPunchTime
                }).AsQueryable();

                return SelectQuery;

            }
            else
            {
                var rawData = rDB.Hr_RawData.Where(r => r.ComId == comid && !r.IsDelete && r.DtPunchDate >= startDate && r.DtPunchDate <= endDate).ToList();
                var empInfo = _context.HR_Emp_Info.Where(i => i.ComId == comid && !i.IsDelete).ToList();
                var designations = _context.Cat_Designation.ToList();
                var sections = _context.Cat_Section.ToList();
                var departments = _context.Cat_Department.ToList();

                var _query = from r in rawData
                            join i in empInfo on r.EmpId equals i.EmpId
                            join d in designations on new { i.ComId, i.DesigId } equals new { d.ComId, d.DesigId }
                            join s in sections on new { i.ComId, i.SectId } equals new { s.ComId, s.SectId }
                            join dp in departments on new { i.ComId, i.DeptId } equals new { dp.ComId, dp.DeptId }
                            select new
                            {
                                r,
                                i,
                                d,
                                s,
                                dp
                            };

                if (punchData.DeptName != "undefined")
                {
                    _query = _query.Where(x => x.dp.DeptName.ToLower().Contains(punchData.DeptName));
                }
                if (punchData.DesigName != "undefined")
                {
                    _query = _query.Where(x => x.d.DesigName.ToLower().Contains(punchData.DesigName));
                }
                if (punchData.SectName != "undefined")
                {
                    _query = _query.Where(x => x.s.SectName.ToLower().Contains(punchData.SectName));
                }
                if (punchData.EmpName != "undefined")
                {
                    _query = _query.Where(x => x.i.EmpName.ToLower().Contains(punchData.EmpName));
                }
                if (punchData.EmpCode != "undefined")
                {
                    _query = _query.Where(x => x.i.EmpCode.ToLower().Contains(punchData.EmpCode));
                }
                DateTime dtPunchDate;
                bool isValidFormat = DateTime.TryParseExact(punchData.DtPunchDate, new string[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPunchDate);
                if (isValidFormat)
                {
                    _query = _query.Where(x => x.r.DtPunchDate == dtPunchDate);
                }
                TimeSpan dtPunchTime;

                bool isTimeValid = TimeSpan.TryParseExact(punchData.DtPunchTime, new string[] { "h\\:mm", "hh\\:mm" }, CultureInfo.InvariantCulture, out dtPunchTime);

                if (isValidFormat && isTimeValid)
                {
                    DateTime punchDateTime = dtPunchDate + dtPunchTime;

                    _query = _query.Where(x => x.r.DtPunchTime == punchDateTime);
                }
                var SelectQuery = _query.Select(x => new RawDataModernVM
                {
                    AId = x.r.aId,
                    EmpId = x.r.EmpId,
                    EmpCode = x.i.EmpCode,
                    EmpName = x.i.EmpName,
                    DtJoin = x.i.DtJoin,
                    DesigName = x.d.DesigName,
                    SectName = x.s.SectName,
                    DeptName = x.dp.DeptName,
                    DtPunchDate = x.r.DtPunchDate,
                    DtPunchTime = x.r.DtPunchTime
                }).AsQueryable();

                return SelectQuery;

            }
        }

        public string EmpPunchDataPrint(int? id, string type = "pdf")
        {
            try
            {
                string SqlCmd = "";
                string ReportPath = "";
                var ConstrName = "ApplicationServices";
                var ReportType = "PDF";
                var comid = _httpContext.HttpContext.Session.GetString("comid");

                var reportname = "rptEmpPunchDetails";
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

        public string EditEmpPunchData(int id)
        {
            throw new NotImplementedException();
        }
    }
}
