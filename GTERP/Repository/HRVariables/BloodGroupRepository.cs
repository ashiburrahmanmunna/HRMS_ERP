using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class BloodGroupRepository : BaseRepository<Cat_BloodGroup>, IBloodGroupRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public BloodGroupRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> BloodGroupSelectList()
        {
            return _context.Cat_BloodGroup.Where(x=>!x.IsDelete).Select(x => new SelectListItem
            {
                Value = x.BloodId.ToString(),
                Text = x.BloodName,
                
            });
        }

        public IQueryable<Cat_BloodGroup> GetBloodGroup()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_BloodGroup.Where(x => !x.IsDelete);
        }
    }
}
