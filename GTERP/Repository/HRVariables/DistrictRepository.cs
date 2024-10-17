using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class DistrictRepository : BaseRepository<Cat_District>, IDistrictRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public DistrictRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IQueryable<Cat_District> GetAllDistrict()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");

            return _context.Cat_District.Where(x => !x.IsDelete);
        }

        public IEnumerable<SelectListItem> GetDistrictList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return GetAllDistrict().Select(x => new SelectListItem
            {
                Value = x.DistId.ToString(),
                Text = x.DistName
            });
        }
    }
}
