using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILeaveEncashmentRepository : IBaseRepository<HR_LvEncashment>
    {
        List<HR_LvEncashment> GetLvEncashments();
        IEnumerable<SelectListItem> GetEmpList();
        HR_Leave_Balance CreateLvEncashment(HR_LvEncashment lvEncashment);
        List<Basic> prcBasic(HR_LvEncashment lvEncashment);
        HR_Leave_Balance GetLeaveBalance(int empId, int year);
    }
}
