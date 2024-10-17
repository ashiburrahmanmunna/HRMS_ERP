using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GTERP.Repository.ControllerFolder
{
    public class UnitMastersRepository : SelfRepository<UnitMaster>, IUnitMastersRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<UnitMastersRepository> _logger;


        public UnitMastersRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<UnitMastersRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> UnitGroupId()
        {
            return new SelectList(_context.UnitGroups, "UnitGroupId", "UnitGroupId");
        }

        public IEnumerable<SelectListItem> UnitGroupId1(UnitMaster unitMaster)
        {
            return new SelectList(_context.UnitGroups, "UnitGroupId", "UnitGroupId", unitMaster.UnitGroupId);
        }
    }
}
