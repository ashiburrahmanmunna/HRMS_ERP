using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HRVariables
{
    public class ColorRepository:BaseRepository<Cat_Color>,IColorRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ColorRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IEnumerable<SelectListItem> GetColor()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.ColorId.ToString(),
                Text = x.ColorName
            });
        }
    }
}
