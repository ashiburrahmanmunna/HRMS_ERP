using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IOTAndNightShiftRepository : IBaseRepository<HR_OT_FC>
    {
        IEnumerable<SelectListItem> ProssType();
        void ExistingData(List<HR_OT_FC> hR_OT_FCs);
        List<OTFC> SearchOTAndNight(string prossType);
    }
}
