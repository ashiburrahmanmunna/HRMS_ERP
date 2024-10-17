using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Self;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{

    public class HRReportRepository : SelfRepository<HR_ReportType>, IHRReportRepository
    {
        private readonly GTRDBContext _context;


        private readonly HttpContextAccessor _httpContext;

        public HRReportRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetComId()
        {
            return new SelectList(_context.Companys.ToList(), "CompanyCode", "CompanyName");
        }

        public List<CustomReportVM> GetCustomReport()
        {
            string query = $"Exec HR_PrcCustomerReport";

            var data = Helper.ExecProcMapTList<CustomReportVM>("HR_PrcCustomerReport");
            return data;
        }

        public IEnumerable<SelectListItem> GetReportType()
        {
            var data = _context.HR_ReportType.GroupBy(p => p.ReportType)
                         .Select(g => new
                         {
                             ReportType = g.Key,
                             Count = g.Count()
                         })
                         .ToList();
            return new SelectList(data, "ReportType", "ReportType");
        }
    }
}
