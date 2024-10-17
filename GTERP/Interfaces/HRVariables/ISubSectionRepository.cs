using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ISubSectionRepository : IBaseRepository<Cat_SubSection>
    {
        IEnumerable<SelectListItem> GetSubSectionList();
        List<Cat_SubSection> GetSubSectionAll();
        Cat_SubSection Details(int? id);
    }
}
