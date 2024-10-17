using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IMeetingRepository : IBaseRepository<Cat_Meeting>
    {
        IEnumerable<SelectListItem> GetMeetingList();
    }
}
