using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class WareHouseRepository : BaseRepository<Warehouse>, IWareHouseRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public WareHouseRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetWarehouseList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.WarehouseId.ToString(),
                Text = x.WhName
            });
        }
    }
}
