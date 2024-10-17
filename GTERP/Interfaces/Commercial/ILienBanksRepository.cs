using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces
{
    public interface ILienBanksRepository : IBaseRepository<LienBank>
    {
        public IEnumerable<SelectListItem> CountryId();
        public IEnumerable<SelectListItem> CountryId1(LienBank LienBank);
    }
}
