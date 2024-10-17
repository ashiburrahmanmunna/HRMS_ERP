using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class GradeRepository : BaseRepository<Cat_Grade>, IGradeRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public GradeRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public List<Cat_Grade> GradeAll()
        {
            return _context.Cat_Grade.Where(x => !x.IsDelete).ToList();
        }

        public IEnumerable<SelectListItem> GradeSelectList()
        {
            return _context.Cat_Grade.Where(x => !x.IsDelete).Select(x => new SelectListItem
            {
                Text = x.GradeName,
                Value = x.GradeId.ToString()
            });
        }
    }
}
