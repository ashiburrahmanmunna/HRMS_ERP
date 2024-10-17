using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Accounts
{
    public class PrdUnitRepository:BaseRepository<PrdUnit>,IPrdUnitRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public PrdUnitRepository(GTRDBContext context, IHttpContextAccessor httpcontext) : base(context)
        {
            _context = context;
            _httpContext = httpcontext;
        }

        public List<PrdUnit> GetPrdUnit()
        {
            return GetAll().Where(c => c.PrdUnitId > 0).ToList();
        }
    }
}
