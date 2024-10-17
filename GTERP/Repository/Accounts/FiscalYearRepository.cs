using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class FiscalYearRepository : BaseRepository<Acc_FiscalYear>, IFiscalYearRepository
    {

        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public FiscalYearRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Acc_FiscalYear> GetFiscalYear()
        {

            var data = GetAll()
                .OrderByDescending(x => x.FiscalYearId).ToList();
            return data;
        }


    }
}
