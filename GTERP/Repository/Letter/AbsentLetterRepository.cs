using GTERP.Interfaces.Letter;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Letter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static GTERP.Controllers.EcommerceController;

namespace GTERP.Repository.Letter
{
    public class AbsentLetterRepository : IAbsentLetterRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<AbsentLetterRepository> _logger;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public AbsentLetterRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            ILogger<AbsentLetterRepository> logger
            )
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _logger = logger;
        }

        public string AllLetter(AllLetter AllLettermodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var subReport = new SubReport();

            ReportItem item = new ReportItem();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");
            AllLetterGrid aAllLetter = AllLettermodel.AllLetterPropGrid;
            aAllLetter.FromDate = AllLettermodel.FromDate;
            aAllLetter.ToDate = AllLettermodel.ToDate;


            var comidFromLetterReport = _context.HR_CustomReport
                                       .Include(x => x.HR_ReportType)
                                       .Where(x => x.ComId == comid &&
                                       x.HR_ReportType.ReportName == aAllLetter.ReportType &&
                                       !x.IsDelete).ToList();

            ReportType = aAllLetter.ReportFormat;
            try
            {
                switch (aAllLetter.ReportType)
                {
                    case "Appointment Letter":

                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "', '" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Appointment Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentStaff.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentStaff.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Worker")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentWorker.rdlc";
                            }
                            else
                             {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentWorker.rdlc";
                            }
                            
                        }
                                  
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    
                    case "Appointment Letter_English":

                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Appointment Letter_English".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentEng.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentEng.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Worker")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentEng.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptAppointmentEng.rdlc";
                            }

                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Job application letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Job application letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptJobApplication.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptJobApplication.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptJobApplication.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Joining Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Joining letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptJoiningLetter.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptJoiningLetter.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptJoiningLetter.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    //start
                    case "Letter of Joining":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Joining letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptLetterJoining.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptLetterJoining.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptLetterJoining.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    //end

                    case "Nominee Form":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Nominee Form".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptNomineeForm.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptNomineeForm.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptNomineeForm.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Age verification":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Age verification".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAgeVerification.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAgeVerification.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptAgeVerification.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Age & Fitness Form":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Age & Fitness Form".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptDoctorCheckUp.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptDoctorCheckUp.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptDoctorCheckUp.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Aggrement Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Age verification".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAggrementLetter.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAggrementLetter.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptAggrementLetter.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "InterviewEvaluationForm":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("InterviewEvaluationForm".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptInterviewEvaluation.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptInterviewEvaluation.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptInterviewEvaluation.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Confirmation Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Confirmation Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptConfirmation.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptConfirmation.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptConfirmation.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Confirmation Letter_English":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Confirmation Letter_English".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptConfirmationEng.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptConfirmationEng.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptConfirmationEng.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Increment Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrementStaff.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrementStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrementWorker.rdlc";
                            }
                        }


                       
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Increment Letter_English":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment Letter_English".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrementEng.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrementEng.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrementEng.rdlc";
                            }
                        }



                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Increment With Conformation Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
             if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment With Conformation Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrConfirmation_AFL.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrConfirmation_AFL.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptIncrConfirmation_AFL.rdlc";
                            }
                        }



                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Promotion Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Promotion Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotion.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotion.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotion.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Promotion Letter_English":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Promotion Letter_English".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionEng.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionEng.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionEng.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Increment With Promotion Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment With Promotion Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionIncrStaff.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionIncrStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionIncrWorker.rdlc";
                            }
                        }
                                                              
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Increment With Promotion Letter_English":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Increment With Promotion Letter_English".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionIncrEng.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionIncrEng.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptPromotionIncrEng.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Special Increment":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Special Increment".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSpecialIncrement.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSpecialIncrement.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptSpecialIncrement.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Adjustment Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Adjustment Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAdjustment.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAdjustment.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptAdjustment.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "InterviewForm":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("InterviewForm".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptInterviewForm.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptInterviewForm.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptInterviewForm.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Dismiss Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Dismiss Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendDismiss.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendDismiss.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendDismiss.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Termination Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Termination Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendTermination.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendTermination.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendTermination.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Temporary Suspand Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Temporary Suspand Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendTemporary.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendTemporary.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptSuspendTemporary.rdlc";
                            }
                        }
                       
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    //warning letter
                    case "Warning Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Warning Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptWarningNegligence.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptWarningNegligence.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptWarningNegligence.rdlc";
                            }
                        }
                       
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Showcause Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Showcause Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptNoticeShowcause.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptNoticeShowcause.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptNoticeShowcause.rdlc";
                            }
                        }
                        
                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    ////Add New 1
                    case "Transfer Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Transfer Letter".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptTransferLetter.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptTransferLetter.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptTransferLetter.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Salary Certificate":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Certificate".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSalaryCertificate.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSalaryCertificate.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptSalaryCertificate.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "NOC":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("NOC".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptNOC.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptNOC.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptNOC.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;

                    case "Worker Register":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Worker Register".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptWorkerRegister.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptWorkerRegister.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptWorkerRegister.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    ////Add New 4
                    case "Leave Register":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Leave Register".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptLeaveRegister.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptLeaveRegister.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptLeaveRegister.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    ////Add New 5
                    case "Age Certificate":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Age Certificate".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAge&MedicalCertificate.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptAge&MedicalCertificate.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptAge&MedicalCertificate.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    ////Add New 5
                    case "Extension Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extension Letter".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                           ReportPath = "/ReportViewer/Letter/rptExtensionLetter.rdlc";
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    //4-may-23 Nurjahan Add 1
                    case "Evaluation Form":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Evaluation Form".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptEvaluationStaff.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptEvaluationStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptEvaluationWorker.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "Distribution of Tools list":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Distribution of Tools list".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptToolDistributionList.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptToolDistributionList.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptToolDistributionList.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "Service Book":

                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Service Book".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Letter/rptServiceBook.rdlc";
                        }

                        //ReportPath = "/ReportViewer/Letter/rptServiceBook.rdlc";

                        subReport.strDSNSub = "DataSet1";
                        subReport.strRFNSub = "EmpId";
                        subReport.strQuerySub = "Exec [Letter_rptServiceBookIncLog] '" + aAllLetter.EmpId + "','" + comid + "'";
                        subReport.strRptPathSub = "rptServiceBook2";
                        postData.Add(2, subReport);

                        subReport = new SubReport();
                        subReport.strDSNSub = "DataSet1";
                        subReport.strRFNSub = "EmpId";
                        subReport.strQuerySub = "Exec [Letter_rptServiceBookEL] '" + aAllLetter.EmpId + "','" + comid + "'";
                        subReport.strRptPathSub = "rptServiceBook3";
                        postData.Add(3, subReport);

                        subReport = new SubReport();
                        subReport.strDSNSub = "DataSet1";
                        subReport.strRFNSub = "EmpId";
                        subReport.strQuerySub = "Exec [Letter_rptServiceBookBehave] '" + aAllLetter.EmpId + "','" + comid + "'";
                        subReport.strRptPathSub = "rptServiceBook4";
                        postData.Add(4, subReport);

                        _httpContext.HttpContext.Session.SetObject("rptList", postData);
                        break;
                    //Add 04-Oct-2022 1
                    case "Designation Transfer":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Designation Transfer".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptDesignationWiseTransfer.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptDesignationWiseTransfer.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptDesignationWiseTransfer.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    //Add 04-Oct-2022 2
                    case "Department Transfer":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Department Transfer".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptDepartmentWiseTransfer.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptDepartmentWiseTransfer.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptDepartmentWiseTransfer.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    //Add 04-Oct-2022 3
                    case "Section Transfer":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Section Transfer".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSectionWiseTransfer.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptSectionWiseTransfer.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptSectionWiseTransfer.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    //Add 04-Oct-2022 4
                    case "EmpStatus Transfer":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("EmpStatus Transfer".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptEmpTypeWiseTransfer.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptEmpTypeWiseTransfer.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptEmpTypeWiseTransfer.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "Envelope":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Envelope".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptLeaveEnvelope.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptLeaveEnvelope.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptLeaveEnvelope.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "Background Check":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Background Check".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptBackgroundCheck.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptBackgroundCheck.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptBackgroundCheck.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "Application From of Service":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Application From of Service".ToLower()))
                                {
                                    if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aAllLetter.EmpType == "Officer")
                            {
                                ReportPath = "/ReportViewer/Letter/rptApplicationFromofService.rdlc";
                            }
                            else if (aAllLetter.EmpType == "Staff")
                            {
                                ReportPath = "/ReportViewer/Letter/rptApplicationFromofService.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Letter/rptApplicationFromofService.rdlc";
                            }
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "Worker Register Book":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Worker Register Book".ToLower()))
                                {                                    
                                   ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Letter/rptWorkerRegisterBook.rdlc";
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                    case "PPE Letter":
                        SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
                               "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
                        //RDLC
                        if (comidFromLetterReport.Count > 0)
                        {
                            foreach (var r in comidFromLetterReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PPE Letter".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Letter/rptPPELetter.rdlc";
                        }

                        ConstrName = "ApplicationServices";
                        ReportType = aAllLetter.ReportFormat;

                        break;
                        //Add 14-mar-2024
					case "Service Length":
						SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
							   "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
						//RDLC
						if (comidFromLetterReport.Count > 0)
						{
							foreach (var r in comidFromLetterReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("ServiceLength".ToLower()))
								{
									if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
									{
										ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
										break;
									}
									else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
									{
										ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
										break;
									}
									else if (r.EmpTypeId == 1)
									{
										ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
									}
								}
							} //foreach
						}
						else
						{
							if (aAllLetter.EmpType == "Officer")
							{
								ReportPath = "/ReportViewer/Letter/rptEmpServiceLengthList.rdlc";
							}
							else if (aAllLetter.EmpType == "Staff")
							{
								ReportPath = "/ReportViewer/Letter/rptEmpServiceLengthList.rdlc";
							}
							else
							{
								ReportPath = "/ReportViewer/Letter/rptEmpServiceLengthList.rdlc";
							}
						}

						ConstrName = "ApplicationServices";
						ReportType = aAllLetter.ReportFormat;

						break;

					case "Active Service Length":
						SqlCmd = "Exec Letter_rptLetterAll'" + comid + "','" + aAllLetter.FromDate + "', '" + aAllLetter.ToDate + "','" + userid + "', '" + aAllLetter.Paymode + "','" + aAllLetter.Unit + "','" +
							   "" + aAllLetter.EmpType + "',  '" + aAllLetter.DeptId + "'," + aAllLetter.SectId + "," + aAllLetter.EmpId + ", '" + aAllLetter.LId + "','" + aAllLetter.EmpStatus + "','" + aAllLetter.ReportType + "'";
						//RDLC
						if (comidFromLetterReport.Count > 0)
						{
							foreach (var r in comidFromLetterReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("ActiveServiceLength".ToLower()))
								{
									if (aAllLetter.EmpType == "Officer" && r.EmpTypeId == 3)
									{
										ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
										break;
									}
									else if (aAllLetter.EmpType == "Staff" && r.EmpTypeId == 2)
									{
										ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
										break;
									}
									else if (r.EmpTypeId == 1)
									{
										ReportPath = $"/ReportViewer/Letter/{r.ReportName}";
									}
								}
							} //foreach
						}
						else
						{
							if (aAllLetter.EmpType == "Officer")
							{
								ReportPath = "/ReportViewer/Letter/rptEmpServiceLengthListActive.rdlc";
							}
							else if (aAllLetter.EmpType == "Staff")
							{
								ReportPath = "/ReportViewer/Letter/rptEmpServiceLengthListActive.rdlc";
							}
							else
							{
								ReportPath = "/ReportViewer/Letter/rptEmpServiceLengthListActive.rdlc";
							}
						}

						ConstrName = "ApplicationServices";
						ReportType = aAllLetter.ReportFormat;

						break;
				}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                throw ex;

            }

            _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
            _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
            string filename = aAllLetter.ReportType.ToString() + "_" + aAllLetter.EmpType + "";
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }

        public string Print(int? id, string letterType, string type = "pdf")
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            try
            {
                if (letterType == "Ist")
                {
                    var reportName = "rptAbsentFirst";
                    _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Letter/" + reportName + ".rdlc");
                }
                else if (letterType == "2nd")
                {
                    var reportName = "rptAbsentSecond";
                    _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Letter/" + reportName + ".rdlc");
                }

                else if (letterType == "Final")
                {
                    var reportName = "rptAbsentFinal";
                    _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Letter/" + reportName + ".rdlc");
                }
                else
                {
                    var reportName = "rptAbsentFirst";
                    _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Letter/" + reportName + ".rdlc");
                }

                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Letter_rptLetterAbsent] '" + comid + "','" + id + "','" + letterType + "'");

                string filename = _context.HR_Emp_Info.Where(x => x.EmpId == id).Select(x => x.EmpName).Single().ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                _httpContext.HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                SqlCmd = clsReport.strQueryMain;
                ReportPath = clsReport.strReportPathMain;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                throw ex;
            }

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });

            return callBackUrl;
        }
    }
}
