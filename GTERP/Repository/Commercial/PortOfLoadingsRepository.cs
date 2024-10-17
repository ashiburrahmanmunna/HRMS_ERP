using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GTERP.Repository.Commercial
{
    public class PortOfLoadingsRepository : BaseRepository<PortOfLoading>, IPortOfLoadingsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<PortOfLoadingsRepository> _logger;

        public PortOfLoadingsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<PortOfLoadingsRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }
        public IEnumerable<SelectListItem> CountryId()
        {
            return new SelectList(_context.Countries, "CountryId", "CountryName");
        }

        public IEnumerable<SelectListItem> CountryId1(PortOfLoading PortOfLoading)
        {
            return new SelectList(_context.Countries, "CountryId", "CountryName", PortOfLoading.CountryId);
        }
    }
}
