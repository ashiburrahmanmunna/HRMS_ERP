using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{

    public class HRCustomReportRepository : SelfRepository<HR_CustomReport>, IHRCustomReportRepository
    {
        private readonly GTRDBContext _context;


        private readonly HttpContextAccessor _httpContext;

        public HRCustomReportRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public List<HR_CustomReport> GetAllReport()
        {
            return _context.HR_CustomReport.Include(x => x.HR_ReportType).Include(x=>x.Company).ToList();
        }

        public IEnumerable<SelectListItem> GetComId()
        {
            return new SelectList(_context.Companys.ToList(), "CompanyCode", "CompanyName");
        }

        public IEnumerable<SelectListItem> GetReportType()
        {
            //var data = _context.HR_ReportType.GroupBy(p => p.ReportType)
            //             .Select(g => new
            //             {
            //                 ReportType = g.Key,
            //                 Count = g.Count()
            //             })
            //             .ToList();
            return new SelectList(_context.HR_ReportType.ToList(), "ReportId", "ReportName");
        }
        public IEnumerable<SelectListItem> GetEmpType()
        {            
            return new SelectList(_context.Cat_Emp_Type.ToList(), "EmpTypeId", "EmpTypeName");
        }
        public IEnumerable<SelectListItem> GetReportTypeCustom()
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
