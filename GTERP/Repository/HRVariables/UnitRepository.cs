using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class UnitRepository : SelfRepository<Unit>, IUnitRepository
    {

        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public UnitRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetUnitList()
        {
            return All().Select(x => new SelectListItem
            {
                Value = x.UnitId.ToString(),
                Text = x.UnitName
            });
        }
    }
}
