using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Commercial
{
    public class BankAccountNoRepository : BaseRepository<BankAccountNo>, IBankAccountNoRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<BankAccountNoRepository> _logger;
        public BankAccountNoRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<BankAccountNoRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> SisterConcernCompanyId()
        {
            return new SelectList(_context.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");
        }

        public IEnumerable<SelectListItem> OpeningBankId()
        {
            return new SelectList(_context.OpeningBanks, "OpeningBankId", "OpeningBankName");
        }

        public List<BankAccountNo> GetBanAccNo()
        {
            return GetAll()
                 .Include(x => x.CommercialCompanys)
                 .Include(x => x.OpeningBanks)
                 .Where(x => !x.IsDelete)
                 .ToList();
        }

        public BankAccountNo FindByIdBankAccNo(int? id)
        {
            var data = GetAll()
                 .Include(x => x.CommercialCompanys)
                 .Include(x => x.OpeningBanks)
                 .Where(x => x.BankAccountId == id)
                 .FirstOrDefault();
            return data;
        }
    }
}
