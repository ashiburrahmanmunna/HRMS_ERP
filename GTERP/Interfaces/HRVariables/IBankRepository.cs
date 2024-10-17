using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IBankRepository : IBaseRepository<Cat_Bank>
    {
        IEnumerable<SelectListItem> GetBankList();
        List<Cat_Bank> GetBankInfo();
    }
}
