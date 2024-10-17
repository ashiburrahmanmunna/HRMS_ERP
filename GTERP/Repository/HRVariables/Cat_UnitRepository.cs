using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class Cat_UnitRepository : BaseRepository<Cat_Unit>, ICat_UnitRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public Cat_UnitRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public List<Cat_Unit> GetAllUnitbyCompany(string comid)
        {
            var unitList = _context.Cat_Unit.Where(a => a.ComId == comid).ToList();
            return unitList;
        }
        public IEnumerable<SelectListItem> GetCat_UnitList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.UnitId.ToString(),
                Text = x.UnitName
            });
        }
    }
}
