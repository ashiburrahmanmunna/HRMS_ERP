using GTERP.Interfaces.Base;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IFiscalYearRepository : IBaseRepository<Acc_FiscalYear>
    {
        List<Acc_FiscalYear> GetFiscalYear();
    }
}
