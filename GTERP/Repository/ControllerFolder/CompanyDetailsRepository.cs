using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class CompanyDetailsRepository : BaseRepository<CompanyDetails>, ICompanyDetailsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<CompanyDetailsRepository> _logger;


        public CompanyDetailsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<CompanyDetailsRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> CompanyCode()
        {
            return new SelectList(_context.Companys, "ComId", "CompanyName");
        }

        public IEnumerable<SelectListItem> CompanyCode1()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Companys.Where(x => x.CompanySecretCode == comid), "ComId", "CompanyName");
        }


    }
}
