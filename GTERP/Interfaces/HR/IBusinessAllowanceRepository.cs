using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IBusinessAllowanceRepository : IBaseRepository<HR_Emp_BusinessAllow>
    {
        IEnumerable<SelectListItem> GetBusinessAllowanceList();
        List<BusinessAllowViewModel> BusinessAllowanceList(DateTime? todate);
    }
}
