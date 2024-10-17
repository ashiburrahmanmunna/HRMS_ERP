using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class CompaniesRepository : SelfRepository<Company>, ICompaniesRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<CompaniesRepository> _logger;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public CompaniesRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<CompaniesRepository> logger,
            IHostingEnvironment hostingEnvironment
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<SelectListItem> BaseComId()
        {
            return new SelectList(_context.Companys.Where(p => p.IsGroup == true), "ComId", "CompanyName");
        }

        public IEnumerable<SelectListItem> BusinessTypeId()
        {
            return new SelectList(_context.BusinessType, "BusinessTypeId", "Name");
        }

        public IEnumerable<SelectListItem> CountryId()
        {
            return new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CountryName");
        }

        public List<Company> GetCompanyList()
        {
            var comId = _httpcontext.HttpContext.Session.GetString("comid");
            return _context.Companys.Include(x => x.vCountryCompany)
                .Include(x => x.vBusinessTypeCompany)
                .Where(x => x.CompanyCode == comId && !x.IsDelete).ToList();
        }

        public IEnumerable<SelectListItem> VoucherNoCreatedTypeId()
        {
            return new SelectList(_context.Acc_VoucherNoCreatedTypes, "VoucherNoCreatedTypeId", "VoucherNoCreatedTypeName");
        }
    }
}
