using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class GasChargeRepository : BaseRepository<Cat_GasChargeSetting>, IGasChargeSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public GasChargeRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetGasChargeSetting()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.GasCharge.ToString()
            });
        }
    }
}
