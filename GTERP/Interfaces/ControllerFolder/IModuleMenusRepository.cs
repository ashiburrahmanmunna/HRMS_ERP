using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IModuleMenusRepository : ISelfRepository<ModuleMenu>
    {
        IEnumerable<SelectListItem> ModuleId();
        IEnumerable<SelectListItem> ModuleId1(ModuleMenu moduleMenu);
        IEnumerable<SelectListItem> ModuleGroupId();
        IEnumerable<SelectListItem> ModuleGroupId1(ModuleMenu moduleMenu);
        IEnumerable<SelectListItem> ImageCriteriaId();
        IEnumerable<SelectListItem> ImageCriteriaId1(ModuleMenu moduleMenu);
        IEnumerable<SelectListItem> ModuleMenuId(ModuleMenu moduleMenu);
        IEnumerable<SelectListItem> ParentId(ModuleMenu moduleMenu);
    }
}
