using GTERP.Interfaces.Technicals;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Technicals
{
    public class TechnicalRepository:BaseRepository<Technical>,ITechnicalRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<TechnicalRepository> _logger;


        public TechnicalRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<TechnicalRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public void GetReport(string reportType, DateTime fromDate, DateTime toDate, string rptFormat)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");
            var reportname = "";
            var filename = "";
            string redirectUrl = "";

            if (reportType == "Meeting")
            {
                reportname = "Meeting";
                filename = "rptMeeting_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "Import")
            {
                reportname = "Import";
                filename = "rptImport_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "Waste")
            {
                reportname = "Waste";
                filename = "rptWaste_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "License")
            {
                reportname = "License";
                filename = "rptLicense_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "Fire")
            {
                reportname = "Fire";
                filename = "rptFire_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "Extinguisher")
            {
                reportname = "Extinguisher";
                filename = "rptExtinguisher_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "Training")
            {
                reportname = "Training";
                filename = "rptTraining_List_" + DateTime.Now.Date.ToString();
                var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            else if (reportType == "Visit")
            {
                reportname = "Visit";
                filename = "rptVisit_List_" + DateTime.Now.Date.ToString(); var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }

            string DataSourceName = "DataSet1";
            GTERP.Models.Common.clsReport.strReportPathMain = _httpcontext.HttpContext.Session.GetString("ReportPath");
            GTERP.Models.Common.clsReport.strQueryMain = _httpcontext.HttpContext.Session.GetString("reportquery");
            GTERP.Models.Common.clsReport.strDSNMain = DataSourceName;
        }

        public IEnumerable<SelectListItem> MeetingId()
        {
            return new SelectList(_context.Cat_Meeting, "MeetingId", "Meeting");
        }

        public IEnumerable<SelectListItem> MeetingId1(int? id)
        {
            var Technical = _context.Technical.Find(id);
            return new SelectList(_context.Cat_Meeting, "MeetingId", "Meeting", Technical.MeetingId);
        }

        public IEnumerable<SelectListItem> MeetingType()
        {
            return new SelectList(_context.Cat_Variable.Where(c => c.VarType.Equals("Technical")), "VarName", "VarName");
        }

        public IEnumerable<SelectListItem> MeetingType1(int? id)
        {
            var Technical = _context.Technical.Find(id);
            return new SelectList(_context.Cat_Variable.Where(c => c.VarType.Equals("Technical")), "VarName", "VarName", Technical.MeetingType);
        }
    }
}
