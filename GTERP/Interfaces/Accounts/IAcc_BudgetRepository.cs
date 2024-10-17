using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Accounts
{
    public interface IAcc_BudgetRepository:IBaseRepository<Acc_BudgetMain>
    {
        IEnumerable<SelectListItem> PrdUnitId();
        IEnumerable<SelectListItem> AccountParent();
        IEnumerable<SelectListItem> Account();
        IEnumerable<SelectListItem> Account1();
        IEnumerable<SelectListItem> Country1();
        IEnumerable<SelectListItem> AccountMain1();
        IEnumerable<SelectListItem> FiscalYearId1();
        IEnumerable<SelectListItem> FiscalMonthId1();
        IEnumerable<SelectListItem> Country(int? FiscalYearId, int? FiscalMonthId);
        IEnumerable<SelectListItem> AccountMain(int? FiscalYearId, int? FiscalMonthId);
        IEnumerable<SelectListItem> FiscalYearId(int? FiscalYearId, int? FiscalMonthId);
        IEnumerable<SelectListItem> FiscalMonthId(int? FiscalYearId, int? FiscalMonthId);
    }
}
