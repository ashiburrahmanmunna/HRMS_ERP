using GTERP.Interfaces.Budget;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Budget
{
    public class BudgetProductionTargetRepository:BaseRepository<Budget_ProductionTarget>, IBudgetProductionTargetRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<BudgetProductionTargetRepository> _logger;


        public BudgetProductionTargetRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<BudgetProductionTargetRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> AccId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-3-13"));
            return new SelectList(Acc_ChartOfAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public static List<SelectListItem> CriteriaList = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Add", Value="Add"},
            new SelectListItem() { Text="Less", Value="Less"},
        };

        public IEnumerable<SelectListItem> Criteria()
        {
            return new SelectList(CriteriaList, "Value", "Text"); 
        }

        public List<Budget_ProductionTarget> GetBudget_ProductionTargetList(string FromDate, string ToDate, string criteria)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccId > 0 && c.AccType == "L" && c.AccCode.Contains("2-3-13"));

            List<Budget_ProductionTarget> abcd = new List<Budget_ProductionTarget>();

            if (FromDate == null || FromDate == "")
            {
                abcd = _context.Budget_ProductionTargets.Include(x => x.PrdUnit).Where(x => x.ComId == comid).ToList();
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);
                abcd = _context.Budget_ProductionTargets.Include(x => x.PrdUnit).Where(x => x.ComId == comid).ToList();
            }
            return abcd;
        }

    }
}
