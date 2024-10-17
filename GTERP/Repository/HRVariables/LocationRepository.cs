using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class LocationRepository : BaseRepository<Cat_Location>, ILocationRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public LocationRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetLocationList()
        {
            return _context.Cat_Location.Select(x => new SelectListItem
            {
                Value = x.LId.ToString(),
                Text = x.LocationName
            });
        }
    }
}
