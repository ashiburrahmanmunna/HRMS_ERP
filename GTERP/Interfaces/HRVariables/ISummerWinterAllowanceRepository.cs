using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ISummerWinterAllowanceRepository : IBaseRepository<Cat_SummerWinterAllowanceSetting>
    {
        IEnumerable<SelectListItem> GetSummerWinterList();
    }
}
