using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface ISalaryCheckRepository
    {
        IEnumerable<SelectListItem> GetSalaryCheckList();
        List<SalaryCheck> SalaryCheckList(string prossType,int fun2, int pageIndex, int pageSize, string searchColumns, string searchKeywords);

        List<SalaryCheck> GetAllSalaryCheck(string prossType, int fun2);
        List<TotalCount> SalaryCheckCount(string prossType, int fun2, int pageIndex, int pageSize, string searchColumns, string searchKeywords);
        List<Pross> GetProssType();
    }
}
