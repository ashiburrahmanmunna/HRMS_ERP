using GTERP.Interfaces.Base;
using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IStrengthRepository : IBaseRepository<Cat_Strength>
    {
        IEnumerable<SelectListItem> GetStrengthList();


    }
}
