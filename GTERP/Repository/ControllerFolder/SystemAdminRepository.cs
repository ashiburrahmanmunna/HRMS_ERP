using GTERP.BLL;
using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GTERP.Repository.ControllerFolder
{
    public class SystemAdminRepository : ISystemAdminRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<SystemAdminRepository> _logger;
        PermissionLevel _PL;


        public SystemAdminRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<SystemAdminRepository> logger,
            PermissionLevel pl
            )
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _PL = pl;
        }
    }
}
