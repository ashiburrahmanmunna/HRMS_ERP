using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IEmployeeBehaviourRepository : IBaseRepository<HR_Emp_Behave>
    {
        IEnumerable<SelectListItem> GetEmployeeBehaviourList();
        IEnumerable<HR_Emp_Behave> GetBehaveAll();

        IEnumerable<SelectListItem> GetEmpList();
        IEnumerable<SelectListItem> CatVariableList();

        void DeleteBehave(int id);

    }
}
