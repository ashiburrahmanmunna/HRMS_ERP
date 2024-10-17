using GTERP.Interfaces.Accounts;
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
using System.Threading.Tasks;

namespace GTERP.Repository.Accounts
{
    public class BankClearingRepository : IBankClearingRepository
    {
        private readonly GTRDBContext db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public BankClearingRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            )
        {
            db = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public List<BankClearing> GetBankClearing(string FromDate, string ToDate, string criteria)
        {

            var transactioncomid =_httpContext.HttpContext.Session.GetString("comid");

            var comid = (_httpContext.HttpContext.Session.GetString("comid"));
            var userid = (_httpContext.HttpContext.Session.GetString("userid"));

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            if (criteria == null)
            {
                criteria = "UnPost";
            }

            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);
            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);
            }

            List<BankClearing> BankClearingList = new List<BankClearing>();
            var Title = criteria;
            if (criteria == "All")
            {
                SqlParameter[] parameter = new SqlParameter[4];
                parameter[0] = new SqlParameter("@dtFrom", dtFrom);
                parameter[1] = new SqlParameter("@dtTo", dtTo);
                parameter[2] = new SqlParameter("@status", "3");
                parameter[3] = new SqlParameter("@comid", comid);

                BankClearingList = Helper.ExecProcMapTList<BankClearing>("prcGetBankClear", parameter);
            }
            else if (criteria == "Post")
            {
                SqlParameter[] parameter = new SqlParameter[4];
                parameter[0] = new SqlParameter("@dtFrom", dtFrom);
                parameter[1] = new SqlParameter("@dtTo", dtTo);
                parameter[2] = new SqlParameter("@status", "1");
                parameter[3] = new SqlParameter("@comid", comid);
                BankClearingList = Helper.ExecProcMapTList<BankClearing>("prcGetBankClear", parameter);
            }
            else if (criteria == "UnPost")
            {
                SqlParameter[] parameter = new SqlParameter[4];
                parameter[0] = new SqlParameter("@dtFrom", dtFrom);
                parameter[1] = new SqlParameter("@dtTo", dtTo);
                parameter[2] = new SqlParameter("@status", "0");
                parameter[3] = new SqlParameter("@comid", comid);
                BankClearingList = Helper.ExecProcMapTList<BankClearing>("prcGetBankClear", parameter);
            }
            return BankClearingList;
        }

        public string Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");


            var abcvouchermain = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.ComId == comid).FirstOrDefault();

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
            var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            var Currency = "1";
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptVoucher 0, 'VID','All', '" + comid + "' , '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");
            string filename = db.Acc_VoucherMains.Where(x => x.VoucherId == id).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";

            _httpContext.HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            postData.Add(1, new subReport("rptShowVoucher_ChequeNo", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ChequeNo'"));
            postData.Add(2, new subReport("rptShowVoucher_ReceiptPerson", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ReceiptPerson'"));
            _httpContext.HttpContext.Session.SetObject("rptList", postData);

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }

        public void SetProcess(List<BankClearing> BankClearinglist, string criteria)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");



            if (criteria.ToUpper().ToString() == "Post".ToUpper())
            {
                if (BankClearinglist.Count() > 0)
                {
                    for (var i = 0; i < BankClearinglist.Count(); i++)
                    {
                        string VoucherSubCheckId = BankClearinglist[i].VoucherSubCheckId.ToString();

                        if (int.Parse(VoucherSubCheckId) > 0)
                        {
                            var singlevoucher = db.Acc_VoucherSubChecnos.Where(x => x.VoucherSubCheckId == Convert.ToInt32(VoucherSubCheckId)).FirstOrDefault();

                            singlevoucher.isClear = true;
                            singlevoucher.dtChkClear = (BankClearinglist[i].DtChkClear);

                            db.Entry(singlevoucher).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                      
                    }
                }
            }
            else
            {
                if (BankClearinglist.Count() > 0)
                {
                    for (var i = 0; i < BankClearinglist.Count(); i++)
                    {
                        string VoucherSubCheckId = BankClearinglist[i].VoucherSubCheckId.ToString();

                        if (int.Parse(VoucherSubCheckId) > 0)
                        {
                            var singlevoucher = db.Acc_VoucherSubChecnos.Where(x => x.VoucherSubCheckId == Convert.ToInt32(VoucherSubCheckId)).FirstOrDefault();


                            singlevoucher.isClear = false;
                            singlevoucher.dtChkClear = null;

                            db.Entry(singlevoucher).State = EntityState.Modified;
                            db.SaveChanges();


                        }
                    }
                }
            }

        }
    }
}
