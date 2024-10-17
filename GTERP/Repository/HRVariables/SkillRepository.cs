using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class SkillRepository : BaseRepository<Cat_Skill>, ISkillRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public SkillRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetSkillList()
        {
            return _context.Cat_Skill.Where(x => !x.IsDelete).Select(x => new SelectListItem
            {
                Value = x.SkillId.ToString(),
                Text = x.SkillName
            });
        }

        public List<Cat_Skill> SkillAll()
        {
            return _context.Cat_Skill.Where(x => !x.IsDelete).ToList();
        }
    }
}
