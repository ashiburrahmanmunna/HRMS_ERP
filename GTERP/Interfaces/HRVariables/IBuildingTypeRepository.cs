using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IBuildingTypeRepository : IBaseRepository<Cat_BuildingType>
    {
        IEnumerable<SelectListItem> GetBuildingType();
    }
}
