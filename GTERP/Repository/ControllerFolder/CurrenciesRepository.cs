using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GTERP.Repository.ControllerFolder
{
    public class CurrenciesRepository : SelfRepository<Currency>, ICurrenciesRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<CurrenciesRepository> _logger;

        public CurrenciesRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<CurrenciesRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }
    }
}
