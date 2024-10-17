using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class GovtScheduleJapanLoanRepository : BaseRepository<Acc_GovtSchedule_JapanLoan>, IGovtScheduleJapanLoanRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public GovtScheduleJapanLoanRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Acc_GovtSchedule_JapanLoan> GetAccGovtScheduleJapanLoanList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_GovtSchedule_JapanLoans
                .Include(x => x.vAcc_ChartOfAccount).Where(x => x.ComId == comid).ToList();
        }
    }
}
