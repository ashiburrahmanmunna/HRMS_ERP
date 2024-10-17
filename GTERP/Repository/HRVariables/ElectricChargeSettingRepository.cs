using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class ElectricChargeSettingRepository : BaseRepository<Cat_ElectricChargeSetting>, IElectricChargeSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ElectricChargeSettingRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetElectricChargeSetting()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ElectricCharge.ToString()
            });
        }
    }
}
