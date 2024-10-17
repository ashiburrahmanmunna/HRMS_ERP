using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class IdCardRepository : IIdCardRepository
    {


        private readonly IHttpContextAccessor _httpContext;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        public IdCardRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public List<IdcardGreadData> loadDataByDate(string FromDate, string ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var query = $"EXEC HR_PrcGetEmployeeInfoForIdCard '{comid}','{FromDate}','{ToDate}'";

            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@dtFrom", FromDate);
            parameters[2] = new SqlParameter("@dtTo", ToDate);
            parameters[3] = new SqlParameter("@Type", "IDCardFiltered");

            List<IdcardGreadData> IdcardGreadData = Helper.ExecProcMapTList<IdcardGreadData>("HR_PrcGetEmployeeInfoForIdCard", parameters);
            return IdcardGreadData;

        }

        public List<IdcardGreadData> GetIdcard()
        {

            var comid = _httpContext.HttpContext.Session.GetString("comid");


            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@dtFrom", "");
            parameters[2] = new SqlParameter("@dtTo", "");
            parameters[3] = new SqlParameter("@Type", "IDCardAll");
            var query = $"EXEC HR_PrcGetEmployeeInfoForIdCard '{comid}','','','IDCardAll'";
            return Helper.ExecProcMapTList<IdcardGreadData>("HR_PrcGetEmployeeInfoForIdCard", parameters);

        }

        public string PrintIdcardReport(DateTime fromDate, DateTime toDate, IdCard IdCard)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportItem item = new ReportItem();
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "";
            var ReportType = "PDF";
            var ReportTypeCAT = IdCard.ViewReportCat;
            var ViewReportType = IdCard.ViewReportType;
            List<HR_CustomReport> comidFromIdCard = new List<HR_CustomReport>();

            if (ReportTypeCAT == "Bangla")
            {

                comidFromIdCard = _context.HR_CustomReport
                .Include(x => x.HR_ReportType)
                .Where(x => x.ComId == comid && x.ReportType == "ID Card" && x.ReportName.Contains("Bangla"))
                .ToList();
            }
            else
            {
                comidFromIdCard = _context.HR_CustomReport
               .Include(x => x.HR_ReportType)
               .Where(x => x.ComId == comid && x.ReportType == "ID Card" && x.ReportName.Contains("English"))
               .ToList();
            }

            if (comidFromIdCard.Count > 0)
            {
                foreach (var r in comidFromIdCard)
                {
                    if (ViewReportType == "BackSide")
                    {
                        if (ReportTypeCAT == "Bangla" && r.ReportName.Contains("Back"))
                        {
                            ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                        }
                        else
                        {
                            if (ReportTypeCAT == "English" && r.ReportName.Contains("Back"))
                            {
                                ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                            }
                        }
                    }
                    else
                    {
                        if (ReportTypeCAT == "Bangla" && r.ReportName.Contains("Front"))
                        {
                            ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                        }
                        else
                        {
                            if (ReportTypeCAT == "English" && r.ReportName.Contains("Front"))
                            {
                                ReportPath = $"/ReportViewer/HR/{r.ReportName}";
                            }
                        }
                    }                
                }
            }
            else 
            {
                if (ViewReportType == "BackSide")
                {
                    if (ReportTypeCAT == "Bangla")
                    {
                        ReportPath = "/ReportViewer/HR/rptIDCardPrintBanglaBack.rdlc";
                    }
                    else
                    {
                        ReportPath = "/ReportViewer/HR/rptIDCardPrintBack.rdlc";
                    }
                }
                 else 
                {
                    if (ReportTypeCAT == "Bangla")
                    {
                        ReportPath = "/ReportViewer/HR/rptIDCardPrintBanglaFront.rdlc";
                    }
                  
                    else if(ReportTypeCAT == "English")
                    {
                         ReportPath = "/ReportViewer/HR/rptIDCardPrintFront.rdlc";
                       
                    }
                    else if (ReportTypeCAT == "AuthorizedPersons")
                    {
                        ReportPath = "/ReportViewer/HR/rptEmpList_Authorized.rdlc";

                    }
                    else if (ReportTypeCAT == "FireFighter")
                    {
                        ReportPath = "/ReportViewer/HR/rptEmpList_fire.rdlc";

                    }
                    else 
                    {
                        ReportPath = "/ReportViewer/HR/rptEmpList_rescu.rdlc";

                    }
                 }
               
            }
            
           

                SqlCmd = "Exec HR_rptIdCardPrint '" + comid + "', '" + fromDate + "', '" + toDate + "'";

            ConstrName = "ApplicationServices";
            // ReportType = ReportType; 
            ReportType = IdCard.ViewReportAs;

            _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
            _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
            string filename = "ID Card".ToString();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }

        public void SaveIdcard(IdCard idCard)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            _context.HR_Emp_TempIdCard.RemoveRange(_context.HR_Emp_TempIdCard);
            //insert empid in temp table
            for (int i = 0; i < idCard.Employess.Count; i++)
            {
                HR_Emp_TempIdCard t = new HR_Emp_TempIdCard();
                t.ComId = comid;
                t.EmpId = idCard.Employess[i];
                _context.HR_Emp_TempIdCard.Add(t);
            }
            _context.SaveChanges();
        }


      
    }
}
