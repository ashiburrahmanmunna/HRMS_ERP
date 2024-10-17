using GTERP.Interfaces.Base;
using GTERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Accounts
{
    public interface IPrdUnitRepository : IBaseRepository<PrdUnit>
    {
        List<PrdUnit> GetPrdUnit();
    }
}
