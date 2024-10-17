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
    public class SectionRepository : BaseRepository<Cat_Section>, ISectionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public SectionRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IQueryable<Cat_Section> GetAllSection()
        {
            var ComId = _httpcontext.HttpContext.Session.GetString("comid");
            return _context.Cat_Section.Include(s => s.Dept).Where(x => x.ComId == ComId && !x.IsDelete);
        }

        public List<Cat_Section> GetSectionByCompany(string comid)
        {
            var sectionList = _context.Cat_Section.Where(a => a.ComId == comid).ToList();
            return sectionList;
        }
        public IEnumerable<SelectListItem> GetSectionList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.SectId.ToString(),
                Text = x.SectName
            });
        }
    }
}
