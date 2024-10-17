using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class IncenBandRepository : BaseRepository<Cat_IncenBand>, IIncenBandRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public IncenBandRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetIncenBandList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.InBId.ToString(),
                Text = x.IncenBandName
            });
        }
    }
}
