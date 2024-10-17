using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Accounts
{
    public interface IShareHoldingRepository:IBaseRepository<ShareHolding>
    {
        IEnumerable<SelectListItem> FiscalyearId();
    }
}
