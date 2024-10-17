using GTERP.BLL;
using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class ShowVoucherRepository : IShowVoucherRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<ShowVoucherRepository> _logger;
        private readonly TransactionLogRepository _tranlog;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        IActionContextAccessor _actionContextAccessor;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public ShowVoucherRepository(IHttpContextAccessor httpContext,
            ILogger<ShowVoucherRepository> logger,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            TransactionLogRepository tranlog
            )
        {
            _httpContext = httpContext;
            _context = context;
            _logger = logger;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _actionContextAccessor = actionContextAccessor;
            _tranlog = tranlog;
        }

        public int DefaultCountry()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Companys.Where(a => a.CompanyCode == comid).Select(a => a.CountryId).FirstOrDefault();
        }

        public List<Acc_FiscalMonth> FiscalMonth()
        {
            int fiscalyid = FiscalYear().Max(p => p.FYId);
            return _context.Acc_FiscalMonths.Where(x => x.FYId == fiscalyid).ToList();
        }

        public List<Acc_FiscalYear> FiscalYear()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_FiscalYears.Where(x => x.ComId == comid).ToList();
        }

        public IEnumerable<SelectListItem> PrdUnitId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            return new SelectList(_context.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName");

        }

        public string SetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo, string VoucherFrom, string VoucherTo, int? Currency, int? isPosted, int? isOther, int? FYId, int? VoucherTypeId, int? AccId, int? PrdUnitId)
        {
            var ConstrName = "ApplicationServices";
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var VoucherTypeShortName = "";

            if (criteria == "fy")
            {
                if (FYId != null || FYId.Value > 0)
                {
                    Acc_FiscalYear fiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == FYId).FirstOrDefault();
                    dtFrom = fiscalyear.OpDate;
                    dtTo = fiscalyear.ClDate;
                }
            }


            if (VoucherTypeId != null || VoucherTypeId.Value >= 0)
            {
                Acc_VoucherType Acc_VoucherType = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeId == VoucherTypeId).FirstOrDefault();
                VoucherTypeShortName = Acc_VoucherType.VoucherTypeNameShort;

            }

            var abcvouchermain = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeId == VoucherTypeId).FirstOrDefault();

            var reportname = "rptShowVoucher";// db.Acc_VoucherMains.Where(x => x.VoucherId== id).Select(x => x.VoucherNo).FirstOrDefault();

            if (abcvouchermain != null)
            {
                if (abcvouchermain.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    reportname = "rptShowVoucher_VBP";
                }
                else if (abcvouchermain.VoucherTypeName.ToUpper() == "Journal".ToUpper())
                {
                    reportname = "rptShowVoucher_Journal";

                }
                else if (abcvouchermain.VoucherTypeName.ToUpper() == "Bank Receipt".ToUpper())
                {
                    reportname = "rptShowVoucher_MoneyReceipt";

                }
                else if (abcvouchermain.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    reportname = "rptChk_janata";
                }
            }

            string filename = "";
            string strQueryMain = "";
            if (criteria.ToUpper().ToString() == "No".ToUpper())
            {

                filename = "VoucherNo_From_" + VoucherFrom + "_To_" + VoucherTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();

                strQueryMain = "Exec Acc_rptVoucher '" + isPosted.ToString() + "','VNo','" + VoucherTypeShortName + "', '" + comid + "' , '" +
                    dtFrom + "','" + dtTo + "','" + VoucherFrom + "','" + VoucherTo + "',0, " + Currency + ", '" + AccId + "'";
            }
            else if (criteria.ToUpper().ToString() == "Date".ToUpper())
            {
                filename = "Voucher_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();


                strQueryMain = "Exec Acc_rptVoucher '" + isPosted.ToString() + "','VDATE','" + VoucherTypeShortName + "'  , '" + comid + "' ,'" +
                    dtFrom + "','" + dtTo + "','','',0, " + Currency + ", '" + AccId + "'";


            }
            else if (criteria.ToUpper().ToString() == "fy".ToUpper())
            {
                filename = "Voucher_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();

                strQueryMain = "Exec Acc_rptVoucher '" + isPosted.ToString() + "','VDATE','" + VoucherTypeShortName + "' , '" +
                    comid + "','" + dtFrom + "','" + dtTo + "','','',0, " + Currency + ", '" + AccId + "'";


            }


            var subReport = new SubReport();
            //var subReportObject = new List<SubReport>();

            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "VoucherId";
            subReport.strQuerySub = "Exec [rptShowVoucher_Referance] 'xxxx','" + comid + "','ChequeNo'";
            subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            //subReportObject.Add(subReport);
            postData.Add(2, subReport);



            subReport = new SubReport();
            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "VoucherId";
            subReport.strQuerySub = "Exec [rptShowVoucher_Referance] 'xxxx','" + comid + "','ReceiptPerson'";
            subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            //subReportObject.Add(subReport);
            postData.Add(3, subReport);


            _httpContext.HttpContext.Session.SetObject("rptList", postData);

            string DataSourceName = "DataSet1";
            var ReportPath = "~/ReportViewer/Accounts/" + reportname + ".rdlc";
            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            _httpContext.HttpContext.Session.SetString("reportquery", strQueryMain);
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;


            _tranlog.TransactionLog(_actionContextAccessor.ActionContext.RouteData.Values["controller"].ToString(), _actionContextAccessor.ActionContext.RouteData.Values["action"].ToString(), "Report View", reportname, "Report", reportname);

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return callBackUrl;
        }

        public IEnumerable<SelectListItem> VoucherTypeId()
        {
            return new SelectList(_context.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName").ToList();
        }
    }
}
