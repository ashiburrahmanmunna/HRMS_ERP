using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Budget
{
    public interface IBudgetProductionTargetRepository:IBaseRepository<Budget_ProductionTarget>
    {
        List<Budget_ProductionTarget> GetBudget_ProductionTargetList(string FromDate, string ToDate, string criteria);
        IEnumerable<SelectListItem> AccId();
        IEnumerable<SelectListItem> Criteria();
    }
}
