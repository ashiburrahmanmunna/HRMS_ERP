using GTERP.Interfaces.Base;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IReportPermissionsRepository : IBaseRepository<ReportPermissions>
    {
        List<ReportPermissionsVM> GetReportPermissionsList(string userid);
        void UpdateCountry(Country country);
        void DeleteCountry(int id);
        //void RemoveRangeForReport();
        //void AddRangeForReport(List<ReportPermissions> ReportPermissions);

    }
}
