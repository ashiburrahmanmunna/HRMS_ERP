using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class BankStatementBLRepository : BaseRepository<Acc_BankStatementBalance>, IBankStatementBLRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public BankStatementBLRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IEnumerable<SelectListItem> AccId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts
                .Where(x => x.AccCode.Contains("1-1-11") && x.ComId == comid && x.AccType == "L"), "AccId", "AccName");
        }
    }
}
