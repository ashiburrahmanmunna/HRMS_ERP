using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Commercial
{
    public interface IDestinationsRepository : IBaseRepository<Destination>
    {
        public IEnumerable<SelectListItem> CountryId();
    }
}
