using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Accounts
{
    public class ShareHoldingRepository:BaseRepository<ShareHolding>, IShareHoldingRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<ShareHoldingRepository> _logger;

        public ShareHoldingRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<ShareHoldingRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> FiscalyearId()
        {
            return new SelectList(_context.Acc_FiscalYears
                .OrderByDescending(c => c.FYId)
                .Where(c => c.FYId > 0 && c.ComId == (_httpcontext.HttpContext.Session.GetString("comid"))), "FYId", "FYName");
        }
    }
}
