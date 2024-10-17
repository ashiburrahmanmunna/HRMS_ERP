using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class SignatoryRepository : BaseRepository<Cat_ReportSignatory>, ISignatoryRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public SignatoryRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> ModuleName()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> ReportNames()
        {
            return _context.HR_ReportType.Select(x => new SelectListItem
            {
                Value = x.ReportType.ToString(),
                Text = x.ReportType.ToString()
            }).Distinct();
        }
    }
}
