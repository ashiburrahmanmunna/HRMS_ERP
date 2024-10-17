using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface ICompanyDetailsRepository : IBaseRepository<CompanyDetails>
    {

        IEnumerable<SelectListItem> CompanyCode();
        IEnumerable<SelectListItem> CompanyCode1();
    }
}
