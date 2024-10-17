using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IUserPermissionRepository : IBaseRepository<UserPermission>
    {
        IEnumerable<SelectListItem> Userlist();
    }
}
