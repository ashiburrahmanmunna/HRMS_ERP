using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IPoliceStationRepository : IBaseRepository<Cat_PoliceStation>
    {
        IEnumerable<SelectListItem> GetPoliceStationList();
        public IEnumerable<Cat_PoliceStation> GetPSList();
    }
}
