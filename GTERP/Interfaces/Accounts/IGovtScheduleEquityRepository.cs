using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IGovtScheduleEquityRepository : IBaseRepository<Acc_GovtSchedule_Equity>
    {
        IEnumerable<SelectListItem> AccId();
        List<Acc_GovtSchedule_Equity> GetAccGovtScheduleList();
        IEnumerable<SelectListItem> CriteriaList();
    }
}
