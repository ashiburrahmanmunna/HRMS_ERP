using GTERP.Interfaces.Base;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface ICompanyPermissionsRepository : IBaseRepository<CompanyPermission>
    {
        List<CompanyUser> CompanyUserList(string UserId);
        void CompanyPermissionDelete(int id);
    }
}
