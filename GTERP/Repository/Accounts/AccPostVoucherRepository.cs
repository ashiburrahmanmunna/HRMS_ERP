using GTERP.BLL;
using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class AccPostVoucherRepository : IAccPostVoucherRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<AccPostVoucherRepository> _logger;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        private readonly TransactionLogRepository _tranlog;
        private readonly IActionContextAccessor _actionContextAccessor;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        public AccPostVoucherRepository(IHttpContextAccessor httpContext,
            ILogger<AccPostVoucherRepository> logger,
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
            _tranlog = tranlog;
            _actionContextAccessor = actionContextAccessor;
        }

        public List<Acc_VoucherMain> ModelList1(string FromDate, string ToDate)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            return _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList();
        }

        public List<Acc_VoucherMain> ModelList2(string FromDate, string ToDate)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            return _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.isPosted == true).ToList();
        }

        public List<Acc_VoucherMain> ModelList3(string FromDate, string ToDate)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            return _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.isPosted == false).ToList();
        }

        public string Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");


            var abcvouchermain = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptShowVoucher";// db.Acc_VoucherMains.Where(x => x.VoucherId== id).Select(x => x.VoucherNo).FirstOrDefault();

            if (abcvouchermain.Acc_VoucherType != null)
            {
                if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Payment".ToUpper())
                {
                    reportname = "rptShowVoucher_VBP";
                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Journal".ToUpper())
                {
                    reportname = "rptShowVoucher_Journal";

                }
                else if (abcvouchermain.Acc_VoucherType.VoucherTypeName.ToUpper() == "Bank Receipt".ToUpper())
                {
                    reportname = "rptShowVoucher_MoneyReceipt";

                }
            }

            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            var Currency = "1";
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptVoucher 0, 'VID','All', '" + comid + "' , '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");
            string filename = _context.Acc_VoucherMains.Where(x => x.VoucherId == id).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";
            _httpContext.HttpContext.Session.SetObject("rptList", postData);

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            var subReport = new SubReport();
            var subReportObject = new List<SubReport>();

            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "";
            subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ChequeNo'";
            subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            subReportObject.Add(subReport);

            subReport = new SubReport();
            subReport.strDSNSub = "DataSet1";
            subReport.strRFNSub = "";
            subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ReceiptPerson'";
            subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            subReportObject.Add(subReport);

            var jsonData = JsonConvert.SerializeObject(subReportObject);

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType }); //this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });  //Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);

            return callBackUrl;

        }

        public IQueryable<VoucherView> Query()
        {
            var query = from e in _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType)
                        select new VoucherView
                        {
                            VoucherId = e.VoucherId,
                            VoucherNo = e.VoucherNo,
                            VoucherDate = e.VoucherDate,
                            //VoucherDate = e.VoucherDate,
                            VoucherDesc = e.VoucherDesc,
                            VAmount = e.VAmount,
                            Comid = e.ComId,
                            isPosted = e.isPosted,
                            VoucherTypeName = e.Acc_VoucherType.VoucherTypeName,
                            VoucherTypeNameShort = e.Acc_VoucherType.VoucherTypeNameShort,
                            //Status = e.isPosted.ToString() != "0" ? "Posted" : "Not Posted",
                            Status = e.isPosted != false ? "Posted" : "Not Posted"
                        };
            return query;
        }

        public void SetProcess(string[] voucherid, string criteria)
        {
            if (criteria.ToUpper().ToString() == "Post".ToUpper())
            {
                if (voucherid.Count() > 0)
                {
                    for (var i = 0; i < voucherid.Count(); i++)
                    {
                        string voucheridsingle = voucherid[i];
                        var singlevoucher = _context.Acc_VoucherMains.Where(x => x.VoucherId == Convert.ToInt32(voucheridsingle)).FirstOrDefault();
                        singlevoucher.isPosted = true;
                        _context.Entry(singlevoucher).State = EntityState.Modified;
                        _context.SaveChanges();
                        _tranlog.TransactionLog(_actionContextAccessor.ActionContext.RouteData.Values["controller"].ToString(), _actionContextAccessor.ActionContext.RouteData.Values["action"].ToString(), "Data Post Successfully", singlevoucher.VoucherId.ToString(), criteria, singlevoucher.VoucherId.ToString());
                    }
                }
            }
            else
            {
                if (criteria.ToUpper().ToString() == "UnPost".ToUpper())
                {
                    if (voucherid.Count() > 0)
                    {
                        for (var i = 0; i < voucherid.Count(); i++)
                        {
                            string voucheridsingle = voucherid[i];
                            var singlevoucher = _context.Acc_VoucherMains.Where(x => x.VoucherId == Convert.ToInt32(voucheridsingle)).FirstOrDefault();

                            singlevoucher.isPosted = false;
                            _context.Entry(singlevoucher).State = EntityState.Modified;
                            _context.SaveChanges();
                            _tranlog.TransactionLog(_actionContextAccessor.ActionContext.RouteData.Values["controller"].ToString(), _actionContextAccessor.ActionContext.RouteData.Values["action"].ToString(), "Data UnPost Successfully", singlevoucher.VoucherId.ToString(), criteria, singlevoucher.VoucherId.ToString());
                        }
                    }
                }
            }

        }
    }
}
