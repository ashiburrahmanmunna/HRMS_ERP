using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IUnitMastersRepository : ISelfRepository<UnitMaster>
    {
        IEnumerable<SelectListItem> UnitGroupId();
        IEnumerable<SelectListItem> UnitGroupId1(UnitMaster unitMaster);
    }
}
