using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class PoliceStationRepository : BaseRepository<Cat_PoliceStation>, IPoliceStationRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public PoliceStationRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetPoliceStationList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.PStationId.ToString(),
                Text = x.PStationName
            });
        }

        public IEnumerable<Cat_PoliceStation> GetPSList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_PoliceStation
                .Include(x => x.Cat_District)
                .Where(x => !x.IsDelete);
        }
    }
}
