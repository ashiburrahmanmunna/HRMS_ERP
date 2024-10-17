using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IBankStatementBLRepository : IBaseRepository<Acc_BankStatementBalance>
    {
        IEnumerable<SelectListItem> AccId();
    }
}
