using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class GeneralReportRepository : IGeneralReportRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public GeneralReportRepository
            (
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            )
        {
            _context = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IEnumerable<SelectListItem> AccIdGroup()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "G").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccIdLedger()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccIdNoteOneCT()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_VoucherSubs
            .Where(p => p.Acc_VoucherMain.ComId == comid && p.Note1.Contains("CT") && (p.Note1 + p.Note2).Length > 3)
            .Select(s => new { Text = s.Note1 + s.Note2, Value = s.Note1 + s.Note2 }).Distinct()
            .ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccIdRecPay()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.IsBankItem == true || p.IsCashItem == true && p.AccCode.Contains("1-1-1"))
                .Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

        }

        public List<LedgerDetailsModel> BookingDeliveryChallan(string AccId, string FYId, string dtFrom, string dtTo, string CountryId, string IsLocalCurrency, string SupplierId, string CustomerId, string EmployeeId)
        {

            var result = "";
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

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
            return bookingDeliveryChallan;
        }

        public IEnumerable<SelectListItem> CountryId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var defaultcountry = _context.Companys.Where(a => a.CompanyCode == comid).Select(a => a.CountryId).FirstOrDefault();
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", defaultcountry);
        }

        public IEnumerable<SelectListItem> CustomerId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Customers.Take(10).Where(p => p.comid == comid)
                .Select(s => new { Text = s.CustomerName + " - [ " + s.CustomerCode + " ]", Value = s.CustomerId }).ToList(), "Value", "Text");
        }

        public string GeneralSetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo, int? Currency, int? isDetails, int? isLocalCurr, int? isMaterial, int? FYId, int? AccIdRecPay, int? AccIdLedger, int? AccIdGroup, int? PrdUnitId, int? AccVoucherTypeId, int? SupplierId, int? CustomerId, int? EmployeeId, string AccIdNoteOneCT, string MinAccCode, string MaxAccCode)
        {

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");



            if (criteria == "fy")
            {
                if (FYId != null || FYId.Value > 0)
                {
                    Acc_FiscalYear fiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == FYId).FirstOrDefault();
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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + " ," + isLocalCurr + " ");

                }
                else
                {
                    filename = "ReceiptAndPay_Date_" + dtFrom + "_To_" + dtTo;// db.Acc_VoucherMains.Where(x => x.VoucherNo == VoucherFrom).Select(x => x.VoucherNo + "_" + x.Acc_VoucherType.VoucherTypeName).Single();
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptRecPay '" + comid + "','" + userid + "', '" + dtFrom + "','" + dtTo + "', " + AccIdRecPay + ", " + Currency + "," + PrdUnitId + "  ");

                }

            }
            if (rpttype == "CB")
            {

                reportname = "rptLedgerDetails_Multi_Summarized_CashBook";
                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr_CashBook '" + comid + "', " + AccIdRecPay + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + ",'" + userid + "'," + PrdUnitId + "   ");
                }
                else
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr_CashBook '" + comid + "', " + AccIdRecPay + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + " ,'" + userid + "'," + PrdUnitId + "   ");

                }

            }

            else if (rpttype.ToUpper().ToString() == "Tran".ToUpper())
            {

                reportname = "rptTransaction";

                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {
                    filename = "Transaction_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + " ," + AccVoucherTypeId + "," + PrdUnitId + "   ");
                }
                else
                {
                    filename = "Transaction_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + " ," + PrdUnitId + "  ");
                }
            }
            else if (rpttype.ToUpper().ToString() == "TranVoucherNo".ToUpper())
            {
                reportname = "rptTransaction_VoucherNo";

                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {
                    filename = "Transaction_VoucherNo_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction_VoucherNo '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + " ," + PrdUnitId + "  ");
                }
                else
                {
                    filename = "Transaction_VoucherNo_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTransaction_VoucherNo '" + comid + "', '" + userid + "', '" + dtFrom + "', '" + dtTo + "'," + Currency + "," + AccVoucherTypeId + " ," + PrdUnitId + "  ");
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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + SupplierId + "', '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "   ");


                }
                else
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + SupplierId + "',  '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "    ");


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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + SupplierId + "', '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "   ");


                }
                else
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiDrCr '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + SupplierId + "',  '" + CustomerId + "', '" + EmployeeId + "'," + PrdUnitId + "    ");


                }
            }
            else if (rpttype.ToUpper().ToString() == "ledgerA".ToUpper())
            {

                reportname = "rptLedgerDetails_All";

                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiAll '" + comid + "', " + AccIdLedger + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + "," + isLocalCurr + " ,'" + userid + "' , '" + MinAccCode + "',  '" + MaxAccCode + "', '" + EmployeeId + "'," + PrdUnitId + "   ");


                }
                else
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptLedgerMultiAll '" + comid + "', " + AccIdLedger + ", " + FYId.ToString() + ", '01-Jan-1900', '01-Jan-1900', " + Currency + ", " + isLocalCurr + ",'" + userid + "' ,'" + MinAccCode + "',  '" + MaxAccCode + "', '" + EmployeeId + "'," + PrdUnitId + "    ");


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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'," + PrdUnitId + "   ");


                }
                else
                {
                    filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'," + PrdUnitId + "    ");


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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'," + PrdUnitId + "   ");


                }
                else
                {
                    filename = "SupplierVat" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "' ," + PrdUnitId + "   ");


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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "',0, '" + dtFrom + "', '" + dtTo + "' , '" + userid + "'," + PrdUnitId + "   ");
                }
                else
                {
                    filename = "VatCertificate" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Bill_rptVatCertificate] '" + comid + "', '" + rpttype + "','" + SupplierId + "', " + FYId.ToString() + ",'" + dtFrom + "', '" + dtTo + "', '" + userid + "'," + PrdUnitId + "    ");
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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Supplier_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Supplier' , null,'" + AccIdLedger + "'," + PrdUnitId + " ");
                }
                else
                {
                    filename = "rptAdvanceMoney_Supplier_Schedule" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Supplier_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Supplier' ,'" + FYId.ToString() + "','" + AccIdLedger + "'," + PrdUnitId + "  ");
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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Employee_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Employee' , null ,'" + AccIdLedger + "'," + PrdUnitId + "  ");
                }
                else
                {
                    filename = "rptAdvanceMoney_Employee_Schedule" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Acc_rptAdvanceMoney_Employee_Schedule] '" + comid + "', '" + dtFrom + "' ,'" + dtTo + "' ,'Advance to Employee' ,'" + FYId.ToString() + "','" + AccIdLedger + "'," + PrdUnitId + "  ");
                }
            }

            else if (rpttype.ToUpper().ToString() == "GroupD".ToUpper())
            {

                reportname = "rptGroupDetails";


                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  Acc_rptGroupMultiDrCr '" + comid + "', " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "'," + PrdUnitId + "   ");

                }
                else
                {
                    filename = "LedgerDetails_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  Acc_rptGroupMultiDrCr '" + comid + "', " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "'," + PrdUnitId + "   ");

                }
            }

            else if (rpttype.ToUpper().ToString() == "GroupS".ToUpper())
            {

                reportname = "rptSubsidiaryLedger";
                var Note1 = AccIdNoteOneCT.Replace("&", "");

                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "SubsidiaryLedger_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  Acc_rptSubsidiaryLedger '" + comid + "', " + AccIdGroup + ", 0, '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "' ,'" + Note1 + "'," + PrdUnitId + "  ");

                }
                else
                {
                    filename = "SubsidiaryLedger_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  Acc_rptSubsidiaryLedger '" + comid + "', " + AccIdGroup + ", " + FYId.ToString() + ", '" + dtFrom + "', '" + dtTo + "', " + Currency + " ,'" + userid + "' ,'" + Note1 + "'," + PrdUnitId + "  ");

                }
            }


            else if (rpttype.ToUpper().ToString() == "BankRecon".ToUpper())
            {

                reportname = "rptBankReconciliation";


                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "BankReconciliation_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptBankReconciliation] '" + comid + "', " + AccIdLedger + ",'" + dtFrom + "', '" + dtTo + "' ,'" + userid + "'," + PrdUnitId + "   ");

                }
                else
                {
                    filename = "BankReconciliation_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptBankReconciliation] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "' ,'" + userid + "'," + PrdUnitId + "   ");

                }
            }
            else if (rpttype.ToUpper().ToString() == "CashFlow".ToUpper())
            {

                reportname = "rptCashFlowStatement";


                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptCashFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
                else
                {
                    filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptCashFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
            }
            else if (rpttype.ToUpper().ToString() == "FundFlow".ToUpper())
            {

                reportname = "rptFundFlowStatement";


                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "FundFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFundFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
                else
                {
                    filename = "CashFlowStatement_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFundFlowStatement] '" + comid + "', " + AccIdLedger + ", '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
            }
            else if (rpttype.ToUpper().ToString() == "FdrList".ToUpper())
            {

                reportname = "rptFDRList";


                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList] '" + comid + "', '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
                else
                {
                    filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList] '" + comid + "' , '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
            }
            else if (rpttype.ToUpper().ToString() == "FdrListV".ToUpper())
            {

                reportname = "rptFDRList_Voucher";


                if (criteria.ToUpper().ToString() == "Date".ToUpper())
                {

                    filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList_Voucher] '" + comid + "', '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
                else
                {
                    filename = "FDRList_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec  [Acc_rptFDRList_Voucher] '" + comid + "' , '" + dtFrom + "', '" + dtTo + "'," + PrdUnitId + " ");

                }
            }

            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");

            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";

            _httpContext.HttpContext.Session.SetObject("rptList", postData);
            // Session["rptList"] = postData;

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");// Session["reportquery"].ToString();
            clsReport.strDSNMain = DataSourceName;

            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return redirectUrl;
        }

        public ShowVoucherViewModel Model()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var defaultcountry = _context.Companys.Where(a => a.CompanyCode == comid).Select(a => a.CountryId).FirstOrDefault();
            ShowVoucherViewModel model = new ShowVoucherViewModel();
            List<Acc_FiscalYear> fiscalyear = _context.Acc_FiscalYears.Where(x => x.ComId == comid).ToList();
            int fiscalyid = fiscalyear.Max(p => p.FYId);

            List<Acc_FiscalMonth> fiscalmonth = _context.Acc_FiscalMonths.Where(x => x.FYId == fiscalyid).ToList();

            model.FiscalYs = fiscalyear;
            model.ProcessMonths = fiscalmonth;
            model.CountryId = defaultcountry;
            return model;
        }

        public IEnumerable<SelectListItem> PrdUnitId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
        }

        public IEnumerable<SelectListItem> SupplierList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Suppliers.Where(p => p.ComId == comid)
                .Select(s => new { Text = s.SupplierName + " - [ " + s.SupplierCode + " ]", Value = s.SupplierId }).ToList(), "Value", "Text");
        }

        public Acc_VoucherMain VoucherMain(int VoucherId)
        {
            Acc_VoucherMain Vouchermain = _context.Acc_VoucherMains
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
            return Vouchermain;
        }

        public IEnumerable<SelectListItem> VoucherTypeList()
        {
            return new SelectList(_context.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName").ToList();
        }
    }
}
