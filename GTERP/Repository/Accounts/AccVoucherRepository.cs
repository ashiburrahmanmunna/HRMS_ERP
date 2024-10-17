using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Repository.Accounts
{
    public class AccVoucherRepository:BaseRepository<Acc_VoucherMain>, IAccVoucherRepository
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<AccVoucherRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUrlHelper _urlHelper;

        public clsProcedure _clsProc { get; }
        public WebHelper _webHelper { get; }

        public AccVoucherRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<AccVoucherRepository> logger,
            IConfiguration configuration,
            clsProcedure clsProc,
            WebHelper webHelper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _configuration = configuration;
            _clsProc = clsProc;
            _webHelper = webHelper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IEnumerable<SelectListItem> FiscalMonthId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid).OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");
        }

        public IEnumerable<SelectListItem> FiscalMonthId1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var fiscalYear = _context.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            return new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid).Where(x => x.FYId == fiscalYear.FiscalYearId).OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");
        }

        public IEnumerable<SelectListItem> FiscalYearId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> FiscalYearId1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var fiscalYear = _context.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            return new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
        }

        public IEnumerable<SelectListItem> IntegrationSettingMainId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Integration_Setting_Mains.Where(v => v.ComId == comid && v.IntegrationTableName == "HR_ProcessedDataSal").OrderBy(x => x.MainSLNo), "IntegrationSettingMainId", "IntegrationSettingName");
        }

        public string PrintCheck(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpcontext.HttpContext.Session.GetString("comid");

            var abcvouchermain = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptChk_janata";

            if (abcvouchermain.Acc_VoucherType != null)
            {
                if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    reportname = "rptChk_janata";
                }
            }
            _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;
            _httpcontext.HttpContext.Session.SetString("reportquery", "Exec acc_rptCheckPrint  '" + id + "','" + comid + "' , 'ChequeNo'");

            string filename = _context.Acc_VoucherSubChecnos.Where(x => x.VoucherId == id).Select(x => x.ChkNo).FirstOrDefault();
            _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            string DataSourceName = "DataSet1";

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpcontext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpcontext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            var abcd = _httpcontext.HttpContext.Session.GetString("ReportType");

            if (abcd != null)
            {
                ReportType = abcd;
            }
            else
            {
                ReportType = "PDF";
            }

            var callBackUlr = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUlr;
        }

        public IEnumerable<SelectListItem> UserList()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            var appKey = _httpcontext.HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<AspnetUserList>();
            var le = new AspnetUserList();
            le.Email = "--All User--";
            le.UserId = "1";
            le.UserName = "--All User--";
            l.Add(le);

            foreach (var c in list)
            {
                le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }
            return  new SelectList(l, "UserId", "UserName", userid);
        }

        public string Print1(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpcontext.HttpContext.Session.GetString("comid");

            var abcvouchermain = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptShowVoucher";
            if (abcvouchermain.Acc_VoucherType != null)
            {
                if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    if (abcvouchermain.CountryId == 18)
                    {
                        reportname = "rptShowVoucher_VBP";
                    }
                    else
                    {
                        reportname = "rptShowVoucher_Journal_FC";
                    }
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Journal".ToUpper())
                {
                    if (abcvouchermain.CountryId == 18)
                    {
                        reportname = "rptShowVoucher_Journal";
                    }
                    else
                    {
                        reportname = "rptShowVoucher_Journal_FC";
                    }
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Receipt".ToUpper())
                {
                    if (abcvouchermain.CountryId == 18)
                    {
                        reportname = "rptShowVoucher_MoneyReceipt";
                    }
                    else
                    {
                        reportname = "rptShowVoucher_Journal_FC";
                    }
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Janata Check".ToUpper())
                {
                    reportname = "rptChk_janata";
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Cash Payment".ToUpper())
                {
                    if (abcvouchermain.CountryId == 18)
                    {
                        reportname = "rptShowVoucher_VCP";
                    }
                    else
                    {
                        reportname = "rptShowVoucher_Journal_FC";
                    }
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Cash Receipt".ToUpper())
                {
                    if (abcvouchermain.CountryId == 18)
                    {
                        reportname = "rptShowVoucher";
                    }
                    else
                    {
                        reportname = "rptShowVoucher_Journal_FC";
                    }
                }
            }

            _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            var Currency = "18";
            _httpcontext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptVoucher 0, 'VID','All', '" + comid + "' , '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");

            string filename = _context.Acc_VoucherMains.Where(x => x.VoucherId == id).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
            _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            clsReport.strReportPathMain = _httpcontext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpcontext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            var abcd = _httpcontext.HttpContext.Session.GetString("ReportType");

            if (abcd != null)
            {
                ReportType = abcd;
            }
            else
            {
                ReportType = "PDF";
            }

            postData.Add(1, new subReport("rptShowVoucher_ChequeNo", "", "DataSet1", "Exec dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ChequeNo'"));
            postData.Add(2, new subReport("rptShowVoucher_ReceiptPerson", "", "DataSet1", "Exec dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ReceiptPerson'"));

            _httpcontext.HttpContext.Session.SetObject("rptList", postData);

            var callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }

        public IEnumerable<SelectListItem> Acc_VoucherType(string Type)
        {
            string vouchertypeid = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();
            return new SelectList(_context.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName", vouchertypeid);
        }

        public IEnumerable<SelectListItem> PrdUnitId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return  new SelectList(_context.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName");
        }

        public IEnumerable<SelectListItem> EmpId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var HR_Emp_Info = _context.HR_Emp_Info.Where(c => c.EmpId > 0 && c.ComId == comid).Select(s => new { Text = s.EmpCode + " - " + s.EmpName + " - " + s.Cat_Designation.DesigName, Value = s.EmpId }).ToList();
            return  new SelectList(HR_Emp_Info, "Value", "Text");

        }

        public IEnumerable<SelectListItem> CustomerId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Customer = _context.Customers.Take(1).Where(c => c.CustomerId > 0 && c.comid == comid).Select(s => new { Text = s.CustomerCode + " - " + s.CustomerName, Value = s.CustomerId }).ToList();
            return  new SelectList(Customer, "Value", "Text");
        }

        public IEnumerable<SelectListItem> SupplierId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Supplier = _context.Suppliers.Where(c => c.SupplierId > 0 && c.ComId == comid).Select(s => new { Text = s.SupplierCode + s.SupplierName, Value = s.SupplierId }).ToList();
            return  new SelectList(Supplier, "Value", "Text");
        }

        public IEnumerable<SelectListItem> VoucherTranGroupArray()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var VoucherTranGroupList = _context.VoucherTranGroups.Where(x => x.ComId == comid).ToList();
            return  new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName");
        }

        public IEnumerable<SelectListItem> VoucherTranGroupId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var VoucherTranGroupList = _context.VoucherTranGroups.Where(x => x.ComId == comid).ToList();
            return new SelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName");
        }

        public IEnumerable<SelectListItem> VoucherTranGroupIdRow()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var VoucherTranGroupList = _context.VoucherTranGroups.Where(x => x.ComId == comid).ToList();
            return new SelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName");
        }

        public IEnumerable<SelectListItem> Country()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var transactioncompany = _context.Companys.Include(x => x.vCountryCompany).Where(c => c.CompanySecretCode == comid).FirstOrDefault();
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", transactioncompany.CountryId);
        }

        public IEnumerable<SelectListItem> CountryIdVoucher()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var transactioncompany = _context.Companys.Include(x => x.vCountryCompany).Where(c => c.CompanySecretCode == comid).FirstOrDefault();
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", transactioncompany.CountryId);
        }

        public IEnumerable<SelectListItem> AccountMain()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public Acc_VoucherMain GetVoucherMain(int VoucherId = 0)
        {
            return _context.Acc_VoucherMains
                            .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount).ThenInclude(b => b.ParentChartOfAccount)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                            .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.Country)
                            .Include(a => a.VoucherSubs).ThenInclude(a => a.CountryForeign)
                            .Include(x => x.Acc_VoucherType)
                            .Where(x => x.VoucherId == VoucherId).FirstOrDefault();
        }

        public List<VoucherTranGroup> VoucherTranGroupList()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return _context.VoucherTranGroups.Where(x => x.ComId == comid).ToList();
        }

        public Acc_VoucherMain vouchersamplemodel(int FiscalMonthId = 0, int? IntegrationSettingMainId = 0)
        {
            Acc_VoucherMain vouchersamplemodel = new Acc_VoucherMain();
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var query = $"Exec Integration_Process '{comid}',{IntegrationSettingMainId},{FiscalMonthId} ";

            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@IntegrationSettingMainId", IntegrationSettingMainId);
            sqlParameter[2] = new SqlParameter("@FiscalMonthid", FiscalMonthId);
            Helper.ExecProc("Integration_Process", sqlParameter);

            var fiscalmonthname = _context.Acc_FiscalMonths.Where(x => x.FiscalMonthId == FiscalMonthId && x.ComId == comid).Select(x => x.MonthName).FirstOrDefault();
            string SETTINGRemarks = _context.Cat_Integration_Setting_Mains.Where(x => x.IntegrationSettingMainId == IntegrationSettingMainId).FirstOrDefault().IntegrationRemarks;

            var abcdefgh = _context.Cat_PayrollIntegrationSummary.Include(x => x.Acc_ChartOfAccounts).ThenInclude(x => x.ParentChartOfAccount).Where(x => x.FiscalMonthId == FiscalMonthId && x.DataType == IntegrationSettingMainId.ToString() && x.Acc_ChartOfAccounts.AccType == "L").ToList();
            vouchersamplemodel.VoucherDesc = fiscalmonthname + " " + SETTINGRemarks;

            vouchersamplemodel.VoucherSubs = new List<Acc_VoucherSub>();
            foreach (var item in abcdefgh)
            {
                if ((item.TKDebitLocal + item.TKCreditLocal) > 0)
                {
                    Acc_VoucherSub abc = new Acc_VoucherSub();
                    abc.AccId = item.AccId;
                    abc.TKDebit = item.TKDebitLocal;
                    abc.TKCredit = item.TKCreditLocal;
                    abc.TKDebitLocal = item.TKDebitLocal;
                    abc.TKCreditLocal = item.TKCreditLocal;
                    abc.Note1 = item.Note1; ///IF CT INFORMATION IS THERE --- CAUSE NO OTHER COLUMN HAVE FOR CT IN INTEGRATION TABLE
                    abc.Note2 = item.Note2; ///IF CT INFORMATION IS THERE --- CAUSE NO OTHER COLUMN HAVE FOR CT IN INTEGRATION TABLE
                    abc.Note3 = item.Note3;
                    abc.SLNo = int.Parse(item.SLNo);
                    abc.Acc_ChartOfAccount = item.Acc_ChartOfAccounts;
                    vouchersamplemodel.VoucherSubs.Add(abc);
                    
                }
            };
            return vouchersamplemodel;
        }

        public IEnumerable<SelectListItem> Account()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L");
            return new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> Account1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L");
            return new SelectList(Acc_ChartOfAccount.Where(x => x.IsBankItem == false && x.IsCashItem == false).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }
    }
}
