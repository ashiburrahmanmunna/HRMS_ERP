
using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace GTERP.Interfaces.HRVariables
{
    public interface ICat_Stamp: IBaseRepository<Cat_Stamp>
    {
        public IEnumerable<Cat_Stamp> GetStampList();
    }
}
