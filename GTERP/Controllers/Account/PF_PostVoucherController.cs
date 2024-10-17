using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Controllers.Account
{
    public class PF_PostVoucherController : Controller
    {
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db;
        //public CommercialRepository Repository { get; set; } ///for report service
        public PF_PostVoucherController(GTRDBContext _db, TransactionLogRepository tran)
        {
            db = _db;
            //Repository = repository; ///for report service
            tranlog = tran;
        }


        // GET: PF_VoucherMain
        public ViewResult Index(string FromDate, string ToDate, string criteria)
        {
            var transactioncomid = HttpContext.Session.GetString("comid");

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


            List<PF_VoucherMain> modellist = new List<PF_VoucherMain>();
            ViewBag.Title = criteria;

            if (criteria == "All")
            {
                modellist = db.PF_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList();
            }
            else if (criteria == "Post")
            {
                modellist = db.PF_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.isPosted == true).ToList();

            }
            else if (criteria == "UnPost")
            {
                modellist = db.PF_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.comid == transactioncomid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.isPosted == false).ToList();

            }


            return View(modellist);
        }

        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            var abcvouchermain = db.PF_VoucherMains.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.comid == comid).FirstOrDefault();

            var reportname = "rptShowVoucher";// db.PF_VoucherMains.Where(x => x.VoucherId== id).Select(x => x.VoucherNo).FirstOrDefault();

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


            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = db.PF_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            var Currency = "1";
            HttpContext.Session.SetString("reportquery", "Exec PF_rptVoucher 0, 'VID','All', '" + comid + "' , '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.PF_VoucherMains.Where(x => x.VoucherId == id).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";

            HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            /////////////////////// sub report test to our report server


            //var subReport = new SubReport();
            //var subReportObject = new List<SubReport>();

            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ChequeNo'";
            //subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            //subReportObject.Add(subReport);


            //subReport = new SubReport();
            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ReceiptPerson'";
            //subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            //subReportObject.Add(subReport);


            //var jsonData = JsonConvert.SerializeObject(subReportObject);



            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);


            postData.Add(1, new subReport("rptShowVoucher_ChequeNo", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ChequeNo'"));
            postData.Add(2, new subReport("rptShowVoucher_ReceiptPerson", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ReceiptPerson'"));


            HttpContext.Session.SetObject("rptList", postData);


            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), abcvouchermain.VoucherId.ToString(), "Report", reportname);


            string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });//, new { id = 1 }
            return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }


        public JsonResult SetProcess(string[] voucherid, string criteria)
        {
            if (criteria.ToUpper().ToString() == "Post".ToUpper())
            {
                if (voucherid.Count() > 0)
                {
                    for (var i = 0; i < voucherid.Count(); i++)
                    {
                        string voucheridsingle = voucherid[i];


                        var singlevoucher = db.PF_VoucherMains.Where(x => x.VoucherId == Convert.ToInt32(voucheridsingle)).FirstOrDefault();

                        singlevoucher.isPosted = true;
                        db.Entry(singlevoucher).State = EntityState.Modified;
                        db.SaveChanges();

                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), "Data Post Successfully", singlevoucher.VoucherId.ToString(), criteria, singlevoucher.VoucherId.ToString());

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


                            var singlevoucher = db.PF_VoucherMains.Where(x => x.VoucherId == Convert.ToInt32(voucheridsingle)).FirstOrDefault();

                            singlevoucher.isPosted = false;
                            db.Entry(singlevoucher).State = EntityState.Modified;
                            db.SaveChanges();
                            //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), singlevoucher.VoucherId.ToString(), criteria, singlevoucher.VoucherId.ToString());



                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), "Data UnPost Successfully", singlevoucher.VoucherId.ToString(), criteria, singlevoucher.VoucherId.ToString());

                        }
                    }
                }
            }



            var data = "";
            return Json(data = "1");
        }
        public ActionResult create()
        {
            return View();
        }
        public string prcSaveData(PF_VoucherMain model)
        {
            ArrayList arQuery = new ArrayList();

            try
            {
                var sqlQuery = "";

                return "Data Posted Successfuly";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            finally
            {
                //clsCon = null;
            }
        }

    }
}