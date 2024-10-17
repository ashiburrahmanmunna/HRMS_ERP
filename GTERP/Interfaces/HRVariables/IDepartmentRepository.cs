using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Cat_Department>
    {

        IEnumerable<SelectListItem> GetDepartmentList();

        List<Cat_Department> GetDepartmentByCompany(string comid);
    }
}
