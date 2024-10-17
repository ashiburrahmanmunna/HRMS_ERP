using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IGradeRepository : IBaseRepository<Cat_Grade>
    {
        IEnumerable<SelectListItem> GradeSelectList();
        List<Cat_Grade> GradeAll();
    }
}
