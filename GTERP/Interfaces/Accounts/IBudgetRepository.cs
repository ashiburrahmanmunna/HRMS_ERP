using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.Accounts
{
    public interface IBudgetRepository
    {
        IQueryable<BudgetMainsList> Get();
        void Delete(int id);
        IEnumerable<SelectListItem> FiscalYearId(int? id);
        IEnumerable<SelectListItem> FiscalMonthId(int? id);
        void Edit(int id, BudgetDetails BudgetDetails);
        IEnumerable<SelectListItem> ParentId();
        void Update(BudgetDetails model);
        string Print(int? id, string type);
        List<Acc_BudgetData> BudgetDataLoadByParameter(int? Yearid, int? Monthid, int? ParentId);
        List<BudgetMain> GetAll();
    }
}
