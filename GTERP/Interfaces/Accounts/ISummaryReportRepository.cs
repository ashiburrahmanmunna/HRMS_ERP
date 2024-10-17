using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface ISummaryReportRepository
    {
        List<TrialBalanceModel> TrialBalance();
        ShowVoucherViewModel Model();
        IEnumerable<SelectListItem> CountryId();
        IEnumerable<SelectListItem> AccIdGroup();
        IEnumerable<SelectListItem> PrdUnitId();
        List<Acc_FiscalMonth> FiscalMonth(int? id);

        List<Acc_FiscalYear> FiscalYear();
        List<Acc_FiscalMonth> Acc_FiscalMonth(int? id);
        List<Acc_FiscalHalfYear> Acc_FiscalHalfYear(int? id);
        List<Acc_FiscalHalfYear> FiscalHalfYear(int? id);
        List<Acc_FiscalQtr> Acc_FiscalQtr(int? id);
        List<Acc_FiscalQtr> FiscalQuarter(int? id);
        void SetSession(string criteria, string rptFormat, string rpttype, int? Currency, int? isCompare, int? isCumulative, int? isShowZero, int? isGroup, int? FYId, int? FYHId, int? FYQId, int? FYMId, string FromDate, string ToDate, int? AccIdGroup, int? PrdUnitId);
    }
}
