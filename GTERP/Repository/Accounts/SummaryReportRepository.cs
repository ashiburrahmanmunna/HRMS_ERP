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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class SummaryReportRepository : ISummaryReportRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<SummaryReportRepository> _logger;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        POSRepository POS;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        public SummaryReportRepository(IHttpContextAccessor httpContext,
            ILogger<SummaryReportRepository> logger,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            POSRepository pos

            )
        {
            _httpContext = httpContext;
            _context = context;
            _logger = logger;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            POS = pos;
        }

        public IEnumerable<SelectListItem> AccIdGroup()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(POS.GetChartOfAccountGroup(comid), "AccId", "AccName");
        }

        public List<Acc_FiscalHalfYear> Acc_FiscalHalfYear(int? id)
        {
            var Acc_FiscalHalfYear = FiscalHalfYear(id);
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
            return datahalfyear;
        }

        public List<Acc_FiscalMonth> Acc_FiscalMonth(int? id)
        {
            var Acc_FiscalMonth = FiscalMonth(id);
            List<Acc_FiscalMonth> datamonth = new List<Acc_FiscalMonth>();
            foreach (Acc_FiscalMonth item in Acc_FiscalMonth)
            {
                Acc_FiscalMonth asdf = new Acc_FiscalMonth
                {
                    MonthId = item.MonthId,
                    MonthName = item.MonthName,
                    dtFrom = DateTime.Parse(item.dtFrom).ToString("dd-MMM-yy"),
                    dtTo = DateTime.Parse(item.dtTo).ToString("dd-MMM-yy")
                    //LCDate = DateTime.Parse(item.FirstShipDate.ToString()).ToString("ddd-MMM-yy")
                };
                datamonth.Add(asdf);
            }
            return datamonth;
        }

        public List<Acc_FiscalQtr> Acc_FiscalQtr(int? id)
        {
            var FiscalQuarter = _context.Acc_FiscalQtrs.Where(x => x.FYId == id).ToList();
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
            return dataquarter;
        }

        public IEnumerable<SelectListItem> CountryId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            int defaultcountry = _context.Companys.Where(a => a.CompanyCode == comid).Select(a => a.CountryId).FirstOrDefault();
            return new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", defaultcountry);
        }

        public List<Acc_FiscalHalfYear> FiscalHalfYear(int? id)
        {
            return _context.Acc_FiscalHalfYears.Where(x => x.FYId == id).ToList();
        }

        public List<Acc_FiscalMonth> FiscalMonth(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_FiscalMonths.Where(x => x.FYId == id).ToList();
        }

        public List<Acc_FiscalQtr> FiscalQuarter(int? id)
        {
            return _context.Acc_FiscalQtrs.Where(x => x.FYId == id).ToList();
        }

        public List<Acc_FiscalYear> FiscalYear()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_FiscalYears.Where(x => x.ComId == comid).ToList();
        }

        public ShowVoucherViewModel Model()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            int defaultcountry = _context.Companys.Where(a => a.CompanyCode == comid).Select(a => a.CountryId).FirstOrDefault();
            ShowVoucherViewModel model = new ShowVoucherViewModel();
            List<Acc_FiscalYear> fiscalyear = _context.Acc_FiscalYears.Where(x => x.ComId == comid).ToList();
            int fiscalyid = fiscalyear.Max(p => p.FYId);


            List<Acc_FiscalMonth> fiscalmonth = _context.Acc_FiscalMonths.Where(x => x.FYId == fiscalyid).ToList();
            List<Acc_FiscalQtr> fiscalquarter = _context.Acc_FiscalQtrs.Where(x => x.FYId == fiscalyid).ToList();
            List<Acc_FiscalHalfYear> fiscalhalfyear = _context.Acc_FiscalHalfYears.Where(x => x.FYId == fiscalyid).ToList();

            model.FiscalYs = fiscalyear;
            model.ProcessMonths = fiscalmonth;

            model.ProcessQtr = fiscalquarter;
            model.ProcessHalfYear = fiscalhalfyear;


            model.CountryId = defaultcountry;
            return model;
        }

        public IEnumerable<SelectListItem> PrdUnitId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PrdUnits.Where(c => c.ComId == comid), "PrdUnitId", "PrdUnitName"); //&& c.ComId == (transactioncomid)
        }

        public void SetSession(string criteria, string rptFormat, string rpttype, int? Currency, int? isCompare, int? isCumulative, int? isShowZero, int? isGroup, int? FYId, int? FYHId, int? FYQId, int? FYMId, string FromDate, string ToDate, int? AccIdGroup, int? PrdUnitId)
        {
            string dtFrom = DateTime.Now.ToString();
            string dtTo = DateTime.Now.ToString();

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");
            int strId = 0;

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

            if (criteria == "Year")
            {
                if (FYId != null || FYId.Value > 0)
                {
                    Acc_FiscalYear fiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == FYId).FirstOrDefault();
                    strId = fiscalyear.FYId;
                    dtFrom = fiscalyear.OpDate;
                    dtTo = fiscalyear.ClDate;
                }
            }
            if (criteria.ToUpper().ToString() == "HYear".ToUpper())
            {
                if (FYHId != null || FYHId.Value > 0)
                {
                    Acc_FiscalHalfYear fiscalhalfyear = _context.Acc_FiscalHalfYears.Where(x => x.FiscalHalfYearId == FYHId).FirstOrDefault();
                    strId = fiscalhalfyear.FiscalHalfYearId;
                    dtFrom = fiscalhalfyear.dtFrom;
                    dtTo = fiscalhalfyear.dtTo;
                }
            }
            if (criteria.ToUpper().ToString() == "Quarter".ToUpper())
            {
                if (FYHId != null || FYHId.Value > 0)
                {
                    Acc_FiscalQtr fiscalqtr = _context.Acc_FiscalQtrs.Where(x => x.FiscalQtrId == FYQId).FirstOrDefault();
                    strId = fiscalqtr.FiscalQtrId;
                    dtFrom = fiscalqtr.dtFrom;
                    dtTo = fiscalqtr.dtTo;
                }
            }
            if (criteria.ToUpper().ToString() == "Month".ToUpper())
            {
                if (FYMId != null || FYMId.Value > 0)
                {
                    Acc_FiscalMonth fiscalmonth = _context.Acc_FiscalMonths.Where(x => x.MonthId == FYMId).FirstOrDefault();
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
                        _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTrailBalanceDate  '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + " ,'" + dtFrom + "' ,'" + dtTo + "' ,'" + AccIdGroup + "'  ");

                    }
                    else
                    {
                        reportname = "rptTrialBalanceGroup";

                        filename = "TrialBalanceGroup_Date_" + dtFrom + "_To_" + dtTo;
                        _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTrailBalanceGroupDate  '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + " ,'" + dtFrom + "' ,'" + dtTo + "' ,'" + AccIdGroup + "'  ");

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
                            _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTrailBalance '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + " ," + Currency + ",'" + AccIdGroup + "' ");

                        }
                        else
                        {
                            reportname = "rptTrialBalanceComp";
                            filename = "TrialBalanceCompare_Date_" + dtFrom + "_To_" + dtTo;

                            _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCompareTrialBalance '" + criteria + "', '" + comid + "', '" + strId + "'," + isShowZero + " ," + Currency + " ,'" + AccIdGroup + "' ");

                        }
                    }
                    else
                    {
                        if (isCompare == 0)
                        {
                            reportname = "rptTrialBalanceGroup";

                            filename = "TrialBalanceWithGroup_Date_" + dtFrom + "_To_" + dtTo;
                            _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptTrailBalanceGroup '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + "," + Currency + " ,'" + AccIdGroup + "'");

                        }
                        else ///no report still build
                        {
                            reportname = "rptTrialBalanceCompGroup";
                            filename = "TrialBalanceCompareGroup_Date_" + dtFrom + "_To_" + dtTo;
                            _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCompareTrialBalanceGroup '" + criteria + "', '" + comid + "', '" + strId + "', " + isShowZero + "," + Currency + " ,'" + AccIdGroup + "' ");
                        }
                    }

                }
            }

            else if (rpttype.ToUpper().ToString() == "MC".ToUpper())
            {
                reportname = "rptMaterialConsumed";
                filename = "MaterialConsumed_Date_" + dtFrom + "_To_" + dtTo;
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptMaterialConsumed '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "'," + Currency + " ");

            }

            else if (rpttype.ToUpper().ToString() == "COGS".ToUpper())
            {
                if (isCompare == 1)
                {
                    reportname = "rptCOGSCompare";
                    filename = "CostOfGoodsSoldCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCOGSCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "'");

                }
                else if (isCumulative == 1)
                {
                    reportname = "rptCOGSCompare";
                    filename = "CostOfGoodsSold_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCOGSCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");
                }
                else
                {
                    reportname = "rptCOGS";
                    filename = "CostOfGoodsSold_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCOGS '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");

                }
            }

            else if (rpttype.ToUpper().ToString() == "COB".ToUpper())
            {
                if (isCompare == 1)
                {
                    reportname = "rptCostBreakupCompare";
                    filename = "rptCostBreakupCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCostBreakupCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "'");

                }
                else if (isCumulative == 1)
                {
                    reportname = "rptCostBreakupCompare";
                    filename = "rptCostBreakupCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCostBreakupCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "'");
                }
                else
                {
                    reportname = "rptCostBreakup";
                    filename = "rptCostBreakup_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCostBreakup '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");

                }
            }

            else if (rpttype.ToUpper().ToString() == "IS".ToUpper())
            {
                if (isCompare == 1)
                {

                    reportname = "rptIncomeCompare";
                    filename = "IncomeStatementCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptIncomeCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ");

                }
                else if (isCumulative == 1)
                {

                    reportname = "rptIncomeCompare";
                    filename = "IncomeStatementCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptIncomeCumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',0 ");

                }
                else
                {
                    //reportname = "rptIncome";
                    //filename = "IncomeStatement_Date_" + dtFrom + "_To_" + dtTo;
                    //HttpContext.Session.SetString("reportquery", "Exec Acc_rptIncome '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");

                    reportname = "rptIncome";
                    filename = "IncomeStatement_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec rptIncome_Trinity '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                }
            }

            /////complete


            if (rpttype.ToUpper().ToString() == "OE".ToUpper())
            {
                if (isCompare == 0)
                {
                    reportname = "rptOwnersEquityCarry";
                    filename = "OwnersEquity_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptOwnersEquityCarry '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");

                }
                else
                {
                    reportname = "rptOwnersEquityCarryComp";
                    filename = "OwnersEquityCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptOwnersEquityCarry '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");

                }
            }



            else if (rpttype.ToUpper().ToString() == "BS".ToUpper())
            {
                if (isCompare == 0)
                {
                    reportname = "rptBalanceSheet";
                    filename = "BalanceSheet_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptBalanceSheet '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "',1 ");
                }

                else
                {

                    reportname = "rptBalanceSheetCompFinal";
                    filename = "BalanceSheetCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptBalanceSheetCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "',1");

                }
            }


            else if (rpttype.ToUpper().ToString() == "FA".ToUpper())
            {
                if (isCompare == 0)
                {


                    reportname = "rptAssets";
                    filename = "Asset_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptAssets '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                }
            }

            else if (rpttype.ToUpper().ToString() == "CF".ToUpper())
            {
                if (isCompare == 0)
                {


                    reportname = "rptCashFlow";
                    filename = "CashFlow_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptCashFlow '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                }
            }

            else if (rpttype.ToUpper().ToString() == "FF".ToUpper())
            {
                if (isCompare == 0)
                {


                    reportname = "rptFundFlow";
                    filename = "CashFlow_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptFundFlow '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                }
            }

            else if (rpttype.ToUpper().ToString() == "MR".ToUpper())
            {
                if (isCompare == 0)
                {


                    reportname = "rptManagementRatio";
                    filename = "Management_Ration_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptManagementRatio '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "','" + isShowZero + "','" + Currency + "' ");


                }
            }

            else if (rpttype.ToUpper().ToString() == "NR".ToUpper())
            {
                if (isCompare == 1)
                {
                    reportname = "rptNotesComp";
                    filename = "NotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotesCompare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                }
                else if (isCumulative == 1)
                {
                    reportname = "rptNotesComp";
                    filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotesCumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
                else
                {
                    reportname = "rptNotes";
                    filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_Balancesheet_Cumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


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
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_Balancesheet '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
            }
            else if (rpttype.ToUpper().ToString() == "NISR".ToUpper())
            {


                if (isCompare == 1)
                {
                    reportname = "rptNotesIncomeStatementCompare";
                    filename = "NotesIncomeStatementReport_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_IncomeStatement_Cumulative '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                }
                if (isCumulative == 1)
                {
                    reportname = "rptNotesIncomeStatementCompare";
                    filename = "NotesIncomeStatementReport_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_IncomeStatement_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
                else
                {
                    reportname = "rptNotesIncomeStatement";
                    filename = "NotesIncomeStatementReport_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_IncomeStatement '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
            }

            else if (rpttype.ToUpper().ToString() == "NBCIC".ToUpper())
            {
                if (isCompare == 1)
                {
                    reportname = "rptNotesBCIC";
                    filename = "NotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_BCIC_Compare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                }
                else if (isCumulative == 1)
                {
                    reportname = "rptNotesBCIC";
                    filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_BCIC_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
                else
                {
                    reportname = "rptNotesBCIC";
                    filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_BCIC '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
            }

            else if (rpttype.ToUpper().ToString() == "NFA".ToUpper())
            {
                if (isCompare == 1)
                {
                    reportname = "rptNotes_FixedAsset";
                    filename = "NotesReprotCompare_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_FixedAsset_Compare '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', '" + isShowZero + "','" + Currency + "',0");


                }
                else if (isCumulative == 1)
                {
                    reportname = "rptNotes_FixedAsset";
                    filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_FixedAsset_Cumulative '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
                else
                {
                    reportname = "rptNotes_FixedAsset";
                    filename = "NotesReprot_Date_" + dtFrom + "_To_" + dtTo;
                    _httpContext.HttpContext.Session.SetString("reportquery", "Exec Acc_rptNotes_FixedAsset '" + userid + "','" + comid + "','" + criteria + "','" + strId + "', '" + isShowZero + "' ,'" + Currency + "',1 ");

                }
            }


            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");

            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";

            _httpContext.HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

        }

        public List<TrialBalanceModel> TrialBalance()
        {
            var result = "";
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

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


            var quary = $"Exec Acc_rptTrailBalanceGroup '" + userid + "','" + comid + "','" + criteria + "', '" + strId + "', " + isShowZero + "," + Currency + " ,'" + AccIdGroup + "'";

            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@UserId", userid);
            parameters[1] = new SqlParameter("@ComId", comid);
            parameters[2] = new SqlParameter("@Flag", "Month");
            parameters[3] = new SqlParameter("@Id", 14);
            parameters[4] = new SqlParameter("@IsShowZero", 0);
            parameters[5] = new SqlParameter("@Currency", 18);
            parameters[6] = new SqlParameter("@accid", 0);

            List<TrialBalanceModel> TrialBalanceReport = Helper.ExecProcMapTList<TrialBalanceModel>("Acc_rptTrailBalanceGroup", parameters);
            return TrialBalanceReport;
        }
    }
}
