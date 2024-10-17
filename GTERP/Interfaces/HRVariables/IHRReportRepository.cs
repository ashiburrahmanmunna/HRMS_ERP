using GTERP.Interfaces.Self;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IHRReportRepository : ISelfRepository<HR_ReportType>
    {
        IEnumerable<SelectListItem> GetReportType();
        IEnumerable<SelectListItem> GetComId();
        List<CustomReportVM> GetCustomReport();
    }
}
