using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ICatVariableRepository : IBaseRepository<Cat_Variable>
    {
        IEnumerable<SelectListItem> GetCatVariableList();
        List<Cat_Variable> GetVariableList();
    }
}
