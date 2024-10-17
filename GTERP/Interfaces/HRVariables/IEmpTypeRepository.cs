
using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IEmpTypeRepository : ISelfRepository<Cat_Emp_Type>
    {
        IEnumerable<SelectListItem> GetEmpTypeList();
    }
}
