using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class ExchageRateRepository : BaseRepository<Cat_ExchangeRate>, IExchangeRateRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public ExchageRateRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetExchangeRateList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.ExChangeId.ToString(),
                Text = x.ExchangeRate.ToString()
            });
        }
    }
}
