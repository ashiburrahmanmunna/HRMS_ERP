using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GTERP.Repository.ControllerFolder
{
    public class ModuleGroupsRepository : SelfRepository<ModuleGroup>, IModuleGroupsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<ModuleGroupsRepository> _logger;


        public ModuleGroupsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<ModuleGroupsRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> ModuleId()
        {
            return new SelectList(_context.Modules, "ModuleId", "ModuleName");
        }

        public IEnumerable<SelectListItem> ModuleId1(ModuleGroup moduleGroup)
        {
            return new SelectList(_context.Modules, "ModuleId", "ModuleName", moduleGroup.ModuleId);
        }
    }
}
