using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HR
{
    public interface IEmpShiftInputRepository : IBaseRepository<HR_Emp_ShiftInput>
    {
        IEnumerable<SelectListItem> GetEmpShiftInputList();
        List<EmpForShit> GetEmpShiftAll();
        List<Cat_Shift> GetShiftAll();
        Task<int> EmpShiftPost(List<HR_Emp_ShiftInput> empShifts);

    }
}
