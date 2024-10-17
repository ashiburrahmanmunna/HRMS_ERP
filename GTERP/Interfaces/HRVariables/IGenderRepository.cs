using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IGenderRepository
    {
        IEnumerable<SelectListItem> GenderList();
    }
}
