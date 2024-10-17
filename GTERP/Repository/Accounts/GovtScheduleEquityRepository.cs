using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class GovtScheduleEquityRepository : BaseRepository<Acc_GovtSchedule_Equity>, IGovtScheduleEquityRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public GovtScheduleEquityRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IEnumerable<SelectListItem> AccId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-3-13")); //&& c.ComId == (transactioncomid)
            return new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public static List<SelectListItem> Criteria = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Add", Value="Add"},
            new SelectListItem() { Text="Less", Value="Less"},
        };

        public List<Acc_GovtSchedule_Equity> GetAccGovtScheduleList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.Acc_GovtSchedule_Equity.Include(x => x.vAcc_ChartOfAccount).Where(x => x.ComId == comid).ToList();
            return data;
        }

        public IEnumerable<SelectListItem> CriteriaList()
        {
            return new SelectList(Criteria, "Value", "Text");
        }
    }
}
