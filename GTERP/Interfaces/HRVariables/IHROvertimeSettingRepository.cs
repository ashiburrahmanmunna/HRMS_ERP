using GTERP.Interfaces.Base;
using GTERP.Interfaces.Self;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HRVariables
{
    public interface IHROvertimeSettingRepository : ISelfRepository<HR_OverTimeSetting>
    {
        IEnumerable<SelectListItem> GetCompany();
        List<HR_OverTimeSettingVM> GetOvertimeList();
    }
}
