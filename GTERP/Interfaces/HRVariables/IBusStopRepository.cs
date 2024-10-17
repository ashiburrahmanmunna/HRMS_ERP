using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IBusStopRepository : IBaseRepository<Cat_BusStop>
    {
        IEnumerable<SelectListItem> GetBusStopList();
    }
}
