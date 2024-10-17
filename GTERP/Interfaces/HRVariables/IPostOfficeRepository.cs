using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IPostOfficeRepository : IBaseRepository<Cat_PostOffice>
    {
        IEnumerable<SelectListItem> GetPostOfficeList();
        public IEnumerable<Cat_PostOffice> GetPOList();
    }
}
