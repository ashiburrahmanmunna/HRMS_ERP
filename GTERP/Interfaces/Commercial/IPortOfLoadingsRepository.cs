using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Commercial
{
    public interface IPortOfLoadingsRepository : IBaseRepository<PortOfLoading>
    {
        public IEnumerable<SelectListItem> CountryId();
        public IEnumerable<SelectListItem> CountryId1(PortOfLoading PortOfLoading);
    }
}
