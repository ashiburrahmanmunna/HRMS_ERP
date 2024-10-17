using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ICat_UnitRepository : IBaseRepository<Cat_Unit>
    {
        IEnumerable<SelectListItem> GetCat_UnitList();
        List<Cat_Unit> GetAllUnitbyCompany(string comid);
    }
}
