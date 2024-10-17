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
using Microsoft.EntityFrameworkCore;

namespace GTERP.Controllers.Account
{
    public class Acc_GeneralReportController : Controller
    {
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        //public CommercialRepository Repository { get; set; }

        private GTRDBContext db;
        POSRepository POS;

        public Acc_GeneralReportController(GTRDBContext context, POSRepository _POS, TransactionLogRepository trans)
        {
            db = context;
            POS = _POS;
            //Repository = rep;
        }




        public partial class LedgerDetailsModel
        {
            public string ComName { get; set; }
            public string ComAdd1 { get; set; }
            public string ComAdd2 { get; set; }
            public string ComLogo { get; set; }
            public string ComImgPath { get; set; }
            public string Caption { get; set; }
            public string Caption2 { get; set; }
            public string CaptionCashBook { get; set; }

            public string VoucherId { get; set; }
            public string VoucherNo { get; set; }
            public string dtVoucher { get; set; }
            public string VoucherDate { get; set; }
            public string VoucherDesc { get; set; }
            public string Note1 { get; set; }
            public string Note2 { get; set; }
            public string AccId { get; set; }
            public string AccCode { get; set; }
            public string AccName { get; set; }
            public string AccId1 { get; set; }
            public string AccCode1 { get; set; }
            public string AccName1 { get; set; }
            public string TKDebit { get; set; }
            public string TKCredit { get; set; }
            public string TKDebit1 { get; set; }
            public string TKCredit1 { get; set; }
            public string TKDebit2 { get; set; }
            public string TKCredit2 { get; set; }
            public string RowNo { get; set; }
            public string RowDr { get; set; }
            public string RowCr { get; set; }
            public string Amount { get; set; }
            public string intFlag { get; set; }
            public string IsBatch { get; set; }
            public string OpBalance { get; set; }
            public string ClBalance { get; set; }
            public string ttlDebit { get; set; }
            public string ttlCredit { get; set; }
            public string referance { get; set; }
            public string ReferanceTwo { get; set; }
            public string ReferanceThree { get; set; }
            public string Currency { get; set; }
            public string AccNameOrg { get; set; }
            public string ParentNameOne { get; set; }
            public string ParentNameTwo { get; set; }
            public string ParentNameThree { get; set; }
            public string ParentNameFour { get; set; }
            public string ParentNameFive { get; set; }
            public string ChkNo { get; set; }


        }


