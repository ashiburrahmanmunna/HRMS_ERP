using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HRVariables
{
    public interface ISizeRepository: IBaseRepository<Cat_Size>
    {
        IEnumerable<SelectListItem> GetSize();
    }
}
