using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IBankBranchRepository : IBaseRepository<Cat_BankBranch>
    {
        IEnumerable<SelectListItem> GetBankBranchList();
        List<Cat_BankBranch> GetBankBranchInfo();
    }
}
