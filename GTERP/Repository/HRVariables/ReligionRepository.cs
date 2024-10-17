using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class ReligionRepository : BaseRepository<Cat_Religion>, IReligionRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public ReligionRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<Cat_Religion> GetReligionList()
        {
            return _context.Cat_Religion.Where(x => !x.IsDelete);
        }

        public IEnumerable<SelectListItem> ReligionSelectList()
        {
            return _context.Cat_Religion.Select(x => new SelectListItem
            {
                Value = x.RelgionId.ToString(),
                Text = x.ReligionName
            });
        }
    }
}
