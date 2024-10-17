using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GTERP.Repository.ControllerFolder
{
    public class ModulesRepository : SelfRepository<Module>, IModulesRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<ModulesRepository> _logger;


        public ModulesRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<ModulesRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }
    }
}
