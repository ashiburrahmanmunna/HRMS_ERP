using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace GTERP.Controllers.PF
{
    public class PF_SummaryReportController : Controller
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }
        private TransactionLogRepository tranlog;

        private GTRDBContext db = new GTRDBContext();
        POSRepository POS;

        public PF_SummaryReportController(GTRDBContext context, POSRepository _POS, TransactionLogRepository trans)
        {
            db = context;
            POS = _POS;
            //Repository = rep;
            tranlog = trans;
        }

        public JsonResult GetMonthList(int? id)
        {
            string comid = HttpContext.Session.GetString("comid"); ;

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;
            if (id == null)
            {
                List<PF_FiscalYear> fiscalyear = db.PF_FiscalYear.Where(x => x.ComId == comid).ToList();
                id = fiscalyear.Max(p => p.FYId);
            }

            List<PF_FiscalMonth> PF_FiscalMonth = db.PF_FiscalMonth.Where(x => x.FYId == id).ToList();
            List<PF_FiscalMonth> datamonth = new List<PF_FiscalMonth>();
            foreach (PF_FiscalMonth item in PF_FiscalMonth)
            {
                PF_FiscalMonth asdf = new PF_FiscalMonth
                {
                    MonthId = item.MonthId,
                    MonthName = item.MonthName,
                    dtFrom = DateTime.Parse(item.dtFrom).ToString("dd-MMM-yy"),
                    dtTo = DateTime.Parse(item.dtTo).ToString("dd-MMM-yy")
                    //LCDate = DateTime.Parse(item.FirstShipDate.ToString()).ToString("ddd-MMM-yy")
                };
                datamonth.Add(asdf);
            }

            List<Acc_FiscalHalfYear> Acc_FiscalHalfYear = db.Acc_FiscalHalfYears.Where(x => x.FYId == id).ToList();
            List<Acc_FiscalHalfYear> datahalfyear = new List<Acc_FiscalHalfYear>();
            foreach (Acc_FiscalHalfYear item in Acc_FiscalHalfYear)
            {
                Acc_FiscalHalfYear halfyear = new Acc_FiscalHalfYear
                {
                    FiscalHalfYearId = item.FiscalHalfYearId,
                    HyearName = item.HyearName,
                    dtFrom = DateTime.Parse(item.dtFrom).ToString("dd-MMM-yy"),
                    dtTo = DateTime.Parse(item.dtTo).ToString("dd-MMM-yy")
                };
                datahalfyear.Add(halfyear);
            }

            List<Acc_FiscalQtr> FiscalQuarter = db.Acc_FiscalQtrs.Where(x => x.FYId == id).ToList();
            List<Acc_FiscalQtr> dataquarter = new List<Acc_FiscalQtr>();
            foreach (Acc_FiscalQtr item in FiscalQuarter)
            {
                Acc_FiscalQtr quarter = new Acc_FiscalQtr
                {
                    QtrId = item.QtrId,
                    QtrName = item.QtrName,
                    dtFrom = DateTime.Parse(item.dtFrom).ToString("dd-MMM-yy"),
                    dtTo = DateTime.Parse(item.dtTo).ToString("dd-MMM-yy")
                };
                dataquarter.Add(quarter);
            }

            var data = new { datam = datamonth, datah = datahalfyear, dataq = dataquarter };
            return Json(data);

        }

        // GET: Section
        public ActionResult Report()
        {
            string comid = HttpContext.Session.GetString("comid");
            int defaultcountry = (db.Companys.Where(a => a.CompanyCode == comid).Select(a => a.CountryId).FirstOrDefault());
            ViewBag.CountryId = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", defaultcountry);
            ViewBag.AccIdGroup = new SelectList(POS.GetChartOfAccountGroup(comid), "AccId", "AccName");
            ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)


            PFShowVoucherViewModel model = new PFShowVoucherViewModel();
            List<PF_FiscalYear> fiscalyear = db.PF_FiscalYear.Where(x => x.ComId == comid).ToList();
            int fiscalyid = fiscalyear.Max(p => p.FYId);


            List<Acc_FiscalMonth> fiscalmonth = db.Acc_FiscalMonths.Where(x => x.FYId == fiscalyid).ToList();
            List<Acc_FiscalQtr> fiscalquarter = db.Acc_FiscalQtrs.Where(x => x.FYId == fiscalyid).ToList();
            List<Acc_FiscalHalfYear> fiscalhalfyear = db.Acc_FiscalHalfYears.Where(x => x.FYId == fiscalyid).ToList();

            model.FiscalYs = fiscalyear;
            model.ProcessMonths = fiscalmonth;

            model.ProcessQtr = fiscalquarter;
            model.ProcessHalfYear = fiscalhalfyear;


            model.CountryId = defaultcountry;

            //rptGeneralReport.PrcSetData(model, "Create", dsList);
            return View(model);
        }

        //ComName ComAddress ComAddress2 AccId AccCode AccName AccType opDebit opCredit    TranDebit TranCredit  clDebit clCredit    Caption opBalance   tranBalance clBalance
        //31312C54-659B-4E63-B4BA-2BC3D7B05792 DAP FERTILIZER COMPANY LIMITED Rangadia, Chattogram-4000	NULL	10019	1-1-11-000-00000	Cash at bank G	415982457.55	0.00	879445882.00	1161957367.70	133470971.85	0.00	Trial Balance || Date Range : 01 Aug 2020 To 31 Aug 2020	415982457.55	879445882.00	133470971.85



        public partial class TrialBalanceModel
        {

            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComAddress2 { get; set; }

            public string Caption { get; set; }
            public string opBalance { get; set; }
            public string tranBalance { get; set; }
            public string clBalance { get; set; }


            public string AccCode { get; set; }
            public string AccName { get; set; }
            public string opDebit { get; set; }
            public string opCredit { get; set; }
            public string TranDebit { get; set; }
            public string TranCredit { get; set; }
            public string clDebit { get; set; }
            public string clCredit { get; set; }
            public string AccType { get; set; }


        }

        public ActionResult trailReport()
        {
            try
            {

                var result = "";
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");

                if (comid == null)
                {
                    result = "Please Login first";
                }

                //Exec Acc_rptLedgerMultiDrCr '31312c54-659b-4e63-b4ba-2bc3d7b05792', 10165, 0, '01-Jul-2020', '28-Nov-2020', 18,0 ,'4864add7-0ab2-4c4f-9eb8-6b63a425e665' , '', '', ''
                string criteria = "Month";
                string strId = "14";
                string isShowZero = "0";
                string Currency = "18";
                string AccIdGroup = "0";


                var quary = $"Exec PF_rptTrailBalanceGroup '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + "," + Currency + " ,'" + AccIdGroup + "'";


                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@UserId", userid);
                parameters[1] = new SqlParameter("@ComId", comid);
                parameters[2] = new SqlParameter("@Flag", "Month");
                parameters[3] = new SqlParameter("@Id", 14);
                parameters[4] = new SqlParameter("@IsShowZero", 0);
                parameters[5] = new SqlParameter("@Currency", 18);
                parameters[6] = new SqlParameter("@accid", 0);



                List<TrialBalanceModel> TrialBalanceReport = Helper.ExecProcMapTList<TrialBalanceModel>("PF_rptTrailBalanceGroup", parameters);

                return View(TrialBalanceReport);

                //return Json(new { bookingDeliveryChallan, ex = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(string RptType, string criteria, string rptFormat, string command)
        {
            //var strId = 0;
            //var isCom = 0;
            //var isZero = 0;
            //var isGroup = 0;
            //if (model.isCompare == true){isCom = 1;}
            //if (model.isShowZero == true) { isZero = 1; }
            //if (model.isGroup == true) { isGroup = 1; }

            //if (criteria == "Year")
            //{
            //    if (model.FiscalYearsSum.Count > 0)
            //    {
            //        for (var i = 0; i < model.FiscalYearsSum.Count; i++)
            //        {
            //            if (model.FiscalYearsSum[i].isCheck == true)
            //            {
            //                strId = model.FiscalYearsSum[i].FYId;
            //            }
            //        }
            //    }
            //}
            //if (criteria.ToUpper().ToString() == "HYear".ToUpper())
            //{
            //    for (var i = 0; i < model.HalfYears.Count; i++)
            //    {
            //        if (model.HalfYears[i].isCheck == true)
            //        {
            //            strId = model.HalfYears[i].HalfYId;
            //        }
            //    }
            //}
            //if (criteria.ToUpper().ToString() == "Quarter".ToUpper())
            //{
            //    for (var i = 0; i < model.Quarters.Count; i++)
            //    {
            //        if (model.Quarters[i].isCheck == true)
            //        {
            //            strId = model.Quarters[i].QuarterId;
            //        }
            //    }
            //}
            //if (criteria.ToUpper().ToString() == "Month".ToUpper())
            //{
            //    for (var i = 0; i < model.Monthlys.Count; i++)
            //    {
            //        if (model.Monthlys[i].isCheck == true)
            //        {
            //            strId = model.Monthlys[i].MonthId;
            //        }
            //    }
            //}
            //if (command != "Report")
            //{
            //    DataSet dsList = new DataSet();
            //    DataSet dsDetails = new DataSet();
            //    if (command == "0")
            //    {
            //        dsList = rptSummaryReport.prcGetData("0");
            //        var fiscalyid = "0";
            //        fiscalyid = dsList.Tables[0].Rows[0]["FYId"].ToString();
            //        dsList = rptSummaryReport.prcGetData(fiscalyid);
            //        List<clsCommon.clsCombo2> Curr = clsGenerateList.prcColumnTwo(dsList.Tables[3]);
            //        ViewBag.Currency = Curr;
            //        rptSummaryReport.PrcSetData(model, "Create", dsList);
            //    }
            //    else
            //    {
            //        dsDetails = rptSummaryReport.prcGetData(command);
            //        List<clsCommon.clsCombo2> Curr = clsGenerateList.prcColumnTwo(dsDetails.Tables[3]);
            //        ViewBag.Currency = Curr;
            //        rptSummaryReport.PrcSetData(model, "Create", dsDetails);
            //    }
            //    return View(model);
            //}
            //else
            //{
            //    Dictionary<string, object> postData = new Dictionary<string, object>();
            //    ///postData.Add("Acc_rptFormat", rptFormat);
            //    if (RptType.ToUpper().ToString() == "TB".ToUpper())
            //    {
            //        if (model.isGroup == false)
            //        {
            //            if (model.isCompare == false)
            //            {
            //                postData.Add("Acc_rptCode", "1008");
            //                postData.Add("sqlQuery",
            //                    "Exec " + Session["dbACC"].ToString() + ".dbo.rptTrailBalance 1," + Session["ComId"] +
            //                    ",'" + criteria + "', '" + strId + "'," + model.CurrId + " ");
            //            }
            //            else
            //            {
            //                postData.Add("Acc_rptCode", "1009");
            //                postData.Add("sqlQuery",
            //                    "Exec " + Session["dbACC"].ToString() + ".dbo.rptCompareTrialBalance '" + criteria +
            //                    "', '" + comid + "', '" + strId + "' ");
            //            }
            //        }
            //        else
            //        {
            //            if (model.isCompare == false)
            //            {
            //                postData.Add("Acc_rptCode", "1010");
            //                postData.Add("sqlQuery",
            //                    "Exec " + Session["dbACC"].ToString() + ".dbo.rptTrailBalanceGroup 1," + Session["ComId"] +
            //                    ",'" + criteria + "', '" + strId + "'," + model.CurrId + " ");
            //            }
            //            else ///no report still build
            //            {
            //                postData.Add("Acc_rptCode", "1009");
            //                postData.Add("sqlQuery",
            //                    "Exec " + Session["dbACC"].ToString() + ".dbo.rptCompareTrialBalance '" + criteria +
            //                    "', '" + comid + "', '" + strId + "' ");
            //            }
            //        }
            //    }
            //    if (RptType.ToUpper().ToString() == "MC".ToUpper())
            //    {
            //        postData.Add("Acc_rptCode", "1012");
            //        postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptMaterialConsumed " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "'," + model.CurrId + " ");
            //    }
            //    if (RptType.ToUpper().ToString() == "COGS".ToUpper())
            //    {
            //        if (model.isCompare == false)
            //        {
            //            postData.Add("Acc_rptCode", "1014");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptCOGS " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','"+isZero+"','"+model.CurrId+"' ");
            //        }
            //        else
            //        {
            //            postData.Add("Acc_rptCode", "1015");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptCOGSCompare " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "'");
            //        }
            //    }
            //    if (RptType.ToUpper().ToString() == "IS".ToUpper())
            //    {
            //        if (model.isCompare == false)
            //        {
            //            postData.Add("Acc_rptCode", "1016");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptIncome " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "' ");
            //        }
            //        else
            //        {
            //            postData.Add("Acc_rptCode", "1017");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptIncomeCompare " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "',0 ");
            //        }
            //    }
            //    if (RptType.ToUpper().ToString() == "OE".ToUpper())
            //    {
            //        if (model.isCompare == false)
            //        {
            //            postData.Add("Acc_rptCode", "1018");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptOwnersEquityCarry " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "',1 ");
            //        }
            //        else
            //        {
            //            postData.Add("Acc_rptCode", "1019");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptOwnersEquityCarry " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "',1 ");
            //        }
            //    }
            //    if (RptType.ToUpper().ToString() == "BS".ToUpper())
            //    {
            //        if (model.isCompare == false)
            //        {
            //            postData.Add("Acc_rptCode", "1020");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptBalanceSheet " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "',1 ");
            //        }
            //        else
            //        {
            //            postData.Add("Acc_rptCode", "1021");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptBalanceSheetCompare " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "',1");
            //        }
            //    }
            //    if (RptType.ToUpper().ToString() == "FA".ToUpper())
            //    {
            //        if (model.isCompare == false)
            //        {
            //            postData.Add("Acc_rptCode", "1022");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptAssets " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + isZero + "','" + model.CurrId + "' ");
            //        }
            //    }
            //    if (RptType.ToUpper().ToString() == "NR".ToUpper())
            //    {
            //        if (model.isCompare == false)
            //        {
            //            postData.Add("Acc_rptCode", "1023");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptNotes " + Session["UserId"] + ",'" + comid + "','" + criteria + "',                          '" + strId + "','" + model.CurrId + "',1 ");
            //        }
            //        else
            //        {
            //            postData.Add("Acc_rptCode", "1024");
            //            postData.Add("sqlQuery", "Exec " + Session["dbACC"].ToString() + ".dbo.rptNotesCompare " + Session["UserId"] + ",'" + comid + "','" + criteria + "', '" + strId + "','" + model.CurrId + "',0");
            //        }
            //    }

            //    postData.Add("Acc_rptFormat", rptFormat);
            //    postData.Add("Acc_rptMoreVariable", 0);
            return View();

            ////return RedirectAndPostActionResult.RedirectAndPost("http://27.147.251.124/acs.mis/frmGeneratingReport.aspx", postData);
        }

        public ActionResult PrintReport(int? id, string type)
        {
            try
            {

                return RedirectToAction("Index", "ReportViewer");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost, ActionName("SetSession")]
        public JsonResult SetSession(string criteria, string rptFormat, string rpttype, int? Currency, int? isCompare, int? isCumulative, int? isShowZero, int? isGroup, int? FYId, int? FYHId, int? FYQId, int? FYMId, string FromDate, string ToDate, int? AccIdGroup, int? PrdUnitId)
        {

            try
            {
                string dtFrom = DateTime.Now.ToString();
                string dtTo = DateTime.Now.ToString();

                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");
                int strId = 0;


                if (criteria == "fy")
                {
                    if (FYId != null || FYId.Value > 0)
                    {
                        PF_FiscalYear fiscalyear = db.PF_FiscalYear.Where(x => x.FYId == FYId).FirstOrDefault();
                        dtFrom = fiscalyear.OpDate;
                        dtTo = fiscalyear.ClDate;
                    }
                }

                var reportname = "";
                var filename = "";


                //if (rpttype == "RP")
                //{

                //    if (AccIdRecPay > 0)
                //    {

                //        reportname = "rptRecPayInd";

                //    }
                //    else
                //    {
                //        reportname = "rptRecPay";
                //    }


                //    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                //    {
                //        filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                //        Session["reportquery"] = "Exec rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ";

                //    }
                //    else
                //    {
                //        filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                //        Session["reportquery"] = "Exec rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ";

                //    }

                //}



                //var strId = 0;
                //var isCom = 0;
                //var isShowZero = 0;
                //var isGroup = 0;
                //if (model.isCompare == true) { isCom = 1; }
                //if (model.isShowZero == true) { isShowZero = 1; }
                //if (model.isGroup == true) { isGroup = 1; }

                if (criteria == "Year")
                {
                    if (FYId != null || FYId.Value > 0)
                    {
                        PF_FiscalYear fiscalyear = db.PF_FiscalYear.Where(x => x.FYId == FYId).FirstOrDefault();
                        strId = fiscalyear.FYId;
                        dtFrom = fiscalyear.OpDate;
                        dtTo = fiscalyear.ClDate;
                    }
                }
                if (criteria.ToUpper().ToString() == "HYear".ToUpper())
                {
                    if (FYHId != null || FYHId.Value > 0)
                    {
                        Acc_FiscalHalfYear fiscalhalfyear = db.Acc_FiscalHalfYears.Where(x => x.FiscalHalfYearId == FYHId).FirstOrDefault();
                        strId = fiscalhalfyear.FiscalHalfYearId;
                        dtFrom = fiscalhalfyear.dtFrom;
                        dtTo = fiscalhalfyear.dtTo;
                    }
                }
                if (criteria.ToUpper().ToString() == "Quarter".ToUpper())
                {
                    if (FYHId != null || FYHId.Value > 0)
                    {
                        Acc_FiscalQtr fiscalqtr = db.Acc_FiscalQtrs.Where(x => x.FiscalQtrId == FYQId).FirstOrDefault();
                        strId = fiscalqtr.FiscalQtrId;
                        dtFrom = fiscalqtr.dtFrom;
                        dtTo = fiscalqtr.dtTo;
                    }
                }
                if (criteria.ToUpper().ToString() == "Month".ToUpper())
                {
                    if (FYMId != null || FYMId.Value > 0)
                    {
                        PF_FiscalMonth fiscalmonth = db.PF_FiscalMonth.Where(x => x.MonthId == FYMId).FirstOrDefault();
                        strId = fiscalmonth.FiscalMonthId;
                        dtFrom = fiscalmonth.dtFrom;
                        dtTo = fiscalmonth.dtTo;
                    }
                }



                ///postData.Add("Acc_rptFormat", rptFormat);
                if (rpttype.ToUpper().ToString() == "TB".ToUpper())
                {


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        if (FYMId != null || FYMId.Value > 0)
                        {
                            dtFrom = FromDate;
                            dtTo = ToDate;
                        }


                        if (isGroup == 0)
                        {
                            reportname = "rptTrialBalance";

                            filename = "TrialBalance_Date_" + dtFrom + "_To_" + dtTo;
                            HttpContext.Session.SetString("reportquery", "Exec PF_rptTrailBalanceDate  '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + " ,'" + dtFrom + "' ,'" + dtTo + "' ,'" + AccIdGroup + "'  ");

                        }
                        else
                        {
                            reportname = "rptTrialBalanceGroup";

                            filename = "TrialBalanceGroup_Date_" + dtFrom + "_To_" + dtTo;
                            HttpContext.Session.SetString("reportquery", "Exec PF_rptTrailBalanceGroupDate  '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + " ,'" + dtFrom + "' ,'" + dtTo + "' ,'" + AccIdGroup + "'  ");

                        }


                    }
                    else
                    {





                        if (isGroup == 0)
                        {
                            if (isCompare == 0)
                            {
                                reportname = "rptTrialBalance";

                                filename = "TrialBalance_Date_" + dtFrom + "_To_" + dtTo;
                                HttpContext.Session.SetString("reportquery", "Exec PF_rptTrailBalance '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + ",'" + AccIdGroup + "' ");
                                //string query =/*"reportquery",*/ "Exec PF_rptTrailBalance '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + ",'" + AccIdGroup + "' ";

                            }
                            else
                            {
                                reportname = "rptTrialBalanceComp";
                                filename = "TrialBalanceCompare_Date_" + dtFrom + "_To_" + dtTo;

                                HttpContext.Session.SetString("reportquery", "Exec PF_rptCompareTrialBalance '" + criteria + "', '" + comid + "', '" + strId + "'," + isShowZero + " ," + Currency + " ,'" + AccIdGroup + "' ");

                            }
                        }
                        else
                        {
                            if (isCompare == 0)
                            {
                                reportname = "rptTrialBalanceGroup";

                                filename = "TrialBalanceWithGroup_Date_" + dtFrom + "_To_" + dtTo;
                                HttpContext.Session.SetString("reportquery", "Exec PF_rptTrailBalanceGroup '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + "," + Currency + " ,'" + AccIdGroup + "'");

                            }
                            else ///no report still build
                            {
                                reportname = "rptTrialBalanceCompGroup";
                                filename = "TrialBalanceCompareGroup_Date_" + dtFrom + "_To_" + dtTo;
                                HttpContext.Session.SetString("reportquery", "Exec PF_rptCompareTrialBalanceGroup '" + criteria + "', '" + comid + "', '" + strId + "', " + isShowZero + "," + Currency + " ,'" + AccIdGroup + "' ");
                            }
                        }




                    }
                }




                else if (rpttype.ToUpper().ToString() == "MC".ToUpper())
                {
                    reportname = "rptMaterialConsumed";
                    filename = "MaterialConsumed_Date_" + dtFrom + "_To_" + dtTo;
                    HttpContext.Session.SetString("reportquery", "Exec Acc_rptMaterialConsumed '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "'," + Currency + " ");

                }


                else if (rpttype.ToUpper().ToString() == "COGS".ToUpper())
                {
                    if (isCompare == 1)
                    {
                        reportname = "rptCOGSCompare";
                        filename = "CostOfGoodsSoldCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCOGSCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "'");

                    }
                    else if (isCumulative == 1)
                    {
                        reportname = "rptCOGSCompare";
                        filename = "CostOfGoodsSold_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCOGSCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");
                    }
                    else
                    {
                        reportname = "rptCOGS";
                        filename = "CostOfGoodsSold_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCOGS '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");

                    }
                }


                else if (rpttype.ToUpper().ToString() == "COB".ToUpper())
                {
                    if (isCompare == 1)
                    {
                        reportname = "rptCostBreakupCompare";
                        filename = "rptCostBreakupCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCostBreakupCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "'");


                    }
                    else if (isCumulative == 1)
                    {
                        reportname = "rptCostBreakupCompare";
                        filename = "rptCostBreakupCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCostBreakupCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "'");


                    }
                    else
                    {
                        reportname = "rptCostBreakup";
                        filename = "rptCostBreakup_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCostBreakup '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");

                    }
                }



                else if (rpttype.ToUpper().ToString() == "IS".ToUpper())
                {
                    if (isCompare == 1)
                    {

                        reportname = "rptIncomeCompare";
                        filename = "IncomeStatementCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptIncomeCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ");
                        //string query =/*"reportquery",*/ "Exec Acc_rptIncomeCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ";

                    }
                    else if (isCumulative == 1)
                    {

                        reportname = "rptIncomeCompare";
                        filename = "IncomeStatementCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptIncomeCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ");

                    }
                    else
                    {
                        //reportname = "rptIncome";
                        //filename = "IncomeStatement_Date_" + dtFrom + "_To_" + dtTo;
                        //HttpContext.Session.SetString("reportquery", "Exec Acc_rptIncome '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");

                        reportname = "rptIncome";
                        filename = "IncomeStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rpt_Income '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");
                        //string query =/*"reportquery",*/ "Exec PF_rpt_Income '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ";


                    }
                }

                /////complete


                if (rpttype.ToUpper().ToString() == "OE".ToUpper())
                {
                    if (isCompare == 0)
                    {
                        reportname = "rptOwnersEquityCarry";
                        filename = "OwnersEquity_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptOwnersEquityCarry '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");

                    }
                    else
                    {
                        reportname = "rptOwnersEquityCarryComp";
                        filename = "OwnersEquityCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptOwnersEquityCarry '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");

                    }
                }



                else if (rpttype.ToUpper().ToString() == "BS".ToUpper())
                {
                    if (isCompare == 0)
                    {
                        reportname = "rptBalanceSheet";
                        filename = "BalanceSheet_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptBalanceSheet '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");
                        //string query =/*"reportquery",*/ "Exec Acc_rptBalanceSheet '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ";
                    }

                    else
                    {

                        reportname = "rptBalanceSheetCompFinal";
                        filename = "BalanceSheetCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec pf_rptBalanceSheetCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "',1");

                    }
                }


                else if (rpttype.ToUpper().ToString() == "FA".ToUpper())
                {
                    if (isCompare == 0)
                    {


                        reportname = "rptAssets";
                        filename = "Asset_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptAssets '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "CF".ToUpper())
                {
                    if (isCompare == 0)
                    {


                        reportname = "rptCashFlow";
                        filename = "CashFlow_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptCashFlow '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "FF".ToUpper())
                {
                    if (isCompare == 0)
                    {


                        reportname = "rptFundFlow";
                        filename = "CashFlow_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptFundFlow '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "MR".ToUpper())
                {
                    if (isCompare == 0)
                    {


                        reportname = "rptManagementRatio";
                        filename = "Management_Ration_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptManagementRatio '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "NR".ToUpper())
                {
                    if (isCompare == 1)
                    {
                        reportname = "rptNotesComp";
                        filename = "NotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotesCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                    }
                    else if (isCumulative == 1)
                    {
                        reportname = "rptNotesComp";
                        filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotesCumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                    else
                    {
                        reportname = "rptNotes";
                        filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                }


                else if (rpttype.ToUpper().ToString() == "NBSR".ToUpper())
                {

                    //reportname = "rptNotesBalanceSheet";
                    //filename = "NotesBalanceSheetReprot_Date_" + dtFrom + "_To_" + dtTo;
                    //HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_Balancesheet '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");


                    if (isCompare == 1)
                    {
                        reportname = "rptNotesBalanceSheetCompare";
                        filename = "NotesBalanceSheetReport_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_Balancesheet_Cumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                    }
                    //if (isCumulative == 1)
                    //{
                    //    reportname = "rptNotesBalanceSheetCompare";
                    //    filename = "NotesBalanceSheetReport_Date_" + dtFrom + "_To_" + dtTo;
                    //    HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_Balancesheet_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    //}
                    else
                    {
                        reportname = "rptNotesBalanceSheet";
                        filename = "NotesBalanceSheetReport_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_Balancesheet '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "NISR".ToUpper())
                {


                    if (isCompare == 1)
                    {
                        reportname = "rptNotesIncomeStatementCompare";
                        filename = "NotesIncomeStatementReport_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_IncomeStatement_Cumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                    }
                    if (isCumulative == 1)
                    {
                        reportname = "rptNotesIncomeStatementCompare";
                        filename = "NotesIncomeStatementReport_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_IncomeStatement_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                    else
                    {
                        reportname = "rptNotesIncomeStatement";
                        filename = "NotesIncomeStatementReport_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_IncomeStatement '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                }

                else if (rpttype.ToUpper().ToString() == "NBCIC".ToUpper())
                {
                    if (isCompare == 1)
                    {
                        reportname = "rptNotesBCIC";
                        filename = "NotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_BCIC_Compare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                    }
                    else if (isCumulative == 1)
                    {
                        reportname = "rptNotesBCIC";
                        filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_BCIC_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                    else
                    {
                        reportname = "rptNotesBCIC";
                        filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_BCIC '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                }

                else if (rpttype.ToUpper().ToString() == "NFA".ToUpper())
                {
                    if (isCompare == 1)
                    {
                        reportname = "rptNotes_FixedAsset";
                        filename = "NotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_FixedAsset_Compare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                    }
                    else if (isCumulative == 1)
                    {
                        reportname = "rptNotes_FixedAsset";
                        filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_FixedAsset_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                    else
                    {
                        reportname = "rptNotes_FixedAsset";
                        filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_FixedAsset '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    }
                }


                HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");

                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";

                HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //string redirectUrl = callBackUrl;

                //var vals = reportid.Split(',')[0];

                //need change
                ////// redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintReport", "GeneralReport", new { id = 0 }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }

                TempData["Status"] = "2";
                TempData["Message"] = "Summary Report";
                TempData["FileName"] = filename;
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), reportname, "Report", reportname); //not working 
                string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });
                return Json(new { Url = redirectUrl });

            }

            catch (Exception ex)
            {
                //throw ex;
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }
    }


}

