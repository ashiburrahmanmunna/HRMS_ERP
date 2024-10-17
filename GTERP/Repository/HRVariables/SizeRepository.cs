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
    public class SizeRepository:BaseRepository<Cat_Size>,ISizeRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public SizeRepository(GTRDBContext context,IHttpContextAccessor httpcontext):base(context)
        {
            _context = context;
            _httpContext = httpcontext;
        }
        public IEnumerable<SelectListItem> GetSize()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.SizeId.ToString(),
                Text = x.SizeName
            });
        }
    }
}
