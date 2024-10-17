using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class CatVariableRepository : BaseRepository<Cat_Variable>, ICatVariableRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public CatVariableRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public IEnumerable<SelectListItem> GetCatVariableList()
        {
            var data = _context.Cat_Variable.GroupBy(p => p.VarType)
                           .Select(g => new
                           {
                               VarType = g.Key,
                               Count = g.Count()
                           })
                           .ToList();
            return new SelectList(data, "VarType", "VarType");
        }

        public List<Cat_Variable> GetVariableList()
        {
            var data = _context.Cat_Variable.ToList();
            return data;
        }
    }
}
