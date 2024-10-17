using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IModuleGroupsRepository : ISelfRepository<ModuleGroup>
    {
        IEnumerable<SelectListItem> ModuleId();
        IEnumerable<SelectListItem> ModuleId1(ModuleGroup moduleGroup);
    }
}
