using GTERP.Interfaces.Base;
using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IGatePassMsRepository : IBaseRepository<GatePassMs>
    {
        IEnumerable<SelectListItem> GetGatePassMsList();
        IEnumerable<GatePassMs> GetGatePassMsAll();

        //IEnumerable<SelectListItem> GetGatePassMsList();
        IEnumerable<SelectListItem> CatVariableList();

        void DeleteGatePassMs(int id);


    }
}
