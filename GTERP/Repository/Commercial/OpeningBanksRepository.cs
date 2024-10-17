using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GTERP.Repository.Commercial
{
    public class OpeningBanksRepository : BaseRepository<OpeningBank>, IOpeningBanksRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<OpeningBanksRepository> _logger;

        public OpeningBanksRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<OpeningBanksRepository> logger
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
        public IEnumerable<SelectListItem> CountryId1(OpeningBank openingBank)
        {
            return new SelectList(_context.Countries, "CountryId", "CountryName", openingBank.CountryId);
        }
    }
}
