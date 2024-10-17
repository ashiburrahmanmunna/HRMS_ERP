using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;


namespace GTERP.Interfaces.HR
{
    public interface IEmpReleaseRepository : IBaseRepository<HR_Emp_Released>
    {
        IEnumerable<SelectListItem> HR_Emp_Info();
        IEnumerable<SelectListItem> EmpList();
        IEnumerable<SelectListItem> EmpListEdit(int id);
        IEnumerable<SelectListItem> EmpListWithLessInfo();
        IEnumerable<SelectListItem> CatVariableList();
      
        IQueryable<HR_Emp_Released> GetReleasedAll();
        void ApproveSet(HR_Emp_Released released);
    }
}
