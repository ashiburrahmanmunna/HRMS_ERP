using GTERP.Interfaces.HR_Report;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Payroll;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GTERP.Repository.HR_Report
{

    public class AllHRReportRepository : IAllHRReportRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<AllHRReportRepository> _logger;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();



        public AllHRReportRepository(IHttpContextAccessor httpContext,
            ILogger<AllHRReportRepository> logger,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _logger = logger;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public String JobCard(JobCardVM jobCard)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            ReportItem item = new ReportItem();
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            JobCardGrid aJobCardGrid = jobCard.JobCardGrid;

            if (aJobCardGrid.ToDate == null && aJobCardGrid.FromDate == null)
            {                
                aJobCardGrid.FromDate = jobCard.FromDate;
                aJobCardGrid.ToDate = jobCard.ToDate;
            }
            var comidFromJobCardReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                    .Where(x => x.ComId == comid &&
                    x.HR_ReportType.ReportName == "Job Card" &&
                    x.ReportType == "Job Card" &&
                    !x.IsDelete).ToList();
            ReportType = aJobCardGrid.ReportFormat;




            try
            {
                if (comidFromJobCardReport.Count > 0)
                {
                    foreach (var r in comidFromJobCardReport)
                    {
                        if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Job Card".ToLower()))
                        {
                            ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                        }
                    } //foreach
                }
                else
                {
                    ReportPath = "/ReportViewer/HR/rptJobCard.rdlc";
                }
                SqlCmd = "Exec rptJobCard '" + comid + "', '" + aJobCardGrid.FromDate + "', '" + aJobCardGrid.ToDate + "','" + aJobCardGrid.EmpId + "'," +
                             "" + aJobCardGrid.ShiftId + "," + aJobCardGrid.DesigId + "," + aJobCardGrid.DeptId + ", " + aJobCardGrid.SectId + "," +
                             aJobCardGrid.SubSectionId + "," + aJobCardGrid.EmpTypeId + "," + aJobCardGrid.LineId + "," + aJobCardGrid.UnitId + ","
                             + aJobCardGrid.FloorId + "";

                ConstrName = "ApplicationServices";
                ReportType = aJobCardGrid.ReportFormat;

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = "JobCard".ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        public string JobCard4h(JobCardVM jobCard4h)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportItem item = new ReportItem();
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            JobCardGrid aJobCard4hGrid = jobCard4h.JobCardGrid;

            if (aJobCard4hGrid.ToDate == null && aJobCard4hGrid.FromDate == null)
            {
                aJobCard4hGrid.FromDate = jobCard4h.FromDate;
                aJobCard4hGrid.ToDate = jobCard4h.ToDate;
            }
            var comidFromJobCard4hReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                .Where(x => x.ComId == comid &&
                                x.HR_ReportType.ReportName == "Job Card Buyer" &&
                                x.ReportType == "Job Card Buyer" &&
                                !x.IsDelete).ToList();
            ReportType = aJobCard4hGrid.ReportFormat;

            try
            {
                if (comidFromJobCard4hReport.Count > 0)
                {
                    foreach (var r in comidFromJobCard4hReport)
                    {
                        if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Job Card Buyer".ToLower()))
                        {
                            ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                        }
                    } //foreach
                }
                else
                {
                    ReportPath = "/ReportViewer/HR/rptJobCard4h.rdlc";
                }

                SqlCmd = "Exec rptJobCard4h '" + comid + "', '" + aJobCard4hGrid.FromDate + "', '" + aJobCard4hGrid.ToDate + "','" + aJobCard4hGrid.EmpId + "'," +
                            "" + aJobCard4hGrid.ShiftId + "," + aJobCard4hGrid.DesigId + "," + aJobCard4hGrid.DeptId + ", " + aJobCard4hGrid.SectId + "," +
                            aJobCard4hGrid.SubSectionId + "," + aJobCard4hGrid.EmpTypeId + "," + aJobCard4hGrid.LineId + "," + aJobCard4hGrid.UnitId + ","
                            + aJobCard4hGrid.FloorId + "";

                ConstrName = "ApplicationServices";
                ReportType = aJobCard4hGrid.ReportFormat;             


                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = "JobCard".ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        public string JobCardB(JobCardVM jobCardB)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportItem item = new ReportItem();
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";

            JobCardGrid aJobCardBGrid = jobCardB.JobCardGrid;

            if (aJobCardBGrid.ToDate == null && aJobCardBGrid.FromDate == null)
            {
                aJobCardBGrid.FromDate = jobCardB.FromDate;
                aJobCardBGrid.ToDate = jobCardB.ToDate;
            }
            var comidFromJobCardBReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                    .Where(x => x.ComId == comid &&
                    x.HR_ReportType.ReportName == "Job Card Buyer" &&
                    x.ReportType == "Job Card Buyer" &&
                    !x.IsDelete).ToList();
            ReportType = aJobCardBGrid.ReportFormat;
            try
            {              
                if (comidFromJobCardBReport.Count > 0)
                {
                    foreach (var r in comidFromJobCardBReport)
                    {
                        if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Job Card Buyer".ToLower()))
                        {
                            ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                        }
                    } //foreach
                }
                else
                {
                    ReportPath = "/ReportViewer/HR/rptJobCardB.rdlc";
                }

                SqlCmd = "Exec rptJobCardB '" + comid + "', '" + aJobCardBGrid.FromDate + "', '" + aJobCardBGrid.ToDate + "','" + aJobCardBGrid.EmpId + "'," +
                             "" + aJobCardBGrid.ShiftId + "," + aJobCardBGrid.DesigId + "," + aJobCardBGrid.DeptId + ", " + aJobCardBGrid.SectId + "," +
                             aJobCardBGrid.SubSectionId + "," + aJobCardBGrid.EmpTypeId + "," + aJobCardBGrid.LineId + "," + aJobCardBGrid.UnitId + ","
                             + aJobCardBGrid.FloorId + "";

                ConstrName = "ApplicationServices";
                ReportType = aJobCardBGrid.ReportFormat;

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = "JobCard".ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        public string DynamicJobCard(JobCardVM dynamicJobCard)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportItem item = new ReportItem();
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";

            JobCardGrid aDJobCardGrid = dynamicJobCard.JobCardGrid;

            if (aDJobCardGrid.ToDate == null && aDJobCardGrid.FromDate == null)
            {
                aDJobCardGrid.FromDate = dynamicJobCard.FromDate;
                aDJobCardGrid.ToDate = dynamicJobCard.ToDate;
            }
            var comidFromJobCardBReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                    .Where(x => x.ComId == comid &&
                    x.HR_ReportType.ReportName == "Job Card Buyer" &&
                    x.ReportType == "Job Card Buyer" &&
                    !x.IsDelete).ToList();
            ReportType = aDJobCardGrid.ReportFormat;
            try
            {
                if (comidFromJobCardBReport.Count > 0)
                {
                    foreach (var r in comidFromJobCardBReport)
                    {
                        if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Job Card Buyer".ToLower()))
                        {
                            ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                        }
                    } //foreach
                }
                else
                {
                    ReportPath = "/ReportViewer/HR/rptJobCardDynamic.rdlc";
                }

                SqlCmd = "Exec rptJobCardDynamic '" + comid + "', '" + aDJobCardGrid.FromDate + "', '" + aDJobCardGrid.ToDate + "','" + aDJobCardGrid.EmpId + "'," +
                             "" + aDJobCardGrid.ShiftId + "," + aDJobCardGrid.DesigId + "," + aDJobCardGrid.DeptId + ", " + aDJobCardGrid.SectId + "," +
                             aDJobCardGrid.SubSectionId + "," + aDJobCardGrid.EmpTypeId + "," + aDJobCardGrid.LineId + "," + aDJobCardGrid.UnitId + ","
                             + aDJobCardGrid.FloorId + "";

                ConstrName = "ApplicationServices";
                ReportType = aDJobCardGrid.ReportFormat;

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = "JobCard".ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        public string LeaveReport(LeaveReportVM aLeaveReport)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportItem item = new ReportItem();
            string query = "";
            string path = "";
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            var critiria = "All";
            LeaveProdGrid aLeaveGrid = aLeaveReport.LeaveProdGrid;

            var comidFromLeaveReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                .Where(x => x.ComId == comid &&
                                x.HR_ReportType.ReportName == aLeaveGrid.ReportType &&
                                x.ReportType == "Leave Report" &&
                                !x.IsDelete).ToList();

            ReportType = aLeaveGrid.ReportFormat;
            try
            {
                switch (aLeaveGrid.ReportType)
                {
                    case "General":
                        SqlCmd = "Exec HR_rptLeave '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                          "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                          aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'General', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;
                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("General".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptLeaveList.rdlc";
                        }                        

                        break;

                    case "ML":

                        SqlCmd = "Exec HR_rptLeave'" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                              "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                              aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'ML', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;
                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("ML".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptLeaveList.rdlc";
                        }


                        break;

                    case "First ML":
                        SqlCmd = "Exec HR_rptLeave '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                          "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                          aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'First ML', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;
                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("First ML".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptLeaveList.rdlc";
                        }

                        break;

                    case "Last ML":
                        SqlCmd = "Exec HR_rptLeave '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                              "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                              aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'Last ML', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;

                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Last ML".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptLeaveList.rdlc";
                        }                        
                        
                        break;

                    case "Yearly Leave Details":
                        SqlCmd = "Exec HR_rptLeaveBalance '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                             "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                             aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'Yearly Leave Details', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;
                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Yearly Leave Details".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptLeaveDetails.rdlc";
                        }
                        
                       

                        break;

                    case "Yearly Leave Summary":
                        SqlCmd = "Exec HR_rptLeaveBalance '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                              "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                              aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'Yearly Leave Summary', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;
                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Yearly Leave Summary".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptLeaveSum.rdlc";
                        }                       
                        

                        break;

					//New add 4-mar-2024
					case "Yearly Personal Leave Summary":
						SqlCmd = "Exec HR_rptLeaveBalance '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
							  "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
							  aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'Yearly Personal Leave Summary', '" + critiria + "','=ALL='";

						ConstrName = "ApplicationServices";
						//ReportType = "PDF";
						ReportType = aLeaveReport.ReportFormat;
						if (comidFromLeaveReport.Count > 0)
						{
							foreach (var r in comidFromLeaveReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Yearly Personal Leave Summary".ToLower()))
								{
									ReportPath = $"/ReportViewer/HR/{r.ReportName}";
								}
							} //foreach
						}
						else
						{
							ReportPath = "/ReportViewer/HR/rptPersonalYearlyLeaveSummary_IrishDesign.rdlc";
						}


						break;

					case "Leave Form":
                        SqlCmd = "Exec HR_rptLeaveForm '" + comid + "', '" + aLeaveGrid.FromDate + "', '" + aLeaveGrid.ToDate + "','" + aLeaveGrid.EmpId + "'," +
                             "" + aLeaveGrid.DesigId + "," + aLeaveGrid.DeptId + ", " + aLeaveGrid.SectId + "," +
                             aLeaveGrid.SubSectionId + "," + aLeaveGrid.EmpTypeId + "," + aLeaveGrid.LeaveTypeId + "," + aLeaveGrid.LineId + "," + aLeaveGrid.UnitId + "," + aLeaveGrid.FloorId + ", 'Leave Form', '" + critiria + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aLeaveReport.ReportFormat;
                        //RDLC
                        if (comidFromLeaveReport.Count > 0)
                        {
                            foreach (var r in comidFromLeaveReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Leave Form".ToLower()))
                                {
                                    if (aLeaveGrid.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                        break;
                                    }
                                    else if (aLeaveGrid.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                        break;
                                    }
                                    else
                                    {
                                        ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aLeaveGrid.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/HR/rptLeaveForm.rdlc";
                            }
                            else if (aLeaveGrid.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/HR/rptLeaveForm.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/HR/rptLeaveFormWorker.rdlc";

                            }
                        }
                       

                        break;
                }
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aLeaveGrid.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }


        }
        public String ProdReport(ProdVM prod)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");


            ReportItem item = new ReportItem();

            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            var Criteria = "All";
            ProdGridVM aProdGridVM = prod.ProdGridVM;
            try
            {
                switch (aProdGridVM.ReportType)
                {


                    case "Daily Production":
                        ReportPath = "/ReportViewer/HR/rptAttendDailyProd.rdlc";
                        SqlCmd = "Exec HR_RptDProd '" + comid + "', '" + aProdGridVM.FromDate + "', '" + aProdGridVM.ToDate + "','" + aProdGridVM.EmpId + "'," +
                              "" + aProdGridVM.DesigId + "," + aProdGridVM.DeptId + ", " + aProdGridVM.SectId + "," +
                              aProdGridVM.SubSectionId + "," + aProdGridVM.EmpTypeId + "," + aProdGridVM.LineId + "," + aProdGridVM.UnitId + "," + aProdGridVM.FloorId + "," + aProdGridVM.StyleId + ", 'DailyProd', '" + Criteria.ToLower() + "','=ALL='";
                        ConstrName = "ApplicationServices";
                        ReportType = aProdGridVM.ReportFormat;
                        break;

                    case "Daily Production Summary":
                        ReportPath = "/ReportViewer/HR/rptDailyProdSum.rdlc";
                        SqlCmd = "Exec HR_RptDailyProd '" + comid + "', '" + aProdGridVM.FromDate + "', '" + aProdGridVM.ToDate + "','" + aProdGridVM.EmpId + "'," +
                              "" + aProdGridVM.DesigId + "," + aProdGridVM.DeptId + ", " + aProdGridVM.SectId + "," +
                              aProdGridVM.SubSectionId + "," + aProdGridVM.EmpTypeId + "," + aProdGridVM.LineId + "," + aProdGridVM.UnitId + "," + aProdGridVM.FloorId + "," + aProdGridVM.StyleId + ", 'Section', '" + Criteria.ToLower() + "','=ALL='";
                        ConstrName = "ApplicationServices";
                        ReportType = aProdGridVM.ReportFormat;

                        break;
                }

                if (ReportType is null)
                {
                    ReportType = "PDF";
                }

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aProdGridVM.ReportFormat + "_" + DateTime.Now.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }


        }
        public String MonthlyAttendance(MonthlyAttendanceVM aMonthlyAttendance)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            ReportItem item = new ReportItem();
            string query = "";
            string path = "";
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            var critiria = "All";
            MonthlyAttendanceProdGrid aMonthlyGrid = aMonthlyAttendance.MonthlyAttendanceProdGrid;

            if (aMonthlyGrid.ToDate == null && aMonthlyGrid.FromDate == null)
            {
                aMonthlyGrid.FromDate = aMonthlyAttendance.FromDate;
                aMonthlyGrid.ToDate = aMonthlyAttendance.ToDate;
            }

            var comidFromMonthlyAttendanceReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                            .Where(x => x.ComId == comid &&
                                            x.HR_ReportType.ReportName == aMonthlyGrid.ReportType &&
                                            x.ReportType == "Monthly Attendance" &&
                                            !x.IsDelete).ToList();

            ReportType = aMonthlyGrid.ReportFormat;

            try
            {
                switch (aMonthlyGrid.ReportType)
                {

                    case "Monthly Attendance Summary":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Attendance Summary".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        }
                        
                        SqlCmd = "Exec HR_rptAttendMonthly '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'Monthly Attendance Summary', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;
                    case "Monthly Attendance Summary Sheet":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Attendance Summary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyB.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        SqlCmd = "Exec HR_rptAttendMonthly'" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'Monthly Attendance Summary', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;

                    case "Monthly Absent":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Absent".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } 
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyAbsent.rdlc";
                        }
                        
                        //ReportPath = "/ReportViewer/HR/rptAttendMonthlyAbsent.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyAbsent','" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;
                    case "Monthly Late":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Late".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyLate.rdlc";
                        }
                        
                        //ReportPath = "/ReportViewer/HR/rptAttendMonthlyLate.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyLate', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;
                    case "Monthly Overtime":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Overtime".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyOT.rdlc";
                        }
                        
                        //ReportPath = "/ReportViewer/HR/rptAttendMonthlyOT.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyOvertime', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    case "Monthly Overtime Sheet":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Overtime Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyOT.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthlyOT.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttendB '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyOvertime', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;
                    //5-dec-2023
                    case "Monthly Overtime Sheet (Yearly)":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Overtime Sheet (Yearly)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptMonthlyOtSheet(Yearly)_Unileaver.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthlyOT.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttendB '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId +
                              "', 'MonthlyOvertimeSheet(Yearly)', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    case "Monthly Job Card":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Job Card".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        }
                       
                        //ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyJobCard', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;
                    case "Monthly Job Card Sheet":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Job Card Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttendB '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyJobCard', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    case "Yearly Full Present":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Yearly Full Present".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptYearlyPresent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptYearlyPresent.rdlc";
                        SqlCmd = "Exec HR_RptYearlyFullPresent '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'Yearly Full Present', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    case "Yearly Maximum Present":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Yearly Maximum Present".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptYearlyPresent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptYearlyPresent.rdlc";
                        SqlCmd = "Exec HR_RptYearlyFullPresent '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'Yearly Maximum Present', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    //Only For Unileaver------------------------
                    //New add 25-nov-2023
                    case "Monthly Present & Absent List":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Present & Absent List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptPresent&AbsentListUbl.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthlyAbsent.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend_UBL '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyPresent&AbsentList','" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;
                    case "Monthly Attend Summary":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Attend Summary".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthly_Unileaver.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend'" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'MonthlyAttendVendorWise', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;

                    case "vendorwise_daily_entry":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("vendorwise_daily_entry".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/Vendorwise_daily_entry.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        SqlCmd = "Exec HR_Vendorwise_daily_entry'" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'MonthlyAttendVendorWise', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;
                    case "Monthly Schedule Shift":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Job Card".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptJobCardMonthlyReport.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyJobCardshift', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    case "Shiftwise payment count":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Shiftwise payment count".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails_Unilever.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend_Unilever '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'MonthlyJobCard', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;

                    case "Vendors achieve & fail status":
                        //RDLC
                         if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Attendance Achivment".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyAchivment_Unileaver.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend'" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'MonthlyAttendAchivment', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;                        

                    case "Vendorwise Daily Entry":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Vendorwise daily entry".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMonthlyAchivment_Unileaver.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        SqlCmd = "Exec HR_Vendorwise_daily_entry'" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'MonthlyAttendAchivment', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;       


                    case "Datewise Attendance":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Datewise Attendance".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptJobCard_Unilever_individual.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMonthly.rdlc";
                        SqlCmd = "Exec rptJobCard_ubl'" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "'," +
                              "" + aMonthlyGrid.ShiftId + "," + aMonthlyGrid.DesigId + "," + aMonthlyGrid.DeptId + ", " + aMonthlyGrid.SectId + "," +
                              aMonthlyGrid.SubSectionId + "," + aMonthlyGrid.EmpTypeId + "," + aMonthlyGrid.LineId + "," + aMonthlyGrid.UnitId + "," + aMonthlyGrid.FloorId + ", 'MonthlyJobCard', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";

                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;

                        break;

                    case "Casual Payment Sheet":
                        //RDLC
                        if (comidFromMonthlyAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromMonthlyAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Casual Payment Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptJobCardMonthlyWithPayment.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptJobCardMonthlyDetails.rdlc";
                        SqlCmd = "Exec HR_rptMonthlyAttend '" + comid + "', '" + aMonthlyGrid.FromDate + "', '" + aMonthlyGrid.ToDate + "','" + aMonthlyGrid.EmpId + "','" +
                              "" + aMonthlyGrid.ShiftId + "', '" + aMonthlyGrid.DesigId + "', '" + aMonthlyGrid.DeptId + "', '" + aMonthlyGrid.SectId + "','" +
                              aMonthlyGrid.SubSectionId + "','" + aMonthlyGrid.EmpTypeId + "', '" + aMonthlyGrid.LineId + "', '" + aMonthlyGrid.UnitId + "','" + aMonthlyGrid.FloorId + "', 'CasualPaymentSheet', '" + critiria + "','" + aMonthlyGrid.EmpStatus + "'";
                        ConstrName = "ApplicationServices";
                        ReportType = aMonthlyAttendance.ReportFormat;
                        break;
                }

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aMonthlyGrid.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }




        }
        public String LoanReport(LoanReportVM aLoanReport)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            ReportItem item = new ReportItem();
            string query = "";
            string path = "";
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";

            try
            {
                switch (aLoanReport.ReportName)
                {
                    case "HB Loan Report":

                        ReportPath = "/ReportViewer/Payroll/rptITStatement.rdlc";
                        SqlCmd = "Exec rptITaxStatement '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'HB Loan Report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "MC Loan Report":

                        ReportPath = "/ReportViewer/Payroll/rptLoanMC.rdlc";
                        SqlCmd = "Exec rptLoanDeduction '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'MC Loan Report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;


                    case "WF Loan Report":

                        ReportPath = "/ReportViewer/Payroll/rptLoanMC.rdlc"; ;
                        SqlCmd = "Exec rptLoanDeduction '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'WF Loan Report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "PF Loan Report":

                        ReportPath = "/ReportViewer/Payroll/rptLoanPF.rdlc"; ;
                        SqlCmd = "Exec rptLoanDeduction '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'PF Loan Report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "OTHER Loan Report":

                        ReportPath = "/ReportViewer/Payroll/rptLoanOther.rdlc"; ;
                        SqlCmd = "Exec rptLoanDeduction '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'OTHER Loan Report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;


                    case "MotorCycle Loan Letter":

                        ReportPath = "/ReportViewer/Payroll/rptRecoveryMCLoan.rdlc"; ;
                        SqlCmd = "Exec rptLoanLetter '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'MotorCycle Loan Letter'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "Welfare Loan Letter":

                        ReportPath = "/ReportViewer/Payroll/rptRecoveryWFLoan.rdlc"; ;
                        SqlCmd = "Exec rptLoanLetter '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'Welfare Loan Letter'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "House Building Loan Letter":

                        ReportPath = "/ReportViewer/Payroll/rptRecoveryHBLoan.rdlc"; ;
                        SqlCmd = "Exec rptLoanLetter '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'House Building Loan Letter'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "PF Loan Letter":

                        ReportPath = "/ReportViewer/Payroll/rptRecoveryPFLoan.rdlc"; ;
                        SqlCmd = "Exec rptLoanLetter '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'PF Loan Letter'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                    case "Other Loan Letter":

                        ReportPath = "/ReportViewer/Payroll/rptRecoveryOtherLoan.rdlc"; ;
                        SqlCmd = "Exec rptLoanLetter '" + comid + "', '" + aLoanReport.FromDate + "', '" + aLoanReport.ToDate + "','" + aLoanReport.EmpTypeId + "', " +
                              "" + aLoanReport.SectionId + ", " + aLoanReport.EmpId + ",  'Other Loan Letter'";
                        ConstrName = "ApplicationServices";
                        ReportType = aLoanReport.Format;


                        break;

                }
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aLoanReport.ReportName.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }


        }
        public String IncrementReport(IncrementReportVM aIncrementReport)
        {


            string comid = _httpContext.HttpContext.Session.GetString("comid");

            ReportItem item = new ReportItem();
            string query = "";
            string path = "";
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            IncrementProdGrid aIncrementGrid = aIncrementReport.IncrementProdGrid;

            var comidFromIncrementReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                           .Where(x => x.ComId == comid &&
                                           x.HR_ReportType.ReportName == aIncrementGrid.ReportType &&
                                           !x.IsDelete).ToList();

            try
            {
                switch (aIncrementGrid.ReportType)
                {
                    case "Increment":
                        //RDLC
                        if (comidFromIncrementReport.Count > 0)
                        {
                            foreach (var r in comidFromIncrementReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptIncrList.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptIncrList.rdlc";
                        SqlCmd = "Exec HR_rptIncrement '" + comid + "', '" + aIncrementGrid.FromDate + "', '" + aIncrementGrid.ToDate + "','" + aIncrementGrid.EmpId + "'," +
                              "" + aIncrementGrid.ShiftId + "," + aIncrementGrid.DesigId + "," + aIncrementGrid.DeptId + ", " + aIncrementGrid.SectId + "," +
                              aIncrementGrid.SubSectionId + "," + aIncrementGrid.EmpTypeId + "," + aIncrementGrid.LineId + "," + aIncrementGrid.UnitId + "," 
                              + aIncrementGrid.FloorId + ", 'Increment', '" + "=ALL=" + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        ReportType = aIncrementReport.ReportFormat;

                        break;

                    case "Promotion":
                        //RDLC
                        if (comidFromIncrementReport.Count > 0)
                        {
                            foreach (var r in comidFromIncrementReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Promotion".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptPromotionList.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptPromotionList.rdlc";
                        SqlCmd = "Exec HR_rptIncrement '" + comid + "', '" + aIncrementGrid.FromDate + "', '" + aIncrementGrid.ToDate + "','" + aIncrementGrid.EmpId + "'," +
                              "" + aIncrementGrid.ShiftId + "," + aIncrementGrid.DesigId + "," + aIncrementGrid.DeptId + ", " + aIncrementGrid.SectId + "," +
                              aIncrementGrid.SubSectionId + "," + aIncrementGrid.EmpTypeId + "," + aIncrementGrid.LineId + "," + aIncrementGrid.UnitId + "," 
                              + aIncrementGrid.FloorId + ", 'Promotion', '" + "=ALL=" + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        ReportType = aIncrementReport.ReportFormat;

                        break;

                    case "Increment with Promotion":
                        //RDLC
                        if (comidFromIncrementReport.Count > 0)
                        {
                            foreach (var r in comidFromIncrementReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment with Promotion".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptIncrPromoList.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptIncrPromoList.rdlc";
                        SqlCmd = "Exec HR_rptIncrement '" + comid + "', '" + aIncrementGrid.FromDate + "', '" + aIncrementGrid.ToDate + "','" + aIncrementGrid.EmpId + "'," +
                              "" + aIncrementGrid.ShiftId + "," + aIncrementGrid.DesigId + "," + aIncrementGrid.DeptId + ", " + aIncrementGrid.SectId + "," +
                              aIncrementGrid.SubSectionId + "," + aIncrementGrid.EmpTypeId + "," + aIncrementGrid.LineId + "," + aIncrementGrid.UnitId + "," 
                              + aIncrementGrid.FloorId + ", 'Increment with Promotion', '" + "=ALL=" + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        ReportType = aIncrementReport.ReportFormat;

                        break;

                    case "Increment Entitle":
                        //RDLC
                        if (comidFromIncrementReport.Count > 0)
                        {
                            foreach (var r in comidFromIncrementReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment Entitle".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptIncrEntitle.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptIncrEntitle.rdlc";
                        SqlCmd = "Exec HR_rptIncrement '" + comid + "', '" + aIncrementGrid.FromDate + "', '" + aIncrementGrid.ToDate + "','" + aIncrementGrid.EmpId + "'," +
                              "" + aIncrementGrid.ShiftId + "," + aIncrementGrid.DesigId + "," + aIncrementGrid.DeptId + ", " + aIncrementGrid.SectId + "," +
                              aIncrementGrid.SubSectionId + "," + aIncrementGrid.EmpTypeId + "," + aIncrementGrid.LineId + "," + aIncrementGrid.UnitId + "," 
                              + aIncrementGrid.FloorId + ", 'Increment Entitle', '" + "=ALL=" + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        ReportType = aIncrementReport.ReportFormat;

                        break;

                    case "Promotion Entitle":
                        //RDLC
                        if (comidFromIncrementReport.Count > 0)
                        {
                            foreach (var r in comidFromIncrementReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Promotion Entitle".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptPromotionEntitle.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptPromotionEntitle.rdlc";
                        SqlCmd = "Exec HR_rptIncrement '" + comid + "', '" + aIncrementGrid.FromDate + "', '" + aIncrementGrid.ToDate + "','" + aIncrementGrid.EmpId + "'," +
                              "" + aIncrementGrid.ShiftId + "," + aIncrementGrid.DesigId + "," + aIncrementGrid.DeptId + ", " + aIncrementGrid.SectId + "," +
                              aIncrementGrid.SubSectionId + "," + aIncrementGrid.EmpTypeId + "," + aIncrementGrid.LineId + "," + aIncrementGrid.UnitId + "," 
                              + aIncrementGrid.FloorId + ", 'Promotion Entitle', '" + "=ALL=" + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        ReportType = aIncrementReport.ReportFormat;

                        break;

                    case "Confirmation Entitle":
                        //RDLC
                        if (comidFromIncrementReport.Count > 0)
                        {
                            foreach (var r in comidFromIncrementReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Confirmation Entitle".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptConfirmationEntitle.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptConfirmationEntitle.rdlc";
                        SqlCmd = "Exec HR_rptIncrement '" + comid + "', '" + aIncrementGrid.FromDate + "', '" + aIncrementGrid.ToDate + "','" + aIncrementGrid.EmpId + "'," +
                              "" + aIncrementGrid.ShiftId + "," + aIncrementGrid.DesigId + "," + aIncrementGrid.DeptId + ", " + aIncrementGrid.SectId + "," +
                              aIncrementGrid.SubSectionId + "," + aIncrementGrid.EmpTypeId + "," + aIncrementGrid.LineId + "," + aIncrementGrid.UnitId + "," 
                              + aIncrementGrid.FloorId + ", 'Confirmation Entitle', '" + "=ALL=" + "','=ALL='";

                        ConstrName = "ApplicationServices";
                        ReportType = aIncrementReport.ReportFormat;

                        break;
                }
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aIncrementGrid.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        public String EmployeeReport(EmployeeReportVM aEmployeeReports)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            ReportItem item = new ReportItem();
            string query = "";
            string path = "";
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "";
            var critiria = "All";
            EmployeeReportGrid aEmployeeReport = aEmployeeReports.EmployeeReportPropGrid;
            ReportType = aEmployeeReport.ReportFormat;

            var comidFromEmpInfoReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                            .Where(x => x.ComId == comid &&
                                            x.HR_ReportType.ReportName == aEmployeeReport.ReportType &&
                                            !x.IsDelete).ToList();

            try
            {
                switch (aEmployeeReport.ReportType)
                {
                    case "Employee List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Employee List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpList.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptEmpList.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','AllEmployee'";

                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;

                        break;

                    case "Active Employee List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Active Employee List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpList.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpList.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','Active'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                    case "Inactive Employee List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Inactive Employee List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListReleased.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListReleased.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','InActive'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                    case "New Joining Employee List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("New Joining Employee List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListJoin.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListJoin.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','NewJoining'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                    case "New Joining Employee List-All":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("New Joining Employee List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListJoin.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListJoin.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid + "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','NewJoining-All'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                    case "Released Employee List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Released Employee List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListReleased.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListReleased.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','Released'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "Transport Users List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Transport Users List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpList_BusStoppage.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListReleased.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid + "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','Transport'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "Retirement List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Retirement List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListRetirement.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListRetirement.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','Retirement'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "PRL List":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PRL List".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListPRL.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListPRL.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','PRL'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "PFEntitle":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PFEntitle".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListPF.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListPF.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','PFEntitle'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    //case "ProEntitle":
                    //    ReportPath = "/ReportViewer/HR/rptIPromotionEntitle.rdlc";
                    //    SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                    //"" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                    //+ aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                    //+ aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                    //+ aEmployeeReport.EmpTypeId + ", '" + critiria + "','ProEntitle'";
                    //    ConstrName = "ApplicationServices";
                    //    ReportType = aEmployeeReport.Format;
                    //    

                    //    break;

                    //case "IncrEntitle":
                    //    ReportPath = "/ReportViewer/HR/rptIncrEntitle.rdlc";
                    //    SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                    //"" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                    //+ aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                    //+ aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                    //+ aEmployeeReport.EmpTypeId + ", '" + critiria + "','ImctEntitle'";
                    //    ConstrName = "ApplicationServices";
                    //    ReportType = aEmployeeReport.Format;
                    //    

                    //    break;
                    case "Employee List with Picture":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Employee List With Picture".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptEmpListPicture.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptEmpListPicture.rdlc";
                        SqlCmd = "Exec HR_RptEmployee'" + comid + "','" + userid+ "' ,'" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','EmployeeListwithPicture'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "BCIC_Jonobol_Data":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("BCIC_Jonobol_Data".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptBCIC.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptBCIC.rdlc";
                        SqlCmd = "Exec HR_RptBCIC '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','BCIC_Jonobol_Data'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;


                    case "All_Jonobol_data":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("All_Jonobol_Data".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAllJonobol.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAllJonobol.rdlc";
                        SqlCmd = "Exec HR_AllJonobolData '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','All_Jonobol_data'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "monthly_manpower":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("monthly_manpower".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptManpower.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptManpower.rdlc";
                        SqlCmd = "Exec HR_Rpt_Manpower_monthly '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','monthly_manpower'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "manpower_worker":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("manpower_worker".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptManpowerWorker.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptManpowerWorker.rdlc";
                        SqlCmd = "Exec HR_Rptthree_month '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','manpower_worker'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "manpower_Staff":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("manpower_Staff".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptManpowerStaff.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptManpowerStaff.rdlc";
                        SqlCmd = "Exec HR_Rptthree_month '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','manpower_Staff'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "manpower_Officer":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("manpower_Officer".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptManpowerOfficer.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptManpowerOfficer.rdlc";
                        SqlCmd = "Exec HR_Rptthree_month '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','manpower_Officer'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                    case "List_of_Casual_worker":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("List_of_Casual_worker".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptCasuals_employee.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptCasuals_employee.rdlc";
                        SqlCmd = "Exec HR_empinfo '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','List_of_Casual_worker'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "List_of_Officer":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("List_of_Officer".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptOfficer.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptOfficer.rdlc";
                        SqlCmd = "Exec HR_empinfo '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','List_of_Officer'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "List_of_Staff":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("List_of_Staff".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptStaff.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptStaff.rdlc";
                        SqlCmd = "Exec HR_empinfo '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','List_of_Staff'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "List_of_Worker":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("List_of_Worker".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptworker.rdlc";
                        }
                        ReportPath = "/ReportViewer/HR/rptworker.rdlc";
                        SqlCmd = "Exec HR_empinfo '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','List_of_Worker'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;

                    case "Manpower_monthly_report":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Manpower_monthly_report".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptManpower_monthly_report.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptManpower_monthly_report.rdlc";
                        SqlCmd = "Exec HR_Rptmonthly_manpower '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','Manpower_monthly_report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                    case "Departmentwise_monthly_report":
                        //RDLC
                        if (comidFromEmpInfoReport.Count > 0)
                        {
                            foreach (var r in comidFromEmpInfoReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Departmentwise_monthly_report".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptDepartmentwise_monthly.rdlc";
                        }
                        ReportPath = "/ReportViewer/HR/rptDepartmentwise_monthly.rdlc";
                        SqlCmd = "Exec HR_Departmentwise_monthly '" + comid + "', '" + aEmployeeReport.FromDate + "', '" + aEmployeeReport.ToDate + "'," +
                              "" + aEmployeeReport.DeptId + ", " + aEmployeeReport.DesigId + ", " + aEmployeeReport.SectId + ", " + aEmployeeReport.SubSectId + ", "
                              + aEmployeeReport.ShiftId + ", " + aEmployeeReport.UnitId + ", " + aEmployeeReport.FloorId + ", " + aEmployeeReport.LineId + ", "
                              + aEmployeeReport.GenderId + "," + aEmployeeReport.VarId + ", " + aEmployeeReport.EmpId + ", "
                              + aEmployeeReport.EmpTypeId + ", '" + critiria + "','Departmentwise_monthly_report'";
                        ConstrName = "ApplicationServices";
                        ReportType = aEmployeeReport.ReportFormat;


                        break;
                }



                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aEmployeeReport.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }



        }
        public String DailyAttendance(DailyAttendanceVM aDailyAttendance)
        {

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportItem item = new ReportItem();

            SqlCommand sqlCommand = new SqlCommand();
            TimeSpan fromTime = aDailyAttendance.From.Value.TimeOfDay;
            TimeSpan toTime = aDailyAttendance.To.Value.TimeOfDay;

            string fromTimeString = fromTime.ToString(@"hh\:mm\:ss");
            string toTimeString = toTime.ToString(@"hh\:mm\:ss");

            int Beginning = (int)aDailyAttendance.Beginning;
            int End = (int)aDailyAttendance.End;

            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            try
            {
                DailyAttendanceGrid aDailyAttend = aDailyAttendance.DailyAttendancePropGrid;
                var comidFromAttendanceReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                            .Where(x => x.ComId == comid &&
                                            x.HR_ReportType.ReportName == aDailyAttend.ReportType &&
                                            x.ReportType == "Daily Attendance Report" &&
                                            !x.IsDelete).ToList();

                ReportType = aDailyAttend.ReportFormat;
                var Type = aDailyAttend.ReportType.ToLower();
                switch (Type)
                {
                    // --------------Buyer Start----------------------------------------------------------------//
                    case "attendance sheet":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Attendance Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        SqlCmd = "Exec HR_RptAttendB '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                              "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                              aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'attendance', '=ALL=',' =ALL='";

                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;
                        break;

                    case "overtime sheet":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Overtime sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendOvertime.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendOvertime.rdlc";
                        SqlCmd = "Exec HR_RptAttendB '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                             "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                             aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'overtime', '=ALL=',' =ALL='";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;
                    // --------------Buyer End----------------------------------------------------------------//

                    case "attendance":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Attendance".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        }
                        
                        //ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'attendance', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;
                        break;

                    //Time Range kamrul 
                    case "time range":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("time range".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        SqlCmd = "Exec HR_rptAttendTime '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'attendance', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;
                        break;

                    case "present":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Present".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'present', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "absent":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Absent".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendAbsent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendAbsent.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'absent', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;
//New add 6-mar-2024
					case "absent section wise":
						//RDLC
						if (comidFromAttendanceReport.Count > 0)
						{
							foreach (var r in comidFromAttendanceReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("absent section wise".ToLower()))
								{
									ReportPath = $"/ReportViewer/HR/{r.ReportName}";
								}
							} //foreach
						}
						else
						{
							ReportPath = "/ReportViewer/HR/rptAttendAbsentSectionWise.rdlc";
						}

						//ReportPath = "/ReportViewer/HR/rptAttendAbsent.rdlc";
						SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'absent section wise', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
						ConstrName = "ApplicationServices";
						//ReportType = "PDF";
						ReportType = aDailyAttend.ReportFormat;

						break;

					case "late":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Late".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        SqlCmd = "Exec HR_RptAttend  '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'late', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "missing out time":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Missing Out Time".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendMissing.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendMissing.rdlc";
                        SqlCmd = "Exec HR_RptAttend  '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'missing', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "overtime":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Overtime".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendOvertime.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendOvertime.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'overtime', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "in-out":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("In-Out".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendInOut.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendInOut.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'inout', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

					case "off day":
						//RDLC
						if (comidFromAttendanceReport.Count > 0)
						{
							foreach (var r in comidFromAttendanceReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("off day".ToLower()))
								{
									ReportPath = $"/ReportViewer/HR/{r.ReportName}";
								}
							} //foreach
						}
						else
						{
							ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
						}

						//ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
						SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'off day', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
						ConstrName = "ApplicationServices";
						//ReportType = "PDF";
						ReportType = aDailyAttend.ReportFormat;

						break;

					case "movement":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Movement".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptDailyMovement.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptDailyMovement.rdlc";
                        SqlCmd = "Exec HR_RptDailyMovement '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                             "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                             aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'movement', '=ALL=',' =ALL='";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "summary":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("summary".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSum.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Department'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;
                        break;

                    case "summary (sect)":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("summary (sect)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSumSec.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Section'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "summary (line)":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("summary (line)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSumLine.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Line'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "summary (desig)":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("summary (desig)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSumDesig.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Designation'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;


                    case "summary (sect_line)":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("summary (sect_line)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSumSecLine.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Sect_Line'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;


                    case "summary (desig_line)":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("summary (desig_line)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSumDesigLine.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Designation_Line'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;


                    case "leave":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Leave".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendLeave.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendLeave.rdlc";
                        SqlCmd = "Exec HR_RptAttend'" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'leave', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "continuous absent":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Continuous Absent".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendAbsentCont.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendAbsentCont.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'conabsent', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "manual attendance":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Manual Attendance".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendFixed.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendFixed.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'manual', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";                        
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "holiday":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Holiday".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                              "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                              aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'holiday', '=ALL=',' =ALL='";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "night":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Night".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        SqlCmd = "Exec HR_RptAttend'" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'night', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "over night":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Over Night".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        SqlCmd = "Exec HR_RptAttend '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'overnight', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "summary line":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Line".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendLate.rdlc";
                        SqlCmd = "Exec HR_RptAttendSum '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "', " + aDailyAttend.ShiftId + "," +
                                "'" + aDailyAttend.EmptypeId + "', 'Line','" + aDailyAttend.LineId + "'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "all attendance":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("All Attendance".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendAll.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendAll.rdlc";
                        SqlCmd = "Exec HR_rptAttendAll '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                                 "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                                 aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'attendance', '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;


                    case "daily costing":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Daily Costing".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendDailyCost.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendDailyCost.rdlc";
                        SqlCmd = "Exec HR_RptDailyCosting '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                             "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                             aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'dailycosting',  '=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;

                    case "daily costing summary":

                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Daily Costing Summary".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptDailyCostSum.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptDailyCostSum.rdlc";
                        SqlCmd = "Exec HR_RptDailyCosting '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                             "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                             aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'dailycostingsummary','=ALL=',' =ALL=', '" +
                                 fromTime + "', '" + toTime + "', " + Beginning + ", " + End + "";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;
                    ///19-dec-2023                    
                    case "absent present":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Absent Present".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendAbsentPresent.rdlc";
                        }

                        SqlCmd = "Exec Hr_rptAttendAbtnPrntFinal '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                             "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                             aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Absent Present', '=ALL=',' =ALL='";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                    break;
                    case "off day present":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Off Day Present".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendWHPresent.rdlc";
                        }

                        //ReportPath = "/ReportViewer/HR/rptAttendPresent.rdlc";
                        SqlCmd = "Exec HR_rptAttendWHPresent '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                             "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                             aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Off Day Present', '=ALL=',' =ALL='";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;

                        break;
                    case "contingency labor summary":
                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("contingency labor summary".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSum_Shift.rdlc";
                        }
                        //ReportPath = "/ReportViewer/HR/rptAttendDailyCost.rdlc";
                        SqlCmd = "Exec HR_RptAttendSumShift '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Department'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;
                        break;

                    case "Daily Attendance Report & Contingency Labor Summary":

                        //RDLC
                        if (comidFromAttendanceReport.Count > 0)
                        {
                            foreach (var r in comidFromAttendanceReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary(Shift)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/HR/rptAttendSum_Shift.rdlc";
                        }

                        SqlCmd = "Exec HR_RptAttendSumShift '" + comid + "', '" + aDailyAttend.FromDate + "', '" + aDailyAttend.ToDate + "','" + aDailyAttend.EmpId + "','" +
                         "" + aDailyAttend.ShiftId + "', '" + aDailyAttend.DesigId + "', '" + aDailyAttend.DeptId + "', '" + aDailyAttend.SectId + "','" +
                         aDailyAttend.SubSectId + "','" + aDailyAttend.EmptypeId + "', '" + aDailyAttend.LineId + "', '" + aDailyAttend.UnitId + "','" + aDailyAttend.FloorId + "', 'Department'";
                        ConstrName = "ApplicationServices";
                        //ReportType = "PDF";
                        ReportType = aDailyAttend.ReportFormat;
                        break;
                }

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aDailyAttend.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

            if (ReportType is null)
            {
                ReportType = "PDF";
            }
        }
    }
}
