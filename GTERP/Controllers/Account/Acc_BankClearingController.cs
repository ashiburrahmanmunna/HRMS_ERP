using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Controllers.Account
{
    [OverridableAuthorize]
    public class Acc_BankClearingController : Controller
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db;
        //public CommercialRepository Repository { get; set; } ///for report service
        private TransactionLogRepository tranlog;
        public Acc_BankClearingController(GTRDBContext _db, TransactionLogRepository tran)
        {
            db = _db;
            //Repository = repository; ///for report service
            tranlog = tran;
        }


        // GET: Acc_VoucherMain
        public ViewResult Index(string FromDate, string ToDate, string criteria)
        {
            var transactioncomid = HttpContext.Session.GetString("comid");



            var comid = (HttpContext.Session.GetString("comid"));
            var userid = (HttpContext.Session.GetString("userid"));

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
            //ViewBag.Acc_VoucherNoPrefix = db.Acc_VoucherNoPrefixes.Include(x => x.vVoucherTypes).Where(x => x.isVisible == true && x.vVoucherTypes.isSystem == false).ToList();

            //transactioncomid = "1";
            //var a = ;
            // return View(db.Acc_VoucherMains.Where(p => p.ComId == transactioncomid).ToList());

            List<BankClearing> BankClearingList = new List<BankClearing>();
            ViewBag.Title = criteria;


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

            TempData["Message"] = "Bank Clearing List";

            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), criteria, "psot/unpot", criteria);



            return View(BankClearingList);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(Acc_VoucherMain model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var values = prcSaveData(model);
        //            ViewBag["Message"] = values;
        //            if (values == "Data Posted Successfuly")
        //            {
        //                TempData["successmessage"] = values;
        //                return RedirectToAction("Index");
        //            }
        //            ViewBag.IsError = true;
        //            ModelState.AddModelError("CustomError", values);
        //            return View(model);
        //        }
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.IsError = true;
        //        ModelState.AddModelError("CustomError", ex.Message);
        //        return View(model);
        //    }
        //}
        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


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
            //if (reportname == null)
            //{
            //    reportname = "rptShowVoucher";
            //}

            //HttpContext.Session.SetString("PrintFileName",
            //int WarehouseCount = db.Acc_VoucherMains.Where(x => x.VoucherId == id).Count(); 
            //if (WarehouseCount > 0) { reportname = "rptShowVoucher_SubRpt"; }

            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            var Currency = "1";
            HttpContext.Session.SetString("reportquery", "Exec Acc_rptVoucher 0, 'VID','All', '" + comid + "' , '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.Acc_VoucherMains.Where(x => x.VoucherId == id).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();


            string DataSourceName = "DataSet1";
            //string FormCaption = "Report :: Sales Acknowledgement ...";


            //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" +HttpContext.Session.GetString("comid"); + "',''"));

            HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            /////////////////////// sub report test to our report server

            postData.Add(1, new subReport("rptShowVoucher_ChequeNo", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ChequeNo'"));
            postData.Add(2, new subReport("rptShowVoucher_ReceiptPerson", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptShowVoucher_Referance '" + id + "','" + comid + "','ReceiptPerson'"));


            HttpContext.Session.SetObject("rptList", postData);

            //Session["rptList"] = postData;

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

            //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);

            string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }

        public class BankClearing
        {
            public int Checked { get; set; }
            public int VoucherSubCheckId { get; set; }
            public int VoucherSubCheckNoClearingId { get; set; }
            public string VoucherNo { get; set; }
            public string AccCode { get; set; }
            public string AccName { get; set; }
            public string ChkNo { get; set; }
            public string VoucherDate { get; set; }
            public string DtChk { get; set; }
            public string DtChkTo { get; set; }
            public string DtChkClear { get; set; }
            public bool IsClear { get; set; }
            public string Amount { get; set; }
            public int VoucherId { get; set; }



        }


        public JsonResult SetProcess(List<BankClearing> BankClearinglist, string criteria)
        {

            var comid = (HttpContext.Session.GetString("comid"));
            var userid = (HttpContext.Session.GetString("userid"));



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
                        //else
                        //{

                        //    Acc_VoucherSubCheckno_Clearing abcd = new Acc_VoucherSubCheckno_Clearing();
                        //    abcd.VoucherSubCheckId = BankClearinglist[i].VoucherSubCheckId;
                        //    abcd.dtChkClear = DateTime.Parse(BankClearinglist[i].DtChkClear);
                        //    abcd.VoucherSubCheckNoClearingId = (BankClearinglist[i].VoucherSubCheckNoClearingId);

                        //    abcd.Remarks = "";
                        //    abcd.comid = comid;
                        //    abcd.userid = userid;
                        //    abcd.isClear = true;
                        //    abcd.DateAdded = DateTime.Now.Date;

                        //    db.Add(abcd);
                        //    //var singlevoucher = db.Acc_VoucherSubCheckno_Clearings.Where(x => x.VoucherSubCheckId == Convert.ToInt32(voucheridsingle)).FirstOrDefault();
                        //    db.SaveChanges();
                        //}



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

            var data = "";
            return Json(data = "1");
        }
        public ActionResult create()
        {
            return View();
        }
        public string prcSaveData(Acc_VoucherMain model)
        {
            ArrayList arQuery = new ArrayList();

            try
            {
                var sqlQuery = "";
                // Count total Debit & Credit
                //foreach (var item in model.Collection)
                //{
                //    if (item.IsCheck == true)
                //    {
                //        sqlQuery = " Update tblAcc_Voucher_Main Set IsPosted = 1 ,LuserIdCheck = " + Session["Luserid"].ToString() + "   Where ComId = " + HttpContext.Session.GetString("comid").ToString() + " And VoucherId = " + (item.voucherid) + "";
                //        arQuery.Add(sqlQuery);
                //    }
                //}
                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
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