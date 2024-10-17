using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ISkillRepository : IBaseRepository<Cat_Skill>
    {
        IEnumerable<SelectListItem> GetSkillList();
        List<Cat_Skill> SkillAll();
    }
}
