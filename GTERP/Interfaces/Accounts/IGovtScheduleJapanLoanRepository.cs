using GTERP.Interfaces.Base;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IGovtScheduleJapanLoanRepository : IBaseRepository<Acc_GovtSchedule_JapanLoan>
    {
        List<Acc_GovtSchedule_JapanLoan> GetAccGovtScheduleJapanLoanList();
    }
}
