using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ISignatoryRepository : IBaseRepository<Cat_ReportSignatory>
    {
        IEnumerable<SelectListItem> ReportNames();
        IEnumerable<SelectListItem> ModuleName();
    }
}
