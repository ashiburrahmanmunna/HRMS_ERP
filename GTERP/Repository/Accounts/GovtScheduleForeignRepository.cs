using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class GovtScheduleForeignRepository : BaseRepository<Acc_GovtSchedule_ForeignLoan>, IGovtScheduleForeignRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public GovtScheduleForeignRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Acc_GovtSchedule_ForeignLoan> GetAccGovtScheduleForeignList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_GovtSchedule_ForeignLoans
                 .Include(x => x.vAcc_ChartOfAccount).Where(x => x.ComId == comid).ToList();
        }
    }
}
