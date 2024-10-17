using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class BankRepository : BaseRepository<Cat_Bank>, IBankRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public BankRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public List<Cat_Bank> GetBankInfo()
        {
            return _context.Cat_Bank.Where(x => x.IsDelete == false).ToList();
        }

        public IEnumerable<SelectListItem> GetBankList()
        {
            return GetBankInfo().Select(x => new SelectListItem
            {
                Value = x.BankId.ToString(),
                Text = x.BankName
            });
        }
    }
}
