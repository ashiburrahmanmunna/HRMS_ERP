using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.Accounts
{
    public interface IBudgetReleaseRepository : IBaseRepository<Acc_BudgetRelease>
    {
        IEnumerable<SelectListItem> FiscalYearId();
        IEnumerable<SelectListItem> FiscalMonthId();
        IQueryable<Acc_ChartOfAccount> FilterAccount();
        IEnumerable<SelectListItem> AccId();
        List<Acc_BudgetRelease> GetBudgetRelease();
        Acc_FiscalYear FiscalYear();
        Acc_FiscalMonth FiscalMonth();
        List<Acc_FiscalMonth> GetMonthData(int FiscalYearId);
        BudgetMain BudgetMain(int FiscalYearId);
        BudgetDetails BudgetDetails(int AccId, int FiscalYearId);
        double BudgetRelease(int AccId, int FiscalYearId, int BudgetReleaseId);
        string SetSessionAccountReport(string rptFormat, string FiscalYearId, string AccId);
    }
}
