using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HRVariables
{
    public interface IBloodGroupRepository : IBaseRepository<Cat_BloodGroup>
    {
        IEnumerable<SelectListItem> BloodGroupSelectList();
        IQueryable<Cat_BloodGroup> GetBloodGroup();
    }
}
