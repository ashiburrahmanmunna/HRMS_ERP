using GTERP.Interfaces.Base;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IGovtScheduleForeignRepository : IBaseRepository<Acc_GovtSchedule_ForeignLoan>
    {
        List<Acc_GovtSchedule_ForeignLoan> GetAccGovtScheduleForeignList();
    }
}
