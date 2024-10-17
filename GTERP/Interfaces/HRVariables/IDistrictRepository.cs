using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HRVariables
{
    public interface IDistrictRepository : IBaseRepository<Cat_District>
    {
        IEnumerable<SelectListItem> GetDistrictList();
        public IQueryable<Cat_District> GetAllDistrict();
    }
}
