using GTERP.Interfaces;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GTERP.Repository.Commercial
{
    public class LienBanksRepository : BaseRepository<LienBank>, ILienBanksRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<LienBanksRepository> _logger;

        public LienBanksRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<LienBanksRepository> logger
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

        public IEnumerable<SelectListItem> CountryId1(LienBank LienBank)
        {
            return new SelectList(_context.Countries, "CountryId", "CountryName", LienBank.CountryId);
        }
    }
}
