using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ILocationRepository : IBaseRepository<Cat_Location>
    {
        IEnumerable<SelectListItem> GetLocationList();
    }
}
