using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class BusStopRepository : BaseRepository<Cat_BusStop>, IBusStopRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public BusStopRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetBusStopList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.BusStopId.ToString(),
                Text = x.BusStopName
            });
        }
    }
}