        public ActionResult VoucherReport(int VoucherId)
        {
            try
            {

                //var result = "";
                //var comid = HttpContext.Session.GetString("comid");
                //var userid = HttpContext.Session.GetString("userid");

                //if (comid == null)
                //{
                //    result = "Please Login first";
                //}
                //var quary = $"EXEC Acc_rptLedgerMultiDrCr '{comid}',{AccId},'{FYId}','{dtFrom}','{dtTo}' ,'{CountryId}','{IsLocalCurrency}','{userid}','{SupplierId}','{CustomerId}','{EmployeeId}' ";

                ////Exec Acc_rptLedgerMultiDrCr '31312c54-659b-4e63-b4ba-2bc3d7b05792', 10165, 0, '01-Jul-2020', '28-Nov-2020', 18,0 ,'4864add7-0ab2-4c4f-9eb8-6b63a425e665' , '', '', ''
                //AccId = "10165";
                //FYId = "0";
                //dtFrom = "01-Jul-2020";
                //dtTo = "30-Nov-2020";
                //CountryId = "18";
                //IsLocalCurrency = "0";
                //SupplierId = "";
                //CustomerId = "";
                //EmployeeId = "";




                //SqlParameter[] parameters = new SqlParameter[11];

                //parameters[0] = new SqlParameter("@ComId", comid);
                //parameters[1] = new SqlParameter("@AccId", AccId);
                //parameters[2] = new SqlParameter("@FYId", FYId);

                //parameters[3] = new SqlParameter("@dtFrom", dtFrom);
                //parameters[4] = new SqlParameter("@dtTo", dtTo);
                //parameters[5] = new SqlParameter("@CountryId", CountryId);
                //parameters[6] = new SqlParameter("@IsLocalCurrency", IsLocalCurrency);

                //parameters[7] = new SqlParameter("@userid", userid);
                //parameters[8] = new SqlParameter("@SupplierId", SupplierId);
                //parameters[9] = new SqlParameter("@CustomerId", CustomerId);
                //parameters[10] = new SqlParameter("@EmployeeId", EmployeeId);

                //List<LedgerDetailsModel> bookingDeliveryChallan = Helper.ExecProcMapTList<LedgerDetailsModel>("Acc_rptLedgerMultiDrCr", parameters);



                Acc_VoucherMain Vouchermain = db.Acc_VoucherMains
                    .Include(b => b.VoucherSubs).ThenInclude(b => b.Acc_ChartOfAccount).ThenInclude(b => b.ParentChartOfAccount)
                    .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                    .Include(x => x.Acc_Currency)
                    .Include(x => x.Acc_CurrencyLocal)
                    .Include(x => x.Acc_CurrencyLocal)
                    .Include(x => x.Acc_FiscalMonths)
                    .Include(x => x.Acc_FiscalYears)

                    .Where(x => x.VoucherId == VoucherId).FirstOrDefault();


                return View(Vouchermain);

                //return Json(new { bookingDeliveryChallan, ex = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult DemoReport(string AccId, string FYId, string dtFrom, string dtTo, string CountryId, string IsLocalCurrency, string SupplierId, string CustomerId, string EmployeeId)
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
                var quary = $"EXEC Acc_rptLedgerMultiDrCr '{comid}',{AccId},'{FYId}','{dtFrom}','{dtTo}' ,'{CountryId}','{IsLocalCurrency}','{userid}','{SupplierId}','{CustomerId}','{EmployeeId}' ";

                //Exec Acc_rptLedgerMultiDrCr '31312c54-659b-4e63-b4ba-2bc3d7b05792', 10165, 0, '01-Jul-2020', '28-Nov-2020', 18,0 ,'4864add7-0ab2-4c4f-9eb8-6b63a425e665' , '', '', ''
                AccId = "10165";
                FYId = "0";
                dtFrom = "01-Jul-2020";
                dtTo = "30-Nov-2020";
                CountryId = "18";
                IsLocalCurrency = "0";
                SupplierId = "";
                CustomerId = "";
                EmployeeId = "";




                SqlParameter[] parameters = new SqlParameter[11];

                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@AccId", AccId);
                parameters[2] = new SqlParameter("@FYId", FYId);

                parameters[3] = new SqlParameter("@dtFrom", dtFrom);
                parameters[4] = new SqlParameter("@dtTo", dtTo);
                parameters[5] = new SqlParameter("@CountryId", CountryId);
                parameters[6] = new SqlParameter("@IsLocalCurrency", IsLocalCurrency);

                parameters[7] = new SqlParameter("@userid", userid);
                parameters[8] = new SqlParameter("@SupplierId", SupplierId);
                parameters[9] = new SqlParameter("@CustomerId", CustomerId);
                parameters[10] = new SqlParameter("@EmployeeId", EmployeeId);

                List<LedgerDetailsModel> bookingDeliveryChallan = Helper.ExecProcMapTList<LedgerDetailsModel>("Acc_rptLedgerMultiDrCr", parameters);

                return View(bookingDeliveryChallan);

                //return Json(new { bookingDeliveryChallan, ex = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // GET: Section
        public ActionResult Report()
        {
            string comid = HttpContext.Session.GetString("comid");
            int defaultcountry = (db.Companys.Where(a => a.CompanySecretCode == comid).Select(a => a.CountryId).FirstOrDefault());


            var date = DateTime.Now.ToString("dd-MMM-yyyy");
            var date1 = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.date = date;
            ViewBag.date1 = date1;

            ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", defaultcountry);


            this.ViewBag.AccIdRecPay = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true && p.AccCode.Contains("1-1-1")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            this.ViewBag.AccIdLedger = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            this.ViewBag.AccIdGroup = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "G").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            //this.ViewBag.AccIdNoteOneCT = new SelectList(db.Acc_VoucherSubs
            //    .Include(x=>x.Acc_VoucherMain)
            //    .Where(p => p.Acc_VoucherMain.comid == comid && p.Note1.Contains("CT"))
            //    .Select(s => new { Text = s.Note1 , Value = s.VoucherSubId }).Distinct()
            //    .ToList(), "Value", "Text");
            this.ViewBag.AccIdNoteOneCT = new SelectList(db.Acc_VoucherSubs
            //.Include(x => x.Acc_VoucherMain)
            .Where(p => p.Acc_VoucherMain.ComId == comid && p.Note1.Contains("CT") && (p.Note1 + p.Note2).Length > 3)
            .Select(s => new { Text = s.Note1 + s.Note2, Value = s.Note1 + s.Note2 }).Distinct()
            .ToList(), "Value", "Text");




            this.ViewBag.EmployeeId = new SelectList(db.HR_Emp_Info.Where(p => p.ComId == comid).Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ] " + " - [" + s.Cat_Designation.DesigName + "]", Value = s.EmpId }).ToList(), "Value", "Text");
            this.ViewBag.SupplierId = new SelectList(db.Suppliers.Where(p => p.ComId == comid).Select(s => new { Text = s.SupplierName + " - [ " + s.SupplierCode + " ]", Value = s.SupplierId }).ToList(), "Value", "Text");
            this.ViewBag.CustomerId = new SelectList(db.Customers.Take(10).Where(p => p.comid == comid).Select(s => new { Text = s.CustomerName + " - [ " + s.CustomerCode + " ]", Value = s.CustomerId }).ToList(), "Value", "Text");


            //ViewBag.AccIdRecPay = new SelectList(POS.GetChartOfAccountCashAndBank(comid), "AccId", "AccName");
            //ViewBag.AccIdLedger = new SelectList(POS.GetChartOfAccountLedger(comid), "AccId", "AccName");
            //ViewBag.AccIdGroup = new SelectList(POS.GetChartOfAccountGroup(comid), "AccId", "AccName");



            ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
            ViewBag.VoucherTypeId = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName").ToList();



            ShowVoucherViewModel model = new ShowVoucherViewModel();
            List<Acc_FiscalYear> fiscalyear = db.Acc_FiscalYears.Where(x => x.ComId == comid).ToList();
            int fiscalyid = fiscalyear.Max(p => p.FYId);


            List<Acc_FiscalMonth> fiscalmonth = db.Acc_FiscalMonths.Where(x => x.FYId == fiscalyid).ToList();

            model.FiscalYs = fiscalyear;
            model.ProcessMonths = fiscalmonth;
            model.CountryId = defaultcountry;

            //rptGeneralReport.PrcSetData(model, "Create", dsList);
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(string rpttype, string criteria, string rptFormat)
        {

            //string sqlQuery = "";
            //Dictionary<string, object> postData = new Dictionary<string, object>();
            //string dtOpening = "", dtClose = "";
            //var FYId = 0;
            //var isLocalCurr = 0;
            //if (model.isLocalCurr == true)
            //{
            //    isLocalCurr = 1;
            //}
            //var Material = 0;
            //if (model.isMaterial == true)
            //{
            //    Material = 1;
            //}
            //if (criteria == "fy")
            //{
            //    if (model.FiscalYs != null || model.FiscalYs.Count>0)
            //    {
            //        for (int i = 0; i < model.FiscalYs.Count; i++)
            //        {
            //            if (model.FiscalYs[i].isCheck == true)
            //            {
            //                FYId = int.Parse(model.FiscalYs[i].CloseFYId.ToString());
            //                dtOpening = model.FiscalYs[i].dtOpening;
            //                dtClose = model.FiscalYs[i].dtClose;
            //            }
            //        }
            //    }
            //}
            //if (rpttype == "RP")
            //{

            //    if (int.Parse(AccIdRecPay) > 0)
            //    {
            //        postData.Add("rptCode", "1004");
            //    }
            //    else
            //    {
            //        postData.Add("rptCode", "1003");
            //    }

            //    if (criteria.ToUpper().ToString() == "Date".ToUpper())
            //    {
            //        "Exec Acc_rptRecPay " + Session["comid"] + "," + Session["userid"] + ", '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency+" ");
            //    }
            //    else
            //    {
            //        "Exec Acc_rptRecPay " + Session["comid"] + ", " + Session["userid"] + ", '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ");
            //    }
            //}
            //else if (rpttype.ToUpper().ToString() == "Tran".ToUpper())
            //{
            //    postData.Add("rptCode", "1005");
            //    if (criteria.ToUpper().ToString() == "Date".ToUpper())
            //    {
            //        "Exec Acc_rptTransaction " + Session["comid"] + ", " + Session["userid"] + ", '" + dtFrom + "', '"+ dtTo + "',"+Currency+" ");
            //    }
            //    else
            //    {
            //        "Exec Acc_rptTransaction " + Session["comid"] + ", " + Session["userid"] + ", '" + dtFrom + "', '" + dtTo + "'," + Currency + " ");
            //    }
            //}
            //else if (rpttype.ToUpper().ToString() == "ledgerD".ToUpper())
            //{
            //    postData.Add("rptCode", "1006");
            //    if (criteria.ToUpper().ToString() == "Date".ToUpper())
            //    {
            //        "Exec Acc_rptLedgerMultiDrCr " + Session["comid"] + ", " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + ","+isLocalCurr+" ");
            //    }
            //    else
            //    {
            //        "Exec Acc_rptLedgerMultiDrCr " + Session["comid"] + ", " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + " ");
            //    }
            //}
            //else if (rpttype.ToUpper().ToString() == "GroupD".ToUpper())
            //{
            //    postData.Add("rptCode", "1007");
            //    if (criteria.ToUpper().ToString() == "Date".ToUpper())
            //    {


            //        "Exec  Acc_rptGroupMultiDrCr " + Session["comid"] + ", " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "");
            //    }
            //    else
            //    {
            //        "Exec  Acc_rptGroupMultiDrCr " + Session["comid"] + ", " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + "");
            //    }
            //}

            //postData.Add("rptFormat", rptFormat);
            //postData.Add("rptMoreVariable", 0);
            return View();


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
        public JsonResult SetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo,
            int? Currency, int? isDetails, int? isLocalCurr, int? isMaterial, int? FYId, int? AccIdRecPay, int? AccIdLedger,
            int? AccIdGroup, int? PrdUnitId, int? AccVoucherTypeId,
            int? SupplierId, int? CustomerId, int? EmployeeId, string AccIdNoteOneCT, string MinAccCode, string MaxAccCode)
        {

            try
            {

                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");



                if (criteria == "fy")
                {
                    if (FYId != null || FYId.Value > 0)
                    {
                        Acc_FiscalYear fiscalyear = db.Acc_FiscalYears.Where(x => x.FYId == FYId).FirstOrDefault();
                        dtFrom = fiscalyear.OpDate;
                        dtTo = fiscalyear.ClDate;
                    }
                }

                var reportname = "";
                var filename = "";


                if (rpttype == "RP")
                {

                    if (AccIdRecPay > 0)
                    {

                        reportname = "rptRecPayInd";

                    }
                    else
                    {
                        reportname = "rptRecPay";
                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ," + isLocalCurr + " ");

                    }
                    else
                    {
                        filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + "," + PrdUnitId + "  ");

                    }

                }
                if (rpttype == "CB")
                {


                    //if (isDetails == 1)
                    //{
                    //    reportname = "rptLedgerDetails_Multi";

                    //}
                    //else
                    //{
                    reportname = "rptLedgerDetails_Multi_Summarized_CashBook";

                    //}


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr_CashBook '" + comid + "', " + AccIdRecPay + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + ",'" + userid + "'," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr_CashBook '" + comid + "', " + AccIdRecPay + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + " ,'" + userid + "'," + PrdUnitId + "   ");


                    }



                    ////if (AccIdRecPay > 0)
                    ////{

                    //    reportname = "rptRecPayInd_CashBook";

                    ////}
                    ////else
                    ////{
                    ////    reportname = "rptRecPay";
                    ////}


                    //if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    //{
                    //    filename = "CashBook_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                    //    HttpContext.Session.SetString("reportquery", "Exec Acc_rptCashBook '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + "," + PrdUnitId + "  ");

                    //}
                    //else
                    //{
                    //    filename = "CashBook_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                    //    HttpContext.Session.SetString("reportquery", "Exec Acc_rptCashBook '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + "," + PrdUnitId + "  ");

                    //}

                }


                else if (rpttype.ToUpper().ToString() == "Tran".ToUpper())
                {

                    reportname = "rptTransaction";

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "Transaction_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + " ," + AccVoucherTypeId + "," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "Transaction_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + " ," + PrdUnitId + "  ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "TranVoucherNo".ToUpper())
                {

                    reportname = "rptTransaction_VoucherNo";

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "Transaction_VoucherNo_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction_VoucherNo '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + " ," + PrdUnitId + "  ");


                    }
                    else
                    {
                        filename = "Transaction_VoucherNo_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction_VoucherNo '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + " ," + PrdUnitId + "  ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "ledgerD".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptLedgerDetails_Multi";

                    }
                    else
                    {
                        reportname = "rptLedgerDetails_Multi_Summarized";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + SupplierId + "', '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + SupplierId + "',  '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "    ");


                    }
                }
                else if (rpttype.ToUpper().ToString() == "ledgerFC".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptLedgerDetails_Multi";

                    }
                    else
                    {
                        reportname = "rptLedgerDetails_Multi_Summarized_WithFC";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + SupplierId + "', '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + SupplierId + "',  '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "    ");


                    }
                }
                else if (rpttype.ToUpper().ToString() == "ledgerA".ToUpper())
                {

                    reportname = "rptLedgerDetails_All";

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiAll '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + MinAccCode + "',  '" + MaxAccCode + "', '" + EmployeeId + "'," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiAll '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + MinAccCode + "',  '" + MaxAccCode + "', '" + EmployeeId + "'," + PrdUnitId + "    ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "VC".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptVatCertificate";

                    }
                    else
                    {
                        reportname = "rptVatCertificate";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'," + PrdUnitId + "    ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "SV".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptSupplierVat";

                    }
                    else
                    {
                        reportname = "rptSupplierVat";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "SupplierVat" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'," + PrdUnitId + "   ");


                    }
                    else
                    {
                        filename = "SupplierVat" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "' ," + PrdUnitId + "   ");


                    }
                }
                else if (rpttype.ToUpper().ToString() == "SAIT".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptSupplierAIT";

                    }
                    else
                    {
                        reportname = "rptSupplierAIT";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'," + PrdUnitId + "   ");
                    }
                    else
                    {
                        filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'," + PrdUnitId + "    ");
                    }
                }

                else if (rpttype.ToUpper().ToString() == "ADSUP".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptAdvanceMoney_Supplier_Schedule";

                    }
                    else
                    {
                        reportname = "rptAdvanceMoney_Supplier_Schedule";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "rptAdvanceMoney_Supplier_Schedule" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Supplier_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Supplier' , null,'" + AccIdLedger + "'," + PrdUnitId + " ");
                    }
                    else
                    {
                        filename = "rptAdvanceMoney_Supplier_Schedule" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Supplier_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Supplier' ,'" + FYId.ToString() + "','" + AccIdLedger + "'," + PrdUnitId + "  ");
                    }
                }

                else if (rpttype.ToUpper().ToString() == "ADEMP".ToUpper())
                {
                    if (isDetails == 1)
                    {
                        reportname = "rptAdvanceMoney_Employee_Schedule";

                    }
                    else
                    {
                        reportname = "rptAdvanceMoney_Employee_Schedule";

                    }


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "rptAdvanceMoney_Employee_Schedule" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Employee_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Employee' , null ,'" + AccIdLedger + "'," + PrdUnitId + "  ");
                    }
                    else
                    {
                        filename = "rptAdvanceMoney_Employee_Schedule" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Employee_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Employee' ,'" + FYId.ToString() + "','" + AccIdLedger + "'," + PrdUnitId + "  ");
                    }
                }

                else if (rpttype.ToUpper().ToString() == "GroupD".ToUpper())
                {

                    reportname = "rptGroupDetails";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  Acc_rptGroupMultiDrCr '" + comid + "', " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "'," + PrdUnitId + "   ");

                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  Acc_rptGroupMultiDrCr '" + comid + "', " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "'," + PrdUnitId + "   ");

                    }
                }

                else if (rpttype.ToUpper().ToString() == "GroupS".ToUpper())
                {

                    reportname = "rptSubsidiaryLedger";
                    var Note1 = AccIdNoteOneCT.Replace("&", "");

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "SubsidiaryLedger_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  Acc_rptSubsidiaryLedger '" + comid + "', " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "' ,'" + Note1 + "'," + PrdUnitId + "  ");

                    }
                    else
                    {
                        filename = "SubsidiaryLedger_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  Acc_rptSubsidiaryLedger '" + comid + "', " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "' ,'" + Note1 + "'," + PrdUnitId + "  ");

                    }
                }


                else if (rpttype.ToUpper().ToString() == "BankRecon".ToUpper())
                {

                    reportname = "rptBankReconciliation";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "BankReconciliation_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptBankReconciliation] '" + comid + "', " + AccIdLedger + ",'" + dtFrom + "', '" + dtTo + "' ,'" + userid + "'," + PrdUnitId + "   ");

                    }
                    else
                    {
                        filename = "BankReconciliation_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptBankReconciliation] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "' ,'" + userid + "'," + PrdUnitId + "   ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "CashFlow".ToUpper())
                {

                    reportname = "rptCashFlowStatement";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptCashFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                    else
                    {
                        filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptCashFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "FundFlow".ToUpper())
                {

                    reportname = "rptFundFlowStatement";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "FundFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFundFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                    else
                    {
                        filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFundFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "FdrList".ToUpper())
                {

                    reportname = "rptFDRList";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList] '" + comid + "', '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                    else
                    {
                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList] '" + comid + "' , '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "FdrListV".ToUpper())
                {

                    reportname = "rptFDRList_Voucher";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList_Voucher] '" + comid + "', '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                    else
                    {
                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList_Voucher] '" + comid + "' , '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                    }
                }

                HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");

                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";

                HttpContext.Session.SetObject("rptList", postData);
                // Session["rptList"] = postData;

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");// Session["reportquery"].ToString();
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //string redirectUrl = callBackUrl;

                //TempData["Status"] = "2";
                //TempData["Message"] = "General Report";
                //tranlog.TransactionLog("General Report", "Set Session", "General Report", rpttype, "Report", reportname);

                //return Json(callBackUrl);

                //var vals = reportid.Split(',')[0];

                //need change
                /////redirectUrl = new UrlHelper(Request.).Action("PrintReport", "GeneralReport", new { id = 0 }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
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