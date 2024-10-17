using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HRVariables
{
    public interface ISectionRepository : IBaseRepository<Cat_Section>
    {
        IQueryable<Cat_Section> GetAllSection();
        IEnumerable<SelectListItem> GetSectionList();
        List<Cat_Section> GetSectionByCompany(string comid);
    }
}
