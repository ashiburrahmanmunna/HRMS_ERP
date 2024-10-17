using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GTERP.Repository.Commercial
{
    public class BuyerInformationsRepository : BaseRepository<BuyerInformation>, IBuyerInformationsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<BuyerInformationsRepository> _logger;

        public BuyerInformationsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<BuyerInformationsRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;

        }

        public IEnumerable<SelectListItem> BuyerGroupId()
        {
            return new SelectList(_context.BuyerGroups, "BuyerGroupId", "BuyerGroupName");
        }

        public IEnumerable<SelectListItem> CountryId()
        {
            return new SelectList(_context.Countries, "CountryId", "CountryName");
        }

        public IEnumerable<SelectListItem> EmployeeIdExport()
        {
            return new SelectList(_context.Employee, "EmployeeId", "EmployeeName");
        }

        public IEnumerable<SelectListItem> EmployeeIdImport()
        {
            return new SelectList(_context.Employee, "EmployeeId", "EmployeeName");
        }
    }
}
