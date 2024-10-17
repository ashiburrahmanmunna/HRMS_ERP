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
    public class SubSectionRepository : BaseRepository<Cat_SubSection>, ISubSectionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public SubSectionRepository(GTRDBContext context, IHttpContextAccessor httpcontext) : base(context)
        {
            _context = context;
            _httpContext = httpcontext;
        }

        public Cat_SubSection Details(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_SubSection
                .Include(c => c.Dept)
                .Include(c => c.Sect)
                .Where(x => x.ComId == comid)
                .FirstOrDefault(m => m.SubSectId == id);
        }

        public List<Cat_SubSection> GetSubSectionAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_SubSection
                .Include(s => s.Sect)
                .Include(s => s.Dept)
                .Where(x => x.ComId == comid && !x.IsDelete)
                .ToList();
        }

        public IEnumerable<SelectListItem> GetSubSectionList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Text = x.SubSectName,
                Value = x.SubSectId.ToString()
            });
        }
    }
}
