using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Tax
{
    public interface ITaxRepository : IBaseRepository<Tax_ClientInfo>
    {
        public IEnumerable<SelectListItem> ComList();
        public IEnumerable<SelectListItem> ClientList();

        

    }
}
