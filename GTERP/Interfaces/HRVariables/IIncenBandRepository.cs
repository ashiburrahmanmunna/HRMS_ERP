using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IIncenBandRepository : IBaseRepository<Cat_IncenBand>
    {
        IEnumerable<SelectListItem> GetIncenBandList();
    }
}
