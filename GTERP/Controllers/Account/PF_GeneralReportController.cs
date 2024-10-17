using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Controllers.Account
{
    public class PF_GeneralReportController : Controller
    {
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        //public CommercialRepository Repository { get; set; }

        private GTRDBContext db;
        POSRepository POS;

        public PF_GeneralReportController(GTRDBContext context, POSRepository _POS, TransactionLogRepository trans)
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


                PF_VoucherMain Vouchermain = db.PF_VoucherMains
                    .Include(b => b.VoucherSubs).ThenInclude(b => b.PF_ChartOfAccount).ThenInclude(b => b.ParentChartOfAccount)
                    .Include(a => a.VoucherSubs).ThenInclude(a => a.VoucherSubSections).ThenInclude(a => a.SubSection)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.VoucherSubChecnoes)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.HR_Emp_Infos)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.Customers)
                    .Include(x => x.VoucherSubs).ThenInclude(x => x.Suppliers)
                    .Include(x => x.Acc_Currency)
                    .Include(x => x.Acc_CurrencyLocal)
                    .Include(x => x.Acc_FiscalMonths)
                    .Include(x => x.PF_FiscalYear)

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
                var quary = $"EXEC PF_rptLedgerMultiDrCr '{comid}',{AccId},'{FYId}','{dtFrom}','{dtTo}' ,'{CountryId}','{IsLocalCurrency}','{userid}','{SupplierId}','{CustomerId}','{EmployeeId}' ";

                //Exec PF_rptLedgerMultiDrCr '31312c54-659b-4e63-b4ba-2bc3d7b05792', 10165, 0, '01-Jul-2020', '28-Nov-2020', 18,0 ,'4864add7-0ab2-4c4f-9eb8-6b63a425e665' , '', '', ''
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

                List<LedgerDetailsModel> bookingDeliveryChallan = Helper.ExecProcMapTList<LedgerDetailsModel>("PF_rptLedgerMultiDrCr", parameters);

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


            this.ViewBag.AccIdRecPay = new SelectList(db.PF_ChartOfAccounts.Where(p => p.comid == comid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true && p.AccCode.Contains("1-1-1")).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            this.ViewBag.AccIdLedger = new SelectList(db.PF_ChartOfAccounts.Where(p => p.comid == comid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            this.ViewBag.AccIdGroup = new SelectList(db.PF_ChartOfAccounts.Where(p => p.comid == comid && p.AccType == "G").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
            //this.ViewBag.AccIdNoteOneCT = new SelectList(db.PF_VoucherSubs
            //    .Include(x=>x.PF_VoucherMain)
            //    .Where(p => p.PF_VoucherMain.comid == comid && p.Note1.Contains("CT"))
            //    .Select(s => new { Text = s.Note1 , Value = s.VoucherSubId }).Distinct()
            //    .ToList(), "Value", "Text");
            this.ViewBag.AccIdNoteOneCT = new SelectList(db.PF_VoucherSubs
            .Include(x => x.PF_VoucherMain)
            .Where(p => p.PF_VoucherMain.comid == comid && p.Note1.Contains("CT") && p.Note1.Length > 3)
            .Select(s => new { Text = s.Note1, Value = s.Note1 }).Distinct()
            .ToList(), "Value", "Text");




            this.ViewBag.EmployeeId = new SelectList(db.HR_Emp_Info.Where(p => p.ComId == comid).Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ] " + " - [" + s.Cat_Designation.DesigName + "]", Value = s.EmpId }).ToList(), "Value", "Text");
            this.ViewBag.SupplierId = new SelectList(db.Suppliers.Where(p => p.ComId == comid).Select(s => new { Text = s.SupplierName + " - [ " + s.SupplierCode + " ]", Value = s.SupplierId }).ToList(), "Value", "Text");
            this.ViewBag.CustomerId = new SelectList(db.Customers.Take(10).Where(p => p.comid == comid).Select(s => new { Text = s.CustomerName + " - [ " + s.CustomerCode + " ]", Value = s.CustomerId }).ToList(), "Value", "Text");


            //ViewBag.AccIdRecPay = new SelectList(POS.GetChartOfAccountCashAndBank(comid), "AccId", "AccName");
            //ViewBag.AccIdLedger = new SelectList(POS.GetChartOfAccountLedger(comid), "AccId", "AccName");
            //ViewBag.AccIdGroup = new SelectList(POS.GetChartOfAccountGroup(comid), "AccId", "AccName");



            ViewBag.PrdUnitId = new SelectList(db.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
            ViewBag.VoucherTypeId = new SelectList(db.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName").ToList();



            PFShowVoucherViewModel model = new PFShowVoucherViewModel();
            List<PF_FiscalYear> fiscalyear = db.PF_FiscalYear.Where(x => x.ComId == comid).ToList();
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
                int strId = 0;
                int isShowZero = 0;

                if (criteria == "fy")
                {
                    if (FYId != null || FYId.Value > 0)
                    {
                        PF_FiscalYear fiscalyear = db.PF_FiscalYear.Where(x => x.FYId == FYId).FirstOrDefault();
                        strId = fiscalyear.FYId;
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
                        filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.PF_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.PF_VoucherType.VoucherTypeName).Single();
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ");

                    }
                    else
                    {
                        filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.PF_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.PF_VoucherType.VoucherTypeName).Single();
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ");

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
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptLedgerMultiDrCr_CashBook '" + comid + "', " + AccIdRecPay + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + ",'" + userid + "'  ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptLedgerMultiDrCr_CashBook '" + comid + "', " + AccIdRecPay + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + " ,'" + userid + "'  ");


                    }



                }


                else if (rpttype.ToUpper().ToString() == "Tran".ToUpper())
                {

                    reportname = "rptTransaction";

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "Transaction_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptTransaction '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + " ," + AccVoucherTypeId + "  ");


                    }
                    else
                    {
                        filename = "Transaction_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptTransaction '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + "  ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "TranVoucherNo".ToUpper())
                {

                    reportname = "rptTransaction_VoucherNo";

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "Transaction_VoucherNo_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptTransaction_VoucherNo '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + "  ");


                    }
                    else
                    {
                        filename = "Transaction_VoucherNo_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptTransaction_VoucherNo '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + "  ");

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
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + SupplierId + "', '" + CustomerId + "', '" + EmployeeId + "'  ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + SupplierId + "',  '" + CustomerId + "', '" + EmployeeId + "'   ");


                    }
                }

                else if (rpttype.ToUpper().ToString() == "ledgerA".ToUpper())
                {

                    reportname = "rptLedgerDetails_All";

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptLedgerMultiAll '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + MinAccCode + "',  '" + MaxAccCode + "', '" + EmployeeId + "'  ");


                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptLedgerMultiAll '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + MinAccCode + "',  '" + MaxAccCode + "', '" + EmployeeId + "'   ");


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
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'  ");


                    }
                    else
                    {
                        filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'   ");


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
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'  ");


                    }
                    else
                    {
                        filename = "SupplierVat" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'   ");


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
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'  ");
                    }
                    else
                    {
                        filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'   ");
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
                        HttpContext.Session.SetString("reportquery", "Exec [PF_rptAdvanceMoney_Supplier_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Supplier' , null,'" + AccIdLedger + "'");
                    }
                    else
                    {
                        filename = "rptAdvanceMoney_Supplier_Schedule" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [PF_rptAdvanceMoney_Supplier_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Supplier' ,'" + FYId.ToString() + "','" + AccIdLedger + "' ");
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
                        HttpContext.Session.SetString("reportquery", "Exec [PF_rptAdvanceMoney_Employee_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Employee' , null ,'" + AccIdLedger + "' ");
                    }
                    else
                    {
                        filename = "rptAdvanceMoney_Employee_Schedule" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec [PF_rptAdvanceMoney_Employee_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Employee' ,'" + FYId.ToString() + "','" + AccIdLedger + "' ");
                    }
                }

                else if (rpttype.ToUpper().ToString() == "GroupD".ToUpper())
                {

                    reportname = "rptGroupDetails";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  PF_rptGroupMultiDrCr '" + comid + "', " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "'  ");

                    }
                    else
                    {
                        filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  PF_rptGroupMultiDrCr '" + comid + "', " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "'  ");

                    }
                }

                else if (rpttype.ToUpper().ToString() == "GroupS".ToUpper())
                {

                    reportname = "rptSubsidiaryLedger";
                    var Note1 = AccIdNoteOneCT.Replace("&", "");

                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "SubsidiaryLedger_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  PF_rptSubsidiaryLedger '" + comid + "', " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "' ,'" + Note1 + "' ");

                    }
                    else
                    {
                        filename = "SubsidiaryLedger_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  PF_rptSubsidiaryLedger '" + comid + "', " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "' ,'" + Note1 + "' ");

                    }
                }


                else if (rpttype.ToUpper().ToString() == "BankRecon".ToUpper())
                {

                    reportname = "rptBankReconciliation";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "BankReconciliation_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptBankReconciliation] '" + comid + "', " + AccIdLedger + ",'" + dtFrom + "', '" + dtTo + "' ,'" + userid + "'  ");

                    }
                    else
                    {
                        filename = "BankReconciliation_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptBankReconciliation] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "' ,'" + userid + "'  ");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "CashFlow".ToUpper())
                {

                    reportname = "rptCashFlowStatement";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptCashFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'");

                    }
                    else
                    {
                        filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptCashFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "FundFlow".ToUpper())
                {

                    reportname = "rptFundFlowStatement";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "FundFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptFundFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'");

                    }
                    else
                    {
                        filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptFundFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "FdrList".ToUpper())
                {

                    reportname = "rptFDRList";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptFDRList] '" + comid + "', '" + dtFrom + "', '" + dtTo + "'");

                    }
                    else
                    {
                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptFDRList] '" + comid + "' , '" + dtFrom + "', '" + dtTo + "'");

                    }
                }
                else if (rpttype.ToUpper().ToString() == "FdrListV".ToUpper())
                {

                    reportname = "rptFDRList_Voucher";


                    if (criteria.ToUpper().ToString() == "Date".ToUpper())
                    {

                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptFDRList_Voucher] '" + comid + "', '" + dtFrom + "', '" + dtTo + "'");

                    }
                    else
                    {
                        filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec  [PF_rptFDRList_Voucher] '" + comid + "' , '" + dtFrom + "', '" + dtTo + "'");

                    }
                }

                //PF Accounts------------------------------//
                else if (rpttype.ToUpper().ToString() == "BSPF".ToUpper())
                {
                    //if (isCompare == 0)
                    //{
                    //    reportname = "rptBalanceSheet_PF";
                    //    filename = "PF_BalanceSheet_Date_" + dtFrom + "_To_" + dtTo;
                    //    HttpContext.Session.SetString("reportquery", "Exec PF_BalanceSheet '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");
                    //}

                    //else
                    {

                        reportname = "rptBalanceSheet_PF";
                        filename = "PF_BalanceSheetCompare_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_BalanceSheet '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "',1");

                    }
                }

                else if (rpttype.ToUpper().ToString() == "ISPF".ToUpper())
                {
                    //if (isCompare == 1)
                    //{

                    //    reportname = "rptIncomeCompare";
                    //    filename = "IncomeStatementCompare_Date_" + dtFrom + "_To_" + dtTo;
                    //    HttpContext.Session.SetString("reportquery", "Exec PF_rptIncomeCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ");

                    //}
                    //else if (isCumulative == 1)
                    //{

                    //    reportname = "rptIncomeCompare";
                    //    filename = "IncomeStatementCompare_Date_" + dtFrom + "_To_" + dtTo;
                    //    HttpContext.Session.SetString("reportquery", "Exec PF_rptIncomeCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ");

                    //}
                    //else
                    {
                        reportname = "rptIncome_PF";
                        filename = "PF IncomeStatement_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptIncome '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");
                    }
                }

                else if (rpttype.ToUpper().ToString() == "NRPF".ToUpper())
                {
                    //if (isCompare == 1)
                    //{
                    //    reportname = "rptNotes_PF";
                    //    filename = "PFNotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                    //    HttpContext.Session.SetString("reportquery", "Exec PF_rptNotes_Compare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                    //}
                    //else if (isCumulative == 1)
                    //{
                    //    reportname = "rptNotes_PF";
                    //    filename = "PFNotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    //    HttpContext.Session.SetString("reportquery", "Exec PF_rptNotes_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                    //}
                    //else
                    {
                        reportname = "rptNotes_PF";
                        filename = "PFNotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                        HttpContext.Session.SetString("reportquery", "Exec PF_rptNotes '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

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


                string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }

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