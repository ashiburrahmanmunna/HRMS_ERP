using GTERP.Models;
using GTERP.Repository.Payroll_Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using GTERP.Interfaces.Tax_Report;
using GTERP.Models.ViewModels;
using GTERP.Models.Payroll;
using System;

namespace GTERP.Repository.Tax_Report
{
    public class TaxReportRepository: ITaxReportRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<PayrollReportRepository> _logger;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        public TaxReportRepository(IHttpContextAccessor httpContext,

            GTRDBContext context,
            ILogger<PayrollReportRepository> logger,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _logger = logger;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public string ClientInfo(TaxDto taxDto)
        {
            var callBackUrl = "";
            var ReportPath = "";
            var SqlCmd = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            ReportType = taxDto.ReportFormat;

            try
            {
              
                switch (taxDto.ReportType)
                {
                    case "Client Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptClient.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;
                    case "Company Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptCompany.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;

                    case "Payment Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptPayment.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;
                    case "Tax Submitted Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptTaxSubmitted.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;
                    case "Tax Submitted Time Extension Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptTaxSubTimeExtension.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;
                    case "Tax Acknowledgment Slip Received Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptTaxAcknowledgeSlipReceive.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;
                    case "Tax Certificate Received Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptTaxCertificateReceive.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;
                    case "Date wise In-house Costing Report":
                        SqlCmd = "Exec Tax_rptTaxReport'" + comid + "', '" + taxDto.ClientId + "', '" + taxDto.FromDate + "','" + taxDto.ToDate + "','" +
                               "" + taxDto.ReportType + "','" + taxDto.FiscalYearId + "'";
                        ReportPath = "/ReportViewer/Tax/rptCosting.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = taxDto.ReportFormat;

                        break;

                }
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = taxDto.ReportType.ToString();
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
