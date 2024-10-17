using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IReligionRepository : IBaseRepository<Cat_Religion>
    {
        IEnumerable<SelectListItem> ReligionSelectList();
        IEnumerable<Cat_Religion> GetReligionList();
    }
}
