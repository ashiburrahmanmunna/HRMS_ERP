using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HRVariables
{
    public interface IDesignationRepository : IBaseRepository<Cat_Designation>
    {
        IQueryable<Cat_Designation> GetAllDesignations();
        IEnumerable<SelectListItem> GetDesignationList();

        //IEnumerable<SelectListItem> GetDesignationListByName();

        List<Cat_Designation> GetDesignationsByCompany(string comid);
        void UpdateDesignation(Cat_Designation cat_Designation);
    }
}
