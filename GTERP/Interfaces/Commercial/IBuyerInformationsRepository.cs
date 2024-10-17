using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Commercial
{
    public interface IBuyerInformationsRepository : IBaseRepository<BuyerInformation>
    {
        IEnumerable<SelectListItem> BuyerGroupId();
        IEnumerable<SelectListItem> CountryId();
        IEnumerable<SelectListItem> EmployeeIdImport();
        IEnumerable<SelectListItem> EmployeeIdExport();
    }
}
