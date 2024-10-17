using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class Cat_StampRep : BaseRepository<Cat_Stamp>, ICat_Stamp
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public Cat_StampRep(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
        
        public IEnumerable<Cat_Stamp> GetStampList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Stamp
                .Include(x => x.Cat_PayMode)
                .Where(x => !x.IsDelete && x.ComId==comid);
        }

    }
}
