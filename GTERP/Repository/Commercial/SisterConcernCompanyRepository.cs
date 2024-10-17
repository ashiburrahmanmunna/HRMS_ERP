using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GTERP.Repository.Commercial
{
    public class SisterConcernCompanyRepository : BaseRepository<SisterConcernCompany>, ISisterConcernCompanyRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<SisterConcernCompanyRepository> _logger;

        public SisterConcernCompanyRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<SisterConcernCompanyRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }
    }
}
