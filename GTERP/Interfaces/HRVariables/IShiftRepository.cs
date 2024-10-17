using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IShiftRepository : IBaseRepository<Cat_Shift>
    {
        IEnumerable<SelectListItem> GetAttShiftNameList();
        IEnumerable<SelectListItem> GetShiftList();
        void ShiftDefalutDate(Cat_Shift cat_Shift);
        List<Cat_Shift> GetShiftByCompany(string comid);
    }
}
