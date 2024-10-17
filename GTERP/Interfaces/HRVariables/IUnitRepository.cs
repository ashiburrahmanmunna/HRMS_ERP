using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IUnitRepository : ISelfRepository<Unit>
    {
        IEnumerable<SelectListItem> GetUnitList();
    }
}
