using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HRVariables
{
    public interface IIncomeTaxRepository:IBaseRepository<Cat_IncomeTaxChk>
    {
        List<Pross> GetProssType();
    }
}
