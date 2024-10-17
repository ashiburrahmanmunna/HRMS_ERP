using GTERP.Interfaces.Payroll_Report;
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
using QuickMailer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Payroll_Report
{
    public class PayrollReportRepository : IPayrollReportRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<PayrollReportRepository> _logger;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        public PayrollReportRepository(IHttpContextAccessor httpContext,

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

        //public string AdvSalary(AdvSalaryReport aAdvSalaryReport)
        //{
        //    string comid = _httpContext.HttpContext.Session.GetString("comid");

        //    ReportItem item = new ReportItem();
        //    string query = "";
        //    string path = "";
        //    var callBackUrl = "";
        //    var ReportPath = "";
        //    var SqlCmd = "";
        //    var ConstrName = "";
        //    var ReportType = "";

        //    try
        //    {
        //        switch (aAdvSalaryReport.ButtonString)
        //        {
        //            case "AdvSalarySheet":

        //                ReportPath = "/ReportViewer/Payroll/rptAdvSalarySheet.rdlc";
        //                SqlCmd = "Exec Payroll_rptFestAdvanceSheet '" + comid + "', " +
        //                      "" + aAdvSalaryReport.SectionId + ", '" + aAdvSalaryReport.LineId + "'," + aAdvSalaryReport.EmployeeId + ", '" + aAdvSalaryReport.EmployeeType + "', 'AdvSalarySheet', '" + aAdvSalaryReport.Criteria.ToLower() + "','=ALL='";
        //                ConstrName = "ApplicationServices";
        //                ReportType = "PDF";


        //                break;

        //            case "AdvSalaryPayslip":
        //                ReportPath = "/ReportViewer/Payroll/rptAdvSalarySheet.rdlc";
        //                SqlCmd = "Exec Payroll_rptFestAdvanceSheet '" + comid + "', " +
        //                      "" + aAdvSalaryReport.SectionId + ", '" + aAdvSalaryReport.LineId + "'," + aAdvSalaryReport.EmployeeId + ", '" + aAdvSalaryReport.EmployeeType + "', 'AdvSalaryPayslip', '" + aAdvSalaryReport.Criteria.ToLower() + "','=ALL='";
        //                ConstrName = "ApplicationServices";
        //                ReportType = "PDF";


        //                break;
        //            case "AdvSalaryTopSheet":

        //                ReportPath = "/ReportViewer/Payroll/rptAdvSalaryTopSheet.rdlc";
        //                SqlCmd = "Exec Payroll_rptFestAdvanceSheet '" + comid + "', " +
        //                      "" + aAdvSalaryReport.SectionId + ", '" + aAdvSalaryReport.LineId + "'," + aAdvSalaryReport.EmployeeId + ", '" + aAdvSalaryReport.EmployeeType + "', 'AdvSalaryTopSheet', '" + aAdvSalaryReport.Criteria.ToLower() + "','=ALL='";
        //                ConstrName = "ApplicationServices";
        //                ReportType = "PDF";


        //                break;
        //            case "AdvSalaryBankSheet":

        //                ReportPath = "/ReportViewer/Payroll/rptAdvSalaryBankSheet.rdlc";
        //                SqlCmd = "Exec Payroll_rptFestAdvanceSheet '" + comid + "', " +
        //                      "" + aAdvSalaryReport.SectionId + ", '" + aAdvSalaryReport.LineId + "'," + aAdvSalaryReport.EmployeeId + ", '" + aAdvSalaryReport.EmployeeType + "', 'AdvSalaryBankSheet', '" + aAdvSalaryReport.Criteria.ToLower() + "','=ALL='";
        //                ConstrName = "ApplicationServices";
        //                ReportType = "PDF";


        //                break;
        //            case "Denomination":

        //                ReportPath = "/ReportViewer/Payroll/rptDenomination.rdlc";
        //                SqlCmd = "Exec Payroll_rptFestAdvanceSheet '" + comid + "', " +
        //                      "" + aAdvSalaryReport.SectionId + ", '" + aAdvSalaryReport.LineId + "'," + aAdvSalaryReport.EmployeeId + ", '" + aAdvSalaryReport.EmployeeType + "', 'Denomination', '" + aAdvSalaryReport.Criteria.ToLower() + "','=ALL='";
        //                ConstrName = "ApplicationServices";
        //                ReportType = "PDF";


        //                break;
        //        }
        //        _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
        //        _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
        //        string filename = aAdvSalaryReport.ButtonString.ToString();
        //        _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
        //        callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
        //        return callBackUrl;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        throw ex;
        //    }


        //}

        //public string BoardPaper(BoardPaper BoardPapermodel)
        //{
        //    string SqlCmd = "";
        //    string ReportPath = "";
        //    var ConstrName = "ApplicationServices";
        //    var ReportType = "PDF";

        //    ReportItem item = new ReportItem();
        //    var comid = _httpContext.HttpContext.Session.GetString("comid");

        //    BoardPaperGrid aBoardPaper = BoardPapermodel.BoardPaperPropGrid;
        //    ReportType = aBoardPaper.ReportFormat;

        //    try
        //    {
        //        switch (aBoardPaper.ReportType)
        //        {
        //            case "ফুড এন্ড কনভেন্স ব্যয় বিবরণী":
        //                SqlCmd = "Exec Payroll_rptBoardPaper '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth + "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" + aBoardPaper.EmpStatus + "','ফুড এন্ড কনভেন্স ব্যয় বিবরণী'";
        //                ReportPath = "/ReportViewer/Payroll/rptBoardPaperFC.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "ওভারটাইম ব্যয় বিবরণী":
        //                SqlCmd = "Exec Payroll_rptBoardPaper '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "','ওভারটাইম ব্যয় বিবরণী'";
        //                ReportPath = "/ReportViewer/Payroll/rptBoardPaperOT.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;


        //            case "অতিরিক্ত শ্রমঘন্টা বিবরণী":
        //                SqlCmd = "Exec Payroll_rptBoardPaper '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" +
        //                       aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "','অতিরিক্ত শ্রমঘন্টা বিবরণী'";
        //                ReportPath = "/ReportViewer/Payroll/rptBoardPaperOTHour.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "ওভারটাইম ব্যয় বিবরণী (শাখা অনুযায়ী)":
        //                SqlCmd = "Exec Payroll_rptBoardPaper '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "','ওভারটাইম ব্যয় বিবরণী (শাখা অনুযায়ী)'";
        //                ReportPath = "/ReportViewer/Payroll/rptBoardPaperOTSectWise.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;


        //            case "অতিরিক্ত শ্রমঘন্টা বিবরণী (শাখা অনুযায়ী)":
        //                SqlCmd = "Exec Payroll_rptBoardPaper '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" +
        //                       aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "','অতিরিক্ত শ্রমঘন্টা বিবরণী (শাখা অনুযায়ী)'";
        //                ReportPath = "/ReportViewer/Payroll/rptBoardPaperOTHourSectWise.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Salary Summary Overview":
        //                SqlCmd = "Exec Payroll_rptSalaryOverview '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" +
        //                       aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "','Salary Summary Overview'";
        //                ReportPath = "/ReportViewer/Payroll/rptSalarySumOverview.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Income Tax Certificate":
        //                SqlCmd = "Exec Payroll_rptIncomeTaxCert '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" +
        //                       aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "'";
        //                ReportPath = "/ReportViewer/Payroll/rptIncomeTaxCert.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Income Tax Sheet":
        //                SqlCmd = "Exec Payroll_rptIncomeTaxSheet '" + comid + "', '" + aBoardPaper.ProssType + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                       "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" +
        //                       aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                       aBoardPaper.EmpStatus + "'";
        //                ReportPath = "/ReportViewer/Payroll/rptIncomeTax.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "ITax Statement":
        //                ReportPath = "/ReportViewer/Payroll/rptITStatement.rdlc";
        //                SqlCmd = "Exec Payroll_rptITaxStatement'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','ITax Statement'";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "ITax Summary":
        //                ReportPath = "/ReportViewer/Payroll/rptITSummary.rdlc";
        //                SqlCmd = "Exec Payroll_rptITaxStatement'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','ITax Summary'";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "MotorCycle Loan Letter":
        //                SqlCmd = "Exec HR_rptLoanLetter '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth + "','" + aBoardPaper.EmpTypeId + "', " +
        //                      "" + aBoardPaper.SectId + ", " + aBoardPaper.EmpId + ",  'MotorCycle Loan Letter'";
        //                ReportPath = "/ReportViewer/Payroll/rptRecoveryMCLoan.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Welfare Loan Letter ":
        //                SqlCmd = "Exec HR_rptLoanLetter '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth + "','" + aBoardPaper.EmpTypeId + "', " +
        //                      "" + aBoardPaper.SectId + ", " + aBoardPaper.EmpId + ",  'Welfare Loan Letter'";
        //                ReportPath = "/ReportViewer/Payroll/rptRecoveryWFLoan.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "House Building Loan Letter":
        //                SqlCmd = "Exec HR_rptLoanLetter '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth + "','" + aBoardPaper.EmpTypeId + "', " +
        //                      "" + aBoardPaper.SectId + ", " + aBoardPaper.EmpId + ",  'House Building Loan Letter'";
        //                ReportPath = "/ReportViewer/Payroll/rptRecoveryHBLoan.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Loan Letter":
        //                SqlCmd = "Exec HR_rptLoanLetter '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth + "','" + aBoardPaper.EmpTypeId + "', " +
        //                      "" + aBoardPaper.SectId + ", " + aBoardPaper.EmpId + ",  'PF Loan Letter'";
        //                ReportPath = "/ReportViewer/Payroll/rptRecoveryPFLoan.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Other Loan Letter":
        //                SqlCmd = "Exec HR_rptLoanLetter '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth + "','" + aBoardPaper.EmpTypeId + "', " +
        //                      "" + aBoardPaper.SectId + ", " + aBoardPaper.EmpId + ",  'Other Loan Letter'";
        //                ReportPath = "/ReportViewer/Payroll/rptRecoveryOtherLoan.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Statement Of Member's Account (Total))":
        //                SqlCmd = "Exec Payroll_rptPFIndividual'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','PF Individual Sheet'";
        //                ReportPath = "/ReportViewer/Payroll/rptPFYearlyIndividualSheet.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Member PF Fund Transfer to O.P.":
        //                SqlCmd = "Exec Payroll_rptPFIndividual'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','Member PF Fund Transfer to O.P.'";
        //                ReportPath = "/ReportViewer/Payroll/rptPFFundTransferSheet.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Members Ledger":
        //                SqlCmd = "Exec Payroll_rptPFMLedger'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','PF Members Ledger'";

        //                ReportPath = "/ReportViewer/Payroll/rptPFMembersLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;


        //            case "PF Individual Loan Statement":
        //                SqlCmd = "Exec Payroll_rptPFStatement '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','PF Individual Loan Statement'";
        //                ReportPath = "/ReportViewer/Payroll/rptPFStatement.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Individual Ledger":
        //                SqlCmd = "Exec Payroll_rptPFIndividual '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','PF Individual Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptSalaryPFIndLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Final Settlement":
        //                SqlCmd = "Exec Payroll_rptPFIndividual'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','PF Final Settlement'";
        //                ReportPath = "/ReportViewer/Payroll/rptPFFinalSettlement.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Loan Ledger":
        //                SqlCmd = "Exec Payroll_rptPFLoanLedger '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','PF Loan Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptPFLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "HB Loan Individual Ledger":
        //                SqlCmd = "Exec Payroll_rptPFIndividual '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','HB Loan Individual Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptIndLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "HB Loan Ledger":
        //                SqlCmd = "Exec Payroll_rptHBLoanLedger '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','HB Loan Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptHBLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "MC Loan Individual Ledger":
        //                SqlCmd = "Exec Payroll_rptPFIndividual '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','MC Loan Individual Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptIndLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "MC Loan Ledger":
        //                SqlCmd = "Exec Payroll_rptMCLoanLedger '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','MC Loan Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptMCLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "PF Loan Individual Ledger":
        //                SqlCmd = "Exec Payroll_rptPFIndividual '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','PF Loan Individual Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptIndLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "WF Loan Individual Ledger":
        //                SqlCmd = "Exec Payroll_rptPFIndividual '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','WF Loan Individual Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptIndLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "WF Loan Ledger":
        //                SqlCmd = "Exec Payroll_rptWFLoanLedger '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','WF Loan Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptWFLoanLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Gratuity Ledger":
        //                SqlCmd = "Exec Payroll_rptGratuityIndividual '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','Gratuity Ledger'";
        //                ReportPath = "/ReportViewer/Payroll/rptIndGratuityLedger.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;
        //            case "Member Gratuity Fund Transfer to O.P.":
        //                SqlCmd = "Exec Payroll_rptPFIndividual'" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                        "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "', '" + aBoardPaper.EmpTypeId + "','" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                        aBoardPaper.EmpStatus + "','Member Gratuity Fund Transfer to O.P.'";
        //                ReportPath = "/ReportViewer/Payroll/rptGratuityFundTransferSheet.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Arrear Wages / Salary Bill":
        //                SqlCmd = "Exec Payroll_rptArriarBill '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','Arrear Wages / Salary Bill'";
        //                ReportPath = "/ReportViewer/Payroll/rptArrearBill.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;

        //            case "Arrear Wages / Salary Bank Advice":
        //                SqlCmd = "Exec Payroll_rptArriarBill '" + comid + "', '" + BoardPapermodel.FromMonth + "', '" + BoardPapermodel.ToMonth +
        //                      "', '" + aBoardPaper.Paymode + "','" + aBoardPaper.Unit + "','" + aBoardPaper.EmpTypeId + "',  '" + aBoardPaper.DeptId + "','" + aBoardPaper.SectId + "'," + aBoardPaper.EmpId + ", '" + aBoardPaper.LId + "','" +
        //                      aBoardPaper.EmpStatus + "','Arrear Wages / Salary Bank Advice'";
        //                ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
        //                ConstrName = "ApplicationServices";
        //                ReportType = aBoardPaper.ReportFormat;

        //                break;
        //        }
        //        _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
        //        _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
        //        string filename = aBoardPaper.ReportType.ToString();
        //        _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
        //        string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
        //        return callBackUrl;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        throw ex;
        //    }


        //}

        public string EarnLeaveSheet(EarnLeaveSheet earnLeaveSheet)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            var comid = _httpContext.HttpContext.Session.GetString("comid");

            EarnLeaveSheetGrid aEarnLeaveSheet = earnLeaveSheet.EarnLeaveSheetPropGrid;
            ReportType = aEarnLeaveSheet.ReportFormat;

            var comidFromEarnLeaveSheet = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.ReportType == "EarnLvSheet Report" &&
                                        x.HR_ReportType.ReportName == aEarnLeaveSheet.ReportType &&
                                        !x.IsDelete).ToList();


            try
            {
                switch (aEarnLeaveSheet.ReportType)
                {
                    case "Earn Leave":
                        SqlCmd = "Exec Payroll_rptEarnLeaveEncashment '" + comid + "', '" + aEarnLeaveSheet.ProssType + "', '" + aEarnLeaveSheet.Paymode + "','" + aEarnLeaveSheet.UnitId + "','" +
                               "" + aEarnLeaveSheet.EmpTypeId + "',  '" + aEarnLeaveSheet.DeptId + "',  '" + aEarnLeaveSheet.DesigId + "',"
                               + aEarnLeaveSheet.SectId + ",'" + aEarnLeaveSheet.SubSectId + "'," + aEarnLeaveSheet.EmpId + ", '" + aEarnLeaveSheet.FloorId + "','" + aEarnLeaveSheet.LineId + "','" + aEarnLeaveSheet.EmpStatus + "','Earn Leave','" + aEarnLeaveSheet.BankId + "'";
                        //RDLC
                        if (comidFromEarnLeaveSheet.Count > 0)
                        {
                            foreach (var r in comidFromEarnLeaveSheet)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Earn Leave".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptEarnLeave.rdlc";
                        }
                        
                        break;

                    case "Earn Leave Payslip":
                        SqlCmd = "Exec Payroll_rptEarnLeaveEncashment '" + comid + "', '" + aEarnLeaveSheet.ProssType + "', '" + aEarnLeaveSheet.Paymode + "','" + aEarnLeaveSheet.UnitId + "','" +
                               "" + aEarnLeaveSheet.EmpTypeId + "',  '" + aEarnLeaveSheet.DeptId + "',  '" + aEarnLeaveSheet.DesigId + "',"
                               + aEarnLeaveSheet.SectId + ",'" + aEarnLeaveSheet.SubSectId + "'," + aEarnLeaveSheet.EmpId + ", '" + aEarnLeaveSheet.FloorId + "','" + aEarnLeaveSheet.LineId + "','" + aEarnLeaveSheet.EmpStatus + "','Earn Leave Payslip','" + aEarnLeaveSheet.BankId + "'";
                        //RDLC
                        if (comidFromEarnLeaveSheet.Count > 0)
                        {
                            foreach (var r in comidFromEarnLeaveSheet)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Earn Leave Payslip".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptPaySlipEarnLeave.rdlc";
                        }
                        
                        break;


                    case "Earn Leave Summary Sheet":
                        SqlCmd = "Exec Payroll_rptEarnLeaveEncashment '" + comid + "', '" + aEarnLeaveSheet.ProssType + "', '" + aEarnLeaveSheet.Paymode + "','" + aEarnLeaveSheet.UnitId + "','" +
                               "" + aEarnLeaveSheet.EmpTypeId + "',  '" + aEarnLeaveSheet.DeptId + "',  '" + aEarnLeaveSheet.DesigId + "',"
                               + aEarnLeaveSheet.SectId + ",'" + aEarnLeaveSheet.SubSectId + "'," + aEarnLeaveSheet.EmpId + ", '" + aEarnLeaveSheet.FloorId + "','" + aEarnLeaveSheet.LineId + "','" + aEarnLeaveSheet.EmpStatus + "','Earn Leave Summary Sheet','" + aEarnLeaveSheet.BankId + "'";
                        //RDLC
                        if (comidFromEarnLeaveSheet.Count > 0)
                        {
                            foreach (var r in comidFromEarnLeaveSheet)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Earn Leave Summary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptEarnLeaveSum.rdlc";
                        }
                        
                        break;

                    case "Earn Leave Details":
                        SqlCmd = "Exec Payroll_rptEarnLeaveEncashment '" + comid + "', '" + aEarnLeaveSheet.ProssType + "', '" + aEarnLeaveSheet.Paymode + "'," + aEarnLeaveSheet.UnitId + ",'" +
                               "" + aEarnLeaveSheet.EmpTypeId + "',  " + aEarnLeaveSheet.DeptId + ",  " + aEarnLeaveSheet.DesigId + ","
                               + aEarnLeaveSheet.SectId + "," + aEarnLeaveSheet.SubSectId + "," + aEarnLeaveSheet.EmpId + "," + aEarnLeaveSheet.FloorId + "," + aEarnLeaveSheet.LineId + ",'" + aEarnLeaveSheet.EmpStatus + "','Earn Leave Details','" + aEarnLeaveSheet.BankId + "'";
                        //RDLC
                        if (comidFromEarnLeaveSheet.Count > 0)
                        {
                            foreach (var r in comidFromEarnLeaveSheet)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Earn Leave Details".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptEarnleaveDetails.rdlc";
                        }
                        
                        break;
                    case "Earn Leave Bank Sheet":
                        SqlCmd = "Exec Payroll_rptEarnLeaveEncashment '" + comid + "', '" + aEarnLeaveSheet.ProssType + "', '" + aEarnLeaveSheet.Paymode + "'," + aEarnLeaveSheet.UnitId + ",'" +
                               "" + aEarnLeaveSheet.EmpTypeId + "',  " + aEarnLeaveSheet.DeptId + ",  " + aEarnLeaveSheet.DesigId + ","
                               + aEarnLeaveSheet.SectId + "," + aEarnLeaveSheet.SubSectId + "," + aEarnLeaveSheet.EmpId + "," + aEarnLeaveSheet.FloorId + "," + aEarnLeaveSheet.LineId + ",'" + aEarnLeaveSheet.EmpStatus + "','Earn Leave Bank Sheet','" + aEarnLeaveSheet.BankId + "'";
                        //RDLC
                        if (comidFromEarnLeaveSheet.Count > 0)
                        {
                            foreach (var r in comidFromEarnLeaveSheet)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Earn Leave Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        
                        break;

                    case "Earn Leave Mobile Bank Sheet":
                        SqlCmd = "Exec Payroll_rptEarnLeaveEncashment '" + comid + "', '" + aEarnLeaveSheet.ProssType + "', '" + aEarnLeaveSheet.Paymode + "'," + aEarnLeaveSheet.UnitId + ",'" +
                               "" + aEarnLeaveSheet.EmpTypeId + "',  " + aEarnLeaveSheet.DeptId + ",  " + aEarnLeaveSheet.DesigId + ","
                               + aEarnLeaveSheet.SectId + "," + aEarnLeaveSheet.SubSectId + "," + aEarnLeaveSheet.EmpId + "," + aEarnLeaveSheet.FloorId + "," + aEarnLeaveSheet.LineId + ",'" + aEarnLeaveSheet.EmpStatus + "','Earn Leave Mobile Bank Sheet','" + aEarnLeaveSheet.BankId + "'";
                        //RDLC
                        if (comidFromEarnLeaveSheet.Count > 0)
                        {
                            foreach (var r in comidFromEarnLeaveSheet)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Earn Leave Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        
                        break;

                }
                ConstrName = "ApplicationServices";
                ReportType = aEarnLeaveSheet.ReportFormat;
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aEarnLeaveSheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public string ExtraOTSheet(SalarySheet ExtraOTSheet)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SalarySheetGrid aSalarySheet = ExtraOTSheet.SalarySheetPropGrid;
            ReportType = aSalarySheet.ReportFormat;

            var comidFromSalaryReportB = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.HR_ReportType.ReportName == aSalarySheet.ReportType &&
                                        !x.IsDelete).ToList();

            try
            {
                //kamrul change perameter
                switch (aSalarySheet.ReportType)
                {
                    case "Extra OT Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT Sheet".ToLower()))
                                {
                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptExtraOTSheet.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptExtraOTSheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptExtraOTSheet.rdlc";
                            }
                        }
                        break;

                    case "Extra OT Summary Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSum '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptExtraOTSummarySheet.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptExtraOTSummarySheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptExtraOTSummarySheet.rdlc";
                            }
                        }
                        break;

                    case "Extra OT Bank Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Sheet', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;
                    //3-jan-24 1
                    case "Extra OT With Tiffin":
                        SqlCmd = "Exec Payroll_rptExtraOTSheetTiffin '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                              "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                              + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT With Tiffin".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalarySheetTiffin.rdlc";
                        }
                        break;
                        //2
                    case "After Four Hour OT":
                        SqlCmd = "Exec Payroll_rptExtraOTSheetAfter4Hour '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                              "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                              + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("After Four Hour OT".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOtall.rdlc";
                        }
                        break;
                        //3
                    case "WHOT With Tiffin":
                        SqlCmd = "Exec Payroll_rptExtraOTSheetWHOT '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                              "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                              + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("WHOT With Tiffin".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOTWH.rdlc";
                        }
                        break;
                    //Sum1
                    case "Extra OT WithTiffin Summary Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSumTiffin '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT WithTiffin Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetTiffinSummarySheet.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetTiffinSummarySheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetTiffinSummarySheet.rdlc";
                            }
                        }
                        break;
                    //Sum2
                    case "After Four Hour OT Summary Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSumAfter4Hour '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("After Four Hour OT Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOtallSummarySheet.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOtallSummarySheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOtallSummarySheet.rdlc";
                            }
                        }
                        break;
                    //Sum3
                    case "WH WithTiffin OT Summary Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSumWHOT '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "', '" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("WH WithTiffin OT Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOTWHSummarySheet.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOTWHSummarySheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetEOTWHSummarySheet.rdlc";
                            }
                        }
                        break;
                }
                ConstrName = "ApplicationServices";
                ReportType = aSalarySheet.ReportFormat;
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aSalarySheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        public string FestBonus(FestivalBonus FestivalBonusmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            var comid = _httpContext.HttpContext.Session.GetString("comid");

            FestivalBonusGrid aFestBonus = FestivalBonusmodel.FestivalBonusPropGrid;

            var comidFromFestivalReport = _context.HR_CustomReport
                                           .Include(x => x.HR_ReportType)
                                           .Where(x => x.ComId == comid && 
                                           x.HR_ReportType.ReportName == aFestBonus.ReportType && //x.Cat_Emp_Type.EmpTypeId.ToString() == aFestBonus.EmpTypeId &&
                                           !x.IsDelete).ToList();

            ReportType = aFestBonus.ReportFormat;
            try
            {
                switch (aFestBonus.ReportType)
                {
                    case "Festival Bonus Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Festival Bonus Sheet','" + aFestBonus.BankId + "'";

                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Bonus Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFestivalBonusSheet.rdlc";
                        }
                        break;
                    case "Festival Bonus Payslip":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Festival Bonus Sheet','" + aFestBonus.BankId + "'";

                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Bonus Payslip".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFestBonusPayslip.rdlc";
                        }
                        break;

                    case "Festival Bonus Top Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Festival Bonus Top Sheet' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Bonus Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFestBonusTopSheet.rdlc";
                        }
                        
                        break;


                    case "Festival Bonus Top Sheet-SectWise":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Festival Bonus Top Sheet-SectWise' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Bonus Top Sheet-SectWise".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFestBonusTopSheetSectWise.rdlc";
                        }

                        break;

                    case "Festival Bonus Bank Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Festival Bonus Bank Sheet' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Bonus Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                       
                        break;

                    case "Festival Bonus Mobile Bank Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Festival Bonus Mobile Bank Sheet' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Bonus Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }

                        break;

					//Demonation
					case "Festival Denomination Sheet":
						SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
							   "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
								+ aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
							   aFestBonus.FestivalType + "','Festival Denomination Sheet','" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
						{
							foreach (var r in comidFromFestivalReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Festival Denomination Sheet".ToLower()))
								{
									ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
								}
							}
						}
						else
						{
							ReportPath = "/ReportViewer/Payroll/rptSalarySumDenomination.rdlc";
						}

						break;

					case "Advanced Salary Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Advanced Salary Sheet','" + aFestBonus.BankId + "' ";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Advanced Salary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFestAdvanceSheet.rdlc";
                        }
                        
                        break;

                    case "Advanced Salary Top Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Advanced Salary Top Sheet' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Advanced Salary Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFestBonusTopSheet.rdlc";
                        }
                       
                        break;

                    case "Advanced Salary Bank Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Advanced Salary Bank Sheet','" + aFestBonus.BankId + "' ";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Advanced Salary Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                       
                        break;

                    case "Advanced Salary Mobile Bank Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Advanced Salary Mobile Bank Sheet','" + aFestBonus.BankId + "' ";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Advanced Salary Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }

                        break;
					//new Advance 21-mar-2024
					case "Advanced Denomination Sheet":
						SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
							   "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
								+ aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
							   aFestBonus.FestivalType + "','Advanced Denomination Sheet','" + aFestBonus.BankId + "' ";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
						{
							foreach (var r in comidFromFestivalReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Advanced Denomination Sheet".ToLower()))
								{
									ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
								}
							}
						}
						else
						{
							ReportPath = "/ReportViewer/Payroll/rptSalarySumDenomination.rdlc";
						}

						break;

					case "Nabo Barsha Vhata Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus'" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Nabo Barsha Vhata Sheet' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Nabo Barsha Vhata Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptNaboBarshaSheet.rdlc";
                        }
                       
                        break;

                    case "Nabo Barsha Vhata Top Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Nabo Barsha Vhata Top Sheet','" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Nabo Barsha Vhata Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptNaboBarshaTopSheet.rdlc";
                        }
                        
                        break;

                    case "Incentive Bonus Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Incentive Bonus Sheet','" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Incentive Bonus Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptIncentiveBonusSheet.rdlc";
                        }

                        break;

                    case "Incentive Bonus Top Sheet":
                        SqlCmd = "Exec Payroll_rptFestBonus '" + comid + "', '" + aFestBonus.ProssType + "', '" + aFestBonus.Paymode + "','" + aFestBonus.Unit + "','" +
                               "" + aFestBonus.EmpTypeId + "', " + aFestBonus.DeptId + "," + aFestBonus.DesigId + "," + aFestBonus.SectId + "," + aFestBonus.SubSectId + "," + aFestBonus.EmpId + ","
                                + aFestBonus.FloorId + "," + aFestBonus.LineId + ",'" + aFestBonus.EmpStatus + "'," + "'" +
                               aFestBonus.FestivalType + "','Incentive Bonus Top Sheet' ,'" + aFestBonus.BankId + "'";
                        //RDLC
                        if (comidFromFestivalReport.Count > 0)
                        {
                            foreach (var r in comidFromFestivalReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Incentive Bonus Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptIncentiveBonusTopSheet.rdlc";
                        }

                        break;
                }

                ConstrName = "ApplicationServices";
                ReportType = aFestBonus.ReportFormat;
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aFestBonus.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        public string MGTSalarySheet(SalarySheet SalarySheetmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SalarySheetGrid aSalarySheet = SalarySheetmodel.SalarySheetPropGrid;
            ReportType = aSalarySheet.ReportFormat;
            try
            {
                switch (aSalarySheet.ReportType)
                {
                    case "Salary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheetMGT'" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" 
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                            ReportPath = "/ReportViewer/Payroll/rptSalarySheetMng.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Final Settlement":
                        SqlCmd = "Exec Payroll_rptSalarySheetSettlementMGT'" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        ReportPath = "/ReportViewer/Payroll/rptSalarySettlementMng.rdlc";
                            // ReportPath = "/ReportViewer/Payroll/rptSalarySheetWorker.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Payslip":
                        SqlCmd = "Exec Payroll_rptSalarySheetMGT'" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        ReportPath = "/ReportViewer/Payroll/rptPaySlipMng.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Summary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySumMGT'" +comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        ReportPath = "/ReportViewer/Payroll/rptSummurySheetMng.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Salary Top Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT  '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Salary Top Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryTopSheet.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT'" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Bank Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalarySheetBank.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;
                    case "Cash Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT'" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Cash Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalarySheetCash.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Salary Addition Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Salary Addition Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryAddition.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;


                    case "Salary Deduction Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT'" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Salary Deduction Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryDedcution.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Advance Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Advance Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryAdv.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "PF Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT  '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','PF Sheet'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Attendance Bonus":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT  '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Attendance Bonus'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryAttBonus.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Income Tax":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT  '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Income Tax'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryTax.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Denomination":
                        SqlCmd = "Exec Payroll_rptSalaryAllMGT  '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','"
                               + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "','Denomination'";
                        ReportPath = "/ReportViewer/Payroll/rptSalarySumDenomination.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;

                    case "Salary Reconciliation":
                        SqlCmd = "Exec HR_rptSalaryReconciliationMGT  '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "'," + aSalarySheet.SectId + "," + aSalarySheet.EmpId + ", '" + aSalarySheet.LId + "','" +
                               aSalarySheet.EmpStatus + "'";
                        ReportPath = "/ReportViewer/Payroll/rptSalaryReconciliation.rdlc";
                        ConstrName = "ApplicationServices";
                        ReportType = aSalarySheet.ReportFormat;

                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

            _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
            _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
            string filename = aSalarySheet.ReportType.ToString();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }

        public string MLGetReport(int? lvId, string rptFormat)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var reportname = "";
            var filename = "";
            string redirectUrl = "";

            int empid = _context.HR_Emp_ML.Where(x => x.LvId == lvId).Select(x => x.EmpId).Single();
            int EmpTypeId = (int)_context.HR_Emp_Info.Where(x => x.EmpId == empid).Select(x => x.EmpTypeId).Single();

            if (EmpTypeId == 2)
            {
                reportname = "rptML";
            }
            else if (EmpTypeId == 3)
            {
                reportname = "rptML";
            }
            else
            {
                reportname = "rptMLWorker";
            }
            filename = "rptML" + DateTime.Now.Date.ToString();
            var query = "Exec HR_rptML '" + comid + "', '" + lvId + "'";

            _httpContext.HttpContext.Session.SetString("reportquery", "Exec HR_rptML '" + comid + "', '" + lvId + "'");

            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Payroll/" + reportname + ".rdlc");
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";
            GTERP.Models.Common.clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            GTERP.Models.Common.clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            GTERP.Models.Common.clsReport.strDSNMain = DataSourceName;

            //var ConstrName = "ApplicationServices";
            //string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat }); //Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return callBackUrl;
        }

        public string PFSheet(PFSheet PFSheetmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            //ReportItem item = new ReportItem();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            PFSheetGrid aPFSheet = PFSheetmodel.PFSheetPropGrid;


            var comidFromPFReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.HR_ReportType.ReportName == aPFSheet.ReportType && //x.Cat_Emp_Type.EmpTypeId.ToString() == aPFSheet.EmpTypeId &&
                                        !x.IsDelete).ToList();
            try
            {
                switch (aPFSheet.ReportType)
                {
                    case "PF Report":
                        SqlCmd = "Exec HR_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheet.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheet.rdlc";
                            }
                        }
                        break;


                    case "PF Summary":
                        SqlCmd = "Exec HR_rptPFSummary '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSum.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSum.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSum.rdlc";
                            }
                        }
                        break;


                    case "Monthly PF Sheet":
                        SqlCmd = "Exec PF_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                            }
                        }
                        break;

                    case "PF Details":
                        SqlCmd = "Exec PF_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetWithMonth.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetWithMonth.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetWithMonth.rdlc";
                            }
                        }
                        break;

                    case "Summery Sheet individual":
                        SqlCmd = "Exec PF_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSumInd.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSumInd.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSumInd.rdlc";
                            }
                        }
                        break;
                    case "First PF Sheet":
                        SqlCmd = "Exec HR_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                            }
                        }
                        break;

                    case "Ledger Sheet":
                        SqlCmd = "Exec HR_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedger.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedger.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedger.rdlc";
                            }
                        }
                        break;

                    case "PF Individual Report":
                        SqlCmd = "Exec HR_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetPaySlip.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetPaySlip.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetPaySlip.rdlc";
                            }
                        }
                        break;

                    //add new report 20-mar-23 num-1

                    case "PF Ledger Payment Sheet":
                        SqlCmd = "Exec HR_rptPFSheet_Eastport '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                    "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                                 + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Ledger Payment Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerPaymentSheet.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerPaymentSheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerPaymentSheet.rdlc";
                            }
                        }
                        break;


                    case "PF Ledger Payment Sheet(S)":
                        SqlCmd = "Exec HR_rptPFSheet_Eastport '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                    "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                                 + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Ledger Payment Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerPaymentSheet.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerPaymentSheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerPaymentSheet.rdlc";
                            }
                        }
                        break;

                    //add new report 20-mar-23 num-2

                    case "PF Ledger Company Sheet":
                        SqlCmd = "Exec HR_rptPFSheet_Eastport '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Ledger Company Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerCompanySheet.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerCompanySheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerCompanySheet.rdlc";
                            }
                        }
                        break;

                    //add new report 20-mar-23 num-3

                    case "PF Ledger Own Sheet":
                        SqlCmd = "Exec HR_rptPFSheet_Eastport '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                                 "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                                 + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Ledger Own Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerOwnSheet.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerOwnSheet.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFLedgerOwnSheet.rdlc";
                            }
                        }
                        break;

                    //add new report 20-mar-23 num-4

                    case "Total PF Ledger Sheet":
                        SqlCmd = "Exec HR_rptPFLedgerSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                                 "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                                 + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Total PF Ledger Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptTotalPfLedger.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptTotalPfLedger.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptTotalPfLedger.rdlc";
                            }
                        }
                        break;

                    //add new report 20-mar-23 num-5

                    case "PF Check Paid Sheet":
                        SqlCmd = "Exec HR_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                                    "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                                    + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Check Paid Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPfCheckPaid.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPfCheckPaid.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPfCheckPaid.rdlc";
                            }
                        }
                        break;

                    //add new report 20-mar-23 num-6

                    case "PF Interest Sheet":
                        SqlCmd = "Exec HR_rptPFSheet_Eastport '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                                    "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                                    + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Interest Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFInterest.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFInterest.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFInterest.rdlc";
                            }
                        }
                        break;


                }

                switch (aPFSheet.ReportType)
                {
                    case "PF Individual Report":
                        SqlCmd = "Exec HR_rptPFSheet '" + comid + "', '" + aPFSheet.ProssType + "', '" + aPFSheet.Paymode + "','" + aPFSheet.UnitId + "','" +
                               "" + aPFSheet.EmpTypeId + "',  '" + aPFSheet.DeptId + "',  '" + aPFSheet.DesigId + "',"
                               + aPFSheet.SectId + ",'" + aPFSheet.SubSectId + "'," + aPFSheet.EmpId + ", '" + aPFSheet.FloorId + "','" + aPFSheet.LineId + "','" + aPFSheet.EmpStatus + "','" + aPFSheet.ReportType + "','" + aPFSheet.BankId + "'";
                        //RDLC
                        if (comidFromPFReport.Count > 0)
                        {
                            foreach (var r in comidFromPFReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    if (aPFSheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aPFSheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aPFSheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetPaySlip.rdlc";
                            }
                            else if (aPFSheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetPaySlip.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPFSheetPaySlip.rdlc";
                            }
                        }
                        break;

                }
                ReportType = aPFSheet.ReportFormat;

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aPFSheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = this._urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        public string SalarySheet(SalarySheet SalarySheetmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            //ReportItem item = new ReportItem();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SalarySheetGrid aSalarySheet = SalarySheetmodel.SalarySheetPropGrid;

            
            var comidFromSalaryReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.ReportType == "Salary Report" &&
                                        x.HR_ReportType.ReportName == aSalarySheet.ReportType && //x.Cat_Emp_Type.EmpTypeId.ToString() == aSalarySheet.EmpTypeId &&
                                        !x.IsDelete).ToList();
            try
            {
                switch (aSalarySheet.ReportType)
                {
                    case "Salary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet".ToLower()))
                                {
                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetWorker.rdlc";                                

                            }
                        }
                        break;

                    case "Payslip":
                        SqlCmd = "Exec Payroll_rptSalarySheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Payslip".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipWorker.rdlc";
                            }
                        }
                        break;

                    case "Summary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySum '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetWorker.rdlc";
                            }
                        }
                        break;

                    case "Summary Sheet-(Section Wise)":
                        SqlCmd = "Exec Payroll_rptSalarySumSectWise '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet-(Section Wise)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSummurySheet-SectWise.rdlc";
                        }
                        break;

                    case "Summary Sheet-(SubSection Wise)":
                        SqlCmd = "Exec Payroll_rptSalarySumSubSectWise '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet-(SubSection Wise)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                           ReportPath = "/ReportViewer/Payroll/rptSummurySheet-SubSectWise.rdlc";
                        }
                        break;

                    case "Summary Sheet-(Category Wise)":
                        SqlCmd = "Exec Payroll_rptSalarySumCategoryWise '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet-(Category Wise)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSummurySheet-CategoryWise.rdlc";
                        }
                        break;

                    case "Final Settlement":
                        SqlCmd = "Exec Payroll_rptSalarySheetSettlement '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Final Settlement".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementWorker.rdlc";
                            }
                        }
                        break;

                    case "Salary Settlement_Eng":
                        SqlCmd = "Exec Payroll_rptSalarySheetSettlement '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Final Settlement".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlement_Eng.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlement_Eng.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlement_Eng.rdlc";
                            }
                        }
                        break;

                    case "Extra OT Summary Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSum '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT Summary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptExtraOTSummarySheet.rdlc";
                        }
                        break;

                    case "Extra OT Sheet":
                        SqlCmd = "Exec Payroll_rptExtraOTSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Extra OT Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptExtraOTSheet.rdlc";
                        }
                        break;

                    case "Salary Top Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Salary Top Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryTopSheet.rdlc";
                        }
                        break;

                    case "Cash Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Cash Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Cash Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "Mobile Bank Sheet":
                         SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Mobile Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "Salary Addition Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Salary Addition Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Addition Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryAddition.rdlc";
                        }
                        break;

                    case "Salary Deduction Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Salary Deduction Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Deduction Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryDedcution.rdlc";
                        }
                        break;

                    case "Advance Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Advance Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Advance Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryAdv.rdlc";
                        }
                        break;

                    case "PF Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','PF Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PF Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryPF.rdlc";
                        }
                        break;

                    case "Attendance Bonus":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Attendance Bonus','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Attendance Bonus".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryAttBonus.rdlc";
                        }
                        break;

                    case "Income Tax":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Income Tax','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Income Tax".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryTax.rdlc";
                        }
                        break;

                    case "Loan Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Loan Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Loan Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptLoanStatement.rdlc";
                        }
                        break;

                    case "Denomination":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Denomination','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Denomination".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalarySumDenomination.rdlc";
                        }
                        break;
                    case "Salary Sheet-Net Earnings":
                        SqlCmd = "Exec Payroll_rptSalarySheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                                                       "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                                                       + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet-Net Earnings".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalarySheetWithoutDed.rdlc";
                        }
                        break;
                    case "Salary Summary Sheet-Net Earnings":
                        SqlCmd = "Exec Payroll_rptSalarySum '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Summary Sheet-Net Earnings".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSummurySheetWithoutDed.rdlc";
                        }
                        break;

                    case "Salary Reconciliation":
                        SqlCmd = "Exec Payroll_rptSalaryReconciliation '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Reconciliation".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryReconciliation.rdlc";
                        }
                        break;

                    case "Holiday Payment Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Holiday Payment Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Holiday Payment Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptHolidayPaymentSheet.rdlc";
                        }
                        break;

                    case "Holiday Payment Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Holiday Payment Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Holiday Payment Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "Holiday Payment Top Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Holiday Payment Top Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Holiday Payment Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptHolidayPaymentTopSheet.rdlc";
                        }
                        break;

                    case "Holiday Payment Mobile Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Holiday Payment Mobile Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Holiday Payment Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "OT Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','OT Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("OT Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptOTSheetWorker.rdlc";
                        }
                        break;

                    case "OT Bank Sheet":   
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','OT Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("OT Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "OT Top Sheet":

                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','OT Top Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("OT Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptOTTopSheet.rdlc";
                        }
                        break;

                    case "OT Top 10 Sheet":

                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','OT Top 10 Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("OT Top 10 Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptTop10OverTime.rdlc";
                        }
                        break;

                    case "FC Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','FC Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("FC Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFCSheet.rdlc";
                        }
                        break;

                    case "FC Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','FC Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("FC Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "FC Top Sheet":

                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','FC Top Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("FC Top Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptFCSummarySheet.rdlc";
                        }
                        break;

                    //New add 25-nov-2023
                    case "WCE Salary Sheet(PR)":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                                                       "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                                                       + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','WCE Salary Sheet(PR)','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("WCE Salary Sheet(PR)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptWCESalarySheetPRUbl.rdlc";
                        }
                        break;
                    //New add 26-nov-2023
                    case "Contingent PaySlip English":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Contingent PaySlip English','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Contingent PaySlip English".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptContingentPaySlipEnglishUbl.rdlc";
                        }
                        break;

                    //New add 26-nov-2023
                    case "WCE Salary Sheet":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','WCE Salary Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("WCE Salary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptWCESalarySheetUbl.rdlc";
                        }
                        break;

                    //New add 27-nov-2023
                    case "Contingent PaySlip Bangla":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Contingent PaySlip Bangla','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Contingent PaySlip Bangla".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptContingentPaySlipBanglaUbl.rdlc";
                        }
                        break;
                    //New add 27-nov-2023
                    case "BCE Salary Sheet Bangla":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','BCE Salary Sheet Bangla','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("BCE Salary Sheet Bangla".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptBCESalarySheetBanglaUbl.rdlc";
                        }
                        break;
                    //New add 28-nov-2023
                    case "BCE Salary Sheet(PR)":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','BCE Salary Sheet(PR)','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("BCE Salary Sheet(PR)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptBCESalarySheet(PR)Ubl.rdlc";
                        }
                        break;
                    //New add 28-nov-2023
                    case "WCE Summary Sheet":

                        SqlCmd = "Exec Payroll_rptSalarySum_UBL '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','WCE Summary Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("WCE Summary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptWCESummarySheetUbl.rdlc";
                        }
                        break;
                    //New add 28-nov-2023
                    case "Bank Salary Sheet Ubl":

                        SqlCmd = "Exec Payroll_rptSalaryBankSheet_UBL '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet Ubl','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet Ubl".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptBankSalarySheetUbl.rdlc";
                        }
                        break;

                    //30-dec-23
                    case "Bank Salary Sheet DBBL":

                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet DBBL','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet DBBL".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankACCDBBL.rdlc";
                        }
                        break;
                    //30-dec-23
                    case "Bank Salary Sheet UCBL":

                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet UCBL','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet UCBL".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankACCUCBL.rdlc";
                        }
                        break;
                    //2-dec-2023
                    case "PR Salary Summary Sheet":

                        SqlCmd = "Exec Payroll_rptSalarySum_UBL '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','PR Salary Summary Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("PR Salary Summary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptPRSalarySum_Ubl.rdlc";
                        }
                        break;
                        //3-dec-2023
                    case "Salary Sheet (BOF)":

                        SqlCmd = "Exec Payroll_rptSalarySheet_Ubl '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                                                       "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                                                       + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Salary Sheet (BOF)','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet (BOF)".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalarySheet(BOF)_Ubl.rdlc";
                        }
                        break;
                    //10-feb-2024
                    case "Monthly Allowance Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryAll '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Monthly Allowance Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Monthly Allowance Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptMonthlyAllowance.rdlc";
                        }
                        break;
					// 4-Apr-2024
					case "Salary Sheet Excel":
						SqlCmd = "Exec Payroll_rptSalarySheetExcel '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
							   "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
							   + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
						//RDLC
						if (comidFromSalaryReport.Count > 0)
						{
							foreach (var r in comidFromSalaryReport)
							{
								if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet Excel".ToLower()))
								{
									if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
									{
										ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
										break;
									}
									else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
									{
										ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
										break;
									}
									else if (r.EmpTypeId == 1)
									{
										ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
									}
								}
							} //foreach
						}
						else
						{
							if (aSalarySheet.EmpTypeId == "3")
							{
								ReportPath = "/ReportViewer/Payroll/rptSalarySheetExcel.rdlc";
							}
							else if (aSalarySheet.EmpTypeId == "2")
							{
								ReportPath = "/ReportViewer/Payroll/rptSalarySheetExcel.rdlc";
							}
							else
							{
								ReportPath = "/ReportViewer/Payroll/rptSalarySheetExcel.rdlc";

							}
						}
						break;

				}
                ReportType = aSalarySheet.ReportFormat;

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aSalarySheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = this._urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        public string SalarySheetB(SalarySheet SalarySheetBmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SalarySheetGrid aSalarySheet = SalarySheetBmodel.SalarySheetPropGrid;
            ReportType = aSalarySheet.ReportFormat;

            var comidFromSalaryReportB = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.ReportType== "Salary Sheet Buyer" &&
                                        x.HR_ReportType.ReportName == aSalarySheet.ReportType && //x.Cat_Emp_Type.EmpTypeId.ToString() == aSalarySheet.EmpTypeId &&
                                        !x.IsDelete).ToList();

            try
            {

                switch (aSalarySheet.ReportType)
                {
                    case "Salary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheetB '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet".ToLower()))
                                {
                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetWorker.rdlc";
                            }
                        }
                        break;

                    case "Payslip":
                        SqlCmd = "Exec Payroll_rptSalarySheetB '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Payslip".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipWorker.rdlc";
                            }
                        }
                        break;

                    case "Summary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySumB '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetWorker.rdlc";
                            }
                        }
                        break;

                    case "Summary Sheet-(Section Wise)":
                        SqlCmd = "Exec Payroll_rptSalarySumSectWiseB '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet-(Section Wise)".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetWorker.rdlc";
                            }
                        }
                        break;

                    case "Final Settlement":
                        SqlCmd = "Exec Payroll_rptSalarySheetSettlement '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Final Settlement".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementStaff.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementWorker.rdlc";
                            }
                        }
                        break;

                    case "Cash Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Cash Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Cash Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Sheet Buyer','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    //Add 2-jan-2023
                    case "Bank Salary Sheet UCBL":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet UCBL Buyer','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet UCBL Buyer".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankACCUCBLBuyer.rdlc";
                        }
                        break;

                    //Add 2-jan-2023
                    case "Bank Salary Sheet DBBL":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet DBBL Buyer','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet DBBL Buyer".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankACCUCBLBuyer.rdlc";
                        }
                        break;

                    case "Mobile Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Mobile Bank Sheet Buyer','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;
                    case "Salary Sheet With Attendance":
                        SqlCmd = "Exec Payroll_rptAttSalarySheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet With Attendance".ToLower()))
                                {                                    
                                   ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                             ReportPath = "/ReportViewer/Payroll/rptAttSalarySheet.rdlc";
                        }
                        break;

                }
                ConstrName = "ApplicationServices";
                ReportType = aSalarySheet.ReportFormat;
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aSalarySheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }


        }

        public string DynamicSalarySheet(SalarySheet SalarySheetBmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SalarySheetGrid aSalarySheet = SalarySheetBmodel.SalarySheetPropGrid;
            ReportType = aSalarySheet.ReportFormat;

            var comidFromSalaryReportB = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.ReportType == "Dynamic Salary Sheet" &&
                                        x.HR_ReportType.ReportName == aSalarySheet.ReportType && //x.Cat_Emp_Type.EmpTypeId.ToString() == aSalarySheet.EmpTypeId &&
                                        !x.IsDelete).ToList();

            try
            {

                switch (aSalarySheet.ReportType)
                {
                    case "Salary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheetDynamic '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Salary Sheet".ToLower()))
                                {
                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySheetWorker.rdlc";
                            }
                        }
                        break;

                    case "Payslip":
                        SqlCmd = "Exec Payroll_rptSalarySheetDynamic '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Payslip".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptPaySlipWorker.rdlc";
                            }
                        }
                        break;

                    case "Summary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySumDynamic '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetWorker.rdlc";
                            }
                        }
                        break;

                    case "Summary Sheet-(Section Wise)":
                        SqlCmd = "Exec Payroll_rptSalarySumSectWiseDynamic '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Summary Sheet-(Section Wise)".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetOfficer.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSummurySheetWorker.rdlc";
                            }
                        }
                        break;


                    case "Final Settlement":
                        SqlCmd = "Exec Payroll_rptSalarySheetSettlement '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Final Settlement".ToLower()))
                                {

                                    if (aSalarySheet.EmpTypeId == "3" && r.EmpTypeId == 3)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (aSalarySheet.EmpTypeId == "2" && r.EmpTypeId == 2)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                        break;
                                    }
                                    else if (r.EmpTypeId == 1)
                                    {
                                        ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                    }
                                }
                            } //foreach
                        }
                        else
                        {
                            if (aSalarySheet.EmpTypeId == "3")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementStaff.rdlc";
                            }
                            else if (aSalarySheet.EmpTypeId == "2")
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementStaff.rdlc";
                            }
                            else
                            {
                                ReportPath = "/ReportViewer/Payroll/rptSalarySettlementWorker.rdlc";
                            }
                        }
                        break;

                    case "Cash Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Cash Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Cash Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;

                    case "Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Sheet Dynamic','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;
                    //1
                    case "Bank Salary Sheet DBBL":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet DBBL Dynamic','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet DBBL".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankACCDBBLDynamic.rdlc";
                        }
                        break;
                    //2
                    case "Bank Salary Sheet UCBL":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Salary Sheet UCBL Dynamic','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Salary Sheet UCBL".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankACCUCBLDynamic.rdlc";
                        }
                        break;
                    case "Mobile Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Mobile Bank Sheet Dynamic','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReportB.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReportB)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Mobile Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                                //else
                                //{
                                //    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                //}
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;
                }
                ConstrName = "ApplicationServices";
                ReportType = aSalarySheet.ReportFormat;
                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aSalarySheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }


        }

        public string CasualSalarySheet(SalarySheet CasualSalarySheetmodel)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            //ReportItem item = new ReportItem();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SalarySheetGrid aSalarySheet = CasualSalarySheetmodel.SalarySheetPropGrid;


            var comidFromSalaryReport = _context.HR_CustomReport.Include(x => x.HR_ReportType)
                                        .Where(x => x.ComId == comid &&
                                        x.HR_ReportType.ReportName == aSalarySheet.ReportType && //x.Cat_Emp_Type.EmpTypeId.ToString() == aSalarySheet.EmpTypeId &&
                                        !x.IsDelete).ToList();
            try
            {
                switch (aSalarySheet.ReportType)
                {
                    case "Casual Salary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheetCasual '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','1','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Casual Salary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptPaymentSheetCasual.rdlc";
                        }
                        break;

                    

                    case "Casual Payslip":
                        SqlCmd = "Exec Payroll_rptSalarySheetCasual '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','2','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Casual Payslip".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptPaySlipWorker.rdlc";

                        }
                        break;

                    case "Casual Summary Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheetCasual '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','3','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Casual Summary Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            } //foreach
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSummurySheet_Casual.rdlc";
                        }
                        break;

                    case "Cash Sheet":
                        SqlCmd = "Exec Payroll_rptSalaryBankSheet '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Cash Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Cash Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;



                    case "Bank Sheet":
                        SqlCmd = "Exec Payroll_rptSalarySheetCasual '" + comid + "', '" + aSalarySheet.ProssType + "', '" + aSalarySheet.Paymode + "','" + aSalarySheet.UnitId + "','" +
                               "" + aSalarySheet.EmpTypeId + "',  '" + aSalarySheet.DeptId + "',  '" + aSalarySheet.DesigId + "',"
                               + aSalarySheet.SectId + ",'" + aSalarySheet.SubSectId + "'," + aSalarySheet.EmpId + ", '" + aSalarySheet.FloorId + "','" + aSalarySheet.LineId + "','" + aSalarySheet.EmpStatus + "','Bank Sheet','" + aSalarySheet.BankId + "'";
                        //RDLC
                        if (comidFromSalaryReport.Count > 0)
                        {
                            foreach (var r in comidFromSalaryReport)
                            {
                                if (r.ComId == comid && r.HR_ReportType.ReportName.ToLower().Contains("Bank Sheet".ToLower()))
                                {
                                    ReportPath = $"/ReportViewer/Payroll/{r.ReportName}";
                                }
                            }
                        }
                        else
                        {
                            ReportPath = "/ReportViewer/Payroll/rptSalaryBankSheet.rdlc";
                        }
                        break;
                }
                ReportType = aSalarySheet.ReportFormat;

                _httpContext.HttpContext.Session.SetString("ReportPath", ReportPath.ToString());
                _httpContext.HttpContext.Session.SetString("reportquery", SqlCmd);
                string filename = aSalarySheet.ReportType.ToString();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                string callBackUrl = this._urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
                return callBackUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
    }
}
