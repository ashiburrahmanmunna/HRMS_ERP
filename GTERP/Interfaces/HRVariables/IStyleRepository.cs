using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IStyleRepository : IBaseRepository<Cat_Style>
    {
        IEnumerable<SelectListItem> GetStyleList();
        IEnumerable<SelectListItem> GetColor();
        IEnumerable<SelectListItem> GetSize();
    }
}
