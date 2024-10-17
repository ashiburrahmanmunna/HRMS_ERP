using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ISummerWinterAllowRepository : IBaseRepository<HR_SummerWinterAllowance>
    {
        IEnumerable<SelectListItem> GetSummerWinterAllowList();
        List<SummerWinterAllowViewModel> GetSummerWinterAllowAll(int? SWAId);
        IEnumerable<SelectListItem> SWAllowList();
        HR_SummerWinterAllowance AllowCalculation(HR_SummerWinterAllowance item, Cat_SummerWinterAllowanceSetting setting);
    }
}
