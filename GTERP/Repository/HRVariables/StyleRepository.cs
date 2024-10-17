using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{

    public class StyleRepository : BaseRepository<Cat_Style>, IStyleRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public StyleRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetColor()
        {
            var data = _context.Cat_Variable.Where(x => x.VarType == "Color").ToList();
            return data.Select(x => new SelectListItem
            {
                Value=x.VarName,
                Text=x.VarName
            });
        }

        public IEnumerable<SelectListItem> GetSize()
        {
            var data = _context.Cat_Variable.Where(x => x.VarType == "Size").ToList();
            return data.Select(x => new SelectListItem
            {
                Value = x.VarName,
                Text = x.VarName
            });
        }

        public IEnumerable<SelectListItem> GetStyleList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.StyleId.ToString(),
                Text = x.StyleName + " (" + x.Rate + ")" 
            });
        }
    }
}
