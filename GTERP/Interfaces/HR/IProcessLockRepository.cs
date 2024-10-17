using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IProcessLockRepository : IBaseRepository<HR_ProcessLock>
    {
        IEnumerable<SelectListItem> LockTypeList();
        Acc_FiscalYear FiscalYear();
        Acc_FiscalMonth FiscalMonth();
        IEnumerable<SelectListItem> FiscalYearID();
        IEnumerable<SelectListItem> FiscalMonthID();
        IEnumerable<SelectListItem> FiscalMonthID2();
        IEnumerable<Acc_FiscalMonth> GetFiscalMonth(int FiscalYearId);
        List<HR_ProcessLock> GetProcessLockData();

    }
}
