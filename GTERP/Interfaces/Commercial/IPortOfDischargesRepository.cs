using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Commercial
{
    public interface IPortOfDischargesRepository : IBaseRepository<PortOfDischarge>
    {
        public IEnumerable<SelectListItem> CountryId();
        public IEnumerable<SelectListItem> CountryId1(PortOfDischarge PortOfDischarge);
    }
}
