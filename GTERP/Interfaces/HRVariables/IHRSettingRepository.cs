using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IHRSettingRepository : IBaseRepository<Cat_HRSetting>
    {
        IEnumerable<SelectListItem> GetHRSettingList();
        IEnumerable<SelectListItem> GetCompanyName();
        List<CatHRSettingVM> GetAllData();
    }
}
