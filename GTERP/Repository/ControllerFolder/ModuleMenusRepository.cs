using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class ModuleMenusRepository : SelfRepository<ModuleMenu>, IModuleMenusRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<ModuleMenusRepository> _logger;


        public ModuleMenusRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<ModuleMenusRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> ImageCriteriaId()
        {
            return new SelectList(_context.ImageCriterias, "ImageCriteriaId", "ImageCriteriaCaption");
        }

        public IEnumerable<SelectListItem> ImageCriteriaId1(ModuleMenu moduleMenu)
        {
            return new SelectList(_context.ImageCriterias, "ImageCriteriaId", "ImageCriteriaCaption", moduleMenu.ImageCriteriaId);
        }

        public IEnumerable<SelectListItem> ModuleGroupId()
        {
            return new SelectList(_context.ModuleGroups, "ModuleGroupId", "ModuleGroupCaption");
        }

        public IEnumerable<SelectListItem> ModuleGroupId1(ModuleMenu moduleMenu)
        {
            return new SelectList(_context.ModuleGroups.Where(x => x.ModuleGroupId == moduleMenu.ModuleGroupId), "ModuleGroupId", "ModuleGroupCaption", moduleMenu.ModuleGroupId);
        }

        public IEnumerable<SelectListItem> ModuleId()
        {
            return new SelectList(_context.Modules, "ModuleId", "ModuleCaption");
        }

        public IEnumerable<SelectListItem> ModuleId1(ModuleMenu moduleMenu)
        {
            return new SelectList(_context.Modules, "ModuleId", "ModuleCaption", moduleMenu.ModuleId);
        }

        public IEnumerable<SelectListItem> ModuleMenuId(ModuleMenu moduleMenu)
        {
            return new SelectList(_context.ModuleGroups, "ModuleMenuId", "ModuleGroupCaption", moduleMenu.ModuleGroupId);
        }

        public IEnumerable<SelectListItem> ParentId(ModuleMenu moduleMenu)
        {
            return new SelectList(_context.ModuleMenus.Where(x => x.ModuleGroupId == moduleMenu.ModuleGroupId && x.isParent == 1), "ModuleMenuId", "ModuleMenuCaption", moduleMenu.ParentId);
        }
    }
}
