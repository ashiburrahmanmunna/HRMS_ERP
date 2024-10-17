using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface ICompaniesRepository : ISelfRepository<Company>
    {
        List<Company> GetCompanyList();
        IEnumerable<SelectListItem> BusinessTypeId();
        IEnumerable<SelectListItem> BaseComId();
        IEnumerable<SelectListItem> CountryId();
        IEnumerable<SelectListItem> VoucherNoCreatedTypeId();
    }
}
