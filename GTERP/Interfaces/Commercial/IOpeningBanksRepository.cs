using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Commercial
{
    public interface IOpeningBanksRepository : IBaseRepository<OpeningBank>
    {
        public IEnumerable<SelectListItem> CountryId();
        public IEnumerable<SelectListItem> CountryId1(OpeningBank openingBank);
    }
}
